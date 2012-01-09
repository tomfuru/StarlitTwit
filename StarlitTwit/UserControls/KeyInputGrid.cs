using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StarlitTwit
{
    public partial class KeyInputGrid : UserControl
    {
        private List<Tuple<KeyInputRight, object>> _datalist = new List<Tuple<KeyInputRight, object>>();

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public KeyInputGrid()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.Selectable, true);

            pnlScroll.MouseWheel += pnl_MouseWheel;
            vScrollBar.MouseWheel += pnl_MouseWheel;

            vScrollBar.MouseCaptureChanged += focus_ByMouse;
            splitter1.MouseDown += focus_ByMouse;
            vScrollBar.MouseDown += focus_ByMouse;
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region pnl_MouseWheel ホイール
        //-------------------------------------------------------------------------------
        //
        private void pnl_MouseWheel(object sender, MouseEventArgs e)
        {
            int delta = -e.Delta / 120 * vScrollBar.SmallChange;
            int result = vScrollBar.Value + delta;
            result = Math.Min(vScrollBar.Maximum - vScrollBar.LargeChange + 1, result);
            result = Math.Max(vScrollBar.Minimum, result);
            vScrollBar.Value = result;
        }
        #endregion (pnl_MouseWheel)
        //-------------------------------------------------------------------------------
        #region focus_ByMouse マウス操作によりフォーカス
        //-------------------------------------------------------------------------------
        //
        private void focus_ByMouse(object sender, EventArgs e)
        {
            this.Focus();
        }
        #endregion (focus_ByMouse)

        //-------------------------------------------------------------------------------
        #region -GetOneItem アイテム一つ分のコントロールを取得
        //-------------------------------------------------------------------------------
        //
        private Tuple<Label, KeyInputRight> GetOneItem(string title, KeyData keydata)
        {
            var l = new Label() {
                AutoSize = false,
                AutoEllipsis = true,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(flpnlLeft.Width, 16),
                Text = title,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };
            l.MouseClick += focus_ByMouse;
            //l.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            var r = new KeyInputRight(keydata) {
                Width = flpnlRight.Width
            };
            r.MouseClick += focus_ByMouse;
            //r.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            return new Tuple<Label, KeyInputRight>(l, r);
        }
        #endregion (GetOneItem)

        //-------------------------------------------------------------------------------
        #region +AddLabel ラベル追加
        //-------------------------------------------------------------------------------
        //
        public void AddLabel(string left_text = "", string right_text = "")
        {
            var ll = new Label() {
                AutoSize = false,
                Font = new Font(this.Font, FontStyle.Bold),
                Size = new Size(flpnlLeft.Width, 16),
                Text = left_text,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0),
            };
            ll.MouseClick += focus_ByMouse;
            var lr = new Label() {
                AutoSize = false,
                Font = new Font(this.Font, FontStyle.Bold),
                Size = new Size(flpnlRight.Width, 16),
                Text = right_text,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };
            lr.MouseClick += focus_ByMouse;

            flpnlLeft.Controls.Add(ll);
            flpnlRight.Controls.Add(lr);

            if (flpnlLeft.PreferredSize.Height > this.Height) {
                vScrollBar.Enabled = true;
                vScrollBar.Maximum = flpnlLeft.PreferredSize.Height;
                vScrollBar.Value = 0;
                vScrollBar.LargeChange = this.Height;
            }
        }
        #endregion (AddLabel)

        //-------------------------------------------------------------------------------
        #region +AddItems アイテム追加
        //-------------------------------------------------------------------------------
        //
        public void AddItems(IEnumerable<Tuple<string, KeyData, object>> data)
        {
            foreach (var item in data) {
                var tpl = GetOneItem(item.Item1, item.Item2);
                flpnlLeft.Controls.Add(tpl.Item1);
                flpnlRight.Controls.Add(tpl.Item2);
                _datalist.Add(new Tuple<KeyInputRight, object>(tpl.Item2, item.Item3));
            }


            if (flpnlLeft.PreferredSize.Height > this.Height) {
                vScrollBar.Enabled = true;
                vScrollBar.Maximum = flpnlLeft.PreferredSize.Height;
                vScrollBar.Value = 0;
                vScrollBar.LargeChange = this.Height;
            }
        }
        #endregion (+AddItems)

        //-------------------------------------------------------------------------------
        #region +GetItems アイテム取得
        //-------------------------------------------------------------------------------
        //
        public IEnumerable<Tuple<KeyData, object>> GetItems()
        {
            foreach (var item in _datalist) {
                yield return new Tuple<KeyData, object>(item.Item1.GetKeyData(), item.Item2);
            }
        }
        #endregion (GetItems)

        //-------------------------------------------------------------------------------
        #region pnlLeft_Resize 左パネルサイズ変更時
        //-------------------------------------------------------------------------------
        //
        private void pnlLeft_Resize(object sender, EventArgs e)
        {
            flpnlLeft.Width = pnlLeft.Width;
            foreach (Control c in flpnlLeft.Controls) {
                c.Width = pnlLeft.Width;
            }
        }
        #endregion (flpnlLeft_Resize)
        //-------------------------------------------------------------------------------
        #region pnlRight_Resize 右パネルサイズ変更時
        //-------------------------------------------------------------------------------
        //
        private void flpnlRight_Resize(object sender, EventArgs e)
        {
            flpnlRight.Width = pnlRight.Width;
            foreach (Control c in flpnlRight.Controls) {
                c.Width = pnlRight.Width;
            }
        }
        #endregion (flpnlRight_Resize)

        //-------------------------------------------------------------------------------
        #region pnlScroll_Resize リサイズ時
        //-------------------------------------------------------------------------------
        //
        private void pnlScroll_Resize(object sender, EventArgs e)
        {
            if (vScrollBar.Enabled = (flpnlLeft.PreferredSize.Height > this.Height)) {
                vScrollBar.Maximum = flpnlLeft.PreferredSize.Height;
                vScrollBar.LargeChange = this.Height;
            }
        }
        #endregion (pnlScroll_Resize)

        //-------------------------------------------------------------------------------
        #region vScrollBar_ValueChanged 動かす
        //-------------------------------------------------------------------------------
        //
        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            flpnlLeft.Location = new Point(0, -vScrollBar.Value);
            flpnlRight.Location = new Point(0, -vScrollBar.Value);
        }
        #endregion (vScrollBar_ValueChanged)

        //===============================================================================
        /// <summary>描画抑制フラグ</summary>
        private bool _bSuspendDraw = false;
        #region +SuspendPaint 描画抑制
        //-------------------------------------------------------------------------------
        /// <summary>描画抑制</summary>
        public void SuspendPaint()
        {
            _bSuspendDraw = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (SuspendPaint)
        #region +ResumePaint 描画再開
        //-------------------------------------------------------------------------------
        /// <summary>描画再開</summary>
        public void ResumePaint()
        {
            _bSuspendDraw = false;
        }
        #endregion (ResumePaint)
        //-------------------------------------------------------------------------------
        #region #[override]WndProc
        //-------------------------------------------------------------------------------
        //
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (_bSuspendDraw && m.Msg == 0x000f) { return; }
            base.WndProc(ref m);
        }
        #endregion (#[override]WndProc)
    }
}
