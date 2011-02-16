//タブをドラッグで移動できるようにしたクラス。
using System.Windows.Forms;
using System.Drawing;
using System;
using System.ComponentModel;

namespace TwitterClient
{
    // 利用： http://d.hatena.ne.jp/barycentric/20091022/1256225627
    public class TabControlEx : TabControl
    {
        private int mouseDownPointX = 0;
        private int mouseDownPointY = 0;
        private Rectangle dragBoxFromMouseDown = Rectangle.Empty;

        [DefaultValue(0)]
        [Description("このインデックスより右のタブのみ動かせます。")]
        public int MinMovableIndex { get; set; }
        [DefaultValue(int.MaxValue)]
        [Description("このインデックスより左のタブのみ動かせます。")]
        public int MaxMovableIndex { get; set; }

        public event EventHandler<TabExchangeEventArgs> TabExchanged;

        public TabControlEx()
        {
            MinMovableIndex = 0;
            MaxMovableIndex = int.MaxValue;

            AllowDrop = true;
            ClearDragTarget();
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            //■イベントが起きた位置をクライアント座標ポイントptに変換する．
            Point pt = PointToClient(new Point(e.X, e.Y));

            //■ptからhovering overタブを得る．
            TabPage hoverTab = GetTabPageByPoint(pt);

            //■タブがキチンと取れているかどうかで条件分岐
            if (hoverTab != null && e.Data.GetDataPresent(typeof(TabPage))) {
                //タブが取得できた場合の処理

                e.Effect = DragDropEffects.Move;
                TabPage draggedTab = (TabPage)e.Data.GetData(typeof(TabPage));

                int srcTabIndex = FindIndex(draggedTab);
                int dstTabIndex = FindIndex(hoverTab);

                // 条件
                if (srcTabIndex > MaxMovableIndex || srcTabIndex < MinMovableIndex
                 || dstTabIndex > MaxMovableIndex || dstTabIndex < MinMovableIndex) {
                     e.Effect = DragDropEffects.None;
                    return; 
                }

                if (srcTabIndex != dstTabIndex) {
                    this.SuspendLayout();//★これ大事
                    TabPage tmp = TabPages[srcTabIndex];
                    TabPages[srcTabIndex] = TabPages[dstTabIndex];
                    TabPages[dstTabIndex] = tmp;

                    SelectedTab = draggedTab;

                    this.ResumeLayout();//★これも大事

                    if (TabExchanged != null) {
                        TabExchanged(this, new TabExchangeEventArgs(srcTabIndex,dstTabIndex));
                    }
                }
            }
            else {
                //タブが取得できなかった場合の処理
                e.Effect = DragDropEffects.None;//何もしなくて良い
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                if (this.TabCount <= 1) { return; }

                Point pt = new Point(mouseDownPointX, mouseDownPointY);
                TabPage tp = GetTabPageByPoint(pt);

                if (tp != null) {
                    DoDragDrop(tp, DragDropEffects.All);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ClearDragTarget();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ClearDragTarget();
        }

        private void ClearDragTarget()
        {
            dragBoxFromMouseDown = Rectangle.Empty;
            mouseDownPointX = 0;
            mouseDownPointY = 0;
        }

        private void SetupDragTarget(int x, int y)
        {
            Size dragSize = SystemInformation.DragSize;

            dragBoxFromMouseDown = new Rectangle(new Point(x - (dragSize.Width / 2),
                                                          y - (dragSize.Height / 2)), dragSize);
            mouseDownPointX = x;
            mouseDownPointY = y;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ClearDragTarget();

            if (e.Button != MouseButtons.Left || e.Clicks >= 2) { return; }

            SetupDragTarget(e.X, e.Y);
        }

        //■GetTabPageByTab : Point --> TabPage + {null}
        private TabPage GetTabPageByPoint(Point pt)
        {
            for (int i = 0; i < TabPages.Count; i++) {
                if (GetTabRect(i).Contains(pt)) { return TabPages[i]; }
            }

            return null;
        }

        //■FindIndex: TabPage --> Int + { null }
        private int FindIndex(TabPage page)
        {
            for (int i = 0; i < TabPages.Count; i++) {
                if (TabPages[i] == page) { return i; }
            }
            return -1;
        }
    }

    public class TabExchangeEventArgs : EventArgs
    {
        public int ChangedTabIndex1 { get; private set; }
        public int ChangedTabIndex2 { get; private set; }

        public TabExchangeEventArgs(int i, int j)
        {
            ChangedTabIndex1 = i;
            ChangedTabIndex2 = j;
        }
    }
}