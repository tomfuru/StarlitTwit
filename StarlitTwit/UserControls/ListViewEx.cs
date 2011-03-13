using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    class ListViewEx : ListView
    {
        public ListViewEx()
            : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
