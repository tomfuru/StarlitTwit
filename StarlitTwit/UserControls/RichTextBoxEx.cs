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
    public partial class RichTextBoxEx : RichTextBox
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public RichTextBoxEx()
        {
            InitializeComponent();
            this.LanguageOption = RichTextBoxLanguageOptions.UIFonts;

            this.tsmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiCopy.Click += tsmiCopy_Click;
            this.tsmiCopy.Text = "コピー(&C)";
            this.tsmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.tsmiCut.Click += tsmiCut_Click;
            this.tsmiCut.Text = "切り取り(&T)";
            this.tsmiPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tsmiPaste.Click += tsmiPaste_Click;
            this.tsmiPaste.Text = "貼り付け(&P)";
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region Event
        //-------------------------------------------------------------------------------
        #region contextMenu_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            bool existSelection = (SelectionLength > 0);
            tsmiCopy.Enabled = existSelection;
            if (tsmiPaste.Visible = tsmiCut.Visible = !this.ReadOnly) {
                tsmiCut.Enabled = existSelection;
                tsmiPaste.Enabled = CanPaste();
            }
        }
        #endregion (contextMenu_Opening)
        //-------------------------------------------------------------------------------
        #region tsmiCopy_Click コピーメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (this.SelectionLength > 0) {
                this.Copy();
                Clipboard.SetText(Clipboard.GetText(TextDataFormat.Text));
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmiCopy_Click)
        //-------------------------------------------------------------------------------
        #region tsmiCut_Click 切り取りメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiCut_Click(object sender, EventArgs e)
        {
            if (this.SelectionLength > 0) {
                this.Cut();
                Clipboard.SetText(Clipboard.GetText(TextDataFormat.Text));
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmiCut_Click)
        //-------------------------------------------------------------------------------
        #region tsmiPaste_Click 貼り付けメニュークリック
        //-------------------------------------------------------------------------------
        //
        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            this.Paste(DataFormats.GetFormat(DataFormats.Text));
        }
        //-------------------------------------------------------------------------------
        #endregion (tsmiCopy_Click)
        //-------------------------------------------------------------------------------
        #endregion (Event)

        //-------------------------------------------------------------------------------
        #region %[override]OnKeyDown キーダウン時
        //-------------------------------------------------------------------------------
        //
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Control) {
                if (e.KeyCode == Keys.V) {
                    this.Paste(DataFormats.GetFormat(DataFormats.Text));
                    e.Handled = true;
                }
            }
        }
        #endregion (%[override]OnKeyDown)

        //-------------------------------------------------------------------------------
        #region %CanPaste 貼り付け可能か
        //-------------------------------------------------------------------------------
        //
        protected bool CanPaste()
        {
            var data = Clipboard.GetDataObject();
            return data.GetDataPresent(DataFormats.Text);
        }
        #endregion (CanPaste)
    }
}
