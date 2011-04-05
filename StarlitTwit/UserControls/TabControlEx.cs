//タブをドラッグで移動できるようにしたクラス。
using System.Windows.Forms;
using System.Drawing;
using System;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;

namespace StarlitTwit
{
    // 交換について　利用： http://d.hatena.ne.jp/barycentric/20091022/1256225627
    public class TabControlEx : TabControl
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        private int mouseDownPointX = 0;
        private int mouseDownPointY = 0;
        private Rectangle dragBoxFromMouseDown = Rectangle.Empty;

        private bool _bSuspendDraw = false;

        [DefaultValue(0)]
        [Description("このインデックスより右のタブのみ動かせます。")]
        public int MinMovableIndex { get; set; }
        [DefaultValue(int.MaxValue)]
        [Description("このインデックスより左のタブのみ動かせます。")]
        public int MaxMovableIndex { get; set; }
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        public event EventHandler<TabMoveEventArgs> TabExchanged;
        /// <summary>マウスが上にあるタブ</summary>
        int _hot = -1;
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public TabControlEx()
        {
            _tabpageCollection = new TabPageExCollection(base.TabPages);
            MinMovableIndex = 0;
            MaxMovableIndex = int.MaxValue;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this.SizeMode = TabSizeMode.Normal;
            this.ItemSize = new Size(0, 15);

            AllowDrop = true;
            ClearDragTarget();
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region OnControlAdded
        //-------------------------------------------------------------------------------
        //
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPageEx) { // Controls.AddでAddされるように
                this.TabPages.AddListOnly((TabPageEx)e.Control);
            }
        }
        #endregion (OnControlAdded)
        //-------------------------------------------------------------------------------
        #region OnControlRemoved
        //-------------------------------------------------------------------------------
        //
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (e.Control is TabPageEx) {
                this.TabPages.RemoveListOnly((TabPageEx)e.Control);
            }
        }
        #endregion (OnControlRemoved)

        //-------------------------------------------------------------------------------
        #region OnPaint 描画
        //-------------------------------------------------------------------------------
        //
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //TabControlの背景を塗る
            e.Graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);

            if (this.TabPages.Count == 0) { return; }

            //TabPageの枠を描画する
            TabPageEx page;
            if (this.SelectedIndex >= 0) {
                page = this.TabPages[this.SelectedIndex];
                Rectangle pageRect = new Rectangle(page.Bounds.X - 2, page.Bounds.Y - 2,
                                                   page.Bounds.Width + 5, page.Bounds.Height + 5);
                TabRenderer.DrawTabPage(e.Graphics, pageRect);
            }

            //タブを描画する
            for (int i = 0; i < base.TabPages.Count; i++) {
                page = this.TabPages[i];
                Rectangle tabRect = this.GetTabRect(i);

                //Console.WriteLine(string.Format("Text:{0} Size:{1} TextSize:{2}", page.Text, tabRect.Size,TextRenderer.MeasureText(page.Text,this.Font)));

                //表示するタブの状態を決定する
                TabItemState state;
                if (!this.Enabled) { state = TabItemState.Disabled; }
                else if (this.SelectedIndex == i) { state = TabItemState.Selected; }
                else if (i == _hot) { state = TabItemState.Hot; }
                else { state = TabItemState.Normal; }

                //選択されたタブとページの間の境界線を消すために、
                //描画する範囲を大きくする
                if (this.SelectedIndex == i) {
                    if (this.Alignment == TabAlignment.Top) {
                        tabRect.Height += 1;
                    }
                    else if (this.Alignment == TabAlignment.Bottom) {
                        tabRect.Y -= 2;
                        tabRect.Height += 2;
                    }
                    else if (this.Alignment == TabAlignment.Left) {
                        tabRect.Width += 1;
                    }
                    else if (this.Alignment == TabAlignment.Right) {
                        tabRect.X -= 2;
                        tabRect.Width += 2;
                    }
                }

                //画像のサイズを決定する
                Size imgSize;
                if (this.Alignment == TabAlignment.Left || this.Alignment == TabAlignment.Right) {
                    imgSize = new Size(tabRect.Height, tabRect.Width);
                }
                else { imgSize = tabRect.Size; }

                //Bottomの時はTextを表示しない（Textを回転させないため）
                string tabText = page.Text;
                if (this.Alignment == TabAlignment.Bottom) { tabText = ""; }

                //タブの画像を作成する
                Bitmap bmp = new Bitmap(imgSize.Width, imgSize.Height);
                Graphics g = Graphics.FromImage(bmp);
                //高さに1足しているのは、下にできる空白部分を消すため
                TabRenderer.DrawTabItem(g, new Rectangle(0, 0, bmp.Width, bmp.Height + 1),
                                        tabText, page.Font, false, state);
                g.Dispose();

                //画像を回転する
                if (this.Alignment == TabAlignment.Bottom) { bmp.RotateFlip(RotateFlipType.Rotate180FlipNone); }
                else if (this.Alignment == TabAlignment.Left) { bmp.RotateFlip(RotateFlipType.Rotate270FlipNone); }
                else if (this.Alignment == TabAlignment.Right) { bmp.RotateFlip(RotateFlipType.Rotate90FlipNone); }

                //Bottomの時はTextを描画する
                if (this.Alignment == TabAlignment.Bottom) {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g = Graphics.FromImage(bmp);
                    g.DrawString(page.Text, page.Font, SystemBrushes.ControlText,
                                 new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                    g.Dispose();
                    sf.Dispose();
                }

                //画像を描画する
                e.Graphics.DrawImage(bmp, tabRect.X, tabRect.Y, bmp.Width, bmp.Height);

                bmp.Dispose();
            }
        }
        #endregion (OnPaint)

        //-------------------------------------------------------------------------------
        #region #[override]OnDragOver
        //-------------------------------------------------------------------------------
        //
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            //■イベントが起きた位置をクライアント座標ポイントptに変換する．
            Point pt = PointToClient(new Point(e.X, e.Y));

            //■ptからhovering overタブを得る．
            TabPageEx hoverTab = GetTabPageByPoint(pt);

            //■タブがキチンと取れているかどうかで条件分岐
            if (hoverTab != null && e.Data.GetDataPresent(typeof(TabPageEx))) {
                //タブが取得できた場合の処理

                e.Effect = DragDropEffects.Move;
                TabPageEx draggedTab = (TabPageEx)e.Data.GetData(typeof(TabPageEx));

                int srcTabIndex = FindIndex(draggedTab);
                int dstTabIndex = FindIndex(hoverTab);

                // 条件
                if (srcTabIndex > MaxMovableIndex || srcTabIndex < MinMovableIndex
                 || dstTabIndex > MaxMovableIndex || dstTabIndex < MinMovableIndex) {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                if (srcTabIndex != dstTabIndex) {
                    //SuspendPaint();
                    this.SuspendLayout();//★これ大事
                    //TabPageEx tmp = TabPages[srcTabIndex];
                    //TabPages[srcTabIndex] = TabPages[dstTabIndex];
                    //TabPages[dstTabIndex] = tmp;
                    TabPageEx mvtab = TabPages[srcTabIndex];
                    this.Visible = false;
                    TabPages.Remove(mvtab);
                    TabPages.Insert(dstTabIndex, mvtab);
                    SelectedTab = draggedTab;
                    this.Visible = true;
                    this.ResumeLayout(true);//★これも大事
                    //ResumePaint();

                    if (TabExchanged != null) {
                        TabExchanged(this, new TabMoveEventArgs(srcTabIndex, dstTabIndex));
                    }
                }
            }
            else {
                //タブが取得できなかった場合の処理
                e.Effect = DragDropEffects.None;//何もしなくて良い
            }
        }
        #endregion (#[override]OnDragOver)
        //-------------------------------------------------------------------------------
        #region #[override]OnMouseMove
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                if (this.TabCount <= 1) { return; }

                Point pt = new Point(mouseDownPointX, mouseDownPointY);
                TabPageEx tp = GetTabPageByPoint(pt);

                if (tp != null) {
                    DoDragDrop(tp, DragDropEffects.All);
                }
            }

            TabPageEx page = this.GetTabPageByPoint(this.PointToClient(Cursor.Position));
            int newHot;
            if (page == null) { newHot = -1; }
            else {
                newHot = TabPages.IndexOf(page);
            }
            if (newHot != _hot) {
                _hot = newHot;
                this.Invalidate();
            }
        }
        #endregion (#[override]OnMouseMove)
        //-------------------------------------------------------------------------------
        #region #[override]OnMouseUp
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ClearDragTarget();
        }
        #endregion (#[override]OnMouseUp)
        //-------------------------------------------------------------------------------
        #region #[override]OnMouseLeave
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ClearDragTarget();

            _hot = -1;
            this.Invalidate();
        }
        //-------------------------------------------------------------------------------
        #endregion (#[override]OnMouseLeave)
        //-------------------------------------------------------------------------------
        #region #[override]OnMouseDown
        //-------------------------------------------------------------------------------
        //
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ClearDragTarget();

            if (e.Button != MouseButtons.Left || e.Clicks >= 2) { return; }

            SetupDragTarget(e.X, e.Y);
        }
        #endregion (#[override]OnMouseDown)

        //-------------------------------------------------------------------------------
        #region -ClearDragTarget
        //-------------------------------------------------------------------------------
        //
        private void ClearDragTarget()
        {
            dragBoxFromMouseDown = Rectangle.Empty;
            mouseDownPointX = 0;
            mouseDownPointY = 0;
        }
        #endregion (-ClearDragTarget)
        //-------------------------------------------------------------------------------
        #region -SetupDragTarget
        //-------------------------------------------------------------------------------
        //
        private void SetupDragTarget(int x, int y)
        {
            Size dragSize = SystemInformation.DragSize;

            dragBoxFromMouseDown = new Rectangle(new Point(x - (dragSize.Width / 2),
                                                          y - (dragSize.Height / 2)), dragSize);
            mouseDownPointX = x;
            mouseDownPointY = y;
        }
        #endregion (SetupDragTarget)

        //-------------------------------------------------------------------------------
        #region -GetTabPageByPoint
        //-------------------------------------------------------------------------------
        //
        //■GetTabPageByTab : Point --> TabPage + {null}
        private TabPageEx GetTabPageByPoint(Point pt)
        {
            for (int i = 0; i < TabPages.Count; i++) {
                if (GetTabRect(i).Contains(pt)) { return TabPages[i]; }
            }

            return null;
        }
        #endregion (-GetTabPageByPoint)
        //-------------------------------------------------------------------------------
        #region -FindIndex
        //-------------------------------------------------------------------------------
        //
        //■FindIndex: TabPage --> Int + { null }
        private int FindIndex(TabPage page)
        {
            for (int i = 0; i < TabPages.Count; i++) {
                if (TabPages[i] == page) { return i; }
            }
            return -1;
        }
        #endregion (-FindIndex)

        //-------------------------------------------------------------------------------
        #region -SuspendPaint 描画抑制
        //-------------------------------------------------------------------------------
        /// <summary>描画抑制</summary>
        public void SuspendPaint()
        {
            _bSuspendDraw = true;
        }
        //-------------------------------------------------------------------------------
        #endregion (SuspendPaint)
        //-------------------------------------------------------------------------------
        #region -ResumePaint 描画再開
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
            if (_bSuspendDraw && (m.Msg == 0x000f || m.Msg == 0x0014)) { return; }
            base.WndProc(ref m);
        }
        #endregion (#[override]WndProc)
        //-------------------------------------------------------------------------------

        TabPageExCollection _tabpageCollection;
        /// <summary>タブコントロールのタブページのコレクションを取得します。</summary>
        public new TabPageExCollection TabPages
        {
            get { return _tabpageCollection; }
        }

        public new TabPageEx SelectedTab
        {
            get { return _tabpageCollection[base.TabPages.IndexOf(base.SelectedTab)]; }
            set
            {
                if (!_tabpageCollection.Contains(value)) { return; }
                base.SelectedTab = base.TabPages[_tabpageCollection.IndexOf(value)];
            }
        }

        public new TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                base.Alignment = value;
                OnAlignmentChanged(EventArgs.Empty);
            }
        }

        //-------------------------------------------------------------------------------
        #region OnAlignmentChanged
        //-------------------------------------------------------------------------------
        //
        protected void OnAlignmentChanged(EventArgs e)
        {
            foreach (var page in this.TabPages) { page.AdjustWidth(); }
        }
        #endregion (OnAlignmentChanged)

        //-------------------------------------------------------------------------------
        #region (class)TabPageExCollection
        //-------------------------------------------------------------------------------
        public class TabPageExCollection : IList<TabPageEx>, ICollection<TabPageEx>, IEnumerable<TabPageEx>
        {
            private List<TabPageEx> _tabPageList = new List<TabPageEx>();
            private TabPageCollection _baseCollection;

            internal TabPageExCollection(TabPageCollection baseCollection) { _baseCollection = baseCollection; }

            public int IndexOf(TabPageEx item)
            {
                return _tabPageList.IndexOf(item);
            }

            public void Insert(int index, TabPageEx item)
            {
                _baseCollection.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                _baseCollection.RemoveAt(index);
            }

            public TabPageEx this[int i]
            {
                get { return _tabPageList[i]; }
                set { _tabPageList[i] = value; }
            }

            internal void AddListOnly(TabPageEx item)
            {
                _tabPageList.Add(item);
            }

            public void Add(TabPageEx item)
            {
                _baseCollection.Add(item);
            }

            public void Clear()
            {
                _baseCollection.Clear();
            }

            public bool Contains(TabPageEx item)
            {
                return _tabPageList.Contains(item);
            }

            public void CopyTo(TabPageEx[] array, int arrayIndex)
            {
                _tabPageList.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _tabPageList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            internal bool RemoveListOnly(TabPageEx item)
            {
                return _tabPageList.Remove(item);
            }

            public bool Remove(TabPageEx item)
            {
                if (_tabPageList.Contains(item)) {
                    _baseCollection.Remove(item);
                    return true;
                }
                return false;
            }

            public IEnumerator<TabPageEx> GetEnumerator()
            {
                return _tabPageList.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _tabPageList.GetEnumerator();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)TabPageExCollection)
    }



    //-------------------------------------------------------------------------------
    #region (class)TabMoveEventArgs
    //-------------------------------------------------------------------------------
    public class TabMoveEventArgs : EventArgs
    {
        public int MoveSrcIndex { get; private set; }
        public int MoveDstIndex { get; private set; }

        public TabMoveEventArgs(int i, int j)
        {
            MoveSrcIndex = i;
            MoveDstIndex = j;
        }
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)TabMoveEventArgs)
}