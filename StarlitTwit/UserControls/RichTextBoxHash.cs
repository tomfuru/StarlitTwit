using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace StarlitTwit
{
    public partial class RichTextBoxHash : RichTextBoxExBase
    {
        private List<Range> _hashList;
        private Range _onRange;

        /// <summary>テキストボックス内の特殊項目(URL除く)がクリックされた時に発生するイベント</summary>
        public event EventHandler<TweetItemClickEventArgs> TweetItemClick;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        public RichTextBoxHash()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +[new]Text プロパティ
        //-------------------------------------------------------------------------------
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; /*OnTextChanged(EventArgs.Empty);*/ }
        }
        //-------------------------------------------------------------------------------
        #endregion (Text プロパティ：)

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
        #region ChangeFonts フォントを変更
        //-------------------------------------------------------------------------------
        //
        public void ChangeFonts()
        {
            if (_hashList != null) { _hashList.Clear(); }
            _hashList = GetHashRangesByRegex();

            foreach (Range r in _hashList) {
                Select(r.start, r.length);
                SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold | FontStyle.Underline);
                SelectionColor = Color.Blue;
            }
        }
        #endregion (ChangeFonts)
        //-------------------------------------------------------------------------------
        #region GetHashRangesByRegex 正規表現を利用してハッシュを抽出します。
        //-------------------------------------------------------------------------------
        //
        private List<Range> GetHashRangesByRegex()
        {
            List<Range> list = new List<Range>();
            Regex r = new Regex(@"(?<hash>[@#][a-zA-Z0-9_]+?)($|[^a-zA-Z0-9_])");
            foreach (Match m in r.Matches(this.Text)) {
                Group g = m.Groups["hash"];
                list.Add(new Range(g.Index, g.Length));
            }
            return list;
        }
        #endregion (GetHashRangesByRegex)

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            
        }
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
