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
    public partial class FrmDispLists : Form
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>フォームの用途</summary>
        public EFormType FormType { get; private set; }
        private FrmMain _mainForm = null;
        private ImageListWrapper _imageListWrapper = null;
        /// <summary>FormType=UserList,UserBelongedList,UserSubscribingListの時必須．ユーザー名</summary>
        public string UserScreenName { get; set; }
        private long _next_cursor = -1;

        private List<ListData> _listList = new List<ListData>();

        /// <summary>ロード中画像</summary>
        private Bitmap _loadingimg;
        /// <summary>アニメーション管理クラス</summary>
        private ImageAnimation _imageAnimation;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region EFormType 列挙体：フォームの種類
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォームがどのような目的で使われるかを表します。
        /// </summary>
        public enum EFormType
        {
            /// <summary>自分のリスト</summary>
            MyList,
            /// <summary>指定ユーザーのリスト</summary>
            UserList,
            /// <summary>自分が所属しているリスト</summary>
            MyBelongedList,
            /// <summary>指定ユーザーが所属しているリスト</summary>
            UserBelongedList,
            /// <summary>自分がフォローしているリスト</summary>
            MySubscribingList,
            /// <summary>指定ユーザーがフォローしているリスト</summary>
            UserSubscribingList
        }
        //-------------------------------------------------------------------------------
        #endregion (EFormType)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public FrmDispLists(FrmMain mainForm, ImageListWrapper imgListWrapper, EFormType formtype)
        {
            InitializeComponent();

            _mainForm = mainForm;
            _imageListWrapper = imgListWrapper;
            lstvList.SmallImageList = imgListWrapper.ImageList;
            FormType = formtype;

            UserScreenName = null;

            _loadingimg = (Bitmap)StarlitTwit.Properties.Resources.NowLoadingS.Clone();
            _imageAnimation = new ImageAnimation(_loadingimg);
            _imageAnimation.FrameUpdated += Image_Animate;

            btnAddNewList.Visible = (formtype == EFormType.MyList);
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Utilization.SetModelessDialogCenter(this);

            switch (FormType) {
                case EFormType.MyList:
                    this.Text = "リスト一覧";
                    break;
                case EFormType.UserList:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    this.Text = string.Format("{0}のリスト一覧", UserScreenName);
                    break;
                case EFormType.MyBelongedList:
                    this.Text = "所属リスト一覧";
                    break;
                case EFormType.UserBelongedList:
                    this.Text = string.Format("{0}の所属リスト一覧", UserScreenName);
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    break;
                case EFormType.MySubscribingList:
                    this.Text = "フォロー中リスト一覧";
                    break;
                case EFormType.UserSubscribingList:
                    Debug.Assert(UserScreenName != null, "UserScreenNameが設定されていない");
                    this.Text = string.Format("{0}のフォロー中リスト一覧", UserScreenName);
                    break;
                default:
                    break;
            }

            if (FormType == EFormType.MyList) {
                lstvList.Columns.Add(new ColumnHeader() { Text = "公開", Width = 50 });
            }
            else { lstvList.Columns.Insert(1, new ColumnHeader() { Text = "所有者", Width = 90 }); }
            tsslLabel.Text = "取得中...";
            lblCount.Text = "";
            Utilization.InvokeTransaction(() => GetUsers());
        }
        //-------------------------------------------------------------------------------
        #endregion (OnLoad)
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
        #region menuRow_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void menuRow_Opening(object sender, CancelEventArgs e)
        {
            if (lstvList.SelectedItems.Count == 0) { e.Cancel = true; return; }
            ListData listdata = (ListData)lstvList.SelectedItems[0].Tag;

            tsmiEditList.Visible = tsmiDeleteList.Visible = tsSepListEdit.Visible = (FormType == EFormType.MyList);
            // TODO: リストフォロー確認？
            //tsmiListSubscribe.Visible = 
            //tsmiListUnSubscribe.Visible = 

            // TODO: 実装次第項目削除
            tsmiEditList.Visible = tsmiDeleteList.Visible = tsSepListEdit.Visible =
            toolStripMenuItem2.Visible = tsmiListSubscribe.Visible = tsmiListUnSubscribe.Visible =
            tsmiDispListStatuses.Visible = false;
        }
        #endregion (menuRow_Opening)
        //-------------------------------------------------------------------------------
        #region tsmiEditList_Click リスト編集
        //-------------------------------------------------------------------------------
        //
        private void tsmiEditList_Click(object sender, EventArgs e)
        {
            // TODO:リスト編集
        }
        #endregion (tsmiEditList_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDeleteList_Click リスト削除
        //-------------------------------------------------------------------------------
        //
        private void tsmiDeleteList_Click(object sender, EventArgs e)
        {
            // TODO:リスト削除
        }
        #endregion (tsmiDeleteList_Click)
        //-------------------------------------------------------------------------------
        #region tsmiMakeListTab_Click リストタブ追加
        //-------------------------------------------------------------------------------
        //
        private void tsmiMakeListTab_Click(object sender, EventArgs e)
        {
            ListData listdata = (ListData)lstvList.SelectedItems[0].Tag;

            _mainForm.MakeNewTab(TabSearchType.List, listdata.Slug, listdata.OwnerScreenName);
        }
        #endregion (tsmiMakeListTab_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispListStatuses_Click リストの発言表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispListStatuses_Click(object sender, EventArgs e)
        {
            // TODO:Implement DispListStatuses
            //Utilization.ShowUserTweet(_mainForm, FrmDispStatuses.EFormType.ListStatuses, 
        }
        #endregion (tsmiDispListStatuses_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispListUsers_Click リスト内ユーザー表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispListUsers_Click(object sender, EventArgs e)
        {
            ListData listdata = (ListData)lstvList.SelectedItems[0].Tag;
            Utilization.ShowUsersForm(_mainForm, _imageListWrapper, FrmDispUsers.EFormType.ListMember, listdata.OwnerScreenName, listdata.Slug);
        }
        #endregion (tsmiDispListUsers_Click)
        //-------------------------------------------------------------------------------
        #region tsmiDispListSubscriber_Click リストのフォロワ表示
        //-------------------------------------------------------------------------------
        //
        private void tsmiDispListSubscriber_Click(object sender, EventArgs e)
        {
            ListData listdata = (ListData)lstvList.SelectedItems[0].Tag;
            Utilization.ShowUsersForm(_mainForm, _imageListWrapper, FrmDispUsers.EFormType.ListSubscriber, listdata.OwnerScreenName, listdata.Slug);
        }
        #endregion (tsmiDispListSubscriber_Click)
        //-------------------------------------------------------------------------------
        #region tsmiListSubscribe_Click リストフォロー
        //-------------------------------------------------------------------------------
        //
        private void tsmiListSubscribe_Click(object sender, EventArgs e)
        {
            // TODO:Implement List To Subscribe
        }
        #endregion (tsmiListSubscribe_Click)
        //-------------------------------------------------------------------------------
        #region tsmiListUnSubscribe_Click リストフォロー解除
        //-------------------------------------------------------------------------------
        //
        private void tsmiListUnSubscribe_Click(object sender, EventArgs e)
        {
            // TODO:Implement List To UnSubscribe
        }
        #endregion (tsmiListUnSubscribe_Click)
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
                    int index = lstvList.Items.IndexOf(lvi);
                    if (index < 0) { return; }
                    ListData l = _listList[index];

                    ttInfo.SetToolTip(lv, l.Description);
                    //ToolTipを再びアクティブにする
                    ttInfo.Active = true;
                }
                //ポイントされているアイテムを記憶する
                _lastListViewItem = lvi;
            }
        }
        #endregion (lstvList_MouseMove)
        //-------------------------------------------------------------------------------
        #region btnAddNewList_Click 新規リストボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAddNewList_Click(object sender, EventArgs e)
        {
            using (FrmEditList frm = new FrmEditList(true, _listList.Select(list => list.Name))) {
                if (frm.ShowDialog() == DialogResult.OK) {
                    AddList(frm.ListData.AsEnumerable());
                }
            }
        }
        #endregion (btnAddNewList_Click)
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
        #region btnAppend_Click 追加取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {
            Utilization.InvokeTransaction(() => GetUsers());
        }
        #endregion (btnAppend_Click)
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
            Image img = _imageListWrapper.GetImage(_listList[e.ItemIndex].OwnerIconURL);
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
        #region -GetUsers ユーザー取得 using Twitter API
        //-------------------------------------------------------------------------------
        //
        private void GetUsers()
        {
            try {
                IEnumerable<ListData> listdata = null;

                SequentData<ListData> listseq = null;
                switch (FormType) {
                    case EFormType.MyList:
                        listseq = FrmMain.Twitter.lists_Get(cursor: _next_cursor);
                        break;
                    case EFormType.MyBelongedList:
                        listseq = FrmMain.Twitter.lists_memberships(cursor: _next_cursor);
                        break;
                    case EFormType.MySubscribingList:
                        listseq = FrmMain.Twitter.lists_subscriptions(cursor: _next_cursor);
                        break;
                    case EFormType.UserList:
                        listseq = FrmMain.Twitter.lists_Get(UserScreenName, _next_cursor);
                        break;
                    case EFormType.UserBelongedList:
                        listseq = FrmMain.Twitter.lists_memberships(UserScreenName, _next_cursor);
                        break;
                    case EFormType.UserSubscribingList:
                        listseq = FrmMain.Twitter.lists_subscriptions(UserScreenName, _next_cursor);
                        break;
                }
                if (listseq != null) {
                    listdata = listseq.Data;
                    _next_cursor = listseq.NextCursor;

                    this.Invoke(new Action(() =>
                    {
                        AddList(listdata);
                        lblCount.Text = string.Format("{0}個見つかりました", _listList.Count);
                        tsslLabel.Text = "取得完了しました。";
                    }));
                }

                this.Invoke(new Action(() => btnAppend.Enabled = (_next_cursor != 0)));
            }
            catch (InvalidOperationException) { }
            catch (TwitterAPIException) {
                this.Invoke(new Action(() =>
                {
                    tsslLabel.Text = "取得に失敗しました。";
                    btnAppend.Enabled = true;
                }));
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetUsers)
        //-------------------------------------------------------------------------------
        #region -AddList リスト追加
        //-------------------------------------------------------------------------------
        //
        private void AddList(IEnumerable<ListData> listdata)
        {
            List<Tuple<ListViewItem, string>> urllist = new List<Tuple<ListViewItem, string>>();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (var list in listdata) {
                if (_listList.Exists(l => l.ID == list.ID)) { continue; } // 重複防止
                ListViewItem item = new ListViewItem();
                item.Tag = list;
                if (!_imageListWrapper.ImageContainsKey(list.OwnerIconURL)
                 && !urllist.Any(t => t.Item2.Equals(list.OwnerIconURL))) { urllist.Add(new Tuple<ListViewItem, string>(item, list.OwnerIconURL)); }
                if (FormType != EFormType.MyList) {
                    item.SubItems.Add(list.OwnerScreenName);
                }
                item.SubItems.Add(list.Name);
                item.SubItems.Add(list.MemberCount.ToString());
                item.SubItems.Add(list.SubscriberCount.ToString());
                if (FormType == EFormType.MyList) { item.SubItems.Add((list.Public) ? "公開" : "非公開"); }

                items.Add(item);
                _listList.Add(list);
            }
            lstvList.Items.AddRange(items.ToArray());

            if (urllist.Count > 0) {
                Utilization.InvokeTransaction(() => GetImages(urllist));
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (AddList)
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
    }
}
