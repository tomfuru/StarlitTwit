/*
 * メモ
 * ・ContorolsのIndexは，0から順につけられ，重複するとそれ以上のものを1ずらす．
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwitterClient;

namespace Test
{
    public partial class FormFlowTest : Form
    {
        public FormFlowTest()
        {
            InitializeComponent();
        }

        int i = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            UctlDispTwitRow r = new UctlDispTwitRow(new TwitData());
            r.BorderStyle = BorderStyle.FixedSingle;
            r.Name = "Row" + i.ToString();
            r.SetNameLabel();
            flowLayoutPanel1.Controls.Add(r);
            flowLayoutPanel1.Controls.SetChildIndex(r, 1);
            i++;

            foreach (Control ctl in flowLayoutPanel1.Controls) {
                Console.WriteLine(ctl.Name + " : " + flowLayoutPanel1.Controls.GetChildIndex(ctl).ToString()); 
            }
            Console.WriteLine("----");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count > 0) {
                flowLayoutPanel1.Controls.RemoveAt(0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in flowLayoutPanel1.Controls) {
                if (!(ctl is UctlDispTwitRow)) { continue; }
                UctlDispTwitRow r = (UctlDispTwitRow)ctl;
                //r.SetTweetLabel();
            }
        }
    }
}
