using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StarlitTwit
{
    public static class StatusFilter
    {
        //-------------------------------------------------------------------------------
        #region +[static]ThroughFilters 発言がフィルタを通るかどうか判別します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言がフィルタを通るかどうか判別します。
        /// </summary>
        /// <param name="twitdata">発言</param>
        /// <param name="filters">フィルター配列</param>
        /// <param name="friends_ids">フォローしている人のID</param>
        /// <returns></returns>
        public static bool ThroughFilters(TwitData twitdata, IEnumerable<StatusFilterInfo> filters, HashSet<long> friends_ids)
        {
            Debug.Assert(twitdata != null && filters != null && friends_ids != null);
            return filters.Where(sfi => sfi.Enabled)
                          .All(sfi => ThroughFilter(twitdata, sfi, friends_ids));
        }
        #endregion (ThroughFilters)

        //-------------------------------------------------------------------------------
        #region +[static]ThroughFilter 発言がフィルタ1つを通るかどうか判別します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 発言がフィルタを通るかどうか判別します。
        /// </summary>
        /// <param name="twitdata">発言</param>
        /// <param name="filter">フィルター</param>
        /// <param name="friends_ids">フォローしている人のID</param>
        /// <returns></returns>
        /// <remarks>通ると判断したらすぐにtrueを返していくようにする</remarks>
        public static bool ThroughFilter(TwitData twitdata, StatusFilterInfo filter, HashSet<long> friends_ids)
        {
            Debug.Assert(twitdata != null && filter != null && friends_ids != null);

            // ユーザー抽出
            switch (filter.User_FilterType) {
                case StatusFilterUserType.All:
                    break;
                case StatusFilterUserType.Following:
                    if (!friends_ids.Contains(twitdata.UserID)) { return true; }
                    break;
                case StatusFilterUserType.Unfollowing:
                    if (friends_ids.Contains(twitdata.UserID)) { return true; }
                    break;
                case StatusFilterUserType.UserList: // 通る：パターンが存在，そのいずれかに当てはまっていない
                    if (filter.User_Patterns != null
                     && filter.User_Patterns.Any(pattern => !Regex.IsMatch(twitdata.UserScreenName, pattern))) { return true; }
                    break;
                default:
                    Debug.Assert(false, "不正なフィルタ");
                    return false;
            }

            if (filter.Status_FilterType != StatusFilterStatusType.All) {
                // 発言パターン抽出
                if ((filter.Status_FilterType & StatusFilterStatusType.NormalTweet) != StatusFilterStatusType.NormalTweet
                 && twitdata.TwitType == TwitType.Normal && twitdata.Mention_UserID < 0) { return true; }
                else if ((filter.Status_FilterType & StatusFilterStatusType.ReplyTweet) != StatusFilterStatusType.ReplyTweet
                 && twitdata.TwitType == TwitType.Normal && twitdata.Mention_UserID >= 0) { return true; }
                else if ((filter.Status_FilterType & StatusFilterStatusType.Retweet) != StatusFilterStatusType.Retweet
                 && twitdata.IsRT()) { return true; }
            }

            // Text抽出
            if (filter.Status_Text_Patterns != null
             && filter.Status_Text_Patterns.Any(pattern => !Regex.IsMatch(twitdata.MainTwitData.Text, pattern))) { return true; }

            // Client抽出
            if (filter.Status_Client_Patterns != null
             && filter.Status_Client_Patterns.Any(pattern => !Regex.IsMatch(twitdata.MainTwitData.Source, pattern))) { return true; }

            return false;
        }
        #endregion (ThroughFilter)
    }

    //-------------------------------------------------------------------------------
    #region (class)StatusFilterInfo
    //-------------------------------------------------------------------------------
    /// <summary>フィルタリングに関する情報クラス</summary>
    [Serializable]
    public class StatusFilterInfo
    {
        #region Variables
        //-------------------------------------------------------------------------------
        // フィルタ名
        public string Name = "";
        /// <summary>フィルタが有効かどうか</summary>
        public bool Enabled = true;
        // 対象ユーザー情報
        /// <summary>フィルター対象ユーザー</summary>
        public StatusFilterUserType User_FilterType = StatusFilterUserType.All;
        /// <summary>
        /// <para>User_FilterType=UserListのみ</para>
        /// <para>フィルターするユーザーの正規表現</para>
        /// </summary>
        public string[] User_Patterns = null;
        // 対象発言情報
        /// <summary>フィルター対象発言種類</summary>
        public StatusFilterStatusType Status_FilterType = StatusFilterStatusType.All;
        /// <summary>フィルターするテキストのパターン</summary>
        public string[] Status_Text_Patterns = null;
        /// <summary>フィルターするクライアント名のパターン</summary>
        public string[] Status_Client_Patterns = null;
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region +DescriptionMessage フィルタリング情報の文字列を取得します。
        //-------------------------------------------------------------------------------
        //
        public string DescriptionMessage()
        {
            StringBuilder sb = new StringBuilder();

            // Name
            sb.Append("Name:");
            sb.AppendLine(Name);

            // User
            sb.Append("User:");
            sb.AppendLine(User_FilterType.ToString());
            if (User_FilterType == StatusFilterUserType.UserList) {
                sb.Append("[UserPattern:");
                if (User_Patterns != null && User_Patterns.Length > 0) {
                    sb.AppendLine();
                    foreach (string pattern in User_Patterns) {
                        sb.Append('"');
                        sb.Append(pattern);
                        sb.Append('"');
                        sb.AppendLine();
                    }
                }
                else { sb.Append("(nothing)"); }
                sb.Append(']');
                sb.AppendLine();
            }

            // Status
            sb.Append("Status:");
            sb.AppendLine(Status_FilterType.ToString());

            sb.Append("[TextPattern:");
            if (Status_Text_Patterns != null && Status_Text_Patterns.Length > 0) {
                sb.AppendLine();
                foreach (string pattern in Status_Text_Patterns) {
                    sb.Append('"');
                    sb.Append(pattern);
                    sb.Append('"');
                    sb.AppendLine();
                }
            }
            else { sb.Append("(nothing)"); }
            sb.Append(']');

            sb.Append("[ClientPattern:");
            if (Status_Client_Patterns != null && Status_Client_Patterns.Length > 0) {
                foreach (string pattern in Status_Client_Patterns) {
                    sb.Append('"');
                    sb.Append(pattern);
                    sb.Append('"');
                    sb.AppendLine();
                }
            }
            else { sb.Append("(nothing)"); }
            sb.Append(']');

            return sb.ToString();
        }
        #endregion (DescriptionMessage)

        //-------------------------------------------------------------------------------
        #region +[override]ToString 文字列へ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このインスタンスを文字列に変換します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((Enabled) ? "" : "(無効)") + Name;
        }
        #endregion (ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)StatusFilterInfo)

    //-------------------------------------------------------------------------------
    #region +StatusFilterUserType 列挙体
    //-------------------------------------------------------------------------------
    /// <summary>フィルターにおいて対象とするユーザー</summary>
    public enum StatusFilterUserType
    {
        /// <summary>全ユーザーの発言を対象</summary>
        All,
        /// <summary>フォローしているユーザーの発言</summary>
        Following,
        /// <summary>フォローしていないユーザーの発言(Replyのみ)</summary>
        Unfollowing,
        /// <summary>正規表現リストを使用して指定</summary>
        UserList
    }
    //-------------------------------------------------------------------------------
    #endregion (StatusFilterUserType 列挙体)

    //-------------------------------------------------------------------------------
    #region +StatusFilterStatusType 列挙体
    //-------------------------------------------------------------------------------
    /// <summary>フィルターにおいて対象とするステータス</summary>
    [Flags]
    public enum StatusFilterStatusType
    {
        /// <summary>なし</summary>
        None = 0,
        /// <summary>通常ツイート</summary>
        NormalTweet = 1,
        /// <summary>リプライツイート</summary>
        ReplyTweet = 2,
        /// <summary>リツイート</summary>
        Retweet = 4,
        /// <summary>全ツイート</summary>
        All = 7
    }
    #endregion (StatusFilterStatusType 列挙体)
}
