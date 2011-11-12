using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    //-------------------------------------------------------------------------------
    #region (class)ShortcutKeyData
    //-------------------------------------------------------------------------------
    [Serializable]
    public class ShortcutKeyData
    {
        // TODO: KeyData
        /// <summary>メインフォーム上のショートカット</summary>
        public SerializableDictionary<int, ShortcutType_MainForm> MainFormShortcutDic = new SerializableDictionary<int,ShortcutType_MainForm>();
        /// <summary>発言上のショートカット</summary>
        public SerializableDictionary<int, ShortcutType_Status> StatusShortcutDic = new SerializableDictionary<int,ShortcutType_Status>();

        //-------------------------------------------------------------------------------
		#region DefaultData デフォルトのデータを返します
		//-------------------------------------------------------------------------------
		//
		public static ShortcutKeyData DefaultData()
		{
			var dic = new ShortcutKeyData();
            // TODO:
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
        お気に入り追加+リツイート,
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
}
