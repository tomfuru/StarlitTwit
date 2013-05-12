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

namespace StarlitTwit
{
    partial class Twitter
    {
        //-------------------------------------------------------------------------------
        #region Member
        //-------------------------------------------------------------------------------
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }
        public string ScreenName { get; private set; }
        public long ID { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public StreamDelegateInfo DelegateInfo_UserStream;
        //-------------------------------------------------------------------------------
        #endregion (Member)
        //-------------------------------------------------------------------------------
        #region Constants
        //-------------------------------------------------------------------------------
        private const double API_VERSION = 1.1;
        private const string CONSUMER_KEY = "qvDNWLP7uX8zHsEzWiwuQ";
        private const string CONSUMER_SECRET = "Z7qNcllzRb9Iah3qfFmqUruZ0OAj5s0gdBd1zvHUs";
        private const string GET = "GET";
        private const string POST = "POST";
        private const string DELETE = "DELETE";
        private const string JSON = "json";
        private const string EXT_JSON = ".json";

        public const string URLtwi = "https://twitter.com/";
        public static readonly string URLapi;
        public static readonly string URLapiNoVer;
        public static readonly string URLStreamApi;
        public static readonly string URLUserStreamApi;

        private readonly string[] DATETIME_FORMATS;
        public const bool DEFAULT_INCLUDE_ENTITIES = true;
        public const string MENTION_REGEX_PATTERN = @"(@|＠)(?<entity>[a-zA-Z0-9_]+?)($|[^a-zA-Z0-9_])";
        //public const string HASHTAG_REGEX_PATTERN = @"(?<entity>#(?!\d+($|\s))\w+)($|\s)";
        public const string HASHTAG_REGEX_PATTERN = @"(?<entity>(#|＃)(?!\d+($|[^a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}]))[a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}]+)($|[^a-zａ-ｚA-ZＡ-Ｚ_\p{Nd}\p{Lo}\p{Lm}])";
        //-------------------------------------------------------------------------------
        #endregion (Constants)

        //-------------------------------------------------------------------------------
        #region +SetUser ユーザー設定
        //-------------------------------------------------------------------------------
        //
        public void SetUser(string access_token, string access_token_secret, string screen_name, long id)
        {
            AccessToken = access_token;
            AccessTokenSecret = access_token_secret;
            ScreenName = screen_name;
            ID = id;

            IsAuthenticated = true;
        }
        #endregion (SetUser)
        //-------------------------------------------------------------------------------
        #region +SetScreenname スクリーンネーム設定
        //-------------------------------------------------------------------------------
        //
        public void SetScreenname(string screen_name)
        {
            ScreenName = screen_name;
        }
        #endregion (SetScreenname)

        //===============================================================================
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        static Twitter()
        {
            URLapi = @"https://api.twitter.com/" + API_VERSION.ToString() + '/';
            URLapiNoVer = @"https://api.twitter.com/";
            URLStreamApi = @"https://stream.twitter.com/" + API_VERSION.ToString() + '/';
            URLUserStreamApi = @"https://userstream.twitter.com/" + API_VERSION.ToString() + "/user.json";
        }
        //
        public Twitter()
        {
            IsAuthenticated = false;

            List<string> strList = new List<string>();
            foreach (string str in StarlitTwit.Properties.Settings.Default.DateTimeFormat) { strList.Add(str); }
            DATETIME_FORMATS = strList.ToArray();
        }
        #endregion (コンストラクタ)

        //===============================================================================
        #region For Special API
        //-------------------------------------------------------------------------------
        #region -CallUpdateWithMedia
        //-------------------------------------------------------------------------------
        //
        private XElement CheckAndUpdateImage(string url, string content_name, Image image, string image_filename, Dictionary<string, string> paramdic)
        {
            string contentType;
            Guid guid = image.RawFormat.Guid;
            if (guid.Equals(ImageFormat.Jpeg.Guid)) { contentType = "jpeg"; }
            else if (guid.Equals(ImageFormat.Png.Guid)) { contentType = "png"; }
            else if (guid.Equals(ImageFormat.Gif.Guid)) { contentType = "gif"; }
            else { throw new InvalidOperationException("画像がjpg,png,gif以外のフォーマットです"); }

            return PostImageToAPIJson(url, content_name, image_filename, image, contentType);
        }
        #endregion (CallUpdateWithMedia)
        //-------------------------------------------------------------------------------
        #region RunStreaming
        //-------------------------------------------------------------------------------
        //
        private CancellationTokenSource RunStreaming(string url, Dictionary<string, string> paramdic)
        {
            CancellationTokenSource cts = new CancellationTokenSource(); // Cancelのためのオブジェクト
            CancellationToken token = cts.Token;

            var action = this.DelegateInfo_UserStream.Main;
            var endact = this.DelegateInfo_UserStream.End;
            var connectdact = this.DelegateInfo_UserStream.Connected;
            var erroract = this.DelegateInfo_UserStream.Error;
            var reconnect_wait_time = this.DelegateInfo_UserStream.Reconnect_WaitTime;

            bool all_replies = paramdic.ContainsKey("replies") && paramdic["replies"].Equals("all");

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
        #endregion (RunStreaming)

        //-------------------------------------------------------------------------------
        #endregion (For Special API)

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
            string url = URLapiNoVer + "oauth/request_token";

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
            return URLapiNoVer + "oauth/authorize?oauth_token=" + strRequestToken;
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
            string url = URLapiNoVer + "oauth/access_token";

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

        //===============================================================================
        #region Private Methods
        //-------------------------------------------------------------------------------
        #region -GetByAPIJson APIから取得(Json ver)
        //-------------------------------------------------------------------------------
        //
        private XElement GetByAPIJson(string uri)
        {
            WebResponse res = RequestWeb(uri, GET, false);

            const string HEADER_LIMIT = "x-rate-limit-limit";
            const string HEADER_REMAINING = "x-rate-limit-remaining";
            const string HEADER_RESET = "x-rate-limit-reset";

            if (res.Headers.AllKeys.Any(str => string.Equals(str, HEADER_LIMIT, StringComparison.OrdinalIgnoreCase))
             && res.Headers.AllKeys.Any(str => string.Equals(str, HEADER_REMAINING, StringComparison.OrdinalIgnoreCase))
             && res.Headers.AllKeys.Any(str => string.Equals(str, HEADER_RESET, StringComparison.OrdinalIgnoreCase))) {
                //API_Max = int.Parse(res.Headers["X-RateLimit-Limit"]);
                //API_Rest = int.Parse(res.Headers["X-RateLimit-Remaining"]);

                string[] tmp = res.Headers[HEADER_LIMIT].Split(',');
                int api_Max = int.Parse(tmp[tmp.Length - 1]);
                tmp = res.Headers[HEADER_REMAINING].Split(',');
                int api_Rest = int.Parse(tmp[tmp.Length - 1]);
                tmp = res.Headers[HEADER_RESET].Split(',');
            }

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
        #region -PostToAPIJson APIに投稿(Json ver)
        //-------------------------------------------------------------------------------
        //
        private XElement PostToAPIJson(string uri)
        {
            WebResponse res = RequestWeb(uri, POST, false);

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
        #endregion (PostToAPIJson)
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
        #region PostImageToAPIJson 画像を投稿(Json ver)
        //-------------------------------------------------------------------------------
        //
        private XElement PostImageToAPIJson(string uri, string content_name, string filename, Image image, string imageContentType)
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
                startsb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", content_name, filename);
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
        #endregion (PostImageToAPIJson)
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
            return from stat in el.Elements()
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
            return from stat in el.Elements()
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
            return from stat in el.Elements()
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

                return from stat in el.Element("statuses").Elements("item")
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
            return from stat in el.Elements()
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
                XElement main = el.Element("relationship");
                XElement source = main.Element("source");
                XElement target = main.Element("target");
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
        #region -ConvertToFriendshipData XElementからFriendShipData型に変換します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからFriendShipData型に変換します
        /// </summary>
        /// <returns></returns>
        private FriendshipData ConvertToFriendshipData(XElement el)
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
        #region -ConvertToFriendshipDataArray XElementからFriendShipDataの列挙型に変換します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// XElementからFriendShipDataの列挙型に変換します
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FriendshipData> ConvertToFriendshipDataArray(XElement el)
        {
            return from stat in el.Elements()
                   select ConvertToFriendshipData(stat);
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
            return from stat in el.Elements()
                   select ConvertToSuggestionCategoryData(stat);
        }
        #endregion (ConvertToSuggestionCategoryDataArray)
        //-------------------------------------------------------------------------------
        #region ConvertToIDArray XElementからlong型に変換します
        //-------------------------------------------------------------------------------
        //
        private IEnumerable<long> ConvertToIDArray(XElement el)
        {
            return from id in el.Elements("item")
                   select long.Parse(id.Value);
        }
        #endregion (ConvertToIDArray)
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
        //-------------------------------------------------------------------------------
        #region -ConvertToSearchMetaData XElementからSearchMetaData型に変換します。
        //-------------------------------------------------------------------------------
        //
        private SearchMetaData ConvertToSearchMetaData(XElement el)
        {
            try {
                return new SearchMetaData() {
                    Completed_in = float.Parse(el.Element("completed_in").Value),
                    Count = int.Parse(el.Element("count").Value),
                    Max_id = long.Parse(el.Element("max_id").Value),
                    Query = el.Element("query").Value,
                    Since_id = long.Parse(el.Element("since_id").Value)
                };
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToSearchMetaData)
        //===============================================================================
        #region -ConvertToSearchReturnData XElementからTuple<IEnumerable<TwitData>, SearchMetaData>に変換します。
        //-------------------------------------------------------------------------------
        //
        private Tuple<IEnumerable<TwitData>, SearchMetaData> ConvertToSearchReturnData(XElement el)
        {
            try {
                return new Tuple<IEnumerable<TwitData>, SearchMetaData>(
                    ConvertToTwitDataArray(el.Element("statuses")),
                    ConvertToSearchMetaData(el.Element("search_metadata"))
                );
            }
            catch (NullReferenceException ex) {
                Log.DebugLog(ex);
                Log.DebugLog(el.ToString());
                throw new TwitterAPIException(1001, "予期しないXmlです。");
            }
        }
        #endregion (ConvertToSearchReturnData)
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
            if (!this.IsAuthenticated) {
                throw new InvalidOperationException("認証されていません。");
            }
        }
        #endregion (AssertAuthenticated)
        //-------------------------------------------------------------------------------
        #endregion (Private Util Methods)
    }

    //-------------------------------------------------------------------------------
    #region StreamDelegateInfo 構造体：
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public struct StreamDelegateInfo
    {
        /// <summary></summary>
        public Action<UserStreamItemType, object> Main;
        public Action End;
        public Action Connected;
        public Action<bool, int> Error;
        public int Reconnect_WaitTime;
    }
    //-------------------------------------------------------------------------------
    #endregion (StreamDelegateInfo)
}
