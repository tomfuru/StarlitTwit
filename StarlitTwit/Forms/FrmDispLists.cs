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
        #region Member
        //-------------------------------------------------------------------------------
        /// <summary>フォームの用途</summary>
        public EFormType FormType { get; private set; }
        private FrmMain _mainForm = null;
        private ImageListWrapper _imageListWrapper = null;
        /// <summary>FormType=UserList,UserBelongedList,UserSubscribingListの時必須．ユーザー名</summary>
        public string UserScreenName { get; set; }
        private long _next_cursor = -1;

        private List<ListData> _listList = new List<ListData>();
        //-------------------------------------------------------------------------------
        #endregion (Member)

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
            FormType = formtype;

            UserScreenName = null;
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
                lstvList.Columns.Add(new ColumnHeader() { Name = "公開", Width = 50 });
            }
            { lstvList.Columns.Insert(0, new ColumnHeader() { Name = "所有者", Width = 100 }); }
            tsslLabel.Text = "取得中...";
            lblCount.Text = "";
            Utilization.InvokeTransaction(() => GetUsers());
        }
        //-------------------------------------------------------------------------------
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
        #region btnAppend_Click 追加取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {

        }
        #endregion (btnAppend_Click)
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
                    case EFormType.UserList:
                        break;
                    case EFormType.MyBelongedList:
                        break;
                    case EFormType.UserBelongedList:
                        break;
                    case EFormType.MySubscribingList:
                        break;
                    case EFormType.UserSubscribingList:
                        break;
                }
                if (listseq != null) {
                    listdata = listseq.Data;
                    _next_cursor = listseq.NextCursor;

                    this.Invoke(new Action(() =>
                    {
                        AddList(listdata);
                        lblCount.Text = string.Format("{0}人見つかりました", _listList.Count);
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

        }
        //-------------------------------------------------------------------------------
        #endregion (AddList)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)
    }
}
