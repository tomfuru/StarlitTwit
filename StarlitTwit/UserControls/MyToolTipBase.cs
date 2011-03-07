using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public abstract class MyToolTipBase : Component
    {
        //-------------------------------------------------------------------------------
        #region メンバー
        //-------------------------------------------------------------------------------
        /// <summary>表示部分</summary>
        private FrmDisp _disp = null;
        /// <summary>タイマー</summary>
        private Timer _timer = null;
        /// <summary>現在の状態</summary>
        private ToolTipState _currentState = ToolTipState.EnterWait;
        //-------------------------------------------------------------------------------
        #endregion (メンバー)

        //-------------------------------------------------------------------------------
        #region -ToolTipState 列挙体：状態
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 現在の状態を表します。
        /// </summary>
        private enum ToolTipState : byte
        {
            /// <summary>マウスがコントロールに入るのを待っている</summary>
            EnterWait,
            /// <summary>マウスがコントロールに入り指定時間が経つのを待っている</summary>
            DisplayWait,
            /// <summary>表示中</summary>
            Displaying,
            /// <summary>表示が終わりマウスがコントロールから出るのを待っている</summary>
            FinDisplay
        }
        //-------------------------------------------------------------------------------
        #endregion (ToolTipState)

        //-------------------------------------------------------------------------------
        #region プロパティ
        //-------------------------------------------------------------------------------
        #region BackColor プロパティ：背景
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 背景色を取得または設定します。
        /// </summary>
        [Category("表示")]
        [Description("背景色を指定します。")]
        public Color BackColor { get; set; }
        #endregion (BackColor)
        //-------------------------------------------------------------------------------
        #region Size プロパティ：ToolTipサイズ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ToolTipのサイズを取得します。
        /// </summary>
        [Browsable(false)]
        public Size Size { get; protected set; }
        #endregion (Size)
        //-------------------------------------------------------------------------------
        #region DisplayDuration プロパティ：表示の長さ
        //-------------------------------------------------------------------------------
        private int _dispDuration = 5000;
        /// <summary>
        /// ツールチップを表示している時間を取得または設定します。(0以下は永久)
        /// </summary>
        [Category("表示")]
        [Description("ツールチップを表示している時間をミリ秒単位で指定します。0以下に指定すると永久に表示します")]
        [DefaultValue(5000)]
        public int DisplayDuration
        {
            get { return _dispDuration; }
            set
            {
                if (value < 0) { throw new ArgumentException("負の値は指定できません。"); }
                _dispDuration = value;
            }
        }
        #endregion (DisplayDuration)
        //-------------------------------------------------------------------------------
        #region InitialDelay プロパティ：表示までの長さ
        //-------------------------------------------------------------------------------
        private int _initDelay = 500;
        /// <summary>
        /// マウスポインタがきてからツールチップを表示するまでの時間を取得または設定します。(0以下はすぐ)
        /// </summary>
        [Category("表示")]
        [Description("マウスポインタがきてからツールチップを表示するまでの時間をミリ秒単位で指定します。0以下に指定するとすぐに表示します。")]
        [DefaultValue(500)]
        public int InitialDelay
        {
            get { return _initDelay; }
            set
            {
                if (value < 0) { throw new ArgumentException("負の値は指定できません。"); }
                _initDelay = value;
            }
        }
        #endregion (InitialDelay)
        //-------------------------------------------------------------------------------
        #region DisplayControl プロパティ：ツールチップを表示するコントロール
        //-------------------------------------------------------------------------------
        private Control _dispControl = null;
        /// <summary>
        /// ツールチップを表示するコントロールを取得または設定します．
        /// </summary>
        [Category("表示")]
        [Description("ツールチップを表示するコントロールを取得または設定します。")]
        public Control DisplayControl
        {
            get { return _dispControl; }
            set
            {
                if (value == null) { return; }

                if (_dispControl != null) {
                    _dispControl.MouseLeave -= DispControl_Leave;
                    _dispControl.MouseEnter -= DispControl_Enter;
                }
                _dispControl = value;
                _dispControl.MouseLeave += DispControl_Leave;
                _dispControl.MouseEnter += DispControl_Enter;
                _dispControl.MouseDown += DispControl_MouseDown;
            }
        }
        #endregion (DisplayControl)
        //-------------------------------------------------------------------------------
        #endregion (プロパティ)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        /// <summary>ツールチップが表示される直前に発生するイベント</summary>
        public event EventHandler ShowToolTip;
        /// <summary>ツールチップが隠された時に発生するイベント</summary>
        public event EventHandler HideToolTip;
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        #region (Form)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        protected MyToolTipBase()
            : base()
        {
            BackColor = SystemColors.Info;
        }
        //-------------------------------------------------------------------------------
        #endregion ((void))
        #region (Form, IContainer)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        protected MyToolTipBase(IContainer cont)
            : this()
        {
            cont.Add(_disp);
        }
        //-------------------------------------------------------------------------------
        #endregion ((IContainer))
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region DispControl_MouseDown 表示コントロールでマウスダウン時
        //-------------------------------------------------------------------------------
        //
        private void DispControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (_timer != null) {
                _timer.Stop();
                _currentState = ToolTipState.EnterWait;
                Hide();
            }
        }
        #endregion (DispControl_MouseDown)
        //-------------------------------------------------------------------------------
        #region DispControl_Enter 表示コントロールに入ったとき
        //-------------------------------------------------------------------------------
        //
        private void DispControl_Enter(object sender, EventArgs e)
        {
            if (CanselDisplay()) { return; }

            if (_timer != null) {
                _timer.Stop();
            }
            else {
                _timer = new Timer();
                _timer.Tick += Timer_Tick;
                if (_initDelay > 0) { _timer.Interval = _initDelay; }
            }
            if (_initDelay <= 0) {
                _currentState = ToolTipState.Displaying;
                Display();
                if (_dispDuration > 0) {
                    _timer.Interval = _dispDuration;
                    _timer.Start();
                }
            }
            else {
                _currentState = ToolTipState.DisplayWait;
                _timer.Interval = _initDelay;
                _timer.Start();
            }
        }
        #endregion (DispControl_Enter)
        //-------------------------------------------------------------------------------
        #region DispControl_Leave 表示コントロールから離れた時
        //-------------------------------------------------------------------------------
        //
        private void DispControl_Leave(object sender, EventArgs e)
        {
            if (_timer != null) {
                _timer.Stop();
                _currentState = ToolTipState.EnterWait;
                Hide();
            }
        }
        #endregion (DispControl_Leave)
        //-------------------------------------------------------------------------------
        #region Timer_Tick タイマーに指定された時間が経過したとき
        //-------------------------------------------------------------------------------
        //
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            switch (_currentState) {
                case ToolTipState.DisplayWait:
                    _currentState = ToolTipState.Displaying;
                    Display();
                    if (_dispDuration > 0) {
                        _timer.Interval = _dispDuration;
                        _timer.Start();
                    }
                    break;
                case ToolTipState.Displaying:
                    _currentState = ToolTipState.FinDisplay;
                    Hide();
                    break;
            }
        }
        #endregion (Timer_Tick)

        //-------------------------------------------------------------------------------
        #region #[abstruct]Disp_Paint 表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ描画/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void Disp_Paint(object sender, PaintEventArgs e);
        #endregion (Disp_Paint)

        //-------------------------------------------------------------------------------
        #region #[virtual]CanselDisplay 表示抑制
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 表示を抑制するかどうかの条件設定
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanselDisplay() { return false; }
        #endregion (CanselDisplay)

        //-------------------------------------------------------------------------------
        #region #[virtual]OnShowToolTip ツールチップが表示される直前
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップが表示される直前に実行される。
        /// </summary>
        protected virtual void OnShowToolTip()
        {
            if (ShowToolTip != null) { ShowToolTip(this, EventArgs.Empty); }
        }
        #endregion (OnShowToolTip)
        //-------------------------------------------------------------------------------
        #region #[virtual]OnHideToolTip ツールチップが隠れた時
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップが隠れる時に実行される。
        /// </summary>
        protected virtual void OnHideToolTip()
        {
            if (HideToolTip != null) { HideToolTip(this, EventArgs.Empty); }
        }
        #endregion (OnHideToolTip)

        //-------------------------------------------------------------------------------
        #region -Display 表示処理
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 表示処理
        /// </summary>
        private void Display()
        {
            OnShowToolTip();

            _disp = new FrmDisp() {
                StartPosition = FormStartPosition.Manual,
                BackColor = BackColor
            };
            _disp.Paint += Disp_Paint;

            // テキスト設定
            _disp.Size = Size.Add(Size, _disp.Margin.Size);

            Point pDisp;
            if (DisplayControl != null) {
                Point pC = DisplayControl.PointToClient(Cursor.Position);

                pDisp = _dispControl.PointToScreen(pC);

                pDisp.X += 8;
                pDisp.Y += 20;
                /// 下に余裕があるか見る
            }
            else { pDisp = new Point(0, 0); }
            _disp.Location = pDisp;
            _disp.Show();
        }
        #endregion (Display)
        //-------------------------------------------------------------------------------
        #region -Hide 隠蔽処理
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 隠蔽処理
        /// </summary>
        private void Hide()
        {
            if (_disp != null) {
                _disp.Close();
            }
            OnHideToolTip();
        }
        #endregion (Hide)

        //-------------------------------------------------------------------------
        #region (Class)FrmDisp
        //-------------------------------------------------------------------------------
        public class FrmDisp : Form
        {
            //-------------------------------------------------------------------------------
            #region #[override]ShowWithoutActivation プロパティ
            //-------------------------------------------------------------------------------
            protected override bool ShowWithoutActivation
            {
                get { return true; }
            }
            //-------------------------------------------------------------------------------
            #endregion (#[override]ShowWithoutActivation)

            //-------------------------------------------------------------------------------
            #region コンストラクタ
            //-------------------------------------------------------------------------------
            //
            internal FrmDisp()
                : base()
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            #endregion (コンストラクタ)

            //-------------------------------------------------------------------------
            #region CreateParams
            //-------------------------------------------------------------------------------
            protected override CreateParams CreateParams
            {
                get
                {
                    const int WS_EX_DLGMODALFRAME = 0x00000001;
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle = cp.ExStyle | WS_EX_DLGMODALFRAME;
                    return cp;
                }
            }
            //-------------------------------------------------------------------------------
            #endregion (CreateParams)
        }
        //-------------------------------------------------------------------------------
        #endregion ((Class)FrmDisp)
    }
}
