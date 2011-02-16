using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class FormListView : Form
    {
        public FormListView()
        {
            InitializeComponent();
        }

        private void FormListView_Load(object sender, EventArgs e)
        {
            ImageList imglist = new ImageList();
            

            ListViewItem item = new ListViewItem("a", "b");
            
            //listView1.Items.Add(
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
