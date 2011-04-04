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
    public partial class TabPageEx : TabPage
    {
        string _dispText = "";
        //-------------------------------------------------------------------------------
        #region Text プロパティ：
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
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
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            AdjustWidth();
        }
        #endregion (#OnTextChanged)

        //-------------------------------------------------------------------------------
        #region -AdjustWidth 幅を調節
        //-------------------------------------------------------------------------------
        /// <remarks>
        /// width(i^n) = 5+3n
        /// width(Tab) = width(Text) + 7
        /// min(width(Tab)) = 60
        /// ---------------------------
        /// ～54 : ""
        /// 55,56,57 : i*17 = 63
        /// ・・・
        /// </remarks>
        private void AdjustWidth()
        {
            int width = TextRenderer.MeasureText(Text, this.Font).Width;
            int iNum = Math.Max((width - 4) / 3, 0);
            base.Text = new string('i', iNum);
        }
        #endregion (AdjustWidth)
    }
}
