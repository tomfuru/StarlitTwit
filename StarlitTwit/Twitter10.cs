﻿using System;
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

/* Twitter API Resources(Most Recent)
 * Timelines
 * Tweets
 * Search
 * Direct Message
 * Friends&Tweets
 % Users
 * Suggested Users
 * Favorites
 % Lists
 % Accounts
 - Notification
 - Saved Searches
 - Local Trends
 - Place&Geo
 - Trends
 % Block
 * Spam Reporting
 * OAuth
 - Help
 - Legal
 * Deprecated
 */

namespace StarlitTwit
{
    /// <summary>
    /// TwitterのAPI処理を行うためのクラス。
    /// </summary>
    public class Twitter10
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
        private const string XML = "xml";
        private const string JSON = "json";
        public static readonly string URLtwi;
        public static readonly string URLapi;
        public static readonly string URLapiUpload;
        public static readonly string URLapiSSL;
        public static readonly string URLapiSSLnoVer;
        public static readonly string URLsearch;

        public const bool DEFAULT_INCLUDE_ENTITIES = true;

        public const string MENTION_REGEX_PATTERN = @"(@|＠)(?<entity>[a-zA-Z0-9_]+?)($|[^a-zA-Z0-9_])";
        //public const string HASHTAG_REGEX_PATTERN = @"(?<entity>#(?!\d+($|\s))\w+)($|\s)";
        public const string HASHTAG_REGEX_PATTERN = @"(?<entity>(#|＃)(?!\d+($|[^a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}]))[a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}]+)($|[^a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}])";
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
        static Twitter10()
        {
            URLtwi = @"http://twitter.com/";
            URLapi = @"http://api.twitter.com/" + API_VERSION.ToString() + '/';
            URLapiUpload = @"https://upload.twitter.com/" + API_VERSION.ToString() + '/';
            URLapiSSL = @"https://api.twitter.com/" + API_VERSION.ToString() + '/';
            URLapiSSLnoVer = @"https://api.twitter.com/";
            URLsearch = @"http://search.twitter.com/";
        }
        //
        public Twitter10()
        {
            API_Max = -1;
            API_Rest = -1;

            List<string> strList = new List<string>();
            foreach (string str in StarlitTwit.Properties.Settings.Default.DateTimeFormat) { strList.Add(str); }
            DATETIME_FORMATS = strList.ToArray();
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region Timelines Resources
        //-------------------------------------------------------------------------------
        #region +statuses_home_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/home_timelineメソッド</para>
        /// <para>Returns the 20 most recent statuses, including retweets if they exist, posted by the authenticating user and the user's they follow.</para>
        /// <para>上限800</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_home_timeline(int count = -1, long since_id = -1, long max_id = -1,
                                                bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES,
                                                bool exclude_replies = false, bool contributor_details = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (exclude_replies) { paramdic.Add("exclude_replies", exclude_replies.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/home_timeline.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_home_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_mentions
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/mentionsメソッド</para>
        /// <para>Returns the 20 most recent mentions (status containing @username) for the authenticating user.</para>
        /// <para>上限800</para>
        /// </summary>
        public IEnumerable<TwitData> statuses_mentions(int count = -1, long since_id = -1, long max_id = -1,
                                            bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES,
                                            bool contributor_details = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/mentions.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_mentions)
        //-------------------------------------------------------------------------------
        #region +statuses_retweeted_by_me
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/retweeted_by_meメソッド</para>
        /// <para>Returns the 20 most recent retweets posted by the authenticating user.</para>
        /// </summary>
        public IEnumerable<TwitData> statuses_retweeted_by_me(int count = -1, long since_id = -1, long max_id = -1,
                                                   bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
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
        /// <para>statuses/retweeted_to_meメソッド</para>
        /// <para>Returns the 20 most recent retweets posted by users the authenticating user follow.</para>
        /// </summary>
        public IEnumerable<TwitData> statuses_retweeted_to_me(int count = -1, long since_id = -1, long max_id = -1,
                                                   bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
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
        /// <para>statuses/retweets_of_meメソッド</para>
        /// <para>Returns the 20 most recent tweets of the authenticated user that have been retweeted by others.</para>
        /// </summary>
        public IEnumerable<TwitData> statuses_retweets_of_me(int count = -1, long since_id = -1, long max_id = -1,
                                                  bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweets_of_me.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweets_of_me)
        //-------------------------------------------------------------------------------
        #region +statuses_user_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/user_timelineメソッド</para>
        /// <para> 	 Returns the 20 most recent statuses posted by the authenticating user.</para>
        /// </summary>
        public IEnumerable<TwitData> statuses_user_timeline(long user_id = -1, string screen_name = "", int count = -1, long since_id = -1, long max_id = -1,
                                           bool trim_user = false, bool include_rts = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES,
                                           bool exclude_replies = false, bool contributor_details = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (exclude_replies) { paramdic.Add("exclude_replies", exclude_replies.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/user_timeline.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_user_timeline)
        //-------------------------------------------------------------------------------
        #region +statuses_retweeted_to_user
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/retweeted_to_user メソッド</para>
        /// <para>Returns the 20 most recent retweets posted by users the specified user follows.</para>
        /// </summary>
        /// <param name="id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_retweeted_to_user(long id = -1, string screen_name = "", int count = -1, long since_id = -1, long max_id = -1,
                                           bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (id > 0) { paramdic.Add("id", id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweeted_to_user.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweeted_to_user)
        //-------------------------------------------------------------------------------
        #region +statuses_retweeted_by_user
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/retweeted_by_user メソッド</para>
        /// <para>Returns the 20 most recent retweets posted by the specified user.</para>
        /// </summary>
        /// <param name="id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        public IEnumerable<TwitData> statuses_retweeted_by_user(long id = -1, string screen_name = "", int count = -1, long since_id = -1, long max_id = -1,
                                           bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (id > 0) { paramdic.Add("id", id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweeted_by_user.xml", GET, paramdic);

            return ConvertToTwitDataArray(GetByAPI(url));
        }
        #endregion (statuses_retweeted_by_user)
        //-------------------------------------------------------------------------------
        #endregion (Timelines)

        //-------------------------------------------------------------------------------
        #region Tweets Resources
        //-------------------------------------------------------------------------------
        #region +statuses_id_retweeted_by
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/id/retweeted_byメソッド</para>
        /// <para>Show user objects of up to 100 members who retweeted the status.</para>
        /// </summary>
        /// <param name="id">[required]</param>
        /// <remarks>trim_user/include_entitiesはAPIに公式でない？</remarks>
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
        /// <para>statuses/id/retweeted_by/idsメソッド </para>
        /// <para>Show user ids of up to 100 users who retweeted the status.</para>
        /// </summary>
        /// <param name="id">[required]</param>
        /// <remarks>stringify_idsオプションは不要？</remarks>
        public IEnumerable<long> statuses_id_retweeted_by_ids(long id, int count = -1, int page = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}statuses/{1}/retweeted_by/ids.xml", URLapi, id), GET, paramdic);
            XElement el = GetByAPI(url);

            var ids = from elem in el.Elements("id")
                      select long.Parse(elem.Value);
            return ids;
        }
        #endregion (statuses_id_retweeted_by_ids)
        //-------------------------------------------------------------------------------
        #region +statuses_retweets
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/retweetsメソッド</para>
        /// <para>Returns up to 100 of the first retweets of a given tweet.</para>
        /// </summary>
        /// <param name="id">[required]リツイートを見る発言のID</param>
        /// <param name="count">[option] &lt;100</param>
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
        #region +statuses_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/showメソッド</para>
        /// <para>Returns a single status, specified by the id parameter below.</para>
        /// </summary>
        /// <param name="withAuthParam">認証をつけてAPI呼び出しするかどうか</param>
        /// <param name="id">[required]取得する発言ID</param>
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
        #region +statuses_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/destroyメソッド</para>
        /// <para>Destroys the status specified by the required ID parameter.</para>
        /// </summary>
        /// <param name="id">[required]削除するID</param>
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
        /// <para>statuses/retweetメソッド</para>
        /// <para>Retweets a tweet.</para>
        /// </summary>
        /// <param name="id">[required]リツイート対象の発言ID</param>
        /// <remarks>403:update limit</remarks>
        public TwitData statuses_retweet(long id, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"statuses/retweet/" + id.ToString() + ".xml", POST, paramdic);
            return ConvertToTwitData(PostToAPI(url));
        }
        #endregion (statuses_retweet)
        //-------------------------------------------------------------------------------
        #region +statuses_update
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/updateメソッド</para>
        /// <para>Updates the authenticating user's status, also known as tweeting.</para>
        /// </summary>
        /// <param name="status">[required]発言内容</param>
        /// <param name="place_id">[option]GET geo/reverse_geocodeで取得できるID</param>
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
        #region +statuses_update_with_media
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/update_with_mediaメソッド</para>
        /// <para>Updates the authenticating user's status and attaches media for upload.</para>
        /// </summary>
        /// <returns></returns>
        public TwitData statuses_update_with_media(string status, Image image, string image_filename, bool possibly_sensitive = false, long in_reply_to_status_id = -1, 
                                            double latitude = double.NaN, double longtitude = double.NaN, string place_id = "", bool display_coordinates = false)
        {
            string contentType;
            Guid guid = image.RawFormat.Guid;
            if (guid.Equals(ImageFormat.Jpeg.Guid)) { contentType = "jpeg"; }
            else if (guid.Equals(ImageFormat.Png.Guid)) { contentType = "png"; }
            else if (guid.Equals(ImageFormat.Gif.Guid)) { contentType = "gif"; }
            else { throw new InvalidOperationException("画像がjpg,png,gif以外のフォーマットです"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("status", Utilization.UrlEncode(status));

                if (in_reply_to_status_id > 0) { paramdic.Add("in_reply_to_status_id", in_reply_to_status_id.ToString()); }
                if (possibly_sensitive) { paramdic.Add("possibly_sensitive", possibly_sensitive.ToString().ToLower()); }
                if (!double.IsNaN(latitude) && !double.IsNaN(longtitude)) {
                    paramdic.Add("lat", latitude.ToString());
                    paramdic.Add("long", longtitude.ToString());
                }
                if (!string.IsNullOrEmpty(place_id)) { paramdic.Add("place_id", place_id); }
                if (display_coordinates) { paramdic.Add("display_coordinates", display_coordinates.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapiUpload + @"statuses/update_with_media.xml", POST, paramdic);

            XElement el = PostImageToAPI(url, "media[]", image_filename, image, contentType);
            return ConvertToTwitData(el);
        }
        #endregion (statuses_update_with_media)
        //-------------------------------------------------------------------------------
        #endregion (Tweets)

        //-------------------------------------------------------------------------------
        #region Search Resources
        //-------------------------------------------------------------------------------
        #region +search
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>search メソッド</para>
        /// <para>Returns tweets that match a specified query.</para>
        /// </summary>
        /// <param name="q">[required]検索条件</param>
        /// <param name="lang">[unuse]ISO 639-1 code</param>
        /// <param name="locale">[option](only ja is currently effective)</param>
        /// <param name="rpp">[option] &lt;=100</param>
        /// <param name="page">[option] rpp * page &lt;=1500</param>
        /// <param name="max_id">[unuse]</param>
        /// <param name="since">[unuse]YYYY-MM-DD</param>
        /// <param name="until">[option]YYYY-MM-DD</param>
        /// <param name="geocode">[unuse]</param>
        /// <param name="show_user"></param>
        /// <param name="result_type">[recent/popular/mixed]</param>
        /// <returns></returns>
        /// <remarks>API Documentationにはsince,max_id無</remarks>
        public IEnumerable<TwitData> search(string q, string lang = "ja", string locale = "ja",
            int rpp = -1, int page = -1, long max_id = -1, long since_id = -1, string since = "",
            string until = "", object geocode = null, bool show_user = false, string result_type = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (string.IsNullOrEmpty(q)) { throw new ArgumentException("qかphraseのどちらかの引数は必須です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(q)) { paramdic.Add("q", Utilization.UrlEncode(q)); }
                if (!string.IsNullOrEmpty(lang)) { paramdic.Add("lang", lang); }
                if (!string.IsNullOrEmpty(locale)) { paramdic.Add("locale", locale); }
                if (rpp > 0) { paramdic.Add("rpp", rpp.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                //if (!string.IsNullOrEmpty(since)) { paramdic.Add("since", since); }
                if (!string.IsNullOrEmpty(until)) { paramdic.Add("until", until); }
                // geocode
                if (show_user) { paramdic.Add("show_user", show_user.ToString().ToLower()); }
                if (!string.IsNullOrEmpty(result_type)) { paramdic.Add("result_type", result_type); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLsearch + "search.json", GET, paramdic);

            XElement el = GetByAPIJson(url);

            IEnumerable<TwitData> data = ConvertToTwitDataSearch(el);
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
        #endregion (Search)

        //-------------------------------------------------------------------------------
        #region Direct Messages Resources
        //-------------------------------------------------------------------------------
        #region +direct_messages
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>direct_messagesメソッド</para>
        /// <para>Returns the 20 most recent direct messages sent to the authenticating user.</para>
        /// </summary>
        public IEnumerable<TwitData> direct_messages(long since_id = -1, long max_id = -1, int count = -1, int page = -1,
                                                     bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapiSSL + @"direct_messages.xml", GET, paramdic);

            return ConvertToTwitDataArrayDM(GetByAPI(url));
        }
        #endregion (direct_messages)
        //-------------------------------------------------------------------------------
        #region +direct_messages_sent
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>direct_messages/sentメソッド</para>
        /// <para>Returns the 20 most recent direct messages sent by the authenticating user.</para>
        /// </summary>
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

            string url = GetUrlWithOAuthParameters(URLapiSSL + @"direct_messages/sent.xml", GET, paramdic);

            return ConvertToTwitDataArrayDM(GetByAPI(url));
        }
        #endregion (direct_messages_sent)
        //-------------------------------------------------------------------------------
        #region +direct_messages_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>direct_messages/destroyメソッド</para>
        /// <para>Destroys the direct message specified in the required ID parameter.</para>
        /// </summary>
        /// <param name="id">削除先発言ID</param>
        public TwitData direct_messages_destroy(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapiSSL + @"direct_messages/destroy/" + id.ToString() + ".xml", POST, paramdic);
            return ConvertToTwitDataDM(PostToAPI(url));
        }
        #endregion (direct_messages_destroy)
        //-------------------------------------------------------------------------------
        #region +direct_messages_new
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>direct_messages/newメソッド</para>
        /// <para>Sends a new direct message to the specified user from the authenticating user.</para>
        /// </summary>
        /// <param name="screen_name">[select]送信先の名前</param>
        /// <param name="user_id">[select]送信先のユーザーID</param>
        /// <param name="text">送信テキスト</param>
        /// <param name="include_entities">[option]</param>
        public TwitData direct_messages_new(string screen_name, long user_id, string text, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (string.IsNullOrEmpty(screen_name) && user_id <= 0) { throw new ArgumentException("ScreenNameかUserIDの少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                paramdic.Add("text", Utilization.UrlEncode(text));
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapiSSL + @"direct_messages/new.xml", POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToTwitDataDM(el);
        }
        #endregion (direct_messages_new)
        //-------------------------------------------------------------------------------
        #region -direct_messages_id (Not Found?)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>direct_messages_idメソッド</para>
        /// <para>Returns a single direct message, specified by an id parameter.</para>
        /// </summary>
        /// <param name="id">[required]DirctMessageのID</param>
        /// <returns></returns>
        private TwitData direct_messages_id(long id)
        {
            string url = GetUrlWithOAuthParameters(string.Format(@"{0}direct_messages/{1}.xml", URLapiSSL, id), GET);

            return ConvertToTwitDataDM(GetByAPI(url));
        }
        #endregion (direct_messages_id)
        //-------------------------------------------------------------------------------
        #endregion (Direct Message)

        //-------------------------------------------------------------------------------
        #region Friends & Followers Resources
        //-------------------------------------------------------------------------------
        #region +followers_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>followers/ids メソッド</para>
        /// <para>Returns an array of numeric IDs for every user following the specified user.</para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>APIによるとAuth必要らしい？stringify_idsは不要？</remarks>
        public SequentData<long> followers_ids(bool withAuthParam, long user_id = -1, string screen_name = null, long cursor = -1)
        {
            if (!withAuthParam && user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
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
        #region +friends_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friends/ids メソッド</para>
        /// <para>Returns an array of numeric IDs for every user the specified user is following.</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        /// <remarks>stringify_idsは不要？</remarks>
        public SequentData<long> friends_ids(bool withAuthParam, long user_id = -1, string screen_name = null, long cursor = -1)
        {
            if (!withAuthParam && user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
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
        #region +friendships_exists
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/exists メソッド</para>
        /// <para>Test for the existence of friendship between two users.</para>
        /// <para>return [userA follows userB]?</para>
        /// </summary>
        /// <param name="withAuth">認証を含めるか</param>
        /// <param name="user_id_a">[select:A] userA ID</param>
        /// <param name="screen_name_a">[select:A] userA ScreenName</param>
        /// <param name="user_b_id">[select:B] userB ID</param>
        /// <param name="screen_name_b">[select:B] userB ScreenName</param>
        /// <returns>ユーザーA follows ユーザーB?</returns>
        public bool friendships_exists(bool withAuth, long user_id_a = -1, string screen_name_a = "",
                                                      long user_id_b = -1, string screen_name_b = "")
        {
            if (user_id_a <= 0 && string.IsNullOrEmpty(screen_name_a)) { throw new ArgumentException("ユーザーA：ユーザーIDかScreenNameの少なくとも1つは必要です。"); }
            if (user_id_b <= 0 && string.IsNullOrEmpty(screen_name_b)) { throw new ArgumentException("ユーザーB：ユーザーIDかScreenNameの少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id_a > 0) { paramdic.Add("user_id_a", user_id_a.ToString()); }
                if (!string.IsNullOrEmpty(screen_name_a)) { paramdic.Add("screen_name_a", screen_name_a); }
                if (user_id_b > 0) { paramdic.Add("user_id_b", user_id_b.ToString()); }
                if (!string.IsNullOrEmpty(screen_name_b)) { paramdic.Add("screen_name_b", screen_name_b); }
            }

            string urlbase = URLapi + @"friendships/exists.xml?";
            string url = (withAuth) ? GetUrlWithOAuthParameters(urlbase, GET, paramdic)
                                    : urlbase + '?' + JoinParameters(paramdic);
            XElement el = GetByAPI(url);

            return bool.Parse(el.Value);
        }
        #endregion (friendships_exists)
        //-------------------------------------------------------------------------------
        #region +friendships_incoming
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/incoming メソッド</para>
        /// <para>Returns an array of numeric IDs for every user who has a pending request to follow the authenticating user.</para>
        /// </summary>
        public SequentData<long> friendships_incoming(long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}friendships/incoming.xml", URLapi), GET, paramdic);
            XElement el = GetByAPI(url);

            var ids = from id in el.Element("ids").Elements("id")
                      select long.Parse(id.Value);

            return new SequentData<long>(ids,
               long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (friendships_incoming)
        //-------------------------------------------------------------------------------
        #region +friendships_outgoing
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>frinedships/outgoing メソッド</para>
        /// <para>Returns an array of numeric IDs for every protected user for whom the authenticating user has a pending follow request.</para>
        /// </summary>
        public SequentData<long> friendships_outgoing(long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("cursor", cursor.ToString());
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}friendships/outgoing.xml", URLapi), GET, paramdic);
            XElement el = GetByAPI(url);

            var ids = from id in el.Element("ids").Elements("id")
                      select long.Parse(id.Value);

            return new SequentData<long>(ids,
               long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (friendships_outgoing)
        //-------------------------------------------------------------------------------
        #region +friendships_show 2ユーザー間の情報確認
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/show メソッド</para>
        /// <para>Returns detailed information about the relationship between two users.</para>
        /// </summary>
        /// <param name="source_id">[option:1] subject user</param>
        /// <param name="source_screen_name">[option:1] subject user</param>
        /// <param name="target_id">[option:2] target user</param>
        /// <param name="target_screen_name">[option:2] target user</param>
        public RelationshipData friendships_show(long source_id = -1, string source_screen_name = null,
                                       long target_id = -1, string target_screen_name = null)
        {
            if (source_id <= 0 && string.IsNullOrEmpty(source_screen_name)) { throw new ArgumentException("対象ユーザー：ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            if (target_id <= 0 && string.IsNullOrEmpty(target_screen_name)) { throw new ArgumentException("ターゲットユーザー：ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (source_id > 0) { paramdic.Add("source_id", source_id.ToString()); }
                if (!string.IsNullOrEmpty(source_screen_name)) { paramdic.Add("source_screen_name", source_screen_name); }
                if (target_id > 0) { paramdic.Add("target_id", target_id.ToString()); }
                if (!string.IsNullOrEmpty(target_screen_name)) { paramdic.Add("target_screen_name", target_screen_name); }
            }

            string urlbase = URLapi + @"friendships/show.xml";
            string url = urlbase + '?' + JoinParameters(paramdic);

            XElement el = GetByAPI(url);

            return ConvertToRelationshipData(el);
        }
        #endregion (friendships_show)
        //-------------------------------------------------------------------------------
        #region +friendships_create フォロー
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/create メソッド</para>
        /// <para>Allows the authenticating users to follow the user specified in the ID parameter.</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="follow">notificationを有効にするか[option]</param>
        /// <returns></returns>
        /// <remarks>include_entitiesはAPI Documentationには無し</remarks>
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
        #region +friendships_destroy フォロー解除
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/destroy メソッド</para>
        /// <para>Allows the authenticating users to unfollow the user specified in the ID parameter.</para>
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
        #region +friendships_lookup
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships_lookup メソッド</para>
        /// <para>Returns the relationship of the authenticating user to the comma separated list of up to 100 screen_names or user_ids provided.</para>
        /// </summary>
        public IEnumerable<FriendshipData> friendships_lookup(long[] user_ids = null, string[] screen_names = null)
        {
            if ((user_ids == null || user_ids.Length == 0) && (screen_names == null || screen_names.Length == 0)) {
                throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if ((user_ids != null && user_ids.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_ids, false)); }
                if ((screen_names != null && screen_names.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_names, false)); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"friendships/lookup.xml", GET, paramdic);
            XElement el = GetByAPI(url);

            return ConvertToFriendShipDataArray(el);
        }
        #endregion (friendships_lookup)
        //-------------------------------------------------------------------------------
        #region +friendships_update
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/update メソッド</para>
        /// <para>Allows one to enable or disable retweets and device notifications from the specified user.</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <param name="device">[option]Enable/disable device notifications from the target user.</param>
        /// <param name="retweets">[option]Enable/disable device notifications from the target user.</param>
        /// <returns></returns>
        public RelationshipData friendships_update(long user_id = -1, string screen_name = "", bool? device = null, bool? retweets = null)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            if (!device.HasValue && !retweets.HasValue) { throw new ArgumentException("deviceかretweetsの少なくとも1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (device.HasValue) { paramdic.Add("device", device.Value.ToString().ToLower()); }
                if (retweets.HasValue) { paramdic.Add("retweets", retweets.Value.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"friendships/update.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToRelationshipData(el);
        }
        #endregion (friendships_update)
        //-------------------------------------------------------------------------------
        #region +friendships_no_retweet_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>friendships/no_retweet_ids メソッド</para>
        /// <para>Returns an array of user_ids that the currently authenticated user does not want to see retweets from.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<long> friendships_no_retweet_ids()
        {
            string url = GetUrlWithOAuthParameters(string.Format("{0}friendships/no_retweet_ids.xml", URLapi), GET);
            XElement el = GetByAPI(url);

            var enumid = el.Elements("id");
            return from id in enumid
                   select long.Parse(id.Value);
        }
        #endregion (friendships_no_retweet_ids)
        //-------------------------------------------------------------------------------
        #endregion (Friends & Followers)

        //-------------------------------------------------------------------------------
        #region Users Resources
        //-------------------------------------------------------------------------------
        #region +users_lookup
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>users/lookup メソッド</para>
        /// <para>Return up to 100 users worth of extended information, specified by either ID, screen name, or combination of the two.</para>
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
        #region +users_profile_image
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
        /// <para>users/profile_image メソッド</para>
        /// <para>Access the profile image in various sizes for the user with the indicated screen_name</para>
        /// </summary>
        /// <param name="screen_name">[required]ユーザー名</param>
        /// <param name="size">[option]画像サイズ</param>
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
        #region +users_search
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>users/search メソッド</para>
        /// <para>Runs a search for users similar to Find People button on Twitter.com.</para>
        /// </summary>
        /// <param name="q">[required]</param>
        /// <param name="page">[option]</param>
        /// <param name="per_page">[option] 20以下</param>
        /// <returns></returns>
        /// <remarks>Only the first 1000 matches are available.</remarks>
        public IEnumerable<UserProfile> users_search(string q, int page = -1, int per_page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("q", q);
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (per_page > 0) { paramdic.Add("per_page", per_page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}users/search.xml", URLapi), GET, paramdic);
            return ConvertToUserProfileArray(GetByAPI(url));
        }
        #endregion (users_search)
        //-------------------------------------------------------------------------------
        #region +users_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>users/show メソッド</para>
        /// <para>Returns extended information of a given user, specified by ID or screen name as per the required id parameter.</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
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
        // users/contributees
        // users/contributors
        //-------------------------------------------------------------------------------
        #endregion (Users)

        //-------------------------------------------------------------------------------
        #region Suggested Users
        //-------------------------------------------------------------------------------
        #region users_suggestions
        //-------------------------------------------------------------------------------
        /// <summary>
        /// Access to Twitter's suggested user list. This returns the list of suggested user categories.
        /// </summary>
        /// <param name="lang"></param>
        public IEnumerable<SuggestionCategoryData> users_suggestions(string lang = "ja")
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(lang)) { paramdic.Add("lang", lang); }
            }

            string tail = (paramdic.Count != 0) ? '?' + JoinParameters(paramdic) : "";

            string url = URLapi + @"users/suggestions.xml" + tail;
            return ConvertToSuggestionCategoryDataArray(GetByAPI(url));
        }
        #endregion (users_suggestions)
        //-------------------------------------------------------------------------------
        #region users_suggestions_slug
        //-------------------------------------------------------------------------------
        //
        public IEnumerable<UserProfile> users_suggestions_slug(string slug, string lang = "ja")
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(lang)) { paramdic.Add("lang", lang); }
            }
            string tail = (paramdic.Count != 0) ? '?' + JoinParameters(paramdic) : "";

            string url = string.Format(@"{0}users/suggestions/{1}.xml{2}", URLapi, Utilization.UrlEncode(slug), tail);
            XElement el = GetByAPI(url);
            return ConvertToUserProfileArray(el.Element("users"));
        }
        #endregion (users_suggestions_slug)
        //-------------------------------------------------------------------------------
        #region users_suggestions_slug_members
        //-------------------------------------------------------------------------------
        //
        public IEnumerable<UserProfile> users_suggestions_slug_members(string slug)
        {
            string url = string.Format(@"{0}users/suggestions/{1}/members.xml", URLapi, Utilization.UrlEncode(slug));
            return ConvertToUserProfileArray(GetByAPI(url));
        }
        #endregion (users_suggestions_slug_members)
        //-------------------------------------------------------------------------------
        #endregion (Suggested Users)

        //-------------------------------------------------------------------------------
        #region Favorites Resources
        //-------------------------------------------------------------------------------
        #region +favorites
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>favorites メソッド(お気に入り取得)</para>
        /// <para>Returns the 20 most recent favorite statuses for the authenticating user or user specified by the ID parameter in the requested format.</para>
        /// </summary>
        /// <param name="screen_name">[select]user_idより優先</param>
        /// <param name="user_id">[select]</param>
        /// <remarks>since_idとskip_statusはAPI Documentationにない</remarks>
        public IEnumerable<TwitData> favorites(string screen_name = "", long user_id = -1, int page = -1, long since_id = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("id", screen_name); }
                else if (user_id >= 0) { paramdic.Add("id", user_id.ToString()); }
                if (page >= 0) { paramdic.Add("page", page.ToString()); }
                if (since_id >= 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"favorites.xml", GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToTwitDataArray(el);
        }
        #endregion (favorites)
        //-------------------------------------------------------------------------------
        #region +favorites_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>favorites_create メソッド</para>
        /// <para>Favorites the status specified in the ID parameter as the authenticating user.</para>
        /// </summary>
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
        #region +favorites_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>favorites_destroy メソッド(お気に入り削除)</para>
        /// <para>Un-favorites the status specified in the ID parameter as the authenticating user.</para>
        /// </summary>
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
        #endregion (Favorites)

        //-------------------------------------------------------------------------------
        #region Lists Resources
        //-------------------------------------------------------------------------------
        #region +list_all
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/all</para>
        /// <para>Returns all lists the authenticating or specified user subscribes to, including their own.</para>
        /// </summary>
        /// <param name="screen_name">[option]双方ない時は認証ユーザー</param>
        /// <param name="user_id">[option]双方ない時は認証ユーザー</param>
        /// <returns></returns>
        public IEnumerable<ListData> list_all(string screen_name = "", long user_id = -1)
        {
            if (user_id <= 0 && string.IsNullOrEmpty(screen_name)) { AssertAuthenticated(); } // 認証確認

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/all.xml", URLapi), GET, paramdic);
            return ConvertToListDataArray(GetByAPI(url));
        }
        #endregion (list_all)
        //-------------------------------------------------------------------------------
        #region +lists_statuses
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/statuses</para>
        /// <para>Returns tweet timeline for members of the specified list.</para>
        /// <para></para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="owner_screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public IEnumerable<TwitData> lists_statuses(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1,
            long since_id = -1, long max_id = -1, int per_page = -1, int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool include_rts = false)
        {
            if (string.IsNullOrEmpty(owner_screen_name) && owner_id < 0) { AssertAuthenticated(); } // 認証確認

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (per_page > 0) { paramdic.Add("per_page", per_page.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/statuses.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToTwitDataArray(el);
        }
        #endregion (lists_statuses)
        //-------------------------------------------------------------------------------
        #region +lists_memberships
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists_memberships メソッド</para>
        /// <para>Returns the lists the specified user has been added to.</para>
        /// </summary>
        /// <param name="user_id">[select option]追加されているリストを調べるユーザーID。両方なければ認証ユーザー</param>
        /// <param name="screen_name">[select option]追加されているリストを調べるユーザー名。両方なければ認証ユーザー</param>
        /// <param name="filter_to_owner_lists">[option]trueに設定すると認証ユーザーの所有するリストのみ</param>
        public SequentData<ListData> lists_memberships(long user_id = -1, string screen_name = "", long cursor = -1, bool filter_to_owner_lists = false)
        {
            if (((user_id <= 0 && string.IsNullOrEmpty(screen_name)) || filter_to_owner_lists)) { AssertAuthenticated(); } // 認証確認

            string user = (user_id <= 0 && string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(user)) { paramdic.Add("screen_name", user); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                paramdic.Add("cursor", cursor.ToString());
                if (filter_to_owner_lists) { paramdic.Add("filter_to_owned_lists", filter_to_owner_lists.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/memberships.xml", URLapi), GET, paramdic);
            XElement el = GetByAPI(url);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")),
                long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_memberships)
        //-------------------------------------------------------------------------------
        #region +lists_subscriptions
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists_subscriptionsメソッド</para>
        /// <para>Obtain a collection of the lists the specified user is subscribed to, 20 lists per page by default.</para>
        /// </summary>
        /// <param name="screen_name">フォローしているリストを調べるユーザー名</param>
        public SequentData<ListData> lists_subscriptions(long user_id = -1, string screen_name = "", int count = -1, long cursor = -1)
        {
            if (user_id <= 0 && string.IsNullOrEmpty(screen_name)) { AssertAuthenticated(); } // 認証確認

            string user = (user_id <= 0 && string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (!string.IsNullOrEmpty(user)) { paramdic.Add("screen_name", user); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                paramdic.Add("cursor", cursor.ToString());
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}/lists/subscriptions.xml", URLapi), GET, paramdic);
            XElement el = GetByAPI(url);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")),
                long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_subscriptions)
        //===============================================================================
        #region +list_subscribers
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers メソッド</para> 
        /// <para>Returns the subscribers of the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="owner_screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <param name="cursor">[option]データベース上のカーソル</param>
        /// <returns></returns>
        public SequentData<UserProfile> list_subscribers(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1,
                                                         long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/subscribers.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")),
                    long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (list_subscribers_Get)
        //-------------------------------------------------------------------------------
        #region +list_subscribers_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers/create メソッド</para>
        /// <para>Subscribes the authenticated user to the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="owner_screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public ListData list_subscribers_create(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1)
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
            }
            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/subscribers/create.xml", URLapi), POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_subscribers_create)
        //-------------------------------------------------------------------------------
        #region +list_subscribers_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers/show メソッド</para>
        /// <para>Check if the specified user is a member of the specified list.</para>
        /// <para>見つからなければ404エラーを返す</para>
        /// </summary>
        /// <param name="user_id">[select1]削除するユーザーのID</param>
        /// <param name="screen_name">[select1]削除するユーザーのScreenName</param>
        /// <param name="list_id">[select2]リストID</param>
        /// <param name="slug">[select2]リストのslug</param>
        /// <param name="owner_screen_name">[select2 option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select2 option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public UserProfile list_subscribers_show(long user_id = -1, string screen_name = "",
                                                 long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1,
                                                 bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/subscribers/show.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (list_subscribers_show)
        //-------------------------------------------------------------------------------
        #region +list_subscribers_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/subscribers/destroy メソッド</para>
        /// <para>Unsubscribes the authenticated user from the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="owner_screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public ListData list_subscribers_destroy(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1)
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/subscribers/destroy.xml", URLapi), POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_subscribers_destroy)
        //===============================================================================
        #region +list_create_all
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/create_all メソッド</para>
        /// <para>Adds multiple members to a list, by specifying a comma-separated list of member ids or screen names.</para>
        /// <para>最大100人まで一斉に追加可能</para>
        /// </summary>
        /// <param name="list_id">[select1]リストID</param>
        /// <param name="slug">[select1]リストのslug</param>
        /// <param name="user_ids">[select2]</param>
        /// <param name="screen_names">[select2]</param>
        /// <remarks>リストの最大人数は500．</remarks>
        /// <returns></returns>
        public object list_create_all(long list_id = -1, string slug = "", long[] user_ids = null, string[] screen_names = null)
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 && string.IsNullOrEmpty(slug)) { throw new ArgumentException("リストの特定に必要な情報が足りません。"); }
            if ((user_ids == null || user_ids.Length == 0) && (screen_names == null || screen_names.Length == 0)) {
                throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) {
                    paramdic.Add("slug", slug);
                    paramdic.Add("owner_id", ID.ToString());
                }
                if ((user_ids != null && user_ids.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_ids, false)); }
                if ((screen_names != null && screen_names.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_names, false)); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/members/create_all.xml", URLapi), POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_create_all)
        //-------------------------------------------------------------------------------
        #region +list_members_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/member メソッド</para>
        /// <para>Check if the specified user is a member of the specified list.</para>
        /// <para>見つからなければ404エラーを返す</para>
        /// </summary>
        /// <param name="user_id">[select1]削除するユーザーのID</param>
        /// <param name="screen_name">[select1]削除するユーザーのScreenName</param>
        /// <param name="list_id">[select2]リストID</param>
        /// <param name="slug">[select2]リストのslug</param>
        /// <param name="owner_screen_name">[select2 option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select2 option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public UserProfile list_members_show(long user_id = -1, string screen_name = "",
                                             long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1,
                                             bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/members/show.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (list_members_show)
        //-------------------------------------------------------------------------------
        #region +list_members
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/members メソッド</para> 
        /// <para>Returns the members of the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="owner_screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <param name="cursor">[option]データベース上のカーソル</param>
        /// <returns></returns>
        public SequentData<UserProfile> list_members(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1,
                                                         long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                paramdic.Add("cursor", cursor.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/members.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")),
                    long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (list_members)
        //-------------------------------------------------------------------------------
        #region +list_members_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/members/create メソッド</para>
        /// <para>Add a member to a list.</para>
        /// </summary>
        /// <param name="list_id">[select1]リストID</param>
        /// <param name="slug">[select1]リストのslug</param>
        /// <param name="user_id">[select2]削除するユーザーのID</param>
        /// <param name="screen_name">[select2]削除するユーザーのScreenName</param>
        /// <returns></returns>
        public ListData list_members_create(long list_id = -1, string slug = "", long user_id = -1, string screen_name = "")
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 && string.IsNullOrEmpty(slug)) { throw new ArgumentException("リストの特定に必要な情報が足りません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) {
                    paramdic.Add("slug", slug);
                    paramdic.Add("owner_id", ID.ToString());
                }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/members/create.xml", URLapi), POST, paramdic);

            XElement el = PostToAPI(url);
            return ConvertToListData(el);
        }
        #endregion (list_members_create)
        //-------------------------------------------------------------------------------
        #region +list_members_destory
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>list/members/destroy メソッド</para>
        /// <para>Removes the specified member from the list</para>
        /// </summary>
        /// <param name="list_id">[select1]リストID</param>
        /// <param name="slug">[select1]リストのslug</param>
        /// <param name="user_id">[select2]削除するユーザーのID</param>
        /// <param name="screen_name">[select2]削除するユーザーのScreenName</param>
        /// <returns></returns>
        public ListData list_members_destroy(long list_id = -1, string slug = "", long user_id = -1, string screen_name = "")
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 && string.IsNullOrEmpty(slug)) { throw new ArgumentException("リストの特定に必要な情報が足りません。"); }

            if (string.IsNullOrEmpty(ScreenName)) { throw new InvalidOperationException("認証されていません。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) {
                    paramdic.Add("slug", slug);
                    paramdic.Add("owner_id", ID.ToString());
                }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists/members/destroy.xml", URLapi), POST, paramdic);

            return ConvertToListData(PostToAPI(url));
        }
        #endregion (list_members_destroy)
        //===============================================================================
        #region +lists_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/destroy メソッド</para>
        /// <para>Deletes the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <returns></returns>
        /// <remarks>owner_screen_name or owner_idは必要?</remarks>
        public ListData lists_destroy(long list_id = -1, string slug = "")
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 && string.IsNullOrEmpty(slug)) { throw new ArgumentException("リストIDかslugのどちらか1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) {
                    paramdic.Add("slug", slug);
                    paramdic.Add("owner_id", ID.ToString());
                }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/destroy.xml", URLapiSSL), POST, paramdic);

            return ConvertToListData(PostToAPI(url));
        }
        #endregion (lists_destroy)
        //-------------------------------------------------------------------------------
        #region +lists_update
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/update メソッド</para>
        /// <para>Updates the specified list.</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="name">[option]リストの新しい名前</param>
        /// <param name="isPrivate">[option]privateにする時にtrue</param>
        /// <param name="description">[option]リストの説明</param>
        /// <returns>変更前が返る？/owner_screen_name or owner_idは必要?</returns>
        public ListData lists_update(long list_id = -1, string slug = "",
                                     string name = null, bool isPrivate = false, string description = null)
        {
            AssertAuthenticated(); // 認証確認

            if (list_id <= 0 && string.IsNullOrEmpty(slug)) { throw new ArgumentException("リストIDかslugのどちらか1つは必要です。"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) {
                    paramdic.Add("slug", slug);
                    paramdic.Add("owner_id", ID.ToString());
                }
                if (!string.IsNullOrEmpty(name)) { paramdic.Add("name", Utilization.UrlEncode(name)); }
                paramdic.Add("mode", (isPrivate) ? "private" : "public");
                if (!string.IsNullOrEmpty(description)) { paramdic.Add("description", Utilization.UrlEncode(description)); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/update.xml", URLapi), POST, paramdic);
            return ConvertToListData(PostToAPI(url));
        }
        #endregion (lists_update)
        //-------------------------------------------------------------------------------
        #region +lists_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/create メソッド</para>
        /// <para>Creates a new list for the authenticated user</para>
        /// </summary>
        /// <param name="name">リストの名前</param>
        /// <param name="isPrivate">[option]privateにする時にtrue</param>
        /// <param name="description">[option]リストの説明</param>
        /// <returns></returns>
        public ListData lists_create(string name, bool isPrivate = false, string description = null)
        {
            AssertAuthenticated(); // 認証確認

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("name", Utilization.UrlEncode(name));
                if (isPrivate) { paramdic.Add("mode", "private"); }
                if (!string.IsNullOrEmpty(description)) { paramdic.Add("description", Utilization.UrlEncode(description)); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/create.xml", URLapi), POST, paramdic);
            return ConvertToListData(PostToAPI(url));
        }
        #endregion (lists_create)
        //-------------------------------------------------------------------------------
        #region +lists
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists メソッド</para>
        /// <para>Returns the lists of the specified (or authenticated) user.</para>
        /// </summary>
        /// <param name="user_id">[select]リストの所有者のUserID。両方なければ認証ユーザー</param>
        /// <param name="screen_name">[select]リストの所有者のScreenName。両方なければ認証ユーザー</param>
        /// <param name="cursor">[option]データベース上のカーソル</param>
        /// <returns></returns>
        public SequentData<ListData> lists(long user_id = -1, string screen_name = "", long cursor = -1)
        {
            if (user_id <= 0 && string.IsNullOrEmpty(screen_name)) { AssertAuthenticated(); } // 認証確認 

            string user = (user_id <= 0 && string.IsNullOrEmpty(screen_name)) ? ScreenName : screen_name;
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(user)) { paramdic.Add("screen_name", user); }
                paramdic.Add("cursor", cursor.ToString());
            }

            string url = GetUrlWithOAuthParameters(string.Format("{0}lists.xml", URLapi), GET, paramdic);

            XElement el = GetByAPI(url);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")),
                                             long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }
        #endregion (lists_Get)
        //-------------------------------------------------------------------------------
        #region +lists_show
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>lists/show メソッド</para>
        /// <para>Returns the specified list</para>
        /// </summary>
        /// <param name="list_id">[select]リストID</param>
        /// <param name="slug">[select]リストのslug</param>
        /// <param name="screen_name">[select option]slugを指定する場合にowner_idかどちらかが必要．リストの作成者のScreenName</param>
        /// <param name="owner_id">[select option]slugを指定する場合にscreen_nameかどちらかが必要．リストの作成者のUserID</param>
        /// <returns></returns>
        public ListData lists_show(long list_id = -1, string slug = "", string owner_screen_name = "", long owner_id = -1)
        {
            if (list_id <= 0 &&
                (string.IsNullOrEmpty(slug) ||
                 (!string.IsNullOrEmpty(slug) && owner_id <= 0 && string.IsNullOrEmpty(owner_screen_name)))) {
                throw new ArgumentException("リストの特定に必要な情報が足りません。");
            }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (!string.IsNullOrEmpty(slug)) { paramdic.Add("slug", slug); }
                if (!string.IsNullOrEmpty(owner_screen_name)) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
            }

            string url = GetUrlWithOAuthParameters(string.Format(@"{0}lists/show.xml", URLapiSSL), GET, paramdic);
            return ConvertToListData(GetByAPI(url));
        }
        #endregion (lists_Show)
        //-------------------------------------------------------------------------------
        #endregion (Lists)

        //-------------------------------------------------------------------------------
        #region Accounts Resources
        //-------------------------------------------------------------------------------
        #region +account_rate_limit_status
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>account/rate_limit_statusメソッド</para>
        /// <para>Returns the remaining number of API requests available to the requesting user before the API limit is reached for the current hour.</para>
        /// </summary>
        /// <param name="withAuth">認証を行うかどうか。認証しない場合はIP依存のデータが返る</param>
        /// <returns>残数データ</returns>
        public APILimitData account_rate_limit_status(bool withAuth)
        {
            string urlbase = URLapi + @"account/rate_limit_status.xml";
            string url = (withAuth) ? GetUrlWithOAuthParameters(urlbase, GET)
                                    : urlbase;

            XElement el = GetByAPI(url);
            APILimitData data = ConvertToAPILimitData(el);

            API_Max = data.HourlyLimit;
            API_Rest = data.Remaining;

            return data;
        }
        #endregion (account_rate_limit_status)
        // account/verify_credentials
        // account/end_session
        // account/update_delivery_device
        //-------------------------------------------------------------------------------
        #region +account_update_profile プロフィール更新
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>account_update_profileメソッド</para>
        /// <para>Sets values that users are able to set under the "Account" tab of their settings page</para>
        /// </summary>
        /// <param name="name">[select at least one]</param>
        /// <param name="url">[select at least one]</param>
        /// <param name="location">[select at least one]</param>
        /// <param name="description">[select at least one]</param>
        /// <returns></returns>
        public UserProfile account_update_profile(string name = null, string url = null, string location = null, string description = null,
                                                  bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
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
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_post = GetUrlWithOAuthParameters(URLapi + @"account/update_profile.xml", POST, paramdic);

            XElement el = PostToAPI(url_post);
            return ConvertToUserProfile(el);
        }
        #endregion (account_update_profile)
        // account/update_profile_background_image
        // account/update_profile_colors
        //-------------------------------------------------------------------------------
        #region +account_update_profile_image 画像更新
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>account/update_profile_image メソッド</para>　
        /// <para>Updates the authenticating user's profile image.</para>
        /// <para>返り値のUserProfileではURLが反映されてない可能性があるので，最低5秒待ってから取得する。</para>
        /// </summary>
        /// <param name="imgFileName">画像ファイルパス</param>
        /// <param name="image">画像</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        public UserProfile account_update_profile_image(string imgFileName, Image image, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            string contentType;
            Guid guid = image.RawFormat.Guid;
            if (guid.Equals(ImageFormat.Jpeg.Guid)) { contentType = "jpeg"; }
            else if (guid.Equals(ImageFormat.Png.Guid)) { contentType = "png"; }
            else if (guid.Equals(ImageFormat.Gif.Guid)) { contentType = "gif"; }
            else { throw new InvalidOperationException("画像がjpg,png,gif以外のフォーマットです"); }

            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"account/update_profile_image.xml", POST, paramdic);

            return ConvertToUserProfile(PostImageToAPI(url, "image", imgFileName, image, contentType));
        }
        #endregion (account_update_profile_image)
        // account/totals
        // account/settings(GET)
        // account/settings(POST)
        //-------------------------------------------------------------------------------
        #endregion (Accounts)

        //-------------------------------------------------------------------------------
        #region Block Resources
        //-------------------------------------------------------------------------------
        #region +blocks_blocking
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>blocks/blocking メソッド</para>
        /// <para>Returns an array of user objects that the authenticating user is blocking.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserProfile> blocks_blocking(int page = -1, int per_page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (per_page > 0) { paramdic.Add("per_page", per_page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/blocking.xml", GET, paramdic);
            return ConvertToUserProfileArray(GetByAPI(url, true));
        }
        #endregion (blocks_blocking)
        //-------------------------------------------------------------------------------
        #region +blocks_blocking_ids
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>blocks/blocking/ids メソッド</para>
        /// <para>Returns an array of numeric user ids the authenticating user is blocking.</para>
        /// </summary>
        /// <remarks>stringify_idsは不要?</remarks>
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
        #region -blocks_exists (未実装)
        //-------------------------------------------------------------------------------
        //
        private void blocks_exists()
        {
            throw new NotImplementedException();
        }
        #endregion (blocks_exists)
        //-------------------------------------------------------------------------------
        #region +blocks_create
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>blocks/create メソッド</para>
        /// <para>Blocks the specified user from following the authenticating user</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        public UserProfile blocks_create(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/create.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (blocks_create)
        //-------------------------------------------------------------------------------
        #region +blocks_destroy
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>blocks/destroy メソッド</para>
        /// <para>Un-blocks the user specified in the ID parameter for the authenticating user</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        public UserProfile blocks_destroy(long user_id = -1, string screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"blocks/destroy.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (blocks_destroy)
        //-------------------------------------------------------------------------------
        #endregion (Block)]

        //-------------------------------------------------------------------------------
        #region Spam Reporting Resources
        //-------------------------------------------------------------------------------
        #region +report_spam
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>report_spam メソッド</para>
        /// <para>The user specified in the id is blocked by the authenticated user and reported as a spammer.</para>
        /// </summary>
        /// <param name="user_id">[select]</param>
        /// <param name="screen_name">[select]</param>
        /// <returns></returns>
        public UserProfile report_spam(long user_id = -1, string screen_name = null)
        {
            if (user_id == -1 && string.IsNullOrEmpty(screen_name)) { throw new ArgumentException("ユーザーIDかスクリーン名の少なくとも1つは必要です。"); }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id != -1) { paramdic.Add("user_id", user_id.ToString()); }
                if (!string.IsNullOrEmpty(screen_name)) { paramdic.Add("screen_name", screen_name); }
            }

            string url = GetUrlWithOAuthParameters(URLapi + @"report_spam.xml", POST, paramdic);
            XElement el = PostToAPI(url);
            return ConvertToUserProfile(el);
        }
        #endregion (report_spam)
        //-------------------------------------------------------------------------------
        #endregion (Spam Reporting Resources)

        //-------------------------------------------------------------------------------
        #region OAuth Resources
        //-------------------------------------------------------------------------------
        #region +oauth_request_token
        //-------------------------------------------------------------------------------
        /// <summary>
        /// request_tokenを返します。
        /// </summary>
        /// <param name="request_token_Secret"></param>
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        /// <returns></returns>
        public string oauth_request_token(out string request_token_Secret)
        {
            string url = URLapiSSLnoVer + "oauth/request_token";

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
        #region +oauth_authorize_URL
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザーが認証するためのURLを返します。
        /// </summary>
        /// <param name="strRequestToken"></param>
        /// <returns></returns>
        public string oauth_authorize_URL(string strRequestToken)
        {
            return URLapiSSLnoVer + "oauth/authorize?oauth_token=" + strRequestToken;
        }
        //-------------------------------------------------------------------------------
        #endregion (oauth_authorize)
        //-------------------------------------------------------------------------------
        #region +oauth_access_token
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 最終認証を行います。正式なoauth_tokenが返ります。
        /// </summary>
        /// <param name="pin">ユーザーが貰ったキー</param>
        /// <param name="reqToken">request_token</param>
        /// <param name="reqTokenSecret">request_token_secret</param>
        /// <param name="access_token_secret">正式なoauth_token_secret</param>
        public UserAuthInfo oauth_access_token(string pin, string reqToken, string reqTokenSecret)
        {
            string url = URLapiSSLnoVer + "oauth/access_token";

            SortedDictionary<string, string> parameters = GenerateParameters(reqToken);
            parameters.Add("oauth_verifier", pin);
            string signature = GenerateSignature(reqTokenSecret, GET, url, parameters);
            parameters.Add("oauth_signature", Utilization.UrlEncode(signature));
            string response = HttpGet(url, parameters);
            Dictionary<string, string> dic = ParseResponse(response);

            UserAuthInfo userdata = new UserAuthInfo() {
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
        #endregion (OAuth)

        //-------------------------------------------------------------------------------
        #region Deprecated Resources
        //-------------------------------------------------------------------------------
        #region +statuses_public_timeline
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>statuses/public_timelineメソッド</para>
        /// <para>Returns the 20 most recent statuses, including retweets if they exist, from non-protected users.</para>
        /// <para>The public timeline is cached for 60 seconds</para>
        /// </summary>
        [Obsolete("streamingAPIを使用してください。")]
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
        [Obsolete("statuses/home_timelineを使用してください。")]
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
        #region +statuses_friends
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォローしている人を返します。返り値：(ユーザーリスト，next_cursor, previous_cursor）
        /// </summary>
        /// <param name="user_id">[option]</param>
        /// <param name="screen_name">[option]</param>
        /// <param name="cursor">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        [Obsolete("follower/idsとusers/lookupを使用してください。")]
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
        #region +statuses_followers
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォローされている人を返します。返り値：(ユーザーリスト，next_cursor, previous_cursor）
        /// </summary>
        /// <param name="user_id">[option]</param>
        /// <param name="screen_name">[option]</param>
        /// <param name="cursor">[option]</param>
        /// <param name="include_entities">[option]</param>
        /// <returns></returns>
        [Obsolete("friends/idsとusers/lookupを使用してください。")]
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
        #endregion (Deprecated)

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
                            sr.ReadToEnd();
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
        public CancellationTokenSource userstream_user(bool all_replies, Action<UserStreamItemType, object> action, Action endact = null, Action connectdact = null, Action<bool, int> erroract = null, int reconnect_wait_time = 0)
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
                #region ストリーミングスレッド
                //-----------------------------------------------------------------
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); // User-Agent?
                req.ReadWriteTimeout = 90000;

                Encoding enc = Encoding.UTF8;

                HttpWebResponse res;
                bool error_caused = false;
                try {
                    res = (HttpWebResponse)req.GetResponse();
                    if (connectdact != null) { connectdact(); }
                    reconnect_wait_time = 0; // 接続できたらwait_time <- 0

                    byte[] b = new byte[0x4000];            // 16KBの受信バッファ
                    StringBuilder sb = new StringBuilder(); // 受信文字列バッファ

                    const string NEWLINE = "\r\n";
                    // データ受信時コールバック
                    bool canceled = false;
                    AsyncCallback callback = null;

                    callback = ar =>
                    {
                        #region データ受信時コールバック
                        //-----------------------------------------------
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

                                        if (item.Item1 == UserStreamItemType.disconnect) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            else { canceled = true; }　// 接続きれた
                        }
                        catch (WebException ex) {
                            if (ex.Status == WebExceptionStatus.RequestCanceled) { return; } // キャンセルした時
                            //Message.ShowInfoMessage("WebException");
                            error_caused = true;
                        }
                        catch (IOException) {
                            //Message.ShowInfoMessage("IOException");
                            error_caused = true;
                        }
                    //-----------------------------------------------
                    #endregion データ受信時コールバック
                    };

                    Stream resStream = res.GetResponseStream();
                    resStream.BeginRead(b, 0, b.Length, callback, resStream);

                    while (true) { // キャンセル確認ループ
                        if (token.IsCancellationRequested) {
                            req.Abort();
                            canceled = true;
                            break;
                        }
                        else if (canceled || error_caused) { break; }
                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex) {
                    error_caused = true;
                    if (!(ex is WebException) && !(ex is IOException)) {
                        Log.DebugLog(ex);
                    }
                }

                if (error_caused) {
                    int reconnect_time = (reconnect_wait_time == 0) ? 1 : reconnect_wait_time * 2;
                    if (erroract != null) { erroract(all_replies, reconnect_time); }
                    return;
                }
                else if (endact != null) { endact(); }
                //-----------------------------------------------------------------
                #endregion ストリーミングスレッド
            };

            Thread thread = new Thread(ReadStreaming);
            thread.IsBackground = true;
            thread.Start();

            return cts;
        }
        #endregion (userstream_user)

        //===============================================================================
        #region +IsAuthenticated 認証済みかどうか
        //-------------------------------------------------------------------------------
        //
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(ScreenName);
        }
        #endregion (IsAuthenticated)

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

            // XML以外が帰ってきた時はエラー
            if (!res.ContentType.Contains(XML)) {
                throw new TwitterAPIException(1000, "Xmlデータ以外のデータを受信しました。");
            }

            using (Stream resStream = res.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8)) {
                    string s = "";
                    try {
                        s = reader.ReadToEnd();

                        MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(s));
                        return XElement.Load(m);
                    }
                    catch (XmlException ex) {
                        Log.DebugLog(ex);
                        Log.DebugLog(s);
                        throw new TwitterAPIException(1000, ex.Message);
                    }
                    catch (WebException ex) {
                        throw new TwitterAPIException(1, ex.Message);
                    }
                    catch (IOException ex) {
                        throw new TwitterAPIException(1, ex.Message);
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

            // JSON以外が帰ってきた時はエラー
            if (!res.ContentType.Contains(JSON)) {
                throw new TwitterAPIException(1000, "Jsonデータ以外のデータを受信しました。");
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
                    catch (WebException ex) {
                        throw new TwitterAPIException(1, ex.Message);
                    }
                    catch (IOException ex) {
                        throw new TwitterAPIException(1, ex.Message);
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
                try {
                    img = Image.FromStream(s);
                }
                catch (WebException ex) {
                    throw new TwitterAPIException(1, ex.Message);
                }
                catch (IOException ex) {
                    throw new TwitterAPIException(1, ex.Message);
                }
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

            // XML以外が帰ってきた時はエラー
            if (!res.ContentType.Contains(XML)) {
                throw new TwitterAPIException(1000, "Xmlデータ以外のデータを受信しました。");
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
                    catch (WebException ex) {
                        throw new TwitterAPIException(1, ex.Message);
                    }
                    catch (IOException ex) {
                        throw new TwitterAPIException(1, ex.Message);
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

            // XML以外が帰ってきた時はエラー
            if (!res.ContentType.Contains(XML)) {
                throw new TwitterAPIException(1000, "Xmlデータ以外のデータを受信しました。");
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
                    catch (WebException ex) {
                        throw new TwitterAPIException(1, ex.Message);
                    }
                    catch (IOException ex) {
                        throw new TwitterAPIException(1, ex.Message);
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
            req.ContentLength = 0;

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
        private XElement PostImageToAPI(string uri, string content_name, string filename, Image image, string imageContentType)
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
                startsb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", content_name ,filename);
                startsb.AppendLine();
                startsb.Append("Content-Type: image/");
                startsb.AppendLine(imageContentType);
                startsb.AppendLine("Content-Transfer-Encoding: binary");
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
            WebResponse res = RequestWeb(url + '?' + JoinParameters(parameters), GET, false);
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
                    RetweetedCount = int.Parse(el.Element("retweet_count").Value),
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
                };
                XElement mainel = (notRT) ? el : RTel;
                if (mainel.Element("entities") != null) {
                    IEnumerable<URLData> urldata = ConvertToURLData(mainel.Element("entities"), false);
                    foreach (var u in urldata) {
                        data.MainTwitData.Text = data.MainTwitData.Text.Replace(u.shorten_url, u.expand_url);
                    }
                    data.UrlData = urldata.ToArray();
                }
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
        #region -ConvertToTwitDataArray XElementからTwitDataの列挙型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからTwitDataの列挙型に変換します。
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
        #region -ConvertToTwitDataUserStream UserStreamから来るデータを変換したXElementをTwitDataに変換します。
        //-------------------------------------------------------------------------------
        //
        private TwitData ConvertToTwitDataUserStream(XElement el)
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
                };
                XElement mainel = (notRT) ? el : RTel;
                if (mainel.Element("entities") != null) {
                    IEnumerable<URLData> urldata = ConvertToURLData(mainel.Element("entities"), true);
                    foreach (var u in urldata) {
                        data.MainTwitData.Text = data.MainTwitData.Text.Replace(u.shorten_url, u.expand_url);
                    }
                    data.UrlData = urldata.ToArray();
                }
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
        #endregion (ConvertToTwitDataUserStream)
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
        #region -ConvertToTwitDataArrayDM XElementからTwitDataの列挙型に変換します。(DM用)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>XElementからTwitDataの列挙型に変換します。</para>
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
                    Following = bool.Parse(el.Element("following").Value),
                    Public = el.Element("mode").Value.Equals("public"),
                    OwnerID = long.Parse(el.Element("user").Element("id").Value),
                    OwnerScreenName = el.Element("user").Element("screen_name").Value,
                    OwnerIconURL = el.Element("user").Element("profile_image_url").Value
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
        #region -ConvertToListDataArray XElementからListDataの列挙型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからListDataの列挙型に変換します。
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
        #region -ConvertToTwitDataSearch XElementからTwitDataの配列型に変換します。
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<TwitData> ConvertToTwitDataSearch(XElement el)
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
                        Mention_StatusID = -1,
                        Mention_UserID = -1, // TryParseLong(xel.Element("to_user_id").Value),
                        Text = ConvertSpecialChar(xel.Element("text").Value),
                        Source = CutSourceString(ConvertSpecialChar(xel.Element("source").Value)),
                        UserID = long.Parse(xel.Element("from_user_id").Value),
                        UserName = "",
                        IconURL = xel.Element("profile_image_url").Value,
                        UserScreenName = xel.Element("from_user").Value,
                        UserProtected = false,
                        RTTwitData = null
                    };

                    if (xel.Element("entities") != null) {
                        IEnumerable<URLData> urldata = ConvertToURLData(xel.Element("entities"), true);
                        foreach (var u in urldata) {
                            data.MainTwitData.Text = data.MainTwitData.Text.Replace(u.shorten_url, u.expand_url);
                        }
                        data.UrlData = urldata.ToArray();
                    }

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
        #endregion (ConvertToTwitDataSearch)
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
                    FriendNum = int.Parse(el.Element("friends_count").Value),
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
        #region -ConvertToUserProfileArray XElementからUserProfileの列挙型に変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからUserProfileの列挙型に変換します。
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
        #region -ConvertToFriendShipData XElementからFriendShipData型に変換します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからFriendShipData型に変換します
        /// </summary>
        /// <returns></returns>
        private FriendshipData ConvertToFriendShipData(XElement el)
        {
            try {
                var fsd = new FriendshipData() {
                    UserID = long.Parse(el.Element("id").Value),
                    UserName = el.Element("name").Value,
                    UserScreenName = el.Element("screen_name").Value,
                    Followed = false,
                    Following = false
                };
                foreach (var c in el.Elements("connecton")) {
                    switch (c.Value) {
                        case "following":
                            fsd.Following = true;
                            break;
                        case "followed_by":
                            fsd.Followed = true;
                            break;
                    }
                }
                return fsd;
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToFriendShipData)
        //-------------------------------------------------------------------------------
        #region -ConvertToFriendShipDataArray XElementからFriendShipDataの列挙型に変換します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからFriendShipDataの列挙型に変換します
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FriendshipData> ConvertToFriendShipDataArray(XElement el)
        {
            return from stat in el.Elements("relationship")
                   select ConvertToFriendShipData(stat);
        }
        #endregion (ConvertToFriendShipDataArray)
        //-------------------------------------------------------------------------------
        #region -ConvertToSuggestionCategoryData XElementからSuggestionCategoryData型へ変換します
        //-------------------------------------------------------------------------------
        //
        private SuggestionCategoryData ConvertToSuggestionCategoryData(XElement el)
        {
            try {
                return new SuggestionCategoryData() {
                    name = el.Element("name").Value,
                    slug = el.Element("slug").Value,
                    size = int.Parse(el.Element("size").Value)
                };
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToSuggestionCategoryData)
        //-------------------------------------------------------------------------------
        #region ConvertToSuggestionCategoryDataArray XElementからSuggetsionCategoryData型の列挙型へ変換します
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<SuggestionCategoryData> ConvertToSuggestionCategoryDataArray(XElement el)
        {
            return from stat in el.Elements("category")
                   select ConvertToSuggestionCategoryData(stat);
        }
        #endregion (ConvertToSuggestionCategoryDataArray)
        //-------------------------------------------------------------------------------
        #region -ConvertToEntityData XElementからEntityData型に変換します
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<EntityData> ConvertToEntityData(XElement el, bool convertMention = true, bool convertHashTag = true, bool convertUrl = true)
        {
            if (el == null) { yield break; }

            // mention
            if (convertMention) {
                var mentions = from m in el.Element("user_mentions").Elements("user_mention")
                               select new EntityData(ItemType.User,
                                                     Range.Make(int.Parse(m.Attribute("start").Value),
                                                                int.Parse(m.Attribute("end").Value)),
                                                     m.Element("screen_name").Value);
                foreach (var item in mentions) { yield return item; }
            }

            // hashtags
            if (convertHashTag) {
                var hashtags = from h in el.Element("hashtags").Elements("hashtag")
                               select new EntityData(ItemType.HashTag,
                                                     Range.Make(int.Parse(h.Attribute("start").Value),
                                                                int.Parse(h.Attribute("end").Value)),
                                                     h.Element("text").Value);
                foreach (var item in hashtags) { yield return item; }
            }

            // url
            if (convertUrl) {
                var urls = from u in el.Element("urls").Elements("url")
                           select new EntityData(null,
                                                 Range.Make(int.Parse(u.Attribute("start").Value),
                                                            int.Parse(u.Attribute("end").Value)),
                                                 u.Element("url").Value,
                                                 u.Element("expanded_url").Value);
                foreach (var item in urls) { yield return item; }
            }
        }
        #endregion (ConvertToEntityData)
        //-------------------------------------------------------------------------------
        #region -ConvertToURLData XElementからURLData型に変換します。
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<URLData> ConvertToURLData(XElement entityEl, bool isJsonConvertData)
        {
            // urls
            if (entityEl.Element("urls") != null) {
                var urls = (isJsonConvertData) ? entityEl.Element("urls").Elements("item") : entityEl.Element("urls").Elements("url");
                var urldata = from u in urls
                              where !string.IsNullOrEmpty(u.Element("expanded_url").Value)
                              select new URLData() {
                                  shorten_url = u.Element("url").Value,
                                  expand_url = u.Element("expanded_url").Value
                              };
                foreach (var d in urldata) { yield return d; }
            }

            // media
            if (entityEl.Element("media") != null) {
                var medias = (isJsonConvertData) ? entityEl.Element("media").Elements("item") : entityEl.Element("media").Elements("creative");
                var mediadata = from m in medias
                                select new URLData() {
                                    shorten_url = m.Element("url").Value,
                                    expand_url = m.Element("media_url_https").Value
                                };
                foreach (var d in mediadata) { yield return d; }
            }
        }
        #endregion (ConvertToURLData)
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
                    IEnumerable<long> friend_ids = from id in el.Element("friends").Elements("item")
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
                else if (el.Element("scrub_geo") != null) {
                    var value = Tuple.Create(long.Parse(el.Element("scrub_geo").Element("user_id_str").Value), long.Parse(el.Element("scrub_geo").Element("up_to_status_id_str").Value));
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.location_dalelete, value);
                }
                else if (el.Element("status_withheld") != null) {
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.status_withheld, null);
                }
                else if (el.Element("user_withheld") != null) {
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.user_withheld, null);
                }
                else if (el.Element("disconnect") != null) {
                    var value = Tuple.Create(int.Parse(el.Element("disconnect").Element("code").Value), el.Element("disconnect").Element("reason").Value);
                    return new Tuple<UserStreamItemType, object>(UserStreamItemType.disconnect, value);
                }
                else {
                    // status
                    var twitdata = ConvertToTwitDataUserStream(el);
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
            using (XmlDictionaryReader xmldreader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(jsonStr), XmlDictionaryReaderQuotas.Max)) {
                return XElement.Load(xmldreader);
            }
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
            try {
                return DateTime.ParseExact(str, DATETIME_FORMATS, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
            }
            catch (ArgumentException) {
                Log.DebugLog(string.Format("未知のDateTime:{0}", str));
                return DateTime.Now;
            }
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
        private string ConcatWithComma(Array array, bool withSpace = true)
        {
            bool isfirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (object o in array) {
                if (!isfirst) {
                    if (withSpace) { sb.Append(", "); }
                    else { sb.Append(','); }
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
            // @から始まるUserMentionのEntity
            Regex rm = new Regex(MENTION_REGEX_PATTERN);
            foreach (Match m in rm.Matches(text)) {
                Group g = m.Groups["entity"];
                yield return new EntityData(ItemType.User, new Range(g.Index - 1, g.Length + 1), g.Value);
            }
            // #から始まるHashTagのEntity
            Regex rh = new Regex(HASHTAG_REGEX_PATTERN);
            foreach (Match m in rh.Matches(text)) {
                Group g = m.Groups["entity"];
                yield return new EntityData(ItemType.HashTag, new Range(g.Index, g.Length), g.Value);
            }
            // URLのEntity
            Regex ru = new Regex(Utilization.URL_REGEX_PATTERN);
            foreach (Match m in ru.Matches(text)) {
                string value;
                if (m.Value[0] == 't') { value = 'h' + m.Value; }
                else { value = m.Value; }
                yield return new EntityData(null, new Range(m.Index, m.Length), value);
            }
        }
        #endregion (GetEntitiesByRegex)
        //===============================================================================
        #region -AssertAuthenticated 認証済みを確認
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 認証済みを確認します。認証済みでない場合は例外を発します。
        /// </summary>
        private void AssertAuthenticated()
        {
            if (!IsAuthenticated()) {
                throw new InvalidOperationException("認証されていません。");
            }
        }
        #endregion (AssertAuthenticated)
        //-------------------------------------------------------------------------------
        #endregion (Private Util Methods)
    }
}
