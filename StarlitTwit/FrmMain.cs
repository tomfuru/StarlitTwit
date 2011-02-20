/*
 * メインフォーム
 * 
 * ●履歴
 * 2010/10/01 作成開始
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StarlitTwit
{
    public partial class FrmMain : Form
    {
        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            // 初期設定
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.WebRequest.DefaultWebProxy = null;

            DEFAULT_TABPAGES = new TabPage[] { tabpgHome, tabpgReply, tabpgHistory, tabpgDirect, /* tabpgPublic */ };

            imageListWrapper.ImageList.Images.Add(STR_IMGLIST_CROSS, StarlitTwit.Properties.Resources.cross);

            tssLabel.Text = "";
            tsslRestAPI.Text = "";
            lblTweetStatus.Text = "";

            //tabpgPublic.ToolTipText = "全体";

            Twitter = new Twitter();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //===============================================================================
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>TwitterAPI呼び出しクラス</summary>
        public static Twitter Twitter { get; private set; }
        /// <summary>設定データ</summary>
        public static SettingsData SettingsData { get; private set; }

        /// <summary>タブと表示コントロールの辞書</summary>
        private Dictionary<TabPage, UctlDispTwit> _dispTwitDic = new Dictionary<TabPage, UctlDispTwit>();

        /// <summary>自動取得用データ</summary>
        private Dictionary<UctlDispTwit, AutoRenewData> _autoRenewDic = new Dictionary<UctlDispTwit, AutoRenewData>();
        /// <summary>自動取得用スレッド</summary>
        private Thread _bgThread;
        /// <summary>認証済みかどうか</summary>
        private bool _isAuthenticated;
        /// <summary>スレッドストップ用シグナル(タブ追加/削除操作を行わない)</summary>
        private ManualResetEventSlim _mreThreadRun = new ManualResetEventSlim(true);
        /// <summary>スレッド確認用シグナル(タブ追加/削除操作を行わない)</summary>
        private ManualResetEventSlim _mreThreadConfirm = new ManualResetEventSlim(false);
        /// <summary>スレッドストップ用シグナル(タブ追加/削除操作を行う)</summary>
        private ManualResetEventSlim _mreThreadTabRun = new ManualResetEventSlim(true);
        /// <summary>スレッド確認用シグナル(タブ追加/削除操作を行う)</summary>
        private ManualResetEventSlim _mreThreadTabConfirm = new ManualResetEventSlim(false);
        /// <summary>排他的処理中にtrue</summary>
        private bool _bIsProcessing = false;
        /// <summary>排他処理同時開始抑制用</summary>
        private object _objKeyProcessStart = new object();

        /// <summary>URL短縮処理中かどうか</summary>
        private bool _bURLShortening = false;

        #region 発言状態関連
        //-------------------------------------------------------------------------------
        /// <summary>発言状態かどうか</summary>
        private StatusState _stateStatusState = StatusState.Normal;
        /// <summary>発言状態によって使用するID</summary>
        private long _statlID = -1;
        /// <summary>発言状態によって使用する名前</summary>
        private string _statsName = "";
        //-------------------------------------------------------------------------------
        #endregion (発言状態関連)
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region プロパティ
        //-------------------------------------------------------------------------------
        #region ImageListWrapper プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ImageListWrapperを取得します。
        /// </summary>
        public ImageListWrapper ImageListWrapper
        {
            get { return imageListWrapper; }
        }
        #endregion (ImageListWrapper)
        //-------------------------------------------------------------------------------
        #region ImageList プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ImageListを返します。
        /// </summary>
        public ImageList ImageList
        {
            get { return imageListWrapper.ImageList; }
        }
        #endregion (ImageList)
        //-------------------------------------------------------------------------------
        #endregion (プロパティ)

        //===============================================================================
        #region 定数
        //-------------------------------------------------------------------------------
        /// <summary>×マークの画像のKey</summary>
        public const string STR_IMGLIST_CROSS = "CROSS";
        /// <summary>発言可能な長さ</summary>
        private const int MAX_LENGTH = 140;
        /// <summary>デフォルトである(消せない)タブページ</summary>
        private readonly TabPage[] DEFAULT_TABPAGES;
        /// <summary>残りAPI表示フォーマット</summary>
        private const string REST_API_FORMAT = "API残: {0}/{1}";
        /// <summary>取得中表示フォーマット</summary>
        private const string GETTING_FORMAT = "タブ:{0} 取得中...";
        private const string STR_FIRST_GET_NUM = "初期取得件数:";
        private const string STR_RENEW_GET_NUM = "追加取得件数:";
        private const string STR_GET_INTERVAL = "取得間隔:";
        private const string STR_NOT_AUTOGET = "自動取得無し";
        private const string STR_SECOND = "秒";
        private const string STR_NUM = "件";
        private readonly SystemSound SYSTEMSOUND = SystemSounds.Question;
        const int BALOON_DURATION = 10000;
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region 型
        //-------------------------------------------------------------------------------
        #region -AutoRenewData クラス
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 自動取得用データ
        /// </summary>
        private class AutoRenewData
        {
            /// <summary>基準時間</summary>
            public DateTime Standard;
            /// <summary>更新間隔</summary>
            public TimeSpan Interval;
            /// <summary>強制更新</summary>
            public volatile bool IsForce;
            /// <summary>取得方法</summary>
            public GetTweetType GetTweetType;
        }
        //-------------------------------------------------------------------------------
        #endregion (AutoRenewData)

        //-------------------------------------------------------------------------------
        #region -GetTweetType 列挙体：つぶやきの取得方法
        //-------------------------------------------------------------------------------
        /// <summary>
        /// つぶやきの取得方法
        /// </summary>
        private enum GetTweetType
        {
            /// <summary>最も新しいもの</summary>
            MostRecent,
            /// <summary>より新しいもの</summary>
            MoreRecent,
            /// <summary>より古いもの</summary>
            Older
        }
        //-------------------------------------------------------------------------------
        #endregion (GetTweetType)

        //-------------------------------------------------------------------------------
        #region -StatusState 列挙体：発言状態
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言の状態を表します。
        /// </summary>
        private enum StatusState : byte
        {
            /// <summary>通常</summary>
            Normal,
            /// <summary>リプライ状態</summary>
            Reply,
            /// <summary>引用状態</summary>
            Quote,
            /// <summary>ダイレクトメッセージ送信状態</summary>
            DirectMessage
        }
        //-------------------------------------------------------------------------------
        #endregion (StatusState)
        //-------------------------------------------------------------------------------
        #endregion (型)

        //===============================================================================
        #region イベント
        //-------------------------------------------------------------------------------
        #region FrmMain_Load フォームロード時
        //-------------------------------------------------------------------------------
        //
        private void FrmMain_Load(object sender, EventArgs e)
        {
            SettingsData = SettingsData.Restore();

            // フォーム関係復元
            this.Size = SettingsData.WindowSize;
            if (SettingsData.WindowPosition.X >= 0 && SettingsData.WindowPosition.Y >= 0) {
                this.Location = SettingsData.WindowPosition;
            }
            if (SettingsData.WindowMaximized) { this.WindowState = FormWindowState.Maximized; }

            txtTwit.Text = "";

            // ↓設定を復元↓

            ConfigTabAndUserDispControl(tabpgHome, uctlDispHome);
            ConfigTabAndUserDispControl(tabpgReply, uctlDispReply);
            ConfigTabAndUserDispControl(tabpgHistory, uctlDispHistory);
            ConfigTabAndUserDispControl(tabpgDirect, uctlDispDirect);
            /*ConfigTabAndUserDispControl(tabpgPublic, uctlDispPublic);*/

            foreach (TabPage tabpage in DEFAULT_TABPAGES) {
                tabpage.ToolTipText = DefaultTabToString(tabpage);
            }

            if (SettingsData.TabDataDic != null) {
                foreach (TabData tabdata in SettingsData.TabDataDic.Values) { MakeTab(tabdata, false); }
            }
            else { SettingsData.TabDataDic = new SerializableDictionary<string, TabData>(); }

            if (SettingsData.UserInfoList != null && SettingsData.UserInfoList.Count > 0) {
                Twitter.AccessToken = SettingsData.UserInfoList[0].AccessToken;
                Twitter.AccessTokenSecret = SettingsData.UserInfoList[0].AccessTokenSecret;
                Twitter.ScreenName = SettingsData.UserInfoList[0].ScreenName;
                Twitter.ID = SettingsData.UserInfoList[0].ID;
                _isAuthenticated = true;
                _profileRenew_IsForce = true;
                lblUserName.Text = "(未取得)";
                tsmi_プロフィール.Enabled = true;
                foreach (ToolStripMenuItem item in tsmi_プロフィール.DropDownItems) { item.Enabled = true; }
            }
            else {
                SettingsData.UserInfoList = new List<UserInfo>();
                _isAuthenticated = false;
            }

            // スレッド作成
            _bgThread = new Thread(AutoGetTweet);
            _bgThread.IsBackground = true;
            _bgThread.Name = "AutoGetThread";
            //_bgThread.SetApartmentState(ApartmentState.STA);
            _bgThread.Start();
        }
        #endregion (FrmMain_Load)
        //-------------------------------------------------------------------------------
        #region FrmMain_FormClosed フォームクローズ時
        //-------------------------------------------------------------------------------
        //
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // フォーム情報保存
            SettingsData.WindowPosition = this.Location;
            SettingsData.WindowSize = this.Size;
            SettingsData.WindowMaximized = (this.WindowState == FormWindowState.Maximized);

            SettingsData.Save();

            tasktray.Visible = false;
        }
        #endregion (FrmMain_FormClosed)
        //-------------------------------------------------------------------------------
        #region FrmMain_ResizeBegin サイズ変更開始時
        //-------------------------------------------------------------------------------
        //
        private void FrmMain_ResizeBegin(object sender, EventArgs e)
        {

        }
        #endregion (FrmMain_ResizeBegin)
        //-------------------------------------------------------------------------------
        #region FrmMain_ResizeEnd サイズ変更終了時
        //-------------------------------------------------------------------------------
        //
        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {

        }
        #endregion (FrmMain_ResizeEnd)
        //-------------------------------------------------------------------------------
        #region ↓諸コントロール
        //-------------------------------------------------------------------------------
        #region btnTwit_Click つぶやくボタン
        //-------------------------------------------------------------------------------
        //
        private void btnTwit_Click(object sender, EventArgs e)
        {
            Action<string> act = new Action<string>(Update);
            string text = SettingsData.Header + txtTwit.Text + SettingsData.Footer;
            act.BeginInvoke(text, Utilization.InvokeCallback, null);
        }
        #endregion (btnTwit_Click)
        //-------------------------------------------------------------------------------
        #region txtTwit_TextChanged テキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtTwit_TextChanged(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;

            int combinedlength = SettingsData.Header.Length + txtbox.Text.Length + SettingsData.Footer.Length;

            // 残り文字数表示処理
            int restLen = MAX_LENGTH - combinedlength;
            //int restLen = MAX_LENGTH - _twitter.UrlEncode(txtbox.Text).Length;

            if (restLen < 0) {
                lblRest.ForeColor = Color.Red;
                lblRest.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold);
            }
            else {
                lblRest.ForeColor = SystemColors.WindowText;
                lblRest.Font = new Font(this.Font.FontFamily, this.Font.Size);
            }
            lblRest.Text = restLen.ToString();

            // 返信処理
            switch (_stateStatusState) {
                case StatusState.Reply:
                    if (!txtbox.Text.StartsWith(_statsName)) { ReSetStatusState(); }
                    break;
                case StatusState.Quote:
                    break;
                case StatusState.DirectMessage:
                    break;
                default:
                    break;
            }

            // URL短縮可否
            if (!_bURLShortening) {
                string[] urls = Utilization.ExtractURL(txtTwit.Text);
                btnURLShorten.Enabled = (urls.Length > 0) && URLShortener.ExistShortenableURL(urls);
            }
        }
        #endregion (txtTwit_TextChanged)
        //-------------------------------------------------------------------------------
        #region txtTwit_KeyDown キー押下時
        //-------------------------------------------------------------------------------
        //
        private void txtTwit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                if (!e.Shift || !e.Control) {
                    e.SuppressKeyPress = true;
                    Action<string> act = new Action<string>(Update);
                    string text = SettingsData.Header + txtTwit.Text + SettingsData.Footer;
                    act.BeginInvoke(text, Utilization.InvokeCallback, null);
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (txtTwit_KeyDown)
        //-------------------------------------------------------------------------------
        #region btnStateReset_Click ×ボタン(状態リセットボタン)クリック時
        //-------------------------------------------------------------------------------
        //
        private void btnStateReset_Click(object sender, EventArgs e)
        {
            _stateStatusState = StatusState.Normal;
            lblTweetStatus.Text = "";
            btnStateReset.Visible = false;
        }
        #endregion (btnStateReset_Click)
        //-------------------------------------------------------------------------------
        #region tabTwitDisp_TabMoved タブ移動時
        //-------------------------------------------------------------------------------
        //
        private void tabTwitDisp_TabMoved(object sender, TabMoveEventArgs e)
        {
            TabControlEx tab = (TabControlEx)sender;
            int movableMinIndex = tab.MinMovableIndex;

            lock (SettingsData.TabDataDic) {
                List<KeyValuePair<string, TabData>> list = new List<KeyValuePair<string, TabData>>(SettingsData.TabDataDic);
                var val = list[e.MoveSrcIndex - movableMinIndex];
                list.Remove(val);
                list.Insert(e.MoveDstIndex - movableMinIndex, val);

                SettingsData.TabDataDic = new SerializableDictionary<string, TabData>();
                foreach (var kvp in list) {
                    SettingsData.TabDataDic.Add(kvp.Key, kvp.Value);
                }
                SettingsData.Save();
            }
        }
        #endregion (tabTwitDisp_TabMoved)
        //-------------------------------------------------------------------------------
        #region DispTwit_TweetItemClick 特殊項目クリック時
        //-------------------------------------------------------------------------------
        //
        private void DispTwit_TweetItemClick(object sender, TweetItemClickEventArgs e)
        {
            MakeNewTab(e.Type, e.Item);
        }
        #endregion (DispTwit_TweetItemClick)
        //-------------------------------------------------------------------------------
        #region llblFollowing_LinkClicked フォロー数ラベルクリック時
        //-------------------------------------------------------------------------------
        //
        private void llblFollowing_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmFollower frm = new FrmFollower(ImageList) {
                FormType = FrmFollower.EFormType.Following,
                Text = "フォローしている人"
            };
            frm.ShowDialog(this);
        }
        #endregion (llblFollowing_LinkClicked)
        //-------------------------------------------------------------------------------
        #region llblFollower_LinkClicked フォロワー数ラベルクリック時
        //-------------------------------------------------------------------------------
        //
        private void llblFollower_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmFollower frm = new FrmFollower(ImageList) {
                FormType = FrmFollower.EFormType.Follower,
                Text = "フォローされている人"
            };
            frm.ShowDialog(this);
        }
        #endregion (llblFollower_LinkClicked)
        //-------------------------------------------------------------------------------
        #region DispTwit_OpenURLRequest URLオープン要請時
        //-------------------------------------------------------------------------------
        //
        private void DispTwit_OpenURLRequest(object sender, OpenURLEventArgs e)
        {
            Utilization.OpenBrowser(e.url, e.useInternalBrowser);
        }
        #endregion (OpenURLRequest)
        //-------------------------------------------------------------------------------
        #region btnURLShorten_ButtonClick URL短縮ボタンクリック
        //-------------------------------------------------------------------------------
        //
        private void btnURLShorten_ButtonClick(object sender, EventArgs e)
        {
            TextURLShorten(SettingsData.URLShortenType);
        }
        #endregion (btnURLShorten_ButtonClick)
        //-------------------------------------------------------------------------------
        #region menuShortenType_Opening URL短縮ボタンメニュー表示時
        //-------------------------------------------------------------------------------
        //
        private void menuShortenType_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripMenuItem item in menuShortenType.Items) {
                item.Checked = ((URLShortenType)item.Tag == SettingsData.URLShortenType);
            }
        }
        #endregion (menuShortenType_Opening)
        //-------------------------------------------------------------------------------
        #region btnURLShorten_MenuClick URL短縮ボタンメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void btnURLShorten_MenuClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            URLShortenType type = (URLShortenType)tsmi.Tag;
            SettingsData.URLShortenType = type;

            TextURLShorten(type);
        }
        #endregion (btnURLShorten_MenuClick)
        //-------------------------------------------------------------------------------
        #endregion (諸コントロール)
        //===============================================================================
        #region ↓行右クリックメニュー
        //-------------------------------------------------------------------------------
        #region TwitMenu_RowContextmenu_Click 行右クリックメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_RowContextmenu_Click(object sender, TwitRowMenuEventArgs e)
        {
            switch (e.EventType) {
                case RowEventType.Reply:
                    TwitMenu_Reply_Click(sender, e);
                    break;
                case RowEventType.Quote:
                    TwitMenu_Quote_Click(sender, e);
                    break;
                case RowEventType.Retweet:
                    TwitMenu_Retweet_Click(sender, e);
                    break;
                case RowEventType.DirectMessage:
                    TwitMenu_DirectMessage_Click(sender, e);
                    break;
                //-------------------------------------------------------------------------------
                case RowEventType.DisplayConversation:
                    TwitMenu_DisplayConversation_Click(sender, e);
                    break;
                //-------------------------------------------------------------------------------
                case RowEventType.Favorite:
                    TwitMenu_Favorite_Click(sender, e);
                    break;
                case RowEventType.Unfavorite:
                    TwitMenu_UnFavorite_Click(sender, e);
                    break;
                //-------------------------------------------------------------------------------
                case RowEventType.DisplayUserTweet:
                    TwitMenu_DisplayUserTweet_Click(sender, e);
                    break;
                case RowEventType.MakeUserTab:
                    TwitMenu_MakeUserTab_Click(sender, e);
                    break;
                case RowEventType.MakeUserListTab:
                    TwitMenu_MakeUserListTab_Click(sender, e);
                    break;
                //-------------------------------------------------------------------------------
                case RowEventType.Delete:
                    TwitMenu_Delete_Click(sender, e);
                    break;
                //-------------------------------------------------------------------------------
                case RowEventType.OlderTweetRequest:
                    TwitMenu_OlderDataRequest_Click(sender, e);
                    break;
                case RowEventType.MoreRecentTweetRequest:
                    TwitMenu_MoreRecentDataRequest_Click(sender, e);
                    break;
                case RowEventType.SpecifyTimeTweetRequest:
                    TwitMenu_SpecifyTimeTweetRequest_Click(sender, e);
                    break;
            }
        }
        #endregion (TwitMenu_RowContextmenu_Click)
        //===============================================================================
        #region TwitMenu_Reply_Click リプライ
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_Reply_Click(object sender, TwitRowMenuEventArgs e)
        {
            txtTwit.Text = '@' + e.TwitData.UserScreenName + ' ';

            txtTwit.Focus();
            txtTwit.Select(txtTwit.Text.Length, 0);

            _statlID = e.TwitData.StatusID;
            _statsName = txtTwit.Text;
            SetStatusState(StatusState.Reply, e.TwitData.UserScreenName + "宛のリプライ");
        }
        #endregion (TwitMenu_Reply_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_Quote_Click 引用
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_Quote_Click(object sender, TwitRowMenuEventArgs e)
        {
            txtTwit.Text = GetQuoteString(e.TwitData.UserScreenName, e.TwitData.Text);

            txtTwit.Focus();
            txtTwit.Select(0, 0);
        }
        #endregion (TwitMenu_Quote_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_Retweet_Click リツイート
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_Retweet_Click(object sender, TwitRowMenuEventArgs e)
        {
            if (Message.ShowQuestionMessage("リツイートしますか？") == DialogResult.Yes) {
                Twitter.statuses_retweet(e.TwitData.StatusID);
            }
        }
        #endregion (TwitMenu_Retweet_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_DirectMessage_Click ダイレクトメッセージ
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_DirectMessage_Click(object sender, TwitRowMenuEventArgs e)
        {
            txtTwit.Focus();
            txtTwit.Select(0, 0);

            _statlID = e.TwitData.UserID;
            _statsName = e.TwitData.UserScreenName;
            SetStatusState(StatusState.DirectMessage, e.TwitData.UserScreenName + "宛のダイレクトメッセージ");
        }
        #endregion (TwitMenu_DirectMessage_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_DisplayConversation_Click 会話表示
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_DisplayConversation_Click(object sender, TwitRowMenuEventArgs e)
        {
            FrmDispTweet frm = new FrmDispTweet(this, imageListWrapper);
            frm.FormType = FrmDispTweet.EFormType.Conversation;
            frm.ReplyStartTwitdata = e.TwitData;
            frm.Show(this);
        }
        #endregion (TwitMenu_DisplayConversation_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_Favorite_Click お気に入り追加
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_Favorite_Click(object sender, TwitRowMenuEventArgs e)
        {
            if (CreateFavorite(e.TwitData.StatusID)) {
                UctlDispTwit uctldisp = (UctlDispTwit)sender;
                e.TwitData.Favorited = true;
                uctldisp.ReConfigAll();
            }
        }
        #endregion (TwitMenu_Favorite_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_UnFavorite_Click お気に入り削除
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_UnFavorite_Click(object sender, TwitRowMenuEventArgs e)
        {
            if (DestroyFavorite(e.TwitData.StatusID)) {
                UctlDispTwit uctldisp = (UctlDispTwit)sender;
                e.TwitData.Favorited = false;
                uctldisp.ReConfigAll();
            }
        }
        #endregion (TwitMenu_UnFavorite_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_DisplayUserTweet_Click ユーザー発言表示
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_DisplayUserTweet_Click(object sender, TwitRowMenuEventArgs e)
        {
            Utilization.ShowUserTweet(this, e.TwitData.UserScreenName);
        }
        #endregion (TwitMenu_DisplayUserTweet_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_MakeUserTab_Click ユーザータブ追加
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_MakeUserTab_Click(object sender, TwitRowMenuEventArgs e)
        {
            MakeNewTab(TabSearchType.User, e.TwitData.UserScreenName);
        }
        #endregion (TwitMenu_MakeUserTab_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_MakeUserListTab_Click ユーザー所有リストのタブ作成
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_MakeUserListTab_Click(object sender, TwitRowMenuEventArgs e)
        {
            MakeNewTab(TabSearchType.List, null, e.TwitData.UserScreenName);
        }
        #endregion (TwitMenu_MakeUserListTab_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_Delete_Click 削除
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_Delete_Click(object sender, TwitRowMenuEventArgs e)
        {
            if (Message.ShowQuestionMessage("削除してよろしいですか？") == DialogResult.Yes) {
                Delete(e.TwitData.StatusID);
            }
        }
        #endregion (TwitMenu_Delete_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_OlderDataRequest_Click より古い発言取得
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_OlderDataRequest_Click(object sender, TwitRowMenuEventArgs e)
        {
            UctlDispTwit uctldisp = (UctlDispTwit)sender;
            if (_dispTwitDic.Values.Any((u) => u == uctldisp)) {
                InvokeTweetGet(new Func<UctlDispTwit, long, bool>(GetOlderTweets), e.TwitData.StatusID, (UctlDispTwit)sender, "より古いデータ取得中...");
            }
        }
        #endregion (TwitMenu_OlderDataRequest_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_MoreRecentDataRequest_Click より新しい発言取得
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_MoreRecentDataRequest_Click(object sender, TwitRowMenuEventArgs e)
        {
            UctlDispTwit uctldisp = (UctlDispTwit)sender;
            if (_dispTwitDic.Values.Any((u) => u == uctldisp)) {
                InvokeTweetGet(new Func<UctlDispTwit, long, bool>(GetMoreRecentTweets), e.TwitData.StatusID, (UctlDispTwit)sender, "より新しいデータ取得中...");
            }
        }
        #endregion (TwitMenu_MoreRecentDataRequest_Click)
        //-------------------------------------------------------------------------------
        #region TwitMenu_SpecifyTimeTweetRequest_Click 時間を指定して発言を取得
        //-------------------------------------------------------------------------------
        //
        private void TwitMenu_SpecifyTimeTweetRequest_Click(object sender, TwitRowMenuEventArgs e)
        {
            using (FrmGetTweet frm = new FrmGetTweet()) {
                frm.EnableDateTimeTo = true;
                frm.DateTimeTo = e.TwitData.Time;
                if (frm.ShowDialog() == DialogResult.OK) {
                    Utilization.InvokeTransaction(() =>
                      GetSpecifyTimeTweets((UctlDispTwit)sender, frm.EnableDateTimeFrom, frm.DateTimeFrom, frm.EnableDateTimeTo, frm.DateTimeTo),
                    () => this.Invoke(new Action(() => tssLabel.Text = "発言取得中..."))
                    );
                    this.Invoke(new Action(() => tssLabel.Text = ""));
                }
            }
        }
        #endregion (TwitMenu_SpecifyTimeTweetRequest_Click)
        //-------------------------------------------------------------------------------
        #endregion (↓行右クリックメニュー)
        //===============================================================================
        #region ↓フォームメニュー
        //-------------------------------------------------------------------------------
        #region tsmiファイル_設定_Click 設定
        //-------------------------------------------------------------------------------
        //
        private void tsmiファイル_設定_Click(object sender, EventArgs e)
        {
            LockAndProcess("設定画面待機中...", _mreThreadConfirm, _mreThreadRun, new Action(() =>
            {
                using (FrmConfig frmconf = new FrmConfig()) {
                    frmconf.SettingsData = SettingsData;
                    if (frmconf.ShowDialog() == DialogResult.OK) {
                        SettingsData = frmconf.SettingsData;
                        SettingsData.Save();

                        foreach (TabPage tabpage in DEFAULT_TABPAGES) {
                            tabpage.ToolTipText = DefaultTabToString(tabpage);
                            UctlDispTwit dispTwit = _dispTwitDic[tabpage];
                            lock (_autoRenewDic) {
                                var data = _autoRenewDic[dispTwit];
                                int sec = (dispTwit == uctlDispHome) ? SettingsData.GetInterval_Home :
                                          (dispTwit == uctlDispReply) ? SettingsData.GetInterval_Reply :
                                          (dispTwit == uctlDispHistory) ? SettingsData.GetInterval_History :
                                          (dispTwit == uctlDispDirect) ? SettingsData.GetInterval_Direct : 0;
                                data.Interval = new TimeSpan(0, 0, sec);
                            }
                        }

                        foreach (UctlDispTwit uctldisp in _dispTwitDic.Values) {
                            uctldisp.ReConfigAll();
                        }

                        txtTwit_TextChanged(txtTwit, EventArgs.Empty); // 長さ再設定
                    }
                }

            }));
        }
        #endregion (tsmiファイル_設定_Click)
        //-------------------------------------------------------------------------------
        #region tsmiファイル_終了_Click 終了
        //-------------------------------------------------------------------------------
        //
        private void tsmiファイル_終了_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (tsmiファイル_終了_Click)
        //-------------------------------------------------------------------------------
        #region tsmi認証_Click 認証メニュー
        //-------------------------------------------------------------------------------
        //
        private void tsmi認証_Click(object sender, EventArgs e)
        {
            LockAndProcess("認証画面待機中...", _mreThreadConfirm, _mreThreadRun, new Action(() =>
            {
                UserInfo userdata;
                if (Twitter.OAuth(out userdata)) {

                    Twitter.AccessToken = userdata.AccessToken;
                    Twitter.AccessTokenSecret = userdata.AccessTokenSecret;
                    Twitter.ScreenName = userdata.ScreenName;
                    Twitter.ID = userdata.ID;

                    _profileRenew_IsForce = true;

                    //Console.WriteLine("Access_Token: " + access_token);
                    //Console.WriteLine("Access_Token_Secret: " + access_token_secret);

                    SettingsData.UserInfoList.Insert(0, userdata);
                    SettingsData.Save();

                    Message.ShowInfoMessage("認証に成功しました");

                    _mreThreadRun.Set();
                    _isAuthenticated = true;
                }
                else {
                    Message.ShowInfoMessage("認証ができませんでした");
                }
            }));
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmItem_認証_Click)
        //-------------------------------------------------------------------------------
        #region tsmi更新_Click 更新
        //-------------------------------------------------------------------------------
        //
        private void tsmi更新_Click(object sender, EventArgs e)
        {
            UctlDispTwit uctldisp = SelectedUctlDispTwit();

            while (!Monitor.TryEnter(_autoRenewDic)) {
                tssllbl2.Text = "更新待機中...";
                Thread.Sleep(10);
                Application.DoEvents();
            }

            _autoRenewDic[uctldisp].IsForce = true;

            tssllbl2.Text = "";
            Monitor.Exit(_autoRenewDic);
        }
        #endregion (tsmi更新_Click)
        //-------------------------------------------------------------------------------
        #region tsmiSpecifyTime_Click 時刻を指定して発言取得
        //-------------------------------------------------------------------------------
        //
        private void tsmiSpecifyTime_Click(object sender, EventArgs e)
        {
            using (FrmGetTweet frm = new FrmGetTweet()) {
                if (frm.ShowDialog() == DialogResult.OK) {
                    Utilization.InvokeTransaction(() =>
                      GetSpecifyTimeTweets(SelectedUctlDispTwit(), frm.EnableDateTimeFrom, frm.DateTimeFrom, frm.EnableDateTimeTo, frm.DateTimeTo),
                    () => this.Invoke(new Action(() => tssLabel.Text = "発言取得中..."))
                    );
                    this.Invoke(new Action(() => tssLabel.Text = ""));
                }
            }
        }
        #endregion (tsmiSpecifyTime_Click)
        //-------------------------------------------------------------------------------
        #region tsmiClearTweets_Click 発言をクリア
        //-------------------------------------------------------------------------------
        //
        private void tsmiClearTweets_Click(object sender, EventArgs e)
        {
            if (Message.ShowQuestionMessage("現在のタブの発言を全てクリアしてよろしいですか？") == DialogResult.Yes) {
                UctlDispTwit uctldisp = SelectedUctlDispTwit();
                uctldisp.ClearAll();
                //Console.WriteLine(GC.GetTotalMemory(false));
                GC.Collect();
                //Console.WriteLine(GC.GetTotalMemory(false));
            }
        }
        #endregion (tsmiClearTweets_Click)
        //-------------------------------------------------------------------------------
        #region tsmiプロフィール更新_Click フォロー数・フォロワー数・発言数更新
        //-------------------------------------------------------------------------------
        //
        private void tsmiプロフィール更新_Click(object sender, EventArgs e)
        {
            _profileRenew_IsForce = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmiプロフィール更新_Click)
        //-------------------------------------------------------------------------------
        #region tsmi_子画面_DropDownOpening 子画面メニューオープン時
        //-------------------------------------------------------------------------------
        private Dictionary<Form, ToolStripMenuItem> _formMenuDic = null;
        //
        private void tsmi_子画面_DropDownOpening(object sender, EventArgs e)
        {
            tsmi_子画面.DropDownItems.Clear();

            var menuDic = _formMenuDic;
            _formMenuDic = new Dictionary<Form, ToolStripMenuItem>();

            int count = 0;
            foreach (Form form in Application.OpenForms) {
                if (form == this) { continue; }
                ToolStripMenuItem tsmi;
                if (menuDic != null && menuDic.ContainsKey(form)) {
                    tsmi = menuDic[form];
                    menuDic.Remove(form);
                }
                else {
                    tsmi = new ToolStripMenuItem() {
                        Text = form.Text,
                        Tag = form,
                    };
                    tsmi.Click += tsmi小画面_Dialog_Click;
                }
                _formMenuDic.Add(form, tsmi);
                tsmi_子画面.DropDownItems.Add(tsmi);
                count++;
            }

            if (menuDic != null && menuDic.Count > 0) { 
                foreach (var tsmi in menuDic.Values) { tsmi.Dispose(); } 
                menuDic.Clear();
            }

            if (count == 0) { tsmi_子画面.DropDownItems.Add(tsmi子画面_nothing); }
        }
        #endregion (tsmi_子画面_DropDownOpening)
        //-------------------------------------------------------------------------------
        #region tsmi小画面_Dialog_Click 小画面ダイアログメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmi小画面_Dialog_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            Form form = (Form)tsmi.Tag;
            form.BringToFront();
        }
        #endregion (tsmi小画面_Dialog_Click)
        //-------------------------------------------------------------------------------
        #endregion (メニュー)
        //===============================================================================
        #region ↓タブ右クリックメニュー
        //-------------------------------------------------------------------------------
        #region tabTwitDisp_MouseDown タブクリック時
        //-------------------------------------------------------------------------------
        //
        private void tabTwitDisp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                for (int i = 0; i < tabTwitDisp.TabCount; i++) {
                    if (tabTwitDisp.GetTabRect(i).Contains(e.X, e.Y)) {
                        tabTwitDisp.SelectedIndex = i;
                        menuTab.Show(tabTwitDisp, e.Location, ToolStripDropDownDirection.BelowRight);
                    }
                }
            }
        }
        #endregion (tabTwitDisp_MouseDown)
        //-------------------------------------------------------------------------------
        #region menuTab_Opening 表示前
        //-------------------------------------------------------------------------------
        //
        private void menuTab_Opening(object sender, CancelEventArgs e)
        {
            bool isDefault = DEFAULT_TABPAGES.Contains(tabTwitDisp.SelectedTab);
            tsmiTab_DeleteTab.Visible = !isDefault;
            tsmiTab_EditTab.Visible = !isDefault;
        }
        #endregion (menuTab_Opening)
        //-------------------------------------------------------------------------------
        #region tsmiTab_MakeTab_Click タブ作成クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiTab_MakeTab_Click(object sender, EventArgs e)
        {
            MakeNewTab();
        }
        #endregion (tsmiTab_MakeTab_Click)
        //-------------------------------------------------------------------------------
        #region tsmiTab_EditTab_Click タブ編集クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiTab_EditTab_Click(object sender, EventArgs e)
        {
            LockAndProcess("タブ編集画面待機中...", _mreThreadConfirm, _mreThreadRun, new Action(() =>
            {
                TabPage tabpg = tabTwitDisp.SelectedTab;
                TabData tabdata;
                lock (SettingsData.TabDataDic) { tabdata = SettingsData.TabDataDic[(string)tabpg.Tag]; }
                using (FrmMakeTab frm = new FrmMakeTab(Twitter)) {
                    frm.TabData = tabdata;
                    if (frm.ShowDialog() == DialogResult.OK) {
                        lock (SettingsData.TabDataDic) {
                            SettingsData.TabDataDic.Remove((string)tabpg.Tag);
                            _dispTwitDic[tabpg].Tag = tabpg.Tag = tabpg.Text = frm.TabData.TabName;
                            SettingsData.TabDataDic.Add((string)tabpg.Tag, frm.TabData);
                        }
                        SettingsData.Save();
                        tabpg.ToolTipText = TabDataToString(frm.TabData);

                        while (!Monitor.TryEnter(_autoRenewDic)) {
                            tssllbl2.Text = "タブ設定更新待機中...";
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }

                        tssllbl2.Text = "";

                        _autoRenewDic[_dispTwitDic[tabpg]].Interval = new TimeSpan(0, 0, frm.TabData.GetInterval);

                        // 変化があった時
                        if (tabdata.SearchType != frm.TabData.SearchType || tabdata.SearchWord != frm.TabData.SearchWord) {
                            _autoRenewDic[_dispTwitDic[tabpg]].IsForce = true;
                            _dispTwitDic[tabpg].ClearAll();
                        }

                        Monitor.Exit(_autoRenewDic);
                    }
                    else { return; }
                }
            }));
        }
        #endregion (tsmiTab_EditTab_Click)
        //-------------------------------------------------------------------------------
        #region tsmiTab_DeleteTab_Click タブ削除クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiTab_DeleteTab_Click(object sender, EventArgs e)
        {
            if (Message.ShowQuestionMessage("本当に削除してよろしいですか？") == System.Windows.Forms.DialogResult.Yes) {
                LockAndProcess("タブ削除待機中...", _mreThreadTabConfirm, _mreThreadTabRun, new Action(() =>
                {
                    TabPage tabpg = tabTwitDisp.SelectedTab;
                    tabTwitDisp.TabPages.Remove(tabpg);

                    while (!Monitor.TryEnter(_autoRenewDic)) {
                        tssllbl2.Text = "タブ削除待機中...";
                        Thread.Sleep(1);
                        Application.DoEvents();
                    }
                    tssllbl2.Text = "";
                    _autoRenewDic.Remove(_dispTwitDic[tabpg]);
                    Monitor.Exit(_autoRenewDic);

                    _dispTwitDic.Remove(tabpg);
                    lock (SettingsData.TabDataDic) { SettingsData.TabDataDic.Remove((string)tabpg.Tag); }
                    tabpg.Dispose();

                    SettingsData.Save();
                }));
            }
        }
        #endregion (tsmiTab_DeleteTab_Click)
        //-------------------------------------------------------------------------------
        #endregion (タブ右クリックメニュー)
        //===============================================================================
        #region ↓タスクトレイメニュー
        //-------------------------------------------------------------------------------
        #region tsmiTasktray_Display_Click 表示クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiTasktray_Display_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }
        #endregion (tsmiTasktray_Display_Click)
        //-------------------------------------------------------------------------------
        #region tsmiTasktray_Exit_Click 終了クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiTasktray_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (tsmiTasktray_Exit_Click)
        //-------------------------------------------------------------------------------
        #endregion (↓タスクトレイメニュー)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //===============================================================================
        #region メソッド
        //-------------------------------------------------------------------------------
        #region +RegisterUctlDispTwitEvent UctlDispTwitのイベントを登録します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// UctlDispTwitのイベントを登録します。
        /// </summary>
        /// <param name="uctlDisp"></param>
        public void RegisterUctlDispTwitEvent(UctlDispTwit uctlDisp)
        {
            uctlDisp.RowContextMenu_Click += TwitMenu_RowContextmenu_Click;
            uctlDisp.TweetItemClick += DispTwit_TweetItemClick;
            uctlDisp.OpenURLRequest += DispTwit_OpenURLRequest;
        }
        #endregion (RegisterUctlDispTwitEvent)
        //-------------------------------------------------------------------------------
        #region -MakeNewTab 新規タブ作成
        //-------------------------------------------------------------------------------
        #region (TabSearchType,string,[opt]string) Main
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 新規タブを作成します。
        /// </summary>
        /// <param name="type">作成タブタイプ</param>
        /// <param name="data">タブデータ</param>
        /// <param name="listowner">[opt]リストのオーナー</param>
        private void MakeNewTab(TabSearchType type, string data, string listowner = null)
        {
            LockAndProcess("タブ作成待機中...", _mreThreadTabConfirm, _mreThreadTabRun, new Action(() =>
            {
                TabData tabdata = new TabData() {
                    TabName = data,
                    SearchWord = data,
                    ListOwner = listowner
                };

                tabdata.SearchType = type;

                using (FrmMakeTab frm = new FrmMakeTab(Twitter)) {
                    frm.TabData = tabdata;
                    if (frm.ShowDialog() == DialogResult.OK) {
                        tabdata = frm.TabData;
                    }
                    else { return; }
                }

                lock (SettingsData.TabDataDic) { SettingsData.TabDataDic.Add(tabdata.TabName, tabdata); }
                MakeTab(tabdata, true);

                SettingsData.Save();
            }));
        }
        //-------------------------------------------------------------------------------
        #endregion ((TabSearchType,string,(opt)string) Main)
        #region (void)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 新規タブを作成します。
        /// </summary>
        private void MakeNewTab()
        {
            MakeNewTab(TabSearchType.Keyword, "");
        }
        //-------------------------------------------------------------------------------
        #endregion ((void))
        #region (ItemType,string)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 新規タブを作成します。
        /// </summary>
        /// <param name="type">新規タブのタイプ</param>
        /// <param name="data">データ</param>
        private void MakeNewTab(ItemType type, string data)
        {
            TabSearchType searchType;
            switch (type) {
                case ItemType.HashTag:
                    searchType = TabSearchType.Keyword;
                    break;
                case ItemType.User:
                    searchType = TabSearchType.User;
                    break;
                default:
                    return;
            }
            MakeNewTab(searchType, data);
        }
        //-------------------------------------------------------------------------------
        #endregion ((ItemType,string))
        #endregion (MakeNewTab)
        //-------------------------------------------------------------------------------
        #region -MakeTab タブとその内部の作成と設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>タブとその内部の作成と設定を行います。</para>
        /// <para>SettingsDataへの辞書項目追加を行ってからこのメソッドを行えばタブ作成が完了します。</para>
        /// </summary>
        /// <param name="tabdata"></param>
        /// <param name="selectTab">タブを選択するか</param>
        private void MakeTab(TabData tabdata, bool selectTab)
        {
            TabPage newtabpg = new TabPage(tabdata.TabName) {
                Tag = tabdata.TabName
            };
            UctlDispTwit newDispTwit = new UctlDispTwit() {
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Tag = tabdata.TabName
            };
            newtabpg.Controls.Add(newDispTwit);
            tabTwitDisp.TabPages.Add(newtabpg);

            lock (SettingsData.TabDataDic) { newtabpg.ToolTipText = TabDataToString(SettingsData.TabDataDic[(string)newtabpg.Tag]); }

            ConfigTabAndUserDispControl(newtabpg, newDispTwit);

            if (selectTab) { tabTwitDisp.SelectedTab = newtabpg; }
        }
        #endregion (MakeTab)
        //-------------------------------------------------------------------------------
        #region -ConfigTabAndUserDispControl タブと表示コントロールの設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// タブと表示コントロールの設定を行います。SettingsData設定後に行ってください。
        /// </summary>
        /// <param name="tabpage"></param>
        /// <param name="uctlDisp"></param>
        private void ConfigTabAndUserDispControl(TabPage tabpage, UctlDispTwit uctlDisp)
        {
            uctlDisp.ImageListWrapper= imageListWrapper;
            _dispTwitDic.Add(tabpage, uctlDisp);
            RegisterUctlDispTwitEvent(uctlDisp);
            SetAutoRenewData(uctlDisp);
        }
        #endregion (ConfigTabAndUserDispControl)
        //-------------------------------------------------------------------------------
        #region -SetAutoRenewData 自動更新データを設定します。
        //-------------------------------------------------------------------------------
        //
        private void SetAutoRenewData(UctlDispTwit dispTwit)
        {
            AutoRenewData data = new AutoRenewData() {
                IsForce = true,
                Standard = DateTime.Now,
                GetTweetType = GetTweetType.MostRecent
            };

            if (dispTwit == uctlDispHome) { data.Interval = new TimeSpan(0, 0, SettingsData.GetInterval_Home); }
            else if (dispTwit == uctlDispReply) { data.Interval = new TimeSpan(0, 0, SettingsData.GetInterval_Reply); }
            else if (dispTwit == uctlDispHistory) { data.Interval = new TimeSpan(0, 0, SettingsData.GetInterval_History); }
            else if (dispTwit == uctlDispDirect) { data.Interval = new TimeSpan(0, 0, SettingsData.GetInterval_Direct); }
            else {
                lock (SettingsData.TabDataDic) {
                    data.Interval = new TimeSpan(0, 0, SettingsData.TabDataDic[(string)dispTwit.Tag].GetInterval);
                }
            }

            while (!Monitor.TryEnter(_autoRenewDic)) {
                tssllbl2.Text = "自動更新データ追加待機中...";
                Thread.Sleep(10);
                Application.DoEvents();
            }
            tssllbl2.Text = "";
            _autoRenewDic.Add(dispTwit, data);
            Monitor.Exit(_autoRenewDic);
        }
        //-------------------------------------------------------------------------------
        #endregion (SetAutoRenewData)

        //===============================================================================
        #region -SetStatusState 発言状態設定
        //-------------------------------------------------------------------------------
        //
        private void SetStatusState(StatusState state, string lbltext)
        {
            if (state == StatusState.Normal) { return; }

            _stateStatusState = state;
            lblTweetStatus.Text = lbltext;
            btnStateReset.Visible = true;
        }
        #endregion (SetStatusState)
        //-------------------------------------------------------------------------------
        #region -ReSetStatusState 発言状態リセット
        //-------------------------------------------------------------------------------
        //
        private void ReSetStatusState()
        {
            _stateStatusState = StatusState.Normal;
            lblTweetStatus.Text = "";
            btnStateReset.Visible = false;
        }
        #endregion (ReSetStatusState)

        //-------------------------------------------------------------------------------
        #region -SetProfileData プロフィールデータを設定します。
        //-------------------------------------------------------------------------------
        //
        private void SetProfileData(UserProfile profile)
        {
            if (!lblUserName.Font.Bold) {
                lblUserName.Font = new Font(lblUserName.Font, FontStyle.Bold);
                llblFollower.Enabled = llblFollowing.Enabled = true;
            }
            StringBuilder namesb = new StringBuilder();
            if (profile.Protected) { namesb.Append('◆'); }
            namesb.Append(profile.ScreenName);
            namesb.Append('/');
            namesb.Append(profile.UserName);
            lblUserName.Text = namesb.ToString();
            llblFollower.Text = profile.FollowerNum.ToString();
            llblFollowing.Text = profile.FollowingNum.ToString();
            lblStatuses.Text = profile.StatusNum.ToString();

            if (!tsmi_プロフィール.Enabled) {
                tsmi_プロフィール.Enabled = true;
                foreach (ToolStripMenuItem item in tsmi_プロフィール.DropDownItems) { item.Enabled = true; }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (SetProfileData)

        //-------------------------------------------------------------------------------
        #region -TextURLShorten テキストボックス内のURLを短縮します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストボックス内のURLを短縮
        /// </summary>
        /// <param name="type">短縮タイプ</param>
        private void TextURLShorten(URLShortenType type)
        {
            _bURLShortening = true;
            btnURLShorten.Enabled = false;
            string[] urls = Utilization.ExtractURL(txtTwit.Text)
                           .Distinct()
                           .Where((url) => !URLShortener.IsShortenURL(url))
                           .ToArray();
            List<Tuple<string, string>> valList = new List<Tuple<string, string>>();

            Utilization.InvokeTransaction(
                () =>
                {
                    foreach (string url in urls) {
                        string shorturl = URLShortener.Shorten(url, type);
                        valList.Add(new Tuple<string, string>(url, shorturl));
                    }
                }
            );

            string text = txtTwit.Text;
            foreach (var tuple in valList) {
                text = text.Replace(tuple.Item1, tuple.Item2);
            }
            txtTwit.Text = text;

            _bURLShortening = false;
        }
        #endregion (TextURLShorten)

        //===============================================================================
        #region -GetMostRecentTweets 最新のツイートを取得します。 using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 最新のツイートを取得します。
        /// </summary>
        /// <param name="uctldisp">更新を行うUctlDispTwit</param>
        /// <returns>取得に成功したか</returns>
        private bool GetMostRecentTweets(UctlDispTwit uctldisp)
        {
            IEnumerable<TwitData> d;
            bool isFirst = (uctldisp.MaxTweetID == -1);

            try {
                //if (uctldisp == uctlDispPublic) {
                //    d = _twitter.statuses_public_timeline();
                //}
                //else 
                if (uctldisp == uctlDispHome) {
                    int iCount = (isFirst) ? SettingsData.FirstGetNum_Home : SettingsData.RenewGetNum_Home;
                    d = Twitter.statuses_home_timeline(count: iCount);
                }
                else if (uctldisp == uctlDispReply) {
                    int iCount = (isFirst) ? SettingsData.FirstGetNum_Reply : SettingsData.RenewGetNum_Reply;
                    d = Twitter.statuses_mentions(count: iCount);
                }
                else if (uctldisp == uctlDispHistory) {
                    int iCount = (isFirst) ? SettingsData.FirstGetNum_History : SettingsData.RenewGetNum_History;
                    d = Twitter.statuses_user_timeline(count: iCount, include_rts: true);
                }
                else if (uctldisp == uctlDispDirect) {
                    int iCount = (isFirst) ? SettingsData.FirstGetNum_Direct : SettingsData.RenewGetNum_Direct;
                    d = Twitter.direct_messages(count: iCount)
                        .Concat(Twitter.direct_messages_sent(count: iCount))
                        .ToArray();
                }
                else {
                    TabData tabdata;
                    lock (SettingsData.TabDataDic) { tabdata = SettingsData.TabDataDic[(string)uctldisp.Tag]; }
                    int iCount = (isFirst) ? tabdata.FirstGetNum : tabdata.RenewGetNum;
                    switch (tabdata.SearchType) {
                        case TabSearchType.Keyword:
                            d = Twitter.search(q: tabdata.SearchWord, rpp: iCount);
                            break;
                        case TabSearchType.User:
                            d = Twitter.statuses_user_timeline(screen_name: tabdata.SearchWord, count: iCount);
                            break;
                        case TabSearchType.List:
                            d = Twitter.lists_statuses(tabdata.SearchWord, tabdata.ListOwner, per_page: iCount);
                            break;
                        default:
                            Debug.Assert(false, "異常な検索タイプ");
                            d = new TwitData[0];
                            break;
                    }
                }
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex)));
                SYSTEMSOUND.Play();
                return false;
            }
            this.Invoke(new Action(() =>
            {
                string baloontext = uctldisp.AddData(d);
                tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);

                // バルーン設定
                if (!isFirst) {
                    if (uctldisp == uctlDispReply && SettingsData.DisplayReplyBaloon) {
                        if (!string.IsNullOrEmpty(baloontext)) {
                            tasktray.BalloonTipTitle = tasktray.Text + "：Reply 新着有り";
                            tasktray.BalloonTipText = baloontext;
                            tasktray.ShowBalloonTip(BALOON_DURATION);
                        }
                    }
                    else if (uctldisp == uctlDispDirect && SettingsData.DisplayDMBaloon) {
                        if (!string.IsNullOrEmpty(baloontext)) {
                            tasktray.BalloonTipTitle = tasktray.Text + "：Direct Message 新着有り";
                            tasktray.BalloonTipText = baloontext;
                            tasktray.ShowBalloonTip(BALOON_DURATION);
                        }
                    }
                }
            }));
            return true;
        }
        #endregion (RenewTweets)
        //-------------------------------------------------------------------------------
        #region -GetMoreRecentTweets より新しいツイートを取得します。 using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// より新しいツイートを取得します。
        /// </summary>
        /// <param name="uctldisp">更新を行うUctlDispTwit</param>
        /// <returns>取得に成功したか</returns>
        private bool GetMoreRecentTweets(UctlDispTwit uctldisp, long since_id)
        {
            IEnumerable<TwitData> d;
            try {
                if (uctldisp == uctlDispHome) {
                    d = Twitter.statuses_home_timeline(count: SettingsData.RenewGetNum_Home, since_id: since_id);
                }
                else if (uctldisp == uctlDispReply) {
                    d = Twitter.statuses_mentions(count: SettingsData.RenewGetNum_Reply, since_id: since_id);
                }
                else if (uctldisp == uctlDispHistory) {
                    d = Twitter.statuses_user_timeline(count: SettingsData.RenewGetNum_History, since_id: since_id, include_rts: true);
                }
                else if (uctldisp == uctlDispDirect) {
                    d = Twitter.direct_messages(count: SettingsData.RenewGetNum_Direct, since_id: since_id)
                        .Concat(Twitter.direct_messages_sent(count: SettingsData.RenewGetNum_Direct, since_id: since_id))
                        .ToArray();
                }
                else {
                    TabData tabdata;
                    lock (SettingsData.TabDataDic) { tabdata = SettingsData.TabDataDic[(string)uctldisp.Tag]; }
                    switch (tabdata.SearchType) {
                        case TabSearchType.Keyword:
                            d = Twitter.search(q: tabdata.SearchWord, rpp: tabdata.RenewGetNum, since_id: since_id);
                            break;
                        case TabSearchType.User:
                            d = Twitter.statuses_user_timeline(screen_name: tabdata.SearchWord, count: tabdata.RenewGetNum, since_id: since_id);
                            break;
                        case TabSearchType.List:
                            d = Twitter.lists_statuses(tabdata.SearchWord, tabdata.ListOwner, per_page: tabdata.RenewGetNum, since_id: since_id);
                            break;
                        default:
                            Debug.Assert(false, "異常な検索タイプ");
                            d = new TwitData[0];
                            break;
                    }
                }
            }
            catch (TwitterAPIException ex) {
                tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex);
                SYSTEMSOUND.Play();
                return false;
            }
            this.Invoke(new Action(() =>
            {
                uctldisp.AddData(d);
                tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);
            }));

            return true;
        }
        #endregion (RenewTweets)
        //-------------------------------------------------------------------------------
        #region -GetOlderTweets 古いツイートを取得します。 using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// より古いツイートを取得します。
        /// </summary>
        /// <param name="uctldisp">更新を行うUctlDispTwit</param>
        /// <returns>取得に成功したか</returns>
        private bool GetOlderTweets(UctlDispTwit uctldisp, long max_id)
        {
            IEnumerable<TwitData> d;
            try {
                if (uctldisp == uctlDispHome) {
                    d = Twitter.statuses_home_timeline(count: SettingsData.RenewGetNum_Home, max_id: max_id);
                }
                else if (uctldisp == uctlDispReply) {
                    d = Twitter.statuses_mentions(count: SettingsData.RenewGetNum_Reply, max_id: max_id);
                }
                else if (uctldisp == uctlDispHistory) {
                    d = Twitter.statuses_user_timeline(count: SettingsData.RenewGetNum_History, max_id: max_id, include_rts: true);
                }
                else if (uctldisp == uctlDispDirect) {
                    d = Twitter.direct_messages(count: SettingsData.RenewGetNum_Direct, max_id: max_id)
                        .Concat(Twitter.direct_messages_sent(count: SettingsData.RenewGetNum_Direct, max_id: max_id))
                        .ToArray();
                }
                else {
                    TabData tabdata;
                    lock (SettingsData.TabDataDic) { tabdata = SettingsData.TabDataDic[(string)uctldisp.Tag]; }
                    switch (tabdata.SearchType) {
                        case TabSearchType.Keyword:
                            d = Twitter.search(q: tabdata.SearchWord, rpp: tabdata.RenewGetNum, max_id: max_id);
                            break;
                        case TabSearchType.User:
                            d = Twitter.statuses_user_timeline(screen_name: tabdata.SearchWord, count: tabdata.RenewGetNum, max_id: max_id);
                            break;
                        case TabSearchType.List:
                            d = Twitter.lists_statuses(tabdata.SearchWord, tabdata.ListOwner, per_page: tabdata.RenewGetNum, max_id: max_id);
                            break;
                        default:
                            Debug.Assert(false, "異常な検索タイプ");
                            d = new TwitData[0];
                            break;
                    }
                }
            }
            catch (TwitterAPIException ex) {
                tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex);
                SYSTEMSOUND.Play();
                return false;
            }
            this.Invoke(new Action(() =>
            {
                uctldisp.AddData(d);
                tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);
            }));

            return true;
        }
        //-------------------------------------------------------------------------------
        #endregion (GetOlderTweets)
        //-------------------------------------------------------------------------------
        #region -GetSpecifyTimeTweets 指定時間の間のツイートを取得します。 using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private bool GetSpecifyTimeTweets(UctlDispTwit uctldisp, bool useFromDateTime, DateTime dtFrom, bool useToDateTime, DateTime dtTo)
        {
            const int MAX_HOME = 800;
            const int MAX_MENTION = 800;
            const int MAX_USER = 3200;
            const int MAX_SEARCH = 1500;
            const int MAX_LIST = 800;

            if (useFromDateTime && useToDateTime && dtFrom.CompareTo(dtTo) > 0) {
                DateTime tmp = dtFrom;
                dtFrom = dtTo;
                dtTo = dtFrom;
            }

            // 間にあるかを判別する関数
            Func<DateTime, DateTime, DateTime, bool> dtbetween = (dt, dtfrom, dtto) => (dt.CompareTo(dtfrom) >= 0) && (dt.CompareTo(dtto) <= 0);
            Func<DateTime, bool> inbetween = (dt) =>
                !(useFromDateTime && dt.CompareTo(dtFrom) < 0) &&
                !(useToDateTime && dt.CompareTo(dtTo) > 0);

            List<TwitData> datalist = new List<TwitData>();

            try {
                bool findStart = false;
                int i = 1;
                while (true) {
                    IEnumerable<TwitData> d = null;
                    if (uctldisp == uctlDispHome) {
                        if (i == MAX_HOME / 200 + 1) { break; } // 800まで
                        d = Twitter.statuses_home_timeline(count: 200, page: i);
                    }
                    else if (uctldisp == uctlDispReply) {
                        if (i == MAX_MENTION / 200 + 1) { break; } // 800まで
                        d = Twitter.statuses_mentions(count: 200, page: i);
                    }
                    else if (uctldisp == uctlDispHistory) {
                        if (i == MAX_USER / 200 + 1) { break; } // 3200まで
                        d = Twitter.statuses_user_timeline(count: 200, page: i, include_rts: true);
                    }
                    else if (uctldisp == uctlDispDirect) {
                        // ?まで(暫定200)
                        d = Twitter.direct_messages(count: 200, page: i);
                        datalist.AddRange(d);
                        // ?まで(暫定200)
                        d = Twitter.direct_messages_sent(count: 200, page: i);
                        datalist.AddRange(d);
                        break;
                    }
                    else {
                        TabData tabdata;
                        lock (SettingsData.TabDataDic) { tabdata = SettingsData.TabDataDic[(string)uctldisp.Tag]; }
                        bool isBreak = false;
                        switch (tabdata.SearchType) {
                            case TabSearchType.Keyword:
                                if (i == MAX_SEARCH / 100 + 1) { isBreak = true; break; }// 1500まで
                                d = Twitter.search(q: tabdata.SearchWord, rpp: 100, page: i);
                                break;
                            case TabSearchType.User:
                                if (i == MAX_USER / 200 + 1) { isBreak = true; break; }// 3200まで
                                d = Twitter.statuses_user_timeline(screen_name: tabdata.SearchWord, count: 200, page: i);
                                break;
                            case TabSearchType.List:
                                if (i == MAX_LIST / 200 + 1) { isBreak = true; break; }// 800まで
                                d = Twitter.lists_statuses(tabdata.SearchWord, tabdata.ListOwner, per_page: 200, page: i);
                                break;
                            default:
                                Debug.Assert(false, "異常な検索タイプ");
                                isBreak = true;
                                break;
                        }
                        if (isBreak) { break; }
                    }
                    if (d.Count() == 0) { break; }
                    if (!findStart) { findStart = !useToDateTime || dtbetween(dtTo, d.Last().Time, d.First().Time); }
                    datalist.AddRange(d);
                    if (findStart && useFromDateTime && dtbetween(dtFrom, d.Last().Time, d.First().Time)) { break; }
                    i++;
                    this.Invoke(new Action(() => tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max)));
                }
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex)));
                SYSTEMSOUND.Play();
                return false;
            }
            finally {
                this.Invoke(new Action(() =>
                {
                    uctldisp.AddData(datalist.Where((td) => inbetween(td.Time)).ToArray());
                    tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);
                }));
            }
            return true;
        }
        #endregion (GetSpecifyTimeTweets)

        //===============================================================================
        #region -Update 投稿を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void Update(string text)
        {
            if (text.Length == 0 /*|| text.Length > MAX_LENGTH*/) { return; }

            try {
                this.Invoke(new Action(() =>
                {
                    this.ActiveControl = null;
                    txtTwit.Enabled = false;
                    btnTwit.Enabled = false;
                    tssLabel.Text = "投稿中...";
                }));

                switch (_stateStatusState) {
                    case StatusState.Normal:
                        Twitter.statuses_update(text);
                        break;
                    case StatusState.Reply:
                        Twitter.statuses_update(text, _statlID);
                        break;
                    case StatusState.Quote:
                        Twitter.statuses_update(text);
                        break;
                    case StatusState.DirectMessage:
                        Twitter.direct_messages_new(_statsName, _statlID, text);
                        break;
                }

                lock (_autoRenewDic) {
                    _autoRenewDic[uctlDispHome].IsForce = true;
                }
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => { tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex); }));
                SYSTEMSOUND.Play();
                return;
            }
            finally {
                this.Invoke(new Action(() =>
                {
                    txtTwit.Enabled = true;
                    btnTwit.Enabled = true;
                    txtTwit.Focus();
                }));
            }

            this.Invoke(new Action(() =>
            {
                ReSetStatusState();
                txtTwit.Text = "";
            }));
        }
        #endregion (Update)
        //-------------------------------------------------------------------------------
        #region -Delete 投稿の削除を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void Delete(long statusid)
        {
            try {
                Twitter.statuses_destroy(statusid);
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => { tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex); }));
                SYSTEMSOUND.Play();
                return;
            }

            ForAllUctlDispTwit(new Action<UctlDispTwit>((uctl) => uctl.RemoveTweet(statusid)));

            Message.ShowInfoMessage("削除しました。");
        }
        #endregion (Delete)

        //===============================================================================
        #region -CreateFavorite お気に入り登録を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// お気に入り登録を行います。
        /// </summary>
        /// <param name="id">登録する発言ID</param>
        /// <returns>成功したかどうか</returns>
        private bool CreateFavorite(long id)
        {
            try {
                Twitter.favorites_create(id);
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => { tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex); }));
                SYSTEMSOUND.Play();
                return false;
            }
            return true;
        }
        #endregion (CreateFavorite)
        //-------------------------------------------------------------------------------
        #region -DestroyFavorite お気に入り削除を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// お気に入り削除を行います。
        /// </summary>
        /// <param name="id">削除する発言ID</param>
        /// <returns>成功したかどうか</returns>
        private bool DestroyFavorite(long id)
        {
            try {
                Twitter.favorites_destroy(id);
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => { tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex); }));
                SYSTEMSOUND.Play();
                return false;
            }
            return true;
        }
        #endregion (CreateFavorite)

        //===============================================================================
        #region -GetProfile プロフィール取得 using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザープロフィールを取得します。
        /// </summary>
        /// <param name="screen_name"></param>
        /// <returns></returns>
        private UserProfile GetProfile(string screen_name)
        {
            try {
                return Twitter.users_show(screen_name: screen_name);
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() => { tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex); }));
                return null;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetProfile)

        //===============================================================================
        #region -GetQuoteString 引用の文字列を取得します。
        //-------------------------------------------------------------------------------
        //
        private string GetQuoteString(string name, string text)
        {
            switch (SettingsData.QuoteType) {
                case QuoteType.QT:
                    return string.Format("QT @{0}: {1}", name, text);
                case QuoteType.RT:
                    return string.Format("RT @{0}: {1}", name, text);
                case QuoteType.DoubleQuotation:
                    return string.Format("“@{0}: {1}”", name, text);
                default:
                    return "";
            }
        }
        #endregion (GetQuoteString)
        //-------------------------------------------------------------------------------
        #region -DefaultTabToString デフォルトで存在するタブを説明する文字列を取得します。
        //-------------------------------------------------------------------------------
        //
        private string DefaultTabToString(TabPage tabpage)
        {
            StringBuilder sb = new StringBuilder();
            int first, renew, interval;
            if (tabpage == tabpgHome) {
                sb.AppendLine("タイムライン");
                first = SettingsData.FirstGetNum_Home;
                renew = SettingsData.RenewGetNum_Home;
                interval = SettingsData.GetInterval_Home;
            }
            else if (tabpage == tabpgReply) {
                sb.AppendLine("リプライ");
                first = SettingsData.FirstGetNum_Reply;
                renew = SettingsData.RenewGetNum_Reply;
                interval = SettingsData.GetInterval_Reply;
            }
            else if (tabpage == tabpgHistory) {
                sb.AppendLine("発言履歴");
                first = SettingsData.FirstGetNum_History;
                renew = SettingsData.RenewGetNum_History;
                interval = SettingsData.GetInterval_History;
            }
            else if (tabpage == tabpgDirect) {
                sb.AppendLine("ダイレクトメッセージ");
                first = SettingsData.FirstGetNum_Direct;
                renew = SettingsData.RenewGetNum_Direct;
                interval = SettingsData.GetInterval_Direct;
            }
            /*else if (tabpage == tabpgPublic) {
                sb.AppendLine("パブリック");
                first = SettingsData.FirstGetNum_Public;
                renew = SettingsData.RenewGetNum_Public;
                interval = SettingsData.GetInterval_Public;
            }*/
            else { return ""; }

            sb.Append(STR_FIRST_GET_NUM);
            sb.Append(first.ToString());
            sb.AppendLine(STR_NUM);

            sb.Append(STR_RENEW_GET_NUM);
            sb.Append(renew.ToString());
            sb.AppendLine(STR_NUM);

            if (interval > 0) {
                sb.Append(STR_GET_INTERVAL);
                sb.Append(interval.ToString());
                sb.Append(STR_SECOND);
            }
            else {
                sb.Append(STR_NOT_AUTOGET);
            }

            return sb.ToString();
        }
        #endregion (DefaultTabToString)
        //-------------------------------------------------------------------------------
        #region -TabDataToString タブ情報を説明する文字列を取得します。
        //-------------------------------------------------------------------------------
        //
        private string TabDataToString(TabData data)
        {
            StringBuilder sb = new StringBuilder();
            switch (data.SearchType) {
                case TabSearchType.Keyword:
                    sb.Append("キーワード:");
                    break;
                case TabSearchType.User:
                    sb.Append("ユーザー:");
                    break;
                case TabSearchType.List:
                    sb.Append("リスト(オーナー:");
                    sb.Append(data.ListOwner);
                    sb.Append("):");
                    break;
            }
            sb.AppendLine(data.SearchWord);

            sb.Append(STR_FIRST_GET_NUM);
            sb.Append(data.FirstGetNum.ToString());
            sb.AppendLine(STR_NUM);

            sb.Append(STR_RENEW_GET_NUM);
            sb.Append(data.RenewGetNum.ToString());
            sb.AppendLine(STR_NUM);

            if (data.GetInterval > 0) {
                sb.Append(STR_GET_INTERVAL);
                sb.Append(data.GetInterval.ToString());
                sb.Append(STR_SECOND);
            }
            else {
                sb.Append(STR_NOT_AUTOGET);
            }

            return sb.ToString();
        }
        #endregion (TabDataToString)

        //===============================================================================
        #region -ForAllUctlDispTwit 全てのツイート表示コントロールに対して処理を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 全てのツイート表示コントロールに対して処理を行います。
        /// </summary>
        /// <param name="action">行う処理</param>
        private void ForAllUctlDispTwit(Action<UctlDispTwit> action)
        {
            foreach (TabPage tabpage in tabTwitDisp.TabPages) {
                UctlDispTwit dispTwit = _dispTwitDic[tabpage];
                action(dispTwit);
            }
        }
        #endregion (ForAllTab)

        //-------------------------------------------------------------------------------
        #region -SelectedUctlDispTwit 選択されているTabpageから選択されているUctlDispTwitを取得します。
        //-------------------------------------------------------------------------------
        //
        private UctlDispTwit SelectedUctlDispTwit()
        {
            if (tabTwitDisp.InvokeRequired) {
                return (UctlDispTwit)tabTwitDisp.Invoke(new Func<UctlDispTwit>(() => _dispTwitDic[tabTwitDisp.SelectedTab]));
            }
            else {
                return _dispTwitDic[tabTwitDisp.SelectedTab];
            }
        }
        #endregion (SelectedUctlDispTwit)

        //-------------------------------------------------------------------------------
        #region -LockAndProcess ロックを行って処理を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ロックを行って処理を行います。
        /// </summary>
        /// <param name="lockingMsg">ロック中に表示するメッセージ</param>
        /// <param name="mreThreadConfirm">スレッドが止まったということを知らせるシグナル</param>
        /// <param name="mreThreadRun">スレッドを止めるためのシグナル</param>
        /// <param name="action">行う処理</param>
        private void LockAndProcess(string lockingMsg, ManualResetEventSlim mreThreadConfirm,
                                                       ManualResetEventSlim mreThreadRun, Action action)
        {
            if (_isAuthenticated) {
                lock (_objKeyProcessStart) {
                    if (_bIsProcessing) { return; } // 重複起動抑制
                    _bIsProcessing = true;
                }

                mreThreadRun.Reset();

                while (!mreThreadConfirm.IsSet) { // スレッドが止まるまで待つ
                    tssllbl2.Text = lockingMsg;
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                tssllbl2.Text = "";
            }

            action();

            if (_isAuthenticated) {
                _bIsProcessing = false;
                mreThreadRun.Set();
            }
        }
        #endregion (LockAndProcess)
        //-------------------------------------------------------------------------------
        #region -InvokeTweetGet 発言取得を別スレッドで行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言取得を別スレッドで行います
        /// </summary>
        /// <param name="getFunc">取得関数</param>
        /// <param name="uctlDisp">取得対象コントロール</param>
        /// <param name="invokingMsg">処理中表示メッセージ</param>
        private void InvokeTweetGet(Func<UctlDispTwit, long, bool> getFunc, long standard_id, UctlDispTwit uctlDisp, string invokingMsg)
        {
            this.Invoke(new Action(() => tssLabel.Text = invokingMsg));
            Utilization.InvokeTransaction(
                () =>
                {
                    try {
                        getFunc(uctlDisp, standard_id);
                    }
                    catch (TwitterAPIException ex) {
                        this.Invoke(new Action(() => tssLabel.Text = Utilization.SubTwitterAPIExceptionStr(ex)));
                        SYSTEMSOUND.Play();
                        return;
                    }
                }
            );
            this.Invoke(new Action(() => tssLabel.Text = ""));
        }
        #endregion (InvokeTweetGet)

        //-------------------------------------------------------------------------------
        #region #[override]ProcessCmdKey キー処理
        //-------------------------------------------------------------------------------
        //
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (!txtTwit.Focused) {
                SelectedUctlDispTwit().ProcessKey(keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion (#[override]ProcessCmdKey)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)

        //===============================================================================
        #region -AutoGetTweet (別スレッド)自動ツイート取得
        //-------------------------------------------------------------------------------
        /// <summary>プロフィール更新強制</summary>
        private bool _profileRenew_IsForce = false;
        /// <summary>プロフィール更新基準時刻</summary>
        private DateTime _profileRenew_Standard;
        //
        private void AutoGetTweet()
        {
            while (!_isAuthenticated) { Thread.Sleep(1000); } // 未認証時はストップ

            try {
                while (true) {
                    // タブ使用イベントSTOP
                    _mreThreadTabConfirm.Set();
                    _mreThreadTabRun.Wait();
                    _mreThreadTabConfirm.Reset();

                    DateTime now = DateTime.Now;
                    // プロフィール更新
                    if (_profileRenew_IsForce) {
                        this.Invoke(new Action(() =>
                        {
                            UserProfile profile = GetProfile(Twitter.ScreenName);
                            if (profile != null) { SetProfileData(profile); }
                            tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);
                        }));
                        _profileRenew_IsForce = false;
                        _profileRenew_Standard = now;
                    }
                    else if (SettingsData.GetInterval_Profile != 0) {
                        TimeSpan ts = now.Subtract(_profileRenew_Standard);
                        if (ts.TotalSeconds > SettingsData.GetInterval_Profile) {
                            this.Invoke(new Action(() =>
                            {
                                UserProfile profile = GetProfile(Twitter.ScreenName);
                                if (profile != null) { SetProfileData(profile); }
                                tsslRestAPI.Text = string.Format(REST_API_FORMAT, Twitter.API_Rest, Twitter.API_Max);
                            }));
                            _profileRenew_IsForce = false;
                            _profileRenew_Standard = now;
                        }
                    }

                    foreach (TabPage tabpage in _dispTwitDic.Keys) {
                        if (!_mreThreadTabRun.IsSet) { break; } // タブ変更機能使用時にforeachから抜ける
                        // タブ未使用イベントSTOP
                        _mreThreadConfirm.Set();
                        _mreThreadRun.Wait();
                        _mreThreadConfirm.Reset();

                        now = DateTime.Now;
                        UctlDispTwit uctlDisp = _dispTwitDic[tabpage];
                        AutoRenewData renewData;
                        lock (_autoRenewDic) {
                            renewData = _autoRenewDic[uctlDisp];
                            if (!renewData.IsForce) {
                                if (renewData.Interval.Ticks == 0) { continue; }
                                TimeSpan ts = now.Subtract(renewData.Standard);
                                if (ts.CompareTo(renewData.Interval) < 0) { continue; }
                            }
                            // 更新
                            this.Invoke(new Action(() => tssLabel.Text = string.Format(GETTING_FORMAT, tabpage.Text)));
                            switch (renewData.GetTweetType) {
                                case GetTweetType.MostRecent:
                                    GetMostRecentTweets(uctlDisp);
                                    break;
                                case GetTweetType.MoreRecent:
                                    //GetMoreRecentTweets(uctlDisp);
                                    break;
                                case GetTweetType.Older:
                                    //GetOlderTweets(uctlDisp);
                                    break;
                            }
                            renewData.IsForce = false;
                            renewData.Standard = DateTime.Now;
                        }
                        this.Invoke(new Action(() => tssLabel.Text = ""));
                    }
                    Thread.Sleep(100);
                }
            }
            //catch (ObjectDisposedException) { } //とってる
            catch (InvalidOperationException) { }
        }
        #endregion (AutoGetTweet)

        //===============================================================================
        public static object _test = null;
        private void splContainer_Panel1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
