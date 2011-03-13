using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace Test
{
    public partial class Form1 : Form
    {
        object _obj = new object();
        private void test1()
        {
            Thread.Sleep(500);
            int j = 0;
            lock (_obj) {
                j++;
                lock (_obj) {
                    j++;
                }
                j++;
            }
        }

        public Form1()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = richTextBox2;
            richTextBox2.LanguageOption = RichTextBoxLanguageOptions.UIFonts;

            imageList1.Images.Add("abc", new Bitmap(16, 16));
            imageList1.Images.Add("abc", new Bitmap(16, 16));

            imageList1.Images.ContainsKey("abc");

            Thread t = new Thread(test1);
            t.Start();

            int i = 0;
            lock (_obj) {
                i++;
                lock (_obj) {
                    i++;
                }
                i++;
            }


            TimeSpan ts1 = new TimeSpan(0, 1, 0);
            TimeSpan ts2 = new TimeSpan(0, 1, 1);



            Size s1 = TextRenderer.MeasureText("A", this.Font);
            Size s2 = TextRenderer.MeasureText("a", this.Font);
            Size s3 = TextRenderer.MeasureText("1", this.Font);
            Size s4 = TextRenderer.MeasureText("ｱ", this.Font);
            Size s5 = TextRenderer.MeasureText("あ", this.Font);

            Size s6 = TextRenderer.MeasureText("あ\nあ", this.Font);

            Font f1 = new Font("MS UI Gothic", 9);
            Font f2 = new Font("MS UI Gothic", 10);
            Font f3 = new Font("MS UI Gothic", 11);

            var tt = new StarlitTwit.MyToolTip(this.components) {
                DisplayControl = label4,
                ToolTipText = "aweaweaw\n4343343",
                 InitialDelay = 500,
                DisplayDuration = 0
            };


            ScrollBarConfig();

            toolTip4.SetToolTip(label2, "aaaaaaaaa\naffdfd");

            toolTip3.SetToolTip(label3, "adsadasdasda\n\nadasd");

            //testPermanentToolTip1.SetToolTip(label4, "sadad\nawdas");

            //toolTip1.SetToolTip(label3, "aaaaaa");

            //toolTip2.SetToolTip(label2);

            //toolTip2.ImageURLs = new string[]{@"http://a2.twimg.com/profile_images/1128242966/______03130x_normal.jpg"
            //,@"http://a1.twimg.com/profile_images/996833013/____3-______normal.png"};

            //toolTip3.SetToolTip(button1, "aaaa\nbbbb\ncccc");

            imageList.Images.Add("load",Test.Properties.Resources.NowLoadingL);

            pictureBox1.Image = imageList.Images["load"];
        }

        private ImageList imageList = new ImageList() { ImageSize = new Size(192, 192) };

        private void ScrollBarConfig()
        {

        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(_img, 0, 0);


        }
        Image _img = null;
        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            string url = @"http://a2.twimg.com/profile_images/1128242966/______03130x_normal.jpg";
            // 画像読込
            WebClient wc = new WebClient();
            Stream stream;
            try { stream = wc.OpenRead(url); }
            catch (WebException) { return; }
            try { _img = Image.FromStream(stream); }
            catch (Exception) { return; }
            stream.Dispose();

            e.ToolTipSize = _img.Size;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolTip4.SetToolTip(label2, "awjeoawjrowj");
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            b = false;
        }

        bool b = false;
        private void toolTip4_Popup(object sender, PopupEventArgs e)
        {
            ToolTip tt = (ToolTip)sender;
            if (b) {

            }
            else {
                tt.Hide(label2);
                tt.Show("わふー", label2, 0);
                b = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label5.Text = "Cursor:" + Cursor.Position.ToString();

            Point p1 = label5.PointToClient(Control.MousePosition);
            Point p2 = label5.Location;
            Point p3 = new Point(p1.X + p2.X, p1.Y + p2.Y);
            Point p4 = Point.Add(p1, new Size(p2));
            label6.Text = "Form:" + p4.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Transaction);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Transaction()
        {
            //toolStripStatusLabel1.Text = "あｓだ";
            toolStripStatusLabelEx1.SetText("あｄさだ");
            toolStripStatusLabelEx1.SetText("ｓだじょ");
        }
    }
}

// "http://a2.twimg.com/profile_images/1128242966/______03130x_normal.jpg";
// 