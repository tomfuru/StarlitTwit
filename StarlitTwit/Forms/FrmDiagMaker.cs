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
    public partial class FrmDiagMaker : Form
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmDiagMaker()
        {
            InitializeComponent();
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region btnClose_Click 閉じる
        //-------------------------------------------------------------------------------
        //
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnClose_Click)

        //-------------------------------------------------------------------------------
        #region txtName_KeyDown
        //-------------------------------------------------------------------------------
        //
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                btn.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        #endregion (txtName_KeyDown)

        private void btnTweet_Click(object sender, EventArgs e)
        {

        }
    }
}
