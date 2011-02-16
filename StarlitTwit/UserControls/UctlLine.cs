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
    /// <summary>
    /// 直線を表すユーザーコントロール
    /// </summary>
    public partial class UctlLine : UserControl
    {
        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        public UctlLine()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //===============================================================================
        #region -Size プロパティ： 使用不可
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 使用不可
        /// </summary>
        [Browsable(false)]
        private new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Size)
        //-------------------------------------------------------------------------------
        #region -Width プロパティ： 使用不可
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 使用不可
        /// </summary>
        [Browsable(false)]
        private new int Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Width)
        //-------------------------------------------------------------------------------
        #region -Height プロパティ： 使用不可
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 使用不可
        /// </summary>
        [Browsable(false)]
        private new int Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Height)

        //-------------------------------------------------------------------------------
        #region LineWidth プロパティ：線の太さを表します。
        //-------------------------------------------------------------------------------
        private int _lineWidth = 1;
        /// <summary>
        /// 線の太さを表します。
        /// </summary>
        [Category("表示")]
        [DefaultValue(1)]
        [Description("線の太さを表します。")]
        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; SetControlSize(); }
        }
        //-------------------------------------------------------------------------------
        #endregion (BorderWidth)
        //-------------------------------------------------------------------------------
        #region Direction プロパティ：線の方向を表します。
        //-------------------------------------------------------------------------------
        private LineDirection _lineDir = LineDirection.Horizontal;
        /// <summary>
        /// 線の方向を表します。
        /// </summary>
        [Category("表示")]
        [DefaultValue(LineDirection.Horizontal)]
        [Description("線の方向を表します。")]
        public LineDirection Direction
        {
            get { return _lineDir; }
            set { _lineDir = value; SetControlSize(); }
        }
        //-------------------------------------------------------------------------------
        #endregion (Direction)
        //-------------------------------------------------------------------------------
        #region Length プロパティ：線の長さを表します。
        //-------------------------------------------------------------------------------
        private int _length = 300;
        /// <summary>
        /// 線の長さを表します。
        /// </summary>
        [Category("表示")]
        [DefaultValue(300)]
        [Description("線の長さを表します。")]
        public int Length
        {
            get { return _length; }
            set { _length = value; SetControlSize(); }
        }
        //-------------------------------------------------------------------------------
        #endregion (Length)

        //===============================================================================
        #region -SetControlSize コントロールサイズを設定します。
        //-------------------------------------------------------------------------------
        //
        private void SetControlSize()
        {
            if (_lineDir == LineDirection.Vertical) {
                base.Size = new Size(_lineWidth, _length);
            }
            else {
                base.Size = new Size(_length, _lineWidth);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (SetControlSize)
    }

    //-------------------------------------------------------------------------------
    #region LineDirection 列挙体：直線の方向
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 直線の方向を表します。
    /// </summary>
    public enum LineDirection
    {
        /// <summary>横</summary>
        Horizontal,
        /// <summary>縦</summary>
        Vertical
    }
    //-------------------------------------------------------------------------------
    #endregion (LineDirection)
}
