/*
 * 1発言行表示ユーザーコントロール
 * 
 * ●仕様
 * 基本：フォントはMS UI GOTHIC,9pt(Height = 12)
 * 横方向のみPADDINGをもたせている
 * アイコンを見せないようにできるように。
 * 
 * ●履歴
 * 2010/10/01 作成開始
 * 
 * 
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
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace StarlitTwit
{
    /// <summary>
    /// 1発言行表示ユーザーコントロール
    /// </summary>
    public partial class UctlDispTwitRow : UserControl
    {
        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 初期化。
        /// </summary>
        public UctlDispTwitRow(TwitData twitdata)
        {
            InitializeComponent();

            TwitData = twitdata;

            Initialize();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //===============================================================================
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>選択されているかどうか。</summary>
        public new bool Focused { get; private set; }
        /// <summary>再読込回数カウントダウン</summary>
        private int _iReRead_RestTime = 0;
        /// <summary>画像辞書(KeyはURL)</summary>
        public ImageListWrapper ImageListWrapper { get; set; }
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //===============================================================================
        #region プロパティ
        //-------------------------------------------------------------------------------
        #region TwitData プロパティ：発言データ
        //-------------------------------------------------------------------------------
        private TwitData _twitData;
        /// <summary>
        /// 発言データ
        /// </summary>
        public TwitData TwitData
        {
            get { return _twitData; }
            set 
            {
                timerSetPicture.Enabled = false;
                _twitData = value; 
            }
        }
        #endregion (TwitData)
        //-------------------------------------------------------------------------------
        #region Text_NameLabel プロパティ：名前ラベルのテキストを取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>名前ラベルのテキストを取得又は設定します。</para>
        /// <para>設定を行う時はSetNameLabelメソッドをしようしてください。</para>
        /// </summary>
        public string Text_NameLabel
        {
            get { return lblName.Text; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Text_NameLabel)
        //-------------------------------------------------------------------------------
        #region Text_TweetLabel プロパティ：呟きラベルのテキストを取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>呟きラベルのテキストを取得します。</para>
        /// <para>設定を行う場合はSetTweetLabelメソッドを使用してください。</para> 
        /// </summary>
        public string Text_TweetLabel
        {
            get { return lblTweet.Text; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Text_TweetLabel)
        //-------------------------------------------------------------------------------
        #region Icon プロパティ：アイコンを取得または設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// アイコンを取得または設定します。
        /// </summary>
        public Image Icon
        {
            get { return picbIcon.Image; }
            set { picbIcon.Image = value; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Icon)
        //-------------------------------------------------------------------------------
        #region IconVisible プロパティ：アイコンを表示するかどうかを取得又は設定
        //-------------------------------------------------------------------------------
        private bool _iconVisible = false;
        /// <summary>
        /// <para>アイコンを表示するかどうかを取得又は設定します。</para>
        /// <para>実際に表示されているかに関わらず「表示するかどうか」を取得します。</para>
        /// </summary>
        public bool IconVisible
        {
            get { return _iconVisible; }
            set { _iconVisible = picbIcon.Visible = value; }
        }
        //-------------------------------------------------------------------------------
        #endregion (IconVisible)
        //-------------------------------------------------------------------------------
        #region SuspendAdjust プロパティ：コントロール位置調整を抑制するか
        //-------------------------------------------------------------------------------
        private bool _suspendAdjust = false;
        /// <summary>
        /// コントロール位置調整を抑制するかどうかを取得または設定。
        /// </summary>
        [Browsable(false)]
        public bool SuspendAdjust
        {
            get { return _suspendAdjust; }
            set { _suspendAdjust = value; }
        }
        #endregion (SuspendAdjust)
        //-------------------------------------------------------------------------------
        #region ToolTipChangeInterval プロパティ：ToolTipの画面の切り替わり時間(ミリ秒)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ToolTipの画面の切り替わり時間(ミリ秒)を取得または設定します。
        /// </summary>
        public int ToolTipChangeInterval
        {
            get { return tooltipPicture.SwitchInterval; }
            set { tooltipPicture.SwitchInterval = value; }
        }
        #endregion (ToolTipChangeInterval)
        //-------------------------------------------------------------------------------
        #endregion (プロパティ)

        //===============================================================================
        #region 定数
        //-------------------------------------------------------------------------------
        // 縦PADDING
        private const int PADDING_TOP = 2;
        private const int PADDING_BOTTOM = 2;
        // 横PADDING
        private const int PADDING_BETWEEN_LEFT_AND_ICON = 2;
        private const int PADDING_BETWEEN_ICON_AND_LABEL = 2;
        private const int PADDING_BETWEEN_LABEL_AND_LEFT = 2;
        // テキストボックスとラベルの位置補正
        private const int REVISION_TEXTBOX_LEFT = 2;
        private const int REVISION_TEXTBOX_RIGHT = 3;
        // 画像取得時間
        private const int PICTURE_REREAD_TIME = 10000;
        //-------------------------------------------------------------------------------
        #endregion (定数)

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
        [Description("URLを開く要請がある時")]
        public event EventHandler<OpenURLEventArgs> OpenURLRequest;
        //-------------------------------------------------------------------------------
        #endregion (Public イベント)
        //-------------------------------------------------------------------------------
        #region rtxtGet_TweetItemClick 特殊項目クリック時
        //-------------------------------------------------------------------------------
        //
        private void rtxtGet_TweetItemClick(object sender, TweetItemClickEventArgs e)
        {
            if (TweetItemClick != null) {
                TweetItemClick.Invoke(this, e);
            }
        }
        #endregion (rtxtGet_TweetItemClick)
        //-------------------------------------------------------------------------------
        #region UctlDispTwitRow_Load ロード時
        //-------------------------------------------------------------------------------
        //
        private void UctlDispTwitRow_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls) {
                c.MouseDown += Controls_MouseDown;
                c.MouseUp += Controls_MouseUp;
                c.MouseMove += Controls_MouseMove;
                c.MouseClick += Controls_MouseClick;
            }
        }
        #endregion (UctlDispTwitRow_Load)
        //-------------------------------------------------------------------------------
        #region Controls_MouseDown コントロールマウスダウン時
        //-------------------------------------------------------------------------------
        //
        private void Controls_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }
        #endregion (Controls_MouseDown)
        #region #[override]OnMouseDown マウスダウン時
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + this.Location.Y, e.Delta);
            base.OnMouseDown(e2);
        }
        #endregion (OnMouseDown)
        //-------------------------------------------------------------------------------
        #region Controls_MouseMove コントロールマウスムーブ時
        //-------------------------------------------------------------------------------
        //
        private void Controls_MouseMove(object sender, MouseEventArgs e)
        {
            Control ctl = (Control)sender;
            this.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, e.X + ctl.Location.X, e.Y + ctl.Location.Y, e.Delta));
        }
        #endregion (Controls_MouseMove)
        #region #[override]OnMouseMove コントロールマウスムーブ時
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + this.Location.Y, e.Delta);
            base.OnMouseMove(e2);
        }
        #endregion (#[override]OnMouseMove)
        //-------------------------------------------------------------------------------
        #region Controls_MouseUp コントロールマウスアップ時
        //-------------------------------------------------------------------------------
        //
        private void Controls_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }
        #endregion (Controls_MouseUp)
        #region #[override]OnMouseUp コントロールマウスアップ時
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + this.Location.Y, e.Delta);
            base.OnMouseUp(e2);
        }
        #endregion (#[override]OnMouseUp)
        //-------------------------------------------------------------------------------
        #region Controls_MouseClick コントロールマウスクリック時
        //-------------------------------------------------------------------------------
        //
        private void Controls_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }
        #endregion (Controls_MouseClick)
        #region #[override]OnMouseClick コントロールマウスクリック時
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseClick(MouseEventArgs e)
        {
            MouseEventArgs e2 = new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + this.Location.Y, e.Delta);
            base.OnMouseClick(e2);
        }
        #endregion (#[override]OnMouseClick)
        //-------------------------------------------------------------------------------
        #region Label_DoubleClick ラベルダブルクリック時
        //-------------------------------------------------------------------------------
        //
        private void Label_DoubleClick(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;

            if (this.Focused && e.Button == MouseButtons.Left) {
                rtxtGet.Visible = false;
                // ラベル→ラベルとクリックしても両方消えないように
                lblName.Visible = lblTweet.Visible = true;

                //rtxtGet.ForeColor = LABEL_FORECOLOR_SELECTED;

                rtxtGet.BackColor = GetColor(true);

                rtxtGet.Location = new Point(lbl.Location.X + REVISION_TEXTBOX_LEFT, lbl.Location.Y);
                rtxtGet.Size = new Size(lbl.Width, lbl.Height);
                rtxtGet.Font = lbl.Font;
                rtxtGet.Text = lbl.Text;
                rtxtGet.ChangeFonts();

                lbl.Visible = false;
                rtxtGet.Visible = true;

                rtxtGet.Focus();
                rtxtGet.Select(0, 0);
            }
        }
        #endregion (Label_DoubleClick)
        //-------------------------------------------------------------------------------
        #region rtxtGet_Leave テキストボックスLeave時
        //-------------------------------------------------------------------------------
        //
        private void rtxtGet_Leave(object sender, EventArgs e)
        {
            lblName.Visible = lblTweet.Visible = true;
            rtxtGet.Visible = false;
        }
        #endregion (txtGet_Leave)
        //-------------------------------------------------------------------------------
        #region rtxtGet_MouseWheel テキストボックスフォーカス時でマウスホイール
        //-------------------------------------------------------------------------------
        //
        private void rtxtGet_MouseWheel(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
        }
        #endregion (txtGet_MouseWheel)
        //-------------------------------------------------------------------------------
        #region tooltipPicture_PrePopup 画像ツールチップポップアップ前
        //-------------------------------------------------------------------------------
        //
        private void tooltipPicture_PrePopup(object sender, CancelEventArgs e)
        {
            if (!FrmMain.SettingsData.DisplayThumbnail) { e.Cancel = true; return; }

            //if (!this.Focused) { e.Cancel = true; return; }

            if (tooltipPicture.ImageURLs == null) {
                // URL設定
                IEnumerable<string> urls = Utilization.ExtractURL(TwitData.Text);
                tooltipPicture.ImageURLs = urls;
            }
        }
        #endregion (tooltipPicture_PrePopup)
        //-------------------------------------------------------------------------------
        #region tooltipReply_Popup リプライツールチップポップアップ時
        //-------------------------------------------------------------------------------
        //
        private void tooltipReply_Popup(object sender, PopupEventArgs e)
        {
            if (this.Focused) { e.Cancel = true; }
        }
        #endregion (tooltipReply_Popup)
        //-------------------------------------------------------------------------------
        #region rtxtGet_LinkClicked リンククリック時
        //-------------------------------------------------------------------------------
        //
        private void rtxtGet_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (OpenURLRequest != null) {
                OpenURLRequest.Invoke(this, new OpenURLEventArgs(e.LinkText, FrmMain.SettingsData.UseInternalWebBrowser));
            }
        }
        #endregion (rtxtGet_LinkClicked)
        //-------------------------------------------------------------------------------
        #region timerSetPicture_Tick 画像取得タイマ
        //-------------------------------------------------------------------------------
        //
        private void timerSetPicture_Tick(object sender, EventArgs e)
        {
            string IconURL = (TwitData.TwitType == TwitType.Retweet) ? TwitData.RTTwitData.IconURL : TwitData.IconURL;
            // 画像読み込み
            if (ImageListWrapper.ImageContainsKey(IconURL)) {
                picbIcon.Image = ImageListWrapper.GetImage(IconURL);
                picbIcon.Visible = true;
                timerSetPicture.Enabled = false;
            }

            _iReRead_RestTime -= timerSetPicture.Interval;
            if (_iReRead_RestTime <= 0) {
                // 終了
                picbIcon.Image = ImageListWrapper.GetImage(FrmMain.STR_IMGLIST_CROSS);
                picbIcon.Visible = true; 
                timerSetPicture.Enabled = false;
            }
        }
        #endregion (timerSetPicture_Tick)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //===============================================================================
        #region メソッド
        //-------------------------------------------------------------------------------
        #region -Initialize 設定初期化
        //-------------------------------------------------------------------------------
        //
        private void Initialize()
        {
            this.TabStop = false;

            this.BackColor = GetColor(false);
            SuspendAdjust = true;
            SetIconConfig();
            SetFontConfig();
            SuspendAdjust = false;
            SetControlLocation();
        }
        #endregion (Initialize)
        //-------------------------------------------------------------------------------
        #region +SelectControl コントロール選択
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コントロールを選択します。
        /// </summary>
        public void SelectControl()
        {
            //foreach (Control ctl in this.Controls) {
            //    if (ctl is Label) { ctl.ForeColor = LABEL_FORECOLOR_SELECTED; }
            //}

            this.BackColor = GetColor(true);
            this.Focused = true;

            tooltipPicture.SetToolTip(lblTweet);
        }
        #endregion (SelectControl)
        //-------------------------------------------------------------------------------
        #region +UnSelectControl コントロール選択解除
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コントロールを選択解除します。
        /// </summary>
        public void UnSelectControl()
        {
            //foreach (Control ctl in this.Controls) {
            //    if (ctl is Label) { ctl.ForeColor = LABEL_FORECOLOR_UNSELECTED; }
            //}
            this.ActiveControl = null;
            this.BackColor = GetColor(false);
            this.Focused = false;

            tooltipPicture.RemoveAll();
        }
        #endregion (UnSelectControl)
        //-------------------------------------------------------------------------------
        #region +SetWidth コントロールの幅を決定します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コントロールの幅を設定します。高さは自動的に調節されます。
        /// </summary>
        /// <param name="width">新しい幅</param>
        public void SetWidth(int width)
        {
            this.Width = width;
            SetControlLocation();
        }
        //-------------------------------------------------------------------------------
        #endregion (SetWidth)
        //-------------------------------------------------------------------------------
        #region +SetNameLabel 名前ラベルをセットします。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>名前ラベルをセットします。</para>
        /// <para>発言情報が設定されている必要があります。</para>
        /// <para>設定後コントロールの高さが変わる可能性があります。</para>
        /// </summary>
        public void SetNameLabel()
        {
            lblName.Text = Utilization.InterpretFormat(TwitData);
            SetControlLocation();
        }
        //-------------------------------------------------------------------------------
        #endregion (SetNameLabel)
        //-------------------------------------------------------------------------------
        #region +SetTweetLabel 発言ラベルをセットします
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>発言ラベルをセットします。</para>
        /// <para>発言情報が設定されている必要があります。</para>
        /// <para>設定後コントロールの高さが変わる可能性があります。</para>
        /// </summary>
        public void SetTweetLabel()
        {
            lblTweet.Text = (TwitData.IsRT()) ? TwitData.RTTwitData.Text : TwitData.Text;
            SetControlLocation();
        }
        #endregion (SetTweetLabel)
        //-------------------------------------------------------------------------------
        #region +SetIcon アイコンをセットします。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>アイコンを設定します。</para>
        /// <para>発言情報のURLから取得するのでアイコンが設定されている必要があります。</para>
        /// <para>アイコンが正しく設定されたらピクチャーボックスが表示されます。</para>
        /// </summary>
        /// <param name="imageList">[option]画像辞書</param>
        /// <returns>アイコンが正しく設定されたかどうか</returns>
        public bool SetIcon()
        {
            if (!IconVisible || ImageListWrapper == null) { return false; }

            string IconURL = (TwitData.TwitType == TwitType.Retweet) ? TwitData.RTTwitData.IconURL : TwitData.IconURL;

            if (string.IsNullOrEmpty(IconURL)) { 
                picbIcon.Image = ImageListWrapper.GetImage(FrmMain.STR_IMGLIST_CROSS);
            }
            else if (ImageListWrapper.ImageContainsKey(IconURL)) {
                picbIcon.Image = ImageListWrapper.GetImage(IconURL);
            }
            else {
                picbIcon.Image = null;
                // タイマーON
                _iReRead_RestTime = PICTURE_REREAD_TIME;
                timerSetPicture.Enabled = true;
            }
            return IconVisible = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (SetIcon)
        //-------------------------------------------------------------------------------
        #region +SetReplyToolTip リプライツールチップをセットします。
        //-------------------------------------------------------------------------------
        //
        public void SetReplyToolTip(string text)
        {
            if (string.IsNullOrEmpty(text)) {
                tooltipReply.RemoveAll();
            }
            else {
                tooltipReply.SetToolTip(lblTweet, text);
            }
        }
        #endregion (SetReplyToolTip)
        //-------------------------------------------------------------------------------
        #region +SetIconConfig アイコンの設定します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>アイコンのサイズを設定します。</para>
        /// <para>コントロール位置設定が行われます。</para>
        /// </summary>
        public void SetIconConfig()
        {
            IconVisible = FrmMain.SettingsData.DisplayIcon;
            picbIcon.Size = new Size(FrmMain.SettingsData.IconSize, FrmMain.SettingsData.IconSize);
            SetControlLocation();
        }
        #endregion (SetIconConfig)
        //-------------------------------------------------------------------------------
        #region +SetFontConfig フォントの設定を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>フォントの設定を行います。</para>
        /// <para>コントロール位置設定が行われます。</para>
        /// </summary>
        public void SetFontConfig()
        {
            lblName.Font = GetFont(true);
            lblName.ForeColor = GetForeColor(true);
            lblTweet.Font = GetFont(false);
            lblTweet.ForeColor = GetForeColor(false);
            SetControlLocation();
        }
        #endregion (SetFontConfig)
        //-------------------------------------------------------------------------------
        #region +SetBackColor 背景色設定を行います。
        //-------------------------------------------------------------------------------
        //
        public void SetBackColor()
        {
            this.BackColor = GetColor(this.Focused);
        }
        #endregion (SetBackColor)
        //-------------------------------------------------------------------------------
        #region +ResetPicturePopup 画像ポップアップを初期化します。
        //-------------------------------------------------------------------------------
        //
        public void ResetPicturePopup()
        {
            tooltipPicture.ImageURLs = null;
        }
        #endregion (ResetPicturePopup)
        //===============================================================================
        #region +SetControlLocation コントロール位置設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>現在のテキストとフォントから，コントロールのサイズと位置を決定します。</para>
        /// <para>Iconの位置，ラベルの位置とサイズ，このコントロールの高さが調整されます。</para>
        /// <para>[SuspendAdjust = trueの時は調整が行われません。]</para>
        /// </summary>
        public void SetControlLocation()
        {
            if (SuspendAdjust) { return; }

            // ラベルの幅を決定
            int picbSize = (_iconVisible) ? picbIcon.Height : 0;

            int iLabelWidth = this.Width - picbSize - PADDING_BETWEEN_ICON_AND_LABEL
                              - PADDING_BETWEEN_LABEL_AND_LEFT - PADDING_BETWEEN_LEFT_AND_ICON;
            // ラベルに最大幅を設定(これによりラベルに自動的に高さが設定される)
            lblTweet.AutoSize = lblName.AutoSize = true;
            lblName.MaximumSize = lblTweet.MaximumSize = new Size(iLabelWidth, 0);
            // 残り項目設定
            bool isHigherIcon = (_iconVisible && picbSize > lblName.Height + lblTweet.Height);
            this.Height = ((!isHigherIcon) ? (lblName.Height + lblTweet.Height) : picbSize)
                          + uctlline.Height + PADDING_TOP + PADDING_BOTTOM;
            // 横に伸ばせるなら伸ばす
            if (lblTweet.Width < lblTweet.MaximumSize.Width) {
                int height = lblTweet.Height;
                lblTweet.MaximumSize = default(Size);
                lblTweet.AutoSize = false;
                lblTweet.Size = new Size(iLabelWidth, height);
            }
            if (lblName.Width < lblName.MaximumSize.Width) {
                int height = lblName.Height;
                lblName.MaximumSize = default(Size);
                lblName.AutoSize = false;
                lblName.Size = new Size(iLabelWidth, height);
            }
            // pictureBoxのほうが大きいとき，ラベルの大きさを合わせる
            if (isHigherIcon) {
                lblTweet.AutoSize = false;
                lblTweet.Height = picbSize - lblName.Height;
            }

            lblName.Location = new Point(PADDING_BETWEEN_LABEL_AND_LEFT + picbSize +
                                         PADDING_BETWEEN_ICON_AND_LABEL, PADDING_TOP);
            lblTweet.Location = new Point(lblName.Location.X, lblName.Height + PADDING_TOP);
            picbIcon.Location = new Point(PADDING_BETWEEN_LEFT_AND_ICON,
                                          (this.Height - picbSize) / 2);
            uctlline.Location = new Point(0, this.Height - 1);
            uctlline.Length = this.Width;
        }
        //-------------------------------------------------------------------------------
        #endregion (SetControlLocation)
        //-------------------------------------------------------------------------------
        #region +SetLineColor 下部の線の色を設定します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 下部線の色を設定します。
        /// </summary>
        /// <param name="isBlack">黒色true,その他の色false</param>
        public void SetLineColor(bool isBlack)
        {
            uctlline.BackColor = (isBlack) ? Color.Black : Color.Red;
        }
        #endregion (SetLineColor)
        //-------------------------------------------------------------------------------
        #region -GetColor 色取得
        //-------------------------------------------------------------------------------
        //
        private Color GetColor(bool selected)
        {
            if (TwitData.TwitType == TwitType.Retweet) {
                return (selected)
                ? FrmMain.SettingsData.ColorRTTweetBackSelected
                : FrmMain.SettingsData.ColorRTTweetBackUnselected;
            }
            else if (TwitData.Mention_StatusID > 0) {
                if (TwitData.Mention_UserID == FrmMain.Twitter.ID) {
                    return (selected)
                    ? FrmMain.SettingsData.ColorReplyToMeTweetBackSelected
                    : FrmMain.SettingsData.ColorReplyToMeTweetBackUnselected;
                }
                else {
                    return (selected)
                    ? FrmMain.SettingsData.ColorReplyToOtherTweetBackSelected
                    : FrmMain.SettingsData.ColorReplyToOtherTweetBackUnselected;
                }
            }
            else {
                return (selected)
                ? FrmMain.SettingsData.ColorNormalTweetBackSelected
                : FrmMain.SettingsData.ColorNormalTweetBackUnselected;
            }
        }
        #endregion (GetColor)
        //-------------------------------------------------------------------------------
        #region -GetFont フォント取得
        //-------------------------------------------------------------------------------
        //
        private Font GetFont(bool isTitle)
        {
            if (TwitData.TwitType == TwitType.Retweet) {
                return (isTitle)
                ? FrmMain.SettingsData.FontRTTweetTitle
                : FrmMain.SettingsData.FontRTTweetText;
            }
            else if (TwitData.Mention_StatusID > 0) {
                if (TwitData.Mention_UserID == FrmMain.Twitter.ID) {
                    return (isTitle)
                    ? FrmMain.SettingsData.FontReplyToMeTweetTitle
                    : FrmMain.SettingsData.FontReplyToMeTweetText;
                }
                else {
                    return (isTitle)
                    ? FrmMain.SettingsData.FontReplyToOtherTweetTitle
                    : FrmMain.SettingsData.FontReplyToOtherTweetText;
                }
            }
            else {
                return (isTitle)
                ? FrmMain.SettingsData.FontNormalTweetTitle
                : FrmMain.SettingsData.FontNormalTweetText;
            }
        }
        #endregion (GetColor)
        //-------------------------------------------------------------------------------
        #region -GetForeColor 前景色取得
        //-------------------------------------------------------------------------------
        //
        private Color GetForeColor(bool isTitle)
        {
            if (TwitData.TwitType == TwitType.Retweet) {
                return (isTitle)
                ? FrmMain.SettingsData.ColorRTTweetTitle
                : FrmMain.SettingsData.ColorRTTweetText;
            }
            else if (TwitData.Mention_StatusID > 0) {
                if (TwitData.Mention_UserID == FrmMain.Twitter.ID) {
                    return (isTitle)
                    ? FrmMain.SettingsData.ColorReplyToMeTweetTitle
                    : FrmMain.SettingsData.ColorReplyToMeTweetText;
                }
                else {
                    return (isTitle)
                    ? FrmMain.SettingsData.ColorReplyToOtherTweetTitle
                    : FrmMain.SettingsData.ColorReplyToOtherTweetText;
                }
            }
            else {
                return (isTitle)
                ? FrmMain.SettingsData.ColorNormalTweetTitle
                : FrmMain.SettingsData.ColorNormalTweetText;
            }
        }
        #endregion (GetForeColor)
        //===============================================================================

        //-------------------------------------------------------------------------------
        #endregion (メソッド)

        //-------------------------------------------------------------------------------
        #region WndProc メッセージ受信 (Comment outing)
        //-------------------------------------------------------------------------------
        //
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{
        //    const int WM_HSCROLL = 0x114;
        //    const int WM_MOUSEHWHEEL = 0x20E;

        //    if (m.Msg == WM_HSCROLL || m.Msg == WM_MOUSEHWHEEL) {
        //        m.Result = (IntPtr)1;

        //        //switch ((int)m.WParam) {
        //        //    case 7864320: //右スクロール
        //        //        //m.Result
        //        //        break;
        //        //    case -7864320: //左スクロール

        //        //        break;
        //        //    default:
        //        //        break;
        //        //}
        //    }
        //    base.WndProc(ref m);

        //}
        #endregion (WndProc)
    }

    //-------------------------------------------------------------------------------
    #region (Class)OpenURLEventArgs
    //-------------------------------------------------------------------------------
    public class OpenURLEventArgs : EventArgs
    {
        /// <summary>URL</summary>
        public string url { get; private set; }

        public bool useInternalBrowser { get; private set; }

        public OpenURLEventArgs(string url, bool useInternalBrowser)
        {
            this.url = url;
            this.useInternalBrowser = useInternalBrowser;
        }
    }
    //-------------------------------------------------------------------------------
    #endregion ((Class)OpenURLEventArgs)
}
