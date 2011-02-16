using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwitterClient
{
    public partial class FrmAuthWebBrowser : Form
    {
        public string PIN { get; private set; }

        public FrmAuthWebBrowser()
        {
            InitializeComponent();
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            PIN = txtPin.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        //-------------------------------------------------------------------------------
        #region +SetURL WebBrowserにURLをセット
        //-------------------------------------------------------------------------------
        /// <summary>
        /// WebBrowserにURLをセット
        /// </summary>
        public void SetURL(string url)
        {
            webBrowser1.Url = new Uri(url);
        }
        //-------------------------------------------------------------------------------
        #endregion (SetURL)
    }
}
