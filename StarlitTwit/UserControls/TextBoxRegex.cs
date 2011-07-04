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
    public partial class TextBoxRegex : TextBox
    {
        public TextBoxRegex()
        {
            InitializeComponent();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            using (FrmInputRegex frm = new FrmInputRegex()) {
                frm.RegexText = this.Text;
                if (frm.ShowDialog() == DialogResult.OK) {
                    this.Text = frm.RegexText;
                }
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            
            this.BackColor = (this.Enabled) ? Color.Snow : SystemColors.Control;
        }
    }
}
