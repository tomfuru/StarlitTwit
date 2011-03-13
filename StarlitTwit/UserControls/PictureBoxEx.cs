using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    /// <summary>
    /// 拡張Picturebox
    /// </summary>
    public class PictureBoxEx : PictureBox
    {
        /// <summary>再読込回数カウントダウン</summary>
        private int _iReRead_RestTime = 0;
        private string _imageKey = null;
        private Timer _timerSetPicture = new Timer() { Interval = 100 };
        public ImageListWrapper ImageListWrapper { get; set; }

        /// <summary>画像取得時間</summary>
        private const int PICTURE_REREAD_TIME = 10000;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public PictureBoxEx()
            : base()
        {
            _timerSetPicture.Tick += timerSetPicture_Tick;
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region +SetFromImageListWrapper ImageListWrapperから画像を取得
        //-------------------------------------------------------------------------------
        //
        public void SetFromImageListWrapper(string imagekey)
        {
            if (string.IsNullOrEmpty(imagekey)) { return; }

            if (ImageListWrapper.ImageContainsKey(imagekey)) {
                this.Image = ImageListWrapper.GetImage(imagekey);
                this.Visible = true;
                return;
            }
            this.Image = StarlitTwit.Properties.Resources.NowLoadingS;
            _iReRead_RestTime = PICTURE_REREAD_TIME;
            _imageKey = imagekey;

            _timerSetPicture.Start();
        }
        #endregion (SetFromImageListWrapper)

        //-------------------------------------------------------------------------------
        #region +CanselSetImage 画像取得キャンセル
        //-------------------------------------------------------------------------------
        //
        public void CanselSetImage()
        {
            _timerSetPicture.Stop();
        }
        #endregion (CanselSetImage)

        //-------------------------------------------------------------------------------
        #region timerSetPicture_Tick タイマー
        //-------------------------------------------------------------------------------
        //
        private void timerSetPicture_Tick(object sender, EventArgs e)
        {
            // 画像読み込み
            if (ImageListWrapper.ImageContainsKey(_imageKey)) {
                this.Image = ImageListWrapper.GetImage(_imageKey);
                this.Visible = true;
                _timerSetPicture.Enabled = false;
            }

            _iReRead_RestTime -= _timerSetPicture.Interval;
            if (_iReRead_RestTime <= 0) {
                // 終了
                this.Image = StarlitTwit.Properties.Resources.cross;
                this.Visible = true;
                _timerSetPicture.Enabled = false;
            }
        }
        #endregion (timerSetPicture_Tick)
    }
}
