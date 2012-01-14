using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace StarlitTwit
{
    //-------------------------------------------------------------------------------
    #region (class)ShortcutKeyData
    //-------------------------------------------------------------------------------
    [Serializable]
    public class ShortcutKeyData : SaveDataClassBase<ShortcutKeyData>
    {
        /// <summary>メインフォーム上のショートカット</summary>
        public SerializableDictionary<KeyData, ShortcutType_MainForm> MainFormShortcutDic = new SerializableDictionary<KeyData, ShortcutType_MainForm>();
        /// <summary>発言上のショートカット</summary>
        public SerializableDictionary<KeyData, ShortcutType_Status> StatusShortcutDic = new SerializableDictionary<KeyData, ShortcutType_Status>();

        //-------------------------------------------------------------------------------
        #region +[override]Save 保存
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このインスタンスをファイルに保存します。失敗した時は別の場所にファイルを保存するかどうか尋ねます。
        /// </summary>
        public override void Save(string filePath)
        {
            const string MESSAGE_1 = "設定の保存ができませんでした。\n設定を別の名前で保存しますか？";
            const string MESSAGE_2 = "設定を別の名前で保存しますか？";
            XmlSerializer serializer = new XmlSerializer(typeof(ShortcutKeyData));
            if (!this.SaveBase(filePath)) {
                string dispMessage = MESSAGE_1;
                do {
                    if (Message.ShowQuestionMessage(dispMessage) == DialogResult.Yes) {
                        using (SaveFileDialog sfd = new SaveFileDialog()) {
                            sfd.FileName = string.Format("{0}_tmp.xml", Path.GetFileNameWithoutExtension(filePath));
                            sfd.Filter = "ショートカット設定ファイル(*.xml)|*.xml";
                            if (sfd.ShowDialog() == DialogResult.OK) {
                                if (!this.SaveBase(sfd.FileName)) {
                                    dispMessage = MESSAGE_1;
                                    continue;
                                }
                                break;
                            }
                            else { dispMessage = MESSAGE_2; }
                        }
                    }
                    else { break; }
                } while (true);
            }
        }
        #endregion (Save)

        //-------------------------------------------------------------------------------
        #region DefaultData デフォルトのデータを返します
        //-------------------------------------------------------------------------------
        //
        public static ShortcutKeyData DefaultData()
        {
            var dic = new ShortcutKeyData();
            //-------------------------------------------------------------------------------
            #region MainFormShortcutDic initialization 
		    //-------------------------------------------------------------------------------
            dic.MainFormShortcutDic.Add(KeyData.FromString("F5"), ShortcutType_MainForm.選択中タブ更新);
            //dic.MainFormShortcutDic.Add(KeyData.FromString(""), ShortcutType_MainForm);
            //-------------------------------------------------------------------------------
		    #endregion (MainFormShortcutDic initialization)
            //-------------------------------------------------------------------------------
            #region StatusShortcutDic initialization
            //-------------------------------------------------------------------------------
            dic.StatusShortcutDic.Add(KeyData.FromString("R"), ShortcutType_Status.リプライ);
            dic.StatusShortcutDic.Add(KeyData.FromString("Q"), ShortcutType_Status.引用);
            dic.StatusShortcutDic.Add(KeyData.FromString("Shift+Q"), ShortcutType_Status.引用リプライ);
            dic.StatusShortcutDic.Add(KeyData.FromString("C"), ShortcutType_Status.会話表示);
            dic.StatusShortcutDic.Add(KeyData.FromString("Shift+R"), ShortcutType_Status.リツイート);
            dic.StatusShortcutDic.Add(KeyData.FromString("Ctrl+R"), ShortcutType_Status.リツイートしたユーザーを表示);
            dic.StatusShortcutDic.Add(KeyData.FromString("Delete"), ShortcutType_Status.削除);
            //dic.StatusShortcutDic.Add(KeyData.FromString(""), ShortcutType_Status);
            //-------------------------------------------------------------------------------
            #endregion (StatusShortcutDic initialization)
            return dic;
        }
        #endregion (DefaultData)
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)ShortcutKeyData)

    //-------------------------------------------------------------------------------
    #region ShortcutType_MainForm 列挙体
    //-------------------------------------------------------------------------------
    public enum ShortcutType_MainForm
    {
        //None,
        設定画面,
        再起動,
        終了,
        時刻を指定して発言取得,
        ユーザー検索画面表示,
        API使用制限回数情報表示,
        認証,
        フォロー数と発言数更新,
        フォロワー表示,
        フレンド表示,
        自分のプロフィール,
        自分のリスト,
        所属リスト表示,
        フォロー中のリスト,
        自分のお気に入り,
        自分のリツイート,
        フォロワーのリツイート,
        自分がされたリツイート,
        ブロックユーザー,
        子画面全消去,
        選択中タブ更新,
        全タブ更新,
        次のタブへ,
        前のタブへ,
        ホームタブへ,
        リプライタブへ,
        履歴タブへ,
        DMタブへ,
        入力テキストボックスにフォーカス,
    }
    //-------------------------------------------------------------------------------
    #endregion (ShortcutType_MainForm 列挙体)
    //-------------------------------------------------------------------------------
    #region ShortcutType_Status 列挙体
    //-------------------------------------------------------------------------------
    public enum ShortcutType_Status
    {
        //None,
        リプライ,
        引用,
        引用リプライ,
        リツイート,
        ダイレクトメッセージ,
        会話表示,
        お気に入り追加,
        お気に入り削除,
        お気に入り追加とリツイート,
        削除,
        発言をブラウザで開く,
        リツイートしたユーザーを表示,
        より古い発言を取得,
        発言者のプロフィールを表示,
        発言者の最近の発言を表示,
        発言者のホームをブラウザで開く,
        返信先ユーザーのプロフィールを表示,
        返信先ユーザーの最近の発言を表示,
        返信先ユーザーのホームをブラウザで開く,
        リツイーターのプロフィールを表示,
        リツイーターの最近の発言を表示,
        リツイーターのホームをブラウザで開く,
    }
    //-------------------------------------------------------------------------------
    #endregion (ShortcutType_Status 列挙体)

    //-------------------------------------------------------------------------------
    #region (class)KeyData
    //-------------------------------------------------------------------------------
    /// <summary>
    /// キー入力データを表す構造体です。
    /// </summary>
    [Serializable]
    public class KeyData : IDeepCopyClonable<KeyData>, IEquatable<KeyData>, IXmlSerializable
    {
        //-------------------------------------------------------------------------------
        #region -(static class)KeysConverter
        //-------------------------------------------------------------------------------
        private static class KeysConverter
        {
            private static Dictionary<string, string> _keys_to_original = new Dictionary<string,string>();
            private static Dictionary<string, string> _original_to_keys = new Dictionary<string, string>();

            static KeysConverter()
            {
                List<Tuple<string, string>> correspondence_list = new List<Tuple<string, string>>();
                for (int i = 0; i <= 9; ++i) { // D0-D9 -> 0-9
                    correspondence_list.Add(Tuple.Create(string.Format("D{0}", i), i.ToString()));
                }

                if (Properties.Settings.Default.KeyboardType.Equals("us")) {
                     correspondence_list.AddRange(new[] {
                        // US keyboardで確認
                        Tuple.Create(Keys.Oemtilde.ToString(),"`"),
                        Tuple.Create(Keys.OemMinus.ToString(),"-"),
                        Tuple.Create(Keys.Oemplus.ToString(),"="),
                        Tuple.Create(Keys.Oem5.ToString(),"\\"),

                        Tuple.Create(Keys.OemOpenBrackets.ToString(),"["),
                        Tuple.Create(Keys.Oem6.ToString(),"]"),

                        Tuple.Create(Keys.Oem1.ToString(),";"),
                        Tuple.Create(Keys.Oem7.ToString(),"'"),

                        Tuple.Create(Keys.Oemcomma.ToString(),","),
                        Tuple.Create(Keys.OemPeriod.ToString(),"."),
                        Tuple.Create(Keys.OemQuestion.ToString(),"/"),

                        Tuple.Create(Keys.Next.ToString(),"PageDown")
                    });
                }
                else {
                     correspondence_list.AddRange(new[] {
                        // JIS keyboardで確認(要確認) 
                        Tuple.Create(Keys.OemMinus.ToString(),"-"),
                        Tuple.Create(Keys.Oem7.ToString(),"^"),
                        Tuple.Create(Keys.Oem5.ToString(),"\\"),

                        Tuple.Create(Keys.Oemtilde.ToString(),"@"),
                        Tuple.Create(Keys.OemOpenBrackets.ToString(),"["),

                        Tuple.Create(Keys.Oemplus.ToString(),";"),
                        Tuple.Create(Keys.Oem1.ToString(),":"),
                        Tuple.Create(Keys.Oem6.ToString(),"]"),

                        Tuple.Create(Keys.Oemcomma.ToString(),","),
                        Tuple.Create(Keys.OemPeriod.ToString(),"."),
                        Tuple.Create(Keys.OemQuestion.ToString(),"/"),
                        Tuple.Create(Keys.OemBackslash.ToString(), "Backslash"),

                        Tuple.Create(Keys.Next.ToString(),"PageDown")
                    });
                }

                foreach (var cor in correspondence_list) {
                    _keys_to_original.Add(cor.Item1, cor.Item2);
                    _original_to_keys.Add(cor.Item2, cor.Item1);
                }
            }

            public static string ConvertKeysToOriginal(string str)
            {
                
                return (_keys_to_original.ContainsKey(str)) ? _keys_to_original[str] : str;
            }

            public static string ConvertOriginalToKeys(string str)
            {
                return (_original_to_keys.ContainsKey(str)) ? _original_to_keys[str] : str;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((static class)KeysConverter)

        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        public const string NOT_CONFIGED = "設定なし";
        private const string CTRL = "Ctrl";
        private const string SHIFT = "Shift";
        private const string ALT = "Alt";
        private const char PLUS = '+';
        private const string PLUS_INTERVAL = " + ";
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region Alt プロパティ：Alt修飾があるかどうか
        //-------------------------------------------------------------------------------
        private bool _alt;
        /// <summary>Alt修飾があるかどうか</summary>
        public bool Alt
        {
            get { return _alt; }
            set { _alt = value; }
        }
        #endregion (Alt)
        //-------------------------------------------------------------------------------
        #region Shift プロパティ：Shift修飾があるかどうか
        //-------------------------------------------------------------------------------
        private bool _shift;
        /// <summary>
        /// Shift修飾があるかどうか
        /// </summary>
        public bool Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }
        #endregion (Shift)
        //-------------------------------------------------------------------------------
        #region Ctrl プロパティ：Crtl修飾があるかどうか
        //-------------------------------------------------------------------------------
        private bool _ctrl;
        /// <summary>
        /// Crtl修飾があるかどうか
        /// </summary>
        public bool Ctrl
        {
            get { return _ctrl; }
            set { _ctrl = value; }
        }
        #endregion (Ctrl)
        /// <summary>メインキー</summary>
        public Keys Key;

        //-------------------------------------------------------------------------------
        #region +IsModifyOnly 修飾キーしかない場合にtrue
        //-------------------------------------------------------------------------------
        //
        public bool IsModifyOnly()
        {
            return (this.Key == Keys.None)
                || (this.Key == Keys.ControlKey)
                || (this.Key == Keys.ShiftKey)
                || (this.Key == Keys.Menu);
        }
        #endregion (IsModifyOnly)

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return ToString(false);
        }
        #endregion (+[override]ToString)
        //-------------------------------------------------------------------------------
        #region +ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public string ToString(bool permitLastPlus)
        {
            StringBuilder sb = new StringBuilder();
            Keys k = this.Key;
            if (this.Ctrl) {
                sb.Append(CTRL);
                if (k == Keys.ControlKey) { k = Keys.None; }
            }
            if (this.Shift) {
                if (sb.Length != 0) { sb.Append(PLUS_INTERVAL); }
                sb.Append(SHIFT);
                if (k == Keys.ShiftKey) { k = Keys.None; }
            }
            if (this.Alt) {
                if (sb.Length != 0) { sb.Append(PLUS_INTERVAL); }
                sb.Append(ALT);
                if (k == Keys.Menu) { k = Keys.None; }
            }
            if (permitLastPlus && sb.Length != 0) { sb.Append(PLUS_INTERVAL); }
            if (k != Keys.None) {
                if (!permitLastPlus && sb.Length != 0) { sb.Append(PLUS_INTERVAL); }
                sb.Append(KeysConverter.ConvertKeysToOriginal(k.ToString()));
            }

            return sb.ToString();
        }
        #endregion (ToString)

        //-------------------------------------------------------------------------------
        #region +[static]FromString 文字列から
        //-------------------------------------------------------------------------------
        //
        public static KeyData FromString(string text)
        {
            if (string.IsNullOrEmpty(text)) { return null; }

            string[] sKeyElements = text.Split(PLUS);        // +で区切る
            KeyData keyData = new KeyData();

            bool validData = false;
            foreach (string s in sKeyElements) {
                string str = s.Trim();
                if (str.Length == 0) { continue; }
                if (str.Equals(CTRL)) {
                    keyData.Ctrl = true;
                    validData = true;
                }
                else if (str.Equals(SHIFT)) {
                    keyData.Shift = true;
                    validData = true;
                }
                else if (str.Equals(ALT)) {
                    keyData.Alt = true;
                    validData = true;
                }
                else if (str.Equals(NOT_CONFIGED)) {
                    return null;
                }
                else {
                    keyData.Key = MyConvert.EnumParse<Keys>(KeysConverter.ConvertOriginalToKeys(str));
                    validData = true;
                }

            }

            if (!validData) { throw new ArgumentException("無効な文字列です"); }

            return keyData;
        }
        #endregion (FromString)



        //-------------------------------------------------------------------------------
        #region DeepCopyClone
        //-------------------------------------------------------------------------------
        public KeyData DeepCopyClone()
        {
            return (KeyData)this.MemberwiseClone();
        }
        #endregion (DeepCopyClone)

        //-------------------------------------------------------------------------------
        #region +Equals 等値
        //-------------------------------------------------------------------------------
        //
        public bool Equals(KeyData other)
        {
            return (other != null)
                && (this._alt == other._alt)
                && (this._shift == other._shift)
                && (this._ctrl == other._ctrl)
                && (this.Key == other.Key);
        }
        #endregion (Equals)

        //-------------------------------------------------------------------------------
        #region +[override]Equals
        //-------------------------------------------------------------------------------
        //
        public override bool Equals(object obj)
        {
            if (obj is KeyData) { return this.Equals((KeyData)obj); }
            return base.Equals(obj);
        }
        #endregion (+[override]Equals)

        //-------------------------------------------------------------------------------
        #region +[override]GetHashCode
        //-------------------------------------------------------------------------------
        //
        public override int GetHashCode()
        {
            return _alt.GetHashCode() ^ _ctrl.GetHashCode() ^ _shift.GetHashCode() ^ Key.GetHashCode();
        }
        #endregion (+[override]GetHashCode)

        //-------------------------------------------------------------------------------
        #region operator ==
        //-------------------------------------------------------------------------------
        //
        public static bool operator ==(KeyData lhs, KeyData rhs)
        {
            if ((object)lhs == null && (object)rhs == null) { return true; }
            if ((object)lhs != null) { return lhs.Equals(rhs); }
            else { return false; }
        }
        #endregion (operator ==)
        #region operator !=
        //-------------------------------------------------------------------------------
        //
        public static bool operator !=(KeyData lhs, KeyData rhs)
        {
            return !(lhs == rhs);
        }
        #endregion (operator !=)

        //-------------------------------------------------------------------------------
        #region IXmlSerializable.GetSchema
        //-------------------------------------------------------------------------------
        //
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }
        #endregion (IXmlSerializable.GetSchema)
        //-------------------------------------------------------------------------------
        #region IXmlSerializable.ReadXml
        //-------------------------------------------------------------------------------
        //
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string str = reader.ReadString();

            KeyData k = KeyData.FromString(str);
            this.Alt = k.Alt;
            this.Shift = k.Shift;
            this.Ctrl = k.Ctrl;
            this.Key = k.Key;

            reader.ReadEndElement();
        }
        #endregion (IXmlSerializable.ReadXml)
        //-------------------------------------------------------------------------------
        #region IXmlSerializable.WriteXml
        //-------------------------------------------------------------------------------
        //
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(this.ToString());
        }
        #endregion (IXmlSerializable.WriteXml)
    }
    //-------------------------------------------------------------------------------
    #endregion (KeyData)
}
