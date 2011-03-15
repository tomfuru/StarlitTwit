using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace UserStreamTest
{
    [DataContract]
    public class USdata
    {
        [DataMember(Name = "place", IsRequired = false)]
        public Place place;
        [DataMember(Name = "user")]
        public User user;
        [DataMember(Name = "coodinates", IsRequired = false)]
        public Coordinates coodinates;
        [DataMember(Name = "text")]
        public string text;
        [DataMember(Name = "in_reply_to_status_id")]
        public string in_reply_to_status_id;
        [DataMember(Name = "truncated")]
        public bool truncated;
        [DataMember(Name = "source")]
        public string source;
        [DataMember(Name = "favorited")]
        public bool favorited;
        [DataMember(Name = "in_reply_to_screen_name")]
        public string in_reply_to_screen_name;
        //[DataMember(Name = "")]

        //[DataMember(Name = "")]

        //[DataMember(Name = "")]

        //[DataMember(Name = "")]

        //[DataMember(Name = "")]

    }

    [DataContract]
    public class Place
    {

    }

    //-------------------------------------------------------------------------------
    #region (class)User
    //-------------------------------------------------------------------------------
    [DataContract]
    public class User
    {
        [DataMember(Name = "statuses_count")]
        public long StatusesCount;
        [DataMember(Name = "profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor;
        [DataMember(Name = "show_all_inline_media")]
        public bool ShowAllInlineMedia;
        [DataMember(Name = "profile_use_background_image")]
        public bool ProfileUseBackgroundImage;
        [DataMember(Name = "contributors_enabled")]
        public bool ContributorsEnabled;
        [DataMember(Name = "profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor;
        [DataMember(Name = "location")]
        public string Location;
        [DataMember(Name = "geo_enabled")]
        public bool GeoEnabled;
        [DataMember(Name = "description")]
        public string Description;
        [DataMember(Name = "friends_count")]
        public int FriendsCount;
        [DataMember(Name = "verified")]
        public bool Verified;
        [DataMember(Name = "favourites_count")]
        public int FavouritesCount;
        [DataMember(Name = "created_at")]
        public string CreatedAt;
        [DataMember(Name = "profile_background_color")]
        public string ProfileBackgroundColor;
        [DataMember(Name = "follow_request_sent")]
        public string FollowRequestSent;
        [DataMember(Name = "time_zone")]
        public string TimeZone;
        [DataMember(Name = "followers_count")]
        public int FollowersCount;
        [DataMember(Name = "url")]
        public string Url;
        [DataMember(Name = "profile_image_url")]
        public string ProfileImageUrl;
        [DataMember(Name = "notifications")]
        public string Notifications;
        [DataMember(Name = "profile_text_color")]
        public string ProfileTextColor;
        [DataMember(Name = "protected")]
        public bool Protected;
        [DataMember(Name = "id_str")]
        public string IdStr;
        [DataMember(Name = "lang")]
        public string Lang;
        [DataMember(Name = "profile_background_image_url")]
        public string ProfileBackgroundImageUrl;
        [DataMember(Name = "screen_name")]
        public string ScreenName;
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "following")]
        public string Following;
        [DataMember(Name = "profile_link_color")]
        public string ProfileLinkColor;
        [DataMember(Name = "id")]
        public long Id;
        [DataMember(Name = "listed_count")]
        public int ListedCount;
        [DataMember(Name = "profile_background_tile")]
        public bool ProfileBackgroundTile;
        [DataMember(Name = "utc_offset")]
        public string UtcOffset;
        [DataMember(Name = "place", IsRequired = false)]
        public Place Place;
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)User)

    [DataContract]
    public class Coordinates
    {

    }
}
