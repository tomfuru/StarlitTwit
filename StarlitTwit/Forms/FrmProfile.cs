using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace StarlitTwit
{
    public partial class FrmProfile : Form
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>メインフォーム</summary>
        public FrmMain _mainForm;
        /// <summary>表示プロフィール</summary>
        private UserProfile _profile;
        /// <summary>関係データ</summary>
        private RelationshipData _relation;
        /// <summary>編集可能かどうか</summary>
        public bool CanEdit { get; private set; }
        /// <summary>プロフィールのユーザー名</summary>
        public string ScreenName { get; private set; }
        /// <summary>プロフィールデータを取得済みか</summary>
        bool _gotProfile = false;
        // 変更確認用
        string _bakName, _bakLoc, _bakUrl, _bakDesc;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmProfile(FrmMain mainForm, bool canEdit, UserProfile profile, ImageListWrapper imagelistwrapper)
            : this(mainForm, canEdit, imagelistwrapper)
        {
            ScreenName = profile.ScreenName;
            _profile = profile;
            SetProfile(_profile);
            _gotProfile = true;
        }
        public FrmProfile(FrmMain mainForm, bool canEdit, string screen_name, ImageListWrapper imagelistwrapper)
            : this(mainForm, canEdit, imagelistwrapper)
        {
            ScreenName = screen_name;
            _profile = null;
            _gotProfile = false;
        }
        private FrmProfile(FrmMain mainForm, bool canEdit, ImageListWrapper imagelistwrapper)
        {
            InitializeComponent();
            _mainForm = mainForm;
            CanEdit = canEdit;
            tsslabel.Text = "";
            if (!canEdit) {
                rtxtDescription.ReadOnly = txtLocation.ReadOnly = txtName.ReadOnly = txtUrl.ReadOnly = true;
                btnImageChange.Visible = lblDescriptionRest.Visible = btnRenew.Visible = false;
                txtUrl.Visible = false;
            }
            else {
                llblWeb.Visible = false;
                lblFollowing_title.Visible = lblFollowed_title.Visible =
                lblFollowing.Visible = lblFollowed.Visible = false;
                tsmiOperation_Follow.Visible = tsmiOperation_UnFollow.Visible =
                tsmiOperation_Block.Visible = tsmiOperation_UnBlock.Visible =
                tsmiOperation_MakeUserTab.Visible = tsmSep_Op1.Visible = tsmSep_Op2.Visible = false;
            }

            picbIcon.ImageListWrapper = imagelistwrapper;

            ScreenName = null;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時イベント
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Utilization.SetModelessDialogCenter(this);

            Debug.Assert(!string.IsNullOrEmpty(ScreenName));
            this.Text = string.Format("{0}のプロフィール", ScreenName);

            Utilization.InvokeTransaction(GetData);

            txtName.Select(0, 0);
        }
        #endregion (OnLoad)

        //-------------------------------------------------------------------------------
        #region #[override]OnFormClosing クローズ中イベント
        //-------------------------------------------------------------------------------
        //
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (CanEdit && (_bakName != txtName.Text
                            || _bakLoc != txtLocation.Text
                            || _bakUrl != txtUrl.Text
                            || _bakDesc != rtxtDescription.Text)) {
                if (Message.ShowQuestionMessage("プロフィールが変更されています。画面を閉じてよろしいですか？") == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }
        #endregion (OnFormClosing)

        //-------------------------------------------------------------------------------
        #region btnImageChange_Click (画像)変更ボタンクリック
        //-------------------------------------------------------------------------------
        //
        private void btnImageChange_Click(object sender, EventArgs e)
        {
            using (FrmRenewProfileImage frm = new FrmRenewProfileImage()) {
                frm.ShowDialog(this);
            }
        }
        #endregion (btnImageChange_Click)

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
        #region メニュー
        //-------------------------------------------------------------------------------
        #region tsmiOperation_Follow_Click フォロー
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_Follow_Click(object sender, EventArgs e)
        {
            UserProfile profile;
            bool? ret = Utilization.Follow(_profile.ScreenName, out profile);
            if (!ret.HasValue) {
                _profile = profile;
                SetProfile(_profile);
            }
            else if (ret.Value) {
                _profile = profile;
                SetProfile(_profile);
            }
        }
        #endregion (tsmiOperation_Follow_Clic)
        //-------------------------------------------------------------------------------
        #region tsmiOperation_UnFollow_Click フォロー解除
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_UnFollow_Click(object sender, EventArgs e)
        {
            UserProfile profile;
            if (Utilization.RemoveFollow(_profile.ScreenName, out profile)) {
                _profile = profile;
                SetProfile(_profile);
            }
        }
        #endregion (tsmiOperation_UnFollow_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOperation_MakeUserTab_Click ユーザータブ作成
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_MakeUserTab_Click(object sender, EventArgs e)
        {
            _mainForm.MakeNewTab(TabSearchType.User, _profile.ScreenName);
        }
        #endregion (tsmiOperation_MakeUserTab_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOperation_List_Click リストに追加・削除
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_List_Click(object sender, EventArgs e)
        {
            using (FrmListOfUser frm = new FrmListOfUser(_mainForm, _profile.ScreenName)) {
                frm.ShowDialog(this);
            }
        }
        #endregion (tsmiOperation_List_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOperation_Block_Click ブロック
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_Block_Click(object sender, EventArgs e)
        {
            if (!FrmMain.SettingsData.ConfirmDialogBlock
             || Message.ShowQuestionMessage("ブロックします。") == System.Windows.Forms.DialogResult.Yes) {
                try {
                    FrmMain.Twitter.blocks_create(screen_name: _profile.ScreenName);
                }
                catch (TwitterAPIException) { return; }
            }
        }
        #endregion (tsmiOperation_Block_Click)
        //-------------------------------------------------------------------------------
        #region smiOperation_UnBlock_Click ブロック解除
        //-------------------------------------------------------------------------------
        //
        private void tsmiOperation_UnBlock_Click(object sender, EventArgs e)
        {
            if (!FrmMain.SettingsData.ConfirmDialogBlock
             || Message.ShowQuestionMessage("ブロック解除します。") == System.Windows.Forms.DialogResult.Yes) {
                try {
                    FrmMain.Twitter.blocks_destroy(screen_name: _profile.ScreenName);
                }
                catch (TwitterAPIException) { return; }
            }
        }
        #endregion (smiOperation_UnBlock_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_Friends_Click フレンド一覧表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_Friends_Click(object sender, EventArgs e)
        {
            FrmDispUsers.EFormType formType;
            string username = null;
            if (CanEdit) {
                formType = FrmDispUsers.EFormType.MyFriend;
            }
            else {
                formType = FrmDispUsers.EFormType.UserFriend;
                username = _profile.ScreenName;
            }

            Utilization.ShowUsersForm(_mainForm, picbIcon.ImageListWrapper, formType, username);
        }
        #endregion (tsmiDisplay_Friends_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_Follower_Click フォロワー一覧表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_Follower_Click(object sender, EventArgs e)
        {
            FrmDispUsers.EFormType formType;
            string username = null;
            if (CanEdit) {
                formType = FrmDispUsers.EFormType.MyFollower;
            }
            else {
                formType = FrmDispUsers.EFormType.UserFollower;
                username = _profile.ScreenName;
            }

            Utilization.ShowUsersForm(_mainForm, picbIcon.ImageListWrapper, formType, username);
        }
        #endregion (tsmiDisplay_Follower_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_Statuses_Click 最近の発言表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_Statuses_Click(object sender, EventArgs e)
        {
            Utilization.ShowStatusesForm(_mainForm, FrmDispStatuses.EFormType.UserStatus, _profile.ScreenName);
        }
        #endregion (tsmiDisplay_Statuses_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_Favorites_Click お気に入り表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_Favorites_Click(object sender, EventArgs e)
        {
            Utilization.ShowStatusesForm(_mainForm, FrmDispStatuses.EFormType.UserFavorite, _profile.ScreenName);
        }
        #endregion (tsmiDisplay_Favorites_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_OwnList_Click 所有リスト表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_OwnList_Click(object sender, EventArgs e)
        {
            FrmDispLists.EFormType formType = (CanEdit) ? FrmDispLists.EFormType.MyList : FrmDispLists.EFormType.UserList;

            Utilization.ShowListsForm(_mainForm, picbIcon.ImageListWrapper, formType, _profile.ScreenName);
        }
        #endregion (tsmiDisplay_OwnList_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_BelongList_Click 所属リスト表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_BelongList_Click(object sender, EventArgs e)
        {
            FrmDispLists.EFormType formType = (CanEdit) ? FrmDispLists.EFormType.MyBelongedList : FrmDispLists.EFormType.UserBelongedList;

            Utilization.ShowListsForm(_mainForm, picbIcon.ImageListWrapper, formType, _profile.ScreenName);
        }
        #endregion (tsmiDisplay_BelongList_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplay_SubscriptList_Click フォローリスト表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplay_SubscriptList_Click(object sender, EventArgs e)
        {
            FrmDispLists.EFormType formType = (CanEdit) ? FrmDispLists.EFormType.MySubscribingList : FrmDispLists.EFormType.UserSubscribingList;

            Utilization.ShowListsForm(_mainForm, picbIcon.ImageListWrapper, formType, _profile.ScreenName);
        }
        #endregion (tsmiDisplay_SubscriptList_Click)

        //-------------------------------------------------------------------------------
        #region tsmiRenew_Click 更新メニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiRenew_Click(object sender, EventArgs e)
        {
            _gotProfile = false;
            btnImageChange.Enabled = btnRenew.Enabled = false;
            Utilization.InvokeTransaction(GetData);
        }
        #endregion (tsmiRenew_Click)
        //-------------------------------------------------------------------------------
        #endregion (メニュー)


        //-------------------------------------------------------------------------------
        #region -GetData 必要データ取得 using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void GetData()
        {
            try {
                if (!_gotProfile) {
                    this.Invoke(new Action(() => tsslabel.Text = "プロフィール取得中..."));
                    _profile = FrmMain.Twitter.users_show(screen_name: ScreenName);
                    this.Invoke(new Action(() => SetProfile(_profile)));
                    _gotProfile = true;
                }

                if (!FrmMain.Twitter.ScreenName.Equals(ScreenName)) {
                    this.Invoke(new Action(() => tsslabel.Text = "関係データ取得中..."));
                    _relation = FrmMain.Twitter.friendships_show(FrmMain.Twitter.ID, target_screen_name: ScreenName);
                    this.Invoke(new Action(() => SetRelationshipData(_relation)));
                }
                this.Invoke(new Action(() =>
                    {
                        btnRetry.Enabled = false;
                        btnImageChange.Enabled = btnRenew.Enabled = true;
                        tsslabel.Text = "取得完了しました。";
                    }));
            }
            catch (TwitterAPIException ex) {
                this.Invoke(new Action(() =>
                {
                    tsslabel.Text = Utilization.SubTwitterAPIExceptionStr(ex);
                    btnRetry.Enabled = true;
                }));
            }
        }
        #endregion (-GetData)

        //-------------------------------------------------------------------------------
        #region -SetProfile プロフィールセット
        //-------------------------------------------------------------------------------
        //
        private void SetProfile(UserProfile profile)
        {
            if (!picbIcon.ImageListWrapper.ImageContainsKey(profile.IconURL)) {
                picbIcon.ImageListWrapper.RequestAddImages(new string[] { profile.IconURL });
            }
            picbIcon.SetFromImageListWrapper(profile.IconURL);

            lblProtected.Visible = profile.Protected;
            lblFollowing.Text = GetFollowingOrNotText(profile.Following);

            lblFollowerNum.Text = profile.FollowerNum.ToString();
            lblFriendNum.Text = profile.FriendNum.ToString();
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

            // メニュー設定
            tsmiOperation_Follow.Visible = !CanEdit && !(tsmiOperation_UnFollow.Visible = profile.Following);

            // データ一時保存
            if (CanEdit) { SaveProfileTemporary(); }
        }
        #endregion (SetProfile)
        //-------------------------------------------------------------------------------
        #region -SetRelationshipData 関係データセット
        //-------------------------------------------------------------------------------
        //
        private void SetRelationshipData(RelationshipData relation)
        {
            //lblBlocking.Visible = relation.Blocking; // friendships_showのblockingが正しい値を返したら有効に
            lblFollowing.Text = GetFollowingOrNotText(relation.Following);
            lblFollowed.Text = GetFollowingOrNotText(relation.Followed);
        }
        #endregion (SetRelationshipData)

        //-------------------------------------------------------------------------------
        #region -GetFollowingOrNotText 有無を表すテキスト取得
        //-------------------------------------------------------------------------------
        //
        private string GetFollowingOrNotText(bool following)
        {
            return (following) ? "○" : "×";
        }
        #endregion (GetFollowingOrNotText)

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
