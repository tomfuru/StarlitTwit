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
        private List<EntityInfo> _entityList;
        private Range _onRange = Range.Empty;

        public const string HASH_REGEX_PATTERN = @"(?<entity>[@#][a-zA-Z0-9_]+?)($|[^a-zA-Z0-9_])";

        /// <summary>テキストボックス内の特殊項目(URL除く)がクリックされた時に発生するイベント</summary>
        public event EventHandler<TweetItemClickEventArgs> TweetItemClick;

        //-------------------------------------------------------------------------------
        #region -EntityInfo 構造体：
        //-------------------------------------------------------------------------------
        /// <summary>
        /// エンティティの情報を表します。
        /// </summary>
        private struct EntityInfo
        {
            /// <summary>アイテムの種類．nullの時はURL</summary>
            public ItemType? type;
            /// <summary>範囲</summary>
            public Range range;
            /// <summary>アイテムの情報を表す文字列</summary>
            public string str;

            public EntityInfo(ItemType? type, Range range, string str)
            {
                this.type = type;
                this.range = range;
                this.str = str;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (EntityInfo)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        public RichTextBoxHash()
        {
            InitializeComponent();
            base.DetectUrls = false;
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +[new]DetectUrls プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 使用できません。
        /// </summary>
        [Description("設定できません。自動的にフォーマットされます。")]
        public new bool DetectUrls
        {
            get { return true; }
        }
        #endregion (DetectUrls)
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
            if (_entityList == null) { return; }

            Range past = new Range(this.SelectionStart, this.SelectionLength);

            int index = this.GetCharIndexFromPosition(e.Location);

            bool onhash = false;
            Range range = Range.Empty;
            foreach (var item in _entityList) {
                if (onhash = item.range.InRange(index)) { range = item.range; break; }
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

            _onRange = Range.Empty;
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
            if (e.Button == MouseButtons.Left) {
                if (!_onRange.IsEmpty) {
                    var entity = _entityList.Find(info => info.range.Equals(_onRange));

                    if (entity.type.HasValue) {
                        if (TweetItemClick != null) { 
                            TweetItemClick.Invoke(this, new TweetItemClickEventArgs(entity.str, entity.type.Value)); 
                        }
                    }
                    else {
                        OnLinkClicked(new LinkClickedEventArgs(entity.str));
                    }
                }
            }
        }
        #endregion (RichTextBoxHash_MouseClick)
        //-------------------------------------------------------------------------------
        #region contextMenu_Opening メニューオープン時
        //-------------------------------------------------------------------------------
        //
        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (_onRange.IsEmpty) {
                DefaultMenuStateChange();
            }
            else {
                var entity = _entityList.Find(info => info.range.Equals(_onRange));

            }
        }
        #endregion (contextMenu_Opening)

        //-------------------------------------------------------------------------------
        #region +ChangeFonts フォントを変更
        //-------------------------------------------------------------------------------
        //
        public void ChangeFonts()
        {
            if (_entityList != null) { _entityList.Clear(); }
            _entityList = GetEntitiesByRegex();

            // 青くなることがあるので全体をまず黒色に
            SelectAll();
            this.SelectionColor = this.ForeColor;

            foreach (var item in _entityList) {
                this.Select(item.range.start, item.range.length);
                FontStyle style = FontStyle.Underline;
                if (item.type.HasValue) { style |= FontStyle.Bold; }
                this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, style);
                this.SelectionColor = Color.Blue;
            }
        }
        #endregion (ChangeFonts)
        //-------------------------------------------------------------------------------
        #region -GetEntitiesByRegex 正規表現を利用してエンティティを抽出します。
        //-------------------------------------------------------------------------------
        //
        private List<EntityInfo> GetEntitiesByRegex()
        {
            List<EntityInfo> list = new List<EntityInfo>();
            Regex r = new Regex(HASH_REGEX_PATTERN);
            foreach (Match m in r.Matches(this.Text)) {
                Group g = m.Groups["entity"];
                switch (g.Value[0]) {
                    case '@':
                        list.Add(new EntityInfo(ItemType.User, new Range(g.Index, g.Length), g.Value.Substring(1)));
                        break;
                    case '#':
                        list.Add(new EntityInfo(ItemType.HashTag, new Range(g.Index, g.Length), g.Value));
                        break;
                    default:
                        Debug.Assert(false,"ここには来ない");
                        break;
                }
            }

            Regex r2 = new Regex(Utilization.URL_REGEX_PATTERN);
            foreach (Match m in r2.Matches(this.Text)) {
                list.Add(new EntityInfo(null, new Range(m.Index, m.Length), m.Value));
            }

            return list;
        }
        #endregion (GetEntitiesByRegex)
    }

    //-------------------------------------------------------------------------------
    #region (Class)Range
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 値の範囲を表します。
    /// </summary>
    public struct Range : IEquatable<Range>
    {
        /// <summary>空のRangeを表します。</summary>
        public static Range Empty = new Range(0, 0);

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
        //-------------------------------------------------------------------------------
        #region +IsEmptyプロパティ：範囲が空かどうか判定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 範囲が空かどうか判定します。
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty
        {
            get { return length == 0; }
        }
        #endregion (IsEmpty)

        //-------------------------------------------------------------------------------
        #region IEquatable<Range>.Equals 等価判断
        //-------------------------------------------------------------------------------
        //
        public bool Equals(Range other)
        {
            return (this.start == other.start && this.length == other.length);
        }
        #endregion (IEquatable<Range>.Equals)

        //-------------------------------------------------------------------------------
        #region +[override]Equals 等価判断
        //-------------------------------------------------------------------------------
        //
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Range)) { return false; }
            return this.Equals((Range)obj);
        }
        #endregion (+[override]Equals)

        //-------------------------------------------------------------------------------
        #region +[override]GetHashCode ハッシュコード取得
        //-------------------------------------------------------------------------------
        //
        public override int GetHashCode()
        {
            return (start.GetHashCode() ^ length.GetHashCode());
        }
        #endregion (+[override]GetHashCode)
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
        /// <summary>URL</summary>
        URL,
        /// <summary>ハッシュタグ</summary>
        HashTag,
        /// <summary>ユーザー</summary>
        User
    }
    //-------------------------------------------------------------------------------
    #endregion (ItemType)
}
