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
    public partial class FrmWebBrowser : Form
    {
        public FrmWebBrowser()
        {
            InitializeComponent();
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
            txtURL.Text = url;
        }
        //-------------------------------------------------------------------------------
        #endregion (SetURL)

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            
        }
    }
}
