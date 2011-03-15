using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class FrmConfig : Form
    {
        //-------------------------------------------------------------------------
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>設定データを取得します。</summary>
        public SettingsData SettingsData { get; set; }
        /// <summary>デフォルト設定データ</summary>
        public readonly SettingsData DefaultSettings = new SettingsData();
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmConfig()
        {
            InitializeComponent();
            SettingsData = null;
            InitializeControl();
            SetAssociateData();
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region FrmConfig_Load フォームロード時
        //-------------------------------------------------------------------------------
        //
        private void FrmConfig_Load(object sender, EventArgs e)
        {
            if (SettingsData == null) {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                throw new ArgumentException("無効な設定データです。");
            }

            SetSettingsData();
        }
        #endregion (FrmConfig_Load)
        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            // 設定
            GetSettingsData();
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセルボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)

        //===============================================================================
        #region NumericUpDown_First_ValueChanged 数値選択コントロール値設定時
        //-------------------------------------------------------------------------------
        //
        private void NumericUpDown_First_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numUpDown = (NumericUpDown)sender;
            numUpDown.Value = (numUpDown.Value / 10) * 10;
        }
        #endregion (NumericUpDown_First_ValueChanged)

        //===============================================================================
        #region txtNameFormat_TextChanged 上段フォーマットテキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtNameFormat_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            Label label = (Label)textbox.Tag;
            label.Text = Utilization.InterpretFormat((TwitData)label.Tag, textbox.Text);
        }
        #endregion (txtNameFormat_TextChanged)
        //-------------------------------------------------------------------------------
        #region btnNameFormatInit_Click 上段フォーマット設定デフォルトクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnNameFormatInit_Click(object sender, EventArgs e)
        {
            txtNameFormat.Text = DefaultSettings.NameFormat;
        }
        #endregion (btnNameFormatInit_Click)
        //-------------------------------------------------------------------------------
        #region btnNameFormatRTInit_Click RT上段フォーマット設定デフォルトクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnNameFormatRTInit_Click(object sender, EventArgs e)
        {
            txtNameFormatRT.Text = DefaultSettings.NameFormatRetweet;
        }
        #endregion (btnNameFormatRTInit_Click)
        //-------------------------------------------------------------------------------
        #region btnNameFormatDMInit_Click DM上段フォーマット設定デフォルトクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnNameFormatDMInit_Click(object sender, EventArgs e)
        {
            txtNameFormatDM.Text = DefaultSettings.NameFormatDM;
        }
        #endregion (btnNameFormatDMInit_Click)
        //-------------------------------------------------------------------------------
        #region btnNameFormatSearchInit_Click 検索上段フォーマット設定デフォルトクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnNameFormatSearchInit_Click(object sender, EventArgs e)
        {
            txtNameFormatSearch.Text = DefaultSettings.NameFormatSearch;
        }
        #endregion (btnNameFormatSearchInit_Click)

        //===============================================================================
        #region btnFontConfig_Click フォント設定ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnFontConfig_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TextBox textbox = button.Tag as TextBox;
            if (textbox == null) { return; }

            using (FontDialog fdialog = new FontDialog()) {
                fdialog.ShowColor = true;
                fdialog.Color = textbox.ForeColor;
                fdialog.Font = textbox.Font;
                if (fdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    textbox.Font = fdialog.Font;
                    textbox.ForeColor = fdialog.Color;
                }
            }
        }
        #endregion (btnFontConfig_Click)
        //-------------------------------------------------------------------------------
        #region btnBackColorConfig_Click 背景色設定ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnBackColorConfig_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TextBox textbox = button.Tag as TextBox;
            if (textbox == null) { return; }

            using (ColorDialog cdialog = new ColorDialog()) {
                cdialog.Color = textbox.BackColor;
                if (cdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    textbox.BackColor = cdialog.Color;
                }
            }
        }
        #endregion (btnBackColorConfig_Click)
        //-------------------------------------------------------------------------------
        #region btnAllFontColorDefault_Click 全てデフォルトに戻すボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnAllFontColorDefault_Click(object sender, EventArgs e)
        {
            if (Message.ShowQuestionMessage("全てのフォント・背景色をデフォルトに戻します", "よろしいですか？") == DialogResult.Yes) {
                txtBCNormalSelected.BackColor = DefaultSettings.ColorNormalTweetBackSelected;
                txtBCNormalUnSelected.BackColor = DefaultSettings.ColorNormalTweetBackUnselected;
                txtBCReplyToMeSelected.BackColor = DefaultSettings.ColorReplyToMeTweetBackSelected;
                txtBCReplyToMeUnSelected.BackColor = DefaultSettings.ColorReplyToMeTweetBackUnselected;
                txtBCReplyToOtherSelected.BackColor = DefaultSettings.ColorReplyToOtherTweetBackSelected;
                txtBCReplyToOtherUnSelected.BackColor = DefaultSettings.ColorReplyToOtherTweetBackUnselected;
                txtBCRTSelected.BackColor = DefaultSettings.ColorRTTweetBackSelected;
                txtBCRTUnSelected.BackColor = DefaultSettings.ColorRTTweetBackUnselected;

                txtExNormalTitle.Font = txtExReplyToMeTitle.Font =
                    txtExReplyToOtherTitle.Font = txtExRTTitle.Font = SettingsData.DEFAULT_FONT_TITLE;

                txtExNormalText.Font = txtExReplyToMeText.Font =
                    txtExReplyToOtherText.Font = txtExRTText.Font = SettingsData.DEFAULT_FONT_TEXT;
            }
        }
        #endregion (btnAllFontColorDefault_Click)
        //===============================================================================
        #region chbWithEnableChange_CheckedChanged アイコン表示有無変更時
        //-------------------------------------------------------------------------------
        //
        private void chbWithEnableChange_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = (CheckBox)sender;
            Control changeControl = (Control)chb.Tag;

            changeControl.Enabled = chb.Checked;
        }
        #endregion (chbDispIcon_CheckedChanged)
        //-------------------------------------------------------------------------------
        #region chbUseInternalWebBrowser_CheckedChanged 内部ウェブブラウザ使用有無変更時
        //-------------------------------------------------------------------------------
        //
        private void chbUseInternalWebBrowser_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = (CheckBox)sender;
            txtWebBrowserPath.Enabled = btnSelectWebBrowser.Enabled = !chb.Checked;
        }
        #endregion (chbUseInternalWebBrowser_CheckedChanged)
        //-------------------------------------------------------------------------------
        #region btnSelectWebBrowser_Click ウェブブラウザ選択ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnSelectWebBrowser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog()) {
                fd.CheckFileExists = true;
                fd.Filter = "実行ファイル(*.exe)|*.exe";
                fd.FileName = txtWebBrowserPath.Text;

                if (fd.ShowDialog() == DialogResult.OK) {
                    txtWebBrowserPath.Text = fd.FileName;
                }
            }
        }
        #endregion (btnSelectWebBrowser_Click)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //-------------------------------------------------------------------------------
        #region メソッド
        //-------------------------------------------------------------------------------
        #region -InitializeControl コントロール初期化
        //-------------------------------------------------------------------------------
        //
        private void InitializeControl()
        {
            // サムネイル設定
            SetComboBoxEnumValue(cmbThumbimgly,typeof(imglyThumbnailType));
            SetComboBoxEnumValue(cmbThumbmovapic,typeof(movapicThumbnailType));
            SetComboBoxEnumValue(cmbThumbowly,typeof(owlyThumbnailType));
            SetComboBoxEnumValue(cmbThumbPhotozou,typeof(PhotozouThumbnailType));
            SetComboBoxEnumValue(cmbThumbplixi, typeof(plixiThumbnailType));
            SetComboBoxEnumValue(cmbThumbTwitpic, typeof(TwitPicThumbnailType));
            SetComboBoxEnumValue(cmbThumbyFrog,typeof(yFrogThumbnailType));

            // URL短縮サービスコンボボックス
            SetComboBoxEnumValue(cmbURLShortenType, typeof(URLShortenType));
        }
        #endregion (InitializeControl)
        //-------------------------------------------------------------------------------
        #region -SetAssociateData コントロールのTagデータを設定
        //-------------------------------------------------------------------------------
        //
        private void SetAssociateData()
        {
            txtNameFormat.Tag = lblNameFormatSample;
            txtNameFormatRT.Tag = lblNameFormatRTSample;
            txtNameFormatDM.Tag = lblNameFormatDMSample;
            txtNameFormatSearch.Tag = lblNameFormatSearchSample;

            DateTime dt = new DateTime(2000, 1, 1, 13, 1, 1);

            lblNameFormatSample.Tag = new TwitData() {
                TwitType = TwitType.Normal,
                UserProtected = true,
                Favorited = true,
                UserName = "Twitter",
                UserScreenName = "twitter",
                Time = dt,
                Source = "web"
            };
            lblNameFormatRTSample.Tag = new TwitData() {
                TwitType = TwitType.Retweet,
                UserScreenName = "twitterapi",
                Time = DateTime.Now,
                Favorited = true,
                RTTwitData = new TwitData() {
                    UserName = "Twitter",
                    UserScreenName = "twitter",
                    Source = "web"
                }
            };
            lblNameFormatDMSample.Tag = new TwitData() {
                TwitType = TwitType.DirectMessage,
                UserName = "Twitter",
                UserScreenName = "twitter",
                DMScreenName = "twitterapi",
                Time = dt,
                Source = "web"
            };
            lblNameFormatSearchSample.Tag = new TwitData() {
                TwitType = TwitType.Search,
                UserScreenName = "twitter",
                Time = dt,
                Source = "web"
            };

            //-------------------------------------------------------------------------------

            btnFontNormalTitle.Tag = txtExNormalTitle;
            btnFontNormalText.Tag = txtExNormalText;
            btnFontReplyToMeTitle.Tag = txtExReplyToMeTitle;
            btnFontReplyToMeText.Tag = txtExReplyToMeText;
            btnFontReplyToOtherTitle.Tag = txtExReplyToOtherTitle;
            btnFontReplyToOtherText.Tag = txtExReplyToOtherText;
            btnFontRTTitle.Tag = txtExRTTitle;
            btnFontRTText.Tag = txtExRTText;

            btnBCNormalSelected.Tag = txtBCNormalSelected;
            btnBCNormalUnSelected.Tag = txtBCNormalUnSelected;
            btnBCReplyToMeSelected.Tag = txtBCReplyToMeSelected;
            btnBCReplyToMeUnSelected.Tag = txtBCReplyToMeUnSelected;
            btnBCReplyToOtherSelected.Tag = txtBCReplyToOtherSelected;
            btnBCReplyToOtherUnSelected.Tag = txtBCReplyToOtherUnSelected;
            btnBCRTSelected.Tag = txtBCRTSelected;
            btnBCRTUnSelected.Tag = txtBCRTUnSelected;

            chbDispIcon.Tag = numIconSize;
            chbDispThumbnail.Tag = numThumbnailInterval;
            chbDispReplyTooltip.Tag = numReplyTooltipDepth;
        }
        #endregion (SetAssociateData)
        //===============================================================================
        #region -SetSettingsData 設定データを設定
        //-------------------------------------------------------------------------------
        //
        private void SetSettingsData()
        {
            #region ■■取得設定■■
            //-------------------------------------------------------------------------------
            numDirect_First.Value = SettingsData.FirstGetNum_Direct;
            numHistory_First.Value = SettingsData.FirstGetNum_History;
            numReply_First.Value = SettingsData.FirstGetNum_Reply;
            numTimeline_First.Value = SettingsData.FirstGetNum_Home;

            numDirect_Add.Value = SettingsData.RenewGetNum_Direct;
            numHistory_Add.Value = SettingsData.RenewGetNum_History;
            numReply_Add.Value = SettingsData.RenewGetNum_Reply;
            numTimeline_Add.Value = SettingsData.RenewGetNum_Home;

            numDirect_Interval.Value = SettingsData.GetInterval_Direct;
            numHistory_Interval.Value = SettingsData.GetInterval_History;
            numReply_Interval.Value = SettingsData.GetInterval_Reply;
            numTimeline_Interval.Value = SettingsData.GetInterval_Home;

            numProfile_Interval.Value = SettingsData.GetInterval_Profile;
            //-------------------------------------------------------------------------------
            #endregion

            #region ■■表示設定■■
            //-------------------------------------------------------------------------------
            txtNameFormat.Text = SettingsData.NameFormat;
            txtNameFormatRT.Text = SettingsData.NameFormatRetweet;
            txtNameFormatDM.Text = SettingsData.NameFormatDM;
            txtNameFormatSearch.Text = SettingsData.NameFormatSearch;

            switch (SettingsData.QuoteType) {
                case QuoteType.QT:
                    rdbQT.Checked = true;
                    break;
                case QuoteType.RT:
                    rdbRT.Checked = true;
                    break;
                case QuoteType.DoubleQuotation:
                    rdb引用符.Checked = true;
                    break;
            }

            chbDispIcon.Checked = SettingsData.DisplayIcon;
            numIconSize.Value = SettingsData.IconSize;
            //-------------------------------------------------------------------------------
            #endregion ■■表示設定■■

            #region ■■フォント・色設定■■
            //-------------------------------------------------------------------------------
            // Font
            txtExNormalTitle.Font = SettingsData.FontNormalTweetTitle;
            txtExNormalText.Font = SettingsData.FontNormalTweetText;
            txtExReplyToMeTitle.Font = SettingsData.FontReplyToMeTweetTitle;
            txtExReplyToMeText.Font = SettingsData.FontReplyToMeTweetText;
            txtExReplyToOtherTitle.Font = SettingsData.FontReplyToOtherTweetTitle;
            txtExReplyToOtherText.Font = SettingsData.FontReplyToOtherTweetText;
            txtExRTTitle.Font = SettingsData.FontRTTweetTitle;
            txtExRTText.Font = SettingsData.FontRTTweetText;

            // ForeColor
            txtExNormalTitle.ForeColor = SettingsData.ColorNormalTweetTitle;
            txtExNormalText.ForeColor = SettingsData.ColorNormalTweetText;
            txtExReplyToMeTitle.ForeColor = SettingsData.ColorReplyToMeTweetTitle;
            txtExReplyToMeText.ForeColor = SettingsData.ColorReplyToMeTweetText;
            txtExReplyToOtherTitle.ForeColor = SettingsData.ColorReplyToOtherTweetTitle;
            txtExReplyToOtherText.ForeColor = SettingsData.ColorReplyToOtherTweetText;
            txtExRTTitle.ForeColor = SettingsData.ColorRTTweetTitle;
            txtExRTText.ForeColor = SettingsData.ColorRTTweetText;

            // BackColor
            txtBCNormalUnSelected.BackColor = SettingsData.ColorNormalTweetBackUnselected;
            txtBCNormalSelected.BackColor = SettingsData.ColorNormalTweetBackSelected;
            txtBCReplyToMeUnSelected.BackColor = SettingsData.ColorReplyToMeTweetBackUnselected;
            txtBCReplyToMeSelected.BackColor = SettingsData.ColorReplyToMeTweetBackSelected;
            txtBCReplyToOtherUnSelected.BackColor = SettingsData.ColorReplyToOtherTweetBackUnselected;
            txtBCReplyToOtherSelected.BackColor = SettingsData.ColorReplyToOtherTweetBackSelected;
            txtBCRTUnSelected.BackColor = SettingsData.ColorRTTweetBackUnselected;
            txtBCRTSelected.BackColor = SettingsData.ColorRTTweetBackSelected;
            //-------------------------------------------------------------------------------
            #endregion ■■フォント・色設定■■

            #region ■■サムネイル設定■■
            //-------------------------------------------------------------------------------
            cmbThumbimgly.SelectedItem = SettingsData.ThumbType_img_ly;
            cmbThumbmovapic.SelectedItem = SettingsData.ThumbType_movapic;
            cmbThumbowly.SelectedItem = SettingsData.ThumbType_ow_ly;
            cmbThumbPhotozou.SelectedItem = SettingsData.ThumbType_photozou;
            cmbThumbplixi.SelectedItem = SettingsData.ThumbType_plixi;
            cmbThumbTwitpic.SelectedItem = SettingsData.ThumbType_twitpic;
            cmbThumbyFrog.SelectedItem = SettingsData.ThumbType_yFrog;
            //-------------------------------------------------------------------------------
            #endregion (■■サムネイル設定■■)

            #region ■■その他設定■■
            //-------------------------------------------------------------------------------
            txtHeader.Text = SettingsData.Header;
            txtFooter.Text = SettingsData.Footer;

            chbDispThumbnail.Checked = SettingsData.DisplayThumbnail;
            numThumbnailInterval.Value = SettingsData.DisplayThumbnailInterval;
            chbDispReplyTooltip.Checked = SettingsData.DisplayReplyToolTip;
            numReplyTooltipDepth.Value = SettingsData.DisplayReplyToolTipDepth;
            chbDispReplyBaloon.Checked = SettingsData.DisplayReplyBaloon;
            chbDispDMBaloon.Checked = SettingsData.DisplayDMBaloon;

            cmbURLShortenType.SelectedItem = SettingsData.URLShortenType;

            chbUseInternalWebBrowser.Checked = SettingsData.UseInternalWebBrowser;
            txtWebBrowserPath.Text = SettingsData.WebBrowserPath;
            //-------------------------------------------------------------------------------
            #endregion ■■その他設定■■
        }
        #endregion (SetSettingsData)
        //===============================================================================
        #region -GetSettingsData 設定データを取得
        //-------------------------------------------------------------------------------
        //
        private void GetSettingsData()
        {
            #region ■■取得設定■■
            //-------------------------------------------------------------------------------
            SettingsData.FirstGetNum_Direct = (int)numDirect_First.Value;
            SettingsData.FirstGetNum_History = (int)numHistory_First.Value;
            SettingsData.FirstGetNum_Reply = (int)numReply_First.Value;
            SettingsData.FirstGetNum_Home = (int)numTimeline_First.Value;

            SettingsData.RenewGetNum_Direct = (int)numDirect_Add.Value;
            SettingsData.RenewGetNum_History = (int)numHistory_Add.Value;
            SettingsData.RenewGetNum_Reply = (int)numReply_Add.Value;
            SettingsData.RenewGetNum_Home = (int)numTimeline_Add.Value;

            SettingsData.GetInterval_Direct = (int)numDirect_Interval.Value;
            SettingsData.GetInterval_History = (int)numHistory_Interval.Value;
            SettingsData.GetInterval_Reply = (int)numReply_Interval.Value;
            SettingsData.GetInterval_Home = (int)numTimeline_Interval.Value;

            SettingsData.GetInterval_Profile = (int)numProfile_Interval.Value;
            //-------------------------------------------------------------------------------
            #endregion ■■取得設定■■

            #region ■■表示設定■■
            //-------------------------------------------------------------------------------
            SettingsData.NameFormat = txtNameFormat.Text;
            SettingsData.NameFormatRetweet = txtNameFormatRT.Text;
            SettingsData.NameFormatDM = txtNameFormatDM.Text;
            SettingsData.NameFormatSearch = txtNameFormatSearch.Text;

            SettingsData.QuoteType = (rdbRT.Checked) ? QuoteType.RT :
                                 (rdb引用符.Checked) ? QuoteType.DoubleQuotation :
                                                       QuoteType.QT;
            SettingsData.DisplayIcon = chbDispIcon.Checked;
            SettingsData.IconSize = (int)numIconSize.Value;
            //-------------------------------------------------------------------------------
            #endregion ■■表示設定■■

            #region ■■フォント・色設定■■
            //-------------------------------------------------------------------------------
            // Font
            SettingsData.FontNormalTweetTitle = txtExNormalTitle.Font;
            SettingsData.FontNormalTweetText = txtExNormalText.Font;
            SettingsData.FontReplyToMeTweetTitle = txtExReplyToMeTitle.Font;
            SettingsData.FontReplyToMeTweetText = txtExReplyToMeText.Font;
            SettingsData.FontReplyToOtherTweetTitle = txtExReplyToOtherTitle.Font;
            SettingsData.FontReplyToOtherTweetText = txtExReplyToOtherText.Font;
            SettingsData.FontRTTweetTitle = txtExRTTitle.Font;
            SettingsData.FontRTTweetText = txtExRTText.Font;

            // ForeColor
            SettingsData.ColorNormalTweetTitle = txtExNormalTitle.ForeColor;
            SettingsData.ColorNormalTweetText = txtExNormalText.ForeColor;
            SettingsData.ColorReplyToMeTweetTitle = txtExReplyToMeTitle.ForeColor;
            SettingsData.ColorReplyToMeTweetText = txtExReplyToMeText.ForeColor;
            SettingsData.ColorReplyToOtherTweetTitle = txtExReplyToOtherTitle.ForeColor;
            SettingsData.ColorReplyToOtherTweetText = txtExReplyToOtherText.ForeColor;
            SettingsData.ColorRTTweetTitle = txtExRTTitle.ForeColor;
            SettingsData.ColorRTTweetText = txtExRTText.ForeColor;

            // BackColor
            SettingsData.ColorNormalTweetBackUnselected = txtBCNormalUnSelected.BackColor;
            SettingsData.ColorNormalTweetBackSelected = txtBCNormalSelected.BackColor;
            SettingsData.ColorReplyToMeTweetBackUnselected = txtBCReplyToMeUnSelected.BackColor;
            SettingsData.ColorReplyToMeTweetBackSelected = txtBCReplyToMeSelected.BackColor;
            SettingsData.ColorReplyToOtherTweetBackUnselected = txtBCReplyToOtherUnSelected.BackColor;
            SettingsData.ColorReplyToOtherTweetBackSelected = txtBCReplyToOtherSelected.BackColor;
            SettingsData.ColorRTTweetBackUnselected = txtBCRTUnSelected.BackColor;
            SettingsData.ColorRTTweetBackSelected = txtBCRTSelected.BackColor;
            //-------------------------------------------------------------------------------
            #endregion ■■フォント・色設定■■

            #region ■■サムネイル設定■■
            //-------------------------------------------------------------------------------
            SettingsData.ThumbType_img_ly = (imglyThumbnailType)cmbThumbimgly.SelectedItem;
            SettingsData.ThumbType_movapic = (movapicThumbnailType)cmbThumbmovapic.SelectedItem;
            SettingsData.ThumbType_ow_ly = (owlyThumbnailType)cmbThumbowly.SelectedItem;
            SettingsData.ThumbType_photozou = (PhotozouThumbnailType)cmbThumbPhotozou.SelectedItem;
            SettingsData.ThumbType_plixi = (plixiThumbnailType)cmbThumbplixi.SelectedItem;
            SettingsData.ThumbType_twitpic = (TwitPicThumbnailType)cmbThumbTwitpic.SelectedItem;
            SettingsData.ThumbType_yFrog = (yFrogThumbnailType)cmbThumbyFrog.SelectedItem;
            //-------------------------------------------------------------------------------
            #endregion (■■サムネイル設定■■)

            #region ■■その他設定■■
            //-------------------------------------------------------------------------------
            SettingsData.Header = txtHeader.Text;
            SettingsData.Footer = txtFooter.Text;

            SettingsData.DisplayThumbnail = chbDispThumbnail.Checked;
            SettingsData.DisplayThumbnailInterval = (int)numThumbnailInterval.Value;
            SettingsData.DisplayReplyToolTip = chbDispReplyTooltip.Checked;
            SettingsData.DisplayReplyToolTipDepth = (int)numReplyTooltipDepth.Value;
            SettingsData.DisplayReplyBaloon = chbDispReplyBaloon.Checked;
            SettingsData.DisplayDMBaloon = chbDispDMBaloon.Checked;

            SettingsData.URLShortenType = (URLShortenType)cmbURLShortenType.SelectedItem;

            SettingsData.UseInternalWebBrowser = chbUseInternalWebBrowser.Checked;
            SettingsData.WebBrowserPath = txtWebBrowserPath.Text;
            //-------------------------------------------------------------------------------
            #endregion ■■その他設定■■
        }
        #endregion (GetSettingsData)

        //-------------------------------------------------------------------------------
        #region -SetComboBoxEnumValue コンボボックスにEnumの値を設定します。
        //-------------------------------------------------------------------------------
        //
        private void SetComboBoxEnumValue(ComboBox combobox, Type enumType)
        {
            if (!enumType.IsEnum) { throw new ArgumentException("enumTypeがEnum型ではありません"); }

            Array array = Enum.GetValues(enumType);
            foreach (var item in array) { combobox.Items.Add(item); }
        }
        #endregion (SetComboBoxEnumValue)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)
    }
}
