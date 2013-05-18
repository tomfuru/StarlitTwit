using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace StarlitTwit
{
    public partial class FrmRenewProfileImage : Form
    {
        Image _image = null;

        public bool ProfileImageChanged { get; private set; }

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmRenewProfileImage()
        {
            InitializeComponent();
            ProfileImageChanged = false;
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region btnFileDialog_Click ...ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnFileDialog_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = Utilization.FILEFORMAT_IMAGES;
                ofd.FileName = txtImagePath.Text;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    txtImagePath.Text = ofd.FileName;
                    GetImageAndSet(ofd.FileName);
                }
            }
        }
        #endregion (btnFileDialog_Click)

        //-------------------------------------------------------------------------------
        #region btnUpdateImage_Click 更新ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnUpdateImage_Click(object sender, EventArgs e)
        {
            Debug.Assert(_image != null);

            try {
                FrmMain.Twitter.account_update_profile_image(_image, Path.GetFileName(txtImagePath.Text));
                Message.ShowInfoMessage("更新しました。");
                ProfileImageChanged = true;
            }
            catch (TwitterAPIException ex) {
                Message.ShowWarningMessage("更新に失敗しました。", Twitter.MakeTwitterAPIExceptionStr(ex));
            }
        }
        #endregion (btnUpdateImage_Click)

        //-------------------------------------------------------------------------------
        #region btnClose_Click 閉じるボタン
        //-------------------------------------------------------------------------------
        //
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnClose_Click)

        //-------------------------------------------------------------------------------
        #region FrmRenewProfileImage_FormClosing 閉じた時
        //-------------------------------------------------------------------------------
        //
        private void FrmRenewProfileImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_image != null) { _image.Dispose(); }
        }
        #endregion (FrmRenewProfileImage_FormClosing)

        //-------------------------------------------------------------------------------
        #region -GetImageAndSet 画像を取得しPictureBoxにセットします。
        //-------------------------------------------------------------------------------
        //
        private void GetImageAndSet(string filePath)
        {
            bool success = false;
            try {
                if (!File.Exists(filePath)) { Message.ShowWarningMessage("指定ファイルは存在しません。"); return; }

                FileInfo fi = new FileInfo(filePath);
                if (fi.Length > 700 * 1024) { Message.ShowWarningMessage("サイズが 700KB を超えています。"); return; }

                Image img;
                try {
                    img = Image.FromFile(filePath);
                }
                catch (ArgumentException) { Message.ShowWarningMessage("画像ファイルではありません。"); return; }

                Guid guid = img.RawFormat.Guid;
                if (!guid.Equals(ImageFormat.Jpeg.Guid) && !guid.Equals(ImageFormat.Png.Guid) && !guid.Equals(ImageFormat.Gif.Guid)) {
                    Message.ShowWarningMessage("画像の形式が不適切です。");
                    return;
                }

                if (_image != null) { _image.Dispose(); }
                _image = (Image)img.Clone();
                picbImage.Image = _image;
                btnUpdateImage.Enabled = true;
                img.Dispose();
                success = true;
            }
            finally {
                if (!success) {
                    btnUpdateImage.Enabled = false;
                    picbImage.Image = null;
                }
            }
        }
        #endregion (GetImageAndSet)
    }
}
