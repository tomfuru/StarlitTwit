using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace StarlitTwit
{
    /// <summary>
    /// Rtf形式の入力を制限したRichTextBox
    /// </summary>
    public class RichTextBoxExBase : RichTextBox
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public RichTextBoxExBase()
        {
            this.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
        }
        #endregion (Constructor)

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
        #region ContextMenu プロパティ：メニュー
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このコントロールに関連付けられているContextMenuStripを取得または設定します。
        /// </summary>
        public new ContextMenuStrip ContextMenuStrip
        {
            get { return base.ContextMenuStrip; }
            set
            {
                if (base.ContextMenuStrip != null) {
                    base.ContextMenuStrip.Opening -= ContextMenu_Opening;
                }
                base.ContextMenuStrip = value;
                value.Opening += ContextMenu_Opening;
            }
        }
        #endregion (ContextMenu)
        //-------------------------------------------------------------------------------
        #region SurpressDefaultMenuStateChange プロパティ：
        //-------------------------------------------------------------------------------
        private bool _surpressDefaultMenuChange;
        /// <summary>
        /// 切り取り・コピー・貼りつけメニューのデフォルトのEnable,Visible変更を抑制します。
        /// </summary>
        [DefaultValue(false)]
        [Description("切り取り・コピー・貼りつけメニューのデフォルトのEnable,Visible変更を抑制します。")]
        public bool SurpressDefaultMenuStateChange
        {
            get { return _surpressDefaultMenuChange; }
            set { _surpressDefaultMenuChange = value; }
        }
        #endregion (SurpressDefaultMenuStateChange)
        //-------------------------------------------------------------------------------
        #region CutMenu プロパティ：切り取りメニュー
        //-------------------------------------------------------------------------------
        private ToolStripItem _cutmenu;
        /// <summary>
        /// 切り取りメニュー
        /// </summary>
        public ToolStripItem CutMenu
        {
            get { return _cutmenu; }
            set
            {
                if (_cutmenu != null) {
                    _cutmenu.Click += CutMenu_Click;
                }
                _cutmenu = value;
                value.Click += CutMenu_Click;
            }
        }
        #endregion (CutMenu)
        //-------------------------------------------------------------------------------
        #region CopyMenu プロパティ：コピーメニュー
        //-------------------------------------------------------------------------------
        private ToolStripItem _copyMenu;
        /// <summary>
        /// コピーメニュー
        /// </summary>
        public ToolStripItem CopyMenu
        {
            get { return _copyMenu; }
            set
            {
                if (_copyMenu != null) {
                    _copyMenu.Click -= CopyMenu_Click;
                }
                _copyMenu = value;
                value.Click += CopyMenu_Click;
            }
        }
        #endregion (CopyMenu)
        //-------------------------------------------------------------------------------
        #region PasteMenu プロパティ：貼り付けメニュー
        //-------------------------------------------------------------------------------
        private ToolStripItem _pasteMenu;
        /// <summary>
        /// 貼り付けメニュー
        /// </summary>
        public ToolStripItem PasteMenu
        {
            get { return _pasteMenu; }
            set
            {
                if (_pasteMenu != null) {
                    _pasteMenu.Click -= PasteMenu_Click;
                }
                _pasteMenu = value;
                value.Click += PasteMenu_Click;
            }
        }
        #endregion (PasteMenu)

        //-------------------------------------------------------------------------------
        #region ContextMenu_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            DefaultMenuStateChange();
        }
        #endregion (ContextMenu_Opening)
        //-------------------------------------------------------------------------------
        #region CutMenu_Click 切り取りメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void CutMenu_Click(object sender, EventArgs e)
        {
            this.Cut();
        }
        #endregion (CutMenu_Click)
        #region CopyMenu_Click コピーメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void CopyMenu_Click(object sender, EventArgs e)
        {
            this.Copy();
        }
        #endregion (CopyMenu_Click)
        #region PasteMenu_Click 貼り付けメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void PasteMenu_Click(object sender, EventArgs e)
        {
            this.Paste();
        }
        #endregion (PasteMenu_Click)

        //-------------------------------------------------------------------------------
        #region #DefaultMenuStateChange デフォルトメニュー状態変更
        //-------------------------------------------------------------------------------
        //
        protected void DefaultMenuStateChange()
        {
            if (_surpressDefaultMenuChange) { return; }

            bool existSelection = (SelectionLength > 0);
            if (_copyMenu != null) {
                _copyMenu.Enabled = existSelection;
            }
            if (_cutmenu != null) {
                _cutmenu.Visible = !this.ReadOnly;
                _cutmenu.Enabled = existSelection;
            }
            if (_pasteMenu != null) {
                _pasteMenu.Visible = !this.ReadOnly;
                _pasteMenu.Enabled = CanPaste();
            }
        }
        #endregion (DefaultMenuStateChange)
        //-------------------------------------------------------------------------------
        #region +[new]Cut 切り取り
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 選択部分を切り取り、クリップボードにコピーします。
        /// </summary>
        public new void Cut()
        {
            if (this.SelectionLength > 0) {
                base.Cut();
                Clipboard.SetText(Clipboard.GetText(TextDataFormat.Text));
            }
        }
        #endregion (Cut)
        //-------------------------------------------------------------------------------
        #region +[new]Copy コピー
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 選択部分をクリップボードにコピーします。
        /// </summary>
        public new void Copy()
        {
            if (this.SelectionLength > 0) {
                base.Copy();
                Clipboard.SetText(Clipboard.GetText(TextDataFormat.Text));
            }
        }
        #endregion (Copy)
        //-------------------------------------------------------------------------------
        #region +[new]Paste 貼りつけ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ペーストを行います。
        /// </summary>
        public new void Paste()
        {
            this.Paste(DataFormats.GetFormat(DataFormats.Text));
        }
        #endregion (Paste)
        //-------------------------------------------------------------------------------
        #region +CanPaste 貼り付け可能か
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ペーストが可能かどうかを返します。
        /// </summary>
        /// <returns>可能かどうか</returns>
        public bool CanPaste()
        {
            var data = Clipboard.GetDataObject();
            return data.GetDataPresent(DataFormats.Text);
        }
        #endregion (CanPaste)
    }
}
