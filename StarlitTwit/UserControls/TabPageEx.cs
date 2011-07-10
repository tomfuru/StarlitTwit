using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace StarlitTwit
{
    [DefaultProperty("Text")]
    public partial class TabPageEx : TabPage
    {
        string _dispText = "";
        //-------------------------------------------------------------------------------
        #region Text プロパティ：
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Localizable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        [DispId(-517)]
        public new string Text
        {
            get { return _dispText; }
            set { _dispText = value; OnTextChanged(EventArgs.Empty); }
        }
        #endregion (Text)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public TabPageEx() : base() { }
        public TabPageEx(string text) : base() { Text = text; }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region #OnTextChanged
        //-------------------------------------------------------------------------------
        //
        bool suspend = false;
        protected override void OnTextChanged(EventArgs e)
        {
            if (suspend) { return; }
            base.OnTextChanged(e);

            suspend = true;
            //this.Text = base.Text;
            AdjustWidth();
            suspend = false;
        }
        #endregion (#OnTextChanged)

        //-------------------------------------------------------------------------------
        #region %AdjustWidth 幅を調節
        //-------------------------------------------------------------------------------
        /// <remarks>
        /// width(i^n) = 5+3n
        /// min(width(Tab)) = 60
        /// ●縦
        /// width(Tab) = width(Text) + 7
        /// ---------------------------
        /// ～54 : ""
        /// 55,56,57 : i*17 = 63
        /// ・・・
        /// ●横
        /// width(Tab) = width(Text) + n
        /// ---------------------------
        /// ～55 : ""
        /// 56,57,58,59 : i*13 = 64
        /// ・・・
        /// </remarks>
        internal void AdjustWidth()
        {
            TabControlEx tab = this.Parent as TabControlEx;
            if (tab == null) { return; }

            int width = TextRenderer.MeasureText(Text, this.Font).Width;
            switch (tab.Alignment) {
                case TabAlignment.Top:
                case TabAlignment.Bottom: {
                        int iNum = Math.Max((width - 4) / 3, 0);
                        base.Text = new string('i', iNum);
                    }
                    break;
                case TabAlignment.Left:
                case TabAlignment.Right: {
                        int iNum = Math.Max((width - 4) / 4, 0);
                        base.Text = new string('i', iNum);
                    }
                    break;
            }
        }
        #endregion (AdjustWidth)

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return "TabPageEx:{" + _dispText + "}";
        }
        #endregion (ToString)
    }
}
