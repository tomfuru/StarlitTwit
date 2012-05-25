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
    public partial class FrmInputName : Form
    {
        public FrmInputName()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get { return txtUserName.Text; }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (txtUserName.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
