using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    //-------------------------------------------------------------------------------
    #region (class)ShortcutKeyData
    //-------------------------------------------------------------------------------
    [Serializable]
    public class ShortcutKeyData : SaveDataClassBase<ShortcutKeyData>
    {
        /// <summary>メインフォーム上のショートカット</summary>
        public SerializableDictionary<KeyData, ShortcutType_MainForm> MainFormShortcutDic = new SerializableDictionary<KeyData,ShortcutType_MainForm>();
        /// <summary>発言上のショートカット</summary>
        public SerializableDictionary<KeyData, ShortcutType_Status> StatusShortcutDic = new SerializableDictionary<KeyData,ShortcutType_Status>();

        //-------------------------------------------------------------------------------
		#region DefaultData デフォルトのデータを返します
		//-------------------------------------------------------------------------------
		//
		public static ShortcutKeyData DefaultData()
		{
			var dic = new ShortcutKeyData();
            // TODO:Shortcut処理
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
        None,
        設定画面,
        再起動,
        終了,
        時刻を指定して発言取得,
        ユーザー検索画面表示,
        API使用制限回数情報表示,
        認証,
        フォロー数・発言数更新,
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
        None,
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
    public class KeyData
    {
        // TODO:Oemなキーを文字列変換時に置換
        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        public const string NOT_CONFIGED = "設定なし";
        private const string CTRL = "Ctrl";
        private const string SHIFT = "Shift";
        private const string ALT = "Alt";
        private const char PLUS = '+';
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
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Ctrl) {
                if (sb.Length != 0) { sb.Append(PLUS); }
                sb.Append(CTRL);
            }
            if (this.Alt) {
                if (sb.Length != 0) { sb.Append(PLUS); }
                sb.Append(ALT);
            }
            if (this.Shift) {
                if (sb.Length != 0) { sb.Append(PLUS); }
                sb.Append(SHIFT);
            }
            if (sb.Length != 0) { sb.Append(PLUS); }
            sb.Append(this.Key.ToString());
            return sb.ToString();
        }
        #endregion (+[override]ToString)

        //-------------------------------------------------------------------------------
        #region +[static]FromString 文字列から
        //-------------------------------------------------------------------------------
        //
        public static KeyData FromString(string text)
        {
            string[] sKeyElements = text.Split(PLUS);        // +で区切る
            KeyData keyData = new KeyData();

            foreach (string str in sKeyElements) {
                if (str.Equals(CTRL)) {
                    keyData.Ctrl = true;
                }
                else if (str.Equals(ALT)) {
                    keyData.Alt = true;
                }
                else if (str.Equals(SHIFT)) {
                    keyData.Shift = true;
                }
                else if (str.Equals(NOT_CONFIGED)) {
                    return null;
                }
                else {
                    keyData.Key = (Keys)MyConvert.EnumParse<Keys>(str);
                }

            }

            return keyData;
        }
        #endregion (FromString)
    }
    //-------------------------------------------------------------------------------
    #endregion (KeyData)
}
