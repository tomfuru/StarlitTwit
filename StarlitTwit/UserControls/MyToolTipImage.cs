﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace StarlitTwit.UserControls
{
    public class MyToolTipImage : MyToolTipBase
    {
        //-------------------------------------------------------------------------------
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>表示URL</summary>
        private string[] _imgURLs = null;
        /// <summary>表示イメージ</summary>
        private Image[] _img = null;
        /// <summary>現在表示中イメージインデックス</summary>
        private int _imgIndex = 0;
        /// <summary>表示切替タイマー</summary>
        private Timer _switchTimer = new Timer() { Interval = 3000 };
        /// <summary>画像処理時のロックオブジェクト</summary>
        private object _lockimg = new object();
        /// <summary>イメージ取得中かどうか</summary>
        private volatile bool _gettingImage = false;
        /// <summary>ロード中表示か</summary>
        private bool _dispLoading = true;
        /// <summary>ロード中画像</summary>
        private Bitmap _loadingimg;
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        /// <summary>画像の四隅の余白</summary>
        private const int PADDING = 1;
        /// <summary>四角を描くためのペン</summary>
        private static readonly Pen PEN = new Pen(new SolidBrush(Color.Black));
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region +SwitchInterval プロパティ：タイマーの切り替わり時間(ミリ秒)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像の切り替わり時間(ミリ秒)を取得または設定します。
        /// </summary>
        [Category("動作")]
        [Description("画像の切り替わり時間(ミリ秒)")]
        [DefaultValue(3000)]
        public int SwitchInterval
        {
            get { return _switchTimer.Interval; }
            set { _switchTimer.Interval = value; }
        }
        #endregion (SwitchInterval)
        //-------------------------------------------------------------------------------
        #region +MaximumSize プロパティ：画像の最大サイズ
        //-------------------------------------------------------------------------------
        private Size _maxSize = new Size(500, 500);
        /// <summary>
        /// 画像の最大サイズ
        /// </summary>
        [Description("画像の最大サイズです。")]
        [Category("配置")]
        public Size MaximumSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }
        #endregion (MaximumSize)
        //-------------------------------------------------------------------------------
        #region +ImageURLs プロパティ：画像URL
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像URLを取得します。
        /// </summary>
        [Browsable(false)]
        public string[] ImageURLs
        {
            get { return _imgURLs; }
        }
        #endregion (ImageURLs)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        #region (void)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTipImage()
            : base()
        {
            Initialize();
        }
        //-------------------------------------------------------------------------------
        #endregion ((void))
        #region (IContainer)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTipImage(IContainer cont)
            : base(cont)
        {
            Initialize();
        }
        //-------------------------------------------------------------------------------
        #endregion ((IContainer))
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region -Initialize 初期化
        //-------------------------------------------------------------------------------
        //
        private void Initialize()
        {
            _switchTimer.Tick += SwitchTimer_Tick;
        }
        #endregion (Initialize)

        //-------------------------------------------------------------------------------
        #region SwitchTimer_Tick 切り替えタイマーイベント
        //-------------------------------------------------------------------------------
        //
        private void SwitchTimer_Tick(object sender, EventArgs e)
        {
            lock (_lockimg) {
                if (_img == null || DisplayForm == null || DisplayForm.IsDisposed) {
                    _switchTimer.Stop();
                    return;
                }

                _imgIndex++;
                _imgIndex %= _img.Length;

                Size = GetPreferSize(_img[_imgIndex].Size);
            }

            ConfigDispForm();
            DisplayForm.Refresh();
        }
        #endregion (SwitchTimer_Tick)
        //-------------------------------------------------------------------------------
        #region Image_Animate 画像フレームが進んだとき
        //-------------------------------------------------------------------------------
        //
        private void Image_Animate(object sender, EventArgs e)
        {
            try {
                if (DisplayForm != null) {
                    DisplayForm.Invalidate();
                }
            }
            catch (InvalidOperationException) { }
        }
        #endregion (Image_Animate)

        //-------------------------------------------------------------------------------
        #region #[override]OnShowToolTip
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ表示時
        /// </summary>
        protected override void OnShowToolTip(CancelEventArgs e)
        {
            base.OnShowToolTip(e);

            lock (_lockimg) {
                if (_imgURLs == null || _imgURLs.Length == 0) { e.Cancel = true; return; }
                Size size;
                if (_img == null) {
                    if (!_gettingImage) {
                        _loadingimg = StarlitTwit.Properties.Resources.NowLoadingL;
                        Utilization.InvokeTransaction(() => GetImages());
                    }

                    size = StarlitTwit.Properties.Resources.NowLoadingL.Size;
                    _dispLoading = true;
                }
                else {
                    /// size config
                    size = _img[_imgIndex].Size;
                    _dispLoading = false;

                    if (!_switchTimer.Enabled) { _switchTimer.Start(); }
                }
                Size = GetPreferSize(size);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (#[override]OnShowToolTip)
        //-------------------------------------------------------------------------------
        #region #[override]OnHideToolTip
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ隠蔽時
        /// </summary>
        protected override void OnHideToolTip()
        {
            _switchTimer.Stop();
        }
        #endregion (OnHideToolTip)

        //-------------------------------------------------------------------------------
        #region #[override]Disp_Paint 表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Disp_Paint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;

            lock (_lockimg) {
                if (!_dispLoading && _img == null) { return; }
                Graphics g = e.Graphics;
                g.Clear(c.BackColor);
                Rectangle drawrect = e.ClipRectangle;
                g.DrawRectangle(PEN, 0, 0, drawrect.Width - 1, drawrect.Height - 1);
                if (_dispLoading) { ImageAnimator.UpdateFrames(_loadingimg); Console.Write("l"); }
                Image img = (_dispLoading) ? _loadingimg : _img[_imgIndex];
                g.DrawImage(img, PADDING, PADDING, drawrect.Width - PADDING * 2, drawrect.Height - PADDING * 2);
            }
        }
        #endregion (Disp_Paint)

        //-------------------------------------------------------------------------------
        #region +SetImageURLs 画像URLを設定します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像URLを設定します。
        /// </summary>
        /// <param name="urls">URL</param>
        public void SetImageURLs(IEnumerable<string> urls)
        {
            lock (_lockimg) {
                _imgURLs = PictureGetter.ConvertURLs(urls).ToArray();
                _imgIndex = 0;
                DisposeImages();
            }
        }
        #endregion (+SetImageURLs)
        //-------------------------------------------------------------------------------
        #region +ClearImageURLs 画像URLをクリアします。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像URLをクリアします。
        /// </summary>
        public void ClearImageURLs()
        {
            lock (_lockimg) {
                _imgURLs = null;
                _imgIndex = 0;
                DisposeImages();
            }
        }
        #endregion (ClearImageURLs)

        //-------------------------------------------------------------------------------
        #region -GetPreferSize 画像サイズから好ましいサイズを取得します。
        //-------------------------------------------------------------------------------
        //
        private Size GetPreferSize(Size size)
        {
            double max_ratio = (double)_maxSize.Height / (double)_maxSize.Width;
            double ratio = (double)size.Height / (double)size.Width;

            if (ratio > max_ratio) {
                if (size.Height > _maxSize.Height) {
                    size = new Size((int)Math.Round(_maxSize.Width / ratio), _maxSize.Height);
                }
            }
            else {
                if (size.Width > _maxSize.Width) {
                    size = new Size(_maxSize.Width, (int)Math.Round(_maxSize.Height * ratio));
                }
            }

            return new Size(size.Width + PADDING * 2, size.Height + PADDING * 2);
        }
        #endregion (GetPreferSize)

        //-------------------------------------------------------------------------------
        #region -GetImages 画像を取得します。(別スレッド)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を取得します。
        /// </summary>
        private void GetImages()
        {
            EventHandler evh = new EventHandler(Image_Animate);
            ImageAnimator.Animate(_loadingimg, evh);

            List<Image> list = new List<Image>();
            foreach (var url in _imgURLs) {
                Image img = Utilization.GetImageFromURL(url);
                if (img != null) { list.Add(img); }
            }

            if (list.Count > 0) {
                lock (_lockimg) { _img = list.ToArray(); }
                if (DisplayForm != null) {
                    CancelEventArgs e = new CancelEventArgs();
                    OnShowToolTip(e);
                    if (!e.Cancel) {
                        try {
                            DisplayForm.Invoke(new Action(() => ConfigDispForm()));
                            DisplayForm.Invalidate();
                        }
                        catch (NullReferenceException){}
                        catch (InvalidOperationException) { }
                    }
                }
            }
            else { lock (_lockimg) { _img = null; } }

            ImageAnimator.StopAnimate(_loadingimg, evh);
            _gettingImage = false;
        }
        //-------------------------------------------------------------------------------
        #endregion (GetImages)
        //-------------------------------------------------------------------------------
        #region -DisposeImages 画像を初期化します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 取得した画像を全て破棄します。
        /// </summary>
        private void DisposeImages()
        {
            if (_img != null) {
                for (int i = 0; i < _img.Length; i++) {
                    if (_img[i] == null) { continue; }
                    _img[i].Dispose();
                }
                _img = null;
            }
        }
        #endregion (DisposeImages)
    }
}

