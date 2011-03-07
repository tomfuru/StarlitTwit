using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class TestPermanentToolTip : ToolTip
    {
        Timer _t = new Timer();
        public TestPermanentToolTip()
        {
            InitializeComponent();
            _t.Tick += new EventHandler(StopTimer);
            _t.Interval = 1000;
        }

        private void TestPermanentToolTip_Popup(object sender, PopupEventArgs e)
        {
            _t.Enabled = true;
        }

        private void StopTimer(object sender, EventArgs e)
        {
            StopTimer();
            _t.Enabled = false;
        }
    }
}
