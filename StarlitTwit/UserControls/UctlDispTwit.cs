/*
 * 発言表示ユーザーコントロール
 * 
 * ●仕様
 * 
 * ●履歴
 * 2010/10/01 作成開始
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Runtime.CompilerServices;

namespace StarlitTwit
{
    /// <summary>
    /// 発言のリストを表示するコントロールです。
    /// </summary>
    public partial class UctlDispTwit : UserControl
    {
        //===============================================================================
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>選択中行</summary>
        [Browsable(false)]
        public UctlDispTwitRow SelectedRow { get; private set; }
        /// <summary>画像辞書(KeyはURL)</summary>
        [Browsable(false)]
        public ImageListWrapper ImageListWrapper { get; set; }
        /// <summary>最大発言ID</summary>
        [Browsable(false)]
        public long MaxTweetID { get; private set; }
        /// <summary>最小発言ID</summary>
        [Browsable(false)]
        public long MinTweetID { get; private set; }
        /// <summary>各発言の情報</summary>
        private SortedList<long, RowData> _rowDataList;
        /// <summary>RTのID一覧</summary>
        private SortedSet<long> _RTidSet;
        /// <summary>発言コントロールリスト</summary>
        private List<UctlDispTwitRow> _rowList = new List<UctlDispTwitRow>();
        /// <summary>少しでも見えている行数</summary>
        private int _iVisibleRowNum = 0;
        /// <summary>全てが見えている行数</summary>
        private int _iAllVisibleRowNum = 0;
        /// <summary>マウスがダウンしている間trueになる</summary>
        private bool _isMouseDowning = false;
        /// <summary>描画抑制フラグ</summary>
        private bool _bSuspendDraw = false;
        /// <summary>一番上の項目が一部見えない時にtrue</summary>
        private bool _existNotAllRow_Top = false;
        /// <summary>一番下の項目が一部見えない時にtrue</summary>
        private bool _existNotAllRow_Bottom = false;
        /// <summary>[static]long比較用クラス</summary>
        static readonly Longcomp CLONGCOMP = new Longcomp();
        /// <summary>調整時ロックオブジェクト</summary>
        private object _lockObj = new object();
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //===============================================================================
        #region 定数
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------
        #endregion (定数)

        //===============================================================================
        #region 内部データ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言1つに関する情報
        /// </summary>
        private class RowData
        {
            /// <summary>発言情報</summary>
            public TwitData TwitData;
            /// <summary>リプライツールチップの文字列</summary>
            public string StrReplyTooltip;
            /// <summary>発言の境界か</summary>
            public bool IsBoundary;
        }
        //-------------------------------------------------------------------------------
        #endregion (内部データ)

        //-------------------------------------------------------------------------------
        #region (Class)Longcomp longの比較クラス
        //-------------------------------------------------------------------------------
        //
        private class Longcomp : IComparer<long>
        {
            public int Compare(long x, long y)
            {
                return -Math.Sign(x - y);
            }
        }
        #endregion ((Class)longcomp)

        //-------------------------------------------------------------------------------
        #region MenuType 列挙体：メニュータイプ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// メニューのタイプ
        /// </summary>
        public enum MenuType
        {
            /// <summary>通常用途</summary>
            Default,
            /// <summary>一部用途が制限されたユーザー用途</summary>
            RestrictedUser,
            /// <summary>会話用途</summary>
            Conversation
        }
        //-------------------------------------------------------------------------------
        #endregion (MenuType)

        //===============================================================================
        #region プロパティ
        //-------------------------------------------------------------------------------
        #region SelectedIndex プロパティ：選択インデックス変更
        //-------------------------------------------------------------------------------
        /// <summary>選択中行インデックス</summary>
        private int _selectedIndex = -1;
        /// <summary>
        /// 選択中行インデックス
        /// </summary>
        [Browsable(false)]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            private set
            {
                Debug.Assert(value < _rowDataList.Count, "指定インデックスが異常");
                _selectedIndex = value;
                OnSelectedIndexChanged();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (SelectedIndex プロパティ)
        //-------------------------------------------------------------------------------
        #region SelectedTwitData プロパティ：選択中データのTwitDataを取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 選択中データの選択データを取得します。
        /// </summary>
        [Browsable(false)]
        public TwitData SelectedTwitData
        {
            get { return (_rowDataList.Values[SelectedIndex]).TwitData; }
        }
        #endregion (SelectedTwitData)
        //-------------------------------------------------------------------------------
        #region MenuType プロパティ：表示タイプを取得または設定
        //-------------------------------------------------------------------------------
        private MenuType _menuType = MenuType.Default;
        /// <summary>
        /// メニュータイプを取得または設定します。
        /// </summary>
        [Description("このコントロールの用途によって異なるメニュータイプを指定します。")]
        [DefaultValue(MenuType.Default)]
        public MenuType ContextMenuType
        {
            get { return _menuType; }
            set { _menuType = value; }
        }
        #endregion (MenuType)
        //-------------------------------------------------------------------------------
        #endregion (プロパティ)

        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        public UctlDispTwit()
        {
            MaxTweetID = MinTweetID = -1;

            InitializeComponent();

            _rowDataList = new SortedList<long, RowData>(CLONGCOMP);
            _RTidSet = new SortedSet<long>();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //===============================================================================
        #region イベント
        //-------------------------------------------------------------------------------
        #region Public イベント
        //-------------------------------------------------------------------------------
        /// <summary>特殊項目クリック時</summary>
        [Category("動作")]
        [Description("特殊項目クリック時")]
        public event EventHandler<TweetItemClickEventArgs> TweetItemClick;
        /// <summary>選択インデックス変更時</summary>
        [Category("動作")]
        [Description("選択インデックス変更時")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>行右クリックメニュークリック時</summary>
        [Category("ツイート")]
        [Description("行右クリックメニュークリック時")]
        public event EventHandler<TwitRowMenuEventArgs> RowContextMenu_Click;
        /// <summary>エンティティ関係メニュークリック時</summary>
        [Category("ツイート")]
        [Description("ツイート内エンティティに関するメニューイベント発生時")]
        public event EventHandler<EntityEventArgs> EntityEvent;
        //-------------------------------------------------------------------------------
        #endregion (Public イベント)
        //-------------------------------------------------------------------------------
        #region menuRow_Opening メニュー項目の設定
        //-------------------------------------------------------------------------------
        //
        private void menuRow_Opening(object sender, CancelEventArgs e)
        {
            // 表示するかどうか
            if (this.SelectedIndex == -1) { e.Cancel = true; return; }

            switch (ContextMenuType) {
                case MenuType.Default:
                    break;
                case MenuType.RestrictedUser:
                    /*tsmiDisplayUserTweet.Visible = */
                    tsmiSpecifyTime.Visible = false;
                    break;
                case MenuType.Conversation:
                    tsmiDispConversation.Visible = tsmiSepConversation.Visible = false;
                    tsmiSpecifyTime.Visible = tsmiMoreRecently.Visible = tsmiOlder.Visible = tsmiSepMoreTweet.Visible = false;
                    break;
            }

            bool isReply = (SelectedTwitData.MainTwitData.Mention_StatusID > 0);
            bool isDirect = SelectedTwitData.IsDM();
            bool isProtected = SelectedTwitData.UserProtected;
            bool isRT = SelectedTwitData.IsRT();
            bool isMine = (SelectedTwitData.UserID == FrmMain.Twitter.ID);
            bool isFavorited = SelectedTwitData.Favorited;

            tsmiReply.Enabled = !isDirect;
            tsmiQuote.Enabled = tsmiRetweet.Enabled = !isDirect && !isProtected;
            tsmiRetweet.Enabled = isRT || !(isMine || isProtected || isDirect);

            tsmiDispConversation.Enabled = isReply;

            tsmiFavorite.Enabled = !isDirect;
            tsmiFavorite.Visible = !isFavorited;
            tsmiUnfavorite.Visible = isFavorited;

            tsmiOpenBrowser_ReplyTweet.Enabled = isReply;

            tsmiDelete.Enabled = isMine;

            tsmiDispRetweeter.Enabled = !isDirect;

            tsmiMoreRecently.Visible = false; // 暫定

            // コンボボックス項目設定
            tsComboUser.Items.Clear();
            tsComboHashtag.Items.Clear();
            tsComboURL.Items.Clear();

            // 発言に関係のあるユーザーをコンボボックスに
            tsComboUser.Items.Add(SelectedTwitData.MainTwitData.UserScreenName);
            if (isRT) { tsComboUser.Items.AddAvoidDup(SelectedTwitData.UserScreenName); }
            if (isReply) { tsComboUser.Items.AddAvoidDup(SelectedTwitData.MainTwitData.Mention_ScreenName); }
            if (isDirect) { tsComboUser.Items.AddAvoidDup(SelectedTwitData.DMScreenName); }

            foreach (EntityData entity in SelectedTwitData.Entities) {
                if (entity.type.HasValue) {
                    switch (entity.type.Value) {
                        case ItemType.HashTag:
                            tsComboHashtag.Items.AddAvoidDup(entity.str);
                            break;
                        case ItemType.User:
                            tsComboUser.Items.AddAvoidDup(entity.str);
                            break;
                    }
                }
                else { // URL
                    tsComboURL.Items.AddAvoidDup(entity.str);
                }
            }
            tsComboUser.SelectedIndex = 0;
            if (tsmiHashtag.Visible = (tsComboHashtag.Items.Count > 0)) {
                tsComboHashtag.SelectedIndex = 0;
            }
            if (tsmiURL.Visible = (tsComboURL.Items.Count > 0)) {
                tsComboURL.SelectedIndex = 0;
                // DropDownの横幅設定
                int maxwidth = 0;
                foreach (object o in tsComboURL.Items) {
                    maxwidth = Math.Max(maxwidth, TextRenderer.MeasureText((string)o, tsComboURL.Font).Width);
                }
                tsComboURL.DropDownWidth = maxwidth + 20;
            }
        }
        #endregion (menuRow_Opening)
        #region ↓tsmi- メニュー項目イベント
        #region tsmiReply_Click 返信メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiReply_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Reply, SelectedTwitData));
            }
        }
        #endregion (tsmiReply_Click)
        #region tsmiQuote_Click 引用メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiQuote_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Quote, SelectedTwitData));
            }
        }
        #endregion (tsmiQuote_Click)
        #region tsmiRetweet_Click リツイートメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiRetweet_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Retweet, SelectedTwitData));
            }
        }
        #endregion (tsmiRetweet_Click)
        #region tsmiDirectMessage_Click ダイレクトメッセージメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDirectMessage_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.DirectMessage, SelectedTwitData));
            }
        }
        #endregion (tsmiDirectMessage_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispConversation_Click 会話を表示メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispConversation_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.DisplayConversation, SelectedTwitData));
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmiDispConversation_Click)
        //-------------------------------------------------------------------------------
        #region tsmiFavorite_Click お気に入りに追加メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiFavorite_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Favorite, SelectedTwitData));
            }
        }
        #endregion (tsmiFavorite_Click)
        #region tsmiUnfavorite_Click お気に入りから削除メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiUnfavorite_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Unfavorite, SelectedTwitData));
            }
        }
        #endregion (tsmiUnfavorite_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDelete_Click 削除メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Delete, SelectedTwitData));
            }
        }
        #endregion (tsmiDelete_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOpenBrowser_ThisTweet_Click ブラウザで開く-このツイートメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowser_ThisTweet_Click(object sender, EventArgs e)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append(SelectedTwitData.MainTwitData.UserScreenName);
            sbUrl.Append("/status/");
            sbUrl.Append(SelectedTwitData.MainTwitData.StatusID);

            Utilization.OpenBrowser(sbUrl.ToString(), FrmMain.SettingsData.UseInternalWebBrowser);
        }
        #endregion (tsmiOpenBrowser_ThisTweet_Click)
        #region tsmiOpenBrowser_ReplyTweet_Click ブラウザで開く-リプライ先メニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowser_ReplyTweet_Click(object sender, EventArgs e)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append(SelectedTwitData.MainTwitData.Mention_ScreenName);
            sbUrl.Append("/status/");
            sbUrl.Append(SelectedTwitData.MainTwitData.Mention_StatusID);

            Utilization.OpenBrowser(sbUrl.ToString(), FrmMain.SettingsData.UseInternalWebBrowser);
        }
        #endregion (tsmiOpenBrowser_ReplyTweet_Click)
        //-------------------------------------------------------------------------------
        #region tsmiUser_DisplayProfile_Click ユーザー：プロフィール表示クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_DisplayProfile_Click(object sender, EventArgs e)
        {
            if (EntityEvent != null) {
                EntityEvent(this, new EntityEventArgs(EntityEventType.User_DisplayProfile, (string)tsComboUser.SelectedItem));
            }
        }
        #endregion (tsmiUser_DisplayProfile_Click)
        #region tsmiUser_DisplayTweets_Click ユーザー：発言を表示クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_DisplayTweets_Click(object sender, EventArgs e)
        {
            if (EntityEvent != null) {
                EntityEvent(this, new EntityEventArgs(EntityEventType.User_DisplayTweets, (string)tsComboUser.SelectedItem));
            }
        }
        #endregion (tsmiUser_DisplayTweets_Click)
        #region tsmiUser_MakeUserTab_Click ユーザー：タブ作成クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_MakeUserTab_Click(object sender, EventArgs e)
        {
            if (EntityEvent != null) {
                EntityEvent(this, new EntityEventArgs(EntityEventType.User_MakeUserTab, (string)tsComboUser.SelectedItem));
            }
        }
        #endregion (tsmiUser_MakeUserTab_Click)
        #region tsmiUser_MakeListTab_Click ユーザー：リストタブ作成クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_MakeListTab_Click(object sender, EventArgs e)
        {
            if (EntityEvent != null) {
                EntityEvent(this, new EntityEventArgs(EntityEventType.User_MakeListTab, (string)tsComboUser.SelectedItem));
            }
        }
        #endregion (tsmiUser_MakeListTab_Click)
        #region tsmiUser_OpenBrowser_Click ユーザー：ブラウザで開くクリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_OpenBrowser_Click(object sender, EventArgs e)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append((string)tsComboUser.SelectedItem);

            Utilization.OpenBrowser(sbUrl.ToString(), FrmMain.SettingsData.UseInternalWebBrowser);
        }
        #endregion (tsmiUser_OpenBrowser_Click)
        #region tsmiUser_Clipboard_Click ユーザー：クリップボードにコピークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiUser_Clipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((string)tsComboUser.SelectedItem);
        }
        #endregion (tsmiUser_Clipboard_Click)
        //-------------------------------------------------------------------------------
        #region tsmiHashtag_MakeTab_Click ハッシュタグ：タブ作成クリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiHashtag_MakeTab_Click(object sender, EventArgs e)
        {
            if (EntityEvent != null) {
                EntityEvent(this, new EntityEventArgs(EntityEventType.Hashtag_MakeTab, (string)tsComboHashtag.SelectedItem));
            }
        }
        #endregion (tsmiHashtag_MakeTab_Click)
        #region tsmiHashtag_Clipboard_Click ハッシュタグ：クリップボードにコピークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiHashtag_Clipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((string)tsComboHashtag.SelectedItem);
        }
        #endregion (tsmiHashtag_Clipboard_Click)
        //-------------------------------------------------------------------------------
        #region tsmiURL_OpenExternalBrowser_Click URL：外部ブラウザで開くクリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiURL_OpenExternalBrowser_Click(object sender, EventArgs e)
        {
            string url = (string)tsComboURL.SelectedItem;
            Utilization.OpenBrowser(url, false);
        }
        #endregion (tsmiURL_OpenExternalBrowser_Click)
        #region tsmiURL_OpenInternalBrowser_Click URL：内部ブラウザで開くクリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiURL_OpenInternalBrowser_Click(object sender, EventArgs e)
        {
            string url = (string)tsComboURL.SelectedItem;
            Utilization.OpenBrowser(url, true);
        }
        #endregion (tsmiURL_OpenInternalBrowser_Click)
        #region tsmiURL_Clipboard_Click URL：クリップボードにコピークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiURL_Clipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((string)tsComboURL.SelectedItem);
        }
        #endregion (tsmiURL_Clipboard_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispRetweeter_Click Retweetしたユーザーを見るクリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispRetweeter_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.Retweeter, SelectedTwitData));
            }
        }
        #endregion (tsmiDispRetweeter_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOlderData_Click より古いデータメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiOlderData_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.OlderTweetRequest, SelectedTwitData));
            }
        }
        #endregion (tsmiOlderData_Click)
        #region tsmiMoreRecentData_Click より新しいデータメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiMoreRecentData_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.MoreRecentTweetRequest, SelectedTwitData));
            }
        }
        #endregion (tsmiMoreRecentData_Click)
        #region tsmiSpecifyTime_Click 時刻を指定して発言取得
        //-------------------------------------------------------------------------------
        //
        private void tsmiSpecifyTime_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.SpecifyTimeTweetRequest, SelectedTwitData));
            }
        }
        #endregion (tsmiSpecifyTime_Click)
        #endregion (tsmi-)
        #region tsmiEntityItem_MouseMove EntityItemマウス移動時
        //-------------------------------------------------------------------------------
        /// <remarks>こうしないとコンボボックス選択後にハイライトされない</remarks>
        private void tsmiEntityItem_MouseMove(object sender, MouseEventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            tsmi.Select();
        }
        #endregion (tsmiEntityItem_MouseMove)
        //===============================================================================
        #region pnlflow_MouseDown マウスダウン時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseDown(object sender, MouseEventArgs e)
        {
            UctlDispTwitRow row = pnlTweets.GetChildAtPoint(e.Location, GetChildAtPointSkip.Invisible) as UctlDispTwitRow;
            lock (_lockObj) {
                if (row == null) { SelectedIndex = -1; }
                if (row != SelectedRow) { ChangeSelectRow(row); }
            }
            _isMouseDowning = true;
        }
        #endregion (UctlDispTwit_MouseDown)
        #region pnlflow_MouseMove マウスムーブ時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDowning) {
                //UctlDispTwitRow row = pnlflow.GetChildAtPoint(e.Location,GetChildAtPointSkip.Invisible) as UctlDispTwitRow;
                //if (row != SelectedRow) { ChangeSelectRow(row); }
            }
        }
        #endregion (pnlflow_MouseMove)
        #region pnlflow_MouseUp マウスアップ時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDowning = false;
        }
        #endregion (pnlflow_MouseUp)
        #region pnlflow_MouseClick マウスクリック時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseClick(object sender, MouseEventArgs e)
        {

        }
        #endregion (pnlflow_MouseClick)
        //-------------------------------------------------------------------------------
        #region ProcessKey キー処理
        //-------------------------------------------------------------------------------
        private bool _enableKey = true;
        //
        public void ProcessKey(Keys key)
        {
            if (_iVisibleRowNum == 0 || !_enableKey) { return; }

            switch (key) {
                case Keys.Down:
                case Keys.Up:
                    lock (_lockObj) {
                        if (SelectedIndex < 0) {
                            // 一番上選択
                            SelectedIndex = GetAbsoluteRowIntex(0);
                        }
                        else {
                            int iselected = SelectedIndex;
                            if (key == Keys.Up) {
                                // ↑キー
                                if (iselected == 0) { break; }
                                SelectedIndex--;
                            }
                            else {
                                // ↓キー
                                if (iselected == _rowDataList.Count - 1) { break; }
                                SelectedIndex++;
                            }
                        }
                        ChangeSelectRow(SelectedIndex);
                        if (SelectedIndex < vscrbar.Value) { // 選択が上にありすぎる
                            int diff = vscrbar.Value - SelectedIndex;
                            _suspend_vscrbar_ValueChangeEvent = true;
                            vscrbar.Value -= diff;
                            _suspend_vscrbar_ValueChangeEvent = false;
                            AdjustControl(vscrbar.Value, true, diff);
                            pnlTweets.Refresh(); // 押しっぱなしで描画が追いつかないため
                        }
                        else if (SelectedIndex >= vscrbar.Value + _iAllVisibleRowNum) { // 選択が下にありすぎる
                            int diff = vscrbar.Value + _iAllVisibleRowNum - SelectedIndex - 1;
                            _suspend_vscrbar_ValueChangeEvent = true;
                            vscrbar.Value -= diff;
                            _suspend_vscrbar_ValueChangeEvent = false;
                            AdjustControl(SelectedIndex, false, diff);
                            pnlTweets.Refresh(); // 押しっぱなしで描画が追いつかないため
                        }
                    }
                    break;
                case Keys.PageUp:
                    lock (_lockObj) {
                        if (vscrbar.Enabled) {
                            SelectedIndex = Math.Max(SelectedIndex - vscrbar.LargeChange, 0);
                            vscrbar.Value = Math.Max(vscrbar.Value - vscrbar.LargeChange, 0);
                        }
                        else {
                            vscrbar.Value = SelectedIndex = 0;
                        }
                    }
                    break;
                case Keys.PageDown:
                    lock (_lockObj) {
                        if (vscrbar.Enabled) {
                            SelectedIndex = Math.Min(SelectedIndex + vscrbar.LargeChange, _rowDataList.Count - 1);
                            vscrbar.Value = Math.Min(vscrbar.Value + vscrbar.LargeChange, _rowDataList.Count - 1);
                        }
                        else {
                            vscrbar.Value = SelectedIndex = _rowDataList.Count - 1;
                        }
                    }
                    break;
                case Keys.Home:
                    lock (_lockObj) { vscrbar.Value = SelectedIndex = 0; }
                    break;
                case Keys.End:
                    lock (_lockObj) { vscrbar.Value = SelectedIndex = _rowDataList.Count - 1; }
                    break;
            }
        }
        #endregion (ProcessKey)
        //-------------------------------------------------------------------------------
        #region UctlDispTwit_ClientSizeChanged クライアントサイズ変更
        //-------------------------------------------------------------------------------
        private void UctlDispTwit_ClientSizeChanged(object sender, EventArgs e)
        {
            lock (_lockObj) {
                AdjustControl(vscrbar.Value, true);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (UctlDispTwit_ClientSizeChanged クライアントサイズ変更)
        //-------------------------------------------------------------------------------
        #region Row_TweetItemClick 特殊項目クリック時
        //-------------------------------------------------------------------------------
        //
        private void Row_TweetItemClick(object sender, TweetItemClickEventArgs e)
        {
            if (TweetItemClick != null) {
                TweetItemClick.Invoke(sender, e);
            }
        }
        #endregion (Row_TweetItemClick)
        //-------------------------------------------------------------------------------
        #region Row_TextBoxEnter テキストボックスEnter時
        //-------------------------------------------------------------------------------
        //
        private void Row_TextBoxEnter(object sender, EventArgs e)
        {
            _enableKey = false;
        }
        #endregion (Row_TextBoxEnter)
        #region Row_TextBoxLeave テキストボックスLeave時
        //-------------------------------------------------------------------------------
        //
        private void Row_TextBoxLeave(object sender, EventArgs e)
        {
            _enableKey = true;
        }
        #endregion (Row_TextBoxLeave)
        //-------------------------------------------------------------------------------
        #region pnlflow_MouseWheel マウスホイール時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (vscrbar.Enabled) {
                lock (_lockObj) {
                    int moveval = e.Delta / 120;
                    if (moveval > 0) {
                        vscrbar.Value = Math.Max(vscrbar.Minimum, vscrbar.Value - moveval);
                    }
                    else if (moveval < 0) {
                        if (vscrbar.Value + vscrbar.LargeChange - 1 == vscrbar.Maximum) { return; }
                        vscrbar.Value = Math.Min(vscrbar.Maximum - vscrbar.LargeChange + 1, vscrbar.Value - moveval);
                    }
                }
            }
        }
        #endregion (pnlflow_MouseWheel)
        //-------------------------------------------------------------------------------
        #region vscrbar_Scroll スクロールバースクロール時
        //-------------------------------------------------------------------------------
        //
        private void vscrbar_Scroll(object sender, ScrollEventArgs e)
        {
            lock (_lockObj) {
                if (e.NewValue == e.OldValue) { return; }
                vscrbar.Value = e.NewValue;
            }
        }
        #endregion (vscrbar_Scroll)
        //-------------------------------------------------------------------------------
        #region vscrbar_ValueChanged 値変化時
        //-------------------------------------------------------------------------------
        private bool _suspend_vscrbar_ValueChangeEvent = false;
        private int _prevValue = 0;
        //
        private void vscrbar_ValueChanged(object sender, EventArgs e)
        {
            lock (_lockObj) {
                try {
                    if (_suspend_vscrbar_ValueChangeEvent || _prevValue == vscrbar.Value) { return; }

                    AdjustControl(vscrbar.Value, true, _prevValue - vscrbar.Value);
                    pnlTweets.Refresh();
                }
                finally {
                    _prevValue = vscrbar.Value;
                }
            }
        }
        #endregion (vscrbar_ValueChanged)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //===============================================================================
        #region メソッド
        //-------------------------------------------------------------------------------
        #region +AddData 発言データを追加します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言データを追加します。
        /// </summary>
        /// <param name="data">発言データ配列。StatusID降順推奨。</param>
        /// <param name="suspendSetBoundary">[option]境界セットを抑制する時true</param>
        /// <param name="checkRetweetDup">[option]RTのための重複確認をするかどうか</param>
        /// <returns>最初のツイートのデータ</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string AddData(IEnumerable<TwitData> data, bool suspendSetBoundary = false, bool checkRetweetDup = false)
        {
            lock (_lockObj) {
                string retText = "";

                // 選択保存用
                long selectedData = (SelectedIndex >= 0 && SelectedIndex < _rowDataList.Count) ?
                                       _rowDataList.Values[SelectedIndex].TwitData.StatusID : -1;
                ChangeSelectRow(null);
                // 位置保存用
                long locIndex = (vscrbar.Enabled && vscrbar.Value > 0) ?
                                (_existNotAllRow_Top) ? _rowList[_iVisibleRowNum - 1].TwitData.StatusID :
                                                        _rowList[0].TwitData.StatusID : -1;

                List<string> imageURLList = new List<string>();
                //-----内部情報設定-----
                List<RowData> addrowList = new List<RowData>();
                TwitData lastdata = data.LastOrDefault();
                foreach (TwitData t in data) {
                    // 重複排除
                    if (_rowDataList.ContainsKey(t.StatusID) || (checkRetweetDup && CheckRTDup(t))) {
                        if (!suspendSetBoundary && _rowDataList[t.StatusID].IsBoundary) { _rowDataList[t.StatusID].IsBoundary = (t.StatusID == lastdata.StatusID); }
                        continue;
                    }

                    RowData rowdata = new RowData() {
                        TwitData = t,
                        IsBoundary = (!suspendSetBoundary && t.StatusID == lastdata.StatusID)
                    };

                    // 返り値用
                    if (string.IsNullOrEmpty(retText)) { retText = Utilization.MakePopupText(t); }

                    // 画像URL登録
                    string iconURL = t.IsRT() ? t.RTTwitData.IconURL : t.IconURL;
                    if (!ImageListWrapper.ImageContainsKey(iconURL) && !imageURLList.Contains(iconURL)) {
                        imageURLList.Add(iconURL);
                    }

                    addrowList.Add(rowdata);
                    _rowDataList.Add(t.StatusID, rowdata);
                    if (t.IsRT()) { _RTidSet.Add(t.RTTwitData.StatusID); } // RT元データを集合に追加

                    //Console.WriteLine(t.StatusID);

                    // 最小・最大ID更新
                    if (MaxTweetID == -1) { MaxTweetID = MinTweetID = t.StatusID; } // 最初
                    else {
                        MaxTweetID = Math.Max(MaxTweetID, t.StatusID);
                        MinTweetID = Math.Min(MinTweetID, t.StatusID);
                    }
                }

                // 追加分Replyツールチップ設定
                SetRowReplyText(addrowList);

                // アイコン取得要請
                ImageListWrapper.RequestAddImages(imageURLList);

                //-----コントロール設定-----
                // 選択復元
                if (selectedData >= 0) {
                    int newIndex = _rowDataList.IndexOfKey(selectedData);
                    SelectedIndex = newIndex;
                    ChangeSelectRow(newIndex);
                }

                vscrbar.Maximum = _rowDataList.Count - 1;

                if (locIndex >= 0) {
                    // 位置復元
                    int baseIndex = _rowDataList.IndexOfKey(locIndex);
                    if (_existNotAllRow_Top) {
                        AdjustControl(baseIndex, false);
                    }
                    else {
                        _suspend_vscrbar_ValueChangeEvent = true;
                        vscrbar.Value = baseIndex;
                        _suspend_vscrbar_ValueChangeEvent = false;
                        AdjustControl(vscrbar.Value, true); // コントロール位置調整
                    }
                }
                else { AdjustControl(vscrbar.Value, true); } // コントロール位置調整


                return retText;
            }
        }
        #endregion (AddData)
        //-------------------------------------------------------------------------------
        #region +ReConfigAll 全データを再設定します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 全データを再設定します。
        /// </summary>
        public void ReConfigAll()
        {
            lock (_lockObj) { AdjustControl(vscrbar.Value, true); }
        }
        //-------------------------------------------------------------------------------
        #endregion (ReConfigAll)
        //-------------------------------------------------------------------------------
        #region +ClearAll 全行をクリアします。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 全行のクリア
        /// </summary>
        public void ClearAll()
        {
            lock (_lockObj) {
                MaxTweetID = MinTweetID = SelectedIndex = -1;
                SelectedRow = null;

                _rowDataList.Clear();
                _rowList.ForEach((row) => row.Visible = false);
                vscrbar.Value = vscrbar.Minimum;
                vscrbar.Enabled = false;
            }
        }
        #endregion (ClearAll)
        //-------------------------------------------------------------------------------
        #region +RemoveTweet 発言を消去します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言があれば消去を行います。
        /// </summary>
        /// <param name="statusid">消去する発言のID</param>
        /// <returns>消去したかどうか（発言があったかどうか）</returns>
        public bool RemoveTweet(long statusid)
        {
            if (!_rowDataList.ContainsKey(statusid)) { return false; }

            lock (_lockObj) {
                _rowDataList.Remove(statusid);
                if (SelectedIndex == _rowDataList.Keys.IndexOf(statusid)) { SelectedIndex = -1; }
                AdjustControl(vscrbar.Value, true);
            }
            return true;
        }
        #endregion (RemoveTweet)
        //-------------------------------------------------------------------------------
        #region +SetAllRowReplyText 全行リプライ先テキストを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 全ての行のリプライ先テキストを設定します。
        /// </summary>
        /// <param name="renew">falseだと表示はすぐには更新されません。</param>
        public void SetAllRowReplyText(bool renew)
        {
            SetRowReplyText(_rowDataList.Values);
            if (renew) {
                foreach (var row in _rowList) {
                    row.SetReplyToolTip(_rowDataList[row.TwitData.StatusID].StrReplyTooltip);
                }
            }
        }
        #endregion (SetAllRowReplyText)
        //-------------------------------------------------------------------------------
        #region +TraceReply リプライをトレース
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 指定発言IDからリプライをトレースしていきます。
        /// </summary>
        /// <param name="startStatusID">開始発言ID</param>
        /// <returns></returns>
        public IEnumerable<TwitData> TraceReply(long startStatusID)
        {
            long statusid = startStatusID;
            do {
                if (!_rowDataList.ContainsKey(statusid)) { break; }
                TwitData data = _rowDataList[statusid].TwitData.MainTwitData;
                yield return data;
                statusid = data.Mention_StatusID;
            } while (statusid != -1);
        }
        #endregion (TraceReply)
        //===============================================================================
        #region -AdjustControl コントロールの内容・位置・サイズの調整を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コントロールの内容・位置・サイズの調整を行います。
        /// </summary>
        /// <param name="startIndex">一番上(flowDirForward=true)もしくは一番下(flowDirForward=false)のインデックス</param>
        /// <param name="flowDirForward">上(true)と下(false)のどちらから詰めていくか</param>
        /// <param name="shiftvalue">上向き正とするシフト量。0の時は無効</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AdjustControl(int startIndex, bool flowDirForward, int shiftvalue = 0)
        {
            //Console.WriteLine("AdjustControl start");
            //DateTime dt = DateTime.Now;

            if (_rowDataList.Count == 0) { return; }

            // 描画・レイアウト抑制
            SuspendPaint();
            pnlTweets.SuspendLayout();

            // 選択中行発言ID取得
            long selectedStatusID = (SelectedIndex == -1) ? -1 : _rowDataList.Values[SelectedIndex].TwitData.StatusID;
            SelectedRow = null;

            // 全カラムの位置・幅・高さ調整
            int maxHeight = pnlTweets.ClientSize.Height;
            bool needScrollbar = true;
            bool isForward = flowDirForward;
            bool existNotAllVisibleRow = false;
            int rowindex = 0;
            int rowdataindex = startIndex;
            int height = (flowDirForward) ? 0 : maxHeight;

            // 1個ずらしの場合
            if (shiftvalue != 0 && Math.Abs(shiftvalue) < _iVisibleRowNum) {
                if (shiftvalue < 0) {
                    int shiftnum = -shiftvalue;
                    if (flowDirForward) { // シフトするのみ
                        startIndex -= (_existNotAllRow_Top) ? 1 : 0;
                        for (int i = shiftnum; i < _iVisibleRowNum; i++) { // シフト
                            UctlDispTwitRow row = _rowList[i];
                            row.Invalidate();
                            row.Location = new Point(0, height);
                            ChangeSelectRow(row, selectedStatusID);
                            height += row.Height;
                            existNotAllVisibleRow = (height > maxHeight);
                        }
                        for (int i = 0; i < shiftnum; i++) { // ずらす
                            UctlDispTwitRow exRow = _rowList[0];
                            _rowList.RemoveAt(0);
                            _rowList.Insert(_iVisibleRowNum - 1, exRow);
                        }
                        rowindex = _iVisibleRowNum - shiftnum;
                        rowdataindex = startIndex + rowindex;
                    }
                    else { // flowDirForward = false
                        UctlDispTwitRow exRow = null;
                        shiftnum -= (_existNotAllRow_Bottom) ? 1 : 0;
                        for (int i = 0; i < shiftnum; i++) { // 新しく見える部分
                            if (height <= 0) { break; }
                            exRow = _rowList[i];
                            RowData rowdata = _rowDataList.Values[rowdataindex];
                            exRow.TwitData = rowdata.TwitData;
                            exRow.Visible = true;
                            ConfigRow(exRow, rowdata.StrReplyTooltip, rowdata.IsBoundary, selectedStatusID); // 行設定
                            exRow.Location = new Point(0, height - exRow.Height);
                            height -= exRow.Height;
                            existNotAllVisibleRow = (height < 0);

                            _rowList.RemoveAt(i);
                            _rowList.Insert(0, exRow);
                            rowindex++;
                            rowdataindex--;
                        }
                        for (int i = _iVisibleRowNum - 1; i >= shiftnum; i--) { // シフト
                            if (height <= 0) { break; }
                            UctlDispTwitRow row = _rowList[_iVisibleRowNum - 1];
                            row.Invalidate();
                            row.Location = new Point(0, height - row.Height);
                            ChangeSelectRow(row, selectedStatusID);
                            height -= row.Height;
                            existNotAllVisibleRow = (height < 0);

                            _rowList.RemoveAt(_iVisibleRowNum - 1);
                            _rowList.Insert(0, row);
                            rowindex++;
                        }
                        rowdataindex = startIndex - rowindex;
                    }
                }
                else { // if shiftvalue > 0
                    int shiftnum = shiftvalue;
                    if (shiftnum < _iVisibleRowNum) {
                        if (flowDirForward) {
                            UctlDispTwitRow exRow = null;
                            shiftnum -= (_existNotAllRow_Top) ? 1 : 0;
                            bool needMove = _existNotAllRow_Top;
                            for (int i = 0; i < shiftnum; i++) { // 新しい部分
                                if (height >= maxHeight) { break; }
                                exRow = _rowList[_iVisibleRowNum - 1];
                                RowData rowdata = _rowDataList.Values[rowdataindex];
                                exRow.TwitData = rowdata.TwitData;
                                exRow.Visible = true;
                                ConfigRow(exRow, rowdata.StrReplyTooltip, rowdata.IsBoundary, selectedStatusID); // 行設定
                                exRow.Location = new Point(0, height);
                                height += exRow.Height;
                                existNotAllVisibleRow = (height > maxHeight);

                                _rowList.RemoveAt(_iVisibleRowNum - 1);
                                _rowList.Insert(i, exRow);
                                rowindex++;
                                rowdataindex++;
                            }
                            for (int i = shiftnum; i < _iVisibleRowNum; i++) { // シフト
                                if (height >= maxHeight) { break; }
                                UctlDispTwitRow row = _rowList[i];
                                row.Invalidate();
                                row.Location = new Point(0, height);
                                ChangeSelectRow(row, selectedStatusID);
                                height += row.Height;
                                existNotAllVisibleRow = (height > maxHeight);
                                rowindex++;
                            }
                            rowdataindex = startIndex + rowindex;
                        }
                        else { // flowDirForward = false
                            Debug.Assert(false, "未実装(不要), 現状実装しない");
                        }
                    }
                }
            }

            while ((isForward && height < maxHeight) || (!isForward && height > 0)) {
                if (_rowDataList.Count <= rowdataindex) {// isForward=trueのみ．一番下までいった
                    if (startIndex == 0) { needScrollbar = false; break; } // 数が少なくてスクロールバーもいらないような時
                    isForward = false;
                    rowdataindex = startIndex - 1;
                    // 下詰めに
                    int dif = maxHeight - height;
                    for (int i = 0; i < rowindex; i++) {
                        _rowList[i].Location = new Point(0, _rowList[i].Location.Y + dif);
                    }
                    height = dif;
                    continue;
                }
                else if (rowdataindex == -1) { break; } // isForward=falseのみ．全て見終わった

                RowData rowdata = _rowDataList.Values[rowdataindex];
                UctlDispTwitRow row;
                if (_rowList.Count <= rowindex) {
                    // 行作成
                    row = MakeTwitRow(rowdata.TwitData);
                    _rowList.Add(row);
                    pnlTweets.Controls.Add(row);
                }
                else {
                    row = _rowList[rowindex];
                    row.TwitData = rowdata.TwitData;
                    row.Visible = true;
                }

                ConfigRow(row, rowdata.StrReplyTooltip, rowdata.IsBoundary, selectedStatusID); // 行設定

                if (isForward) {
                    row.Location = new Point(0, height);
                    height += row.Height;
                    existNotAllVisibleRow = (height > maxHeight);

                    rowdataindex++;
                }
                else {
                    row.Location = new Point(0, height - row.Height);
                    height -= row.Height;
                    existNotAllVisibleRow = (height < 0);
                    // 現在行を先頭にして移動
                    _rowList.Remove(row);
                    _rowList.Insert(0, row);

                    rowdataindex--;
                }
                rowindex++;
            }


            // 描画・レイアウト再開
            pnlTweets.ResumeLayout(false);
            pnlTweets.PerformLayout();
            ResumePaint();

            _iVisibleRowNum = rowindex;
            _iAllVisibleRowNum = _iVisibleRowNum - ((existNotAllVisibleRow) ? 1 : 0);
            _existNotAllRow_Top = !isForward && existNotAllVisibleRow;
            _existNotAllRow_Bottom = isForward && existNotAllVisibleRow;

            if (_rowList.Count > _iVisibleRowNum) {
                // 不要な行のVisibleをfalseに
                for (int i = _iVisibleRowNum; i < _rowList.Count; i++) {
                    _rowList[i].Visible = false;
                }
            }

            // スクロールバー設定
            _suspend_vscrbar_ValueChangeEvent = true;
            if (vscrbar.Enabled = needScrollbar) {
                vscrbar.LargeChange = Math.Max(0, _iAllVisibleRowNum);
                vscrbar.Maximum = _rowDataList.Count - 1;

                if (isForward) { vscrbar.Value = startIndex; }
                else { vscrbar.Value = rowdataindex + 2; }
            }
            _suspend_vscrbar_ValueChangeEvent = false;

            //Console.WriteLine("AdjustControl end");

            //Console.WriteLine(DateTime.Now.Subtract(dt));
        }
        //-------------------------------------------------------------------------------
        #endregion (AdjustControl)
        //-------------------------------------------------------------------------------
        #region -MakeTwitRow 行を作成しイベントを登録して返します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 行を作成しイベントを登録して返します。
        /// </summary>
        /// <param name="twitdata">行の発言データ</param>
        /// <returns></returns>
        private UctlDispTwitRow MakeTwitRow(TwitData twitdata)
        {
            UctlDispTwitRow row = new UctlDispTwitRow(twitdata);
            row.ImageListWrapper = ImageListWrapper;
            row.MouseDown += pnlflow_MouseDown;
            row.MouseUp += pnlflow_MouseUp;
            row.MouseMove += pnlflow_MouseMove;
            row.MouseClick += pnlflow_MouseClick;
            row.TweetItemClick += Row_TweetItemClick;
            row.TextBoxEnter += Row_TextBoxEnter;
            row.TextBoxLeave += Row_TextBoxLeave;
            return row;
        }
        #endregion (MakeTwitRow)
        //-------------------------------------------------------------------------------
        #region -ConfigRow 行に対して一連の設定を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 行に対して一連の設定を行います。
        /// </summary>
        /// <param name="row">設定を行う対象の行</param>
        /// <param name="tooltipStr">ToolTip文字列</param>
        /// <pparam name="selectedStatusID">選択されている項目のStatusID</pparam>
        private void ConfigRow(UctlDispTwitRow row, string tooltipStr, bool isBoundary, long selectedStatusID)
        {
            // 選択中か判別
            if (selectedStatusID >= 0) {
                if (selectedStatusID == row.TwitData.StatusID) {
                    row.SelectControl();
                    SelectedRow = row;
                }
                else { row.UnSelectControl(); }
            }

            row.SetBackColor();
            row.Invalidate();

            row.SetLineColor(!isBoundary);

            row.SetReplyToolTip(tooltipStr);
            row.ResetPicturePopup();

            row.SuspendAdjust = true;
            row.SetWidth(pnlTweets.ClientSize.Width);
            row.SetIconConfig();
            row.SetFontConfig();
            row.SetNameLabel();
            row.SetTweetLabel();
            row.SetIcon();
            row.SuspendAdjust = false;
            row.SetControlLocation();
        }
        #endregion (ConfigRow)
        //-------------------------------------------------------------------------------
        #region -CheckRTDup Retweetの重複を調べます。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// Retweetの重複を調べます。
        /// </summary>
        /// <param name="twitdata"></param>
        /// <returns></returns>
        private bool CheckRTDup(TwitData twitdata)
        {
            if (twitdata.IsRT()) {
                long id = twitdata.RTTwitData.StatusID;
                return _rowDataList.ContainsKey(id) || _RTidSet.Contains(id);
            }
            return false;
        }
        #endregion (CheckRTDup)
        //-------------------------------------------------------------------------------
        #region -SetRowReplyText 行リプライ先テキストを表示します。
        //-------------------------------------------------------------------------------
        //
        private void SetRowReplyText(IEnumerable<RowData> rowdata)
        {
            foreach (RowData row in rowdata) {
                if (FrmMain.SettingsData.DisplayReplyToolTip && row.TwitData.Mention_StatusID > 0) {
                    int depth;
                    if ((depth = FrmMain.SettingsData.DisplayReplyToolTipDepth) == 0) { depth = int.MaxValue; }
                    string replytext = GetReplyText(row.TwitData.Mention_StatusID, depth);
                    row.StrReplyTooltip = replytext;
                }
                else { row.StrReplyTooltip = ""; }
            }
        }
        #endregion (SetRowReplyText)
        //-------------------------------------------------------------------------------
        #region -GetReplyText リプライ先のテキストを取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リプライ先のテキストを取得します。
        /// </summary>
        /// <param name="reply_status_id">リプライ先ID</param>
        /// <param name="ctlList">まだ追加されていない行のリスト</param>
        /// <returns></returns>
        private string GetReplyText(long reply_status_id, int depth)
        {
            if (depth <= 0) { return "..."; }

            // 既に追加さてれいる行内
            if (_rowDataList.ContainsKey(reply_status_id)) {
                string deeper = "";
                TwitData tdata = _rowDataList[reply_status_id].TwitData;
                if (tdata.Mention_StatusID > 0) {
                    deeper = GetReplyText(tdata.Mention_StatusID, depth - 1);
                }
                if (!string.IsNullOrEmpty(deeper)) {
                    return string.Join("\n", Utilization.InterpretFormat(tdata), tdata.Text) + "\n\n" + deeper;
                }
                else {
                    return string.Join("\n", Utilization.InterpretFormat(tdata), tdata.Text);
                }
            }
            return "...";
        }
        #endregion (GetReplyText)
        //-------------------------------------------------------------------------------
        #region -ChangeSelectRow 選択行変更
        //-------------------------------------------------------------------------------
        #region (UctlDispTwitRow)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 指定された行を選択します．
        /// </summary>
        /// <param name="row"></param>
        private void ChangeSelectRow(UctlDispTwitRow row)
        {
            if (SelectedRow != null) {
                SelectedRow.UnSelectControl();
                SelectedRow = null;
            }

            if (row != null) {
                SelectedRow = row;
                SelectedIndex = GetAbsoluteRowIntex(_rowList.IndexOf(row));
                SelectedRow.SelectControl();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((UctlDispTwitRow))
        #region (int)
        /// <summary>
        /// 指定したインデックスを持つ行があれば選択します。
        /// </summary>
        /// <param name="absIndex"></param>
        private void ChangeSelectRow(int absIndex)
        {
            UctlDispTwitRow row = GetRowFromIndex(absIndex);
            ChangeSelectRow(row);
        }
        //-------------------------------------------------------------------------------
        #endregion ((int))
        #region (UctlDispTwitRow, long)
        //-------------------------------------------------------------------------------
        //
        private void ChangeSelectRow(UctlDispTwitRow row, long selectedStatusID)
        {
            if (row.TwitData != null && row.TwitData.StatusID == selectedStatusID) {
                row.SelectControl();
                SelectedRow = row;
            }
            else { row.UnSelectControl(); }
        }
        //-------------------------------------------------------------------------------
        #endregion (UctlDispTwitRow, long)
        #endregion (ChangeSelectRow)
        //-------------------------------------------------------------------------------
        #region -OnSelectedIndexChanged 選択インデックス変更時
        //-------------------------------------------------------------------------------
        //
        private void OnSelectedIndexChanged()
        {
            if (SelectedIndexChanged != null) { SelectedIndexChanged.Invoke(this, EventArgs.Empty); }
        }
        #endregion (OnSelectedIndexChanged)
        //===============================================================================
        #region -GetRowFromIndex 絶対インデックスから行を取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 絶対インデックスから行を取得します。見えてない部分に該当する場合はnullが返ります。
        /// </summary>
        /// <returns></returns>
        private UctlDispTwitRow GetRowFromIndex(int absIndex)
        {
            int lookIndex = GetLookRowIndex(absIndex);
            return (lookIndex >= 0 && lookIndex < _iVisibleRowNum)
                    ? _rowList[lookIndex]
                    : null;
        }
        #endregion (GetRowFromIndex)
        //-------------------------------------------------------------------------------
        #region -GetLookRowIndex 見た目の行インデックスを取得
        //-------------------------------------------------------------------------------
        //
        private int GetLookRowIndex(int absRowIndex)
        {
            return absRowIndex - vscrbar.Value + ((_existNotAllRow_Top) ? 1 : 0);
        }
        #endregion (GetLookRowIndex)
        #region -GetAbsoluteRowIntex 絶対行インデックスを取得
        //-------------------------------------------------------------------------------
        //
        private int GetAbsoluteRowIntex(int lookRowIndex)
        {
            return lookRowIndex + vscrbar.Value - ((_existNotAllRow_Top) ? 1 : 0);
        }
        #endregion (GetAbsoluteRowIntex)
        //===============================================================================
        #region -SuspendPaint 描画抑制
        //-------------------------------------------------------------------------------
        /// <summary>描画抑制</summary>
        public void SuspendPaint()
        {
            _bSuspendDraw = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (SuspendPaint)
        //-------------------------------------------------------------------------------
        #region -ResumePaint 描画再開
        //-------------------------------------------------------------------------------
        /// <summary>描画再開</summary>
        public void ResumePaint()
        {
            _bSuspendDraw = false;
        }
        #endregion (ResumePaint)
        //-------------------------------------------------------------------------------
        #region #[override]WndProc
        //-------------------------------------------------------------------------------
        //
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (_bSuspendDraw && m.Msg == 0x000f) { return; }
            base.WndProc(ref m);
        }
        #endregion (#[override]WndProc)
        //===============================================================================
        #region -GetIcons 画像取得（別スレッド）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を取得します。
        /// </summary>
        private void GetIcons(IEnumerable<string> iconURLs)
        {
            foreach (var url in iconURLs) {
                Image img = Utilization.GetImageFromURL(url);
                if (img != null) {
                    this.Invoke(new Action(() => ImageListWrapper.ImageAdd(url, img)));
                }
            }
        }
        #endregion (GetIcons)


        //-------------------------------------------------------------------------------
        #endregion (メソッド)

        //-------------------------------------------------------------------------------
        #region 不要
        /*
        #region -AddRows 発言行追加
        //-------------------------------------------------------------------------------
        //
        private void AddRows(UctlDispTwitRow[] rows, SearchDirection searchDir)
        {
            pnlflow.Visible = false;
            //Stopwatch sw = Stopwatch.StartNew();

            switch (searchDir) {
                case SearchDirection.Forward: {
                        foreach (UctlDispTwitRow row in rows.Reverse()) {
                            int i;
                            for (i = 0; i < pnlflow.Controls.Count; i++) {
                                UctlDispTwitRow ctlrow = pnlflow.Controls[i] as UctlDispTwitRow;
                                if (ctlrow.TwitData.StatusID <= row.TwitData.StatusID) { break; }
                            }
                            //if (i != pnlflow.Controls.Count && ((UctlDispTwitRow)pnlflow.Controls[i]).TwitData.StatusID == row.TwitData.StatusID) {
                            //    // かぶるStatus_IDがあった時スルー
                            //    continue;
                            //}
                            pnlflow.Controls.Add(row);
                            pnlflow.Controls.SetChildIndex(row, i);
                            row.TabIndex = 0;
                        }
                    }
                    break;
                case SearchDirection.Backward: {
                        foreach (UctlDispTwitRow row in rows) {
                            int i;
                            for (i = pnlflow.Controls.Count - 1; i >= 0; i--) {
                                UctlDispTwitRow ctlrow = pnlflow.Controls[i] as UctlDispTwitRow;
                                if (ctlrow.TwitData.StatusID >= row.TwitData.StatusID) { break; }
                            }
                            //if (i != -1 && ((UctlDispTwitRow)pnlflow.Controls[i]).TwitData.StatusID == row.TwitData.StatusID) {
                            //    // かぶるStatus_IDがあった時スルー
                            //    continue;
                            //}
                            i++;  // 挿入位置は+1
                            pnlflow.Controls.Add(row);
                            pnlflow.Controls.SetChildIndex(row, i);
                            row.TabIndex = 0;
                        }
                    }
                    break;
                case SearchDirection.BinarySearch: {
                        Func<int, int, int, int> f = (status_ID, min, max) =>
                        {
                            int med = (min + max) / 2;
                            //pnlflow.Controls[med]

                            return 0;
                        };

                        foreach (UctlDispTwitRow row in rows) {

                        }
                    }
                    break;
            }

            //Console.WriteLine(searchDir.ToString() + " : " + sw.ElapsedMilliseconds.ToString() + "ms");
            pnlflow.Visible = true;
        }
        #endregion (AddRows)
        //-------------------------------------------------------------------------------
        #region -AddRowHead リスト先頭に発言行を追加
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>リスト先頭に発言行を追加。</para>
        /// <para></para>
        /// </summary>
        /// <remarks>
        /// Controls[A,B,C]にRows[0,1,2]を入れると，
        /// Controls[0,1,2,A,B,C]になる．
        /// </remarks>
        /// <param name="rows">追加行</param>
        public void AddRowHead(params UctlDispTwitRow[] rows)
        {
            pnlflow.Visible = false;
            foreach (UctlDispTwitRow row in rows.Reverse()) {
                pnlflow.Controls.Add(row);
                pnlflow.Controls.SetChildIndex(row, 0);
                row.TabIndex = 0;
            }
            pnlflow.Visible = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (AddRowHead)
        //-------------------------------------------------------------------------------
        #region -AddRowTail リスト末尾に発言行を追加
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リスト末尾に発言行を追加
        /// </summary>
        /// <remarks>
        /// Controls[A,B,C]にRows[0,1,2]を入れると，
        /// Controls[A,B,C,0,1,2]になる．
        /// </remarks>
        /// <param name="rows">追加行</param>
        public void AddRowTail(params UctlDispTwitRow[] rows)
        {
            pnlflow.Visible = false;
            foreach (UctlDispTwitRow row in rows) {
                pnlflow.Controls.Add(row);
                pnlflow.Controls.SetChildIndex(row, this.Controls.Count - 1);

            }
            pnlflow.Visible = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (AddRowTail)
        //-------------------------------------------------------------------------------
        #region -SearchStatusID 同じ発言IDの要素を見つけます。
        //-------------------------------------------------------------------------------
        //
        private UctlDispTwitRow SearchStatusID(long status_id)
        {
            if (pnlflow.Controls.Count == 0) { return null; }

            // status_id : 0大----小15
            int left = 0, right = pnlflow.Controls.Count - 1;
            int index;

            while (true) {
                index = (right + left) / 2;
                UctlDispTwitRow row = (UctlDispTwitRow)pnlflow.Controls[index];
                if (row.TwitData.StatusID == status_id) {
                    return row;
                }
                else {
                    if (right <= left) { return null; }
                    if (row.TwitData.StatusID > status_id) {
                        left = index + 1;
                    }
                    else {
                        right = index - 1;
                    }
                }
            }
        }
        #endregion (SearchStatusID)
        */
        #endregion 不要
    }

    //-----------------------------------------------------------------------------------
    #region (Class)TwitRowMenuEventArgs
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// 呟き行データに関するイベントのための情報を提供します。
    /// </summary>
    public class TwitRowMenuEventArgs : EventArgs
    {
        /// <summary>イベントの種類</summary>
        public RowEventType EventType { get; private set; }
        /// <summary>呟きデータ</summary>
        public TwitData TwitData { get; private set; }

        //-------------------------------------------------------------------------------
        #region コンストラクタ 初期化
        //-------------------------------------------------------------------------------
        /// <summary>
        /// TwitRowMenuEventArgsクラスを初期化します。
        /// </summary>
        public TwitRowMenuEventArgs(RowEventType type, TwitData data)
        {
            EventType = type;
            TwitData = data;
        }
        #endregion (コンストラクタ)
    }
    //-----------------------------------------------------------------------------------
    #endregion ((Class)SearchInfoEventArgs)

    //-------------------------------------------------------------------------------
    #region +RowEventType 列挙体：呟き行データに関するイベントの種類
    //-------------------------------------------------------------------------------
    /// <summary>
    ///呟き行データに関するイベントの種類
    /// </summary>
    public enum RowEventType
    {
        /// <summary>リプライ メニュー</summary>
        Reply,
        /// <summary>引用 メニュー</summary>
        Quote,
        /// <summary>リツイート メニュー</summary>
        Retweet,
        /// <summary>ダイレクトメッセージ メニュー</summary>
        DirectMessage,
        /// <summary>会話を表示 メニュー</summary>
        DisplayConversation,
        /// <summary>お気に入りに追加 メニュー</summary>
        Favorite,
        /// <summary>お気に入りから削除 メニュー</summary>
        Unfavorite,
        /// <summary>削除 メニュー</summary>
        Delete,
        /// <summary>Retweetしたユーザー メニュー</summary>
        Retweeter,
        /// <summary>より古い発言取得 メニュー</summary>
        OlderTweetRequest,
        /// <summary>より新しい発言取得 メニュー</summary>
        MoreRecentTweetRequest,
        /// <summary>時刻を指定して発言取得 メニュー</summary>
        SpecifyTimeTweetRequest
    }
    //-------------------------------------------------------------------------------
    #endregion (RowEventType)

    //-----------------------------------------------------------------------------------
    #region (Class)EntityEventArgs
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// 呟き行データに関するイベントのための情報を提供します。
    /// </summary>
    public class EntityEventArgs : EventArgs
    {
        /// <summary>イベントの種類</summary>
        public EntityEventType EventType { get; private set; }
        /// <summary>エンティティデータ</summary>
        public string Data { get; private set; }

        //-------------------------------------------------------------------------------
        #region コンストラクタ 初期化
        //-------------------------------------------------------------------------------
        /// <summary>
        /// EntityEventArgsクラスを初期化します。
        /// </summary>
        public EntityEventArgs(EntityEventType type, string data)
        {
            EventType = type;
            Data = data;
        }
        #endregion (コンストラクタ)
    }
    //-----------------------------------------------------------------------------------
    #endregion ((Class)SearchInfoEventArgs)

    //-------------------------------------------------------------------------------
    #region +EntityEventType 列挙体：エンティティデータに関するイベントの種類
    //-------------------------------------------------------------------------------
    /// <summary>
    /// エンティティデータに関するイベントの種類
    /// </summary>
    public enum EntityEventType
    {
        /// <summary>ユーザー：プロフィール表示</summary>
        User_DisplayProfile,
        /// <summary>ユーザー：発言表示</summary>
        User_DisplayTweets,
        /// <summary>ユーザー：ユーザータブ作成</summary>
        User_MakeUserTab,
        /// <summary>ユーザー：リストタブ作成</summary>
        User_MakeListTab,
        /// <summary>ハッシュタグ：タブ作成</summary>
        Hashtag_MakeTab
    }
    //-------------------------------------------------------------------------------
    #endregion (EntityEventType)

    //-----------------------------------------------------------------------------------
    #region +SearchType 列挙体：検索タイプ
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 検索タイプ
    /// </summary>
    public enum SearchType : byte
    {
        /// <summary>public_timelineから</summary>
        Public,
        /// <summary>home_timelineから</summary>
        Home,
        // <summary>friends_timelineから</summary>
        //Friend,
        /// <summary>user_timelineから</summary>
        User,
        /// <summary>mentionから</summary>
        Reply
    }
    //-------------------------------------------------------------------------------
    #endregion (SearchType)

    //-------------------------------------------------------------------------------
    #region +SearchDirection 列挙体：検索方法
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 挿入の際の検索方法を指定します。
    /// </summary>
    public enum SearchDirection
    {
        /// <summary>先頭から順に挿入場所を検索。最新のデータを挿入する際推奨。</summary>
        Forward,
        /// <summary>末尾から順に挿入場所を検索。古いのデータを挿入する際推奨。</summary>
        Backward,
        /// <summary>中央から二分探索で検索。古いか新しいか分からないデータを挿入する際推奨。</summary>
        BinarySearch
    }
    //-------------------------------------------------------------------------------
    #endregion (SearchDirection)
}
