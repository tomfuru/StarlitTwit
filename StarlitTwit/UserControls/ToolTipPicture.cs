using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace StarlitTwit
{
    public class ToolTipPicture : PermanentToolTip
    {
        //-------------------------------------------------------------------------------
        #region 変数
        //-------------------------------------------------------------------------------
        /// <summary>表示イメージ</summary>
        private Image[] _img = null;
        /// <summary>現在表示中イメージインデックス</summary>
        private int _imgIndex = 0;
        /// <summary>コントロール</summary>
        private Control _control;
        private Timer timer;
        private IContainer components;
        private object _lockimg = new object();
        private bool _displayUpper = false;
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region +ImageURLs プロパティ：
        //-------------------------------------------------------------------------------
        private IEnumerable<string> _imageURLs;
        /// <summary>
        /// 画像表示URL配列
        /// </summary>
        [Browsable(false)]
        public IEnumerable<string> ImageURLs
        {
            get { return _imageURLs; }
            set
            {
                lock (_lockimg) {
                    _imageURLs = value;
                    DisposeImages();
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (ImageURLs)
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
            get { return timer.Interval; }
            set { timer.Interval = value; }
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
        #region 定数
        //-------------------------------------------------------------------------------
        private const int PADDING = 1;
        /// <summary>四角を描くためのペン</summary>
        private static readonly Pen PEN = new Pen(new SolidBrush(Color.Black));
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public ToolTipPicture()
            : base()
        {
            InitializeComponent();
        }
        public ToolTipPicture(IContainer cont)
            : base(cont)
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------
        #region Public イベント
        //-------------------------------------------------------------------------------
        /// <summary>ポップアップ前に発生するイベントです。</summary>
        [Category("動作")]
        public event EventHandler<CancelEventArgs> PrePopup;
        //-------------------------------------------------------------------------------
        #endregion (Public イベント)

        //-------------------------------------------------------------------------------
        #region InitializeComponent 初期設定
        //-------------------------------------------------------------------------------
        //
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            // 
            // ToolTipPicture
            // 
            this.AutoPopDelay = 5000;
            this.InitialDelay = 1000;
            this.OwnerDraw = true;
            this.ReshowDelay = 100;
            this.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.ToolTipPicture_Draw);
            this.Popup += new System.Windows.Forms.PopupEventHandler(this.ToolTipPicture_Popup);

        }
        //-------------------------------------------------------------------------------
        #endregion (InitializeComponent)

        //-------------------------------------------------------------------------------
        #region ToolTipPicture_Popup ツールチップ表示時
        //-------------------------------------------------------------------------------
        private bool bSuspendPopupEvent = false;
        //
        private void ToolTipPicture_Popup(object sender, PopupEventArgs e)
        {
            if (bSuspendPopupEvent) { return; }

            bSuspendPopupEvent = true;

            try {
                if (PrePopup != null) {
                    CancelEventArgs args = new CancelEventArgs(false);
                    PrePopup.Invoke(this, args);
                    if (args.Cancel) { e.Cancel = true; return; }
                }

                lock (_lockimg) {
                    if (_control == null || _imageURLs == null) { e.Cancel = true; return; }

                    if (_img == null) { GetImages(); }
                    if (_imageURLs == null) { e.Cancel = true; return; }

                    Size ttSize = _img[_imgIndex].Size;
                    if (!JutOutSize(ttSize).IsEmpty && !_displayUpper) {
                        e.Cancel = true;
                        bSuspendPopupEvent = false;
                        Display();
                        return;
                    }

                    double multW = ttSize.Width / (double)_maxSize.Width,
                           multH = ttSize.Height / (double)_maxSize.Height;
                    bool needScale = (Math.Max(multW, multH) > 1);
                    if (needScale) {
                        if (multW > multH) {
                            ttSize.Height = (ttSize.Height * _maxSize.Width) / ttSize.Width;
                            ttSize.Width = _maxSize.Width;
                        }
                        else {
                            ttSize.Width = (ttSize.Width * _maxSize.Height) / ttSize.Height;
                            ttSize.Height = _maxSize.Height;
                        }
                    }

                    e.ToolTipSize = new Size(ttSize.Width + PADDING * 2, ttSize.Height + PADDING * 2);
                }
            }
            finally {
                bSuspendPopupEvent = false;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (Name)
        //-------------------------------------------------------------------------------
        #region ToolTipPicture_Draw ツールチップドロー時
        //-------------------------------------------------------------------------------
        //
        private void ToolTipPicture_Draw(object sender, DrawToolTipEventArgs e)
        {
            lock (_lockimg) {
                if (_img == null) { return; }

                if (_img.Length > 0 && !timer.Enabled) { timer.Enabled = true; }

                e.DrawBackground();
                Graphics g = e.Graphics;
                if (_img[_imgIndex] != null) {
                    Rectangle drawrect = e.Bounds;
                    g.DrawRectangle(PEN, 0, 0, drawrect.Width - 1, drawrect.Height - 1);
                    g.DrawImage(_img[_imgIndex], PADDING, PADDING, drawrect.Width - PADDING * 2, drawrect.Height - PADDING * 2);
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (ToolTipPicture_Draw)
        //-------------------------------------------------------------------------------
        #region OnTimerTick 時間経過時
        //-------------------------------------------------------------------------------
        //
        protected override void OnTimerTick()
        {
            lock (_lockimg) {
                if (_img == null) { return; } // 保険

                _imgIndex++;
                _imgIndex %= _img.Length;
            }

            // Point計算
            Display();
        }
        //-------------------------------------------------------------------------------
        #endregion (timer_Tick)
        //-------------------------------------------------------------------------------
        #region Parent_MouseLeave 親コントロールから離れた時
        //-------------------------------------------------------------------------------
        //
        private void Parent_MouseLeave(object sender, EventArgs e)
        {
            this.Hide(_control);
            timer.Enabled = false;
            _imgIndex = 0;
        }
        //-------------------------------------------------------------------------------
        #endregion (Parent_MouseLeave)

        //-------------------------------------------------------------------------------
        #region -Display 表示
        //-------------------------------------------------------------------------------
        //
        private void Display()
        {
            Size imgSize = _img[_imgIndex].Size;
            Size size = JutOutSize(imgSize);
            _displayUpper = true;
            try {
                Point p = _control.PointToClient(Cursor.Position);
                int x = p.X;
                int y = p.Y;
                if (size.Width > 0) { x -= imgSize.Width + 2;}
                if (size.Height > 0) { y -= imgSize.Height + 2; }
                this.Show("show", _control, x, y, this.AutoPopDelay);
            }
            finally {
                _displayUpper = false;
            }
        }
        #endregion (Display)
        //-------------------------------------------------------------------------------
        #region -JutOutSize 画面からはみ出るサイズ
        //-------------------------------------------------------------------------------
        //
        private Size JutOutSize(Size dispSize)
        {
            Rectangle rect = new Rectangle(Point.Add(Cursor.Position, new Size(0, Cursor.Current.Size.Height)), dispSize);
            Screen currentScr = null;
            foreach (Screen scr in Screen.AllScreens) { // 現在スクリーン取得
                if (scr.Bounds.Contains(Cursor.Position)) {
                    currentScr = scr;
                    break;
                }
            }
            if (currentScr == null) { return Size.Empty; }

            Rectangle cross = Rectangle.Intersect(currentScr.Bounds, rect);
            return new Size(rect.Width - cross.Width, rect.Height - cross.Height);
        }
        #endregion (JutOutSize)

        //-------------------------------------------------------------------------------
        #region +SetToolTip ツールチップ表示セット
        //-------------------------------------------------------------------------------
        //
        public void SetToolTip(Control control)
        {
            if (_control != null) {
                _control.MouseLeave += Parent_MouseLeave;
            }
            _control = control;
            _control.MouseLeave += Parent_MouseLeave;

            this.SetToolTip(control, "set");
        }
        //-------------------------------------------------------------------------------
        #endregion (SetToolTip)
        //-------------------------------------------------------------------------------
        #region +GetImages 画像を取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を取得します。
        /// </summary>
        public void GetImages()
        {
            List<Image> list = new List<Image>();
            foreach (var url in PictureGetter.ConvertURLs(ImageURLs)) {
                Image img = Utilization.GetImageFromURL(url);
                if (img != null) { list.Add(img); }
            }

            if (list.Count > 0) {
                _img = list.ToArray();
            }
            else {
                ImageURLs = null;
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

        //-------------------------------------------------------------------------------
        #region 未使用
        /*-------------------------------------------------------------------------------
        #region -GetImageFromTwitpic Twitpicから画像を取得します。
        //-------------------------------------------------------------------------------
        //
        private Image GetImageFromTwitpicBig(string url)
        {
            const string check = "<img class=\"photo\" id=\"photo-display\" src=\"";

            try {
                WebRequest req1 = WebRequest.Create(url);
                WebResponse res1 = req1.GetResponse();
                string twitpic;
                using (Stream stream = res1.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        twitpic = reader.ReadToEnd();
                    }
                }
                int startIndex = twitpic.IndexOf(check);
                if (startIndex == -1) { return null; }
                startIndex += check.Length;

                int endIndex = twitpic.IndexOf('"', startIndex);
                if (endIndex == -1) { return null; }
                string picurl = twitpic.Substring(startIndex, endIndex - startIndex);

                Image img = Utilization.GetImageFromURL(picurl);
                return img;
            }
            catch (WebException) {
                return null;
            }
            throw new NotImplementedException();
        }
        #endregion (GetImageFromTwitpic)
        */
        #endregion
    }
}
