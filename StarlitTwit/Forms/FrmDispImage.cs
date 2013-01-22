using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace StarlitTwit
{
    public partial class FrmDispImage : Form
    {
        private string _imageUrl;

        private readonly int _frame_width;
        private readonly int _frame_height;

        private readonly Image _loadingImg;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmDispImage(string imageUrl)
        {
            InitializeComponent();

            Debug.Assert(imageUrl != null);
            this.Text = imageUrl;
            _imageUrl = imageUrl;

            _frame_width = this.Width - this.ClientSize.Width;
            _frame_height = this.Height - this.ClientSize.Height;

            _loadingImg = (Image)Properties.Resources.NowLoadingL.Clone();
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region FrmDispImage_Load
        //-------------------------------------------------------------------------------
        //
        private void FrmDispImage_Load(object sender, EventArgs e)
        {
            picbImage.Image = _loadingImg;

            Thread th = new Thread(GetImage);
            th.IsBackground = false;
            th.Start();
        }
        #endregion (FrmDispImage_Load)

        //-------------------------------------------------------------------------------
        #region (別スレッド)GetImage
        //-------------------------------------------------------------------------------
        //
        private void GetImage()
        {
            string url = PictureGetter.ConvertURL(_imageUrl);
            Image img = Utilization.GetImageFromURL(url);
            if (img != null) {
                this.Invoke((Action)(() =>
                {
                    this.Width = img.Width + _frame_width;
                    this.Height = img.Height + _frame_height;
                    picbImage.Image = img;
                }));
            }
            else {
                this.Invoke((Action)(() =>
                {
                    this.Width = Properties.Resources.failed.Width + _frame_width;
                    this.Height = Properties.Resources.failed.Height + _frame_height;
                    picbImage.Image = Properties.Resources.failed;
                }));
            }
        }
        #endregion (GetImage)
    }
}
