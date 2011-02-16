using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwitterClient
{
    public class NumericUpDownExtended :NumericUpDown
    {
        public NumericUpDownExtended()
        {
            this.Accelerations.Clear();
            //this.Accelerations.Add(new NumericUpDownAcceleration(0, 10));
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x20A) {
                int wheeldelta = ((int)m.WParam >> 16);
                if (wheeldelta > 0) {
                    if (this.Value + this.Increment > this.Maximum) {
                        this.Value = this.Maximum;
                    }
                    else {
                        this.Value += this.Increment;
                    }
                }
                else {
                    if (this.Value - this.Increment < this.Minimum) {
                        this.Value = this.Minimum;
                    }
                    else {
                        this.Value -= this.Increment;
                    }
                }
            }
            else {
                base.WndProc(ref m);
            }
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
