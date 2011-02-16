using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace TwitterClient
{
    public class RichTextBoxHash : RichTextBox
    {
        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        public RichTextBoxHash()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        private List<Range> _hashList;
        private bool _suspendTextChanged = false;
        private Range _onRange;

        /// <summary>テキストボックス内の特殊項目(URL除く)がクリックされた時に発生するイベント</summary>
        public event EventHandler<TweetItemClickEventArgs> TweetItemClick;

        //-------------------------------------------------------------------------------
        #region +[new]Text プロパティ
        //-------------------------------------------------------------------------------
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; /*OnTextChanged(EventArgs.Empty); */}
        }
        //-------------------------------------------------------------------------------
        #endregion (Text プロパティ：)

        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_TextChanged テキストチェンジ時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_TextChanged(object sender, EventArgs e)
        {
            if (_suspendTextChanged) { return; }

            //Stopwatch sw = Stopwatch.StartNew();
            if (_hashList != null) { _hashList.Clear(); }
            _hashList = GetHashRanges();

            foreach (Range r in _hashList) {
                Select(r.start, r.length);
                _suspendTextChanged = true;
                SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold | FontStyle.Underline);
                SelectionColor = Color.Blue;
                _suspendTextChanged = false;
            }

            //Console.WriteLine(sw.ElapsedMilliseconds.ToString() + "ms");
            // 選択解除？
        }
        #endregion (RichTextBoxHash_TextChanged)
        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_MouseMove マウス移動時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_MouseMove(object sender, MouseEventArgs e)
        {
            if (_hashList == null) { return; }

            Range past = new Range(this.SelectionStart, this.SelectionLength);

            int index = this.GetCharIndexFromPosition(e.Location);

            bool onhash = false;
            Range range = default(Range);
            foreach (Range r in _hashList) {
                if (onhash = r.InRange(index)) { range = r; break; }
            }
            if (onhash) {

                Point p = this.GetPositionFromCharIndex(range.start);
                Rectangle rec = new Rectangle(p, TextRenderer.MeasureText(this.Text.Substring(range.start, range.length), this.Font));

                if (rec.Contains(e.Location)) {
                    _onRange = range;
                    if (this.Cursor != Cursors.Hand) {
                        this.Cursor = Cursors.Hand;
                    }
                    return;
                }
            }

            _onRange = new Range(0, 0);
            if (this.Cursor != Cursors.IBeam) {
                this.Cursor = Cursors.IBeam;
            }
        }
        #endregion (RichTextBoxHash_MouseMove)

        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_MouseClick マウスクリック時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (_onRange.length > 0) {
                    char ctype = Text[_onRange.start];
                    string item;

                    ItemType type;
                    switch (ctype) {
                        case '@':
                            type = ItemType.User;
                            item = Text.Substring(_onRange.start + 1, _onRange.length - 1);
                            break;
                        case '#':
                            type = ItemType.HashTag;
                            item = Text.Substring(_onRange.start, _onRange.length);
                            break;
                        default:
                            return;
                    }

                    if (TweetItemClick != null) { TweetItemClick.Invoke(this, new TweetItemClickEventArgs(item, type)); }
                }
            }
        }
        #endregion (RichTextBoxHash_MouseClick)

        //-------------------------------------------------------------------------------
        #region -GetHashRanges レンジを返し続けます。
        //-------------------------------------------------------------------------------
        //
        private List<Range> GetHashRanges()
        {
            List<Range> list = new List<Range>();
            int index = 0;
            while (true) {
                int len,
                    start = GetHashRange(index, out len);
                if (start == -1) { break; }
                index = start + len;
                list.Add(new Range(start, len));
            }
            return list;
        }
        #endregion (GetHashRanges)
        //-------------------------------------------------------------------------------
        #region -GetHashRange ハッシュの範囲を求めます.
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ハッシュの範囲("#～～")を求めます。
        /// </summary>
        /// <param name="searchstartindex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private int GetHashRange(int searchstartindex, out int length)
        {
            string text = this.Text;
            const string AVAILABLE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
            

            int start = text.IndexOfAny(new char[]{'#','@'}, searchstartindex);

            // [#,@がない],[#,@が最後]の時はもう#がないので-1を返す
            if (start == -1 || start == text.Length - 1) { length = 0; return -1; }

            // [#,@の次の文字が半角英数じゃない]の時は続きを探す
            if (!AVAILABLE_CHARS.Contains(text[start + 1])) { return GetHashRange(start + 1, out length); }

            // [#,@の後に続きがある]時は空白を探す
            int end = text.IndexOfAny(new char[] { ' ', '　' }, start + 2);
            if (end == -1) { end = this.TextLength; } // 空白がなかったら後ろは最後

            length = end - start;
            return start;
        }
        #endregion (GetHashRange)

        //-------------------------------------------------------------------------------
        #region InitializeComponent
        //-------------------------------------------------------------------------------
        //
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RichTextBoxHash
            // 
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RichTextBoxHash_MouseClick);
            this.TextChanged += new System.EventHandler(this.RichTextBoxHash_TextChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RichTextBoxHash_MouseMove);
            this.ResumeLayout(false);

        }
        #endregion (InitializeComponent)
    }

    //-------------------------------------------------------------------------------
    #region (Class)Range
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 値の範囲を表します。
    /// </summary>
    public struct Range
    {
        /// <summary>始まり</summary>
        public int start;
        /// <summary>長さ</summary>
        public int length;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public Range(int st, int len)
        {
            start = st;
            length = len;
        }
        #endregion (コンストラクタ)
        //-------------------------------------------------------------------------------
        #region +InRange 値が範囲の中にあるか判別します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 値がこの範囲の中にあるかどうか判断します。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="includeBorder">[option]境界を含むかどうか</param>
        /// <returns></returns>
        public bool InRange(int value, bool includeBorder = true)
        {
            return (includeBorder)
               ? ((value >= start) && (value <= start + length))
               : ((value > start) && (value < start + length));
        }
        #endregion (InRange)
    }
    #endregion ((Class)Range)

    //-------------------------------------------------------------------------------
    #region (Class)TweetItemClickEventArgs  クリックしたアイテムの情報
    //-------------------------------------------------------------------------------
    //
    public class TweetItemClickEventArgs : EventArgs
    {
        /// <summary>アイテムの内容</summary>
        public string Item { get; private set; }
        /// <summary>アイテムの種類</summary>
        public ItemType Type { get; private set; }

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public TweetItemClickEventArgs(string item, ItemType type)
        {
            Item = item;
            Type = type;
        }
        #endregion (コンストラクタ)
    }
    #endregion (TweetItemClickEventArgs )

    //-------------------------------------------------------------------------------
    #region +ItemType 列挙体：種類
    //-------------------------------------------------------------------------------
    /// <summary>
    /// クリックしたアイテムの種類
    /// </summary>
    public enum ItemType : byte
    {
        /// <summary>ハッシュタグ</summary>
        HashTag,
        /// <summary>ユーザー</summary>
        User,
    }
    //-------------------------------------------------------------------------------
    #endregion (ItemType)
}
