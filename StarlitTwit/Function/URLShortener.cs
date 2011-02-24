using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Xml;

namespace StarlitTwit
{
    /// <summary>
    /// URL短縮の機能を提供します。
    /// </summary>
    public static partial class URLShortener
    {
        private static readonly Dictionary<URLShortenType, IURLShortenInfo> SHORTEN_DIC;

        //-------------------------------------------------------------------------------
        #region 静的コンストラクタ
        //-------------------------------------------------------------------------------
        //
        static URLShortener()
        {
            SHORTEN_DIC = new Dictionary<URLShortenType,IURLShortenInfo>()  {
                {URLShortenType.bit_ly, new ShortenerSet_bit_ly.Shortener_bit_ly()},
                {URLShortenType.j_mp, new ShortenerSet_bit_ly.Shortener_j_mp()} 
            };
        }
        //-------------------------------------------------------------------------------
        #endregion (静的コンストラクタ)

        //-------------------------------------------------------------------------------
        #region -(interface)IURLShortenInfo URL短縮サービスクラスインタフェース
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URL短縮サービスの情報を表すクラスのインタフェースです。
        /// </summary>
        public interface IURLShortenInfo
        {
            /// <summary>
            /// URLを短縮します。
            /// </summary>
            /// <param name="url">短縮するURL</param>
            /// <returns>短縮されたURL</returns>
            string Shorten(string url);

            /// <summary>
            /// 短縮URLを元に戻します。
            /// </summary>
            /// <param name="shortenURL">短縮されたURL</param>
            /// <returns>短縮するURL</returns>
            string Expand(string shortenURL);

            /// <summary>
            /// URLがそのサービスで短縮されたURLかどうかを判別します。
            /// </summary>
            /// <param name="url">URL</param>
            /// <returns>そのサービスで短縮されたURLかどうか</returns>
            bool IsShortenURL(string url);
        }
        //-------------------------------------------------------------------------------
        #endregion (-(interface)IURLShortenInfo)

        //-------------------------------------------------------------------------------
        #region (classes)実装短縮サービス
        //-------------------------------------------------------------------------------
        #region (class)bit.ly / j.mp Shortener
        //-------------------------------------------------------------------------------
        public static class ShortenerSet_bit_ly
        {
            private const string bit_ly_username = @"startf";
            private const string bit_ly_APIKey = @"R_c1e1f4935de9874be813d8178d2adb5a";

            //-------------------------------------------------------------------------------
            #region (class)Shortener_bit_ly
            //-------------------------------------------------------------------------------
            public class Shortener_bit_ly : IURLShortenInfo
            {
                //public readonly Shortener_bit_ly Instance = 

                private const string bit_ly_URL = @"http://bit.ly/";
                private const string API_bit_ly_URL = @"http://api.bit.ly/";



                string IURLShortenInfo.Shorten(string url)
                {
                    return Shorten(url, API_bit_ly_URL);
                }

                string IURLShortenInfo.Expand(string shortenURL)
                {
                    return Expand(shortenURL);
                }

                bool IURLShortenInfo.IsShortenURL(string url)
                {
                    return url.StartsWith(bit_ly_URL);
                }
            }
            //-------------------------------------------------------------------------------
            #endregion ((Class) Shortener_bit_ly)
            //-------------------------------------------------------------------------------
            #region (class)Shortener_j_mp
            //-------------------------------------------------------------------------------
            public class Shortener_j_mp : IURLShortenInfo
            {
                private const string j_mp_URL = @"http://j.mp/";
                private const string API_j_mp_URL = @"http://api.j.mp/";

                string IURLShortenInfo.Shorten(string url)
                {
                    return Shorten(url, API_j_mp_URL);
                }

                string IURLShortenInfo.Expand(string shortenURL)
                {
                    return Expand(shortenURL);
                }

                bool IURLShortenInfo.IsShortenURL(string url)
                {
                    return url.StartsWith(j_mp_URL);
                }
            }
            //-------------------------------------------------------------------------------
            #endregion ((Class)Shortener_j_mp)

