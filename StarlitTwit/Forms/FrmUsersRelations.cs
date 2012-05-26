using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace StarlitTwit
{
    public partial class FrmUsersRelations : Form
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        private string[] _userNames = null;

        private long[] _follower_ids = null;
        private int _next_follower = 0;
        private bool _all_read_follower = false;
        private List<UserProfile> _profile_followers = new List<UserProfile>();

        private long[] _friend_ids = null;
        private int _next_friend = 0;
        private bool _all_read_friend = false;
        private List<UserProfile> _profile_friends = new List<UserProfile>();

        private object _lock_displayrelationbtn = new object();

        private Image _loadingimg;
        private ImageListWrapper _imageListWrapper;
        private ImageAnimation _imageAnimation;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmUsersRelations(ImageListWrapper imgListWrapper)
        {
            InitializeComponent();

            _imageListWrapper = imgListWrapper;
            lstvCommonFollower.SmallImageList = imgListWrapper.ImageList;
            lstvCommonFriend.SmallImageList = imgListWrapper.ImageList;

            tsslabel.Text = "";
            userSelector.Notifier = NotifierMethod;

            _loadingimg = (Bitmap)StarlitTwit.Properties.Resources.NowLoadingS.Clone();
            _imageAnimation = new ImageAnimation(_loadingimg);
            _imageAnimation.FrameUpdated += Image_Animate;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Utilization.SetModelessDialogCenter(this);
        }
        #endregion (OnLoad)

        //-------------------------------------------------------------------------------
        #region Image_Animate 画像フレームが進んだとき
        //-------------------------------------------------------------------------------
        //
        private void Image_Animate(object sender, EventArgs e)
        {
            try {
                this.Invoke((Action)(() => {
                    lstvCommonFollower.Invalidate();
                    lstvCommonFriend.Invalidate();
                }));
            }
            catch (InvalidOperationException) { }
        }
        #endregion (Image_Animate)
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
        //-------------------------------------------------------------------------------
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
            List<UserProfile> profileList = (sender == lstvCommonFollower) ? _profile_followers : _profile_friends;
            Image img = _imageListWrapper.GetImage(profileList[e.ItemIndex].IconURL);
            if (img != null) {
                e.Graphics.DrawImage(img, e.Bounds.Location);
            }
            else if (_imageAnimation != null) {
                e.Graphics.DrawImage(_imageAnimation.Image, e.Bounds.Location);
            }
        }
        #endregion (lstvList_DrawSubItem)
        //-------------------------------------------------------------------------------
        #region lstvList_MouseMove マウスオーバー時
        //-------------------------------------------------------------------------------
        private ListViewItem _lastListViewItem = null;
        //
        private void lstvList_MouseMove(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            //マウスポインタのあるアイテムを取得
            ListViewHitTestInfo hi = lv.HitTest(e.X, e.Y);
            ListViewItem lvi = hi.Item;
            //ポイントされているアイテムが変わった時
            if (lvi != _lastListViewItem) {
                //アクティブを解除
                if (ttInfo.Active)
                    ttInfo.Active = false;

                if (lvi != null) {
                    //ToolTipのテキストを設定しなおす
                    List<UserProfile> profileList = (lv == lstvCommonFollower) ? _profile_followers : _profile_friends;
                    ttInfo.SetToolTip(lv, GetToolTipText(lv, profileList, lvi));
                    //ToolTipを再びアクティブにする
                    ttInfo.Active = true;
                }
                //ポイントされているアイテムを記憶する
                _lastListViewItem = lvi;
            }
        }
        #endregion (lstvList_MouseMove)

        //-------------------------------------------------------------------------------
        #region NotifierMethod 通知メソッド中身
        //-------------------------------------------------------------------------------
        //
        private void NotifierMethod(string str)
        {
            tsslabel.Text = str;
        }
        #endregion (NotifierMethod)

        //-------------------------------------------------------------------------------
        #region userSelector_SelectedUserNamesChanging
        //-------------------------------------------------------------------------------
        private void userSelector_SelectedUserNamesChanging(object sender, SelectedUserNamesChangingEventArgs e)
        {
            btnDisplayRelations.Enabled = (e.SelectedItemsNum > 1);
        }
        //-------------------------------------------------------------------------------
        #endregion (userSelector_SelectedUserNamesChanging)

        //-------------------------------------------------------------------------------
        #region btnDisplayRelations_Click 関係表示ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnDisplayRelations_Click(object sender, EventArgs e)
        {
            btnAppendFollower.Enabled = btnAppendFriends.Enabled = false;
            btnDisplayRelations.Enabled = false;

            _all_read_follower = _all_read_friend = false;
            _follower_ids = _friend_ids = null;
            _next_follower = _next_friend = 0;
            _profile_followers.Clear();
            _profile_friends.Clear();
            lstvCommonFollower.Items.Clear();
            lstvCommonFriend.Items.Clear();
            
            _userNames = userSelector.SelectedUserNames.ToArray();

            Utilization.InvokeTransaction(() =>
                {
                    try {
                        this.Invoke((Action)(() => tsslabel.Text = "フォロワーデータ取得中..."));
                        GetCommonFollowers();
                        this.Invoke((Action)(() => lblCommonFollowerNum.Text = _follower_ids.Length.ToString()));
                        AddFollowerProfiles();
                        this.Invoke((Action)(() => tsslabel.Text = "フレンドデータ取得中..."));
                        GetCommonFriends();
                        this.Invoke((Action)(() => lblCommonFriendsNum.Text = _friend_ids.Length.ToString()));
                        AddFriendProfiles();
                        this.Invoke((Action)(() => 
                            {
                                tsslabel.Text = "データ取得完了しました"; 
                                _all_read_follower = (_follower_ids.Length == _next_follower);
                                btnAppendFollower.Enabled = !_all_read_follower;
                                _all_read_friend = (_friend_ids.Length == _next_follower);
                                btnAppendFriends.Enabled = !_all_read_friend;
                                btnDisplayRelations.Enabled = true;
                            }));
                    }
                    catch (InvalidOperationException) { }
                });
        }
        #endregion (btnDisplayRelations_Click)

        //-------------------------------------------------------------------------------
        #region btnAppendFollower_Click フォロワー追加取得ボタン
        //-------------------------------------------------------------------------------
        //        
        private void btnAppendFollower_Click(object sender, EventArgs e)
        {
            btnAppendFollower.Enabled = false;
            btnDisplayRelations.Enabled = false;
            tsslabel.Text = "フォロワーデータ取得中...";

            Utilization.InvokeTransaction(() =>
                {
                    try {
                        AddFollowerProfiles();
                        this.Invoke((Action)(() =>
                        {
                            tsslabel.Text = "データ取得完了しました";
                            
                            lock (_lock_displayrelationbtn) {
                                _all_read_follower = (_follower_ids.Length == _next_follower);
                                btnAppendFollower.Enabled = !_all_read_follower;
                                if (_all_read_friend || btnAppendFriends.Enabled) {
                                    btnDisplayRelations.Enabled = true;
                                }
                            }
                        }));
                    }
                    catch (InvalidOperationException) { }
                });
        }
        #endregion (btnAppendFollower_Click)

        //-------------------------------------------------------------------------------
        #region btnAppendFriends_Click フレンド追加取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAppendFriends_Click(object sender, EventArgs e)
        {
            btnAppendFriends.Enabled = false;
            btnDisplayRelations.Enabled = false;
            tsslabel.Text = "フレンドデータ取得中...";

            Utilization.InvokeTransaction(() =>
            {
                try {
                    AddFollowerProfiles();
                    this.Invoke((Action)(() =>
                    {
                        tsslabel.Text = "データ取得完了しました";
                        lock (_lock_displayrelationbtn) {
                            _all_read_friend = (_friend_ids.Length == _next_follower);
                            btnAppendFriends.Enabled = !_all_read_friend;
                            if (_all_read_follower || btnAppendFollower.Enabled) {
                                btnDisplayRelations.Enabled = true;
                            }
                        }
                    }));
                }
                catch (InvalidOperationException) { }
            });
        }
        #endregion (btnAppendFriends_Click)

        //-------------------------------------------------------------------------------
        #region -GetCommonFollowers 共通フォロワーリスト取得
        //-------------------------------------------------------------------------------
        //
        private void GetCommonFollowers()
        {
            IEnumerable<long> cmnFollowers = null;
            foreach (string user in _userNames) {
                IEnumerable<long> followers = null;
                long cursor = -1;
                do {
                    var users = FrmMain.Twitter.followers_ids(FrmMain.Twitter.IsAuthenticated(), screen_name: user, cursor: cursor);
                    cursor = users.NextCursor;
                    if (followers == null) { followers = users.Data; }
                    else { followers = followers.Concat(users.Data); }
                }
                while (cursor != 0);

                if (cmnFollowers == null) { cmnFollowers = followers; }
                else { cmnFollowers = cmnFollowers.Intersect(followers); }
            }

            _follower_ids = cmnFollowers.ToArray();
        }
        #endregion (GetCommonFollowers)
        //-------------------------------------------------------------------------------
        #region -GetCommonFriends 共通フレンドリスト取得
        //-------------------------------------------------------------------------------
        //
        private void GetCommonFriends()
        {
            IEnumerable<long> cmnFriends = null;
            foreach (string user in _userNames) {
                IEnumerable<long> friends = null;
                long cursor = -1;
                do {
                    var users = FrmMain.Twitter.friends_ids(FrmMain.Twitter.IsAuthenticated(), screen_name: user, cursor: cursor);
                    cursor = users.NextCursor;
                    if (friends == null) { friends = users.Data; }
                    else { friends = friends.Concat(users.Data); }
                }
                while (cursor != 0);

                if (cmnFriends == null) { cmnFriends = friends; }
                else { cmnFriends = cmnFriends.Intersect(friends); }
            }

            _friend_ids = cmnFriends.ToArray();
        }
        #endregion (GetCommonFriends)
        //-------------------------------------------------------------------------------
        #region -AddFollowerProfiles フォロワーデータ追加
        //-------------------------------------------------------------------------------
        //
        private void AddFollowerProfiles()
        {
            int cursor = _next_follower;
            try {
                long[] ids = Utilization.SliceArray(_follower_ids, ref cursor, 100);

                var profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                AddList(lstvCommonFollower, _profile_followers, profiles);

                _next_follower = cursor;
            }
            catch (TwitterAPIException ex) {
                tsslabel.Text = Utilization.SubTwitterAPIExceptionStr(ex);
            }
        }
        #endregion (AddFollowerProfiles)
        //-------------------------------------------------------------------------------
        #region -AddFriendProfiles フレンドデータ追加
        //-------------------------------------------------------------------------------
        //
        private void AddFriendProfiles()
        {
            int cursor = _next_friend;
            try {
                long[] ids = Utilization.SliceArray(_friend_ids, ref cursor, 100);

                var profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                AddList(lstvCommonFriend, _profile_friends, profiles);

                _next_friend = cursor;
            }
            catch (TwitterAPIException ex) {
                tsslabel.Text = Utilization.SubTwitterAPIExceptionStr(ex);
            }
        }
        #endregion (AddFriendProfiles)

        //-------------------------------------------------------------------------------
        #region -AddList リストに追加
        //-------------------------------------------------------------------------------
        //
        private void AddList(ListView lv, List<UserProfile> profileList, IEnumerable<UserProfile> profiles)
        {
            List<Tuple<ListViewItem, string>> urllist = new List<Tuple<ListViewItem, string>>();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (var p in profiles) {
                if (profileList.Exists(prof => prof.UserID == p.UserID)) { continue; } // 重複防止
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

                profileList.Add(p);
            }
            this.Invoke((Action)(() => lv.Items.AddRange(items.ToArray())));

            if (urllist.Count > 0) {
                Utilization.InvokeTransaction(() => GetImages(urllist));
            }
            //lstvList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        #endregion (AddList)
        //-------------------------------------------------------------------------------
        #region -GetToolTipText ツールチップのテキストを取得します。
        //-------------------------------------------------------------------------------
        //
        private string GetToolTipText(ListView lv, List<UserProfile> profileList, ListViewItem item)
        {
            int index = lv.Items.IndexOf(item);
            if (index < 0) { return ""; }

            UserProfile p = profileList[index];
            return Utilization.GetProfileDescriptionString(p);
        }
        #endregion (GetToolTipText)

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
    }
}
