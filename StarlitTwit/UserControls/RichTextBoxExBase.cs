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
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>履歴管理クラス</summary>
        private HistoryManager<Tuple<string, int>> _strHistory = new HistoryManager<Tuple<string, int>>(new Tuple<string, int>("", 0));
        /// <summary>履歴追加抑制</summary>
        private bool _suspendHistoryAdd = false;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

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
                else if (!e.Shift && e.KeyCode == Keys.Z) {
                    if (_strHistory.CanUndo()) {
                        this.Undo();
                    }
                    e.Handled = true;
                }
                else if ((e.Shift && e.KeyCode == Keys.Z) || e.KeyCode == Keys.Y) {
                    if (_strHistory.CanRedo()) {
                        this.Redo();
                    }
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
        #region KeepHistoryNum プロパティ：
        //-------------------------------------------------------------------------------
        private int _keepHistoryNum = 5;
        /// <summary>
        /// 履歴を保持する個数を取得または設定します。
        /// </summary>
        [DefaultValue(5)]
        [Description("履歴を保持する個数を取得または設定します。")]
        public int KeepHistoryNum
        {
            get { return _keepHistoryNum; }
            set { _keepHistoryNum = value; }
        }
        #endregion (KeepHistoryNum)
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
                if (_cutmenu != null) { _cutmenu.Click += CutMenu_Click; }
                _cutmenu = value;
                if (value != null) { value.Click += CutMenu_Click; }
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
                if (_copyMenu != null) { _copyMenu.Click -= CopyMenu_Click; }
                _copyMenu = value;
                if (value != null) { value.Click += CopyMenu_Click; }
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
                if (_pasteMenu != null) { _pasteMenu.Click -= PasteMenu_Click; }
                _pasteMenu = value;
                if (value != null) { value.Click += PasteMenu_Click; }
            }
        }
        #endregion (PasteMenu)
        //-------------------------------------------------------------------------------
        #region UndoMenu プロパティ：元に戻すメニュー
        //-------------------------------------------------------------------------------
        private ToolStripMenuItem _undoMenu;
        /// <summary>
        /// 元に戻すメニュー
        /// </summary>
        public ToolStripMenuItem UndoMenu
        {
            get { return _undoMenu; }
            set
            {
                if (_undoMenu != null) { _undoMenu.Click -= UndoMenu_Click; }
                _undoMenu = value;
                if (value != null) { value.Click += UndoMenu_Click; }
            }
        }
        #endregion (UndoMenu)
        //-------------------------------------------------------------------------------
        #region RedoMenu プロパティ：やり直しメニュー
        //-------------------------------------------------------------------------------
        private ToolStripMenuItem _redoMenu;
        /// <summary>
        /// やり直しメニュー
        /// </summary>
        public ToolStripMenuItem RedoMenu
        {
            get { return _redoMenu; }
            set
            {
                if (_redoMenu != null) { _redoMenu.Click -= RedoMenu_Click; }
                _redoMenu = value;
                if (value != null) { value.Click += RedoMenu_Click; }
            }
        }
        #endregion (RedoMenu)

        //-------------------------------------------------------------------------------
        #region ContextMenu_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (_surpressDefaultMenuChange) { return; }
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
        #region UndoMenu_Click 元に戻すメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void UndoMenu_Click(object sender, EventArgs e)
        {
            this.Undo();
        }
        #endregion (UndoMenu_Click)
        #region RedoMenu_Click やり直しメニュークリック時
        //-------------------------------------------------------------------------------
        //
        private void RedoMenu_Click(object sender, EventArgs e)
        {
            this.Redo();
        }
        #endregion (RedoMenu_Click)

        //-------------------------------------------------------------------------------
        #region #[override]OnTextChanged テキスト変更時
        //-------------------------------------------------------------------------------
        //
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (!_suspendHistoryAdd) {
                _strHistory.AddHistory(new Tuple<string, int>(this.Text, this.SelectionStart));
            }
        }
        #endregion (#[override]OnTextChanged)
        //-------------------------------------------------------------------------------
        #region +SetTextWithoutHistoryAdd 履歴追加無しでテキスト設定
        //-------------------------------------------------------------------------------
        //
        public void SetTextWithoutHistoryAdd(string str)
        {
            _suspendHistoryAdd = true;
            this.Text = str;
            _suspendHistoryAdd = false;
        }
        #endregion (+SetTextWithoutHistoryAdd)

        //-------------------------------------------------------------------------------
        #region #DefaultMenuStateChange デフォルトメニュー状態変更
        //-------------------------------------------------------------------------------
        //
        protected void DefaultMenuStateChange()
        {
            if (_undoMenu != null) {
                _undoMenu.Visible = !this.ReadOnly;
                _undoMenu.Enabled = _strHistory.CanUndo();
            }
            if (_redoMenu != null) {
                _redoMenu.Visible = !this.ReadOnly;
                _redoMenu.Enabled = _strHistory.CanRedo();
            }

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
        #region +[new]Undo 元に戻す
        //-------------------------------------------------------------------------------
        //
        public new void Undo()
        {
            if (_strHistory.CanUndo()) {
                _suspendHistoryAdd = true;
                var tup = _strHistory.Undo();
                this.Text = tup.Item1;
                this.SelectionStart = tup.Item2;
                _suspendHistoryAdd = false;
            }
        }
        #endregion (Undo)
        //-------------------------------------------------------------------------------
        #region +[new]Redo やり直し
        //-------------------------------------------------------------------------------
        //
        public new void Redo()
        {
            if (_strHistory.CanRedo()) {
                _suspendHistoryAdd = true;
                var tup = _strHistory.Redo();
                this.Text = tup.Item1;
                this.SelectionStart = tup.Item2;
                _suspendHistoryAdd = false;
            }
        }
        #endregion (Redo)
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
