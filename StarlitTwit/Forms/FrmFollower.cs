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
    /// <summary>
    /// フォロワー/フォローしている人を表示するフォームです。
    /// </summary>
    public partial class FrmFollower : Form
    {
        public EFormType FormType { get; set; }

        private List<UserProfile> _profileList = new List<UserProfile>();
        private long _next_cursor = -1;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmFollower(ImageList imgList = null)
        {
            InitializeComponent();
            lstvList.SmallImageList = imgList;
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
            /// <summary>フォローされているユーザー</summary>
            Follower,
            /// <summary>フォローしているユーザー</summary>
            Following
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
            if (FormType == EFormType.Follower) {
                lstvList.Columns.Add(new ColumnHeader() { Text = "", Width = 100 });
            }
            tsslabel.Text = "取得中...";
            (new Action(GetUsers)).BeginInvoke(Utilization.InvokeCallback, null);
        }
        #endregion (FrmFollower_Load)
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
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (lstvList.SelectedItems.Count == 0) { e.Cancel = true; return; }
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;

            tsmiFollow.Visible = !prof.FolllowRequestSent && !prof.Following;
            tsmiRemove.Visible = !prof.FolllowRequestSent && prof.Following;

            toolStripMenuItem1.Visible = !prof.FolllowRequestSent;
        }
        #endregion (menuRow_Opening)
        //-------------------------------------------------------------------------------
        #region tsmiFollow_Click フォロークリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiFollow_Click(object sender, EventArgs e)
        {
            if (Message.ShowQuestionMessage("フォローします。よろしいですか？") == System.Windows.Forms.DialogResult.Yes) {
                UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
                bool? ret = Follow(prof.ScreenName);
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
            if (Message.ShowQuestionMessage("フォローを解除します。よろしいですか？") == System.Windows.Forms.DialogResult.Yes) {
                UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
                if (RemoveFollow(prof.ScreenName)) {
                    switch (FormType) {
                        case EFormType.Follower:
                            lstvList.SelectedItems[0].SubItems[3].Text = "";
                            ((UserProfile)lstvList.SelectedItems[0].Tag).Following = false;
                            break;
                        case EFormType.Following:
                            lstvList.Items.Remove(lstvList.SelectedItems[0]);
                            break;
                    }
                }
            }
        }
        #endregion (tsmiRemove_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDisplayUserTweet_Click 発言表示クリック時
        //-------------------------------------------------------------------------------
        //
        private void tsmiDisplayUserTweet_Click(object sender, EventArgs e)
        {
            UserProfile prof = (UserProfile)lstvList.SelectedItems[0].Tag;
            Utilization.ShowUserTweet((FrmMain)this.Owner, prof.ScreenName);
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
        #region btnAppend_Click 追加取得ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {
            tsslabel.Text = "取得中...";
            (new Action(GetUsers)).BeginInvoke(Utilization.InvokeCallback, null);
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
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //-------------------------------------------------------------------------------
        #region メソッド
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
            sb.AppendLine("●プロフィール：");
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
                ListViewItem item = new ListViewItem();
                item.Tag = p;
                item.ImageKey = p.IconURL;
                if (!lstvList.SmallImageList.Images.ContainsKey(p.IconURL)) { urllist.Add(new Tuple<ListViewItem, string>(item, p.IconURL)); }
                ListViewItem.ListViewSubItem si = new ListViewItem.ListViewSubItem();
                item.SubItems.Add(p.ScreenName);
                item.SubItems.Add(p.UserName);
                if (p.FolllowRequestSent) { item.SubItems.Add("リクエスト済"); }
                else { item.SubItems.Add((p.Following) ? "フォロー中" : ""); }
                items.Add(item);
            }
            lstvList.Items.AddRange(items.ToArray());
            _profileList.AddRange(profiles);

            (new Action<Tuple<ListViewItem, string>[]>(GetImages)).BeginInvoke(urllist.ToArray(), Utilization.InvokeCallback, null);

            //lstvList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        #endregion (AddList)
        //-------------------------------------------------------------------------------
        #region -GetUsers ユーザー取得 using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void GetUsers()
        {
            bool canFinalize = true;
            try {
                IEnumerable<UserProfile> profiles = null;
                Tuple<IEnumerable<UserProfile>, long, long> proftpl;
                switch (FormType) {
                    case EFormType.Follower:
                        proftpl = FrmMain.Twitter.statuses_followers(cursor: _next_cursor);
                        profiles = proftpl.Item1;
                        _next_cursor = proftpl.Item2;
                        break;
                    case EFormType.Following:
                        proftpl = FrmMain.Twitter.statuses_friends(cursor: _next_cursor);
                        profiles = proftpl.Item1;
                        _next_cursor = proftpl.Item2;
                        break;
                }
                if (profiles != null) {
                    this.Invoke(new Action(() =>
                    {
                        AddList(profiles);
                        tsslabel.Text = "取得完了しました。";
                    }));
                }
            }
            catch (InvalidOperationException) {
                canFinalize = false;
            }
            catch (TwitterAPIException) {
                this.Invoke(new Action(() => tsslabel.Text = "取得に失敗しました。"));
            }
            finally {
                if (canFinalize) {
                    this.Invoke(new Action(() => btnAppend.Enabled = (_next_cursor != 0)));
                }
            }
        }
        #endregion (GetUsers)
        //-------------------------------------------------------------------------------
        #region -GetImages 画像取得と追加 (別スレッド処理)
        //-------------------------------------------------------------------------------
        //
        private void GetImages(Tuple<ListViewItem, string>[] data)
        {
            try {
                foreach (var d in data) {
                    Image img = Utilization.GetImageFromURL(d.Item2);
                    if (img != null) {
                        lstvList.SmallImageList.Images.Add(d.Item2, img);
                        this.Invoke(new Action(() => ResetImageKey(d.Item1)));
                    }
                }
            }
            catch (InvalidOperationException) { }
        }
        #endregion (GetImages)
        //-------------------------------------------------------------------------------
        #region -ResetImageKey 画像キーを再設定して画像を表示させます
        //-------------------------------------------------------------------------------
        //
        private void ResetImageKey(ListViewItem item)
        {
            item.ImageKey = item.ImageKey;
        }
        #endregion (ResetImageKey)
        //-------------------------------------------------------------------------------
        #region -Follow フォローを行います using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private bool? Follow(string screen_name)
        {
            try {
                UserProfile ret = FrmMain.Twitter.friendships_create(screen_name: screen_name);
                if (ret.Protected && !ret.Following) { return null; }
            }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (Follow)
        //-------------------------------------------------------------------------------
        #region -RemoveFollow フォロー解除を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private bool RemoveFollow(string screen_name)
        {
            try { FrmMain.Twitter.friendships_destroy(screen_name: screen_name); }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (RemoveFollow)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)
    }


}
