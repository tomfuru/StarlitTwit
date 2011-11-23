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
    public partial class KeyInputGrid : UserControl
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public KeyInputGrid()
        {
            InitializeComponent();
        }
        #endregion (Constructor)

        private List<Tuple<KeyInputRight, object>> _datalist = new List<Tuple<KeyInputRight, object>>();

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
            //l.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            var r = new KeyInputRight(keydata) {
                Width = flpnlRight.Width
            };
            //r.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            return new Tuple<Label, KeyInputRight>(l, r);
        }
        #endregion (GetOneItem)

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
        #region flpnlLeft_Resize 左パネルサイズ変更時
        //-------------------------------------------------------------------------------
        //
        private void flpnlLeft_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flpnlLeft.Controls) {
                c.Width = flpnlLeft.Width;
            }
            flpnlLeft.AutoScroll = true; // サイズ変えただけだと横スクロールバーが出るのでこれで消す
        }
        #endregion (flpnlLeft_Resize)
        //-------------------------------------------------------------------------------
        #region flpnlRight_Resize 右パネルサイズ変更時
        //-------------------------------------------------------------------------------
        //
        private void flpnlRight_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flpnlRight.Controls) {
                c.Width = flpnlRight.Width;
            }
            flpnlRight.AutoScroll = true; // サイズ変えただけだと横スクロールバーが出るのでこれで消す
        }
        #endregion (flpnlRight_Resize)
    }
}
