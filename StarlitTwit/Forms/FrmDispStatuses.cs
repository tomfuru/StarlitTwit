using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace StarlitTwit
{
    public partial class FrmDispStatuses : Form
    {
        //-------------------------------------------------------------------------------
        #region メンバー
        //-------------------------------------------------------------------------------
        /// <summary>フォームのタイプ</summary>
        public EFormType FormType { get; private set; }
        /// <summary>FormType=User,UserFavorite,ListStatusesの時必須，ユーザー名</summary>
        public string UserScreenName { get; set; }
        /// <summary>FormType=Conversationの時必須，最初の発言データ</summary>
        public IEnumerable<TwitData> ReplyStartTwitdata { get; set; }
        /// <summary>FormType=ListStatusesの時必須，リストID</summary>
        public string ListID { get; set; }

        const int GET_NUM = 50;
        //-------------------------------------------------------------------------------
        #endregion (メンバー)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispStatuses(FrmMain parent, ImageListWrapper imageListWrapper,EFormType formtype)
        {
            InitializeComponent();
            ReplyStartTwitdata = null;
            FormType = formtype;
            uctlDispTwit.ImageListWrapper = imageListWrapper;
            parent.RegisterUctlDispTwitEvent(uctlDispTwit);
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region EFormType 列挙体：フォームの種類
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォームがどのような目的で使われるかを表します。
        /// </summary>
        public enum EFormType
        {
            /// <summary>ユーザー発言表示</summary>
            UserStatus,
            /// <summary>会話表示</summary>
            Conversation,
            //
            /// <summary>自分のRetweet</summary>
            MyRetweet,
            /// <summary>フォロワーのRetweet</summary>
            FollowersRetweet,
            /// <summary>フォロワーの自分のRetweet</summary>
            FollowersRetweetToMe,
            /// <summary>リストの発言</summary>
            ListStatuses,
            /// <summary>自分のお気に入り</summary>
            MyFavorite,
            /// <summary>指定ユーザーのお気に入り</summary>
            UserFavorite
        }
        //-------------------------------------------------------------------------------
        #endregion (EFormType)

        //-------------------------------------------------------------------------------
        #region FrmDispTweet_Load フォームロード時
        //-------------------------------------------------------------------------------
        //
        private void FrmDispTweet_Load(object sender, EventArgs e)
        {
            Utilization.SetModelessDialogCenter(this);
        }
        #endregion (FrmDispTweet_Load)

        //-------------------------------------------------------------------------------
        #region FrmReply_Shown フォーム表示時
        //-------------------------------------------------------------------------------
        //
        private void FrmReply_Shown(object sender, EventArgs e)
        {
            switch (FormType) {
                case EFormType.UserStatus:
                    if (!string.IsNullOrEmpty(UserScreenName)) {
                        this.Text = string.Format("{0}の発言", UserScreenName);
                        uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                        uctlDispTwit.RowContextMenu_Click += new EventHandler<TwitRowMenuEventArgs>(TwitMenu_OlderDataRequest_Click);
                        tsslabel.Text = "発言取得中...";
                        Utilization.InvokeTransaction(() => GetUserTweets(UserScreenName, -1));
                    }
                    else {
                        Debug.Assert(false, "ReplyStartTwitdataが設定されていません。");
                        this.Close();
                    }
                    break;
                case EFormType.Conversation:
                    if (ReplyStartTwitdata != null) {
                        this.Text = "会話";
                        uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.Conversation;
                        TwitData[] data = ReplyStartTwitdata.ToArray();
                        uctlDispTwit.AddData(data , true);
                        tsslabel.Text = "リプライ取得中...";
                        Utilization.InvokeTransaction(() => GetReplies(data[data.Length -1].Mention_StatusID));
                    }
                    else {
                        Debug.Assert(false, "ReplyStartTwitdataが設定されていません。");
                        this.Close();
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (FrmReply_Shown)

        //-------------------------------------------------------------------------------
        #region FrmDispTweet_FormClosed フォームクローズ時
        //-------------------------------------------------------------------------------
        //
        private void FrmDispTweet_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        #endregion (FrmDispTweet_FormClosed)

        //-------------------------------------------------------------------------------
        #region TwitMenu_OlderDataRequest_Click より古い発言要求
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_OlderDataRequest_Click(object sender, TwitRowMenuEventArgs e)
        {
            if (e.EventType == RowEventType.OlderTweetRequest) {
                tsslabel.Text = "発言取得中...";
                Utilization.InvokeTransaction(() => GetUserTweets(UserScreenName, e.TwitData.StatusID));
            }
        }
        #endregion (TwitMenu_OlderDataRequest_Click)

        //-------------------------------------------------------------------------------
        #region -GetUserTweets （別スレッド：発言取得)
        //-------------------------------------------------------------------------------
        //
        private void GetUserTweets(string screen_name, long max_id = -1)
        {
            try {
                try {
                    IEnumerable<TwitData> d = FrmMain.Twitter.statuses_user_timeline(screen_name: screen_name, max_id: max_id, count: GET_NUM);
                    this.Invoke(new Action(() => uctlDispTwit.AddData(d)));
                }
                catch (TwitterAPIException) {
                    this.Invoke(new Action(() => tsslabel.Text = "発言が取得できませんでした。"));
                    return;
                }
                this.Invoke(new Action(() => tsslabel.Text = "発言の取得が完了しました。"));
            }
            catch (InvalidOperationException) { }
        }
        #endregion (GetUserTweets)

        //-------------------------------------------------------------------------------
        #region -GetReplies (別スレッド：リプライ取得)
        //-------------------------------------------------------------------------------
        //
        private void GetReplies(long status_id)
        {
            try {
                while (status_id >= 0) {
                    TwitData data ;
                    if (!Utilization.GetTwitDataFromID(status_id, out data)) {
                        this.Invoke(new Action(() => tsslabel.Text = "取得できなかった発言があります。"));
                        return;
                    }
                    this.Invoke(new Action(() => uctlDispTwit.AddData(new TwitData[] { data }, true)));
                    status_id = data.Mention_StatusID;
                }
                this.Invoke(new Action(() => tsslabel.Text = "会話の取得が完了しました。"));
            }
            catch (InvalidOperationException) { }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetReplies)
    }
}
