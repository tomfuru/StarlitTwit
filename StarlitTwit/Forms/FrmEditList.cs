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
        public ListData ListData { get; private set; }

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
            _listNames = listNames;

            this.Text = (_isNew = isNew) ? "リスト新規作成" : "リスト編集";
            _list_id = list_id;

            Debug.Assert(isNew || list_id != null, "list_idが引数に与えられていません");
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタン
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckItems()) {
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
        #region -CheckItems アイテムチェック
        //-------------------------------------------------------------------------------
        //
        private bool CheckItems()
        {
            // TODO:打ってる途中にチェック
            if (txtListName.Text.Length == 0 || _listNames.Any(str => txtListName.Text.Equals(str))) {
                Message.ShowWarningMessage("リスト名が異常です。");
                return false;
            }

            return true;
        }
        #endregion (CheckItems)

        //-------------------------------------------------------------------------------
        #region -MakeList リスト作成 using Twitter API
        //-------------------------------------------------------------------------------
        //
        private bool MakeList()
        {
            try {
                ListData = FrmMain.Twitter.lists_Create(txtListName.Text, rdbUnPublic.Checked, txtDescription.Text);
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
                ListData = FrmMain.Twitter.lists_Update(_list_id, txtListName.Text, rdbUnPublic.Checked, txtDescription.Text);
            }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (UpdateList)
    }
}
