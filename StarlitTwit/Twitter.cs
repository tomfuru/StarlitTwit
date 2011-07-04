using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

/* Twitter API Resource 
 * statuses (Timeline)
 * statuses (Status)
 * users
 * local trends
 * list
 * list members
 * list subscribers
 * direct messages
 * friendships
 * friends and followers
 * account
 * favorites
 * notifications
 * blocks
 * spam reporting
 * oauth
 * geo
 * legal
 * help
 */

namespace StarlitTwit
{
    /// <summary>
    /// TwitterのAPI処理を行うためのクラス。
    /// </summary>
    public class Twitter
    {
        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        private const int API_VERSION = 1;

        private const string CONSUMER_KEY = "qvDNWLP7uX8zHsEzWiwuQ";
        private const string CONSUMER_SECRET = "Z7qNcllzRb9Iah3qfFmqUruZ0OAj5s0gdBd1zvHUs";
        private const string GET = "GET";
        private const string POST = "POST";
        private const string DELETE = "DELETE";
        public static readonly string URLapi;
        public static readonly string URLtwi;
        public static readonly string URLsearch;

        public const bool DEFAULT_INCLUDE_ENTITIES = false;

        public const string HASH_REGEX_PATTERN = @"(?<entity>(@|#[a-zA-Z_])[a-zA-Z0-9_]+?)($|[^a-zA-Z0-9_])";
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //===============================================================================
        #region メンバー
        //-------------------------------------------------------------------------------
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string ScreenName { get; set; }
        public long ID { get; set; }

        public int API_Max { get; private set; }
        public int API_Rest { get; private set; }

        private readonly string[] DATETIME_FORMATS;
        //-------------------------------------------------------------------------------
        #endregion (メンバー)

        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        static Twitter()
        {
            URLtwi = @"http://twitter.com/";
            URLapi = @"http://api.twitter.com/" + API_VERSION.ToString() + '/';
            URLsearch = @"http://search.twitter.com/";
        }
        //
        public Twitter()
        {
            API_Max = -1;
            API_Rest = -1;

            List<string> strList = new List<string>();
            foreach (string str in StarlitTwit.Properties.Settings.Default.DateTimeFormat) { strList.Add(str); }
            DATETIME_FORMATS = strList.ToArray();
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region statuses/ (Timeline関連)
        //-------------------------------------------------------------------------------
        #region +statuses_public_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/public_timelineメソッド
        /// </summary>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_public_timeline(bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = URLapi + @"statuses/public_timeline.xml";

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_public_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_home_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/home_timelineメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_home_timeline(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                 bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/home_timeline.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_home_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_friends_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/friends_timelineメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_rts">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_friends_timeline(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                    bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/friends_timeline.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_friends_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_user_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/user_timelineメソッド
        /// </summary>
        /// <param name="user_id">[option]</param>
        /// <param name="screen_name">[option]</param>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_rts">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_user_timeline(long user_id = -1, string screen_name = "", long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                           bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/user_timeline.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_user_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_mentions
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/mentionsメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_rts">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_mentions(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                            bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/mentions.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_mentions)
        //-------------------------------------------------------------------------------
        #region +statuses_retweeted_by_me
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweeted_by_meメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_retweeted_by_me(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                   bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweeted_by_me.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweeted_by_me)
        //-------------------------------------------------------------------------------
        #region +statuses_retweeted_to_me
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweeted_to_meメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_retweeted_to_me(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                   bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweeted_to_me.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweeted_to_me)
        //-------------------------------------------------------------------------------
        #region +statuses_retweets_of_me
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweets_of_meメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_retweets_of_me(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                  bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweets_of_me.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweets_of_me)
        //-------------------------------------------------------------------------------
        #endregion (statuses/ (Timeline関連))

        //-------------------------------------------------------------------------------
        #region statuses/ (Status関連)
        //-------------------------------------------------------------------------------
        #region +statuses_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/showメソッド
        /// </summary>
        /// <param name="id">取得する発言ID</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public TwitData statuses_show(bool withAuthParam, long id, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            StringBuilder sburl = new StringBuilder();
            sburl.Append(URLapi);
            sburl.Append(@"statuses/show/");
            sburl.Append(id);
            sburl.Append(".xml");
            string url;
            if (withAuthParam) {
                url = GetUrlWithOAuthParameters(sburl.ToString(), GET, paramdic);
            }
            else {
                sburl.Append(JoinParameters(paramdic));
                url = sburl.ToString();
            }

            return ConvertToTwitData(GetByAPI(url));
        }
        #endregion (statuses_show)
        //-------------------------------------------------------------------------------
        #region +statuses_update
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/updateメソッド
        /// </summary>
        public TwitData statuses_update(string status, long in_reply_to_status_id = -1, double latitude = double.NaN, double longtitude = double.NaN,
                                    string place_id = "", bool display_coordinates = false, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("status", Utilization.UrlEncode(status));

                if (in_reply_to_status_id > 0) { paramdic.Add("in_reply_to_status_id", in_reply_to_status_id.ToString()); }
                if (!double.IsNaN(latitude) && !double.IsNaN(longtitude)) {
                    paramdic.Add("lat", latitude.ToString());
                    paramdic.Add("long", longtitude.ToString());
                }
                if (!string.IsNullOrEmpty(place_id)) { paramdic.Add("place_id", place_id); }
                if (display_coordinates) { paramdic.Add("display_coordinates", display_coordinates.ToString().ToLower()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/update.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToTwitData(el);
        }
        #endregion (statuses_update)
        //-------------------------------------------------------------------------------
        #region +statuses_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/destroyメソッド
        /// </summary>
        /// <param name="id">削除するID</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public TwitData statuses_destroy(long id, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/destroy/" + id.ToString() + ".xml", POST, paramdic);
            return ConvertToTwitData(PostToAPI(url));
        }
        #endregion (statuses_destroy)
        //-------------------------------------------------------------------------------
        #region +statuses_retweet
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweetメソッド
        /// </summary>
        /// <param name="id">リツイート対象の発言ID</param>
        /// <remarks>403:update limit</remarks>
        public void statuses_retweet(long id)
        {
            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweet/" + id.ToString() + ".xml", POST);
            TwitData d = ConvertToTwitData(PostToAPI(url));
        }
        #endregion (statuses_retweet)
        //-------------------------------------------------------------------------------
        #region +statuses_retweets
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweetsメソッド
        /// </summary>
        /// <param name="id">リツイートを見る発言のID</param>
        /// <param name="count">[option] &lt;100</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_retweets(long id, int count = -1, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}statuses/retweets/{1}.xml", URLapi, id), GET, paramdic);
            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweets)
        //-------------------------------------------------------------------------------
        #region +statuses_id_retweeted_by
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/id/retweeted_byメソッド
        /// </summary>
        public IEnumerable<UserProfile> statuses_id_retweeted_by(long id, int count = -1, int page = -1, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}statuses/{1}/retweeted_by.xml", URLapi, id), GET, paramdic);
            return ConvertToUserProfileArray(GetByAPI(url));
        }
        #endregion (statuses_id_retweeted_by)
        //-------------------------------------------------------------------------------
        #region +statuses_id_retweeted_by_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/id/retweeted_by/idsメソッド 
        /// </summary>
        public IEnumerable<long> statuses_id_retweeted_by_ids(long id, int count = -1, int page = -1, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}statuses/{1}/retweeted_by/ids.xml", URLapi, id), GET, paramdic);
            XElement el = GetByAPI(url);

            var ids = from elem in el.Elements("id")
                      select long.Parse(elem.Value);
            return ids;
        }
        #endregion (statuses_id_retweeted_by_ids)
        //-------------------------------------------------------------------------------
        #endregion (statuses/ (Status関連))

