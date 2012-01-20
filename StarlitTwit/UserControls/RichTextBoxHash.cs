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
        //private List<EntityData> _entityList;
        private EntityData[] _entities;
        private Range _onRange = Range.Empty;
        private Range _mouseDownRange;

        /// <summary>テキストボックス内の特殊項目(URL除く)がクリックされた時に発生するイベント</summary>
        public event EventHandler<TweetItemClickEventArgs> TweetItemClick;

        private Font _urlFont = null;
        private Font _entityFont = null;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        public RichTextBoxHash()
        {
            InitializeComponent();
            base.DetectUrls = false;

            FontStyle style = FontStyle.Underline;
            _urlFont = new Font(this.Font.FontFamily, this.Font.Size, style);
            style |= FontStyle.Bold;
            _entityFont = new Font(this.Font.FontFamily, this.Font.Size, style);
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
        #region EnableEntity プロパティ
        //-------------------------------------------------------------------------------
        private bool _enableEntity = true;
        /// <summary>
        /// Entityのクリックを有効にするか
        /// </summary>
        public bool EnableEntity
        {
            get { return _enableEntity; }
            set { _enableEntity = value; }
        }
        #endregion (EnableEntity)

        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_MouseMove マウス移動時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_MouseMove(object sender, MouseEventArgs e)
        {
            if (!EnableEntity || _entities == null || _entities.Length == 0) { return; }
            
            Range past = new Range(this.SelectionStart, this.SelectionLength);

            int index = this.GetCharIndexFromPosition(e.Location);

            bool onhash = false;
            EntityData entityData = default(EntityData);
            foreach (var item in _entities) {
                if (onhash = item.range.InRange(index)) { entityData = item; break; }
            }

            if (onhash) {
                Range range = entityData.range;
                int i2 = range.Start;
                // 一行ごとにまとめてRectangleを求め含まれているか確認する
                while (i2 < range.Start + range.Length) {
                    int startInd = i2;
                    Point p = this.GetPositionFromCharIndex(i2);
                    i2++;
                    while (i2 < range.Start + range.Length) {
                        Point p2 = this.GetPositionFromCharIndex(i2);
                        if (p2.Y > p.Y) { break; }
                        i2++;
                    }
                    
                    Rectangle rec = new Rectangle(p, TextRenderer.MeasureText(this.Text.Substring(startInd, i2 - startInd), (entityData.type.HasValue) ? _entityFont : _urlFont));
                    if (rec.Contains(e.Location)) {
                        _onRange = range;
                        if (this.Cursor != Cursors.Hand) {
                            this.Cursor = Cursors.Hand;
                        }
                        return;
                    }
                }
            }

            _onRange = Range.Empty;
            if (this.Cursor != Cursors.IBeam) {
                this.Cursor = Cursors.IBeam;
            }
        }
        #endregion (RichTextBoxHash_MouseMove)
        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_MouseDown マウスダウン時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_MouseDown(object sender, MouseEventArgs e)
        {
            if (EnableEntity && e.Button == MouseButtons.Left) {
                _mouseDownRange = _onRange; // 今マウスカーソル上にあるエンティティ記録
            }
        }
        #endregion (RichTextBoxHash_MouseDown)
        //-------------------------------------------------------------------------------
        #region RichTextBoxHash_MouseUp マウスアップ時
        //-------------------------------------------------------------------------------
        //
        private void RichTextBoxHash_MouseUp(object sender, MouseEventArgs e)
        {
            if (EnableEntity && e.Button == MouseButtons.Left) {
                if (!_onRange.IsEmpty && !_mouseDownRange.IsEmpty
                 && _onRange.Start == _mouseDownRange.Start && _onRange.Length == _mouseDownRange.Length // マウスダウンした時と同じものの上か
                 && this.SelectionLength == 0) { // テキスト選択しようとしてるときはクリックイベントを起こさない
                    var entity = Array.Find(_entities, info => info.range.Equals(_onRange));
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
        #endregion (RichTextBoxHash_MouseUp)

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
                var entity = Array.Find(_entities, info => info.range.Equals(_onRange));
                // TODO Entityごとのメニュー？
            }
        }
        #endregion (contextMenu_Opening)

        //-------------------------------------------------------------------------------
        #region +ChangeFonts フォントを変更
        //-------------------------------------------------------------------------------
        //
        public void ChangeFonts(EntityData[] data)
        {
            if (data == null) { return; }
            //if (_entityList != null) { _entityList.Clear(); }
            //_entityList = GetEntitiesByRegex();
            _entities = data;

            // 青くなることがあるので全体をまず黒色に
            SelectAll();
            this.SelectionColor = this.ForeColor;

            foreach (var item in data) {
                this.Select(item.range.Start, item.range.Length);
                this.SelectionFont = (item.type.HasValue) ? _entityFont : _urlFont;
                this.SelectionColor = Color.Blue;
            }
        }
        #endregion (ChangeFonts)
    }



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

}
