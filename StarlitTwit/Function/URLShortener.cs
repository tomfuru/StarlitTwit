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
    public static class URLShortener
    {
        const string bit_ly_username = @"startf";
        const string bit_ly_APIKey = @"R_c1e1f4935de9874be813d8178d2adb5a";
        const string bit_ly_url = @"http://bit.ly/";
        const string j_mp_url = @"http://j.mp/";
        
        //-------------------------------------------------------------------------------
        #region +Shorten URL短縮
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを短縮します。
        /// </summary>
        /// <param name="url">短縮するURL</param>
        /// <returns>短縮URL</returns>
        public static string Shorten(string url, URLShortenType type)
        {
            StringBuilder sburl = new StringBuilder();
            switch (type) {
                case URLShortenType.bit_ly:
                case URLShortenType.j_mp: {
                        if (type == URLShortenType.bit_ly) {
                            sburl.Append("http://api.bit.ly/");
                        }
                        else {
                            sburl.Append("http://api.j.mp/");
                        }
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
                default:
                    return null;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (Shorten)

        //-------------------------------------------------------------------------------
        #region +Expand 短縮URLを戻す
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 短縮URLを元に戻します。戻せない場合はそのままのURLが返ります。
        /// </summary>
        /// <param name="shortenUrl">短縮URL</param>
        /// <returns>元のURL</returns>
        public static string Expand(string shortenUrl)
        {
            if (shortenUrl.StartsWith(bit_ly_url) || shortenUrl.StartsWith(j_mp_url)) {
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

            return shortenUrl;
        }
        #endregion (+Expand)

        //-------------------------------------------------------------------------------
        #region +[static]IsShortenURL 短縮済みURLかどうかを判定します。
        //-------------------------------------------------------------------------------
        //
        public static bool IsShortenURL(string url)
        {
            return (url.StartsWith(bit_ly_url) || url.StartsWith(j_mp_url));
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

        //-------------------------------------------------------------------------------
        #region -GetXmlFromWeb Webにリクエストを送信して結果をXmlとして取得します。
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
