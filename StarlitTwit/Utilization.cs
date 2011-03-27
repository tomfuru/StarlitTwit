using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StarlitTwit
{
    /// <summary>
    /// 汎用処理です。
    /// </summary>
    public static class Utilization
    {
        public const char CHR_LOCKED = '◆';
        public const char CHR_FAVORITED = '★';
        public const string STR_DATETIMEFORMAT = "yyyy/MM/dd HH:mm:ss";
        public const string URL_REGEX_PATTERN = @"h?(ttp|ttps)://[-_.!~*'()0-9a-zA-Z;?:@&=+$,%#/]+";

        //-------------------------------------------------------------------------------
        #region +[static]UrlEncode
        //-------------------------------------------------------------------------------
        //
        /// <remarks>参考/利用：http://d.hatena.ne.jp/nojima718/20100129/1264792636 </remarks>
        public static string UrlEncode(string value)
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
        #region +[static]Follow フォローを行います using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォローを行います。返り値は成功:true,失敗:false,成功したが承認待ち:null
        /// </summary>
        /// <param name="screen_name">フォローするユーザーのScreenName</param>
        /// <returns>成功:true,失敗:false,成功したが認証待ち:null</returns>
        public static bool? Follow(string screen_name)
        {
            try {
                UserProfile ret = FrmMain.Twitter.friendships_create(screen_name: screen_name);
                if (ret.Protected && !ret.Following) { return null; }
            }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (Follow)
        //-------------------------------------------------------------------------------
        #region +[static]RemoveFollow フォロー解除を行います using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォロー解除を行います。できたかどうかが返ります。
        /// </summary>
        /// <param name="screen_name">フォロー解除するユーザーのScreenName</param>
        /// <returns></returns>
        public static bool RemoveFollow(string screen_name)
        {
            try { FrmMain.Twitter.friendships_destroy(screen_name: screen_name); }
            catch (TwitterAPIException) { return false; }
            return true;
        }
        #endregion (RemoveFollow)

        //-------------------------------------------------------------------------------
        #region +[static]GetTwitDataFromID IDから呟きに関するデータを取得します using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// IDから呟きデータを取得します。Forbiddenで取得できなかった場合は認証をつけてもう一度リトライします。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="twitData"></param>
        /// <returns></returns>
        public static bool GetTwitDataFromID(long id, out TwitData twitData)
        {
            try {
                twitData = FrmMain.Twitter.statuses_show(false, id);
            }
            catch (TwitterAPIException ex) {
                if (ex.ErrorStatusCode == 403) {
                    try {
                        twitData = FrmMain.Twitter.statuses_show(true, id);
                    }
                    catch (TwitterAPIException) {
                        twitData = default(TwitData);
                        return false;
                    }
                    return true;
                }
                twitData = default(TwitData);
                return false;
            }

            return true;
        }
        #endregion (GetTwitDataFromID)

        //-------------------------------------------------------------------------------
        #region +[static]GetProfile プロフィール取得 using TwitterAPI
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザープロフィールを取得します。
        /// </summary>
        /// <param name="screen_name"></param>
        /// <returns></returns>
        public static UserProfile GetProfile(string screen_name)
        {
            try {
                return FrmMain.Twitter.users_show(screen_name: screen_name);
            }
            catch (TwitterAPIException) {
                return null;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetProfile)

        //-------------------------------------------------------------------------------
        #region +[static]InterpretFormat フォーマットを解釈して返します。
        //-------------------------------------------------------------------------------
        #region (TwitData)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォーマットを解釈して返します。設定データのフォーマットを使用します。
        /// </summary>
        /// <param name="twitdata">発言データ</param>
        /// <returns></returns>
        public static string InterpretFormat(TwitData twitdata)
        {
            string format;
            switch (twitdata.TwitType) {
                case TwitType.Normal:
                    format = FrmMain.SettingsData.NameFormat;
                    break;
                case TwitType.Retweet:
                    format = FrmMain.SettingsData.NameFormatRetweet;
                    break;
                case TwitType.DirectMessage:
                    format = FrmMain.SettingsData.NameFormatDM;
                    break;
                case TwitType.Search:
                    format = FrmMain.SettingsData.NameFormatSearch;
                    break;
                default:
                    format = "";
                    break;
            }
            return InterpretFormat(twitdata, format);
        }
        //-------------------------------------------------------------------------------
        #endregion ((TwitData))
        #region (TwitData, string)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォーマットを解釈して返します。
        /// </summary>
        /// <param name="twitdata">発言データ</param>
        /// <param name="format">フォーマット</param>
        /// <returns></returns>
        public static string InterpretFormat(TwitData twitdata, string format)
        {
            const char DOLLER = '$';
            const char PARENTHESIS_START = '(';
            const char PARENTHESIS_END = ')';
            const string LOCKED = "Locked";
            const string FAVORITED = "Favorited";
            const string NAME = "Name";
            const string SCREENNAME = "ScreenName";
            const string SOURCE = "Source";
            const string DATETIME = "DateTime";
            const string RTDATETIME = "RTDateTime";
            const string RETWEETER = "Retweeter";
            const string RECIPIENT = "Recipient";

            string formatRep = format.Replace(@"\n", "\n");
            bool bIsRT = twitdata.IsRT(),
                 bIsDM = twitdata.IsDM(),
                 bIsSt = (twitdata.TwitType == TwitType.Search);
            int iStart, iEnd, iBase = 0;
            StringBuilder sb = new StringBuilder();

            while (true) {
                iStart = formatRep.IndexOf(DOLLER, iBase);
                iEnd = (iStart == formatRep.Length) ? -1 : formatRep.IndexOf(DOLLER, iStart + 1);
                if (iStart == -1 || iEnd == -1) {
                    sb.Append(formatRep.Substring(iBase));
                    break;
                }
                sb.Append(formatRep.Substring(iBase, iStart - iBase));

                string key = formatRep.Substring(iStart + 1, iEnd - iStart - 1);
                if (!bIsSt && !bIsDM && !bIsRT && key.Equals(LOCKED)) {
                    if (twitdata.UserProtected) { sb.Append(CHR_LOCKED); }
                }
                else if (!bIsSt && !bIsDM && key.Equals(FAVORITED)) {
                    if (twitdata.Favorited) { sb.Append(CHR_FAVORITED); }
                }
                else if (!bIsSt && key.Equals(NAME)) {
                    sb.Append(twitdata.MainTwitData.UserName);
                }
                else if (key.Equals(SCREENNAME)) {
                    sb.Append(twitdata.MainTwitData.UserScreenName);
                }
                else if (!bIsDM && key.Equals(SOURCE)) {
                    sb.Append(twitdata.MainTwitData.Source);
                }
                else if (bIsRT && key.Equals(RETWEETER)) {
                    sb.Append(twitdata.UserScreenName);
                }
                else if (bIsDM && key.Equals(RECIPIENT)) {
                    sb.Append(twitdata.DMScreenName);
                }
                else if (key.StartsWith(DATETIME)) {
                    int ikStart = key.IndexOf(PARENTHESIS_START),
                        ikEnd = key.IndexOf(PARENTHESIS_END);
                    if (ikStart == DATETIME.Length && ikEnd == key.Length - 1) {
                        string dateFormat = key.Substring(ikStart + 1, ikEnd - ikStart - 1);
                        sb.Append(twitdata.Time.ToString(dateFormat));
                    }
                }
                else if (bIsRT && key.StartsWith(RTDATETIME)) {
                    int ikStart = key.IndexOf(PARENTHESIS_START),
                        ikEnd = key.IndexOf(PARENTHESIS_END);
                    if (ikStart == RTDATETIME.Length && ikEnd == key.Length - 1) {
                        string dateFormat = key.Substring(ikStart + 1, ikEnd - ikStart - 1);
                        sb.Append(twitdata.RTTwitData.Time.ToString(dateFormat));
                    }
                }

                iBase = iEnd + 1;
            }
            return sb.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion ((TwitData, string))
        //-------------------------------------------------------------------------------
        #endregion (InterpretFormat)

        //-------------------------------------------------------------------------------
        #region +[static]GetImageFromURL 画像取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 指定URLから画像を取得します。
        /// </summary>
        /// <param name="url">画像URL</param>
        /// <returns></returns>
        public static Image GetImageFromURL(string url)
        {
            WebClient wc = new WebClient();

            Bitmap bmp = null;

            try {
                using (Stream stream = wc.OpenRead(url))
                using (Image img = Image.FromStream(stream)) {
                    bmp = new Bitmap(img);
                }
            }
            catch (WebException) { return null; }
            catch (ArgumentException) { return null; }

            return bmp;
        }
        #endregion (GetImageFromURL)

        //-------------------------------------------------------------------------------
        #region +[static]SubTwitterAPIExceptionStr TwitterAPI例外の文字列を返します。
        //-------------------------------------------------------------------------------
        //
        public static string SubTwitterAPIExceptionStr(TwitterAPIException ex)
        {
            switch (ex.ErrorStatusCode) {
                case 0:
                    // Connection Failure
                    return "ネットワークに接続されていない可能性があります。";
                case 400:
                    // Bad Request
                    return "APIの実行制限の可能性があります。";
                case 401:
                    // Not Authorized
                    return "認証に失敗しました。";
                case 403:
                    // Forbidden
                    return "使用できないAPIです。";
                case 404:
                    // Not Found
                    return "見つからないAPIです。";
                case 408:
                    // Request Timeout
                    return "要求がタイムアウトしました";
                case 500:
                    // Internal Server Error
                    return "Twitterのサーバーに問題があります。";
                case 502:
                    // Bad Gateway
                    return "Twitterのサーバーが停止しています。";
                case 503:
                    // Service Unavailable
                    return "Twitterが高負荷によりダウンしています。";
                case 1000:
                    // Faliure XmlLoad
                    return "予期しないデータを取得しました。";
                case 1001:
                    // Unexpected Xml
                    return "予期しないXmlデータを取得しました。";
                default:
                    Log.DebugLog(ex);
                    return "不明なエラーです。";
            }
        }
        #endregion (SubTwitterAPIExceptionStr)

        //-------------------------------------------------------------------------------
        #region +[static]GetHostName URLからホスト名を取得します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLからホスト名を取得します。
        /// </summary>
        /// <param name="url"></param>
        public static string GetHostName(string url)
        {
            const string TTP = @"ttp://";
            const string HTTP = @"http://";
            //const string HTTPS = @"https://";

            int start;
            if (url.StartsWith(HTTP)) { start = HTTP.Length; }
            else if (url.StartsWith(TTP)) { start = TTP.Length; }
            //else if (url.StartsWith(HTTPS)) { start = HTTPS.Length; }
            else { return null; }

            int end = url.IndexOf('/', start);
            if (end == -1) { end = url.Length; }

            return url.Substring(start, end - start);
        }
        #endregion (GetHostName)

        //-------------------------------------------------------------------------------
        #region +[static]InvokeTransactionDoingEvents イベント処理をしながら処理を行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// イベント処理を行いつつ処理を行います。
        /// </summary>
        /// <param name="act">行う処理</param>
        /// <param name="endAct">[option]処理終了時に1回だけ行う処理</param>
        public static void InvokeTransactionDoingEvents(Action act, Action endAct = null)
        {
            IAsyncResult res = InvokeTransaction(act, endAct);

            while (!res.IsCompleted) {
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }
        #endregion (InvokeTransactionDoingEvents)

        //-------------------------------------------------------------------------------
        #region +[static]InvokeTransaction 処理を別スレッドで行います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 処理を別スレッドで行います。
        /// </summary>
        /// <param name="act">別スレッドで行いたい処理</param>
        /// <param name="endAct">[option]処理終了時に1回だけ行う処理</param>
        public static IAsyncResult InvokeTransaction(Action act, Action endAct = null)
        {
            return act.BeginInvoke((ar) =>
            {
                Utilization.InvokeCallback(ar);
                if (endAct != null) { endAct(); }
            }
            , null);
        }
        #endregion (InvokeTransaction)

        //-------------------------------------------------------------------------------
        #region +[static]InvokeCallback Invoke完了時に呼び出すメソッド
        //-------------------------------------------------------------------------------
        /// <summary>
        /// EndInvokeを動的に呼び出します。
        /// </summary>
        /// <param name="ar"></param>
        public static void InvokeCallback(IAsyncResult ar)
        {
            AsyncResult asyncResult = (AsyncResult)ar;

            dynamic delg = asyncResult.AsyncDelegate;
            try {
                delg.EndInvoke(ar);
            }
            catch (TargetInvocationException ex) {
                throw ex.InnerException;
            }
        }
        #endregion (Callback)

        //-------------------------------------------------------------------------------
        #region +[static]ShowUserProfile ユーザープロフィールフォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザープロフィールフォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="canEdit">自分かどうか</param>
        /// <param name="screen_name">ユーザー名</param>
        /// <returns>プロフィール取得に成功したかどうか</returns>
        public static bool ShowUserProfile(FrmMain parent,bool canEdit, string screen_name)
        {
            UserProfile profile = GetProfile(screen_name);
            if (profile != null) {
                ShowUserProfile(parent, canEdit, profile);
                return true;
            }
            return false;
        }
        /// <summary>
        /// ユーザープロフィールフォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="canEdit">自分かどうか</param>
        /// <param name="screen_name">ユーザー名</param>
        public static void ShowUserProfile(FrmMain parent, bool canEdit, UserProfile profile)
        {
            FrmProfile frm = new FrmProfile(canEdit, profile, parent.ImageListWrapper);
            frm.Show(parent);
        }
        #endregion (ShowUserProfile)

        //-------------------------------------------------------------------------------
        #region +[static]ShowUserTweet ユーザー発言フォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザー発言フォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="screen_name">ユーザー名</param>
        public static void ShowUserTweet(FrmMain parent, string screen_name)
        {
            FrmDispTweet frm = new FrmDispTweet(parent, parent.ImageListWrapper);
            frm.FormType = FrmDispTweet.EFormType.User;
            frm.UserScreenName = screen_name;
            frm.Show(parent);
        }
        #endregion (ShowUserTweet)

        //-------------------------------------------------------------------------------
        #region +[static]OpenBrowser URLを開きます。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを開きます。
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="useInternalBrowser">内部ブラウザを使用するかどうか</param>
        public static void OpenBrowser(string url, bool useInternalBrowser)
        {
            if (useInternalBrowser) {
                var browserFrm = new FrmWebBrowser();
                browserFrm.SetURL(url);
                browserFrm.Show();
                browserFrm.Activate();
            }
            else {
                if (File.Exists(FrmMain.SettingsData.WebBrowserPath)) {
                    Process.Start(FrmMain.SettingsData.WebBrowserPath, url);
                }
                else { Process.Start(url); }
            }
        }
        #endregion (OpenBrowser)

        //-------------------------------------------------------------------------------
        #region +[static]SetModelessDialogCenter モードレスダイアログを所有フォームの中央に配置します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 所有フォーム(Owner)があればその中央にダイアログを配置します。
        /// </summary>
        /// <param name="dialog">設置するダイアログ</param>
        public static void SetModelessDialogCenter(Form dialog)
        {
            if (dialog.Owner != null) {
                dialog.StartPosition = FormStartPosition.Manual;
                dialog.Left = dialog.Owner.Left + (dialog.Owner.Width - dialog.Width) / 2;
                dialog.Top = dialog.Owner.Top + (dialog.Owner.Height - dialog.Height) / 2;
            }
        }
        #endregion (SetModelessDialogCenter)

        //-------------------------------------------------------------------------------
        #region +[static]ExtractURL URL部分を抜き出します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストからURL部分を抜き出します。
        /// </summary>
        /// <param name="text">抜き出すテキスト</param>
        /// <returns>URLの配列。</returns>
        public static IEnumerable<string> ExtractURL(string text)
        {
            #region comment out 
            //const string HTTP = @"http://";
            //const string ENDCHARS = " 　";

            //int index = 0;

            //while (true) {
            //    int start = (index < text.Length) ? text.IndexOf(HTTP, index) : -1;
            //    if (start == -1) { break; }
            //    int end = text.IndexOfAny(ENDCHARS.ToCharArray(), start);
            //    if (end == -1) { end = text.Length; }

            //    string url = text.Substring(start, end - start);
            //    yield return url;
            //    index = end + 1;
            //}
            #endregion

            MatchCollection mc = Regex.Matches(text, URL_REGEX_PATTERN);
            foreach (Match match in mc) {
                yield return match.Value;
            }
        }
        #endregion (ExtractURL)

        //-------------------------------------------------------------------------------
        #region +[static]Swap 値を交換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 値を交換します。
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="val1">値1</param>
        /// <param name="val2">値2</param>
        public static void Swap<T>(ref T val1, ref T val2)
        {
            T tmp = val1;
            val1 = val2;
            val2 = val1;
        }
        #endregion (Swap)
    }
}
