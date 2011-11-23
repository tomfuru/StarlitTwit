using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class FrmShortcutKeyEdit : Form
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmShortcutKeyEdit()
        {
            InitializeComponent();

            keyInputGrid1.AddItems(new Tuple<string, KeyData, object>("aaa", new KeyData() { Key = Keys.B }, null).AsEnumerable());
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタン
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセルボタン
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)
    }
}
