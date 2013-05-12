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
    public partial class FrmMakeTab : Form
    {
        //-------------------------------------------------------------------------------
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>リストデータ</summary>
        private IEnumerable<ListData> _listData = null;
        /// <summary>リストオーナー</summary>
        private string _listOwner = null;
        /// <summary>データ文字列</summary>
        private string _strdata = null;
        /// <summary>変更前のタブ名</summary>
        private string _strPrevTabName = null;
        /// <summary>タブ作成データ</summary>
        public TabData TabData { get; set; }
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// タブ作成フォームを初期化します。
        /// </summary>
        /// <param name="twitter">Twitter APIを使用するためのクラス</param>
        public FrmMakeTab()
        {
            InitializeComponent();

            pnlList.Location = pnlUser.Location = pnlKeyword.Location;
            this.Size = new Size(287, 189);
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool isAuthenticated = FrmMain.Twitter.IsAuthenticated;

            object[] items = (isAuthenticated) ? 
                new object[] {
                TabSearchType.Keyword,
                TabSearchType.User,
                TabSearchType.List
            }
            : new object[] {
                TabSearchType.Keyword,
                TabSearchType.User
            };

            cmbSearchType.Items.AddRange(items);

            if (TabData != null) {
                InitItems();
            }
            else {
                cmbSearchType.SelectedIndex = 0;
            }

            if (isAuthenticated) {
                _listOwner = (string.IsNullOrEmpty(TabData.ListOwner)) ? FrmMain.Twitter.ScreenName : TabData.ListOwner;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (OnLoad)
        //-------------------------------------------------------------------------------
        #region btnOK_Click OK
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) { return; }

            SetTabData();

            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセル
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)
        //-------------------------------------------------------------------------------
        #region cmbSearchType_SelectedIndexChanged 検索タイプ選択　using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void cmbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((TabSearchType)cmbSearchType.SelectedItem) {
                case TabSearchType.Keyword:
                    pnlKeyword.Visible = true;
                    pnlList.Visible = pnlUser.Visible = false;
                    txtkeyword_TextChanged(txtKeyword, EventArgs.Empty);
                    break;
                case TabSearchType.User:
                    pnlUser.Visible = true;
                    pnlKeyword.Visible = pnlList.Visible = false;
                    txtUserName_TextChanged(txtUserName, EventArgs.Empty);
                    break;
                case TabSearchType.List:
                    pnlList.Visible = true;
                    pnlKeyword.Visible = pnlUser.Visible = false;
                    ConfigListList();   // リスト設定
                    if (cmbList.SelectedIndex != -1) {
                        cmbList_SelectedIndexChanged(cmbList, EventArgs.Empty);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (cmbSearchType_SelectedIndexChanged)
        //-------------------------------------------------------------------------------
        #region txtkeyword_TextChanged キーワードテキストボックステキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtkeyword_TextChanged(object sender, EventArgs e)
        {
            if (txtTabName.Text.Equals(_strdata) || string.IsNullOrEmpty(txtTabName.Text)) {
                txtTabName.Text = txtKeyword.Text;
            }
            _strdata = txtKeyword.Text;
        }
        //-------------------------------------------------------------------------------
        #endregion (txtkeyword_TextChanged)
        //-------------------------------------------------------------------------------
        #region txtUserName_TextChanged ユーザー名テキストボックステキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (txtTabName.Text.Equals(_strdata) || string.IsNullOrEmpty(txtTabName.Text)) {
                txtTabName.Text = txtUserName.Text;
            }
            _strdata = txtUserName.Text;
        }
        //-------------------------------------------------------------------------------
        #endregion (txtUserName_TextChanged)
        //-------------------------------------------------------------------------------
        #region cmbList_SelectedIndexChanged リストコンボボックス選択変更時
        //-------------------------------------------------------------------------------
        //
        private void cmbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtTabName.Text.Equals(_strdata) || string.IsNullOrEmpty(txtTabName.Text)) {
                txtTabName.Text = cmbList.SelectedItem.ToString();
            }
            _strdata = cmbList.SelectedItem.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (cmbList_SelectedIndexChanged)
        //-------------------------------------------------------------------------------
        #region txtTabName_TextChanged タブ表示名テキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtTabName_TextChanged(object sender, EventArgs e)
        {

        }
        //-------------------------------------------------------------------------------
        #endregion (txtTabName_TextChanged)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //-------------------------------------------------------------------------------
        #region メソッド
        //-------------------------------------------------------------------------------
        #region -InitItems 初期設定された項目をセット
        //-------------------------------------------------------------------------------
        //
        private void InitItems()
        {
            cmbSearchType.SelectedItem = TabData.SearchType;
            switch (TabData.SearchType) {
                case TabSearchType.Keyword:
                    txtKeyword.Text = TabData.SearchWord;
                    break;
                case TabSearchType.User:
                    txtUserName.Text = TabData.SearchWord;
                    break;
                case TabSearchType.List:
                    if (!string.IsNullOrEmpty(TabData.SearchWord)) {
                        cmbList.SelectedItem = TabData.SearchWord;
                    }
                    break;
            }

            txtTabName.Text = TabData.TabName;

            if (TabData.FirstGetNum != 0) {
                numTimeline_First.Value = TabData.FirstGetNum;
                numTimeline_Add.Value = TabData.RenewGetNum;
                numTimeline_Interval.Value = TabData.GetInterval;
            }

            _strPrevTabName = TabData.TabName;
        }
        #endregion (InitItems)
        //-------------------------------------------------------------------------------
        #region -ConfigListList リストのリストを取得し設定します。 using TwitterAPI
        //-------------------------------------------------------------------------------
        //
        private void ConfigListList()
        {
            if (_listData == null) {
                try {
                    Debug.Assert(!string.IsNullOrEmpty(_listOwner));

                    lblListOwner.Text = "ユーザー:" + _listOwner;
                                        
                    //long next_cursor = -1;
                    //IEnumerable<ListData> lists = Utilization.EmptyIEnumerable<ListData>();
                    //do {
                    //    var seqdata = FrmMain.Twitter.lists_list();
                    //    next_cursor = seqdata.NextCursor;
                    //    lists = lists.Concat(seqdata.Data);
                    //} while (next_cursor != 0);

                    IEnumerable<ListData> lists = FrmMain.Twitter.lists_list();

                    _listData = lists.ToArray();
                    if (_listData.Count() > 0) {
                        cmbList.Items.AddRange(_listData.Select((data) => (object)data.Slug).ToArray());
                        cmbList.SelectedIndex = 0;
                    }
                }
                catch (TwitterAPIException ex) {
                    Message.ShowErrorMessage(Utilization.SubTwitterAPIExceptionStr(ex));
                }
            }
        }
        #endregion (ConfigListList)
        //-------------------------------------------------------------------------------
        #region -CheckInput 入力チェック
        //-------------------------------------------------------------------------------
        //
        private bool CheckInput()
        {
            if (cmbSearchType.SelectedIndex == -1) {
                Message.ShowInfoMessage("検索方法が設定されていません。");
                return false;
            }

            switch ((TabSearchType)cmbSearchType.SelectedItem) {
                case TabSearchType.Keyword:
                    if (string.IsNullOrEmpty(txtKeyword.Text)) {
                        Message.ShowInfoMessage("検索語が空白です。");
                        return false;
                    }
                    break;
                case TabSearchType.User:
                    if (string.IsNullOrEmpty(txtUserName.Text)) {
                        Message.ShowInfoMessage("検索ユーザー名が空白です。");
                        return false;
                    }
                    break;
                case TabSearchType.List:
                    if (cmbList.SelectedIndex == -1) {
                        Message.ShowInfoMessage("リストが選択されていません。");
                        return false;
                    }
                    break;
                default:
                    Message.ShowInfoMessage("検索方法が不正です。");
                    return false;
            }

            if (string.IsNullOrEmpty(txtTabName.Text)) {
                Message.ShowInfoMessage("タブ名が空白です。");
                return false;
            }

            if ((_strPrevTabName == null || !_strPrevTabName.Equals(txtTabName.Text))
                && FrmMain.SettingsData.TabDataDic.Keys.Contains(txtTabName.Text)) {

                Message.ShowInfoMessage("名前が重複するタブがあります。別のタブ名を指定して下さい。");
                return false;
            }
            return true;
        }
        //-------------------------------------------------------------------------------
        #endregion (CheckInput)
        //-------------------------------------------------------------------------------
        #region -SetTabData タブデータをセット
        //-------------------------------------------------------------------------------
        //
        private void SetTabData()
        {
            Func<TabSearchType, string> getwordfunc = (type) =>
            {
                switch (type) {
                    case TabSearchType.Keyword:
                        return txtKeyword.Text;
                    case TabSearchType.User:
                        return txtUserName.Text;
                    case TabSearchType.List:
                        return cmbList.SelectedItem.ToString();
                    default:
                        return null;
                }
            };

            TabData = new TabData() {
                SearchType = (TabSearchType)cmbSearchType.SelectedItem,
                TabName = txtTabName.Text,
                ListOwner = _listOwner,
                FirstGetNum = (int)numTimeline_First.Value,
                RenewGetNum = (int)numTimeline_Add.Value,
                GetInterval = (int)numTimeline_Interval.Value,
                SearchWord = getwordfunc((TabSearchType)cmbSearchType.SelectedItem)
            };
        }
        //-------------------------------------------------------------------------------
        #endregion (SetTabData)
        //-------------------------------------------------------------------------------
        #endregion (メソッド)
    }
}
