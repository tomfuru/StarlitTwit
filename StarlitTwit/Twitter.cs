using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Diagnostics;

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
        public static readonly string URLapi;
        public static readonly string URLtwi;
        public static readonly string URLsearch;
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
        public IEnumerable<TwitData> statuses_public_timeline(bool trim_user = false, bool include_entities = false)
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
                                                 bool trim_user = false, bool include_entities = false)
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
                                                    bool trim_user = false, bool include_rts = false, bool include_entities = false)
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
                                           bool trim_user = false, bool include_rts = false, bool include_entities = false)
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
                                            bool trim_user = false, bool include_rts = false, bool include_entities = false)
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
                                                   bool trim_user = false, bool include_entities = false)
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
                                                   bool trim_user = false, bool include_entities = false)
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
                                                  bool trim_user = false, bool include_entities = false)
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
        public TwitData statuses_show(bool withAuthParam, long id, bool trim_user = false, bool include_entities = false)
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
                                    string place_id = "", bool display_coordinates = false, bool trim_user = false, bool include_entities = false)
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
        public TwitData statuses_destroy(long id, bool trim_user = false, bool include_entities = false)
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
        #region +statuses_retweets（未デバッグ）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/retweetsメソッド（未デバッグ）
        /// </summary>
        /// <param name="id">リツイートを見る発言のID</param>
        /// <param name="count">[option] &lt;100</param>
        /// <param name="trim_user">[option]</param>
        /// <param name="include_entities">[option]</param>
        public IEnumerable<TwitData> statuses_retweets(long id, int count = -1, bool trim_user = false, bool include_entities = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweets/" + id.ToString() + ".xml", GET, paramdic);
            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweets)
        //-------------------------------------------------------------------------------
        #region +statuses_id_retweeted_by (未実装)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/id/retweeted_byメソッド (未実装)
        /// </summary>
        public void statuses_id_retweeted_by()
        {

        }
        #endregion (statuses_id_retweeted_by)
        //-------------------------------------------------------------------------------
        #region +statuses_id_retweeted_by_ids (未実装)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// statuses/id/retweeted_by/idsメソッド (未実装)
        /// </summary>
        public void statuses_id_retweeted_by_ids()
        {

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
        public UserProfile users_show(long user_id = -1, string screen_name = null, bool include_entities = false)
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
        public IEnumerable<UserProfile> users_lookup(long[] user_ids = null, string[] screen_names = null, bool include_entities = false)
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
        public Tuple<IEnumerable<UserProfile>, long, long> statuses_friends(long user_id = -1, string screen_name = null, long cursor = -1, bool include_entities = false)
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
            return new Tuple<IEnumerable<UserProfile>, long, long>(ConvertToUserProfileArray(el.Element("users")), int.Parse(el.Element("next_cursor").Value), int.Parse(el.Element("previous_cursor").Value));
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
        public Tuple<IEnumerable<UserProfile>, long, long> statuses_followers(long user_id = -1, string screen_name = null, long cursor = -1, bool include_entities = false)
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
            return new Tuple<IEnumerable<UserProfile>, long, long>(ConvertToUserProfileArray(el.Element("users")), int.Parse(el.Element("next_cursor").Value), int.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (statuses_followers)
        //-------------------------------------------------------------------------------
        #endregion (users/)

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
        public IEnumerable<TwitData> direct_messages(long since_id = -1, long max_id = -1, int count = -1, int page = -1, bool include_entities = false)
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
        public IEnumerable<TwitData> direct_messages_sent(long since_id = -1, long max_id = -1, int count = -1, int page = -1, bool include_entities = false)
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
        public TwitData direct_messages_new(string screen_name, long user_id, string text, bool include_entities = false)
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
        public TwitData direct_messages_destroy(long id, bool include_entities = false)
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
        #region list/ (リスト関連)
        //-------------------------------------------------------------------------------
        #region lists_Add リスト追加（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト追加メソッド
        /// </summary>
        /// <returns></returns>
        private object lists_Add()
        {

            //string url = GetUrlWithOAuthParameters(URL + @"lists.xml", POST, paramdic);

            //XElement el = PostToAPI(url);
            //return ConvertToTwitDataDM(el);

            throw new NotImplementedException();
        }
        #endregion (lists_Add)
        //-------------------------------------------------------------------------------
        #region lists_Update リスト更新（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト更新
        /// </summary>
        /// <returns></returns>
        private object lists_Update()
        {
            //string url = GetUrlWithOAuthParameters(URL + @"lists.xml", POST, paramdic);

            //XElement el = PostToAPI(url);
            //return ConvertToTwitDataDM(el);

            throw new NotImplementedException();
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
        public Tuple<IEnumerable<ListData>, long, long> lists_Get(string screen_name = "", long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string scrname = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;

            string url = GetUrlWithOAuthParameters(URLapi + scrname + @"/lists.xml", GET, paramdic);

            XElement el = GetByAPI(url);

            return new Tuple<IEnumerable<ListData>, long, long>(ConvertToListData(el.Element("lists")), int.Parse(el.Element("next_cursor").Value), int.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_Get)
        //-------------------------------------------------------------------------------
        #region list_Show リスト情報取得（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// list リスト情報取得
        /// </summary>
        /// <returns></returns>
        private object list_Show()
        {
            //string url = GetUrlWithOAuthParameters(URL + @"lists.xml", GET, paramdic);

            //XElement el = GetByAPI(url);
            //return ConvertToTwitDataDM(el);

            throw new NotImplementedException();
        }
        #endregion (list_Show)
        //-------------------------------------------------------------------------------
        #region lists_Delete リスト削除（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists リスト削除
        /// </summary>
        /// <returns></returns>
        private object lists_Delete()
        {
            //string url = GetUrlWithOAuthParameters(URL + @"lists.xml", "DELETE", paramdic);

            //XElement el = PostToAPI(url,"DELETE");
            //return ConvertToTwitDataDM(el);

            throw new NotImplementedException();
        }
        #endregion (lists_Delete)
        //-------------------------------------------------------------------------------
        #region lists_statuses リスト発言取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists/statuses リストの発言取得
        /// </summary>
        /// <param name="list_id">リストのID</param>
        /// <param name="screen_name">[option]リストの作成者のScreenName。省略すると自分。</param>
        /// <param name="since_id">[option]</param>
        /// <param name="max_id">[option]</param>
        /// <param name="per_page">[option]</param>
        /// <param name="page">[option]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> lists_statuses(string list_id, string screen_name = "", long since_id = -1, long max_id = -1, int per_page = -1, int page = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (per_page > 0) { paramdic.Add("per_page", per_page.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
            }

            string scrname = (string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;

            string url = GetUrlWithOAuthParameters(URLapi + scrname + @"/lists/" + list_id + @"/statuses.xml", GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToTwitDataArray(el);
        }
        #endregion (lists_statuses)
        //-------------------------------------------------------------------------------
        #region lists_memberships（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists_membershipsメソッド（未実装）
        /// </summary>
        private void lists_memberships()
        {

        }
        #endregion (lists_memberships)
        //-------------------------------------------------------------------------------
        #region lists_subscriptions（未実装）
        //-------------------------------------------------------------------------------
        /// <summary>
        /// lists_subscriptionsメソッド（未実装）
        /// </summary>
        private void lists_subscriptions()
        {

        }
        #endregion (lists_subscriptions)
        //-------------------------------------------------------------------------------
        #endregion (list)

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
        /// <param name="follow">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile friendships_create(long user_id = -1, string screen_name = null, bool follow = false, bool include_entities = false)
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
        public UserProfile friendships_destroy(long user_id = -1, string screen_name = null, bool include_entities = false)
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
        public UserProfile account_update_profile(string name = null, string url = null, string location = null, string description = null, bool include_entities = false)
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
        public TwitData favorites_create(long id, bool include_entities = false)
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
        public TwitData favorites_destroy(long id, bool include_entities = false)
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
        #region UserStream　(test)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テスト
        /// </summary>
        public void userstream_statuses_sample()
        {
            const string URL_SAMPLE = @"http://stream.twitter.com/1/statuses/sample.json";
            string url = GetUrlWithOAuthParameters(URL_SAMPLE, GET, null);

            WebRequest req = WebRequest.Create(url);

            WebResponse res = req.GetResponse();
            using (Stream stream = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream)) {
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();
                    Console.WriteLine(line);
                }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (UserStream)

        //===============================================================================
        #region Private Methods
        //-------------------------------------------------------------------------------
        #region -GetByAPI APIから取得
        //-------------------------------------------------------------------------------
        //
        private XElement GetByAPI(string uri, bool renewAPIrest = true)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Method = GET;
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
        #region -PostToAPI APIに投稿
        //-------------------------------------------------------------------------------
        //
        private XElement PostToAPI(string uri)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Method = POST;
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
        #region -GetByAPIJson APIから取得(Json ver)
        //-------------------------------------------------------------------------------
        //
        private XElement GetByAPIJson(string uri)
        {
            WebRequest req = WebRequest.Create(uri);
            req.Method = GET;
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
                bool notRT = (el.Element("retweeted_status") == null);
                return new TwitData() {
                    TwitType = (notRT) ? TwitType.Normal : TwitType.Retweet,
                    DMScreenName = "",
                    StatusID = long.Parse(el.Element("id").Value),
                    Time = StringToDateTime(el.Element("created_at").Value),
                    Favorited = bool.Parse(el.Element("favorited").Value),
                    Mention_StatusID = TryParseLong(el.Element("in_reply_to_status_id").Value),
                    Mention_UserID = TryParseLong(el.Element("in_reply_to_user_id").Value),
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
                            StatusID = long.Parse(el.Element("retweeted_status").Element("id").Value),
                            Time = StringToDateTime(el.Element("retweeted_status").Element("created_at").Value),
                            Favorited = bool.Parse(el.Element("retweeted_status").Element("favorited").Value),
                            Mention_StatusID = TryParseLong(el.Element("retweeted_status").Element("in_reply_to_status_id").Value),
                            Mention_UserID = TryParseLong(el.Element("retweeted_status").Element("in_reply_to_user_id").Value),
                            Text = ConvertSpecialChar(el.Element("retweeted_status").Element("text").Value),
                            Source = CutSourceString(el.Element("retweeted_status").Element("source").Value),
                            UserID = long.Parse(el.Element("retweeted_status").Element("user").Element("id").Value),
                            UserName = el.Element("retweeted_status").Element("user").Element("name").Value,
                            IconURL = el.Element("retweeted_status").Element("user").Element("profile_image_url").Value,
                            UserScreenName = el.Element("retweeted_status").Element("user").Element("screen_name").Value,
                            UserProtected = bool.Parse(el.Element("retweeted_status").Element("user").Element("protected").Value)
                        }
                };
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
                return new TwitData() {
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
        #region -ConvertToListData XElementからListDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからListDataの配列型に変換します。
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private IEnumerable<ListData> ConvertToListData(XElement el)
        {
            try {
                return from stat in el.Elements("list")
                       select new ListData() {
                           ID = long.Parse(stat.Element("id").Value),
                           Name = stat.Element("slug").Value,
                           Description = stat.Element("description").Value,
                           SubscriberCount = int.Parse(stat.Element("subscriber_count").Value),
                           MemberCount = int.Parse(stat.Element("member_count").Value),
                           Public = stat.Element("mode").Value.Equals("public"),
                           OwnerID = long.Parse(stat.Element("user").Element("id").Value),
                           OwnerScreenName = stat.Element("user").Element("screen_name").Value
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
        #region -ConvertToTwitDataJson XElementからTwitDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<TwitData> ConvertToTwitDataJson(XElement el)
        {
            try {
                return from stat in el.Element("results").Elements("item")
                       select new TwitData() {
                           TwitType = StarlitTwit.TwitType.Search,
                           DMScreenName = "",
                           StatusID = long.Parse(stat.Element("id").Value),
                           Time = StringToDateTime(stat.Element("created_at").Value),
                           Favorited = false,
                           Mention_StatusID = TryParseLong(stat.Element("to_user_id").Value),
                           Mention_UserID = -1,
                           Text = ConvertSpecialChar(stat.Element("text").Value),
                           Source = CutSourceString(ConvertSpecialChar(stat.Element("source").Value)),
                           UserID = long.Parse(stat.Element("from_user_id").Value),
                           UserName = "",
                           IconURL = stat.Element("profile_image_url").Value,
                           UserScreenName = stat.Element("from_user").Value,
                           UserProtected = false,
                           RTTwitData = null
                       };
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
                    Source_UserID = long.Parse(source.Element("id").Value),
                    Target_ScreenName = target.Element("screen_name").Value,
                    Target_UserID = long.Parse(target.Element("id").Value),
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
        #endregion (Private Util Methods)
    }

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
        /// <summary>ダイレクトメッセージの送信先のユーザー名</summary>
        public string DMScreenName;
        /// <summary>発言をお気に入りに登録しているか</summary>
        public bool Favorited;
        /// <summary>RT発言情報</summary>
        public TwitData RTTwitData;

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


    //-----------------------------------------------------------------------------------
    #region (Class)TwitterAPIException
    //-------------------------------------------------------------------------------
    /// <summary>
    /// <para>TwitterクラスにおいてAPIからエラーが返された時にスローされる例外</para>
    /// <para>・ステータスコード</para>
    /// <para>-1 Unknown Error           不明なエラー(ただし以下以外のものも不明なエラーとして扱われる)</para>
    /// <para>0 Connection Failure:      接続に失敗しました。</para>
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
