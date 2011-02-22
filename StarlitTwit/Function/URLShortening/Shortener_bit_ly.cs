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
    public static class ShortenerSet_bit_ly
    {
        private const string bit_ly_username = @"startf";
        private const string bit_ly_APIKey = @"R_c1e1f4935de9874be813d8178d2adb5a";

        //-------------------------------------------------------------------------------
        #region (Class)Shortener_bit_ly
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
        #region (Class)Shortener_j_mp
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
}
