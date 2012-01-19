using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace StarlitTwit
{
    public partial class KeyInputGrid : UserControl
    {
        //-------------------------------------------------------------------------------
        #region (private class)Counter
        //-------------------------------------------------------------------------------
        private class Counter
        {
            private uint c;
            public Counter(uint init) { c = init; }
            public uint Increment() { return ++c; }
            public uint Decrement() { Debug.Assert(c > 0); return --c; }
            public uint Value() { return c; }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)Counter)

        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>データのリスト</summary>
        private Dictionary<int, List<Tuple<KeyInputRight, object>>> _dataListDic = new Dictionary<int, List<Tuple<KeyInputRight, object>>>();
        private Dictionary<int, List<Tuple<KeyData, Counter>>> _duplicateDic = new Dictionary<int, List<Tuple<KeyData, Counter>>>();
        private int _index = 0;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

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
        private Tuple<Label, KeyInputRight> GetOneItem(string title, KeyData keydata, int groupID)
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
            var r = new KeyInputRight(keydata, groupID) {
                Width = flpnlRight.Width
            };
            r.KeyDataChanging += keyinputright_KeyDataChanging;
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
                vScrollBar.SmallChange = 20;
            }
        }
        #endregion (AddLabel)

        //-------------------------------------------------------------------------------
        #region +AddItems アイテム追加
        //-------------------------------------------------------------------------------
        /// <summary>
        /// キーを排他的に割り当てるべきグループは1度に登録してください。
        /// </summary>
        /// <param name="data"></param>
        public void AddItems(IEnumerable<Tuple<string, KeyData, object>> data)
        {
            List<Tuple<KeyInputRight, object>> list;
            if (!_duplicateDic.ContainsKey(_index)) {
                _duplicateDic.Add(_index, new List<Tuple<KeyData, Counter>>());
                list = new List<Tuple<KeyInputRight, object>>();
                _dataListDic.Add(_index, list);
            }
            else {
                list = _dataListDic[_index];
            }

            foreach (var item in data) {
                var tpl = GetOneItem(item.Item1, item.Item2, _index);
                flpnlLeft.Controls.Add(tpl.Item1);
                flpnlRight.Controls.Add(tpl.Item2);
                list.Add(new Tuple<KeyInputRight, object>(tpl.Item2, item.Item3));
            }


            if (flpnlLeft.PreferredSize.Height > this.Height) {
                vScrollBar.Enabled = true;
                vScrollBar.Maximum = flpnlLeft.PreferredSize.Height;
                vScrollBar.Value = 0;
                vScrollBar.LargeChange = this.Height;
                vScrollBar.SmallChange = 20;
            }

            //++_index;
        }
        #endregion (+AddItems)

        //-------------------------------------------------------------------------------
        #region +GetItems アイテム取得
        //-------------------------------------------------------------------------------
        //
        public IEnumerable<Tuple<KeyData, object>> GetItems()
        {
            for (int i = 0; i < _index; i++) {
                foreach (var item in _dataListDic[i]) {
                    KeyData kd = item.Item1.Keydata;
                    if (kd == null) { continue; }
                    yield return new Tuple<KeyData, object>(kd, item.Item2);
                }
            }
        }
        #endregion (GetItems)

        //-------------------------------------------------------------------------------
        #region +ExistDuplication 重複の有無
        //-------------------------------------------------------------------------------
        //
        public bool ExistDuplication()
        {
            return _duplicateDic.Any(kvp => kvp.Value.Count > 0);
        }
        #endregion (ExistDuplication)

        //-------------------------------------------------------------------------------
        #region keyinputright_KeyDataChanging キーデータ変更時
        //-------------------------------------------------------------------------------
        //
        private void keyinputright_KeyDataChanging(object sender, KeyDataChangingEventArgs e)
        {
            var kir = (KeyInputRight)sender;

            if (e.PreviousKeyData == e.NewKeyData) { return; } // 変わってない
            var list = _dataListDic[kir.GroupID];
            // 重複解除確認
            bool prevDupCheck = false;
            if (kir.IsWarning) {
                int index = _duplicateDic[kir.GroupID].FindIndex(t => t.Item1 == e.PreviousKeyData);
                Debug.Assert(index >= 0);
                var tuple = _duplicateDic[kir.GroupID][index];
                if (tuple.Item2.Value() == 2) {
                    _duplicateDic[kir.GroupID].RemoveAt(index);
                    prevDupCheck = true;
                }
                else { tuple.Item2.Decrement(); }
            }

            // 重複確認
            uint numDup = 1;
            bool isThisDuplicated = false;
            if (e.NewKeyData == null) { return; }
            foreach (var item in list) {
                if (item.Item1 == kir) { continue; }
                if (prevDupCheck && item.Item1.Keydata == e.PreviousKeyData) {
                    item.Item1.IsWarning = false;
                }
                else if (item.Item1.Keydata == e.NewKeyData) {
                    item.Item1.IsWarning = true;
                    isThisDuplicated = true;
                    ++numDup;
                }
            }

            if (numDup == 2) { _duplicateDic[kir.GroupID].Add(new Tuple<KeyData, Counter>(e.NewKeyData, new Counter(2))); }
            else if (numDup > 2) { _duplicateDic[kir.GroupID].Find(t => t.Item1 == e.NewKeyData).Item2.Increment(); }
            kir.IsWarning = isThisDuplicated;
        }
        #endregion (keyinputright_KeyDataChanging)

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