            //-------------------------------------------------------------------------------
            #region -[static]Shorten URL短縮
            //-------------------------------------------------------------------------------
            //
            private static string Shorten(string url, string urldomain)
            {
                StringBuilder sburl = new StringBuilder();
                sburl.Append(urldomain);
                sburl.Append(@"v3/shorten?login=");
                sburl.Append(bit_ly_username);
                sburl.Append("&apiKey=");
                sburl.Append(bit_ly_APIKey);
                sburl.Append("&longUrl=");
                sburl.Append(Utilization.UrlEncode(url));
                sburl.Append("&format=xml");

                string requestUrl = sburl.ToString();
                XElement el = GetXmlFromWeb(requestUrl);

                return el.Element("data").Element("url").Value;
            }
            #endregion (-Shorten)
            //-------------------------------------------------------------------------------
            #region -[static]Expand URL展開
            //-------------------------------------------------------------------------------
            //
            private static string Expand(string shortenUrl)
            {
                StringBuilder sburl = new StringBuilder();
                sburl.Append(@"http://api.bit.ly/v3/expand?login=");
                sburl.Append(bit_ly_username);
                sburl.Append("&apiKey=");
                sburl.Append(bit_ly_APIKey);
                sburl.Append("&shortUrl=");
                sburl.Append(Utilization.UrlEncode(shortenUrl));
                sburl.Append("&format=xml");

                string requestUrl = sburl.ToString();
                XElement el = GetXmlFromWeb(requestUrl);

                return el.Element("data").Element("entry").Element("long_url").Value;
            }
            #endregion (Expand)
            //-------------------------------------------------------------------------------
            #region -[static]GetXmlFromWeb Webにリクエストを送信して結果をXmlとして取得します。
            //-------------------------------------------------------------------------------
            /// <summary>
            /// Webにリクエストを送信して結果をXmlとして取得します。
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            private static XElement GetXmlFromWeb(string url)
            {
                WebRequest req = WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";

                WebResponse res;
                try {
                    res = req.GetResponse();
                }
                catch (WebException ex) {
                    Log.DebugLog(ex);
                    return null;
                }

                using (Stream resStream = res.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(resStream, Encoding.ASCII)) {
                        try {
                            return XElement.Load(reader);
                        }
                        catch (XmlException ex) {
                            Log.DebugLog(ex);
                            return null;
                        }
                    }
                }
            }
            #endregion (GetXmlFromWeb)
        }
        //-------------------------------------------------------------------------------
        #endregion (bit.ly / j.mp Shortener)
        //-------------------------------------------------------------------------------
        #endregion ((classes)実装短縮サービス)

        //-------------------------------------------------------------------------------
        #region +[static]Shorten URL短縮
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを短縮します。
        /// </summary>
        /// <param name="url">短縮するURL</param>
        /// <returns>短縮URL</returns>
        public static string Shorten(string url, URLShortenType type)
        {
            return SHORTEN_DIC[type].Shorten(url);
        }
        //-------------------------------------------------------------------------------
        #endregion (Shorten)

        //-------------------------------------------------------------------------------
        #region +[static]Expand 短縮URLを戻す
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 短縮URLを元に戻します。戻せない場合はそのままのURLが返ります。
        /// </summary>
        /// <param name="shortenUrl">短縮URL</param>
        /// <returns>元のURL</returns>
        public static string Expand(string shortenUrl)
        {
            foreach (var info in SHORTEN_DIC.Values) {
                if (info.IsShortenURL(shortenUrl)) {
                    return info.Expand(shortenUrl);
                }
            }
            return shortenUrl;
        }
        #endregion (+Expand)

        //-------------------------------------------------------------------------------
        #region +[static]IsShortenURL 短縮済みURLかどうかを判定します。
        //-------------------------------------------------------------------------------
        //
        public static bool IsShortenURL(string url)
        {
            return SHORTEN_DIC.Values.Any(info => info.IsShortenURL(url));
        }
        #endregion (IsShortenURL)
        //-------------------------------------------------------------------------------
        #region +[static]ExistShortenableURL 短縮可能なURLが存在するかどうかを返します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 短縮可能なURLが存在するかどうかを返します。
        /// </summary>
        /// <param name="urls">URL</param>
        /// <returns></returns>
        public static bool ExistShortenableURL(string[] urls)
        {
            return !urls.All((url) => IsShortenURL(url));
        }
        #endregion (ExistShortenableURL)
    }

    //-------------------------------------------------------------------------------
    #region URLShortenType 列挙体：URL短縮の種類
    //-------------------------------------------------------------------------------
    /// <summary>
    /// URL短縮の種類を表します。
    /// </summary>
    public enum URLShortenType
    {
        /// <summary>bit.lyを使用</summary>
        bit_ly,
        /// <summary>j.mpを使用</summary>
        j_mp
    }
    //-------------------------------------------------------------------------------
    #endregion (URLShortenType)
}
