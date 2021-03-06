﻿using System;
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
        /// <summary>最後の発言のStatusID</summary>
        private long _last_status_id = -1;

        private const int GET_NUM = 50;
        //-------------------------------------------------------------------------------
        #endregion (メンバー)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispStatuses(FrmMain parent, ImageListWrapper imageListWrapper, EFormType formtype)
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
        #region #[override]OnLoad フォームロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Utilization.SetModelessDialogCenter(this);

            switch (FormType) {
                case EFormType.UserStatus:
                    Debug.Assert(!string.IsNullOrEmpty(UserScreenName), "ReplyStartTwitdataが設定されていません。");
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = string.Format("{0}の発言", UserScreenName);
                    break;
                case EFormType.Conversation:
                    Debug.Assert(ReplyStartTwitdata != null, "ReplyStartTwitdataが設定されていません。");
                    this.Text = "会話";
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.Conversation;
                    TwitData[] data = ReplyStartTwitdata.ToArray();
                    uctlDispTwit.AddData(data, true);
                    _last_status_id = data[data.Length - 1].Mention_StatusID;
                    break;
                case EFormType.MyRetweet:
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = "自分のリツイート";
                    break;
                case EFormType.FollowersRetweet:
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = "フォロワーのリツイート";
                    break;
                case EFormType.FollowersRetweetToMe:
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = "自分がされたリツイート";
                    break;
                case EFormType.MyFavorite:
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = "お気に入り";
                    break;
                case EFormType.UserFavorite:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていません");
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = string.Format("{0}のお気に入り", UserScreenName);
                    break;
                case EFormType.ListStatuses:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていません。");
                    Debug.Assert(ListID != null, "ListIDが設定されていません。");
                    uctlDispTwit.ContextMenuType = UctlDispTwit.MenuType.RestrictedUser;
                    this.Text = string.Format("{0}のリスト{1}の発言", UserScreenName, ListID);
                    break;
            }
        }
        #endregion (FrmDispTweet_Load)

        //-------------------------------------------------------------------------------
        #region #[override]OnShown フォーム表示時
        //-------------------------------------------------------------------------------
        //
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Utilization.InvokeTransaction(() => GetTweets());
        }
        //-------------------------------------------------------------------------------
        #endregion (FrmReply_Shown)

        //-------------------------------------------------------------------------------
        #region btnClose_Click 閉じるボタン
        //-------------------------------------------------------------------------------
        //
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnClose_Click)

        //-------------------------------------------------------------------------------
        #region btnAppend_Click 追加取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {
            Utilization.InvokeTransaction(() => GetTweets());
        }
        #endregion (btnAppend_Click)

        //-------------------------------------------------------------------------------
        #region #[override]OnFormClosed フォームクローズ時
        //-------------------------------------------------------------------------------
        //
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
        #endregion (FrmDispTweet_FormClosed)

        //-------------------------------------------------------------------------------
        #region #[override]ProcessCmdKey キー処理
        //-------------------------------------------------------------------------------
        //
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (!btnAppend.Focused && !btnClose.Focused) {
                KeyData key = Utilization.ConvertKeysToKeyData(keyData);
                uctlDispTwit.ProcessKey(key);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion (#[override]ProcessCmdKey)

        //-------------------------------------------------------------------------------
        #region -GetTweets 発言取得(別スレッド)
        //-------------------------------------------------------------------------------
        //
        private void GetTweets()
        {
            bool disableAppend = false;
            try {
                try {
                    this.Invoke((Action)(() =>
                    {
                        tsslabel.Text = "発言取得中...";
                        btnAppend.Enabled = false;
                    }));

                    IEnumerable<TwitData> d = null;
                    string changedStatusText = null;
                    switch (FormType) {
                        case EFormType.UserStatus:
                            d = FrmMain.Twitter.statuses_user_timeline(screen_name: UserScreenName, max_id: _last_status_id - 1, count: GET_NUM, include_rts: true);
                            if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            else { disableAppend = true; }
                            break;
                        case EFormType.Conversation:
                            disableAppend = true;
                            changedStatusText = "全ての会話を取得完了しました。";
                            while (_last_status_id >= 0) {
                                TwitData data;
                                if (!Utilization.GetTwitDataFromID(_last_status_id, out data)) {
                                    changedStatusText = "取得できなかった発言があります。";
                                    disableAppend = false;
                                    break;
                                }
                                this.Invoke((Action)(() => uctlDispTwit.AddData(data.AsEnumerable(), true)));
                                _last_status_id = data.Mention_StatusID;
                            }
                            d = new TwitData[0];
                            break;
                        case EFormType.MyRetweet:
                            // TODO 消去
                            //d = FrmMain.Twitter.statuses_retweeted_by_me(max_id: _last_status_id - 1, count: GET_NUM);
                            //if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            //else { disableAppend = true; }
                            break;
                        case EFormType.FollowersRetweet:
                            // TODO 消去
                            //d = FrmMain.Twitter.statuses_retweeted_to_me(max_id: _last_status_id - 1, count: GET_NUM);
                            //if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            //else { disableAppend = true; }
                            break;
                        case EFormType.FollowersRetweetToMe:
                            d = FrmMain.Twitter.statuses_retweets_of_me(max_id: _last_status_id - 1, count: GET_NUM);
                            if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            else { disableAppend = true; }
                            break;
                        case EFormType.ListStatuses:
                            d = FrmMain.Twitter.lists_statuses(slug: ListID, owner_screen_name: UserScreenName, max_id: _last_status_id - 1, count: GET_NUM, include_rts: true);
                            if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            else { disableAppend = true; }
                            break;
                        case EFormType.MyFavorite:
                            d = FrmMain.Twitter.favorites_list(max_id: _last_status_id);
                            if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            disableAppend = (d.Count() == 0);
                            break;
                        case EFormType.UserFavorite:
                            d = FrmMain.Twitter.favorites_list(max_id: _last_status_id);
                            if (d.Count() > 0) { _last_status_id = d.Min(td => td.StatusID); }
                            disableAppend = (d.Count() == 0);
                            break;
                        default:
                            Debug.Assert(false); // ここには来ない
                            return;
                    }

                    this.Invoke((Action)(() =>
                    {
                        uctlDispTwit.AddData(d);
                        tsslabel.Text = (changedStatusText == null) ? "取得完了しました。" : changedStatusText;
                        btnAppend.Enabled = !disableAppend;
                    }));
                }
                catch (TwitterAPIException) {
                    this.Invoke((Action)(() =>
                    {
                        tsslabel.Text = "取得に失敗しました。";
                        btnAppend.Enabled = !disableAppend;
                    }));
                }
            }
            catch (InvalidOperationException) { }
        }
        #endregion (GetTweets)
    }
}
