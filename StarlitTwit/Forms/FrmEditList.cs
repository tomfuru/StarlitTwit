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
    public partial class FrmEditList : Form
    {
        private IEnumerable<string> _listNames = null;
        private bool _isNew;
        private string _list_id;
        /// <summary>作成/更新されたリストデータ</summary>
        public ListData ListData { get; set; }

        //-------------------------------------------------------------------------------
        #region Constructor コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="isNew">新規作成:true,編集:false</param>
        /// <param name="listNames">存在するリストの一覧</param>
        public FrmEditList(bool isNew, IEnumerable<string> listNames, string list_id = null)
        {
            InitializeComponent();
            lblWarning.Text = "";
            _listNames = listNames;

            this.Text = (_isNew = isNew) ? "リスト新規作成" : "リスト編集";
            _list_id = list_id;

            Debug.Assert(isNew || list_id != null, "list_idが引数に与えられていません");
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            if (!_isNew) {
                txtListName.Text = ListData.Name;
                txtDescription.Text = ListData.Description;
                rdbUnPublic.Checked = !ListData.Public;
            }
            // 何故かCenterParentにならないので一応
            Utilization.SetModelessDialogCenter(this);
        }
        #endregion (OnLoad)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタン
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_isNew) {
                if (!MakeList()) {
                    Message.ShowWarningMessage("作成に失敗しました。");
                    return;
                }
                Message.ShowInfoMessage("リストを作成しました。");
            }
            else {
                if (!UpdateList()) {
                    Message.ShowWarningMessage("更新に失敗しました。");
                    return;
                }
                Message.ShowInfoMessage("リストを更新しました。");
            }
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)

        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセルボタン
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnCansel_Click)

        //-------------------------------------------------------------------------------
        #region txtListName_TextChanged リスト名テキスト変更時
        //-------------------------------------------------------------------------------
        //
        private void txtListName_TextChanged(object sender, EventArgs e)
        {
            if (txtListName.Text.Length == 0) {
                lblWarning.Text = "リスト名を入力してください";
                btnOK.Enabled = false;
            }
            else if (!txtListName.Text.Equals(_list_id) && _listNames.Any(str => txtListName.Text.Equals(str))) {
                lblWarning.Text = "既に使用されているリスト名です";
                btnOK.Enabled = false;
            }
            else {
                lblWarning.Text = "";
                btnOK.Enabled = true;
            }
        }
        #endregion (txtListName_TextChanged)

        //-------------------------------------------------------------------------------
        #region -MakeList リスト作成 using Twitter API
        //-------------------------------------------------------------------------------
        //
        private bool MakeList()
        {
            try {
                ListData = FrmMain.Twitter.lists_create(txtListName.Text, rdbUnPublic.Checked, txtDescription.Text);
            }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (MakeList)

        //-------------------------------------------------------------------------------
        #region -UpdateList リスト更新 using Twitter API
        //-------------------------------------------------------------------------------
        //
        private bool UpdateList()
        {
            try {
                ListData = FrmMain.Twitter.lists_update(slug: _list_id, name: txtListName.Text, isPrivate: rdbUnPublic.Checked, description: txtDescription.Text, owner_id: FrmMain.Twitter.ID);
            }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (UpdateList)

    }
}
