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
    public partial class FrmDispTweet : Form
    {
        //-------------------------------------------------------------------------------
        #region メンバー
        //-------------------------------------------------------------------------------
        /// <summary>フォームのタイプ</summary>
        public EFormType FormType { get; set; }
        /// <summary>FormType=Userの時必須，ユーザー名</summary>
        public string UserScreenName { get; set; }
        /// <summary>FormType=Conversationの時必須，最初の発言データ</summary>
        public TwitData ReplyStartTwitdata { get; set; }

        const int GET_NUM = 50;
        //-------------------------------------------------------------------------------
        #endregion (メンバー)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispTweet(FrmMain parent, ImageList imageList)
        {
            InitializeComponent();
            ReplyStartTwitdata = null;
            FormType = EFormType.User;
            uctlDispTwit.ImageList = imageList;
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
            User,
            /// <summary>会話表示</summary>
            Conversation
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
                case EFormType.User:
                    if (!string.IsNullOrEmpty(UserScreenName)) {
                        this.Text = string.Format("{0}の発言", UserScreenName);
                        uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                        tsslabel.Text = "発言取得中...";
                        (new Action<string>(GetUserTweets)).BeginInvoke(UserScreenName, Utilization.InvokeCallback, null);
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
                        uctlDispTwit.AddData(new TwitData[] { ReplyStartTwitdata }, true);
                        tsslabel.Text = "リプライ取得中...";
                        (new Action<long>(GetReplies)).BeginInvoke(ReplyStartTwitdata.Mention_StatusID, Utilization.InvokeCallback, null);
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
        #region -GetUserTweets （別スレッド：発言取得)
        //-------------------------------------------------------------------------------
        //
        private void GetUserTweets(string screen_name)
        {
            try {
                try {
                    TwitData[] d = FrmMain.Twitter.statuses_user_timeline(screen_name: screen_name, count: GET_NUM);
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
                    TwitData data = null;
                    try {
                        data = FrmMain.Twitter.statuses_show(status_id);
                    }
                    catch (TwitterAPIException) {
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
