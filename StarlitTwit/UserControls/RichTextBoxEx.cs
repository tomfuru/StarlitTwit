using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class RichTextBoxEx : RichTextBoxExBase
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public RichTextBoxEx()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region contextMenu_Opening
        //-------------------------------------------------------------------------------
        //
        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            tsSeparator.Visible = !this.ReadOnly;
        }
        #endregion (contextMenu_Opening)
    }
}
