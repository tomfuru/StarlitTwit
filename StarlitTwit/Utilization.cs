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
using System.Drawing.Imaging;

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
        public const string URL_REGEX_PATTERN = @"h?(ttp|ttps)://[-_!~*'()0-9a-zA-Z;@&=+$,%.]+\.[a-zA-Z]+[-_.!~*'()0-9a-zA-Z;?:@&=+$,%#/]*";

        private const string SAVEFILE_NAME = @"Settings.dat";

        public const string FILEFORMAT_IMAGES = "画像ファイル (*.jpg,*.jpeg,*.png,*.gif)|*.jpg;*.jpeg;*.png;*.gif";

        //-------------------------------------------------------------------------------
        #region +[static]ConvertKeysToKeyData
        //-------------------------------------------------------------------------------
        //
        public static KeyData ConvertKeysToKeyData(Keys k)
        {
            KeyData keydata = new KeyData();
            if ((k & Keys.Control) == Keys.Control) {
                k = k & ~Keys.Control;
                keydata.Ctrl = true;
            }
            if ((k & Keys.Shift) == Keys.Shift) {
                k = k & ~Keys.Shift;
                keydata.Shift = true;
            }
            if ((k & Keys.Alt) == Keys.Alt) {
                k = k & ~Keys.Alt;
                keydata.Alt = true;
            }
            keydata.Key = k;

            return keydata;
        }
        #endregion (ConvertKeysToKeyData)
        //-------------------------------------------------------------------------------
        #region +[static]ConvertKeyDataToKeys
        //-------------------------------------------------------------------------------
        //
        public static Keys ConvertKeyDataToKeys(KeyData keydata)
        {
            Keys key = keydata.Key;
            if (keydata.Ctrl) { key |= Keys.Control; }
            if (keydata.Shift) { key |= Keys.Shift; }
            if (keydata.Alt) { key |= Keys.Alt; }
            return key;
        }
        #endregion (ConvertKeyDataToKeys)

        //-------------------------------------------------------------------------------
        #region +[static]GetDefaultSettingsDataFilePath デフォルト設定ファイルパス取得
        //-------------------------------------------------------------------------------
        //
        public static string GetDefaultSettingsDataFilePath()
        {
            return Path.Combine(Application.StartupPath, SAVEFILE_NAME);
        }
        #endregion (GetDefaultSettingsDataFilePath)

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
        #region +[static]CountTextLength 投稿時の文字列長を計算します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 投稿時の文字列長を計算します。長いURLはhttp://t.co/*******に変換されたとして計算します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountTextLength(string str)
        {
            int len = str.Length;
            foreach (Match m in Regex.Matches(str, URL_REGEX_PATTERN)) {
                len -= Math.Max(m.Length - 19, 0);
            }
            return len;
        }
        #endregion (CountTextLength)

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
            UserProfile profile;
            return Follow(screen_name, out profile);
        }
        /// <summary>
        /// フォローを行います。返り値は成功:true,失敗:false,成功したが承認待ち:null
        /// </summary>
        /// <param name="screen_name">フォローするユーザーのScreenName</param>
        /// <param name="newProfile">新しいUserProfile(失敗時はnull)</param>
        /// <returns>成功:true,失敗:false,成功したが認証待ち:null</returns>
        public static bool? Follow(string screen_name, out UserProfile newProfile)
        {
            try {
                newProfile = FrmMain.Twitter.friendships_create(screen_name: screen_name);
                if (newProfile.Protected && !newProfile.Following) { return null; }
            }
            catch (TwitterAPIException) {
                newProfile = null;
                return false;
            }
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
            UserProfile profile;
            return RemoveFollow(screen_name, out profile);
        }
        /// <summary>
        /// フォロー解除を行います。できたかどうかが返ります。
        /// </summary>
        /// <param name="screen_name">フォロー解除するユーザーのScreenName</param>
        /// <param name="newProfile">新しいUserProfile(失敗時はnull)</param>
        /// <returns></returns>
        public static bool RemoveFollow(string screen_name, out UserProfile newProfile)
        {
            try { newProfile = FrmMain.Twitter.friendships_destroy(screen_name: screen_name); }
            catch (TwitterAPIException) {
                newProfile = null;
                return false;
            }
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
                twitData = FrmMain.Twitter.statuses_show_id(id);
            }
            catch (TwitterAPIException) {
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
        #region +[static]SliceArray 配列を必要な部分だけスライスします
        //-------------------------------------------------------------------------------
        //
        public static T[] SliceArray<T>(T[] userIDs, ref int index, int size)
        {
            int firstIndex = index;
            index = Math.Min(userIDs.Length, index + size); // 次の開始index

            T[] ids = new T[index - firstIndex];
            Array.Copy(userIDs, firstIndex, ids, 0, index - firstIndex);
            return ids;
        }
        #endregion (SliceUserID)
        //-------------------------------------------------------------------------------
        #region +[static]SortProfiles users/lookupで返ってきたデータをuserIDsの並びのとおりに返します。
        //-------------------------------------------------------------------------------
        //
        public static IEnumerable<UserProfile> SortProfiles(IEnumerable<UserProfile> profiles, long[] ids)
        {
            UserProfile[] profs = profiles.ToArray();
            foreach (long id in ids) {
                UserProfile prof = profs.FirstOrDefault(p => p.UserID == id);
                if (prof != null) { yield return prof; }
            }
        }
        #endregion (SortProfiles)

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
        public static string InterpretFormat(TwitData twitdata, string format)
        {
            const char DOLLER = '$';
            const char PERCENT = '%';
            const char PARENTHESIS_START = '(';
            const char PARENTHESIS_END = ')';
            const string LOCKED = "Locked";
            const string FAVORITED = "Favorited";
            const string NAME = "Name";
            const string SCREENNAME = "ScreenName";
            const string SOURCE = "Source";
            const string RTCOUNT = "RTCount";
            const string DATETIME = "DateTime";
            const string RTDATETIME = "RTDateTime";
            const string RETWEETER = "Retweeter";
            const string RECIPIENT = "Recipient";

            string formatRep = format.Replace(@"\n", "\n");
            bool bIsRT = TwitData.IsRT(twitdata),
                 bIsDM = TwitData.IsDM(twitdata),
                 bIsSt = (twitdata.TwitType == TwitType.Search);
            int iBase = 0;
            StringBuilder sb = new StringBuilder();

            while (true) {
                bool endOfWhile = false;
                int iEnd;
                switch (formatRep[iBase])
	            {
                    case DOLLER: 
                        iEnd = (iBase == formatRep.Length) ? -1 : formatRep.IndexOf(DOLLER, iBase + 1);
                        if (iBase == -1 || iEnd == -1) { endOfWhile = true; break; }

                        string key = formatRep.Substring(iBase + 1, iEnd - iBase - 1);
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
                        else if (key.StartsWith(RTCOUNT)) {
                            sb.Append(twitdata.RetweetedCount);
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
                        break;
                    case PERCENT:
                        iEnd = (iBase == formatRep.Length) ? -1 : formatRep.IndexOf(PERCENT, iBase + 1);
                        if (iBase == -1 || iEnd == -1) { endOfWhile = true; break; }

                        if (iEnd - iBase > 1 &&  twitdata.RetweetedCount > 0) {
                            string sentence = formatRep.Substring(iBase + 1, iEnd - iBase - 1);
                            sb.Append(InterpretFormat(twitdata, sentence));
                        }
                        break;
		            default:
                        iEnd = iBase;
                        sb.Append(formatRep[iBase]);
                        break;
	            }

                iBase = iEnd + 1;
                if (iBase == formatRep.Length) { endOfWhile = true; }

                if (endOfWhile) { break; }
            }
            return sb.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion ((TwitData, string))
        //-------------------------------------------------------------------------------
        #endregion (InterpretFormat)
        //-------------------------------------------------------------------------------
        #region +[static]MakePopupText TwitDataからPopupに使われるTextを作成して返します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// TwitDataからPopupに使われるTextを作成して返します。
        /// </summary>
        /// <param name="twitdata">Textの元になるTwitData</param>
        /// <returns></returns>
        public static string MakePopupText(TwitData twitdata)
        {
            return Utilization.InterpretFormat(twitdata) + '\n' + twitdata.Text;
        }
        #endregion (MakePopupText)

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
        #region +[static]GetProfileDescriptionString プロファイルを説明する文字列を生成しｍさう
        //-------------------------------------------------------------------------------
        //
        public static string GetProfileDescriptionString(UserProfile p)
        {
            StringBuilder sb = new StringBuilder();
            if (p.Protected) { sb.AppendLine("◆非公開アカウント"); }
            sb.Append("●ユーザー名：");
            sb.AppendLine(p.ScreenName);
            sb.Append("●名称：");
            sb.AppendLine(p.UserName);
            sb.AppendLine("●位置情報");
            sb.AppendLine(p.Location);
            sb.AppendLine("●自己紹介：");
            sb.AppendLine(p.Description);
            sb.Append("●フォロー数：");
            sb.AppendLine(p.FriendNum.ToString());
            sb.Append("●フォロワー数：");
            sb.AppendLine(p.FollowerNum.ToString());
            sb.Append("●発言数：");
            sb.AppendLine(p.StatusNum.ToString());
            sb.Append("●お気に入り数：");
            sb.Append(p.FavoriteNum.ToString());

            return sb.ToString();
        }
        #endregion (GetProfileDescriptionString)
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
                case 1:
                    // Disconnected
                    return "接続が切断されました。";
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
                    return "予期しないデータを取得しました。(ブラウザ等でログインが必要？)";
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
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion (Callback)

        //-------------------------------------------------------------------------------
        #region +[static]ShowListsForm リスト一覧フォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リスト一覧フォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="screen_name">ユーザー名</param>
        public static void ShowListsForm(FrmMain parent, ImageListWrapper imageListWrapper, FrmDispLists.EFormType type, string screen_name = null)
        {
            if (!Utilization.ExistFrmLists(type, screen_name)) {
                FrmDispLists frm = new FrmDispLists(parent, imageListWrapper, type) {
                    UserScreenName = screen_name
                };
                frm.Show(parent);
            }
        }
        #endregion (ShowListsForm)
        //-------------------------------------------------------------------------------
        #region +[static]ShowUsersForm ユーザー一覧フォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザー一覧フォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="imageListWrapper"></param>
        /// <param name="type">フォームタイプ</param>
        /// <param name="screen_name"></param>
        /// <param name="retweet_id"></param>
        public static void ShowUsersForm(FrmMain parent, ImageListWrapper imageListWrapper, FrmDispUsers.EFormType type, string screen_name = null, string list_id = null, long retweet_id = -1)
        {
            if (!Utilization.ExistFrmUsers(type, screen_name, list_id, retweet_id)) {
                FrmDispUsers frm = new FrmDispUsers(parent, imageListWrapper, type) {
                    UserScreenName = screen_name,
                    ListID = list_id,
                    RetweetStatusID = retweet_id
                };
                frm.Show(parent);
            }
        }
        #endregion (+[static]ShowUserListForm)
        //-------------------------------------------------------------------------------
        #region +[static]ShowProfileForm ユーザープロフィールフォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザープロフィールフォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="canEdit">自分かどうか</param>
        /// <param name="screen_name">ユーザー名</param>
        /// <returns>プロフィール取得に成功したかどうか</returns>
        public static void ShowProfileForm(FrmMain parent, bool canEdit, string screen_name)
        {
            if (!Utilization.ExistFrmProfile(canEdit, screen_name)) {
                FrmProfile frm = new FrmProfile(parent, canEdit, screen_name, parent.ImageListWrapper);
                frm.Show(parent);
            }
        }
        /// <summary>
        /// ユーザープロフィールフォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="canEdit">自分かどうか</param>
        /// <param name="profile">プロフィールデータ</param>
        public static void ShowProfileForm(FrmMain parent, bool canEdit, UserProfile profile)
        {
            if (!Utilization.ExistFrmProfile(canEdit, profile.ScreenName)) {
                FrmProfile frm = new FrmProfile(parent, canEdit, profile, parent.ImageListWrapper);
                frm.Show(parent);
            }
        }
        #endregion (ShowUserProfile)
        //-------------------------------------------------------------------------------
        #region +[static]ShowStatusesForm ユーザー発言フォームを表示します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ユーザー発言フォームを表示します。
        /// </summary>
        /// <param name="parent">最上位フォーム</param>
        /// <param name="screen_name">ユーザー名</param>
        public static void ShowStatusesForm(FrmMain parent, FrmDispStatuses.EFormType formType, string screen_name = null, string listID = null, IEnumerable<TwitData> conversations = null)
        {
            if (!ExistFrmStatuses(formType, screen_name, listID, conversations)) {
                FrmDispStatuses frm = new FrmDispStatuses(parent, parent.ImageListWrapper, formType);
                frm.ReplyStartTwitdata = conversations;
                frm.UserScreenName = screen_name;
                frm.ListID = listID;
                frm.Show(parent);
            }
        }
        #endregion (ShowUserTweet)

        //-------------------------------------------------------------------------------
        #region +[static]ExistFrmLists 既にあるFrmListsを探す
        //-------------------------------------------------------------------------------
        //
        public static bool ExistFrmLists(FrmDispLists.EFormType type, string screen_name = null)
        {
            Func<FrmDispLists, bool> judgeFunc = f =>
                f.FormType == type
                && !((type == FrmDispLists.EFormType.UserList
                   || type == FrmDispLists.EFormType.UserBelongedList
                   || type == FrmDispLists.EFormType.UserSubscribingList)
                  && f.UserScreenName != screen_name);

            return ExistForm<FrmDispLists>(judgeFunc);
        }
        #endregion (ExistFrmLists)
        //-------------------------------------------------------------------------------
        #region +[static]ExistFrmProfile すでにあるFrmProfileを探す
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 既にあるFrmProfileと同じものがあるかどうか探し，あれば再前面にします。
        /// </summary>
        /// <param name="canEdit">編集可能性</param>
        /// <param name="screen_name">ユーザー名</param>
        /// <returns></returns>
        public static bool ExistFrmProfile(bool canEdit, string screen_name)
        {
            Func<FrmProfile, bool> judgeFunc = f =>
                f.CanEdit == canEdit
                && f.ScreenName == screen_name;

            return ExistForm<FrmProfile>(judgeFunc);
        }
        #endregion (ExistFrmProfile)
        //-------------------------------------------------------------------------------
        #region +[static]ExistFrmUsers 既にあるFrmUsersを探す
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 既にあるFrmUsersと同じものがあるかどうか探し，あれば最前面にします。
        /// </summary>
        /// <param name="type">タイプ</param>
        /// <param name="screen_name">UserFollower,UserFriendタイプのみ必要</param>
        /// <param name="retweet_id">Retweeterタイプのみ必要</param>
        /// <returns>あればtrue,なければfalse</returns>
        public static bool ExistFrmUsers(FrmDispUsers.EFormType type, string screen_name = null, string listID = null, long retweet_id = -1)
        {
            Func<FrmDispUsers, bool> judgeFunc = f =>
                f.FormType == type
                && !((type == FrmDispUsers.EFormType.UserFollower || type == FrmDispUsers.EFormType.UserFriend) && f.UserScreenName != screen_name)
                && !(type == FrmDispUsers.EFormType.Retweeter && f.RetweetStatusID != retweet_id)
                && !((type == FrmDispUsers.EFormType.ListMember || type == FrmDispUsers.EFormType.ListSubscriber) && f.UserScreenName != screen_name && f.ListID != listID);

            return ExistForm<FrmDispUsers>(judgeFunc);
        }
        #endregion (ExistFrmFollower)
        //-------------------------------------------------------------------------------
        #region +[static]ExistFrmStatuses 既にあるFrmDispStatusesを探す
        //-------------------------------------------------------------------------------
        //
        public static bool ExistFrmStatuses(FrmDispStatuses.EFormType type, string screen_name = null, string listID = null, IEnumerable<TwitData> conversations = null)
        {
            Func<FrmDispStatuses, bool> judgeFunc = f =>
               f.FormType == type
               && !((type == FrmDispStatuses.EFormType.UserFavorite || type == FrmDispStatuses.EFormType.UserStatus) && f.UserScreenName != screen_name)
               && !(type == FrmDispStatuses.EFormType.Conversation && f.ReplyStartTwitdata.First().StatusID == conversations.First().StatusID)
               && !(type == FrmDispStatuses.EFormType.ListStatuses && f.UserScreenName != screen_name && f.ListID != listID);

            return ExistForm<FrmDispStatuses>(judgeFunc);
        }
        #endregion (ExistFrmTweets)
        //-------------------------------------------------------------------------------
        #region -[static]ExistForm 既にあるフォームを探す
        //-------------------------------------------------------------------------------
        //
        private static bool ExistForm<F>(Func<F, bool> judgeFunc) where F : Form
        {
            F form = Application.OpenForms
                       .OfType<F>()
                       .FirstOrDefault(judgeFunc);

            if (form != null) { form.BringToFront(); }
            return form != null;
        }
        #endregion (ExistForm)

        //-------------------------------------------------------------------------------
        #region +[static]OpenBrowser URLを開きます。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを開きます。
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="useInternalBrowser">内部ブラウザを使用するかどうか</param>
        public static void OpenBrowser(string url, bool useInternalBrowser, bool thinkShindanMaker = true)
        {
            const string URL_FORMAT = @"http://shindanmaker.com/";

            if (thinkShindanMaker && url.StartsWith(URL_FORMAT)) {
                int no;
                if (int.TryParse(url.Remove(0, URL_FORMAT.Length), out no)) {
                    var diagFrm = new FrmDiagMaker(no, FrmMain.Twitter.ScreenName);
                    diagFrm.Show();
                    diagFrm.Activate();
                    return;
                }
            }

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
        #region +[static]ConvertImage 画像を変換します
        //-------------------------------------------------------------------------------
        //
        public static Image ConvertImage(Image img, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, format);
            Image new_img = Image.FromStream(ms);
            return new_img;
        }
        #endregion (ConvertImage)

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
            val2 = tmp;
        }
        #endregion (Swap)
        //-------------------------------------------------------------------------------
        #region +[static]EmptyIEnumerable 空の要素を列挙します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 任意の型の空の要素をIEnumerableジェネリックとして列挙します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIEnumerable<T>()
        {
            yield break;
        }
        #endregion (EmptyIEnumerable)
    }
}
