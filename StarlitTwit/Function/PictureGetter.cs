using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace StarlitTwit
{
    public static partial class PictureGetter
    {
        /// <summary>[static]読み込み可能な画像の拡張子</summary>
        private static readonly string[] IMAGE_EXTENSIONS;
        /// <summary>Thumbnailへのコンバータ</summary>
        private static readonly IThumbnailConverter[] CONVERTERS;

        //-------------------------------------------------------------------------------
        #region -(interface)IThumbnailConverter 画像サムネイルへのコンバータのインタフェース
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像へのURLをThumbnailに変換するためのメソッドを定義するクラスのインタフェースです。
        /// </summary>
        private interface IThumbnailConverter
        {
            /// <summary>
            /// URLをThumbnailURLに変換できるかどうか
            /// </summary>
            /// <returns>できるかどうか</returns>
            bool IsEffectiveURL(string url);

            /// <summary>
            /// URLをThumbailのURLに変換します。
            /// </summary>
            /// <param name="url">URL</param>
            /// <returns>ThumbnailのURL</returns>
            string ConvertToThumbnailURL(string url);
        }
        //-------------------------------------------------------------------------------
        #endregion (-(interface)IThumbnailConverter)

        //-------------------------------------------------------------------------------
        #region (private classes)実装コンバータ
        //-------------------------------------------------------------------------------
        #region サムネイルタイプ文字(列)
        //-------------------------------------------------------------------------------
        private const string THUMB = "thumb";
        private const string MINI = "mini";
        private const string DEFAULT = "default";
        private const char S = 's';
        private const char T = 't';
        private const char M = 'm';
        private const char L = 'l';
        private const string THUMBNAIL = "thumbnail";
        private const string MEDIUM = "medium";
        private const string BIG = "big";
        private const string LARGE = "large";
        private const string ORIG = "orig";
        //-------------------------------------------------------------------------------
        #endregion (サムネイルタイプ文字列)
        //-------------------------------------------------------------------------------
        #region (class)Youtube Converter
        //-------------------------------------------------------------------------------
        private class YoutubeConverter : IThumbnailConverter
        {
            const string CHECKPATTERN = @"^http://(www.youtube.com/watch\?v\=|youtu.be/)([\w-]+)";
            const string THUMBFORMAT = @"http://i.ytimg.com/vi/{0}/{1}.jpg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                Match m = Regex.Match(url, CHECKPATTERN);

                string size;
                switch (FrmMain.SettingsData.ThumbType_youtube) {
                    case YoutubeThumbnailType.デフォルト:
                        size = DEFAULT;
                        break;
                    case YoutubeThumbnailType.大きいサイズ:
                        size = "0";
                        break;
                    default:
                        return null;
                }

                return string.Format(THUMBFORMAT, m.Groups[2].Value, size);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)Youtube Converter)
        //-------------------------------------------------------------------------------
        #region (class)Nicovideo Converter
        //-------------------------------------------------------------------------------
        private class NicovideoConverter : IThumbnailConverter
        {
            const string CHECKPATTERN = @"^http://(www.nicovideo.jp/watch|nico.ms)/(sm|nm|so)(\d+)"; // スレッドIDは未対応
            const string THUMBFORMAT = @"http://tn-skr{0}.smilevideo.jp/smile?i={1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                if (FrmMain.SettingsData.ThumbType_nicovideo != NicovideoThumbnailType.表示する) { return null; }

                Match m = Regex.Match(url, CHECKPATTERN);
                int no = int.Parse(m.Groups[3].Value);
                return string.Format(THUMBFORMAT, (no % 4) + 1, no);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)Nicovideo Converter)
        //-------------------------------------------------------------------------------
        #region (class)Twitpic Converter
        //-------------------------------------------------------------------------------
        private class TwitpicConverter : IThumbnailConverter
        {
            const string CHECKPATTERN = @"^http://twitpic.com/[a-z0-9]+$";
            const string THUMBFORMAT = @"http://twitpic.com/show/{0}/{1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);

                //const string TWITPIC = @"twitpic.com";
                //string hostname = Utilization.GetHostName(url);
                //if (hostname == null) { return false; }
                //try {
                //    IPHostEntry entry = Dns.GetHostEntry(hostname);
                //    IPHostEntry twitpicentry = Dns.GetHostEntry(TWITPIC);
                //    return (entry.AddressList.Except(twitpicentry.AddressList).Count() == 0);
                //}
                //catch (SocketException) { return false; }
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                string type;
                switch (FrmMain.SettingsData.ThumbType_twitpic) {
                    case TwitPicThumbnailType.thumb:
                        type = THUMB;
                        break;
                    case TwitPicThumbnailType.mini:
                        type = MINI;
                        break;
                    default:
                        return null;
                }
                return string.Format(THUMBFORMAT, type, url.Split('/').Last());
            }
        }
        #endregion (TwitpicConverter)
        //-------------------------------------------------------------------------------
        #region (class)Photozou Converter
        //-------------------------------------------------------------------------------
        private class PhotozouConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://photozou.jp/photo/show/[0-9]+/[0-9]+$";
            private const string INFOAPI_URL = @"http://api.photozou.jp/rest/photo_info";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                if (FrmMain.SettingsData.ThumbType_photozou != PhotozouThumbnailType.表示する) { return null; }

                string id = url.Split('/').Last();
                string apiurl = INFOAPI_URL + @"?photo_id=" + id;
                try {
                    WebRequest req = WebRequest.Create(apiurl);
                    WebResponse res = req.GetResponse();
                    using (Stream stream = res.GetResponseStream()) {
                        XElement el = XElement.Load(stream);
                        return el.Element("info").Element("photo").Element("thumbnail_image_url").Value;
                    }
                }
                catch (Exception) { return null; }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (Photozou Converter)
        //-------------------------------------------------------------------------------
        #region (class)yFrog Converter
        //-------------------------------------------------------------------------------
        private class yFrogConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://yfrog.com/[a-z0-9]+$";
            private const string THUMBNAIL = ".th.jpg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                if (FrmMain.SettingsData.ThumbType_yFrog != yFrogThumbnailType.表示する) { return null; }
                return url + THUMBNAIL;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (yFrog Converter)
        //-------------------------------------------------------------------------------
        #region (class)img.ly Converter
        //-------------------------------------------------------------------------------
        private class img_lyConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://img.ly/[0-9a-zA-Z]+$";
            private const string THUMBFORMAT = @"http://img.ly/show/{0}/{1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {

                string type;
                switch (FrmMain.SettingsData.ThumbType_img_ly) {
                    case imglyThumbnailType.thumb:
                        type = THUMB;
                        break;
                    case imglyThumbnailType.mini:
                        type = MINI;
                        break;
                    default:
                        return null;
                }

                return string.Format(THUMBFORMAT, type, url.Split('/').Last());
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (img.ly Converter)
        //-------------------------------------------------------------------------------
        #region (class)movapic Converter
        //-------------------------------------------------------------------------------
        private class movapicConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://movapic.com/pic/[0-9]{15}[0-9a-z]+$";
            private const string THUMBFORMAT = @"http://image.movapic.com/pic/{0}_{1}.jpeg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {

                char type;
                switch (FrmMain.SettingsData.ThumbType_movapic) {
                    case movapicThumbnailType.s:
                        type = S;
                        break;
                    case movapicThumbnailType.t:
                        type = T;
                        break;
                    default:
                        return null;
                }

                return string.Format(THUMBFORMAT, type, url.Split('/').Last());
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)movapic Converter)
        //-------------------------------------------------------------------------------
        #region (class)plixi Converter
        //-------------------------------------------------------------------------------
        private class plixiConverter : IThumbnailConverter
        {
            private const string PATTERN = @"^http://(tweetphoto.com|plixi.com/p|lockerz.com/s)/[0-9]+$";
            private const string THUMBURLFORMAT = @"http://api.plixi.com/api/TPAPI.svc/imagefromurl?size={0}&url={1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, PATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {

                string type;
                switch (FrmMain.SettingsData.ThumbType_plixi) {
                    case plixiThumbnailType.thumbnail:
                        type = THUMBNAIL;
                        break;
                    case plixiThumbnailType.medium:
                        type = MEDIUM;
                        break;
                    case plixiThumbnailType.big:
                        type = BIG;
                        break;
                    default:
                        return null;
                }

                return string.Format(THUMBURLFORMAT, type, url);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)plixi Converter)
        //-------------------------------------------------------------------------------
        #region (class)ow.ly Converter
        //-------------------------------------------------------------------------------
        private class ow_lyConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://ow.ly/i/[0-9a-zA-Z]+$";
            private const string THUMBFORMAT = @"http://static.ow.ly/photos/thumb/{0}.jpg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                if (FrmMain.SettingsData.ThumbType_ow_ly != owlyThumbnailType.表示する) { return null; }
                return string.Format(THUMBFORMAT, url.Split('/').Last());
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (ow.ly Converter)
        //-------------------------------------------------------------------------------
        #region (class)twipplephoto Converter
        //-------------------------------------------------------------------------------
        private class TwipplePhotoConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://p.twipple.jp/[0-9a-zA-Z]+$";
            private const string THUMBFORMAT = @"http://p.twipple.jp/show/{0}/{1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                string type;
                switch (FrmMain.SettingsData.ThumbType_twipplephoto) {
                    case twipplephotoThumbnailType.thumb:
                        type = THUMB;
                        break;
                    case twipplephotoThumbnailType.large:
                        type = LARGE;
                        break;
                    case twipplephotoThumbnailType.orig:
                        type = ORIG;
                        break;
                    default:
                        return null;
                }
                return string.Format(THUMBFORMAT, type, url.Split('/').Last());
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)twipplephoto Converter)
        //-------------------------------------------------------------------------------
        #region (class)instagram Converter
        //-------------------------------------------------------------------------------
        private class instagramConverter : IThumbnailConverter
        {
            private const string CHECKPATTERN = @"^http://instagr.am/p/[0-9a-zA-Z]+/$";
            private const string CHECKPATTERN2 = @"http://instagram.com/p/[0-9a-zA-Z]+/$";
            private const string THUMBFORMAT = @"http://instagr.am/p/{0}/media/?size={1}";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return Regex.IsMatch(url, CHECKPATTERN) || Regex.IsMatch(url,CHECKPATTERN2);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                char type;
                switch (FrmMain.SettingsData.ThumbType_instagram) {
                    case instagramThumbnailType.t:
                        type = T;
                        break;
                    case instagramThumbnailType.m:
                        type = M;
                        break;
                    case instagramThumbnailType.l:
                        type = L;
                        break;
                    default:
                        return null;
                }
                return string.Format(THUMBFORMAT,url.Split(new char[]{'/'},StringSplitOptions.RemoveEmptyEntries).Last(),type);
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)instagram Converter)
        //-------------------------------------------------------------------------------
        #endregion ((classes)実装コンバータ)

        //-------------------------------------------------------------------------------
        #region static コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PictureGetter()
        {
            IMAGE_EXTENSIONS = GetImageCodecInfo();
            CONVERTERS = new IThumbnailConverter[] { 
                            new YoutubeConverter(),
                            new NicovideoConverter(),
                            new TwitpicConverter(),
                            new PhotozouConverter(),
                            new yFrogConverter(),
                            new img_lyConverter(),
                            new movapicConverter(),
                            new plixiConverter(),
                            new ow_lyConverter(),
                            new TwipplePhotoConverter(),
                            new instagramConverter()
                        };
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +[static]IsPictureURL 画像URLかどうか判断します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像URLかどうか判断します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static bool IsPictureURL(string url)
        {
            return IMAGE_EXTENSIONS.Any(extention => url.EndsWith(extention))
                || CONVERTERS.Any(converter => converter.IsEffectiveURL(url));
        }
        #endregion (IsPictureURL)

        //-------------------------------------------------------------------------------
        #region +[static]ConvertURL URLを変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを画像のURLに変換します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static string ConvertURL(string url)
        {
            if (IMAGE_EXTENSIONS.Any(extention => url.EndsWith(extention, StringComparison.OrdinalIgnoreCase))) { return url; }
            else {
                foreach (var converter in CONVERTERS) {
                    if (converter.IsEffectiveURL(url)) {
                        return converter.ConvertToThumbnailURL(url);
                    }
                }
            }
            return null;
        }
        #endregion (ConvertURL)

        //-------------------------------------------------------------------------------
        #region +[static]ConvertURLs URLを変換します。無効なURLは返りません。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを変換します。無効なURLは返りません。
        /// </summary>
        /// <param name="urls">URL</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertURLs(IEnumerable<string> urls)
        {
            foreach (string url in urls) {
                string thumburl = ConvertURL(url);
                if (thumburl != null) { yield return thumburl; }
            }
        }
        #endregion (ConvertURLs)

        //-------------------------------------------------------------------------------
        #region -[static]GetImageCodecInfo 画像コーデック情報取得
        //-------------------------------------------------------------------------------
        //
        private static string[] GetImageCodecInfo()
        {
            List<string> extensionList = new List<string>();
            ImageCodecInfo[] infoarray = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo info in infoarray) {
                extensionList.AddRange(info.FilenameExtension.Split(';').Select((s) => s.Remove(0, 2)));
            }
            return extensionList.ToArray();
        }
        //-------------------------------------------------------------------------------
        #endregion (GetImageCodecInfo)
    }

    //-------------------------------------------------------------------------------
    #region YoutubeThumbnailType 列挙体：
    //-------------------------------------------------------------------------------
    public enum YoutubeThumbnailType
    {
        デフォルト,
        大きいサイズ,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (YoutubeThumbnailType)
    //-------------------------------------------------------------------------------
    #region NicovideoThumbnailType 列挙体：
    //-------------------------------------------------------------------------------
    public enum NicovideoThumbnailType
    {
        表示する,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (NicovideoThumbnailType)
    //-------------------------------------------------------------------------------
    #region TwitPicThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum TwitPicThumbnailType : byte
    {
        thumb,
        mini,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (TwitPicThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region PhotozouThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum PhotozouThumbnailType : byte
    {
        表示する,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (PhotozouThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region yFrogThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum yFrogThumbnailType : byte
    {
        表示する,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (yFrogThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region imglyThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum imglyThumbnailType : byte
    {
        thumb,
        mini,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (imglyThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region movapicThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum movapicThumbnailType : byte
    {
        s,
        t,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (movapicThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region plixiThumbnailType 列挙体
    //-------------------------------------------------------------------------------
    public enum plixiThumbnailType : byte
    {
        thumbnail,
        medium,
        big,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (plixiThumbnailType 列挙体)
    //-------------------------------------------------------------------------------
    #region owlyThumbnailType 列挙体：
    //-------------------------------------------------------------------------------
    public enum owlyThumbnailType
    {
        表示する,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (owlyThumbnailType)
    //-------------------------------------------------------------------------------
    #region twipplephotoThumbnailType 列挙体：
    //-------------------------------------------------------------------------------
    public enum twipplephotoThumbnailType
    {
        thumb,
        large,
        orig,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (twipplephotoThumbnailType)
    //-------------------------------------------------------------------------------
    #region instagramThumbnailType 列挙体：
    //-------------------------------------------------------------------------------
    public enum instagramThumbnailType
    {
        t,
        m,
        l,
        表示しない
    }
    //-------------------------------------------------------------------------------
    #endregion (instagramThumbnailType)
}
