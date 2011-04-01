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
    public partial class FrmProfile : Form
    {
        private UserProfile _profile;

        /// <summary>編集可能かどうか</summary>
        public bool CanEdit { get; private set; }
        /// <summary>プロフィールのユーザー名</summary>
        public string ScreenName { get { return (_profile != null) ? _profile.ScreenName : null; } }
        // 変更確認用
        string _bakName, _bakLoc, _bakUrl, _bakDesc;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmProfile(bool canEdit, UserProfile profile, ImageListWrapper imagelistwrapper)
        {
            InitializeComponent();
            CanEdit = canEdit;
            if (!canEdit) {
                rtxtDescription.ReadOnly = txtLocation.ReadOnly = txtName.ReadOnly = txtUrl.ReadOnly = true;
                lblDescriptionRest.Visible = btnRenew.Visible = false;
                txtUrl.Visible = false;
            }
            else {
                llblWeb.Visible = false;
            }
            _profile = profile;
            picbIcon.ImageListWrapper = imagelistwrapper;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region FrmProfile_Load ロード時イベント
        //-------------------------------------------------------------------------------
        //
        private void FrmProfile_Load(object sender, EventArgs e)
        {
            Utilization.SetModelessDialogCenter(this);
            SetProfile(_profile);
            if (CanEdit) { SaveProfileTemporary(); }
            txtName.Select(0, 0);
        }
        #endregion (FrmProfile_Load)

        //-------------------------------------------------------------------------------
        #region FrmProfile_FormClosing クローズ中イベント
        //-------------------------------------------------------------------------------
        //
        private void FrmProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CanEdit && (_bakName != txtName.Text
                            || _bakLoc != txtLocation.Text
                            || _bakUrl != txtUrl.Text
                            || _bakDesc != rtxtDescription.Text)) {
                if (Message.ShowQuestionMessage("プロフィールが変更されています。画面を閉じてよろしいですか？") == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }
        #endregion (FrmProfile_FormClosing)

        //-------------------------------------------------------------------------------
        #region btnRenew_Click 更新ボタン using Twitter API
        //-------------------------------------------------------------------------------
        //
        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (Message.ShowQuestionMessage("プロフィールを更新します。よろしいですか？") == DialogResult.Yes) {
                try {
                    FrmMain.Twitter.account_update_profile(txtName.Text, txtUrl.Text, txtLocation.Text, rtxtDescription.Text);
                    SaveProfileTemporary();
                    Message.ShowInfoMessage("プロフィールを更新しました。");
                }
                catch (TwitterAPIException) {
                    Message.ShowErrorMessage("プロフィールの更新に失敗しました。");
                }
            }
        }
        #endregion (btnRenew_Click)

        //-------------------------------------------------------------------------------
        #region llblWeb_LinkClicked リンククリック時
        //-------------------------------------------------------------------------------
        //
        private void llblWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilization.OpenBrowser(llblWeb.Text, FrmMain.SettingsData.UseInternalWebBrowser);
        }
        #endregion (llblWeb_LinkClicked)

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
        #region rtxtDescription_TextChanged テキスト変更
        //-------------------------------------------------------------------------------
        //
        private void rtxtDescription_TextChanged(object sender, EventArgs e)
        {
            const string FORMAT = "あと{0}文字";
            const int MAX_DESCRIPTION_LENGTH = 160;
            lblDescriptionRest.Text = string.Format(FORMAT, MAX_DESCRIPTION_LENGTH - rtxtDescription.TextLength);
        }
        #endregion (rtxtDescription_TextChanged)

        //-------------------------------------------------------------------------------
        #region -SetProfile プロフィールセット
        //-------------------------------------------------------------------------------
        //
        private void SetProfile(UserProfile profile)
        {
            this.Text = string.Format("{0}のプロフィール", profile.ScreenName);

            if (!picbIcon.ImageListWrapper.ImageContainsKey(profile.IconURL)) {
                picbIcon.ImageListWrapper.RequestAddImages(new string[] { profile.IconURL });
            }
            picbIcon.SetFromImageListWrapper(profile.IconURL);

            lblProtected.Visible = profile.Protected;
            lblFollow.Visible = profile.Following;

            lblFollowerNum.Text = profile.FollowerNum.ToString();
            lblFollowingNum.Text = profile.FollowingNum.ToString();
            lblFavoriteNum.Text = profile.FavoriteNum.ToString();
            lblStatusNum.Text = profile.StatusNum.ToString();
            lblListedNum.Text = profile.ListedNum.ToString();

            lblTimeZone.Text = profile.TimeZone;

            lblUserID.Text = profile.UserID.ToString();
            lblRegisterTime.Text = profile.RegisterTime.ToString(Utilization.STR_DATETIMEFORMAT);
            lblScreenName.Text = profile.ScreenName;
            txtName.Text = profile.UserName;
            txtLocation.Text = profile.Location;
            txtUrl.Text = llblWeb.Text = profile.URL;
            rtxtDescription.Text = profile.Description;

            if (profile.LastTwitData != null) {
                lblLastStatusTime.Text = string.Format("({0})", profile.LastTwitData.Time.ToString(Utilization.STR_DATETIMEFORMAT));
                lblLastStatus.Text = profile.LastTwitData.Text;
            }
            else {
                lblLastStatusTime.Visible = false;
                lblLastStatus.Text = "(無し)";
            }
        }
        #endregion (SetProfile)

        //-------------------------------------------------------------------------------
        #region -SaveProfileTemporary 一時的なプロフィール保存
        //-------------------------------------------------------------------------------
        //
        private void SaveProfileTemporary()
        {
            _bakName = txtName.Text;
            _bakLoc = txtLocation.Text;
            _bakUrl = txtUrl.Text;
            _bakDesc = rtxtDescription.Text;
        }
        #endregion (SaveProfileTemporary)
    }
}
