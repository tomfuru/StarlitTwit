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

namespace TwitterClient
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
        //public Dictionary<string, Image> ImageDic { get; set; }
        public ImageList ImageList { get; set; }
        /// <summary>最大発言ID</summary>
        [Browsable(false)]
        public long MaxTweetID { get; private set; }
        /// <summary>最小発言ID</summary>
        [Browsable(false)]
        public long MinTweetID { get; private set; }
        /// <summary>各発言の情報</summary>
        private SortedList<long, RowData> _rowDataList;
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
        private bool _existGapVscr = false;
        /// <summary>[static]long比較用クラス</summary>
        static readonly Longcomp CLONGCOMP = new Longcomp();
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
            get { return (_rowDataList.Values[_selectedIndex]).TwitData; }
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
        /// <summary>URLオープン要請時</summary>
        [Category("動作")]
        [Description("URLオープン要請時")]
        public event EventHandler<OpenURLEventArgs> OpenURLRequest;
        /// <summary>選択インデックス変更時</summary>
        [Category("動作")]
        [Description("選択インデックス変更時")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>行右クリックメニュークリック時</summary>
        [Category("ツイート")]
        [Description("行右クリックメニュークリック時")]
        public event EventHandler<TwitRowMenuEventArgs> RowContextMenu_Click;
        //-------------------------------------------------------------------------------
        #endregion (Public イベント)
        //-------------------------------------------------------------------------------
        #region menuRow_Opening メニュー項目の設定
        //-------------------------------------------------------------------------------
        //
        private void menuRow_Opening(object sender, CancelEventArgs e)
        {
            // 表示するかどうか
            if (this._selectedIndex == -1) { e.Cancel = true; return; }

            switch (ContextMenuType) {
                case MenuType.Default:
                    break;
                case MenuType.Conversation:
                    tsmiDispConversation.Visible = tsmiSepConversation.Visible = false;
                    tsmiSpecifyTime.Visible = tsmiMoreRecently.Visible = tsmiOlder.Visible = tsmiSepMoreTweet.Visible = false;
                    break;
            }

            bool isReply = (SelectedTwitData.Mention_StatusID > 0);
            bool isDirect = (SelectedTwitData.TwitType == TwitType.DirectMessage);
            bool isProtected = SelectedTwitData.UserProtected;
            bool isMine = (SelectedTwitData.UserID == FrmMain.Twitter.ID);
            bool isFavorited = SelectedTwitData.Favorited;

            tsmiReply.Enabled = !isDirect;
            tsmiQuote.Enabled = tsmiRetweet.Enabled = !isDirect && !isProtected;
            tsmiRetweet.Enabled = !(isMine || isDirect);

            tsmiDispConversation.Enabled = isReply;

            tsmiFavorite.Enabled = !isDirect;
            tsmiFavorite.Visible = !isFavorited;
            tsmiUnfavorite.Visible = isFavorited;

            tsmiOpenBrowser_ReplyTweet.Enabled = isReply;

            tsmiDelete.Enabled = isMine;

            tsmiMoreRecently.Visible = false; // 暫定
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
        #region tsmiOpenBrowser_UserHome_Click ブラウザで開く-このユーザーのホームメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowser_UserHome_Click(object sender, EventArgs e)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append(SelectedTwitData.UserScreenName);

            if (OpenURLRequest != null) {
                OpenURLRequest.Invoke(this, new OpenURLEventArgs(sbUrl.ToString(),
                                                                 FrmMain.SettingsData.UseInternalWebBrowser));
            }
        }
        #endregion (tsmiOpenBrowser_UserHome_Click)
        #region tsmiOpenBrowser_ThisTweet_Click ブラウザで開く-このツイートメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowser_ThisTweet_Click(object sender, EventArgs e)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append(SelectedTwitData.UserScreenName);
            sbUrl.Append("/status/");
            sbUrl.Append(SelectedTwitData.StatusID);

            if (OpenURLRequest != null) {
                OpenURLRequest.Invoke(this, new OpenURLEventArgs(sbUrl.ToString(),
                                                                 FrmMain.SettingsData.UseInternalWebBrowser));
            }
        }
        #endregion (tsmiOpenBrowser_ThisTweet_Click)
        #region tsmiOpenBrowser_ReplyTweet_Click ブラウザで開く-リプライ先メニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowser_ReplyTweet_Click(object sender, EventArgs e)
        {
            TwitData twitdata;
            if (Utilization.GetTwitDataFromID(SelectedTwitData.Mention_StatusID, out twitdata)) {
                StringBuilder sbUrl = new StringBuilder();
                sbUrl.Append(Twitter.URLtwi);
                sbUrl.Append(twitdata.UserScreenName);
                sbUrl.Append("/status/");
                sbUrl.Append(twitdata.StatusID);

                if (OpenURLRequest != null) {
                    OpenURLRequest.Invoke(this, new OpenURLEventArgs(sbUrl.ToString(),
                                                                     FrmMain.SettingsData.UseInternalWebBrowser));
                }
            }
            else {
                Message.ShowWarningMessage("返信先ツイートが見つかりませんでした。");
            }
        }
        #endregion (tsmiOpenBrowser_ReplyTweet_Click)
        //-------------------------------------------------------------------------------
        #region tsmiMakeUserTab_Click このユーザーのタブを作成メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiMakeUserTab_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.MakeUserTab, SelectedTwitData));
            }
        }
        #endregion (tsmiMakeUserTab_Click)
        #region tsmiMakeUserListTab_Click このユーザーのリストのタブを作成メニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiMakeUserListTab_Click(object sender, EventArgs e)
        {
            if (RowContextMenu_Click != null) {
                RowContextMenu_Click.Invoke(this, new TwitRowMenuEventArgs(RowEventType.MakeUserListTab, SelectedTwitData));
            }
        }
        #endregion (tsmiMakeUserListTab_Click)
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
        //===============================================================================
        #region pnlflow_MouseDown マウスダウン時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseDown(object sender, MouseEventArgs e)
        {
            UctlDispTwitRow row = pnlflow.GetChildAtPoint(e.Location, GetChildAtPointSkip.Invisible) as UctlDispTwitRow;
            if (row == null) { _selectedIndex = -1; }
            if (row != SelectedRow) { ChangeSelectRow(row); }

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
        //
        public void ProcessKey(Keys key)
        {
            if (pnlflow.Controls.Count == 0) { return; }

            switch (key) {
                case Keys.Down:
                case Keys.Up:
                    if (_selectedIndex < 0) {
                        // 一番上選択
                        _selectedIndex = GetAbsoluteRowIntex(0);
                    }
                    else {
                        int iselected = SelectedIndex;
                        if (key == Keys.Up) {
                            // ↑キー
                            if (iselected == 0) { break; }
                            _selectedIndex--;
                        }
                        else {
                            // ↓キー
                            if (iselected == _rowDataList.Count - 1) { break; }
                            _selectedIndex++;
                        }
                    }
                    ChangeSelectRow(_selectedIndex);
                    ScrollSelectedRowIntoView();
                    break;
                case Keys.PageUp:
                    if (vscrbar.Enabled) {
                        _selectedIndex = Math.Max(_selectedIndex - vscrbar.LargeChange, 0);
                        vscrbar.Value = Math.Max(vscrbar.Value - vscrbar.LargeChange, 0);
                    }
                    else {
                        vscrbar.Value = _selectedIndex = 0;
                    }
                    break;
                case Keys.PageDown:
                    if (vscrbar.Enabled) {
                        _selectedIndex = Math.Min(_selectedIndex + vscrbar.LargeChange, _rowDataList.Count - 1);
                        vscrbar.Value = Math.Min(vscrbar.Value + vscrbar.LargeChange, _rowDataList.Count - 1);
                    }
                    else {
                        vscrbar.Value = _selectedIndex = _rowDataList.Count - 1;
                    }
                    break;
                case Keys.Home:
                    vscrbar.Value = _selectedIndex = 0;
                    break;
                case Keys.End:
                    vscrbar.Value = _selectedIndex = _rowDataList.Count - 1;
                    break;
            }
        }
        #endregion (ProcessKey)
        //-------------------------------------------------------------------------------
        #region UctlDispTwit_ClientSizeChanged クライアントサイズ変更
        //-------------------------------------------------------------------------------
        private void UctlDispTwit_ClientSizeChanged(object sender, EventArgs e)
        {
            AdjustControll(vscrbar.Value, true);
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
        #region Row_OpenURLRequest URLオープン要請時
        //-------------------------------------------------------------------------------
        //
        private void Row_OpenURLRequest(object sender, OpenURLEventArgs e)
        {
            if (OpenURLRequest != null) {
                OpenURLRequest.Invoke(this, e);
            }
        }
        #endregion (Row_OpenURLRequest)
        //-------------------------------------------------------------------------------
        #region pnlflow_MouseWheel マウスホイール時
        //-------------------------------------------------------------------------------
        //
        private void pnlflow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (vscrbar.Enabled) {
                int moveval = e.Delta / 120;
                if (moveval > 0) {
                    vscrbar.Value = Math.Max(vscrbar.Minimum, vscrbar.Value - moveval);
                }
                else {
                    vscrbar.Value = Math.Min(vscrbar.Maximum, vscrbar.Value - moveval);
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
            if (e.NewValue == e.OldValue) { return; }
            vscrbar.Value = e.NewValue;
        }
        #endregion (vscrbar_Scroll)
        //-------------------------------------------------------------------------------
        #region vscrbar_ValueChanged 値変化時
        //-------------------------------------------------------------------------------
        bool _suspend_vscrbar_ValueChangeEvent = false;
        //
        private void vscrbar_ValueChanged(object sender, EventArgs e)
        {
            if (_suspend_vscrbar_ValueChangeEvent) { return; }

            _suspend_vscrbar_ValueChangeEvent = true;
            AdjustControll(vscrbar.Value, true);
            pnlflow.Refresh();
            _suspend_vscrbar_ValueChangeEvent = false;
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
        /// <returns>最初のツイートのデータ</returns>
        public string AddData(TwitData[] data)
        {
            string retText = "";

            // 選択保存用
            long selectedData = (_selectedIndex >= 0) ? _rowDataList.Values[_selectedIndex].TwitData.StatusID : -1;
            ChangeSelectRow(null);
            // 位置保存用
            long locIndex = (vscrbar.Enabled && vscrbar.Value > 0) ? (_existGapVscr) ? _rowList[_iVisibleRowNum - 1].TwitData.StatusID : _rowList[0].TwitData.StatusID : -1;

            //-----内部情報設定-----
            List<RowData> addrowList = new List<RowData>();
            foreach (TwitData t in data) {
                // 重複排除
                if (_rowDataList.ContainsKey(t.StatusID)) { continue; }

                RowData rowdata = new RowData() { TwitData = t };

                // 返り値用
                if (string.IsNullOrEmpty(retText)) {
                    retText = Utilization.InterpretFormat(t) + '\n' + t.Text;
                }

                addrowList.Add(rowdata);
                _rowDataList.Add(t.StatusID, rowdata);

                //Console.WriteLine(t.StatusID);

                // 最小・最大ID更新
                if (MaxTweetID == -1) { MaxTweetID = MinTweetID = t.StatusID; } // 最初
                else {
                    MaxTweetID = Math.Max(MaxTweetID, t.StatusID);
                    MinTweetID = Math.Min(MinTweetID, t.StatusID);
                }
            }

            // 追加分Replyツールチップ設定
            if (FrmMain.SettingsData.DisplayReplyToolTip) {
                foreach (RowData row in addrowList) {
                    if (row.TwitData.Mention_StatusID > 0) {
                        int depth;
                        if ((depth = FrmMain.SettingsData.DisplayReplyToolTipDepth) == 0) { depth = int.MaxValue; }
                        string replytext = GetReplyText(row.TwitData.Mention_StatusID, depth);
                        row.StrReplyTooltip = replytext;
                    }
                }
            }

            //-----コントロール設定-----

            // 選択復元
            if (selectedData >= 0) {
                int newIndex = _rowDataList.IndexOfKey(selectedData);
                _selectedIndex = newIndex;
                ChangeSelectRow(newIndex);
            }

            vscrbar.Maximum = _rowDataList.Count - 1;

            if (locIndex >= 0) {
                // 位置復元
                int baseIndex = _rowDataList.IndexOfKey(locIndex);
                if (_existGapVscr) {
                    AdjustControll(baseIndex, false);
                }
                else {
                    _suspend_vscrbar_ValueChangeEvent = true;
                    vscrbar.Value = baseIndex;
                    _suspend_vscrbar_ValueChangeEvent = false;
                    AdjustControll(vscrbar.Value, true); // コントロール位置調整
                }
            }
            else { AdjustControll(vscrbar.Value, true); } // コントロール位置調整

            return retText;
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
            AdjustControll(vscrbar.Value, true);
            //foreach (Control ctl in pnlflow.Controls) {
            //    UctlDispTwitRow row = ctl as UctlDispTwitRow;
            //    if (row == null) { continue; }
            //    row.ToolTipChangeInterval = FrmMain.SettingsData.DisplayThumbnailInterval;
            //    row.SuspendAdjust = true;
            //    row.SetNameLabel();
            //    row.SetIconConfig();
            //    row.SetFontConfig();
            //    row.SuspendAdjust = false;
            //    row.SetControlLocation();
            //}
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
            MaxTweetID = MinTweetID = _selectedIndex = -1;
            SelectedRow = null;

            _rowDataList.Clear();
            _rowList.ForEach((row) => row.Visible = false);
            vscrbar.Value = 0;
            vscrbar.Enabled = false;
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

            _rowDataList.Remove(statusid);
            if (_selectedIndex == _rowDataList.Keys.IndexOf(statusid)) { SelectedIndex = -1; }
            AdjustControll(vscrbar.Value, true);
            return true;
        }
        #endregion (RemoveTweet)
        //===============================================================================
        #region -AdjustControll コントロールの内容・位置・サイズの調整を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コントロールの内容・位置・サイズの調整を行います。
        /// </summary>
        private void AdjustControll(int startIndex, bool flowDirForward, bool isSuspendLayout = true, bool isSuspendPaint = true)
        {
            if (_rowDataList.Count == 0) { return; }

            //this.Visible = false;
            if (isSuspendPaint) { SuspendPaint(); }
            if (isSuspendLayout) { pnlflow.SuspendLayout(); }

            // 選択中行発言ID取得
            long selectedStatusID = (_selectedIndex == -1) ? -1 : _rowDataList.Values[_selectedIndex].TwitData.StatusID;
            SelectedRow = null;

            // 全カラムの位置・幅・高さ調整
            bool needScrollbar = true;
            bool isForward = flowDirForward;
            bool existNotAllVisibleRow = false;
            int rowindex = 0;
            int rowdataindex = startIndex;
            int height = (flowDirForward) ? 0 : pnlflow.Height;
            while ((isForward && height < pnlflow.Height) || (!isForward && height > 0)) {
                if (_rowDataList.Count <= rowdataindex) {// isForward=trueのみ．一番下までいった
                    if (startIndex == 0) { needScrollbar = false; break; } // 数が少なくてスクロールバーもいらないような時
                    isForward = false;
                    rowdataindex = startIndex - 1;
                    int dif = pnlflow.Height - height;
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
                    row = new UctlDispTwitRow(rowdata.TwitData);
                    row.MouseDown += pnlflow_MouseDown;
                    row.MouseUp += pnlflow_MouseUp;
                    row.MouseMove += pnlflow_MouseMove;
                    row.MouseClick += pnlflow_MouseClick;
                    row.TweetItemClick += Row_TweetItemClick;
                    row.OpenURLRequest += Row_OpenURLRequest;
                    _rowList.Add(row);
                    pnlflow.Controls.Add(row);
                }
                else {
                    row = _rowList[rowindex];
                    row.TwitData = rowdata.TwitData;
                    row.Visible = true;
                }

                if (selectedStatusID >= 0) {
                    if (selectedStatusID == row.TwitData.StatusID) { // 選択中か判別
                        row.SelectControl();
                        SelectedRow = row;
                    }
                    else { row.UnSelectControl(); }
                }

                row.SetBackColor();
                row.Invalidate();

                row.SetReplyToolTip(rowdata.StrReplyTooltip);
                row.ResetPicturePopup();

                row.SuspendAdjust = true;
                row.SetWidth(pnlflow.Width);
                row.SetIconConfig();
                row.SetFontConfig();
                row.SetNameLabel();
                row.SetTweetLabel();
                row.SetIcon(ImageList);
                row.SuspendAdjust = false;
                row.SetControlLocation();
                if (isForward) {
                    row.Location = new Point(0, height);
                    height += row.Height;
                    existNotAllVisibleRow = (height > pnlflow.Height);
                }
                else {
                    row.Location = new Point(0, height - row.Height);
                    height -= row.Height;
                    existNotAllVisibleRow = (height < 0);
                }

                if (isForward) { rowdataindex++; }
                else {
                    if (height >= pnlflow.Height) { break; } // これ以上入れるとはみ出る
                    // 現在行を先頭にして移動
                    _rowList.Remove(row);
                    _rowList.Insert(0, row);
                    pnlflow.Controls.SetChildIndex(row, 0);
                    
                    rowdataindex--;
                }
                rowindex++;
            }

            if (isSuspendLayout) {
                pnlflow.ResumeLayout(false);
                pnlflow.PerformLayout();
            }
            if (isSuspendPaint) { ResumePaint(); }

            _iVisibleRowNum = rowindex;
            _iAllVisibleRowNum = _iVisibleRowNum - ((existNotAllVisibleRow) ? 1 : 0);
            if (_rowList.Count > _iVisibleRowNum) {
                for (int i = _iVisibleRowNum; i < _rowList.Count; i++) {
                    _rowList[i].Visible = false;
                }
            }

            _existGapVscr = false;
            // スクロールバー設定
            _suspend_vscrbar_ValueChangeEvent = true;
            if (vscrbar.Enabled = needScrollbar) {
                vscrbar.LargeChange = Math.Max(0, _iVisibleRowNum - 1);
                vscrbar.Maximum = _rowDataList.Count - 1;

                if (!isForward) {
                    vscrbar.Value = rowdataindex + 2;
                    //vscrbar.LargeChange++;
                    //vscrbar.Value = Math.Max(0, vscrbar.Maximum - _iAllVisibleRowNum);
                    _existGapVscr = existNotAllVisibleRow;
                }
            }
            _suspend_vscrbar_ValueChangeEvent = false;
        }
        //-------------------------------------------------------------------------------
        #endregion (AdjustControll)
        //-------------------------------------------------------------------------------
        #region -ScrollSelectedRowIntoView 選択行が見えるようにスクロールします。
        //-------------------------------------------------------------------------------
        //
        private void ScrollSelectedRowIntoView()
        {
            if (SelectedIndex < 0) { return; }
            if (SelectedIndex < vscrbar.Value) { // 選択が上にありすぎる
                vscrbar.Value = SelectedIndex;
                AdjustControll(vscrbar.Value, true);
            }
            else if (SelectedIndex >= vscrbar.Value + _iAllVisibleRowNum) { // 選択が下にありすぎる
                AdjustControll(_selectedIndex, false);//AdjustControllSelectedRowIntoView();
                this.Refresh(); // 処理が重いからか，下おしっぱで表示がついていかないので必要
            }
        }
        #endregion (ScrollSelectedRowIntoView)
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
            return "";
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
                _selectedIndex = GetAbsoluteRowIntex(pnlflow.Controls.IndexOf(row));
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
        #endregion (ChangeSelectRow)
        //-------------------------------------------------------------------------------
        #region -OnSelectedIndexChanged 選択インデックス変更時
        //-------------------------------------------------------------------------------
        //
        private void OnSelectedIndexChanged()
        {
            //if (_selectedIndex >= 0) {
            //    pnlflow.ScrollControlIntoView(pnlflow.Controls[GetLookRowIndex(_selectedIndex)]);
            //}
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
                    ? pnlflow.Controls[lookIndex] as UctlDispTwitRow
                    : null;
        }
        #endregion (GetRowFromIndex)
        //-------------------------------------------------------------------------------
        #region -GetLookRowIndex 見た目の行インデックスを取得
        //-------------------------------------------------------------------------------
        //
        private int GetLookRowIndex(int absRowIndex)
        {
            return absRowIndex - vscrbar.Value + ((_existGapVscr) ? 1 : 0);
        }
        #endregion (GetLookRowIndex)
        #region -GetAbsoluteRowIntex 絶対行インデックスを取得
        //-------------------------------------------------------------------------------
        //
        private int GetAbsoluteRowIntex(int lookRowIndex)
        {
            return lookRowIndex + vscrbar.Value - ((_existGapVscr) ? 1 : 0);
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
        /// SearchInfoEventArgsクラスを初期化します。
        /// </summary>
        /// <param name="info">検索情報</param>
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
    #region RowEventType 列挙体：呟き行データに関するイベントの種類
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
        /// <summary>このユーザーのタブを作成 メニュー</summary>
        MakeUserTab,
        /// <summary>このユーザーのリストのタブを作成 メニュー</summary>
        MakeUserListTab,
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
