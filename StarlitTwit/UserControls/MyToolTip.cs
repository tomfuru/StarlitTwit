using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace StarlitTwit
{
    public class MyToolTip : MyToolTipBase
    {

        //-------------------------------------------------------------------------------
        #region Font プロパティ：フォント
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストのフォントを取得または設定します。
        /// </summary>
        [Category("表示")]
        [Description("テキストのフォントを指定します。")]
        public Font Font { get; set; }
        #endregion (Font)
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
        public string ToolTipText { get; set; }
        #endregion (ToolTipText)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        #region (void)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTip()
            : base()
        {
            ToolTipText = "";
            Font = new Font("MS UI Gothic", 9F);
        }
        //-------------------------------------------------------------------------------
        #endregion ((void))
        #region (IContainer)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// MyToolTipを初期化します。
        /// </summary>
        public MyToolTip(IContainer cont)
            : base(cont)
        {
            ToolTipText = "";
            Font = new Font("MS UI Gothic", 9F);
        }
        //-------------------------------------------------------------------------------
        #endregion ((IContainer))
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region #[override]OnShowToolTip
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ツールチップ表示時
        /// </summary>
        protected override void OnShowToolTip(CancelEventArgs e)
        {
            base.OnShowToolTip(e);
            if (string.IsNullOrEmpty(ToolTipText) || Font == null) { e.Cancel = true; return; }
            Size = TextRenderer.MeasureText(ToolTipText, Font);
        }
        //-------------------------------------------------------------------------------
        #endregion (#[override]OnShowToolTip)

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

            Graphics g = e.Graphics;
            g.Clear(c.BackColor);

            using (Brush brush = new SolidBrush(Color.Black)) {
                //g.DrawString(ToolTipText, Font, brush, 0.0f, 0.0f);
                TextRenderer.DrawText(g, ToolTipText, Font, new Point(0, 0), Color.Black); // TextRendererでサイズを計測したのでこっちで描画(GDI)
            }
        }
        #endregion (Disp_Paint)
    }
}
