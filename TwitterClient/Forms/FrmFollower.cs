using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TwitterClient
{
    /// <summary>
    /// フォロワー/フォローしている人を表示するフォームです。
    /// </summary>
    public partial class FrmFollower : Form
    {
        public FrmFollowType FormType { get; set; }

        private List<UserProfile> _profileList = new List<UserProfile>();

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
        #region FrmFollower_Load ロード時
        //-------------------------------------------------------------------------------
        //
        private void FrmFollower_Load(object sender, EventArgs e)
        {
            if (FormType == FrmFollowType.Follower) {
                lstvList.Columns.Add(new ColumnHeader() { Text = "", Width = 100 });
            }
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
        #region btnAppend_Click 追加取得ボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {

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
        #region -AddList リストに追加
        //-------------------------------------------------------------------------------
        //
        private void AddList(UserProfile[] profiles)
        {
            List<Tuple<ListViewItem, string>> urllist = new List<Tuple<ListViewItem, string>>();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (var p in profiles) {
                ListViewItem item = new ListViewItem();
                item.ImageKey = p.IconURL;
                if (!lstvList.SmallImageList.Images.ContainsKey(p.IconURL)) { urllist.Add(new Tuple<ListViewItem, string>(item, p.IconURL)); }
                ListViewItem.ListViewSubItem si = new ListViewItem.ListViewSubItem();
                item.SubItems.Add(p.ScreenName);
                item.SubItems.Add(p.UserName);
                item.SubItems.Add((p.Following) ? "フォロー中" : "");
                items.Add(item);
            }
            lstvList.Items.AddRange(items.ToArray());
            _profileList.AddRange(profiles);

            (new Action<Tuple<ListViewItem, string>[]>(GetImages)).BeginInvoke(urllist.ToArray(), Utilization.InvokeCallback, null);

            //lstvList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            btnAppend.Enabled = true;
        }
        #endregion (AddList)

        //-------------------------------------------------------------------------------
        #region -GetUsers ユーザー取得
        //-------------------------------------------------------------------------------
        //
        private void GetUsers()
        {
            UserProfile[] profiles = null;
            switch (FormType) {
                case FrmFollowType.Follower:
                    profiles = FrmMain.Twitter.statuses_followers();
                    break;
                case FrmFollowType.Following:
                    profiles = FrmMain.Twitter.statuses_friends();
                    break;
            }
            if (profiles != null) {
                this.Invoke(new Action(() => AddList(profiles)));
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
    }

    //-------------------------------------------------------------------------------
    #region FrmFollowType 列挙体：タイプ
    //-------------------------------------------------------------------------------
    /// <summary>
    /// フォームの表示タイプを指定します。
    /// </summary>
    public enum FrmFollowType : byte
    {
        /// <summary>フォローされているユーザー</summary>
        Follower,
        /// <summary>フォローしているユーザー</summary>
        Following
    }
    //-------------------------------------------------------------------------------
    #endregion (FrmFollowType)
}
