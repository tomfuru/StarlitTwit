using System;
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
        /// <summary>FormType=UserFollowing,UserFollowerの時に設定しなければならない</summary>
        public string UserScreenName { get; set; }
        /// <summary>FormType=ListMember,ListSubscriberの時に設定しなければならない</summary>
        public string ListID { get; set; }
        /// <summary>FormType=Retweeterの時に設定しなければならない</summary>
        public long RetweetStatusID { get; set; }

        private FrmMain _mainForm = null;
        private List<UserProfile> _profileList = new List<UserProfile>();
        private long _next_cursor = -1;
        private int _page = 1;
        private ImageListWrapper _imageListWrapper = null;
        /// <summary>ロード中画像</summary>
        private Bitmap _loadingimg;
        /// <summary>アニメーション管理クラス</summary>
        private ImageAnimation _imageAnimation;
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
            MyFollowing,
            /// <summary>他ユーザーをフォローしているユーザー</summary>
            UserFollower,
            /// <summary>他ユーザーがフォローしているユーザー</summary>
            UserFollowing,
            /// <summary>リツイートしたユーザー</summary>
            Retweeter,
            //
            /// <summary>リストのメンバー</summary>
            ListMember,
            /// <summary>リストのフォロワー</summary>
            ListSubscriber,
            /// <summary>ブロック中のユーザー</summary>
            MyBlocking
        }
        //-------------------------------------------------------------------------------
        #endregion (EFormType)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region FrmFollower_Load ロード時
        //-------------------------------------------------------------------------------
        //
        private void FrmFollower_Load(object sender, EventArgs e)
        {
            Utilization.SetModelessDialogCenter(this);

            switch (FormType) {
                case EFormType.MyFollower:
                    Text = "フォローされている人";
                    break;
                case EFormType.MyFollowing:
                    Text = "フォローしている人";
                    break;
                case EFormType.UserFollower:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    Text = string.Format("{0}をフォローしている人", UserScreenName);
                    break;
                case EFormType.UserFollowing:
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
            }

            if (FormType != EFormType.MyFollowing) {
                lstvList.Columns.Add(new ColumnHeader() { Text = "", Width = 90 });
            }
            tsslabel.Text = "取得中...";
            lblCount.Text = "";
            Utilization.InvokeTransaction(() => GetUsers());
        }
        #endregion (FrmFollower_Load)
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
                this.Invoke(new Action(() => lstvList.Invalidate()));
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
        #region menuRow_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void menuRow_Opening(object sender, CancelEventArgs e)
        {
            if (lstvList.SelectedItems.Count == 0) { e.Cancel = true; return; }
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;

            bool isBlocking = (FormType == EFormType.MyBlocking);

            tsmiFollow.Visible = !isBlocking && !prof.FolllowRequestSent && !prof.Following;
            tsmiRemove.Visible = !isBlocking && !prof.FolllowRequestSent && prof.Following;


            tsmiBlock.Visible = (FormType == EFormType.MyFollower);
            tsmiUnblock.Visible = isBlocking;

            toolStripMenuItem1.Visible = !isBlocking && !prof.FolllowRequestSent;
            toolStripMenuItem3.Visible = isBlocking;
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
                        case EFormType.MyFollowing:
                            RemoveSelectedItem();
                            break;
                    }
                }
            }
        }
        #endregion (tsmiRemove_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispFollowing_Click フレンドを見るクリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispFollowing_Click(object sender, EventArgs e)
        {
            Utilization.ShowUsersForm(_mainForm, _imageListWrapper, EFormType.UserFollowing,
                                         ((UserProfile)lstvList.SelectedItems[0].Tag).ScreenName);
        }
        #endregion (tsmiDispFollowing_Click)
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
            Utilization.ShowStatusesForm((FrmMain)this.Owner, FrmDispStatuses.EFormType.UserStatus, prof.ScreenName);
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
            StringBuilder sb = new StringBuilder();
            if (p.Protected) { sb.AppendLine("◆非公開アカウント"); }
            sb.Append("●ユーザー名：");
            sb.AppendLine(p.ScreenName);
            sb.Append("●名称：");
            sb.AppendLine(p.UserName);
            sb.AppendLine("●位置情報");
            sb.AppendLine(p.Location);
            sb.AppendLine("●自己紹介：");
            sb.AppendLine(p.Description);
            sb.Append("●フォロー数：");
            sb.AppendLine(p.FollowingNum.ToString());
            sb.Append("●フォロワー数：");
            sb.AppendLine(p.FollowerNum.ToString());
            sb.Append("●発言数：");
            sb.AppendLine(p.StatusNum.ToString());
            sb.Append("●お気に入り数：");
            sb.Append(p.FavoriteNum.ToString());

            return sb.ToString();
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
                if (p.FolllowRequestSent) { item.SubItems.Add("リクエスト済"); }
                else { item.SubItems.Add((p.Following) ? "フォロー中" : ""); }
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
                    if (FormType == EFormType.Retweeter) {
                        profiles = FrmMain.Twitter.statuses_id_retweeted_by(RetweetStatusID, 100, _page);
                        _page++;
                        this.Invoke(new Action(() => btnAppend.Enabled = (profiles.Count() > 0)));
                    }
                    else if (FormType == EFormType.MyBlocking) {
                        profiles = FrmMain.Twitter.blocks_blocking(_page, 100);
                        _page++;
                        this.Invoke(new Action(() => btnAppend.Enabled = (profiles.Any(prof => _profileList.All(lprof => lprof.UserID != prof.UserID)))));
                    }
                    else {
                        SequentData<UserProfile> proftpl = null;
                        switch (FormType) {
                            case EFormType.MyFollower:
                                proftpl = FrmMain.Twitter.statuses_followers(cursor: _next_cursor);
                                break;
                            case EFormType.MyFollowing:
                                proftpl = FrmMain.Twitter.statuses_friends(cursor: _next_cursor);
                                break;
                            case EFormType.UserFollower:
                                proftpl = FrmMain.Twitter.statuses_followers(screen_name: UserScreenName, cursor: _next_cursor);
                                break;
                            case EFormType.UserFollowing:
                                proftpl = FrmMain.Twitter.statuses_friends(screen_name: UserScreenName, cursor: _next_cursor);
                                break;
                            case EFormType.ListMember:
                                proftpl = FrmMain.Twitter.list_members_Get(ListID, UserScreenName, _next_cursor);
                                break;
                            case EFormType.ListSubscriber:
                                proftpl = FrmMain.Twitter.list_subscribers_Get(ListID, UserScreenName, _next_cursor);
                                break;
                        }
                        if (proftpl != null) {
                            profiles = proftpl.Data;
                            _next_cursor = proftpl.NextCursor;
                        }
                        this.Invoke(new Action(() => btnAppend.Enabled = (_next_cursor != 0)));
                    }

                    if (profiles != null) {
                        this.Invoke(new Action(() =>
                        {
                            AddList(profiles);
                            lblCount.Text = string.Format("{0}人見つかりました", _profileList.Count);
                            tsslabel.Text = (btnAppend.Enabled) ? "取得完了しました" : "全て取得完了しました";
                        }));
                    }
                }
                catch (TwitterAPIException) {
                    this.Invoke(new Action(() =>
                    {
                        tsslabel.Text = "取得に失敗しました。";
                        btnAppend.Enabled = true;
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
                        this.Invoke(new Action(() => Refresh()));
                    }
                    else {
                        d.Item1.ImageKey = FrmMain.STR_IMAGE_CROSS;
                    }
                }
                _imageAnimation.StopAnimation();
            }
            catch (InvalidOperationException) { }
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
