﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace TwitterClient
{
    /// <summary>
    /// http://d.hatena.ne.jp/nojima718/20100129/1264792636 より
    /// </summary>
    public class Auth
    {
        const string REQUEST_TOKEN_URL = "https://twitter.com/oauth/request_token";
        const string ACCESS_TOKEN_URL = "https://twitter.com/oauth/access_token";
        const string AUTHORIZE_URL = "https://twitter.com/oauth/authorize";

        //-------------------------------------------------------------------------------
        #region 変数
        //-------------------------------------------------------------------------------
        private Random random = new Random();

        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string RequestToken { get; private set; }
        public string RequestTokenSecret { get; private set; }
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }
        public string UserId { get; private set; }
        public string ScreenName { get; private set; }
        //-------------------------------------------------------------------------------
        #endregion (変数)

        //-------------------------------------------------------------------------------
        #region Auth
        //-------------------------------------------------------------------------------
        //
        public Auth(string consumerKey, string consumerSecret)
        {
            ServicePointManager.Expect100Continue = false;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public Auth(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string userId, string screenName)
        {
            ServicePointManager.Expect100Continue = false;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            UserId = userId;
            ScreenName = screenName;
        }
        //-------------------------------------------------------------------------------
        #endregion (Auth)
        //-------------------------------------------------------------------------------
        #region GetRequestToken
        //-------------------------------------------------------------------------------
        //
        public void GetRequestToken()
        {
            SortedDictionary<string, string> parameters = GenerateParameters("");
            string signature = GenerateSignature("", "GET", REQUEST_TOKEN_URL, parameters);
            parameters.Add("oauth_signature", UrlEncode(signature));
            string response = HttpGet(REQUEST_TOKEN_URL, parameters);
            Dictionary<string, string> dic = ParseResponse(response);
            RequestToken = dic["oauth_token"];
            RequestTokenSecret = dic["oauth_token_secret"];
        }
        //-------------------------------------------------------------------------------
        #endregion (GetRequestToken)
        //-------------------------------------------------------------------------------
        #region GetAuthorizeUrl
        //-------------------------------------------------------------------------------
        //
        public string GetAuthorizeUrl()
        {
            return AUTHORIZE_URL + "?oauth_token=" + RequestToken;
        }
        //-------------------------------------------------------------------------------
        #endregion (GetAuthorizeUrl)
        //-------------------------------------------------------------------------------
        #region GetAccessToken
        //-------------------------------------------------------------------------------
        //
        public void GetAccessToken(string pin)
        {
            SortedDictionary<string, string> parameters = GenerateParameters(RequestToken);
            parameters.Add("oauth_verifier", pin);
            string signature = GenerateSignature(RequestTokenSecret, "GET", ACCESS_TOKEN_URL, parameters);
            parameters.Add("oauth_signature", UrlEncode(signature));
            string response = HttpGet(ACCESS_TOKEN_URL, parameters);
            Dictionary<string, string> dic = ParseResponse(response);
            AccessToken = dic["oauth_token"];
            AccessTokenSecret = dic["oauth_token_secret"];
            UserId = dic["user_id"];
            ScreenName = dic["screen_name"];
        }
        //-------------------------------------------------------------------------------
        #endregion (GetAccessToken)
        //-------------------------------------------------------------------------------
        #region Get
        //-------------------------------------------------------------------------------
        //
        public string Get(string url, IDictionary<string, string> parameters)
        {
            SortedDictionary<string, string> parameters2 = GenerateParameters(AccessToken);
            foreach (var p in parameters)
                parameters2.Add(p.Key, p.Value);
            string signature = GenerateSignature(AccessTokenSecret, "GET", url, parameters2);
            parameters2.Add("oauth_signature", UrlEncode(signature));
            return HttpGet(url, parameters2);
        }
        //-------------------------------------------------------------------------------
        #endregion (Get)
        //-------------------------------------------------------------------------------
        #region Post
        //-------------------------------------------------------------------------------
        //
        public string Post(string url, IDictionary<string, string> parameters)
        {
            SortedDictionary<string, string> parameters2 = GenerateParameters(AccessToken);
            foreach (var p in parameters)
                parameters2.Add(p.Key, p.Value);
            string signature = GenerateSignature(AccessTokenSecret, "POST", url, parameters2);
            parameters2.Add("oauth_signature", UrlEncode(signature));
            return HttpPost(url, parameters2);
        }
        //-------------------------------------------------------------------------------
        #endregion (Post)

        //-------------------------------------------------------------------------------
        #region HttpGet
        //-------------------------------------------------------------------------------
        //
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
        //-------------------------------------------------------------------------------
        #region HttpPost
        //-------------------------------------------------------------------------------
        //
        string HttpPost(string url, IDictionary<string, string> parameters)
        {
            byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
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
        //-------------------------------------------------------------------------------
        #region ParseResponse
        //-------------------------------------------------------------------------------
        //
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
        //-------------------------------------------------------------------------------
        #region JoinParameters
        //-------------------------------------------------------------------------------
        //
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
        //-------------------------------------------------------------------------------
        #region GenerateSignature
        //-------------------------------------------------------------------------------
        //
        private string GenerateSignature(string tokenSecret, string httpMethod, string url, SortedDictionary<string, string> parameters)
        {
            string signatureBase = GenerateSignatureBase(httpMethod, url, parameters);
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(UrlEncode(ConsumerSecret) + '&' + UrlEncode(tokenSecret));
            byte[] data = System.Text.Encoding.ASCII.GetBytes(signatureBase);
            byte[] hash = hmacsha1.ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateSignature)
        //-------------------------------------------------------------------------------
        #region GenerateSignatureBase
        //-------------------------------------------------------------------------------
        //
        private string GenerateSignatureBase(string httpMethod, string url, SortedDictionary<string, string> parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append(httpMethod);
            result.Append('&');
            result.Append(UrlEncode(url));
            result.Append('&');
            result.Append(UrlEncode(JoinParameters(parameters)));
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateSignatureBase)
        //-------------------------------------------------------------------------------
        #region GenerateParameters
        //-------------------------------------------------------------------------------
        //
        private SortedDictionary<string, string> GenerateParameters(string token)
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();
            result.Add("oauth_consumer_key", ConsumerKey);
            result.Add("oauth_signature_method", "HMAC-SHA1");
            result.Add("oauth_timestamp", GenerateTimestamp());
            result.Add("oauth_nonce", GenerateNonce());
            result.Add("oauth_version", "1.0");
            if (!string.IsNullOrEmpty(token))
                result.Add("oauth_token", token);
            return result;
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateParameters)

        //-------------------------------------------------------------------------------
        #region UrlEncode
        //-------------------------------------------------------------------------------
        //
        public string UrlEncode(string value)
        {
            string unreserved = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(value);
            foreach (byte b in data) {
                if (b < 0x80 && unreserved.IndexOf((char)b) != -1)
                    result.Append((char)b);
                else
                    result.Append('%' + String.Format("{0:X2}", (int)b));
            }
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (UrlEncode)

        //-------------------------------------------------------------------------------
        #region GenerateNonce
        //-------------------------------------------------------------------------------
        //
        private string GenerateNonce()
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(8);
            for (int i = 0; i < 8; ++i)
                result.Append(letters[random.Next(letters.Length)]);
            return result.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateNonce)

        //-------------------------------------------------------------------------------
        #region GenerateTimestamp
        //-------------------------------------------------------------------------------
        //
        private string GenerateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (GenerateTimestamp)
    }

    class oProgram
    {
        const string CONSUMER_KEY = "hogehogehogehogehoge";
        const string CONSUMER_SECRET = "fugafugafugafugafugafugafugafuga";

        static void oMain(string[] args)
        {
            Auth auth;
            var settings = ConsoleApp.Properties.Settings.Default;

            if (string.IsNullOrEmpty((string)settings["AccessToken"])) {
                auth = new Auth(CONSUMER_KEY, CONSUMER_SECRET);

                // リクエストトークンを取得する
                auth.GetRequestToken();

                // ユーザーにRequestTokenを認証してもらう
                Console.WriteLine("次のURLにアクセスして暗証番号を取得してください：");
                Console.WriteLine(auth.GetAuthorizeUrl());
                Console.Write("暗証番号：");
                string pin = Console.ReadLine().Trim();

                // アクセストークンを取得する
                auth.GetAccessToken(pin);

                // 結果を表示する
                Console.WriteLine("AccessToken: " + auth.AccessToken);
                Console.WriteLine("AccessTokenSecret: " + auth.AccessTokenSecret);
                Console.WriteLine("UserId: " + auth.UserId);
                Console.WriteLine("ScreenName: " + auth.ScreenName);

                // アクセストークンを設定ファイルに保存する
                settings["AccessToken"] = auth.AccessToken;
                settings["AccessTokenSecret"] = auth.AccessTokenSecret;
                settings["UserId"] = auth.UserId;
                settings["ScreenName"] = auth.ScreenName;
                settings.Save();
            }
            else {
                // 設定ファイルから読み込む
                auth = new Auth(CONSUMER_KEY, CONSUMER_SECRET,
                                (string)settings["AccessToken"], (string)settings["AccessTokenSecret"],
                                (string)settings["UserId"], (string)settings["ScreenName"]);
            }

            // ↓ここらへんは後でちゃんとwrapしたい

            // タイムラインから3件取得してみる
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("count", "3");
            Console.WriteLine(auth.Get("http://twitter.com/statuses/home_timeline.xml", parameters));

            // ポストしてみる
            Console.WriteLine("いまどうしてる？");
            string status = Console.ReadLine();
            parameters.Clear();
            parameters.Add("status", auth.UrlEncode(status));
            Console.WriteLine(auth.Post("http://twitter.com/statuses/update.xml", parameters));
        }
    }
}
