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
    public partial class PermanentToolTip : ToolTip
    {
        const int DURATION = 10000;
        Control _control;
        string _text;

        public PermanentToolTip() : base()
        {
            InitializeComponent();
            AutoPopDelay = DURATION;
        }
        public PermanentToolTip(IContainer cont)
            : base(cont)
        {
            InitializeComponent();
            AutoPopDelay = DURATION;
        }

        private void PermanentToolTip_Popup(object sender, PopupEventArgs e)
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Show(_text, _control, DURATION);
        }

        //-------------------------------------------------------------------------------
        #region Parent_MouseLeave 親コントロールから離れた時
        //-------------------------------------------------------------------------------
        //
        private void Parent_MouseLeave(object sender, EventArgs e)
        {
            this.Hide(_control);
            timer.Enabled = false;
        }
        //-------------------------------------------------------------------------------
        #endregion (Parent_MouseLeave)

        //-------------------------------------------------------------------------------
        #region +SetToolTip ツールチップ表示セット
        //-------------------------------------------------------------------------------
        //
        public new void SetToolTip(Control control,string text)
        {
            base.SetToolTip(control,text);

            if (_control != null) {
                _control.MouseLeave += Parent_MouseLeave;
            }
            _control = control;
            _control.MouseLeave += Parent_MouseLeave;

            _text = text;
        }
        //-------------------------------------------------------------------------------
        #endregion
    }
}
