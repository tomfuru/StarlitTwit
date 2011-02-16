using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwitterClient
{
    
    public class MyToolTip : Component
    {
        //-------------------------------------------------------------------------------
        #region メンバー
        //-------------------------------------------------------------------------------
        /// <summary>表示部分</summary>
        private FrmDisp _disp;
        /// <summary>タイマー</summary>
        private Timer _timer = new Timer();
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
        public Color BackColor
        {
            get { return _disp.BackColor; }
            set { _disp.BackColor = value; }
        }
        #endregion (BackColor)
        //-------------------------------------------------------------------------------
        #region ToolTipText プロパティ：テキスト
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップに表示するテキストを取得または設定します．
        /// </summary>
        [Category("表示")]
        [Description("ツールチップに表示するテキストを指定します。")]
        [TypeConverter(typeof(StringConverter))]
        [DefaultValue("")]
        public string ToolTipText
        {
            get { return _disp.Text; }
            set { _disp.Text = value; }
        }
        #endregion (ToolTipText)
        //-------------------------------------------------------------------------------
        #region Font プロパティ：フォント
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストのフォントを取得または設定します。
        /// </summary>
        [Category("表示")]
        [Description("テキストのフォントを指定します。")]
        public Font Font
        {
            get { return _disp.Font; }
            set { _disp.Font = value; }
        }
        #endregion (Font)
        //-------------------------------------------------------------------------------
        #region DisplayDuration プロパティ：表示の長さ
        //-------------------------------------------------------------------------------
        private int _dispDuration = 5000;
        /// <summary>
        /// ツールチップを表示している時間を取得または設定します。
        /// </summary>
        [Category("表示")]
        [Description("ツールチップを表示している時間をミリ秒単位で指定します。0に指定すると永久に表示します")]
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
        /// マウスポインタがきてからツールチップを表示するまでの時間を取得または設定します。
        /// </summary>
        [Category("表示")]
        [Description("マウスポインタがきてからツールチップを表示するまでの時間をミリ秒単位で指定します。")]
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
                if (_dispControl != null) {
                    _dispControl.MouseLeave -= DispControl_Leave;
                    _dispControl.MouseEnter -= DispControl_Enter;
                }
                _dispControl = value;
                _dispControl.MouseLeave += DispControl_Leave;
                _dispControl.MouseEnter += DispControl_Enter;
            }
        }
        #endregion (DisplayControl)
        //-------------------------------------------------------------------------------
        #endregion (プロパティ)

        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        #region (void)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTip(Form parentForm)
            : base()
        {
            _disp = new FrmDisp(parentForm);

            // 画面
            _disp.Paint += Disp_Paint;

            // タイマー
            _timer.Interval = InitialDelay;
            _timer.Tick += Timer_Tick;
        }
        //-------------------------------------------------------------------------------
        #endregion ((void))
        #region (IContainer)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTip(Form parentForm,IContainer cont)
            : this(parentForm)
        {
            cont.Add(_disp);
        }
        //-------------------------------------------------------------------------------
        #endregion ((IContainer))
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region DispControl_Enter 表示コントロールに入ったとき
        //-------------------------------------------------------------------------------
        //
        private void DispControl_Enter(object sender, EventArgs e)
        {
            _timer.Stop();
            if (_initDelay == 0) {
                _currentState = ToolTipState.Displaying;
                _timer.Interval = _dispDuration;
                _timer.Start();
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
            _timer.Stop();
            _currentState = ToolTipState.EnterWait;
            Hide();
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
                    _timer.Interval = _dispDuration;
                    _currentState = ToolTipState.Displaying;
                    Display();
                    _timer.Start();
                    break;
                case ToolTipState.Displaying:
                    _currentState = ToolTipState.FinDisplay;
                    Hide();
                    break;
            }
        }
        #endregion (Timer_Tick)

        //-------------------------------------------------------------------------------
        #region Disp_Paint 表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ描画/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disp_Paint(object sender,PaintEventArgs e)
        {
            Control c = (Control)sender;

            Graphics g = e.Graphics;
            g.Clear(c.BackColor);
        }
        #endregion (Disp_Paint)

        //-------------------------------------------------------------------------------
        #region -Display 表示処理
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 表示処理
        /// </summary>
        private void Display()
        {
            Size s = TextRenderer.MeasureText(_disp.Text, _disp.Font);

            _disp.Size = s;

            Point pDisp;

            // 下に余裕があるか見る
            pDisp = new Point(Cursor.Position.X, Cursor.Position.Y + Cursor.Current.Size.Height);

            _disp.Location = Point.Add(pDisp,new Size(_dispControl.Location));
            _disp.Show();
            //_disp.
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
            _disp.Hide();
        }
        #endregion (Hide)

        //-------------------------------------------------------------------------
        #region (Class)FrmDisp
        //-------------------------------------------------------------------------------
        private class FrmDisp : Form
        {
            internal Label label = new Label();

            //-------------------------------------------------------------------------------
            #region コンストラクタ
            //-------------------------------------------------------------------------------
            //
            public FrmDisp(Form parent)
                : base()
            {
                //this.SetStyle(ControlStyles.Selectable, false);
                this.Owner = parent;
                this.BackColor = Color.LemonChiffon;
                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;

                this.VisibleChanged += new EventHandler(FrmDisp_VisibleChanged);
            }

            void FrmDisp_VisibleChanged(object sender, EventArgs e)
            {
                if (this.Visible) {
                    this.Owner.Activate();

                }
                else {


                }
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
