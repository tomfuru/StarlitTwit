using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using StarlitTwit;

#pragma warning disable 414 // 未使用変数警告

namespace Test
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------
        #region ThreadTest1
        //-------------------------------------------------------------------------------
        Thread t1;
        Thread t2;
        int i1 = 0;
        int i2 = 0;

        private void f1_1()
        {
            this.Invoke(new Action(() =>
            {
                Thread.Sleep(2000);
                i1 = 1;
            }));
        }
        private void f1_2()
        {
            Thread.Sleep(1000);

            this.Invoke(new Action(() =>
            {
                i2 = 1;
            }));
        }

        int i3 = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Thread(f1_1) { IsBackground = true };
            t2 = new Thread(f1_2) { IsBackground = true };

            t1.Start();
            t2.Start();

            for (int i = 0; i < 1000000; i++) {
                i3++;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (ThreadTest1)

        //-------------------------------------------------------------------------------
        #region AnimationTest
        //-------------------------------------------------------------------------------
        ImageAnimation anim;

        private void f2_1()
        {
            Image img;
            using (Image img2 = Image.FromFile(@"H:\My Documents\Visual Studio 2010\Projects\TwitterClient\StarlitTwit\Resources\NowLoadingS.gif")) {
                img = (Image)img2.Clone();
            }
            anim = new ImageAnimation(img);
            anim.FrameUpdated += new EventHandler(Animation_FrameUpdated);
            anim.StartAnimation();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (anim == null) { f2_1(); }
            else if (anim.Animating) { anim.StopAnimation(); }
            else { anim.StartAnimation(); }
        }

        public void Animation_FrameUpdated(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (anim != null) {
                e.Graphics.DrawImage(anim.Image, 0, 0, panel1.ClientSize.Width, panel1.ClientSize.Height);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (anim != null) {
                anim.ResetAnimation();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (AnimationTest)

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            TabRenderer.DrawTabItem(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.TabItemState.Normal);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            TabRenderer.DrawTabItem(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.TabItemState.Hot);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            TabRenderer.DrawTabItem(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.TabItemState.Selected);
        }


    }
}
