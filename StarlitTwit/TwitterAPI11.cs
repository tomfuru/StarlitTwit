using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;


namespace StarlitTwit
{
    public partial class Twitter
    {
        public IEnumerable<TwitData> statuses_mentions_timeline(int count = -1, long since_id = -1, long max_id = -1, bool trim_user = false, bool contributor_details = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool include_rts = true)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/mentions_timeline" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> statuses_user_timeline(long user_id = -1, string screen_name = "", int count = -1, long since_id = -1, long max_id = -1, bool trim_user = false, bool exclude_replies = false, bool contributor_details = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool include_rts = true)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (exclude_replies) { paramdic.Add("exclude_replies", exclude_replies.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/user_timeline" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> statuses_home_timeline(int count = -1, long since_id = -1, long max_id = -1, bool trim_user = false, bool exclude_replies = false, bool contributor_details = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (exclude_replies) { paramdic.Add("exclude_replies", exclude_replies.ToString().ToLower()); }
                if (contributor_details) { paramdic.Add("contributor_details", contributor_details.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/home_timeline" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> statuses_retweets_of_me(int count = -1, long since_id = -1, long max_id = -1, bool trim_user = false, bool include_user_entities = DEFAULT_INCLUDE_ENTITIES, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_user_entities) { paramdic.Add("include_user_entities", include_user_entities.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/retweets_of_me" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> statuses_retweets_id(long id, int count = -1, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/retweets/" + id.ToString() + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public TwitData statuses_show_id(long id, int count = -1, bool include_my_retweet = true, bool trim_user = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (include_my_retweet) { paramdic.Add("include_my_retweet", include_my_retweet.ToString().ToLower()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/show/" + id.ToString() + EXT_JSON, GET, paramdic);
            return ConvertToTwitData(GetByAPIJson(url_api));
        }

        public TwitData statuses_destroy_id(long id, bool trim_user = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/destroy/" + id.ToString() + EXT_JSON, POST, paramdic);
            return ConvertToTwitData(PostToAPIJson(url_api));
        }

        public TwitData statuses_update(string status, long in_reply_to_status_id = -1, double latitude = double.NaN, double longitude = double.NaN, string place_id = "", bool display_coordinates = true, bool trim_user = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("status", Utilization.UrlEncode(status));
                if (in_reply_to_status_id > 0) { paramdic.Add("in_reply_to_status_id", in_reply_to_status_id.ToString()); }
                if (!double.IsNaN(latitude)) { paramdic.Add("lat", latitude.ToString()); }
                if (!double.IsNaN(longitude)) { paramdic.Add("long", longitude.ToString()); }
                if (place_id.Length > 0) { paramdic.Add("place_id", place_id); }
                if (display_coordinates) { paramdic.Add("display_coordinates", display_coordinates.ToString().ToLower()); }
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/update" + EXT_JSON, POST, paramdic);
            return ConvertToTwitData(PostToAPIJson(url_api));
        }

        public TwitData statuses_retweet_id(long id, bool trim_user = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (trim_user) { paramdic.Add("trim_user", trim_user.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/retweet/" + id.ToString() + EXT_JSON, POST, paramdic);
            return ConvertToTwitData(PostToAPIJson(url_api));
        }

        public TwitData statuses_update_with_media(string status, Image image, string image_filename, bool possibly_sensitive = false, long in_reply_to_status_id = -1, double latitude = double.NaN, double longitude = double.NaN, string place_id = "", bool display_coordinates = true)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("status", Utilization.UrlEncode(status));
                if (possibly_sensitive) { paramdic.Add("possibly_sensitive", possibly_sensitive.ToString().ToLower()); }
                if (in_reply_to_status_id > 0) { paramdic.Add("in_reply_to_status_id", in_reply_to_status_id.ToString()); }
                if (!double.IsNaN(latitude)) { paramdic.Add("lat", latitude.ToString()); }
                if (!double.IsNaN(longitude)) { paramdic.Add("long", longitude.ToString()); }
                if (place_id.Length > 0) { paramdic.Add("place_id", place_id); }
                if (display_coordinates) { paramdic.Add("display_coordinates", display_coordinates.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/update_with_media" + EXT_JSON, POST, paramdic);
            XElement el = CheckAndUpdateImage(url_api, "media[]", image, image_filename, paramdic);
            return ConvertToTwitData(el);
        }

        public SequentData<long> statuses_retweeters_ids(long id, long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                paramdic.Add("cursor", cursor.ToString());
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"statuses/retweeters/ids" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public Tuple<IEnumerable<TwitData>, SearchMetaData> search_tweets(string q, string geocode = "", string lang = "ja", string locale = "ja", string result_type = "recent", int count = -1, string until = "", long since_id = -1, long max_id = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("q", Utilization.UrlEncode(q));
                if (geocode.Length > 0) { paramdic.Add("geocode", geocode); }
                if (lang.Length > 0) { paramdic.Add("lang", lang); }
                if (locale.Length > 0) { paramdic.Add("locale", locale); }
                if (result_type.Length > 0) { paramdic.Add("result_type", result_type); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (until.Length > 0) { paramdic.Add("until", until); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"search/tweets" + EXT_JSON, GET, paramdic);
            return ConvertToSearchReturnData(GetByAPIJson(url_api));
        }

        public CancellationTokenSource streaming_user(bool stall_warnings = true, string with = "followings", string replies = "")
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (stall_warnings) { paramdic.Add("stall_warnings", stall_warnings.ToString().ToLower()); }
                if (with.Length > 0) { paramdic.Add("with", with); }
                if (replies.Length > 0) { paramdic.Add("replies", replies); }
            }
            string url_api = GetUrlWithOAuthParameters(URLUserStreamApi, GET, paramdic);
            return RunStreaming(url_api, paramdic);
        }

        public IEnumerable<TwitData> direct_messages(long since_id = -1, long max_id = -1, int count = -1, int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
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
            string url_api = GetUrlWithOAuthParameters(URLapi + @"direct_messages" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArrayDM(GetByAPIJson(url_api));
        }

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
            string url_api = GetUrlWithOAuthParameters(URLapi + @"direct_messages/sent" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArrayDM(GetByAPIJson(url_api));
        }

        public TwitData direct_messages_show(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"direct_messages/show" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataDM(GetByAPIJson(url_api));
        }

        public TwitData direct_messages_destroy(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"direct_messages/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToTwitDataDM(PostToAPIJson(url_api));
        }

        public TwitData direct_messages_new(string text, long user_id = -1, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("text", text);
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"direct_messages/new" + EXT_JSON, POST, paramdic);
            return ConvertToTwitDataDM(PostToAPIJson(url_api));
        }

        public IEnumerable<long> friendships_no_retweets_ids()
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/no_retweets/ids" + EXT_JSON, GET, paramdic);
            return ConvertToIDArray(GetByAPIJson(url_api));
        }

        public SequentData<long> friends_ids(long user_id = -1, string screen_name = "", long cursor = -1)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friends/ids" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public SequentData<long> followers_ids(long user_id = -1, string screen_name = "", long cursor = -1)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"followers/ids" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public IEnumerable<FriendshipData> friendships_lookup(long[] user_id, string[] screen_name)
        {
            if ((user_id == null || user_id.Length == 0) && (screen_name == null || screen_name.Length == 0)) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if ((user_id != null && user_id.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_id)); }
                if ((screen_name != null && screen_name.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_name)); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/lookup" + EXT_JSON, GET, paramdic);
            return ConvertToFriendshipDataArray(GetByAPIJson(url_api));
        }

        public SequentData<long> friendships_incoming(long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/incoming" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public SequentData<long> friendships_outgoing(long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/outgoing" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public UserProfile friendships_create(long user_id = -1, string screen_name = "", bool follow = true)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (follow) { paramdic.Add("follow", follow.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/create" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

        public UserProfile friendships_destroy(long user_id = -1, string screen_name = "")
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

        public RelationshipData friendships_update(long user_id, string screen_name, bool device = false, bool retweets = false)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            if (!device && !retweets) {
                throw new ArgumentException("device or retweets is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (device) { paramdic.Add("device", device.ToString().ToLower()); }
                if (retweets) { paramdic.Add("retweets", retweets.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/update" + EXT_JSON, POST, paramdic);
            return ConvertToRelationshipData(PostToAPIJson(url_api));
        }

        public RelationshipData friendships_show(long source_id = -1, string source_screen_name = "", long target_id = -1, string target_screen_name = "")
        {
            if (source_id <= 0 && source_screen_name.Length == 0) {
                throw new ArgumentException("source_id or source_screen_name is required argument");
            }
            if (target_id <= 0 && target_screen_name.Length == 0) {
                throw new ArgumentException("target_id or target_screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (source_id > 0) { paramdic.Add("source_id", source_id.ToString()); }
                if (source_screen_name.Length > 0) { paramdic.Add("source_screen_name", source_screen_name); }
                if (target_id > 0) { paramdic.Add("target_id", target_id.ToString()); }
                if (target_screen_name.Length > 0) { paramdic.Add("target_screen_name", target_screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friendships/show" + EXT_JSON, GET, paramdic);
            return ConvertToRelationshipData(GetByAPIJson(url_api));
        }

        public SequentData<UserProfile> friends_list(long user_id, string screen_name, long cursor = -1, bool skip_status = false, bool include_user_entities = true)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_user_entities) { paramdic.Add("include_user_entities", include_user_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"friends/list" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public SequentData<UserProfile> followers_list(long user_id, string screen_name, long cursor = -1, bool skip_status = false, bool include_user_entities = true)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_user_entities) { paramdic.Add("include_user_entities", include_user_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"followers/list" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public UserProfile account_update_profile(string name = "", string url = "", string location = "", string description = "", bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (name.Length == 0 && url.Length == 0 && location.Length == 0 && description.Length == 0) {
                throw new ArgumentException("name or url or location or description is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (name.Length > 0) { paramdic.Add("name", Utilization.UrlEncode(name)); }
                if (url.Length > 0) { paramdic.Add("url", Utilization.UrlEncode(url)); }
                if (location.Length > 0) { paramdic.Add("location", Utilization.UrlEncode(location)); }
                if (description.Length > 0) { paramdic.Add("description", Utilization.UrlEncode(description)); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"account/update_profile" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

        public UserProfile account_update_profile_image(Image image, string image_filename, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"account/update_profile_image" + EXT_JSON, POST, paramdic);
            XElement el = CheckAndUpdateImage(url_api, "image", image, image_filename, paramdic);
            return ConvertToUserProfile(el);
        }

        public SequentData<UserProfile> blocks_list(long cursor = -1, bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"blocks/list" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public SequentData<long> blocks_ids(long cursor = -1)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"blocks/ids" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<long>(ConvertToIDArray(el.Element("ids")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public UserProfile blocks_create(long user_id = -1, string screen_name = "", bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"blocks/create" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

        public UserProfile blocks_destroy(long user_id = -1, string screen_name = "", bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"blocks/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

        public IEnumerable<UserProfile> users_lookup(long[] user_id = null, string[] screen_name = null, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if ((user_id == null || user_id.Length == 0) && (screen_name == null || screen_name.Length == 0)) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if ((user_id != null && user_id.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_id)); }
                if ((screen_name != null && screen_name.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_name)); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/lookup" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfileArray(GetByAPIJson(url_api));
        }

        public UserProfile users_show(long user_id = -1, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/show" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfile(GetByAPIJson(url_api));
        }

        public IEnumerable<UserProfile> users_search(string q, int count = -1, int page = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("q", Utilization.UrlEncode(q));
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (page > 0) { paramdic.Add("page", page.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/search" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfileArray(GetByAPIJson(url_api));
        }

        public IEnumerable<UserProfile> users_contributees(long user_id = -1, string screen_name = "", bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/contributees" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfileArray(GetByAPIJson(url_api));
        }

        public IEnumerable<UserProfile> users_contributors(long user_id = -1, string screen_name = "", bool skip_status = false, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/contributors" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfileArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> favorites_list(long user_id = -1, string screen_name = "", int count = -1, long since_id = -1, long max_id = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            if (screen_name.Length == 0) {
                throw new ArgumentException("screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"favorites/list" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public TwitData favorites_destroy(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"favorites/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToTwitData(PostToAPIJson(url_api));
        }

        public TwitData favorites_create(long id, bool include_entities = DEFAULT_INCLUDE_ENTITIES)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("id", id.ToString());
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"favorites/create" + EXT_JSON, POST, paramdic);
            return ConvertToTwitData(PostToAPIJson(url_api));
        }

        public IEnumerable<ListData> lists_list(long user_id = -1, string screen_name = "", bool reverse = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (reverse) { paramdic.Add("reverse", reverse.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/list" + EXT_JSON, GET, paramdic);
            return ConvertToListDataArray(GetByAPIJson(url_api));
        }

        public IEnumerable<TwitData> lists_statuses(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", int count = -1, long since_id = -1, long max_id = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool include_rts = true)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (count > 0) { paramdic.Add("count", count.ToString()); }
                if (since_id > 0) { paramdic.Add("since_id", since_id.ToString()); }
                if (max_id > 0) { paramdic.Add("max_id", max_id.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (include_rts) { paramdic.Add("include_rts", include_rts.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/statuses" + EXT_JSON, GET, paramdic);
            return ConvertToTwitDataArray(GetByAPIJson(url_api));
        }

        public ListData lists_members_destroy(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long user_id = -1, string screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public SequentData<ListData> lists_memberships(long user_id = -1, string screen_name = "", long cursor = -1, bool filter_to_owned_lists = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (filter_to_owned_lists) { paramdic.Add("filter_to_owned_lists", filter_to_owned_lists.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/memberships" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public SequentData<UserProfile> lists_subscribers(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/subscribers" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public ListData lists_subscribers_create(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/subscribers/create" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public UserProfile lists_subscribers_show(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long user_id = -1, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/subscribers/show" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfile(GetByAPIJson(url_api));
        }

        public ListData lists_subscribers_destroy(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/subscribers/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public ListData lists_members_create_all(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long[] user_id = null, string[] screen_name = null)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if ((user_id == null || user_id.Length == 0) && (screen_name == null || screen_name.Length == 0)) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if ((user_id != null && user_id.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_id)); }
                if ((screen_name != null && screen_name.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_name)); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members/create_all" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public UserProfile lists_members_show(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long user_id = -1, string screen_name = "", bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members/show" + EXT_JSON, GET, paramdic);
            return ConvertToUserProfile(GetByAPIJson(url_api));
        }

        public SequentData<UserProfile> lists_members(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long cursor = -1, bool include_entities = DEFAULT_INCLUDE_ENTITIES, bool skip_status = false)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (include_entities) { paramdic.Add("include_entities", include_entities.ToString().ToLower()); }
                if (skip_status) { paramdic.Add("skip_status", skip_status.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<UserProfile>(ConvertToUserProfileArray(el.Element("users")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public ListData lists_members_create(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long user_id = -1, string screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members/create" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public ListData lists_destroy(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/destroy" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public ListData lists_update(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", string name = "", bool isPrivate = false, string description = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if (name.Length > 0) { paramdic.Add("name", name); }
                paramdic.Add("isPrivate", isPrivate.ToString().ToLower());
                if (description.Length > 0) { paramdic.Add("description", description); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/update" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public ListData lists_create(string name = "", bool isPrivate = false, string description = "")
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                paramdic.Add("name", name);
                paramdic.Add("isPrivate", isPrivate.ToString().ToLower());
                if (description.Length > 0) { paramdic.Add("description", description); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/create" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public ListData lists_show(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "")
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/show" + EXT_JSON, GET, paramdic);
            return ConvertToListData(GetByAPIJson(url_api));
        }

        public SequentData<ListData> lists_subscriptions(long user_id = -1, string screen_name = "", long cursor = -1, bool filter_to_owned_lists = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (filter_to_owned_lists) { paramdic.Add("filter_to_owned_lists", filter_to_owned_lists.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/subscriptions" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public ListData lists_members_destroy_all(long list_id = -1, string slug = "", long owner_id = -1, string owner_screen_name = "", long[] user_id = null, string[] screen_name = null)
        {
            if (list_id <= 0 && slug.Length == 0) {
                throw new ArgumentException("list_id or slug is required argument");
            }
            if ((user_id == null || user_id.Length == 0) && (screen_name == null || screen_name.Length == 0)) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (list_id > 0) { paramdic.Add("list_id", list_id.ToString()); }
                if (slug.Length > 0) { paramdic.Add("slug", slug); }
                if (owner_id > 0) { paramdic.Add("owner_id", owner_id.ToString()); }
                if (owner_screen_name.Length > 0) { paramdic.Add("owner_screen_name", owner_screen_name); }
                if ((user_id != null && user_id.Length > 0)) { paramdic.Add("user_id", ConcatWithComma(user_id)); }
                if ((screen_name != null && screen_name.Length > 0)) { paramdic.Add("screen_name", ConcatWithComma(screen_name)); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/members/destroy_all" + EXT_JSON, POST, paramdic);
            return ConvertToListData(PostToAPIJson(url_api));
        }

        public SequentData<ListData> lists_ownerships(long user_id = -1, string screen_name = "", long cursor = -1, bool filter_to_owned_lists = false)
        {
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
                if (cursor > 0) { paramdic.Add("cursor", cursor.ToString()); }
                if (filter_to_owned_lists) { paramdic.Add("filter_to_owned_lists", filter_to_owned_lists.ToString().ToLower()); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"lists/ownerships" + EXT_JSON, GET, paramdic);
            XElement el = GetByAPIJson(url_api);
            return new SequentData<ListData>(ConvertToListDataArray(el.Element("lists")), long.Parse(el.Element("next_cursor").Value), long.Parse(el.Element("previous_cursor").Value));
        }

        public UserProfile users_report_spam(long user_id = -1, string screen_name = "")
        {
            if (user_id <= 0 && screen_name.Length == 0) {
                throw new ArgumentException("user_id or screen_name is required argument");
            }
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            {
                if (user_id > 0) { paramdic.Add("user_id", user_id.ToString()); }
                if (screen_name.Length > 0) { paramdic.Add("screen_name", screen_name); }
            }
            string url_api = GetUrlWithOAuthParameters(URLapi + @"users/report_spam" + EXT_JSON, POST, paramdic);
            return ConvertToUserProfile(PostToAPIJson(url_api));
        }

    }
}
