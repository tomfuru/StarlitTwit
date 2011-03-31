using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class FrmUserStreamWatch : Form
    {
        public FrmUserStreamWatch()
        {
            InitializeComponent();
        }

        private void FrmUserStreamWatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        public void AddItem(string item)
        {
            Action action = () =>
            {
                listBox.Items.Add(item);
                if (chbAutoScroll.Checked) {
                    listBox.TopIndex = listBox.Items.Count - 1;
                }
            };

            if (listBox.InvokeRequired) {
                listBox.Invoke(action);
            }
            else { action(); }
        }
    }
}
