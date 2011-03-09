using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Net.Sockets;

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
        #region (class)Twitpic Converter
        //-------------------------------------------------------------------------------
        private class TwitpicConverter : IThumbnailConverter
        {

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                const string TWITPIC = @"twitpic.com";

                string hostname = Utilization.GetHostName(url);
                if (hostname == null) { return false; }

                try {
                    IPHostEntry entry = Dns.GetHostEntry(hostname);
                    IPHostEntry twitpicentry = Dns.GetHostEntry(TWITPIC);
                    return (entry.AddressList.Except(twitpicentry.AddressList).Count() == 0);
                }
                catch (SocketException) { return false; }
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                string[] urlpartials = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                return urlpartials[0] + "//" + urlpartials[1] + "/show/thumb/" + urlpartials[2];
            }
        }
        #endregion (TwitpicConverter)
        //-------------------------------------------------------------------------------
        #region (class)Photozou Converter
        //-------------------------------------------------------------------------------
        private class PhotozouConverter : IThumbnailConverter
        {
            private const string DOMAIN = @"http://photozou.jp/";
            private const string INFOAPI_URL = @"http://api.photozou.jp/rest/photo_info";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
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
            private const string DOMAIN = @"http://yfrog.com/";
            private const string THUMBNAIL = ".th.jpg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
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
            private const string DOMAIN = @"http://img.ly/";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DOMAIN);
                sb.Append("show/thumb/");
                sb.Append(url.Split('/').Last());
                return sb.ToString();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (img.ly Converter)
        //-------------------------------------------------------------------------------
        #region (class)movapic Converter
        //-------------------------------------------------------------------------------
        private class movapicConverter : IThumbnailConverter
        {
            private const string DOMAIN = @"http://movapic.com/";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                return string.Format("http://image.movapic.com/pic/s_{0}.jpeg", url.Split('/').Last());
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((class)movapic Converter)
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
                            new TwitpicConverter(),
                            new PhotozouConverter(),
                            new yFrogConverter(),
                            new img_lyConverter(),
                            new movapicConverter()
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
            if (IMAGE_EXTENSIONS.Any(extention => url.EndsWith(extention,StringComparison.OrdinalIgnoreCase))) { return url; }
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
}