        //-------------------------------------------------------------------------------
        #region users/ (ユーザー情報関連)
        //-------------------------------------------------------------------------------
        #region users_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// users/show メソッド
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile users_show(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"users/show.xml", GET, paramdic);
            return ConvertToUserProfile(GetByAPI(url));
        }
        //-------------------------------------------------------------------------------
        #endregion (users_show)
        //-------------------------------------------------------------------------------
        #region users_lookup
        //-------------------------------------------------------------------------------
        /// <summary>
        /// users/lookup メソッド
        /// </summary>
        /// <param name="user_ids">[select]</param>
        /// <param name="screen_names">[select]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public IEnumerable<UserProfile> users_lookup(long[] user_ids = null, string[] screen_names = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if ((user_ids == null || user_ids.Length == 0) && (screen_names == null || screen_names.Length == 0)) {
                throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if ((user_ids != null && user_ids.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_ids)); }
                if ((screen_names != null && screen_names.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_names)); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"users/lookup.xml", POST, paramdic);
            return ConvertToUserProfileArray(PostToAPI(url));
        }
        #endregion (users_lookup)
        //-------------------------------------------------------------------------------
        #region users_profile_image
        //-------------------------------------------------------------------------------
        #region EImageSize 列挙体
        //-------------------------------------------------------------------------------
        /// <summary>users/profile_imageメソッドで使用する画像サイズ</summary>
        public enum EImageSize
        {
            /// <summary>73*73</summary>
            bigger,
            /// <summary>48*48</summary>
            normal,
            /// <summary>24*24</summary>
            mini
        }
        //-------------------------------------------------------------------------------
        #endregion (EImageSize )
        /// <summary>
        /// users/profile_image メソッド
        /// </summary>
        /// <param name="screen_name"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Tuple<string, Image> users_profile_image(string screen_name, EImageSize size = EImageSize.normal)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("size", size.ToString());
            }

            string url = string.Format("{0}users/profile_image/{1}.xml?{2}", URLapi, screen_name, JoinParameters(paramdic));

            return GetByAPIImage(url);
        }
        #endregion (users_profile_image)
        //-------------------------------------------------------------------------------
        #region statuses_friends
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォローしている人を返します。返り値：(ユーザーリスト，next_cursor, previous_cursor）
        /// </summary>
        /// <param name="user_id">[option]</param>
        /// <param name="screen_name">[option]</param>
        /// <param name="cursor">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public SequentData<UserProfile> statuses_friends(long user_id = -1, string screen_name = null, long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id >= 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/friends.xml", GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (statuses_friends)
        //-------------------------------------------------------------------------------
        #region statuses_followers
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォローされている人を返します。返り値：(ユーザーリスト，next_cursor, previous_cursor）
        /// </summary>
        /// <param name="user_id">[option]</param>
        /// <param name="screen_name">[option]</param>
        /// <param name="cursor">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public SequentData<UserProfile> statuses_followers(long user_id = -1, string screen_name = null, long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id >= 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/followers.xml", GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (statuses_followers)
        //-------------------------------------------------------------------------------
        // suggestions
        // suggestions/slug
        //-------------------------------------------------------------------------------
        #endregion (users/)

        //-------------------------------------------------------------------------------
        #region list/ (リスト関連)
        //-------------------------------------------------------------------------------
        #region lists_Create リスト作成
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト作成メソッド
        /// </summary>
        /// <param name="name">リストの名前</param>
        /// <param name="isPrivate">[option]privateにする時にtrue</param>
        /// <param name="description">[option]リストの説明</param>
        /// <returns></returns>
        public object lists_Create(string name, bool isPrivate = false, string description = null)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("name", Utilization.UrlEncode(name));
                if (isPrivate) { paramdic.Add("mode", "private"); }
                if (!string.IsNullOrEmpty(description)) { paramdic.Add("description", Utilization.UrlEncode(description)); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + ScreenName + @"/lists.xml", POST, paramdic);
            return ConvertToListData(PostToAPI(url));
        }
        #endregion (lists_Create)
        //-------------------------------------------------------------------------------
        #region lists_Update リスト更新
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト更新
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="name">[option]リストの新しい名前</param>
        /// <param name="isPrivate">[option]privateにする時にtrue</param>
        /// <param name="description">[option]リストの説明</param>
        /// <returns>変更前が返る？</returns>
        public object lists_Update(string list_id, string name = null, bool isPrivate = false, string description = null)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(name)) { paramdic.Add("name", Utilization.UrlEncode(name)); }
                paramdic.Add("mode", (isPrivate) ? "private" : "public");
                if (!string.IsNullOrEmpty(description)) { paramdic.Add("description", Utilization.UrlEncode(description)); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/{2}.xml", URLapi, ScreenName, list_id), POST, paramdic);
            return ConvertToListData(PostToAPI(url));
        }
        #endregion (lists_Update)
        //-------------------------------------------------------------------------------
        #region lists_Get リストのリスト取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リストのリストを取得します。返り値：(リストのリスト，next_cursor, previous_cursor）
        /// </summary>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="cursor">[option]データベース上のカーソル</param>
        /// <returns></returns>
        public SequentData<ListData> lists_Get(string screen_name = "", long cursor = -1)
        {
            if (string.IsNullOrEmpty(screen_name) && string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(URLapi + user + @"/lists.xml", GET, paramdic);

            XElement el = GetByAPI(url);

            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_Get)
        //-------------------------------------------------------------------------------
        #region lists_Show リスト情報取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト情報取得
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <returns></returns>
        public object lists_Show(string list_id, string screen_name = null)
        {
            if (string.IsNullOrEmpty(screen_name) && string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }
            string scrname = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/{2}.xml", URLapi, ScreenName, list_id), GET);
            return ConvertToListData(GetByAPI(url));
        }
        #endregion (lists_Show)
        //-------------------------------------------------------------------------------
        #region lists_Delete リスト削除
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト削除
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <returns></returns>
        public ListData lists_Delete(string list_id)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/{2}.xml", URLapi, ScreenName, list_id), DELETE);

            return ConvertToListData(DeleteToAPI(url));
        }
        #endregion (lists_Delete)
        //-------------------------------------------------------------------------------
        #region lists_statuses リスト発言取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists/statuses リストの発言取得
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="per_page">[option]</param>
        /// <param name="page">[option]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> lists_statuses(string list_id, string screen_name = "", long since_id = -1, long max_id = -1, int per_page = -1, int page = -1)
        {
            if (string.IsNullOrEmpty(screen_name) && string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (per_page > 0) { paramdic.Add("per_page", per_page.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
            }

            string scrname = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/{2}/statuses.xml", URLapi, scrname, list_id), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToTwitDataArray(el);
        }
        #endregion (lists_statuses)
        //-------------------------------------------------------------------------------
        #region lists_memberships
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists_membershipsメソッド
        /// </summary>
        /// <param name="screen_name">追加されているリストを調べるユーザー名</param>
        public SequentData<ListData> lists_memberships(string screen_name = "", long cursor = -1)
        {
            if (string.IsNullOrEmpty(screen_name) && string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/memberships.xml", URLapi, user), GET, paramdic);
            XElement el = GetByAPI(url);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")),
                long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_memberships)
        //-------------------------------------------------------------------------------
        #region lists_subscriptions
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists_subscriptionsメソッド
        /// </summary>
        /// <param name="screen_name">フォローしているリストを調べるユーザー名</param>
        public SequentData<ListData> lists_subscriptions(string screen_name = "", long cursor = -1)
        {
            if (string.IsNullOrEmpty(screen_name) && string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format(@"{0}{1}/lists/subscriptions.xml", URLapi, user), GET, paramdic);
            XElement el = GetByAPI(url);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")),
                long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_subscriptions)
        //-------------------------------------------------------------------------------
        #endregion (list)

        //-------------------------------------------------------------------------------
        #region list members/ (リスト所属ユーザー関連)
        //-------------------------------------------------------------------------------
        #region list_members_Get
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/members メソッド</para> 
        /// <para>リストメンバー取得</para>
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="cursor">データベース上のカーソル</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public SequentData<UserProfile> list_members_Get(string list_id, string screen_name = "", long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/members.xml", URLapi, user, list_id), GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")),
                    long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (list_members_Get)
        //-------------------------------------------------------------------------------
        #region list_members_Add
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/member メソッド</para>
        /// <para>リストメンバー追加</para>
        /// </summary>
        /// <param name="id">追加するユーザーのID(の文字列)かScreenName</param>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <returns></returns>
        public ListData list_members_Add(string id, string list_id)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id);
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/members.xml", URLapi, ScreenName, list_id), POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_members_Add)
        //-------------------------------------------------------------------------------
        #region list_create_all (使用不可能)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/create_all メソッド</para>
        /// <para>リストメンバー一斉追加</para>
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="user_ids">[select]</param>
        /// <param name="screen_names">[select]</param>
        /// <returns></returns>
        private object list_create_all(string list_id, long[] user_ids = null, string[] screen_names = null)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            if ((user_ids == null || user_ids.Length == 0) && (screen_names == null || screen_names.Length == 0)) {
                throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if ((user_ids != null && user_ids.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_ids)); }
                if ((screen_names != null && screen_names.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_names)); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/create_all.xml", URLapi, ScreenName, list_id), POST, paramdic);

            XElement el = PostToAPI(url);
            throw new NotImplementedException();
        }
        #endregion (list_create_all)
        //-------------------------------------------------------------------------------
        #region list_members_Delete
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/member メソッド</para>
        /// <para>リストメンバー削除</para>
        /// </summary>
        /// <param name="id">削除するユーザーのID(の文字列)かScreenName</param>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <returns></returns>
        public ListData list_members_Delete(string id, string list_id)
        {
            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id);
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/members.xml", URLapi, ScreenName, list_id), DELETE, paramdic);

            XElement el = DeleteToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_members_Delete)
        //-------------------------------------------------------------------------------
        #region list_members_Check
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/member メソッド</para>
        /// <para>リストメンバー所属確認</para>
        /// <para>見つからなければ404エラーを返す</para>
        /// </summary>
        /// <param name="user_id">確認するユーザーのID(の文字列)かScreenName</param>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public object list_members_Check(string user_id, string list_id, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            
            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/members/{3}.xml", URLapi, user, list_id, user_id), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (list_members_Check)
        //-------------------------------------------------------------------------------
        #endregion (list members/ (リスト所属ユーザー関連))

        //-------------------------------------------------------------------------------
        #region list subscribers/ (リストフォローユーザー関連)
        //-------------------------------------------------------------------------------
        #region list_subscribers_Get
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers メソッド</para> 
        /// <para>リストフォローメンバー取得</para>
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="cursor">[option]データベース上のカーソル</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public SequentData<UserProfile> list_subscribers_Get(string list_id, string screen_name = "", long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/subscribers.xml", URLapi, user, list_id), GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")),
                    long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (list_subscribers_Get)
        //-------------------------------------------------------------------------------
        #region list_subscribers_Follow
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers メソッド</para>
        /// <para>リストをフォロー</para>
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <returns></returns>
        public ListData list_subscribers_Follow(string list_id, string screen_name = "")
        {
            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/subscribers.xml",URLapi,user,list_id), POST);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_subscribers_Follow)
        //-------------------------------------------------------------------------------
        #region list_subscribers_Unfollow
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers メソッド</para>
        /// <para>リストをフォロー解除</para>
        /// </summary>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <returns></returns>
        public ListData list_subscribers_Unfollow(string list_id, string screen_name = "")
        {
            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/subscribers.xml", URLapi, user, list_id), DELETE);

            XElement el = DeleteToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_subscribers_Unfollow)
        //-------------------------------------------------------------------------------
        #region list_subscribers_Check
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers メソッド</para>
        /// <para>リストフォロー確認</para>
        /// <para>見つからなければ404エラーを返す</para>
        /// </summary>
        /// <param name="id">確認するユーザーのID(の文字列)かScreenName</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="list_id">リストのID(の文字列)かslug</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile list_subscribers_Check(string user_id, string list_id, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string user = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            string url = GetUrlWithOAuthParameters(string.Format("{0}{1}/{2}/members/{3}.xml", URLapi, user, list_id, user_id), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (list_subscribers_Check)
        //-------------------------------------------------------------------------------
        #endregion (list subscribers/ (リストフォローユーザー関連))

        //-------------------------------------------------------------------------------
        #region direct_messages/ (ダイレクトメッセージ関連)
        //-------------------------------------------------------------------------------
        #region direct_messages
        //-------------------------------------------------------------------------------
        /// <summary>
        /// direct_messagesメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> direct_messages(long since_id = -1, long max_id = -1, int count = -1, int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"direct_messages.xml", GET, paramdic);

            return ConvertToTwitDataArrayDM(GetByAPI(url));
        }
        #endregion (direct_messages)
        //-------------------------------------------------------------------------------
        #region direct_messages_sent
        //-------------------------------------------------------------------------------
        /// <summary>
        /// direct_messages/sentメソッド
        /// </summary>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="count">[option]</param>
        /// <param name="page">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> direct_messages_sent(long since_id = -1, long max_id = -1, int count = -1, int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"direct_messages/sent.xml", GET, paramdic);

            return ConvertToTwitDataArrayDM(GetByAPI(url));
        }
        #endregion (direct_messages_sent)
        //-------------------------------------------------------------------------------
        #region direct_messages_new
        //-------------------------------------------------------------------------------
        /// <summary>
        /// direct_messages/newメソッド
        /// </summary>
        /// <param name="screen_name">送信先の名前</param>
        /// <param name="user_id">送信先のユーザーID</param>
        /// <param name="text">送信テキスト</param>
        /// <param name="include_entities">[option]</param>
        public TwitData direct_messages_new(string screen_name, long user_id, string text, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("screen_name", screen_name);
                paramdic.Add("user_id", user_id.ToString());
                paramdic.Add("text", Utilization.UrlEncode(text));

                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"direct_messages/new.xml", POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToTwitDataDM(el);
        }
        #endregion (direct_messages_new)
        //-------------------------------------------------------------------------------
        #region direct_messages_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// direct_messages/destroyメソッド
        /// </summary>
        /// <param name="id">削除先発言ID</param>
        /// <param name="include_entities">[option]</param>
        public TwitData direct_messages_destroy(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"direct_messages/destroy/" + id.ToString() + ".xml", POST, paramdic);
            return ConvertToTwitDataDM(PostToAPI(url));
        }
        #endregion (direct_messages_destroy)
        //-------------------------------------------------------------------------------
        #endregion (direct_messages/)

        //-------------------------------------------------------------------------------
        #region friendships/ (フレンド関連)
        //-------------------------------------------------------------------------------
        #region friendships_create フォロー
        //-------------------------------------------------------------------------------
        /// <summary>
        /// friendships/create メソッド
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="follow">通知するか[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile friendships_create(long user_id = -1, string screen_name = null, bool follow = true, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (follow) { paramdic.Add("follow", follow.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"friendships/create.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (friendships_create)
        //-------------------------------------------------------------------------------
        #region friendships_destroy フォロー解除
        //-------------------------------------------------------------------------------
        /// <summary>
        /// friendships/destroy メソッド
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile friendships_destroy(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"friendships/destroy.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (friendships_destroy)
        //-------------------------------------------------------------------------------
        #region friendships_exists フォロー有無
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォロー有無を取得します。return [ユーザーA follows ユーザーB]?
        /// </summary>
        /// <param name="withAuth">認証を含めるか</param>
        /// <param name="user_a">ユーザーA</param>
        /// <param name="user_b">ユーザーB</param>
        /// <returns>ユーザーA follows ユーザーB?</returns>
        public bool friendships_exists(bool withAuth, string user_a, string user_b)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("user_a", user_a);
                paramdic.Add("user_b", user_b);
            }

            string urlbase = URLapi + @"friendships/exists.xml?";
            string url = (withAuth) ? GetUrlWithOAuthParameters(urlbase, GET, paramdic)
                                    : urlbase + '?' + JoinParameters(paramdic);
            XElement el = GetByAPI(url);

            return bool.Parse(el.Value);
        }
        #endregion (friendships_exists)
        //-------------------------------------------------------------------------------
        #region friendships_show 2ユーザー間の情報確認
        //-------------------------------------------------------------------------------
        /// <summary>
        /// friendships/show メソッド
        /// </summary>
        /// <param name="source_id">[option:1] subject user</param>
        /// <param name="source_screen_name">[option:1] subject user</param>
        /// <param name="target_id">[option:2] target user</param>
        /// <param name="target_screen_name">[option:2] target user</param>
        /// <returns></returns>
        public object friendships_show(long source_id = -1, string source_screen_name = null,
                                       long target_id = -1, string target_screen_name = null)
        {
            if (source_id == -1 && string.IsNullOrEmpty(source_screen_name)) { throw new ArgumentException("対象ユーザー：ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            if (target_id == -1 && string.IsNullOrEmpty(target_screen_name)) { throw new ArgumentException("ターゲットユーザー：ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (source_id != -1) { paramdic.Add("source_id", source_id.ToString()); }
                if (!string.IsNullOrEmpty(source_screen_name)) { paramdic.Add("source_screen_name", source_screen_name); }
                if (target_id != -1) { paramdic.Add("target_id", target_id.ToString()); }
                if (!string.IsNullOrEmpty(target_screen_name)) { paramdic.Add("target_screen_name", target_screen_name); }
            }

            string urlbase = URLapi + @"friendships/show.xml";
            string url = urlbase + '?' + JoinParameters(paramdic);

            XElement el = GetByAPI(url);

            return ConvertToRelationshipData(el);
        }
        #endregion (friendships_show)
        //-------------------------------------------------------------------------------
        #endregion (friendships/ (フレンド関連))

        //-------------------------------------------------------------------------------
        #region friends and followers/ (フォロー・フォロワー関係)
        //-------------------------------------------------------------------------------
        #region friends_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friends/ids メソッド</para>
        /// <para>指定ユーザーがフォローしているユーザー</para>
        /// </summary>
        /// <returns></returns>
        public object friends_ids(bool withAuthParam, long user_id = -1, string screen_name = null, long cursor = -1)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                paramdic.Add("cursor", cursor.ToString());
            }

            string urlbase = URLapi + @"friends/ids.xml";
            string url = (withAuthParam) ? GetUrlWithOAuthParameters(urlbase, GET, paramdic)
                                         : urlbase + '?' + JoinParameters(paramdic);

            XElement el = GetByAPI(url, withAuthParam);

            var ids = from id in el.Element("ids").Elements("id")
                      select long.Parse(id.Value);

            return new SequentData<long>(ids,
               long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (friends_ids)
        //-------------------------------------------------------------------------------
        #region followers_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>followers/ids メソッド</para>
        /// <para>指定ユーザーをフォローしているユーザー</para>
        /// </summary>
        /// <returns></returns>
        public object followers_ids(bool withAuthParam, long user_id = -1, string screen_name = null, long cursor = -1)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                paramdic.Add("cursor", cursor.ToString());
            }

            string urlbase = URLapi + @"followers/ids.xml";
            string url = (withAuthParam) ? GetUrlWithOAuthParameters(urlbase, GET, paramdic)
                                         : urlbase + '?' + JoinParameters(paramdic);

            XElement el = GetByAPI(url, withAuthParam);

            var ids = from id in el.Element("ids").Elements("id")
                      select long.Parse(id.Value);

            return new SequentData<long>(ids,
               long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (followers_ids)
        //-------------------------------------------------------------------------------
        #endregion (friends and followers/ (フォロー・フォロワー関係))

        //-------------------------------------------------------------------------------
        #region account/ (アカウント関連)
        //-------------------------------------------------------------------------------
        #region account_rate_limit_status
        //-------------------------------------------------------------------------------
        /// <summary>
        /// abbount/rate_limit_statusメソッド
        /// </summary>
        /// <param name="withAuth">認証を行うかどうか。認証しない場合はIP依存のデータが返る</param>
        /// <returns>残数データ</returns>
        public APILimitData account_rate_limit_status(bool withAuth)
        {
            string urlbase = URLapi + @"account/rate_limit_status.xml";
            string url = (withAuth) ? GetUrlWithOAuthParameters(urlbase, GET)
                                    : urlbase;

            XElement el = GetByAPI(url);
            return ConvertToAPILimitData(el);
        }
        #endregion (account_rate_limit_status)
        //-------------------------------------------------------------------------------
        #region account_update_profile プロフィール更新
        //-------------------------------------------------------------------------------
        /// <summary>
        /// account_update_profileメソッド
        /// </summary>
        /// <param name="name">[option]</param>
        /// <param name="url">[option]</param>
        /// <param name="location">[option]</param>
        /// <param name="description">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile account_update_profile(string name = null, string url = null, string location = null, string description = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(url)
             && string.IsNullOrEmpty(location) && string.IsNullOrEmpty(description)) { throw new ArgumentException("更新内容が少なくとも1つ必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(name)) { paramdic.Add("name", Utilization.UrlEncode(name)); }
                if (!string.IsNullOrEmpty(url)) { paramdic.Add("url", Utilization.UrlEncode(url)); }
                if (!string.IsNullOrEmpty(location)) { paramdic.Add("location", Utilization.UrlEncode(location)); }
                if (!string.IsNullOrEmpty(description)) { paramdic.Add("description", Utilization.UrlEncode(description)); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_post = GetUrlWithOAuthParameters(URLapi + @"account/update_profile.xml", POST, paramdic);

            XElement el = PostToAPI(url_post);
            return ConvertToUserProfile(el);
        }
        #endregion (account_update_profile)
        //-------------------------------------------------------------------------------
        #region account_update_profile_image 画像更新
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>account/update_profile_image メソッド</para>　
        /// <para>返り値のUserProfileではURLが反映されてない可能性があるので，最低5秒待ってから取得する。</para>
        /// </summary>
        /// <param name="imgFileName">画像ファイルパス</param>
        /// <param name="image">画像</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile account_update_profile_image(string imgFileName, Image image, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            string contentType;
            Guid guid = image.RawFormat.Guid;
            if (guid.Equals(ImageFormat.Jpeg.Guid)) { contentType = "jpeg"; }
            else if (guid.Equals(ImageFormat.Png)) { contentType = "png"; }
            else if (guid.Equals(ImageFormat.Gif)) { contentType = "gif"; }
            else { throw new InvalidOperationException("画像がjpg,png,gif以外のフォーマットです"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"account/update_profile_image.xml", POST, paramdic);

            return ConvertToUserProfile(PostImageToAPI(url, imgFileName, image, contentType));
        }
        #endregion (account_update_profile_image)

        //-------------------------------------------------------------------------------
        #endregion (account/ (アカウント関連))

        //-------------------------------------------------------------------------------
        #region favorites/ (お気に入り関連)
        //-------------------------------------------------------------------------------
        #region favorites_get お気に入り取得（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// favorites_get お気に入り取得（未実装）
        /// </summary>
        /// <returns></returns>
        private object favorites_get()
        {

            //string url = GetUrlWithOAuthParameters(URL + @"lists.xml", POST, paramdic);

            //XElement el = PostToAPI(url);
            //return ConvertToTwitDataDM(el);

            throw new NotImplementedException();
        }
        #endregion (favorites_get)
        //-------------------------------------------------------------------------------
        #region favorites_create お気に入り設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// favorites_create お気に入り設定
        /// </summary>
        /// <returns></returns>
        public TwitData favorites_create(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"favorites/create/" + id.ToString() + ".xml", POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToTwitData(el);
        }
        #endregion (favorites_create)
        //-------------------------------------------------------------------------------
        #region favorites_destroy お気に入り削除
        //-------------------------------------------------------------------------------
        /// <summary>
        /// favorites_destroy お気に入り削除）
        /// </summary>
        /// <returns></returns>
        public TwitData favorites_destroy(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"favorites/destroy/" + id.ToString() + ".xml", POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToTwitData(el);
        }
        #endregion (favorites_destroy)
        //-------------------------------------------------------------------------------
        #endregion (favorites/)

        //-------------------------------------------------------------------------------
        #region blocks/ (ブロック関連）
        //-------------------------------------------------------------------------------
        #region blocks_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// blocks/create メソッド
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile blocks_create(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/create.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (blocks_create)
        //-------------------------------------------------------------------------------
        #region blocks_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// blocks/destroy メソッド
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile blocks_destroy(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/destroy.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (blocks_destroy)
        //-------------------------------------------------------------------------------
        #region blocks_exists (未実装)
        //-------------------------------------------------------------------------------
        //
        private void blocks_exists()
        {
            throw new NotImplementedException();
        }
        #endregion (blocks_exists)
        //-------------------------------------------------------------------------------
        #region blocks_blocking
        //-------------------------------------------------------------------------------
        /// <summary>
        /// blocks/blocking メソッド
        /// </summary>
        /// <param name="page"></param>
        /// <param name="include_entities"></param>
        /// <returns></returns>
        public IEnumerable<UserProfile> blocks_blocking(int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/blocking.xml", GET, paramdic);
            return ConvertToUserProfileArray(GetByAPI(url, true));
        }
        #endregion (blocks_blocking)
        //-------------------------------------------------------------------------------
        #region blocks_blocking_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// blocks/blocking/ids メソッド
        /// </summary>
        /// <returns></returns>
        public IEnumerable<long> blocks_blocking_ids()
        {
            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/blocking/ids.xml", GET);
            XElement el = GetByAPI(url, true);

            var ids = from id in el.Elements("id")
                      select long.Parse(id.Value);
            return ids;
        }
        #endregion (blocks_blocking_ids)
        //-------------------------------------------------------------------------------
        #endregion (blocks/ (ブロック関連）)

        //-------------------------------------------------------------------------------
        #region search/ (検索関連)
        //-------------------------------------------------------------------------------
        #region search
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>検索を行います。引数qかphraseのどちらかは必須です。</para>
        /// </summary>
        /// <param name="q">[select option]"検索条件"</param>
        /// <param name="phrase">[select option]"(検索語句)&(検索条件)"</param>
        /// <param name="lang">[unuse]</param>
        /// <param name="rpp">[option] &lt;=100</param>
        /// <param name="page">[option] rpp * page &lt;=1500</param>
        /// <param name="max_id">[option]</param>
        /// <param name="since_id">[option]</param>
        /// <param name="since">[option]YYYY-MM-DD</param>
        /// <param name="until">[option]YYYY-MM-DD</param>
        /// <param name="geocode">[unuse]</param>
        /// <param name="show_user"></param>
        /// <param name="result_type">[recent/popular/mixed]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> search(string q = "", string phrase = "", string lang = "jp",
            int rpp = -1, int page = -1, long max_id = -1, long since_id = -1, string since = "",
            string until = "", object geocode = null, bool show_user = false, string result_type = "")
        {
            if (string.IsNullOrEmpty(q) && string.IsNullOrEmpty(phrase)) { throw new ArgumentException("qかphraseのどちらかの引数は必須です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(q)) { paramdic.Add("q", Utilization.UrlEncode(q)); }
                if (!string.IsNullOrEmpty(phrase)) { paramdic.Add("phrase", Utilization.UrlEncode(phrase)); }
                //paramdic.Add("lang", lang);
                if (rpp > 0) { paramdic.Add("rpp", rpp.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (!string.IsNullOrEmpty(since)) { paramdic.Add("since", since); }
                if (!string.IsNullOrEmpty(until)) { paramdic.Add("until", until); }
                // geocode
                if (show_user) { paramdic.Add("show_user", "true"); }
                if (!string.IsNullOrEmpty(result_type)) { paramdic.Add("result_type", result_type); }
            }

            string url = GetUrlWithOAuthParameters(URLsearch + "search.json", GET, paramdic);

            XElement el = GetByAPIJson(url);

            IEnumerable<TwitData> data = ConvertToTwitDataJson(el);
            //string[] user_names = data
            //    .Where((tdata) => !tdata.UserProtected)
            //    .Select((tdata) => tdata.UserScreenName)
            //    .Distinct()
            //    .ToArray();

            //UserData[] userData = users_lookup(screen_names: user_names);

            return data;
        }
        #endregion (search)
        //-------------------------------------------------------------------------------
        #endregion (search)

        //-------------------------------------------------------------------------------
        #region oauth/ (OAuth認証関連)
        //-------------------------------------------------------------------------------
        #region OAuth OAuth認証
        //-------------------------------------------------------------------------------
        //
        public bool OAuth(out UserInfo userdata)
        {
            try {
                string req_token, req_token_secret;

                req_token = oauth_request_token(out req_token_secret);

                string authURL = oauth_authorize_URL(req_token);

                string pin = null;
                // フォーム表示
                using (FrmAuthWebBrowser frmweb = new FrmAuthWebBrowser()) {
                    frmweb.SetURL(authURL);

                    if (frmweb.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        pin = frmweb.PIN;
                    }
                }

                if (pin == null) { userdata = new UserInfo(); return false; }

                userdata = oauth_access_token(pin, req_token, req_token_secret);
            }
            catch (WebException) {
                userdata = new UserInfo();
                return false;
            }
            return true;
        }
        //-------------------------------------------------------------------------------
        #endregion (OAuth)
        //-------------------------------------------------------------------------------
        #region oauth_request_token
        //-------------------------------------------------------------------------------
        /// <summary>
        /// request_tokenを返します。
        /// </summary>
        /// <param name="request_token_Secret"></param>
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        /// <returns></returns>
        private string oauth_request_token(out string request_token_Secret)
        {
            string url = URLtwi + "oauth/request_token";

            SortedDictionary<string, string> parameters = GenerateParameters("");
            string signature = GenerateSignature("", GET, url, parameters);
            parameters.Add("oauth_signature", Utilization.UrlEncode(signature));
            string response = HttpGet(url, parameters);
            Dictionary<string, string> dic = ParseResponse(response);

            request_token_Secret = dic["oauth_token_secret"];
            return dic["oauth_token"];
        }
        //-------------------------------------------------------------------------------
        #endregion (oauth_request_token)
        //-------------------------------------------------------------------------------
        #region oauth_authorize_URL
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザーが認証するためのURLを返します。
        /// </summary>
        /// <param name="strRequestToken"></param>
        /// <returns></returns>
        private string oauth_authorize_URL(string strRequestToken)
        {
            return URLtwi + "oauth/authorize?oauth_token=" + strRequestToken;
        }
        //-------------------------------------------------------------------------------
        #endregion (oauth_authorize)
        //-------------------------------------------------------------------------------
        #region oauth_access_token
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 最終認証を行います。正式なoauth_tokenが返ります。
        /// </summary>
        /// <param name="pin">ユーザーが貰ったキー</param>
        /// <param name="reqToken">request_token</param>
        /// <param name="reqTokenSecret">request_token_secret</param>
        /// <param name="access_token_secret">正式なoauth_token_secret</param>
        private UserInfo oauth_access_token(string pin, string reqToken, string reqTokenSecret)
        {
            string url = URLtwi + "oauth/access_token";

            SortedDictionary<string, string> parameters = GenerateParameters(reqToken);
            parameters.Add("oauth_verifier", pin);
            string signature = GenerateSignature(reqTokenSecret, GET, url, parameters);
            parameters.Add("oauth_signature", Utilization.UrlEncode(signature));
            string response = HttpGet(url, parameters);
            Dictionary<string, string> dic = ParseResponse(response);

            UserInfo userdata = new UserInfo() {
                AccessToken = dic["oauth_token"],
                AccessTokenSecret = dic["oauth_token_secret"],
                ID = long.Parse(dic["user_id"]),
                ScreenName = dic["screen_name"]
            };
            return userdata;

        }
        //-------------------------------------------------------------------------------
        #endregion (oauth_access_token)
        //-------------------------------------------------------------------------------
        #endregion (oauth/)

        //===============================================================================
        #region stream_statuses_sample (test)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テスト
        /// </summary>
        public CancellationTokenSource stream_statuses_sample(Action<string> action)
        {
            const string URL_SAMPLE = @"http://stream.twitter.com/1/statuses/sample.json";
            string url = GetUrlWithOAuthParameters(URL_SAMPLE, GET, null);

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            Utilization.InvokeTransaction(() =>
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse res = req.GetResponse();

                using (Stream stream = res.GetResponseStream())
                using (StreamReader sr = new StreamReader(stream)) {
                    while (!sr.EndOfStream) {
                        if (token.IsCancellationRequested) {
                            string str = sr.ReadToEnd();
                            res.Close();
                            break;
                        }
                        string line = sr.ReadLine();
                        action(line);
                    }
                }
            });

            return cts;
        }
        //-------------------------------------------------------------------------------
        #endregion (stream_statuses_sample)
        //-------------------------------------------------------------------------------
        #region userstream_user
        //-------------------------------------------------------------------------------
        //
        public CancellationTokenSource userstream_user(bool all_replies, Action<UserStreamItemType, object> action, Action endact = null)
        {
            const string URL_SAMPLE = @"https://userstream.twitter.com/2/user.json";
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (all_replies) { paramdic.Add("replies", "all"); }
            }
            string url = GetUrlWithOAuthParameters(URL_SAMPLE, GET, paramdic);

            CancellationTokenSource cts = new CancellationTokenSource(); // Cancelのためのオブジェクト
            CancellationToken token = cts.Token;

            ThreadStart ReadStreaming = () =>
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); // User-Agent?
                req.ReadWriteTimeout = 90000;

                Encoding enc = Encoding.UTF8;

                HttpWebResponse res;
                try {
                    res = (HttpWebResponse)req.GetResponse();
                    byte[] b = new byte[0x4000];            // 16KBの受信バッファ
                    StringBuilder sb = new StringBuilder(); // 受信文字列バッファ

                    const string NEWLINE = "\r\n";
                    // データ受信時コールバック
                    bool canceled = false;
                    AsyncCallback callback = null;
                    callback = ar =>
                    {
                        Stream stream = (Stream)ar.AsyncState;
                        try {
                            int len = stream.EndRead(ar);
                            if (len > 0) {
                                sb.Append(enc.GetString(b, 0, len));
                                stream.BeginRead(b, 0, b.Length, callback, stream);
                                // 受信文字列解析
                                while (true) {
                                    string str = sb.ToString();
                                    int newline = str.IndexOf(NEWLINE);
                                    if (newline == 0) {
                                        sb.Remove(0, NEWLINE.Length);
                                    }
                                    else if (newline > 0) {
                                        string line = sb.ToString(0, newline);
                                        sb.Remove(0, newline + NEWLINE.Length);
                                        var item = ConvertToStreamItem(JsonToXElement(line));　// XElement取得
                                        action(item.Item1, item.Item2); // イベント
                                    }
                                    else { break; }
                                }
                            }
                            else { canceled = true; }　// 接続きれた
                        }
                        catch (WebException ex) {
                            if (ex.Status == WebExceptionStatus.RequestCanceled) { return; } // キャンセルした時
                            //Message.ShowInfoMessage("WebException");
                            canceled = true;// TODO:切断された時
                        }
                        catch (IOException) {
                            //Message.ShowInfoMessage("IOException");
                            canceled = true;// TODO:切断された時
                        }
                    };

                    Stream resStream = res.GetResponseStream();
                    resStream.BeginRead(b, 0, b.Length, callback, resStream);

                    while (true) { // キャンセル確認ループ
                        if (token.IsCancellationRequested) {
                            req.Abort();
                            canceled = true;
                            break;
                        }
                        if (canceled) { break; }
                        Thread.Sleep(10);
                    }
                }
                catch (WebException) {
                    // TODO:既に接続が切れていた時
                    //Message.ShowInfoMessage("WebException");

                }
                catch (IOException) {
                    // TODO:既に接続が切れていた時
                    //Message.ShowInfoMessage("IOException");
                }
                catch (Exception ex) {
                    Log.DebugLog(ex);
                    //Message.ShowInfoMessage("Exception");
                }
                finally { endact(); }
            };

            Thread thread = new Thread(ReadStreaming);
            thread.IsBackground = true;
            thread.Start();

            return cts;
        }
        #endregion (userstream_user)

        //===============================================================================
        #region Private Methods
        //-------------------------------------------------------------------------------
        #region -GetByAPI APIから取得
        //-------------------------------------------------------------------------------
        //
        private XElement GetByAPI(string uri, bool renewAPIrest = true)
        {
            WebResponse res = RequestWeb(uri, GET, renewAPIrest);

            if (renewAPIrest && res.Headers.AllKeys.Contains("X-RateLimit-Limit")
                && res.Headers.AllKeys.Contains("X-RateLimit-Remaining")) {
                //API_Max = int.Parse(res.Headers["X-RateLimit-Limit"]);
                //API_Rest = int.Parse(res.Headers["X-RateLimit-Remaining"]);

                string[] tmp = res.Headers["X-RateLimit-Limit"].Split(',');
                API_Max = int.Parse(tmp[tmp.Length - 1]);
                tmp = res.Headers["X-RateLimit-Remaining"].Split(',');
                API_Rest = int.Parse(tmp[tmp.Length - 1]);
            }

            using (Stream resStream = res.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(resStream, Encoding.ASCII)) {
                    //string s = reader.ReadToEnd();
                    try {
                        return XElement.Load(reader);
                    }
                    catch (XmlException ex) {
                        //Log.DebugLog(ex);
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetByAPI)
        //-------------------------------------------------------------------------------
        #region -GetByAPIJson APIから取得(Json ver)
        //-------------------------------------------------------------------------------
        //
        private XElement GetByAPIJson(string uri)
        {
            WebResponse res = RequestWeb(uri, GET, false);

            using (Stream resStream = res.GetResponseStream()) {
                using (XmlDictionaryReader xmldreader = JsonReaderWriterFactory.CreateJsonReader(resStream, XmlDictionaryReaderQuotas.Max)) {
                    try {
                        return XElement.Load(xmldreader);
                    }
                    catch (XmlException ex) {
                        //Log.DebugLog(ex);
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                }
            }
        }
        #endregion (GetByAPIJson)
        //-------------------------------------------------------------------------------
        #region -GetByAPIImage APIから取得(画像ver)
        //-------------------------------------------------------------------------------
        //
        private Tuple<string, Image> GetByAPIImage(string uri)
        {
            WebResponse res = RequestWeb(uri, GET, false);

            string imgUrl = res.ResponseUri.ToString();
            Image img;
            using (Stream s = res.GetResponseStream()) {
                img = Image.FromStream(s);
            }

            return new Tuple<string, Image>(imgUrl, img);
        }
        #endregion (GetByAPIImage)
        //-------------------------------------------------------------------------------
        #region -PostToAPI APIに投稿
        //-------------------------------------------------------------------------------
        //
        private XElement PostToAPI(string uri)
        {
            WebResponse res = RequestWeb(uri, POST, false);

            using (Stream resStream = res.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(resStream, Encoding.ASCII)) {
                    //string s = reader.ReadToEnd();
                    try {
                        return XElement.Load(reader);
                    }
                    catch (XmlException ex) {
                        //Log.DebugLog(ex);
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (PostToAPI)
        //-------------------------------------------------------------------------------
        #region -DeleteToAPI APIにDeleteで投稿
        //-------------------------------------------------------------------------------
        //
        private XElement DeleteToAPI(string uri)
        {
            WebResponse res = RequestWeb(uri, DELETE, false);

            using (Stream resStream = res.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(resStream, Encoding.ASCII)) {
                    //string s = reader.ReadToEnd();
                    try {
                        return XElement.Load(reader);
                    }
                    catch (XmlException ex) {
                        //Log.DebugLog(ex);
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                }
            }
        }
        #endregion (DeleteToAPI)
        //-------------------------------------------------------------------------------
        #region -RequestWeb 要求
        //-------------------------------------------------------------------------------
        //
        private WebResponse RequestWeb(string uri, string method, bool renewAPIrest)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Method = method;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Timeout = 10000;

            WebResponse res;
            try {
                res = req.GetResponse();
            }
            catch (WebException ex) {
                if (ex.Status == WebExceptionStatus.ProtocolError) {
                    HttpWebResponse webres = (HttpWebResponse)ex.Response;
                    throw new TwitterAPIException((int)webres.StatusCode, webres.StatusDescription);
                }
                else if (ex.Status == WebExceptionStatus.NameResolutionFailure
                      || ex.Status == WebExceptionStatus.ConnectFailure) {
                    throw new TwitterAPIException(0, ex.Message);
                }
                else if (ex.Status == WebExceptionStatus.Timeout) {
                    throw new TwitterAPIException(408, ex.Message);
                }
                else {
                    // 不明な(その他の)エラー
                    Log.DebugLog(ex);
                    throw new TwitterAPIException(-1, ex.Message);
                }
            }

            return res;
        }
        #endregion (RequestWeb)
        //-------------------------------------------------------------------------------
        #region -PostImageToAPI 画像を投稿
        //-------------------------------------------------------------------------------
        //
        private XElement PostImageToAPI(string uri, string filename, Image image, string imageContentType)
        {
            Encoding enc = Encoding.UTF8;

            string boundary = Environment.TickCount.ToString();

            WebRequest req = WebRequest.Create(uri);
            req.Method = POST;
            req.Timeout = 20000;
            req.ContentType = "multipart/form-data; boundary=" + boundary;

            Stream reqStream = req.GetRequestStream();
            {
                StringBuilder startsb = new StringBuilder();
                startsb.Append("--");
                startsb.AppendLine(boundary);
                startsb.AppendFormat("Content-Disposition: form-data; name=\"image\"; filename=\"{0}\"", filename);
                startsb.AppendLine();
                startsb.Append("Content-Type: image/");
                startsb.AppendLine(imageContentType);
                startsb.AppendLine();
                string startData = startsb.ToString();

                byte[] d = enc.GetBytes(startData);
                reqStream.Write(d, 0, d.Length);
            }
            image.Save(reqStream, image.RawFormat);
            {
                string endData = "\n--" + boundary + "--";

                byte[] d = enc.GetBytes(endData);
                reqStream.Write(d, 0, d.Length);
            }

            WebResponse res;
            try {
                res = req.GetResponse();
            }
            catch (WebException ex) {
                if (ex.Status == WebExceptionStatus.ProtocolError) {
                    HttpWebResponse webres = (HttpWebResponse)ex.Response;
                    throw new TwitterAPIException((int)webres.StatusCode, webres.StatusDescription);
                }
                else if (ex.Status == WebExceptionStatus.NameResolutionFailure
                      || ex.Status == WebExceptionStatus.ConnectFailure) {
                    throw new TwitterAPIException(0, ex.Message);
                }
                else if (ex.Status == WebExceptionStatus.Timeout) {
                    throw new TwitterAPIException(408, ex.Message);
                }
                else {
                    // 不明な(その他の)エラー
                    Log.DebugLog(ex);
                    throw new TwitterAPIException(-1, ex.Message);
                }
            }

            using (Stream resStream = res.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(resStream, Encoding.ASCII)) {
                    try {
                        return XElement.Load(reader);
                    }
                    catch (XmlException ex) {
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                }
            }
        }
        #endregion (PostImageToAPI)
        //===============================================================================
        #region -GetUrlWithOAuthParameters OAuthのパラメータ情報を付加したURLを取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// OAuthのパラメータ情報を付加したURLを取得します。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetUrlWithOAuthParameters(string url, string httpMethod, IDictionary<string, string> parameters = null)
        {
            SortedDictionary<string, string> parametersAdd = GenerateParameters(AccessToken);
            if (parameters != null) { foreach (KeyValuePair<string, string> p in parameters) { parametersAdd.Add(p.Key, p.Value); } }
            string signature = GenerateSignature(AccessTokenSecret, httpMethod, url, parametersAdd);
            parametersAdd.Add("oauth_signature", Utilization.UrlEncode(signature));

            return url + '?' + JoinParameters(parametersAdd);
        }
        //-------------------------------------------------------------------------------
        #endregion (GetUrlWithOAuthParameters)
        //-------------------------------------------------------------------------------
        #region -HttpGet
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string HttpGet(string url, IDictionary<string, string> parameters)
        {
            WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
            WebResponse res = req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return result;
        }
        //-------------------------------------------------------------------------------
        #endregion (HttpGet)
        #region -HttpPost
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string HttpPost(string url, IDictionary<string, string> parameters)
        {
            byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
            WebRequest req = WebRequest.Create(url);
            req.Method = POST;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            WebResponse res = req.GetResponse();
            Stream resStream = res.GetResponseStream();
            StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            resStream.Close();
            return result;
        }
        //-------------------------------------------------------------------------------
        #endregion (HttpPost)
        #region -ParseResponse
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private Dictionary<string, string> ParseResponse(string response)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string s in response.Split('&')) {
                int index = s.IndexOf('=');
                if (index == -1)
                    result.Add(s, "");
                else
                    result.Add(s.Substring(0, index), s.Substring(index + 1));
            }
            return result;
        }
        //-------------------------------------------------------------------------------
        #endregion (ParseResponse)
        #region -JoinParameters
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string JoinParameters(IDictionary<string, string> parameters)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (var parameter in parameters) {
                if (first)
                    first = false;
                else
                    result.Append('&');
                result.Append(parameter.Key);
                result.Append('=');
                result.Append(parameter.Value);
            }
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (JoinParameters)
        #region -GenerateSignature
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string GenerateSignature(string tokenSecret, string httpMethod, string url, SortedDictionary<string, string> parameters)
        {
            string signatureBase = GenerateSignatureBase(httpMethod, url, parameters);
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(Utilization.UrlEncode(CONSUMER_SECRET) + '&' + Utilization.UrlEncode(tokenSecret));
            byte[] data = System.Text.Encoding.ASCII.GetBytes(signatureBase);
            byte[] hash = hmacsha1.ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateSignature)
        #region -GenerateSignatureBase
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string GenerateSignatureBase(string httpMethod, string url, SortedDictionary<string, string> parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append(httpMethod);
            result.Append('&');
            result.Append(Utilization.UrlEncode(url));
            result.Append('&');
            result.Append(Utilization.UrlEncode(JoinParameters(parameters)));
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateSignatureBase)
        #region -GenerateParameters
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>Tokenから以下の要素を設定したSortedDictionaryを返します。</para>
        /// <para>oauth_consumer_key</para>
        /// <para>oauth_signature_method</para>
        /// <para>oauth_timestamp</para>
        /// <para>oauth_nonce</para>
        /// <para>oauth_version</para>
        /// <para>oauth_token</para>
        /// </summary>
        /// <param name="token"></param>
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        /// <returns></returns>
        private SortedDictionary<string, string> GenerateParameters(string token)
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();
            result.Add("oauth_consumer_key", CONSUMER_KEY);
            result.Add("oauth_signature_method", "HMAC-SHA1");
            result.Add("oauth_timestamp", GenerateTimestamp());
            result.Add("oauth_nonce", GenerateNonce());
            result.Add("oauth_version", "1.0");
            if (!string.IsNullOrEmpty(token))
                result.Add("oauth_token", token);
            return result;
        }
        #endregion (GenerateParameters)
        #region -GenerateNonce
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string GenerateNonce()
        {
            Random random = new Random();

            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(8);
            for (int i = 0; i < 8; ++i)
                result.Append(letters[random.Next(letters.Length)]);
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateNonce)
        #region -GenerateTimestamp
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        private string GenerateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateTimestamp)
        //-------------------------------------------------------------------------------
        #endregion (Private Methods)

        //-------------------------------------------------------------------------------
        #region Private Util Methods
        //-------------------------------------------------------------------------------
        #region -ConvertToTwitData XElementからTwitDataに変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからTwitData型に変換します。
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private TwitData ConvertToTwitData(XElement el)
        {
            try {
                XElement RTel = el.Element("retweeted_status");
                bool notRT = (RTel == null);

                var data = new TwitData() {
                    TwitType = (notRT) ? TwitType.Normal : TwitType.Retweet,
                    DMScreenName = "",
                    StatusID = long.Parse(el.Element("id").Value),
                    Time = StringToDateTime(el.Element("created_at").Value),
                    Favorited = bool.Parse(el.Element("favorited").Value),
                    Mention_StatusID = TryParseLong(el.Element("in_reply_to_status_id").Value),
                    Mention_UserID = TryParseLong(el.Element("in_reply_to_user_id").Value),
                    Mention_ScreenName = el.Element("in_reply_to_screen_name").Value,
                    Text = ConvertSpecialChar(el.Element("text").Value),
                    Source = CutSourceString(el.Element("source").Value),
                    UserID = long.Parse(el.Element("user").Element("id").Value),
                    UserName = el.Element("user").Element("name").Value,
                    IconURL = el.Element("user").Element("profile_image_url").Value,
                    UserScreenName = el.Element("user").Element("screen_name").Value,
                    UserProtected = bool.Parse(el.Element("user").Element("protected").Value),
                    RTTwitData = (notRT) ? null
                        : new TwitData() {
                            TwitType = StarlitTwit.TwitType.Normal,
                            DMScreenName = "",
                            StatusID = long.Parse(RTel.Element("id").Value),
                            Time = StringToDateTime(RTel.Element("created_at").Value),
                            Favorited = bool.Parse(RTel.Element("favorited").Value),
                            Mention_StatusID = TryParseLong(RTel.Element("in_reply_to_status_id").Value),
                            Mention_UserID = TryParseLong(RTel.Element("in_reply_to_user_id").Value),
                            Mention_ScreenName = RTel.Element("in_reply_to_screen_name").Value,
                            Text = ConvertSpecialChar(RTel.Element("text").Value),
                            Source = CutSourceString(RTel.Element("source").Value),
                            UserID = long.Parse(RTel.Element("user").Element("id").Value),
                            UserName = RTel.Element("user").Element("name").Value,
                            IconURL = RTel.Element("user").Element("profile_image_url").Value,
                            UserScreenName = RTel.Element("user").Element("screen_name").Value,
                            UserProtected = bool.Parse(RTel.Element("user").Element("protected").Value)
                        },
                    //Entities = ConvertToEntityData(el.Element("entities")).ToArray()
                };

                data.Entities = GetEntitiesByRegex(data.MainTwitData.Text).ToArray();
                if (!notRT) { data.RTTwitData.Entities = GetEntitiesByRegex(data.RTTwitData.Text).ToArray(); }

                return data;
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToTwitData)
        //-------------------------------------------------------------------------------
        #region -ConvertToTwitDataArray XElementからTwitDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからTwitDataの配列型に変換します。
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private IEnumerable<TwitData> ConvertToTwitDataArray(XElement el)
        {
            return from stat in el.Elements("status")
                   select ConvertToTwitData(stat);
        }
        #endregion (GetTwitData)
        //-------------------------------------------------------------------------------
        #region -ConvertToTwitDataDM XElementからTwitDataに変換します。(DM用)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>XElementからTwitDataに変換します。</para>
        /// <para>DirectMessageデータ用です。</para>
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private TwitData ConvertToTwitDataDM(XElement el)
        {
            try {
                var data = new TwitData() {
                    TwitType = StarlitTwit.TwitType.DirectMessage,
                    DMScreenName = el.Element("recipient_screen_name").Value,
                    StatusID = long.Parse(el.Element("id").Value),
                    Time = StringToDateTime(el.Element("created_at").Value),
                    Favorited = false,
                    Mention_StatusID = -1,
                    Mention_UserID = -1,
                    Text = ConvertSpecialChar(el.Element("text").Value),
                    Source = "",
                    UserID = long.Parse(el.Element("sender").Element("id").Value),
                    UserName = el.Element("sender").Element("name").Value,
                    IconURL = el.Element("sender").Element("profile_image_url").Value,
                    UserScreenName = el.Element("sender_screen_name").Value,
                    UserProtected = bool.Parse(el.Element("sender").Element("protected").Value),
                    RTTwitData = null
                };

                data.Entities = GetEntitiesByRegex(data.Text).ToArray();

                return data;
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToTwitDataDM)
        //-------------------------------------------------------------------------------
        #region -ConvertToTwitDataArrayDM XElementからTwitDataの配列型に変換します。(DM用)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>XElementからTwitDataの配列型に変換します。</para>
        /// <para>DirectMessageデータ用です。</para>
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TwitData> ConvertToTwitDataArrayDM(XElement el)
        {
            return from stat in el.Elements("direct_message")
                   select ConvertToTwitDataDM(stat);
        }
        #endregion (ConvertToTwitDataArrayDM)
        //-------------------------------------------------------------------------------
        #region -ConvertToListData XElementからListDataに変換します。
        //-------------------------------------------------------------------------------
        //
        private ListData ConvertToListData(XElement el)
        {
            try {
                return new ListData() {
                    ID = long.Parse(el.Element("id").Value),
                    Name = el.Element("name").Value,
                    Slug = el.Element("slug").Value,
                    Description = el.Element("description").Value,
                    SubscriberCount = int.Parse(el.Element("subscriber_count").Value),
                    MemberCount = int.Parse(el.Element("member_count").Value),
                    Public = el.Element("mode").Value.Equals("public"),
                    OwnerID = long.Parse(el.Element("user").Element("id").Value),
                    OwnerScreenName = el.Element("user").Element("screen_name").Value
                };
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToListData)
        //-------------------------------------------------------------------------------
        #region -ConvertToListDataArray XElementからListDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからListDataの配列型に変換します。
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private IEnumerable<ListData> ConvertToListDataArray(XElement el)
        {
            return from stat in el.Elements("list")
                   select ConvertToListData(stat);
        }
        #endregion (ConvertToListData)
        //-------------------------------------------------------------------------------
        #region -ConvertToTwitDataJson XElementからTwitDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<TwitData> ConvertToTwitDataJson(XElement el)
        {
            try {
                Func<XElement, TwitData> makeTwitData = xel =>
                {
                    TwitData data = new TwitData() {
                        TwitType = StarlitTwit.TwitType.Search,
                        DMScreenName = "",
                        StatusID = long.Parse(xel.Element("id").Value),
                        Time = StringToDateTime(xel.Element("created_at").Value),
                        Favorited = false,
                        Mention_StatusID = TryParseLong(xel.Element("to_user_id").Value),
                        Mention_UserID = -1,
                        Text = ConvertSpecialChar(xel.Element("text").Value),
                        Source = CutSourceString(ConvertSpecialChar(xel.Element("source").Value)),
                        UserID = long.Parse(xel.Element("from_user_id").Value),
                        UserName = "",
                        IconURL = xel.Element("profile_image_url").Value,
                        UserScreenName = xel.Element("from_user").Value,
                        UserProtected = false,
                        RTTwitData = null
                    };
                    data.Entities = GetEntitiesByRegex(data.Text).ToArray();
                    return data;
                };

                return from stat in el.Element("results").Elements("item")
                       select makeTwitData(stat);
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToTwitDataJson)
        //-------------------------------------------------------------------------------
        #region -ConvertToUserProfile XElementからUserProfile型に変換します。
        //-------------------------------------------------------------------------------
        //
        private UserProfile ConvertToUserProfile(XElement el)
        {
            try {
                UserProfile profile = new UserProfile() {
                    UserID = long.Parse(el.Element("id").Value),
                    UserName = el.Element("name").Value,
                    ScreenName = el.Element("screen_name").Value,
                    IconURL = el.Element("profile_image_url").Value,
                    URL = el.Element("url").Value,
                    Protected = TryParseBool(el.Element("protected").Value),
                    FolllowRequestSent = TryParseBool(el.Element("follow_request_sent").Value),
                    Location = el.Element("location").Value,
                    Description = el.Element("description").Value,
                    Following = TryParseBool(el.Element("following").Value),
                    FollowerNum = int.Parse(el.Element("followers_count").Value),
                    FollowingNum = int.Parse(el.Element("friends_count").Value),
                    StatusNum = int.Parse(el.Element("statuses_count").Value),
                    ListedNum = int.Parse(el.Element("listed_count").Value),
                    FavoriteNum = int.Parse(el.Element("favourites_count").Value),
                    RegisterTime = StringToDateTime(el.Element("created_at").Value),
                    TimeZone = el.Element("time_zone").Value
                };
                if (el.Element("status") != null) {
                    profile.LastTwitData = new TwitData() {
                        TwitType = TwitType.Normal,
                        UserID = profile.UserID,
                        IconURL = profile.IconURL,
                        UserName = profile.UserName,
                        UserScreenName = profile.ScreenName,
                        UserProtected = profile.Protected,
                        Time = StringToDateTime(el.Element("status").Element("created_at").Value),
                        StatusID = long.Parse(el.Element("status").Element("id").Value),
                        Mention_StatusID = TryParseLong(el.Element("status").Element("in_reply_to_status_id").Value),
                        Mention_UserID = TryParseLong(el.Element("status").Element("in_reply_to_user_id").Value),
                        Text = ConvertSpecialChar(el.Element("status").Element("text").Value),
                        Source = CutSourceString(el.Element("status").Element("source").Value),
                        Favorited = bool.Parse(el.Element("status").Element("favorited").Value)
                    };
                }
                else { profile.LastTwitData = null; }
                return profile;
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (ConvertToUserProfile)
        //-------------------------------------------------------------------------------
        #region -ConvertToUserProfileArray XElementからUserProfileの配列型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからUserProfileの配列型に変換します。
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        private IEnumerable<UserProfile> ConvertToUserProfileArray(XElement el)
        {
            return from stat in el.Elements("user")
                   select ConvertToUserProfile(stat);
        }
        #endregion (ConvertToUserProfileArray)
        //-------------------------------------------------------------------------------
        #region -ConvertToAPILimitData XElementからAPILimitData型に変換します。
        //-------------------------------------------------------------------------------
        //
        private APILimitData ConvertToAPILimitData(XElement el)
        {
            try {
                APILimitData data = new APILimitData() {
                    Remaining = int.Parse(el.Element("remaining-hits").Value),
                    HourlyLimit = int.Parse(el.Element("hourly-limit").Value),
                    ResetTime = StringToDateTime(el.Element("reset-time").Value)
                };

                // reset-time-in-secondsによるAssertion
                long rt = long.Parse(el.Element("reset-time-in-seconds").Value);
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // 基準時刻
                DateTime dt2 = dt.ToLocalTime().AddSeconds(rt);
                Debug.Assert(data.ResetTime == dt2);

                return data;
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToAPILimitData)
        //-------------------------------------------------------------------------------
        #region -ConvertToRelationshipData XElementからRelationshipData型に変換します
        //-------------------------------------------------------------------------------
        //
        private RelationshipData ConvertToRelationshipData(XElement el)
        {
            try {
                XElement source = el.Element("source");
                XElement target = el.Element("target");
                return new RelationshipData() {
                    Source_ScreenName = source.Element("screen_name").Value,
                    Source_UserID = long.Parse(source.Element("id_str").Value),
                    Target_ScreenName = target.Element("screen_name").Value,
                    Target_UserID = long.Parse(target.Element("id_str").Value),
                    Following = bool.Parse(source.Element("following").Value),
                    Followed = bool.Parse(source.Element("followed_by").Value),
                    AllReplies = ParseBoolConsideringNil(source.Element("all_replies")),
                    Blocking = ParseBoolConsideringNil(source.Element("blocking")),
                    //CanDM = bool.Parse(source.Element("can_dm").Value),
                    CanDM = ParseBoolConsideringNil(source.Element("can_dm")),
                    Marked_Spam = ParseBoolConsideringNil(source.Element("marked_spam")),
                    Notification_Enabled = ParseBoolConsideringNil(source.Element("notifications_enabled")),
                    Want_Retweets = ParseBoolConsideringNil(source.Element("want_retweets"))
                };
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToRelationshipData)
        //-------------------------------------------------------------------------------
        #region -ConvertToEntityData XElementからEntityData型に変換します
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<EntityData> ConvertToEntityData(XElement el)
        {
            if (el == null) { yield break; }

            // mention
            var mentions = from m in el.Element("user_mentions").Elements("user_mention")
                           select new EntityData(ItemType.User,
                                                 Range.Make(int.Parse(m.Attribute("start").Value),
                                                            int.Parse(m.Attribute("end").Value)),
                                                 m.Element("screen_name").Value);
            // hashtags
            var hashtags = from h in el.Element("hashtags").Elements("hashtag")
                           select new EntityData(ItemType.HashTag,
                                                 Range.Make(int.Parse(h.Attribute("start").Value),
                                                            int.Parse(h.Attribute("end").Value)),
                                                 h.Element("text").Value);

            // url
            var urls = from u in el.Element("urls").Elements("url")
                       select new EntityData(null,
                                             Range.Make(int.Parse(u.Attribute("start").Value),
                                                        int.Parse(u.Attribute("end").Value)),
                                             u.Element("url").Value);


            foreach (var item in mentions) { yield return item; }
            foreach (var item in hashtags) { yield return item; }
            foreach (var item in urls) { yield return item; }
        }
        #endregion (ConvertToEntityData)
        //===============================================================================
        #region -ConvertToStreamItem XElementをUserStreamのアイテムに変換します。
        //-------------------------------------------------------------------------------
        //
        private Tuple<UserStreamItemType, object> ConvertToStreamItem(XElement el)
        {
            Func<string> Logging = () =>
            {
                string filename = string.Format("Xml/{0}.xml", DateTime.Now.ToString("yyMMddHHmmssffff"));
                using (StreamWriter writer = new StreamWriter(filename)) {
                    writer.Write(el.ToString());
                }
                return filename;
            };

            try {
                if (el.Element("event") != null) {
                    // event
                    string eventName = el.Element("event").Value;
                    UserStreamEventData data = new UserStreamEventData();
                    data.Type = (UserStreamEventType)Enum.Parse(typeof(UserStreamEventType), eventName);

                    data.Time = StringToDateTime(el.Element("created_at").Value);

                    data.TargetUser = ConvertToUserProfile(el.Element("target"));
                    data.SourceUser = ConvertToUserProfile(el.Element("source"));

                    switch (data.Type) {
                        case UserStreamEventType.favorite:
                        case UserStreamEventType.unfavorite:
                            data.TargetTwit = ConvertToTwitData(el.Element("target_object"));
                            break;
                        case UserStreamEventType.list_created:
                        case UserStreamEventType.list_destroyed:
                        case UserStreamEventType.list_member_added:
                        case UserStreamEventType.list_member_removed:
                        case UserStreamEventType.list_updated:
                        case UserStreamEventType.list_user_subscribed:
                        case UserStreamEventType.list_user_unsubscribed:
                            data.TargetList = ConvertToListData(el.Element("target_object"));
                            break;
                    }
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.eventdata, data);
                }
                else if (el.Element("friends") != null) {
                    // friend list
                    IEnumerable<long> friend_ids = from id in el.Elements("friends")
                                                   select long.Parse(id.Value);
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.friendlist, friend_ids);
                }
                else if (el.Element("delete") != null) {
                    XElement del = el.Element("delete");
                    if (del.Element("direct_message") != null) {
                        long delete_id = long.Parse(del.Element("direct_message").Element("id").Value);
                        return new Tuple<UserStreamItemType, object>(UserStreamItemType.directmessage_delete, delete_id);
                    }
                    else {
                        long delete_id = long.Parse(del.Element("status").Element("id").Value);
                        return new Tuple<UserStreamItemType, object>(UserStreamItemType.status_delete, delete_id);
                    }
                }
                else if (el.Element("direct_message") != null) {
                    var dmdata = ConvertToTwitDataDM(el.Element("direct_message"));
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.directmessage, dmdata);
                }
                else if (el.Element("limit") != null) {
                    int value = int.Parse(el.Element("limit").Element("track").Value);
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.tracklimit, value);
                }
                else {
                    // status
                    var twitdata = ConvertToTwitData(el);
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.status, twitdata);
                }
            }
            catch (Exception) {
                string filename = Logging();
                return new Tuple<UserStreamItemType, object>(UserStreamItemType.unknown, filename);
            }
        }
        #endregion (ConvertToStreamItem)
        //-------------------------------------------------------------------------------
        #region -JsonToXElement Json文字列をXElementに変換します。
        //-------------------------------------------------------------------------------
        //
        private XElement JsonToXElement(string jsonStr)
        {
            XmlNode node = JsonConvert.DeserializeXmlNode(jsonStr, "item");
            return XmlNodeToXElement(node);
        }
        #endregion (JsonToXElement)
        //-------------------------------------------------------------------------------
        #region -XmlNodeToXElement XmlNode->XElement
        //-------------------------------------------------------------------------------
        //
        private XElement XmlNodeToXElement(XmlNode node)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xw = doc.CreateWriter()) {
                node.WriteTo(xw);
            }
            return doc.Root;
        }
        #endregion (-XmlNodeToXElement)
        //===============================================================================
        #region -TryParseLong 文字列をlongに変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 文字列をlongに変換します。変換できなければ-1が返ります。
        /// </summary>
        /// <param name="str">変換する文字列</param>
        /// <returns></returns>
        private long TryParseLong(string str, long defaultValue = -1)
        {
            long l;
            if (long.TryParse(str, out l)) {
                return l;
            }
            return defaultValue;
        }
        #endregion (TryParseLong)
        //-------------------------------------------------------------------------------
        #region -TryParseBool 文字列をboolに変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 文字列をboolに変換します。変換できない時やnullの場合はfalseが返ります。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool TryParseBool(string str, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(str)) { return defaultValue; }
            bool b;
            if (bool.TryParse(str, out b)) { return b; }
            return defaultValue;
        }
        #endregion (TryParseBool)
        //-------------------------------------------------------------------------------
        #region -CutSourceString sourceデータの文字列のタグ部分を消します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// sourceデータの文字列のhrefタグ部分を消します。
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        private string CutSourceString(string sourceString)
        {
            int i1 = sourceString.IndexOf('>') + 1,
                i2 = sourceString.IndexOf('<', i1);
            return (i1 == 0) ? sourceString : sourceString.Substring(i1, i2 - i1);
        }
        #endregion (CutSourceString)
        //-------------------------------------------------------------------------------
        #region -StringToDateTime created_atデータの文字列をDateTimeに変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// created_atデータの文字列をDateTimeに変換します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private DateTime StringToDateTime(string str)
        {
            //例1：Fri Jul 16 16:58:46 +0000 2010
            //例2：Mon, 11 Oct 2010 00:00:34 +0000
            //例3：Sun Oct 10 17:25:16 UTC 2010
            //例4：2011-03-12T14:14:01+00:00

            return DateTime.ParseExact(str, DATETIME_FORMATS, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
        }
        #endregion (StringToDateTime)
        //-------------------------------------------------------------------------------
        #region -ParseBoolConsideringNil Nilを考慮して要素からbool値を取り出します。
        //-------------------------------------------------------------------------------
        //
        private bool ParseBoolConsideringNil(XElement el, bool defaultvalue = false)
        {
            bool b;
            return (bool.TryParse(el.Value, out b)) ? b :
                   (el.Value == "") ? bool.Parse(el.Attribute("nil").Value) :
                                      defaultvalue;
        }
        #endregion (ParseBoolConsideringNil)
        //-------------------------------------------------------------------------------
        #region -ConvertSpecialChar 特殊文字を変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 特殊文字を置換します。
        /// [&lt;][&gt;][&amp;][&quot;][&apos;]
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ConvertSpecialChar(string text)
        {
            return text.Replace(@"&lt;", "<")
                       .Replace(@"&gt;", ">")
                       .Replace(@"&amp;", "&")
                       .Replace(@"&quot;", "\"")
                       .Replace(@"&apos;", "'");
        }
        #endregion (ConvertSpecialChar)
        //-------------------------------------------------------------------------------
        #region -ConcatWithComma カンマで内容を連結して返します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// カンマで内容を連結して返します。
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ConcatWithComma(Array array)
        {
            bool isfirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (object o in array) {
                if (!isfirst) {
                    //sb.Append(',');
                    sb.Append(", ");
                }
                isfirst = false;
                sb.Append(o.ToString());
            }
            return Utilization.UrlEncode(sb.ToString());
        }
        #endregion (ConcatWithComma)
        //-------------------------------------------------------------------------------
        #region -GetEntitiesByRegex 正規表現を利用してエンティティを抽出します。
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<EntityData> GetEntitiesByRegex(string text)
        {
            List<EntityData> list = new List<EntityData>();
            Regex r = new Regex(HASH_REGEX_PATTERN);
            foreach (Match m in r.Matches(text)) {
                Group g = m.Groups["entity"];
                switch (g.Value[0]) {
                    case '@':
                        yield return new EntityData(ItemType.User, new Range(g.Index, g.Length), g.Value.Substring(1));
                        break;
                    case '#':
                        yield return new EntityData(ItemType.HashTag, new Range(g.Index, g.Length), g.Value);
                        break;
                    default:
                        Debug.Assert(false, "ここには来ない");
                        break;
                }
            }

            Regex r2 = new Regex(Utilization.URL_REGEX_PATTERN);
            foreach (Match m in r2.Matches(text)) {
                string value;
                if (m.Value[0] == 't') { value = 'h' + m.Value; }
                else { value = m.Value; }
                yield return new EntityData(null, new Range(m.Index, m.Length), value);
            }
        }
        #endregion (GetEntitiesByRegex)
        //-------------------------------------------------------------------------------
        #endregion (Private Util Methods)
    }

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
            get { return IsRT() ? RTTwitData : this; }
        }
        #endregion (MainTwitData)
        //-------------------------------------------------------------------------------
        #region +IsRT Retweetかどうか
        //-------------------------------------------------------------------------------
        /// <summary>
        /// この発言がRetweetかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsRT()
        {
            return (this.TwitType == TwitType.Retweet);
        }
        #endregion (IsRT)
        //-------------------------------------------------------------------------------
        #region +IsDM DirectMessageかどうか
        //-------------------------------------------------------------------------------
        /// <summary>
        /// この発言がDirectMessageかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsDM()
        {
            return (this.TwitType == TwitType.DirectMessage);
        }
        #endregion (IsDM)
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
        public int FollowingNum;
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
        /// <summary></summary>
        public bool Want_Retweets;
        /// <summary></summary>
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

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public EntityData(ItemType? type, Range range, string str)
        {
            this.type = type;
            this.range = range;
            this.str = str;
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
        /// <summary>Track Limit Notices(object:int)</summary>
        tracklimit
        // Location Deletion Notices
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
