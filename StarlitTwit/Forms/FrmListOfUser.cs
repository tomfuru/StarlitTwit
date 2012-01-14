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
    public partial class FrmListOfUser : Form
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        private readonly FrmMain _mainForm;
        private readonly string _screen_name;
        private ListData[] _listdata = null;
        private long _cursor = -1;
        private Dictionary<string, CheckBox> _checkboxdic = new Dictionary<string, CheckBox>();
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmListOfUser(FrmMain mainForm, string screen_name)
        {
            InitializeComponent();

            Debug.Assert(mainForm != null);
            _mainForm = mainForm;

            Debug.Assert(!string.IsNullOrEmpty(screen_name));
            _screen_name = screen_name;

            this.Text = string.Format("{0}の所属しているリスト", screen_name);
            tssLabel.Text = "";
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Utilization.InvokeTransaction(GetData);
        }
        #endregion (#[override]OnLoad)

        //-------------------------------------------------------------------------------
        #region btnRetry_Click 再試行ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnRetry_Click(object sender, EventArgs e)
        {
            btnRetry.Visible = false;
            Utilization.InvokeTransaction(GetData);
        }
        #endregion (btnRetry_Click)

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
        #region chb_list_CheckedChanged リスト項目チェックボックスチェック変更時
        //-------------------------------------------------------------------------------
        //
        private void chb_list_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = sender as CheckBox;
            if (chb == null) { Debug.Assert(false); return; }
            ListData listdata = (ListData)chb.Tag;

            if (chb.Checked) {
                try {
                    this.Invoke(new Action(() => tssLabel.Text = "リストに追加中..."));
                    this.Refresh();
                    FrmMain.Twitter.list_members_create(listdata.ID, screen_name: _screen_name);
                    this.Invoke(new Action(() => tssLabel.Text = "追加完了しました。"));
                }
                catch (TwitterAPIException) {
                    this.Invoke(new Action(() => tssLabel.Text = "追加に失敗しました。"));
                    chb.Checked = false;
                }
            }
            else {
                try {
                    this.Invoke(new Action(() => tssLabel.Text = "リストから削除中..."));
                    this.Refresh();
                    FrmMain.Twitter.list_members_destroy(listdata.ID, screen_name: _screen_name);
                    this.Invoke(new Action(() => tssLabel.Text = "削除完了しました。"));
                }
                catch (TwitterAPIException) {
                    this.Invoke(new Action(() => tssLabel.Text = "削除に失敗しました。"));
                    chb.Checked = true;
                }
            }
        }
        #endregion (chb_list_CheckedChanged)

        //-------------------------------------------------------------------------------
        #region -GetData データ取得・設定
        //-------------------------------------------------------------------------------
        //
        private void GetData()
        {
            try {
                try {
                    if (_listdata == null) { GetLists(); }
                    GetMemberList();
                    this.Invoke(new Action(() => tssLabel.Text = "取得完了しました。"));
                }
                catch (TwitterAPIException) {
                    this.Invoke(new Action(() =>
                    {
                        tssLabel.Text = "取得に失敗しました。";
                        btnRetry.Visible = true;
                    }));
                    return;
                }
            }
            catch (InvalidOperationException) { }
        }
        #endregion (GetData)

        //-------------------------------------------------------------------------------
        #region -GetLists リスト取得
        //-------------------------------------------------------------------------------
        //
        private void GetLists()
        {
            this.Invoke(new Action(() => tssLabel.Text = "リスト一覧取得中..."));

            IEnumerable<ListData> lists = Utilization.EmptyIEnumerable<ListData>();
            do {
                var seqdata = FrmMain.Twitter.lists();
                _cursor = seqdata.NextCursor;
                lists = lists.Concat(seqdata.Data);
            } while (_cursor != 0);

            _cursor = -1;
            _listdata = lists.ToArray();

            // control作成
            int index = 0;
            foreach (var list in lists) {
                CheckBox chb = new CheckBox() {
                    AutoSize = true,
                    Location = new Point(7, 7 + 22 * index),
                    Text = string.Format("{0}{1}", (list.Public) ? "(public)" : "(private)", list.Name),
                    Enabled = false,
                    Tag = list
                };
                chb.CheckedChanged += chb_list_CheckedChanged;

                _checkboxdic.Add(list.Slug, chb);
                index++;
            }
            this.Invoke(new Action(() =>_checkboxdic.Values.ForEach(chb => this.pnlCheckbox.Controls.Add(chb))));
        }
        #endregion (GetLists)

        //-------------------------------------------------------------------------------
        #region -GetMemberList 所属リスト取得
        //-------------------------------------------------------------------------------
        //
        private void GetMemberList()
        {
            this.Invoke(new Action(() => tssLabel.Text = "リスト所属データ取得中..."));

            IEnumerable<ListData> lists = Utilization.EmptyIEnumerable<ListData>();
            do {
                var seqdata = FrmMain.Twitter.lists_memberships(screen_name: _screen_name, cursor: _cursor, filter_to_owner_lists: true);
                _cursor = seqdata.NextCursor;
                lists = lists.Concat(seqdata.Data);
            } while (_cursor != 0);

            this.Invoke(new Action(() =>
            {
                foreach (var list in lists) {
                    if (!_checkboxdic.Keys.Contains(list.Slug)) { Debug.Assert(false); continue; }
                    _checkboxdic[list.Slug].Checked = true;
                }

                _checkboxdic.Values.ForEach(chb => chb.Enabled = true);
            }));
        }
        #endregion (GetMemberList)
    }
}
