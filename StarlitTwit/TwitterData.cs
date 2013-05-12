using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{

    //-----------------------------------------------------------------------------------
    #region +UserAuthInfo 構造体：ユーザー認証情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// ユーザー名やOAuth認証のためのユーザートークンを格納する構造体です。
    /// </summary>
    [Serializable]
    public struct UserAuthInfo
    {
        public string ScreenName;
        public long ID;

        public string AccessToken;
        public string AccessTokenSecret;
    }
    //-------------------------------------------------------------------------------
    #endregion (UserData)
    //-------------------------------------------------------------------------------
    #region (class)SequentData
    //-------------------------------------------------------------------------------
    /// <summary>
    /// Cursorにより位置づけられる連続データを表します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SequentData<T>
    {
        public IEnumerable<T> Data { get; private set; }
        public long NextCursor { get; private set; }
        public long PreviousCursor { get; private set; }

        public SequentData(IEnumerable<T> data, long next_cursor, long previous_cursor)
        {
            Data = data;
            NextCursor = next_cursor;
            PreviousCursor = previous_cursor;
        }
    }
    //-------------------------------------------------------------------------------
    #endregion (SequentData)
    //-----------------------------------------------------------------------------------
    #region TwitData 構造体：1発言に関する情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 1発言に関する情報です。
    /// </summary>
    public class TwitData
    {
        // ツイート種類
        public TwitType TwitType;

        // 発言情報
        /// <summary>発言ID</summary>
        public long StatusID;
        /// <summary>時間</summary>
        public DateTime Time;
        /// <summary>呟き内容</summary>
        public string Text;
        /// <summary>発言クライアント/アプリケーション</summary>
        public string Source;
        /// <summary>リプライ時のみ：返信先ステータスID</summary>
        public long Mention_StatusID;
        /// <summary>リプライ時のみ：返信先ユーザーID</summary>
        public long Mention_UserID;
        /// <summary>リプライ時のみ：返信先ユーザー表示名</summary>
        public string Mention_ScreenName;
        /// <summary>ダイレクトメッセージの送信先のユーザー名</summary>
        public string DMScreenName;
        /// <summary>発言をお気に入りに登録しているか</summary>
        public bool Favorited;
        /// <summary>RT発言情報</summary>
        public TwitData RTTwitData;
        /// <summary>エンティティ</summary>
        public EntityData[] Entities;
        /// <summary>URLデータ</summary>
        public URLData[] UrlData;
        /// <summary>RTされた数</summary>
        public int RetweetedCount;

        // 発言ユーザー情報
        /// <summary>ユーザーID</summary>
        public long UserID;
        /// <summary>ユーザー名</summary>
        public string UserName;
        /// <summary>ユーザー表示名</summary>
        public string UserScreenName;
        /// <summary>アイコンURL</summary>
        public string IconURL;
        /// <summary>ユーザーがプロテクトか</summary>
        public bool UserProtected;

        //-------------------------------------------------------------------------------
        #region MainTwitData プロパティ：表示する際にメインとなるTwitDataを取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 表示する際にメインとなるTwitDataを取得します。
        /// </summary>
        public TwitData MainTwitData
        {
            get { return TwitData.IsRT(this) ? RTTwitData : this; }
        }
        #endregion (MainTwitData)
        //-------------------------------------------------------------------------------
        #region +TextIncludeUserMention TextにUserへの言及が含まれているかどうかを取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// TextにUserへの言及[@(ScreenName)]が含まれているかどうかを取得します。
        /// </summary>
        /// <param name="screen_name"></param>
        /// <returns></returns>
        public bool TextIncludeUserMention(string screen_name)
        {
            return Text.ToLower().Contains('@' + screen_name.ToLower());
        }
        #endregion (TextIncludeUserMention)
        //-------------------------------------------------------------------------------
        #region +TextWithShortenURL 短縮URLでの文字列を取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 短縮URLでの文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public string TextWithShortenURL()
        {
            string text = this.Text;
            if (this.UrlData != null) {
                foreach (var u in this.UrlData) {
                    text = text.Replace(u.expand_url, u.shorten_url);
                }
            }
            return text;
        }
        #endregion (TextWithShortenURL)
        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// この構造体を文字列にします．
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return StatusID.ToString() + " by " + UserScreenName;
        }
        #endregion (ToString)

        //-------------------------------------------------------------------------------
        #region +[static]IsMention Mentionかどうか
        //-------------------------------------------------------------------------------
        //
        public static bool IsMention(TwitData twitdata)
        {
            return (twitdata.MainTwitData.Mention_StatusID >= 0);
        }
        #endregion (IsMention)
        //-------------------------------------------------------------------------------
        #region +[static]IsRT Retweetかどうか
        //-------------------------------------------------------------------------------
        //
        public static bool IsRT(TwitData twitdata)
        {
            return (twitdata.TwitType == TwitType.Retweet);
        }
        #endregion (IsRT)
        //-------------------------------------------------------------------------------
        #region +[static]IsDM DirectMessageかどうか
        //-------------------------------------------------------------------------------
        /// <summary>
        /// この発言がDirectMessageかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public static bool IsDM(TwitData twitdata)
        {
            return (twitdata.TwitType == TwitType.DirectMessage);
        }
        #endregion (IsDM)
        //-------------------------------------------------------------------------------
        #region +[static]IsMine 自分のものかどうか
        //-------------------------------------------------------------------------------
        //
        public static bool IsMine(TwitData twitdata)
        {
            return (twitdata.UserID == FrmMain.Twitter.ID);
        }
        #endregion (+[static]IsMine)
    }
    //-------------------------------------------------------------------------------
    #endregion (TwitData)
    //-----------------------------------------------------------------------------------
    #region ListData 構造体：1リストに関する情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 1リストに関する情報を表します。
    /// </summary>
    public struct ListData
    {
        /// <summary>リスト作成者ID</summary>
        public long OwnerID;
        /// <summary>リスト作成者ScreenName</summary>
        public string OwnerScreenName;
        /// <summary>リスト作成者のアイコンURL</summary>
        public string OwnerIconURL;

        /// <summary>リストのID</summary>
        public long ID;
        /// <summary>リストの名前</summary>
        public string Name;
        /// <summary>リストの通名</summary>
        public string Slug;
        /// <summary>リストの説明</summary>
        public string Description;
        /// <summary>リストフォロワー数</summary>
        public int SubscriberCount;
        /// <summary>リストのメンバー数</summary>
        public int MemberCount;
        /// <summary>リストをフォローしているか</summary>
        public bool Following;
        /// <summary>公開されているかどうか</summary>
        public bool Public;

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このインスタンスを文字列にします。
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return Name;
        }
        //-------------------------------------------------------------------------------
        #endregion (ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (ListData)
    //-----------------------------------------------------------------------------------
    #region UserProfile 構造体：1ユーザーのプロフィールデータ
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 1ユーザーのプロフィールデータを表します．
    /// </summary>
    public class UserProfile
    {
        /// <summary>ユーザーID</summary>
        public long UserID;
        /// <summary>表示名</summary>
        public string ScreenName;
        /// <summary>ユーザー名</summary>
        public string UserName;
        /// <summary>フォロー数</summary>
        public int FriendNum;
        /// <summary>フォロワー数</summary>
        public int FollowerNum;
        /// <summary>発言数</summary>
        public int StatusNum;
        /// <summary>リストされている数</summary>
        public int ListedNum;
        /// <summary>お気に入り数</summary>
        public int FavoriteNum;
        /// <summary>プロテクト中か</summary>
        public bool Protected;
        /// <summary>フォロー要求を送ったかどうか</summary>
        public bool FolllowRequestSent;
        /// <summary>フォローしているか</summary>
        public bool Following;
        /// <summary>アイコンURL</summary>
        public string IconURL;
        /// <summary>URL</summary>
        public string URL;
        /// <summary>場所</summary>
        public string Location;
        /// <summary>プロフィール説明</summary>
        public string Description;
        /// <summary>登録日時</summary>
        public DateTime RegisterTime;
        /// <summary>最終発言データ</summary>
        public TwitData LastTwitData;
        /// <summary>タイムゾーン</summary>
        public string TimeZone;

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return string.Format("{0}(ID:{1})", ScreenName, UserID);
        }
        #endregion (+[override]ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (UserProfile)
    //-------------------------------------------------------------------------------
    #region APILimitData 構造体：API残使用回数に関する情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public struct APILimitData
    {
        /// <summary>残りのAPI使用可能回数</summary>
        public int Remaining;
        /// <summary>1時間あたりのAPI使用可能回数</summary>
        public int HourlyLimit;
        /// <summary>リセット時刻</summary>
        public DateTime ResetTime;

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return string.Format("{0}/{1} Reset:{2}", Remaining, HourlyLimit, ResetTime.ToString(Utilization.STR_DATETIMEFORMAT));
        }
        #endregion (+[override]ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (APILimitData)
    //-------------------------------------------------------------------------------
    #region RelationShipData 構造体：2ユーザー間の情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 2ユーザー間の情報を表します。SourceがTargetに対してどのようであるかを表します。
    /// </summary>
    public struct RelationshipData
    {
        /// <summary>対象ユーザー名</summary>
        public string Source_ScreenName;
        /// <summary>対象ユーザーID</summary>
        public long Source_UserID;
        /// <summary>ターゲットユーザー名</summary>
        public string Target_ScreenName;
        /// <summary>ターゲットユーザーID</summary>
        public long Target_UserID;
        /// <summary>Source follows Target?</summary>
        public bool Following;
        /// <summary>Target follows Source?</summary>
        public bool Followed;
        /// <summary>スパム認定しているか</summary>
        public bool Marked_Spam;
        /// <summary>DMを送れるか</summary>
        public bool CanDM;
        /// <summary>ブロック中か</summary>
        public bool Blocking;
        /// <summary>Nortificationが有効か</summary>
        public bool Notification_Enabled;
        /// <summary>Retweetを受け取るように設定されているか？[要確認]</summary>
        public bool Want_Retweets;
        /// <summary>？[要確認]</summary>
        public bool AllReplies;

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return string.Format("{0} and {1}", Source_ScreenName, Target_ScreenName);
        }
        #endregion (+[override]ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (RelationShipData)
    //-------------------------------------------------------------------------------
    #region FriendshipData 構造体：指定ユーザーのフォロー情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 自分とあるユーザーのフォロー情報を表します。
    /// </summary>
    public struct FriendshipData
    {
        /// <summary>ユーザーID</summary>
        public long UserID;
        /// <summary>ユーザー名</summary>
        public string UserName;
        /// <summary>ユーザー表示名</summary>
        public string UserScreenName;
        /// <summary>フォローしているか</summary>
        public bool Following;
        /// <summary>フォローされているか</summary>
        public bool Followed;
    }
    //-------------------------------------------------------------------------------
    #endregion (FriendShipData)
    //-----------------------------------------------------------------------------------
    #region TwitType 列挙体：発言タイプ
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 発言タイプを表します。
    /// </summary>
    public enum TwitType : byte
    {
        /// <summary>通常ツイート/リプライ</summary>
        Normal,
        /// <summary>リツイート</summary>
        Retweet,
        /// <summary>ダイレクトメッセージ</summary>
        DirectMessage,
        /// <summary>検索結果ツイート</summary>
        Search
    }
    //-------------------------------------------------------------------------------
    #endregion (TwitType)
    //-------------------------------------------------------------------------------
    #region SuggestionCategoryData 構造体：
    //-------------------------------------------------------------------------------
    /// <summary>
    /// Suggestionのカテゴリごとのデータ
    /// </summary>
    public struct SuggestionCategoryData
    {
        /// <summary>名前</summary>
        public string name;
        /// <summary>slug</summary>
        public string slug;
        /// <summary>数</summary>
        public int size;
    }
    //-------------------------------------------------------------------------------
    #endregion (SuggestionCategoryData)
    //-------------------------------------------------------------------------------
    #region SearchMetaData 構造体：
	//-------------------------------------------------------------------------------
	/// <summary>
    /// 
    /// </summary>
    public struct SearchMetaData
    {
        public float Completed_in;
        public long Max_id;
        public string Query;
        // public string Refresh_url;
        public int Count;
        public long Since_id;
    }
    //-------------------------------------------------------------------------------
		#endregion (SearchMetaData)

    //-------------------------------------------------------------------------------
    #region EntityData 構造体
    //-------------------------------------------------------------------------------
    /// <summary>
    /// エンティティの情報を表します。
    /// </summary>
    public struct EntityData
    {
        /// <summary>アイテムの種類．nullの時はURL</summary>
        public ItemType? type;
        /// <summary>範囲</summary>
        public Range range;
        /// <summary>アイテムの情報を表す文字列</summary>
        public string str;
        /// <summary>アイテムの情報を表す文字列2</summary>
        public string str2;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public EntityData(ItemType? type, Range range, string str, string str2 = null)
        {
            this.type = type;
            this.range = range;
            this.str = str;
            this.str2 = str2;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (type.HasValue) {
                sb.Append(type.Value.ToString());
            }
            else { sb.Append("URL"); }
            sb.Append(':');
            sb.Append(str);
            sb.Append(range);
            return sb.ToString();
        }
        #endregion (+[override]ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (EntityData)
    //-------------------------------------------------------------------------------
    #region URLData 構造体
    //-------------------------------------------------------------------------------
    /// <summary>
    /// URLの短縮情報
    /// </summary>
    public struct URLData
    {
        /// <summary>短縮URL</summary>
        public string shorten_url;
        /// <summary>元URL</summary>
        public string expand_url;
    }
    //-------------------------------------------------------------------------------
    #endregion (URLData)
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
        User
    }
    //-------------------------------------------------------------------------------
    #endregion (ItemType)
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

        private int start;
        private int length;

        /// <summary>始まり</summary>
        public int Start { get { return start; } }
        /// <summary>長さ</summary>
        public int Length { get { return length; } }
        /// <summary>終わり</summary>
        public int End { get { return start + length; } }

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public Range(int start, int length)
        {
            this.start = start;
            this.length = length;
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
               ? ((value >= Start) && (value <= Start + Length))
               : ((value > Start) && (value < Start + Length));
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
            get { return Length == 0; }
        }
        #endregion (IsEmpty)

        //-------------------------------------------------------------------------------
        #region IEquatable<Range>.Equals 等価判断
        //-------------------------------------------------------------------------------
        //
        public bool Equals(Range other)
        {
            return (this.Start == other.Start && this.Length == other.Length);
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
            return (Start.GetHashCode() ^ Length.GetHashCode());
        }
        #endregion (+[override]GetHashCode)

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        //
        public override string ToString()
        {
            return string.Format("({0}, {1})", Start, End);
        }
        #endregion (+[override]ToString)

        //-------------------------------------------------------------------------------
        #region +[static]Make Rangeを作成
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 始点と終点からRangeを作成します。
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <returns></returns>
        public static Range Make(int start, int end)
        {
            return new Range(start, end - start);
        }
        #endregion (Make)
    }
    #endregion ((Class)Range)

    //-------------------------------------------------------------------------------
    #region +UserStreamItemType 列挙体
    //-------------------------------------------------------------------------------
    /// <summary>
    /// UserStreamで送られてくるデータの種類を表します。
    /// </summary>
    public enum UserStreamItemType
    {
        /// <summary>不明なデータ(object:string)</summary>
        unknown,
        /// <summary>フレンドリスト(object:IEnumerable(long))</summary>
        friendlist,
        /// <summary>発言・リツイート(object:TwitData)</summary>
        status,
        /// <summary>ダイレクトメッセージ(object:TwitData)</summary>
        directmessage,
        /// <summary>発言削除(object:long[削除された発言のID])</summary>
        status_delete,
        /// <summary>ダイレクトメッセージ削除(object:long[削除された発言のID])</summary>
        directmessage_delete,
        /// <summary>イベントデータ(object:UserStreamEventData)</summary>
        eventdata,
        /// <summary>Track Limit Notices(object:int[track])</summary>
        tracklimit,
        /// <summary>Location Deletion Notices(object:tuple(long[user_id],long[up_to_status_id]))</summary>
        location_dalelete,
        /// <summary>TODO:Unimplemented(object:null)</summary>
        status_withheld,
        /// <summary>TODO:Unimplemented(object:null)</summary>
        user_withheld,
        /// <summary>Disconnection(object:tuple(int[code],string[reason])</summary>
        disconnect
        
    }
    //-------------------------------------------------------------------------------
    #endregion (UserStreamItemType 列挙体)
    //-------------------------------------------------------------------------------
    #region +UserStreamEventType 列挙体
    //-------------------------------------------------------------------------------
    /// <summary>
    /// UserStreamで送られてくるイベントの種類です。
    /// </summary>
    public enum UserStreamEventType
    {
        /// <summary>お気に入り追加</summary>
        favorite,
        /// <summary>お気に入り削除</summary>
        unfavorite,
        /// <summary>フォロー</summary>
        follow,
        /// <summary>ブロック</summary>
        block,
        /// <summary>ブロック解除</summary>
        unblock,
        /// <summary>リストメンバー追加</summary>
        list_member_added,
        /// <summary>リストメンバー削除</summary>
        list_member_removed,
        /// <summary>リスト作成</summary>
        list_created,
        /// <summary>リスト更新</summary>
        list_updated,
        /// <summary>リスト削除</summary>
        list_destroyed,
        /// <summary>リストフォロー追加</summary>
        list_user_subscribed,
        /// <summary>リストフォロー削除</summary>
        list_user_unsubscribed,
        /// <summary>プロフィール更新</summary>
        user_update
    }
    //-------------------------------------------------------------------------------
    #endregion (UserStreamEventType 列挙体)
    //-------------------------------------------------------------------------------
    #region (class)userStreamEventData
    //-------------------------------------------------------------------------------
    /// <summary>
    /// UserStreamで送られてくるイベントに関するデータです。
    /// </summary>
    public class UserStreamEventData
    {
        /// <summary>イベントの種類</summary>
        public UserStreamEventType Type;
        /// <summary>イベント発生時間</summary>
        public DateTime Time;
        /// <summary>ターゲットユーザー情報</summary>
        public UserProfile TargetUser;
        /// <summary>ソースユーザー情報</summary>
        public UserProfile SourceUser;
        /// <summary>ターゲット発言情報(一部イベントのみ)</summary>
        public TwitData TargetTwit;
        /// <summary>ターゲットリスト情報(一部イベントのみ)</summary>
        public ListData TargetList;
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)userStreamEventData)

    //-----------------------------------------------------------------------------------
    #region (Class)TwitterAPIException
    //-------------------------------------------------------------------------------
    /// <summary>
    /// <para>TwitterクラスにおいてAPIからエラーが返された時にスローされる例外</para>
    /// <para>・ステータスコード</para>
    /// <para>-1 Unknown Error           不明なエラー(ただし以下以外のものも不明なエラーとして扱われる)</para>
    /// <para>0 Connection Failure:      接続に失敗しました。</para>
    /// <para>1 Disconnected             接続は切断されました。</para>
    /// <para></para>
    /// <para>(200 OK:                   成功(この場合は例外が投げられないはず)</para>
    /// <para>304 Not Modified:          新しい情報はない</para>
    /// <para>400 Bad Request:           API の実行回数制限に引っ掛かった、などの理由でリクエストを却下した</para>
    /// <para>401 Not Authorized:        認証失敗</para>
    /// <para>403 Forbidden:             権限がないAPI を実行しようとした(following ではない protected なユーザの情報を取得しようとした、など)</para>
    /// <para>404 Not Found:             存在しない API を実行しようとしたり、存在しないユーザを引数で指定して API を実行しようとした</para>
    /// <para>408 Timeout:               要求がタイムアウトした</para>
    /// <para>500 Internal Server Error: Twitter 側で何らかの問題が発生している</para>
    /// <para>502 Bad Gateway:           Twitter のサーバが止まっている、あるいはメンテ中</para>
    /// <para>503 Service Unavailable:   Twitter のサーバの負荷が高すぎて、リクエストを裁き切れない状態になっている</para>
    /// <para></para>
    /// <para>1000 Failure XmlLoad       取得したデータがXmlデータでない</para>
    /// <para>1001 Unexpected Xml        予期したXmlとは違う形式のXml</para>
    /// </summary>
    public class TwitterAPIException : ApplicationException
    {
        /// <summary>エラーコード</summary>
        public int ErrorStatusCode { get; private set; }

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// エラーコードとエラーの説明を用いてTwitterAPIExceptionを初期化します。
        /// </summary>
        /// <param name="errorcode">HTTPエラーコード</param>
        /// <param name="message">HTTPエラー説明</param>
        public TwitterAPIException(int errorcode, string message)
            : base(message)
        {
            ErrorStatusCode = errorcode;
        }
        #endregion (コンストラクタ)
    }
    //-------------------------------------------------------------------------------
    #endregion ((Class)TwitterAPIException)
}
