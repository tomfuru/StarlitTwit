using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Anis.Controls
{
    /// TabMouseClickイベントを処理するメソッドを表します。
    public delegate void TabMouseEventHandler(object sender, TabMouseEventArgs e);
    /// タブ部分がクリックされた際に、
    /// クリックされたタブを取得出来るTabMouseClickイベントを備えたTabControl
    public class AnisTabControl : TabControl
    {
        #region//各種TabMouseEvent定義
        /// クリックされたタブが取得可能なMouseDownイベント
        [Category("アクション"),
        Description("マウスでタブをクリックした時に発生します。")]
        public event TabMouseEventHandler TabMouseDown;
        /// クリックされたタブが取得可能なMouseClickイベント
        [Category("アクション"),
        Description("マウスでタブをクリックした時に発生します。")]
        public event TabMouseEventHandler TabMouseClick;
        /// クリックされたタブが取得可能なMouseUpイベント
        [Category("アクション"),
        Description("マウスでタブをクリックした時に発生します。")]
        public event TabMouseEventHandler TabMouseUp;
        /// クリックされたタブが取得可能なMouseDoubleClickイベント
        [Category("アクション"),
        Description("マウスでタブをダブルクリックした時に発生します。")]
        public event TabMouseEventHandler TabMouseDoubleClick;
        /// クリックされたタブが取得可能なMouseWheelイベント
        [Category("アクション"),
        Description("マウスホイールがタブ上で動かされた時に発生します。")]
        public event TabMouseEventHandler TabMouseWheel;
        /// クリックされたタブが取得可能なMouseMoveイベント
        [Category("アクション"),
        Description("マウスポインタがコントロール上を移動すると発生します。")]
        public event TabMouseEventHandler TabMouseMove;
        #endregion

        /// XMLコメントが無いって警告うざい
        public AnisTabControl()
        {
            #region//TabMouseEvent関連イベント登録
            this.MouseDown += new MouseEventHandler(AnisTabControl_MouseDown);
            this.MouseClick += new MouseEventHandler(AnisTabControl_MouseClick);
            this.MouseUp += new MouseEventHandler(AnisTabControl_MouseUp);
            this.MouseDoubleClick += new MouseEventHandler(AnisTabControl_MouseDoubleClick);
            this.MouseWheel += new MouseEventHandler(AnisTabControl_MouseWheel);
            this.MouseMove += new MouseEventHandler(AnisTabControl_MouseMove);
            #endregion
        }

        /// Point上のタブコントロールの
        ///タブ部分に対応したTabPageのインデックスを取得
        /// インデックスを取得するPoint
        /// Point上にタブが無ければ-1が返ります。。
        private int TabIndexFromPoint(Point point)
        {
            for (int i = 0; i <= this.TabPages.Count - 1; i++) {
                if (this.GetTabRect(i).Contains(point)) {
                    return i;
                }
            }
            return -1;
        }

        #region//Tabマウスイベント
        void AnisTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseDown(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        void AnisTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseClick(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        void AnisTabControl_MouseUp(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseUp(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        void AnisTabControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseDoubleClick(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        void AnisTabControl_MouseWheel(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseWheel(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        void AnisTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            try {
                int index = TabIndexFromPoint(e.Location);
                if (index != -1) {
                    TabMouseEventArgs E = new TabMouseEventArgs(
                      index, e.Button, e.Clicks, e.X, e.Y, e.Delta);
                    this.TabMouseMove(sender, E);
                }
            }
            catch (NullReferenceException) { }
        }
        #endregion

        #region//ドラッグ＆ドロップ
        Point mouseDownPoint = Point.Empty;
        bool sortTabDragDrop;
        /// trueでドラッグ＆ドロップによるタブの
        ///並び替えを可能にします。
        [Category("動作"),
        Description("trueでドラッグ&ドロップによるタブの並び替えを可能にします。")]
        public bool SortTabDragDrop
        {
            set
            {
                sortTabDragDrop = value;
                if (value) {
                    this.MouseDown += new MouseEventHandler(AnisTabControl_MouseDown1);
                    this.MouseUp += new MouseEventHandler(AnisTabControl_MouseUp1);
                    this.TabMouseMove += new TabMouseEventHandler(AnisTabControl_TabMouseMove);
                    this.DragEnter += new DragEventHandler(AnisTabControl_DragEnter);
                    this.DragOver += new DragEventHandler(AnisTabControl_DragOver);
                    this.DragDrop += new DragEventHandler(AnisTabControl_DragDrop);
                }
                else {
                    this.MouseDown -= new MouseEventHandler(AnisTabControl_MouseDown1);
                    this.MouseUp -= new MouseEventHandler(AnisTabControl_MouseUp1);
                    this.TabMouseMove -= new TabMouseEventHandler(AnisTabControl_TabMouseMove);
                    this.DragEnter -= new DragEventHandler(AnisTabControl_DragEnter);
                    this.DragOver -= new DragEventHandler(AnisTabControl_DragOver);
                    this.DragDrop -= new DragEventHandler(AnisTabControl_DragDrop);
                }
            }
            get
            {
                return sortTabDragDrop;
            }
        }

        void AnisTabControl_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                mouseDownPoint = e.Location;
            }
            else {
                mouseDownPoint = Point.Empty;
            }
        }

        void AnisTabControl_MouseUp1(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        void AnisTabControl_TabMouseMove(object sender, TabMouseEventArgs e)
        {
            if (mouseDownPoint != Point.Empty) {
                Rectangle mouseMoveRectangle = new Rectangle(
                  mouseDownPoint.X - 5, mouseDownPoint.Y - 5,
                  10, 10);
                if (!mouseMoveRectangle.Contains(e.Location)) {
                    this.DoDragDrop(this.TabPages[e.TabIndex], DragDropEffects.Move);
                }
            }
        }

        void AnisTabControl_DragOver(object sender, DragEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        void AnisTabControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)) &&
              this.TabPages.Contains((TabPage)(e.Data.GetData(typeof(TabPage)))) &&
              this.TabIndexFromPoint(this.PointToClient(new Point(e.X, e.Y))) != -1) {
                e.Effect = DragDropEffects.Move;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        void AnisTabControl_DragDrop(object sender, DragEventArgs e)
        {
            int index = TabIndexFromPoint(this.PointToClient(new Point(e.X, e.Y)));
            TabPage tabPage = (TabPage)(e.Data.GetData(typeof(TabPage)));
            this.TabPages.Remove(tabPage);
            this.TabPages.Insert(index, tabPage);
            this.SelectTab(index);
        }
        #endregion
    }


    /// TabMouseClickイベントのデータ
    public class TabMouseEventArgs : MouseEventArgs
    {
        private int tabIndex_;
        /// クリックされたタブに対応したタブのインデックス
        public TabMouseEventArgs(
            int tabIndex, MouseButtons button, int clicks, int x, int y, int delta)
            : base(
                button, clicks, x, y, delta)
        {
            tabIndex_ = tabIndex;
        }
        /// クリックされたタブに対応したタブページ
        public int TabIndex
        {
            get
            {
                return tabIndex_;
            }
        }
    }
}