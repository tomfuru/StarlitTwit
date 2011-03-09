using System;
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
        private Timer _timer = new Timer() { Interval = 3000 };
        /// <summary>画像処理時のロックオブジェクト</summary>
        private object _lockimg = new object();
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
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
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
            _timer.Tick += new EventHandler(Timer_Tick);
        }
        #endregion (Initialize)

        //-------------------------------------------------------------------------------
        #region Timer_Tick 切り替えタイマーイベント
        //-------------------------------------------------------------------------------
        //
        private void Timer_Tick(object sender, EventArgs e)
        {
            lock (_lockimg) {
                if (_img == null || _disp == null || _disp.IsDisposed) {
                    _timer.Stop();
                    return;
                }

                _imgIndex++;
                _imgIndex %= _img.Length;
            }
            ConfigDispForm();
            _disp.Refresh();
        }
        #endregion (Timer_Tick)

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
                if (_img == null) {
                    if (_imgURLs != null) { GetImages(); }
                    if (_img == null) { e.Cancel = true; return; }
                }

                /// size config
                Size size = _img[_imgIndex].Size;
                Size = new Size(Math.Min(size.Width, _maxSize.Width) + PADDING * 2, Math.Min(size.Height, _maxSize.Height) + PADDING * 2);
            }
            if (!_timer.Enabled) { _timer.Start(); }
        }
        //-------------------------------------------------------------------------------
        #endregion (#[override]OnShowToolTip)

        //-------------------------------------------------------------------------------
        #region #[override]CanselDisplay 表示キャンセル条件
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanselDisplay()
        {
            lock (_lockimg) {
                if (_imgURLs == null) { return true; }
            }
            return false;
        }
        #endregion (#[override]CanselDisplay)

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
                if (_img == null) { return; }
                Graphics g = e.Graphics;
                g.Clear(c.BackColor);

                Rectangle drawrect = e.ClipRectangle;
                g.DrawRectangle(PEN, 0, 0, drawrect.Width - 1, drawrect.Height - 1);
                g.DrawImage(_img[_imgIndex], PADDING, PADDING, drawrect.Width - PADDING * 2, drawrect.Height - PADDING * 2);
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
                _imgURLs = urls.ToArray();
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
        #region -GetImages 画像を取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を取得します。
        /// </summary>
        private void GetImages()
        {
            List<Image> list = new List<Image>();
            foreach (var url in PictureGetter.ConvertURLs(_imgURLs)) {
                Image img = Utilization.GetImageFromURL(url);
                if (img != null) { list.Add(img); }
            }

            if (list.Count > 0) {
                _img = list.ToArray();
            }
            else {
                _imgURLs = null;
            }
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

