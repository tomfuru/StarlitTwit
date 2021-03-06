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
    /// <summary>
    /// フォロワー/フォローしている人を表示するフォームです。
    /// </summary>
    public partial class FrmDispUsers : Form
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        public EFormType FormType { get; private set; }
        /// <summary>FormType=UserFriend,UserFollowerの時に設定しなければならない</summary>
        public string UserScreenName { get; set; }
        /// <summary>FormType=ListMember,ListSubscriberの時に設定しなければならない</summary>
        public string ListID { get; set; }
        /// <summary>FormType=Retweeterの時に設定しなければならない</summary>
        public long RetweetStatusID { get; set; }

        private FrmMain _mainForm = null;
        private List<UserProfile> _profileList = new List<UserProfile>();
        private long _next_cursor = -1;
        private int _page = 1;

        // users/lookupを使う時のユーザーIDデータ
        private long[] _userIDs = null;

        /// <summary>画像リストのラッパ</summary>
        private ImageListWrapper _imageListWrapper = null;
        /// <summary>ロード中画像</summary>
        private Bitmap _loadingimg;
        /// <summary>アニメーション管理クラス</summary>
        private ImageAnimation _imageAnimation;
        // 検索時用
        private TextBox txtSearchWord = null;
        private Button btnSearch = null;
        private string _strSearchWord = "";

        const int SIZE_PER_UPDATE = 100;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispUsers(FrmMain mainForm, ImageListWrapper imgListWrapper, EFormType formtype)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _imageListWrapper = imgListWrapper;
            lstvList.SmallImageList = imgListWrapper.ImageList;
            FormType = formtype;

            UserScreenName = ListID = null;
            RetweetStatusID = -1;

            _loadingimg = (Bitmap)StarlitTwit.Properties.Resources.NowLoadingS.Clone();
            _imageAnimation = new ImageAnimation(_loadingimg);
            _imageAnimation.FrameUpdated += Image_Animate;

        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region EFormType 列挙体：タイプ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォームの表示タイプを指定します。
        /// </summary>
        public enum EFormType : byte
        {
            /// <summary>自分をフォローしているユーザー</summary>
            MyFollower,
            /// <summary>自分がフォローしているユーザー</summary>
            MyFriend,
            /// <summary>他ユーザーをフォローしているユーザー</summary>
            UserFollower,
            /// <summary>他ユーザーがフォローしているユーザー</summary>
            UserFriend,
            /// <summary>リツイートしたユーザー</summary>
            Retweeter,
            /// <summary>リストのメンバー</summary>
            ListMember,
            /// <summary>リストのフォロワー</summary>
            ListSubscriber,
            /// <summary>ブロック中のユーザー</summary>
            MyBlocking,
            /// <summary>ユーザー検索用</summary>
            UserSearch,
            ///// <summary>おすすめユーザー</summary>
            //Suggestion
        }
        //-------------------------------------------------------------------------------
        #endregion (EFormType)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Utilization.SetModelessDialogCenter(this);

            switch (FormType) {
                case EFormType.MyFollower:
                    Text = "フォローされている人";
                    break;
                case EFormType.MyFriend:
                    Text = "フォローしている人";
                    break;
                case EFormType.UserFollower:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    Text = string.Format("{0}をフォローしている人", UserScreenName);
                    break;
                case EFormType.UserFriend:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    Text = string.Format("{0}がフォローしている人", UserScreenName);
                    break;
                case EFormType.Retweeter:
                    Debug.Assert(RetweetStatusID > 0, "UserScreenNameが設定されていない");
                    Text = string.Format("発言ID{0}のリツイーター", RetweetStatusID);
                    break;
                case EFormType.ListMember:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    Debug.Assert(ListID != null, "ListIDが設定されていない");
                    Text = string.Format("{0}のリスト{1}のメンバー", UserScreenName, ListID);
                    break;
                case EFormType.ListSubscriber:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    Debug.Assert(ListID != null, "ListIDが設定されていない");
                    Text = string.Format("{0}のリスト{1}のフォロワー", UserScreenName, ListID);
                    break;
                case EFormType.MyBlocking:
                    Text = "ブロック中のユーザー";
                    break;
                case EFormType.UserSearch:
                    const int MOVEVALUE = 33;
                    Text = "ユーザー検索";
                    this.Height += MOVEVALUE;
                    lstvList.Location = new Point(lstvList.Location.X, lstvList.Location.Y + MOVEVALUE);
                    lstvList.Height -= MOVEVALUE;
                    lblCount.Location = new Point(lblCount.Location.X, lblCount.Location.Y + MOVEVALUE);
                    txtSearchWord = new TextBox() {
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                        Location = new Point(12, 10),
                        Name = "txtSearchWord",
                        Width = 276
                    };
                    btnSearch = new Button() {
                        Anchor = AnchorStyles.Top | AnchorStyles.Right,
                        Enabled = false,
                        Location = new Point(305, 10),
                        Name = "btnSearch",
                        Text = "検索"
                    };
                    txtSearchWord.TextChanged += txtSearchWord_TextChanged;
                    txtSearchWord.KeyDown += txtSearchWord_KeyDown;
                    btnSearch.Click += btnSearch_Click;
                    this.Controls.Add(txtSearchWord);
                    this.Controls.Add(btnSearch);
                    txtSearchWord.Focus();
                    break;
                //case EFormType.Suggestion:
                //    Text = "おすすめユーザー";
                //    break;
            }

            lblCount.Text = "";
            if (FormType != EFormType.UserSearch) {
                tsslabel.Text = "取得中...";
                Utilization.InvokeTransaction(() => GetUsers());
            }
        }
        #endregion (OnLoad)
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
        #region Image_Animate 画像フレームが進んだとき
        //-------------------------------------------------------------------------------
        //
        private void Image_Animate(object sender, EventArgs e)
        {
            try {
                this.Invoke((Action)(() => lstvList.Invalidate()));
            }
            catch (InvalidOperationException) { }
        }
        #endregion (Image_Animate)
        //-------------------------------------------------------------------------------
        #region lstvList_MouseMove マウスオーバー時
        //-------------------------------------------------------------------------------
        private ListViewItem _lastListViewItem = null;
        //
        private void lstvList_MouseMove(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            //マウスポインタのあるアイテムを取得
            ListViewHitTestInfo hi = lstvList.HitTest(e.X, e.Y);
            ListViewItem lvi = hi.Item;
            //ポイントされているアイテムが変わった時
            if (lvi != _lastListViewItem) {
                //アクティブを解除
                if (ttInfo.Active)
                    ttInfo.Active = false;

                if (lvi != null) {
                    //ToolTipのテキストを設定しなおす
                    ttInfo.SetToolTip(lv, GetToolTipText(lvi));
                    //ToolTipを再びアクティブにする
                    ttInfo.Active = true;
                }
                //ポイントされているアイテムを記憶する
                _lastListViewItem = lvi;
            }
        }
        #endregion (lstvList_MouseMove)
        //-------------------------------------------------------------------------------
        #region txtSearchWord_TextChanged 検索テキストボックス変更時
        //-------------------------------------------------------------------------------
        //
        private void txtSearchWord_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = (txtSearchWord.Text.Length > 0);
        }
        #endregion (txtSearchWord_TextChanged)
        //-------------------------------------------------------------------------------
        #region txtSearchWord_KeyDown
        //-------------------------------------------------------------------------------
        //
        private void txtSearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        #endregion (txtSearchWord_KeyDown)
        //-------------------------------------------------------------------------------
        #region btnSearch_Click 検索クリック時
        //-------------------------------------------------------------------------------
        //
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchWord.Enabled = false;
            btnSearch.Enabled = false;
            lstvList.Items.Clear();
            _profileList.Clear();
            _page = 1;
            _strSearchWord = txtSearchWord.Text;
            tsslabel.Text = "検索中...";
            Utilization.InvokeTransaction(() => GetUsers());
        }
        #endregion (btnSearch_Click)
        //-------------------------------------------------------------------------------
        #region menuRow_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void menuRow_Opening(object sender, CancelEventArgs e)
        {
            if (lstvList.SelectedItems.Count == 0) { e.Cancel = true; return; }
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;

            bool isBlocking = (FormType == EFormType.MyBlocking);
            bool isMe = (prof.UserID == FrmMain.Twitter.ID);

            tsmiFollow.Visible = !isMe && !isBlocking && !prof.FolllowRequestSent && !prof.Following;
            tsmiRemove.Visible = !isMe && !isBlocking && !prof.FolllowRequestSent && prof.Following;

            tsmiBlock.Visible = !isMe && (FormType == EFormType.MyFollower);
            tsmiUnblock.Visible = !isMe && isBlocking;

            tsSepUnderFollow.Visible = !isMe && !isBlocking && !prof.FolllowRequestSent;
            tsSepOverBlock.Visible = !isMe && (FormType == EFormType.MyFollower || isBlocking);
        }
        #endregion (menuRow_Opening)
        //-------------------------------------------------------------------------------
        #region tsmiFollow_Click フォロークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiFollow_Click(object sender, EventArgs e)
        {
            if (!FrmMain.SettingsData.ConfirmDialogFollow
             || Message.ShowQuestionMessage("フォローします。") == System.Windows.Forms.DialogResult.Yes) {
                UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
                bool? ret = Utilization.Follow(prof.ScreenName);
                if (!ret.HasValue) {
                    lstvList.SelectedItems[0].SubItems[3].Text = "リクエスト済";
                    ((UserProfile)lstvList.SelectedItems[0].Tag).FolllowRequestSent = true;
                }
                else if (ret.Value) {
                    //Debug.Assert(FormType == FrmFollowType.Follower, "異常な値");
                    lstvList.SelectedItems[0].SubItems[3].Text = "フォロー中";
                    ((UserProfile)lstvList.SelectedItems[0].Tag).Following = true;
                }
            }
        }
        #endregion (tsmiFollow_Click)
        //-------------------------------------------------------------------------------
        #region tsmiRemove_Click フォロー解除クリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiRemove_Click(object sender, EventArgs e)
        {
            if (!FrmMain.SettingsData.ConfirmDialogFollow
             || Message.ShowQuestionMessage("フォローを解除します。") == System.Windows.Forms.DialogResult.Yes) {
                UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
                if (Utilization.RemoveFollow(prof.ScreenName)) {
                    switch (FormType) {
                        case EFormType.MyFollower:
                            lstvList.SelectedItems[0].SubItems[3].Text = "";
                            ((UserProfile)lstvList.SelectedItems[0].Tag).Following = false;
                            break;
                        case EFormType.MyFriend:
                            RemoveSelectedItem();
                            break;
                    }
                }
            }
        }
        #endregion (tsmiRemove_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispFriend_Click フレンドを見るクリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispFriend_Click(object sender, EventArgs e)
        {
            Utilization.ShowUsersForm(_mainForm, _imageListWrapper, EFormType.UserFriend,
                                         ((UserProfile)lstvList.SelectedItems[0].Tag).ScreenName);
        }
        #endregion (tsmiDispFriend_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispFollower_Click フォロワーを見るクリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispFollower_Click(object sender, EventArgs e)
        {
            Utilization.ShowUsersForm(_mainForm, _imageListWrapper, EFormType.UserFollower,
                                         ((UserProfile)lstvList.SelectedItems[0].Tag).ScreenName);
        }
        #endregion (tsmiDispFollower_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplayUserProfile_Click プロフィール表示クリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplayUserProfile_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
            Utilization.ShowProfileForm(_mainForm, false, prof);
        }
        #endregion (tsmiDisplayUserProfile_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplayUserTweet_Click 発言表示クリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplayUserTweet_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
            Utilization.ShowStatusesForm(_mainForm, FrmDispStatuses.EFormType.UserStatus, prof.ScreenName);
        }
        #endregion (tsmiDisplayUserTweet_Click)
        //-------------------------------------------------------------------------------
        #region tsmiOpenBrowserUserHome_Click ホームをブラウザで開くクリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiOpenBrowserUserHome_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;

            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(Twitter.URLtwi);
            sbUrl.Append(prof.ScreenName);

            Utilization.OpenBrowser(sbUrl.ToString(), FrmMain.SettingsData.UseInternalWebBrowser);
        }
        #endregion (tsmiOpenBrowserUserHome_Click)
        //-------------------------------------------------------------------------------
        #region tsmiBlock_Click ブロッククリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiBlock_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
            if (!FrmMain.SettingsData.ConfirmDialogBlock
             || Message.ShowQuestionMessage("ブロックします。") == System.Windows.Forms.DialogResult.Yes) {
                try {
                    FrmMain.Twitter.blocks_create(screen_name: prof.ScreenName);
                }
                catch (TwitterAPIException) { tsslabel.Text = "ブロックに失敗しました。"; return; }
                RemoveSelectedItem();
                tsslabel.Text = "ブロックを行いました。";
            }
        }
        #endregion (tsmiBlock_Click)
        //-------------------------------------------------------------------------------
        #region tsmiUnblock_Click ブロック解除クリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiUnblock_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
            if (!FrmMain.SettingsData.ConfirmDialogBlock
             || Message.ShowQuestionMessage("ブロック解除します。") == System.Windows.Forms.DialogResult.Yes) {
                try {
                    FrmMain.Twitter.blocks_destroy(screen_name: prof.ScreenName);
                }
                catch (TwitterAPIException) { tsslabel.Text = "ブロック解除に失敗しました。"; return; }
                RemoveSelectedItem();
            }
        }
        #endregion (tsmiUnblock_Click)
        //-------------------------------------------------------------------------------
        #region btnAppend_Click 追加取得ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {
            tsslabel.Text = "取得中...";
            btnAppend.Enabled = false;
            if (btnSearch != null) {
                btnSearch.Enabled = false;
                txtSearchWord.Enabled = false;
            }
            Utilization.InvokeTransaction(() => GetUsers());
        }
        #endregion (btnAppend_Click)
        //-------------------------------------------------------------------------------
        #region lstvList_ColumnWidthChanging 列サイズ変更時
        //-------------------------------------------------------------------------------
        //
        private void lstvList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (e.ColumnIndex == 0) { // 画像部分は動かせないようにする
                e.NewWidth = 52;
                e.Cancel = true;
            }
        }
        #endregion (lstvList_ColumnWidthChanging)
        //===============================================================================
        #region lstvList_DrawColumnHeader ヘッダ描画
        //-------------------------------------------------------------------------------
        //
        private void lstvList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }
        #endregion (lstvList_DrawColumnHeader)
        //-------------------------------------------------------------------------------
        #region lstvList_DrawSubItem アイテム描画
        //-------------------------------------------------------------------------------
        //
        private void lstvList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex > 0) { e.DrawDefault = true; return; }
            e.DrawBackground();
            Image img = _imageListWrapper.GetImage(_profileList[e.ItemIndex].IconURL);
            if (img != null) {
                e.Graphics.DrawImage(img, e.Bounds.Location);
            }
            else if (_imageAnimation != null) {
                e.Graphics.DrawImage(_imageAnimation.Image, e.Bounds.Location);
            }
        }
        #endregion (lstvList_DrawSubItem)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //-------------------------------------------------------------------------------
        #region メソッド
        //-------------------------------------------------------------------------------
        #region -RemoveSelectedItem 選択中項目を消去します
        //-------------------------------------------------------------------------------
        //
        private void RemoveSelectedItem()
        {
            _profileList.RemoveAt(lstvList.SelectedIndices[0]);
            lstvList.Items.Remove(lstvList.SelectedItems[0]);
        }
        #endregion (RemoveSelectedItem)
        //-------------------------------------------------------------------------------
        #region -GetToolTipText ツールチップのテキストを取得します。
        //-------------------------------------------------------------------------------
        //
        private string GetToolTipText(ListViewItem item)
        {
            int index = lstvList.Items.IndexOf(item);
            if (index < 0) { return ""; }

            UserProfile p = _profileList[index];
            return Utilization.GetProfileDescriptionString(p);
        }
        #endregion (GetToolTipText)
        //-------------------------------------------------------------------------------
        #region -AddList リストに追加
        //-------------------------------------------------------------------------------
        //
        private void AddList(IEnumerable<UserProfile> profiles)
        {
            List<Tuple<ListViewItem, string>> urllist = new List<Tuple<ListViewItem, string>>();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (var p in profiles) {
                if (_profileList.Exists(prof => prof.UserID == p.UserID)) { continue; } // 重複防止
                ListViewItem item = new ListViewItem();
                item.Tag = p;
                if (!_imageListWrapper.ImageContainsKey(p.IconURL)) { urllist.Add(new Tuple<ListViewItem, string>(item, p.IconURL)); }
                ListViewItem.ListViewSubItem si = new ListViewItem.ListViewSubItem();
                item.SubItems.Add(p.ScreenName);
                item.SubItems.Add(p.UserName);

                item.SubItems.Add((p.Following) ? "フォロー中" :
                                  (p.FolllowRequestSent) ? "リクエスト済" :
                                  (p.UserID == FrmMain.Twitter.ID) ? "自分" : "");

                items.Add(item);

                _profileList.Add(p);
            }
            lstvList.Items.AddRange(items.ToArray());

            if (urllist.Count > 0) {
                Utilization.InvokeTransaction(() => GetImages(urllist));
            }
            //lstvList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        #endregion (AddList)
        //-------------------------------------------------------------------------------
        #region -GetUsers ユーザー取得 using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void GetUsers()
        {
            try {
                try {
                    IEnumerable<UserProfile> profiles = null;
                    if (FormType == EFormType.UserSearch) {
                        profiles = FrmMain.Twitter.users_search(_strSearchWord, count: 20, page: _page);
                        _page++;
                        this.Invoke((Action)(() => btnAppend.Enabled = (profiles.Any(prof => _profileList.All(lprof => lprof.UserID != prof.UserID)))));
                    }
                    else {
                        bool appendEnable = false;
                        switch (FormType) {
                            case EFormType.MyFollower: {
                                    if (_userIDs == null || _page == _userIDs.Length) {
                                        var data = FrmMain.Twitter.followers_ids(user_id: FrmMain.Twitter.ID, cursor: _next_cursor);
                                        _next_cursor = data.NextCursor;
                                        _userIDs = data.Data.ToArray();
                                        _page = 0;
                                        Thread.Sleep(100);
                                    }
                                    int index = _page;
                                    long[] ids = Utilization.SliceArray(_userIDs, ref index, SIZE_PER_UPDATE);
                                    profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                                    _page = index;
                                    appendEnable = (_next_cursor != 0 || _page < _userIDs.Length);
                                    //proftpl = FrmMain.Twitter.statuses_followers(cursor: _next_cursor);
                                }
                                break;
                            case EFormType.MyFriend: {
                                    if (_userIDs == null || _page == _userIDs.Length) {
                                        var data = FrmMain.Twitter.friends_ids(user_id: FrmMain.Twitter.ID, cursor: _next_cursor);
                                        _next_cursor = data.NextCursor;
                                        _userIDs = data.Data.ToArray();
                                        _page = 0;
                                        Thread.Sleep(100);
                                    }
                                    int index = _page;
                                    long[] ids = Utilization.SliceArray(_userIDs, ref index, SIZE_PER_UPDATE);
                                    profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                                    _page = index;
                                    appendEnable = (_next_cursor != 0 || _page < _userIDs.Length);
                                    //proftpl = FrmMain.Twitter.statuses_friends(cursor: _next_cursor);
                                }
                                break;
                            case EFormType.UserFollower: {
                                    if (_userIDs == null || _page == _userIDs.Length) {
                                        var data = FrmMain.Twitter.followers_ids(screen_name: UserScreenName, cursor: _next_cursor);
                                        _next_cursor = data.NextCursor;
                                        _userIDs = data.Data.ToArray();
                                        _page = 0;
                                        Thread.Sleep(100);
                                    }
                                    int index = _page;
                                    long[] ids = Utilization.SliceArray(_userIDs, ref index, SIZE_PER_UPDATE);
                                    profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                                    _page = index;
                                    appendEnable = (_next_cursor != 0 || _page < _userIDs.Length);
                                    //proftpl = FrmMain.Twitter.statuses_followers(screen_name: UserScreenName, cursor: _next_cursor);
                                }
                                break;
                            case EFormType.UserFriend: {
                                    if (_userIDs == null || _page == _userIDs.Length) {
                                        var data = FrmMain.Twitter.friends_ids(screen_name: UserScreenName, cursor: _next_cursor);
                                        _next_cursor = data.NextCursor;
                                        _userIDs = data.Data.ToArray();
                                        _page = 0;
                                        Thread.Sleep(100);
                                    }
                                    int index = _page;
                                    long[] ids = Utilization.SliceArray(_userIDs, ref index, SIZE_PER_UPDATE);
                                    profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                                    _page = index;
                                    appendEnable = (_next_cursor != 0 || _page < _userIDs.Length);
                                    //proftpl = FrmMain.Twitter.statuses_friends(screen_name: UserScreenName, cursor: _next_cursor);
                                }
                                break;
                            case EFormType.ListMember: {
                                    SequentData<UserProfile> proftpl;
                                    proftpl = FrmMain.Twitter.lists_members(slug: ListID, owner_screen_name: UserScreenName, cursor: _next_cursor);
                                    profiles = proftpl.Data;
                                    _next_cursor = proftpl.NextCursor;
                                    appendEnable = (_next_cursor != 0);
                                }
                                break;
                            case EFormType.ListSubscriber: {
                                    SequentData<UserProfile> proftpl;
                                    proftpl = FrmMain.Twitter.lists_subscribers(slug: ListID, owner_screen_name: UserScreenName, cursor: _next_cursor);
                                    profiles = proftpl.Data;
                                    _next_cursor = proftpl.NextCursor;
                                    appendEnable = (_next_cursor != 0);
                                }
                                break;
                            case EFormType.MyBlocking: {
                                    SequentData<UserProfile> proftpl;
                                    proftpl = FrmMain.Twitter.blocks_list(cursor: _next_cursor);
                                    profiles = proftpl.Data;
                                    _next_cursor = proftpl.NextCursor;
                                    appendEnable = (_next_cursor != 0);
                                }
                                break;
                            case EFormType.Retweeter: {
                                    var ids = FrmMain.Twitter.statuses_retweeters_ids(RetweetStatusID, cursor: _next_cursor);
                                    var ids_data = ids.Data.ToArray();
                                    if (ids_data.Length > 0) {
                                        profiles = FrmMain.Twitter.users_lookup(user_id: ids_data);
                                    }
                                    _next_cursor = ids.NextCursor;
                                    appendEnable = (_next_cursor != 0);
                                }
                                break;
                        }
                        this.Invoke((Action)(() => btnAppend.Enabled = appendEnable));
                    }

                    if (profiles != null) {
                        this.Invoke((Action)(() =>
                        {
                            AddList(profiles);
                            lblCount.Text = string.Format("{0}人見つかりました", _profileList.Count);
                            tsslabel.Text = (btnAppend.Enabled) ? "取得完了しました" : "全て取得完了しました";
                        }));
                    }
                }
                catch (TwitterAPIException) {
                    this.Invoke((Action)(() =>
                    {
                        tsslabel.Text = "取得に失敗しました。";
                        btnAppend.Enabled = true;
                    }));
                }
                finally {
                    this.Invoke((Action)(() =>
                    {
                        if (btnSearch != null) {
                            btnSearch.Enabled = true;
                            txtSearchWord.Enabled = true;
                        }
                    }));
                }
            }
            catch (InvalidOperationException) { }
        }
        #endregion (GetUsers)

        //-------------------------------------------------------------------------------
        #region -GetImages 画像取得と追加 (別スレッド処理)
        //-------------------------------------------------------------------------------
        //
        private void GetImages(IEnumerable<Tuple<ListViewItem, string>> data)
        {
            try {
                _imageAnimation.StartAnimation();
                foreach (var d in data) {
                    Image img = Utilization.GetImageFromURL(d.Item2);
                    if (img != null) {
                        _imageListWrapper.ImageAdd(d.Item2, img);
                        this.Invoke((Action)(() => Refresh()));
                    }
                    else {
                        d.Item1.ImageKey = FrmMain.STR_IMAGE_CROSS;
                    }
                }
            }
            catch (InvalidOperationException) { }
            finally {
                try {
                    _imageAnimation.StopAnimation();
                }
                catch (InvalidOperationException) { }
            }
        }
        #endregion (GetImages)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)

        #region Comment Out
        ////-------------------------------------------------------------------------------
        //#region -ResetImageKey 画像キーを再設定して画像を表示させます
        ////-------------------------------------------------------------------------------
        ////
        //private void ResetImageKey(ListViewItem item)
        //{
        //    item.ImageKey = item.ImageKey;
        //}
        //#endregion (ResetImageKey)
        ////-------------------------------------------------------------------------------
        #endregion (Comment Out)
    }
}
