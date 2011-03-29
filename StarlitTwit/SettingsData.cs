using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Drawing;

namespace StarlitTwit
{
    /// <summary>
    /// 設定データ
    /// </summary>
    public class SettingsData
    {
        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        /// <summary>保存ファイル名</summary>
        public const string SAVEFILE_NAME = @"Settings.dat";
        /// <summary>名前のデフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFORMAT = @"$Locked$$ScreenName$/$Name$ [$DateTime(MM/dd HH:mm:ss)$] (from $Source$)$Favorited$";
        /// <summary>名前のリツイート時デフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFOEMAT_RETWEET = @"$ScreenName$/$Name$(RT:$Retweeter$[$DateTime(HH:mm:ss)$])[$RTDateTime(MM/dd HH:mm:ss)$] (from $Source$)$Favorited$";
        /// <summary>名前のダイレクトメッセージ時デフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFOEMAT_DM = @"$ScreenName$/$Name$ [$DateTime(yyyy/MM/dd HH:mm:ss)$] (to $Recipient$)";
        /// <summary>名前のデフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFORMAT_SEARCH = @"$ScreenName$ [$DateTime(MM/dd HH:mm:ss)$] (from $Source$)";
        /// <summary>下段のデフォルトフォント</summary>
        public static readonly Font DEFAULT_FONT_TEXT = new Font("MS UI Gothic", 9);
        /// <summary>上段のデフォルトフォント</summary>
        public static readonly Font DEFAULT_FONT_TITLE = new Font("MS UI Gothic", 9, FontStyle.Bold);
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region メンバー変数
        //-------------------------------------------------------------------------------
        #region フォーム関係
        //-------------------------------------------------------------------------------
        /// <summary>ウィンドウサイズ</summary>
        public Size WindowSize = new Size(460, 515);
        /// <summary>ウィンドウ位置</summary>
        public Point WindowPosition = new Point(-1, -1);
        /// <summary>最大化されていたか</summary>
        public bool WindowMaximized = false;
        /// <summary>タブの位置</summary>
        public TabAlignment TabAlignment = TabAlignment.Top;
        //-------------------------------------------------------------------------------
        #endregion (フォーム関係)

        //-------------------------------------------------------------------------------
        #region 表示関係
        //-------------------------------------------------------------------------------
        /// <summary>アイコンの表示有無</summary>
        public bool DisplayIcon = true;
        /// <summary>アイコンのサイズ</summary>
        public int IconSize = 48;
        /// <summary>画像サムネイルの表示有無</summary>
        public bool DisplayThumbnail = true;
        /// <summary>画像サムネイルの表示間隔(複数ある時)</summary>
        public int DisplayThumbnailInterval = 3000;
        /// <summary>リプライツールチップを出すかどうか</summary>
        public bool DisplayReplyToolTip = true;
        /// <summary>リプライツールチップの深さ(0だと無限)</summary>
        public int DisplayReplyToolTipDepth = 0;
        /// <summary>リプライがきた時にタスクバーバルーンを表示するか</summary>
        public bool DisplayReplyBaloon = true;
        /// <summary>DMがきた時にタスクバーバルーンを表示するか</summary>
        public bool DisplayDMBaloon = true;
        //-------------------------------------------------------------------------------
        #endregion (表示関係)

        //-------------------------------------------------------------------------------
        #region フォント・背景色
        //-------------------------------------------------------------------------------
        /// <summary>通常ツイートのタイトルのフォント</summary>
        public SerializableFont FontNormalTweetTitle = DEFAULT_FONT_TITLE;
        /// <summary>通常ツイートのタイトルのフォントカラー</summary>
        public SerializableColor ColorNormalTweetTitle = Color.Black;
        /// <summary>通常ツイートのテキストのフォント</summary>
        public SerializableFont FontNormalTweetText = DEFAULT_FONT_TEXT;
        /// <summary>通常ツイートのテキストのフォントカラー</summary>
        public SerializableColor ColorNormalTweetText = Color.Black;
        /// <summary>通常ツイートの未選択時の背景色</summary>
        public SerializableColor ColorNormalTweetBackUnselected = Color.White;
        /// <summary>通常ツイートの選択時の背景色</summary>
        public SerializableColor ColorNormalTweetBackSelected = Color.LightYellow;
        /// <summary>RTのタイトルのフォント</summary>
        public SerializableFont FontRTTweetTitle = DEFAULT_FONT_TITLE;
        /// <summary>RTのタイトルのフォントカラー</summary>
        public SerializableColor ColorRTTweetTitle = Color.Black;
        /// <summary>RTのテキストのフォント</summary>
        public SerializableFont FontRTTweetText = DEFAULT_FONT_TEXT;
        /// <summary>RTのテキストのフォントカラー</summary>
        public SerializableColor ColorRTTweetText = Color.Black;
        /// <summary>RTツイートの未選択時の背景色</summary>
        public SerializableColor ColorRTTweetBackUnselected = Color.LightGreen;
        /// <summary>RTツイートの選択時の背景色</summary>
        public SerializableColor ColorRTTweetBackSelected = Color.LimeGreen;
        /// <summary>自分へのReplyのタイトルのフォント</summary>
        public SerializableFont FontReplyToMeTweetTitle = DEFAULT_FONT_TITLE;
        /// <summary>自分へのReplyのタイトルのフォントカラー</summary>
        public SerializableColor ColorReplyToMeTweetTitle = Color.Black;
        /// <summary>自分へのReplyのテキストのフォント</summary>
        public SerializableFont FontReplyToMeTweetText = DEFAULT_FONT_TEXT;
        /// <summary>自分へのReplyのテキストのフォントカラー</summary>
        public SerializableColor ColorReplyToMeTweetText = Color.Black;
        /// <summary>自分へのReplyツイートの未選択時の背景色</summary>
        public SerializableColor ColorReplyToMeTweetBackUnselected = Color.LightPink;
        /// <summary>自分へのReplyツイートの選択時の背景色</summary>
        public SerializableColor ColorReplyToMeTweetBackSelected = Color.HotPink;
        /// <summary>他人へのReplyのタイトルのフォント</summary>
        public SerializableFont FontReplyToOtherTweetTitle = DEFAULT_FONT_TITLE;
        /// <summary>他人へのReplyのタイトルのフォントカラー</summary>
        public SerializableColor ColorReplyToOtherTweetTitle = Color.Black;
        /// <summary>他人へのReplyのテキストのフォント</summary>
        public SerializableFont FontReplyToOtherTweetText = DEFAULT_FONT_TEXT;
        /// <summary>他人へのReplyのテキストのフォントカラー</summary>
        public SerializableColor ColorReplyToOtherTweetText = Color.Black;
        /// <summary>他人へのReplyツイートの未選択時の背景色</summary>
        public SerializableColor ColorReplyToOtherTweetBackUnselected = Color.LightSkyBlue;
        /// <summary>他人へのReplyツイートの選択時の背景色</summary>
        public SerializableColor ColorReplyToOtherTweetBackSelected = Color.DeepSkyBlue;
        //-------------------------------------------------------------------------------
        #endregion (フォント・背景色)

        //-------------------------------------------------------------------------------
        #region フォーマット
        //-------------------------------------------------------------------------------
        /// <summary>ヘッダー</summary>
        public string Header = "";
        /// <summary>フッター</summary>
        public string Footer = "";
        /// <summary>名前のフォーマット(非リツイート時)</summary>
        public string NameFormat = DEFAULT_NAMEFORMAT;
        /// <summary>名前のフォーマット(リツイート時)</summary>
        public string NameFormatRetweet = DEFAULT_NAMEFOEMAT_RETWEET;
        /// <summary>名前のフォーマット(ダイレクトメッセージ時)</summary>
        public string NameFormatDM = DEFAULT_NAMEFOEMAT_DM;
        /// <summary>名前のフォーマット(検索時)</summary>
        public string NameFormatSearch = DEFAULT_NAMEFORMAT_SEARCH;
        /// <summary>引用の種類</summary>
        public QuoteType QuoteType = QuoteType.QT;
        //-------------------------------------------------------------------------------
        #endregion (フォーマット)

        //-------------------------------------------------------------------------------
        #region 取得関係
        //-------------------------------------------------------------------------------
        /// <summary>タイムラインで最初に取得するツイートの数</summary>
        public int FirstGetNum_Home = 50;
        /// <summary>リプライで最初に取得するツイートの数</summary>
        public int FirstGetNum_Reply = 30;
        /// <summary>発言履歴で最初に取得するツイートの数</summary>
        public int FirstGetNum_History = 30;
        /// <summary>ダイレクトメッセージで最初に取得するツイートの数</summary>
        public int FirstGetNum_Direct = 20;

        /// <summary>タイムラインで更新時に取得するツイートの数</summary>
        public int RenewGetNum_Home = 20;
        /// <summary>リプライで更新時に取得するツイートの数</summary>
        public int RenewGetNum_Reply = 20;
        /// <summary>発言履歴で更新時に取得するツイートの数</summary>
        public int RenewGetNum_History = 20;
        /// <summary>ダイレクトメッセージで更新時に取得するツイートの数</summary>
        public int RenewGetNum_Direct = 20;

        /// <summary>タイムラインでツイートを取得する間隔(秒)</summary>
        public int GetInterval_Home = 60;
        /// <summary>リプライでツイートを取得する間隔(秒)</summary>
        public int GetInterval_Reply = 60;
        /// <summary>発言履歴でツイートを取得する間隔(秒)</summary>
        public int GetInterval_History = 600;
        /// <summary>ダイレクトメッセージでツイートを取得する間隔(秒)</summary>
        public int GetInterval_Direct = 180;

        /// <summary>プロフィールを取得する間隔(秒)</summary>
        public int GetInterval_Profile = 600;
        //-------------------------------------------------------------------------------
        #endregion (ツイート取得関係)

        //-------------------------------------------------------------------------------
        #region URL関係
        //-------------------------------------------------------------------------------
        /// <summary>ウェブブラウザパス</summary>
        public string WebBrowserPath = "";
        /// <summary>内部ウェブブラウザを使うか</summary>
        public bool UseInternalWebBrowser = false;
        /// <summary>URL短縮のタイプ</summary>
        public URLShortenType URLShortenType = URLShortenType.bit_ly;
        //-------------------------------------------------------------------------------
        #endregion (URL関係)

        //-------------------------------------------------------------------------------
        #region 画像サムネイル関係
        //-------------------------------------------------------------------------------
        /// <summary>TwitPicサムネイル</summary>
        public TwitPicThumbnailType ThumbType_twitpic;
        /// <summary>movapicサムネイル</summary>
        public movapicThumbnailType ThumbType_movapic;
        /// <summary>photozouサムネイル</summary>
        public PhotozouThumbnailType ThumbType_photozou;
        /// <summary>img_lyサムネイル</summary>
        public imglyThumbnailType ThumbType_img_ly;
        /// <summary>yFrogサムネイル</summary>
        public yFrogThumbnailType ThumbType_yFrog;
        /// <summary>plixi,tweetphotoサムネイル</summary>
        public plixiThumbnailType ThumbType_plixi;
        /// <summary>ow.lyサムネイル</summary>
        public owlyThumbnailType ThumbType_ow_ly;
        /// <summary>TwipplePhotoサムネイル</summary>
        public twipplephotoThumbnailType ThumbType_twipplephoto;
        //-------------------------------------------------------------------------------
        #endregion (画像サムネイル関係)

        //-------------------------------------------------------------------------------
        #region その他
        //-------------------------------------------------------------------------------
        /// <summary>タブデータ</summary>
        public SerializableDictionary<string, TabData> TabDataDic;
        /// <summary>キー</summary>
        public List<UserInfo> UserInfoList;
        //-------------------------------------------------------------------------------
        #endregion (その他)

        //-------------------------------------------------------------------------------
        #endregion (メンバー変数)

        //-------------------------------------------------------------------------------
        #region +Save 保存
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このインスタンスをファイルに保存します。
        /// </summary>
        public void Save()
        {
            string filePath = Path.Combine(Application.StartupPath, SAVEFILE_NAME);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));
            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex) {
                Message.ShowErrorMessage(ex.ToString(), "設定の保存ができませんでした。");
            }
        }
        #endregion (Save)

        //-------------------------------------------------------------------------------
        #region +[static]Restore 復元
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ファイルから設定を復元します。復元できなかった時は新しい設定データが返ります。
        /// </summary>
        /// <returns></returns>
        public static SettingsData Restore()
        {
            string filePath = Path.Combine(Application.StartupPath, SAVEFILE_NAME);

            if (File.Exists(filePath)) {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));
                try {
                    using (FileStream fs = File.OpenRead(filePath)) {
                        using (XmlReader reader = XmlReader.Create(fs)) {
                            if (serializer.CanDeserialize(reader)) {
                                fs.Seek(0, SeekOrigin.Begin);
                                return (SettingsData)serializer.Deserialize(fs);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Message.ShowErrorMessage(ex.ToString(), "設定が取得できませんでした。");
                }
            }
            return new SettingsData();
        }
        #endregion (Restore)
    }
    //-----------------------------------------------------------------------------------
    #region +QuoteType 列挙体：引用の種類
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// 引用の種類
    /// </summary>
    public enum QuoteType : byte
    {
        /// <summary>QT @(ユーザー名): (発言)</summary>
        QT,
        /// <summary>RT @(ユーザー名): (発言)</summary>
        RT,
        /// <summary>“@(ユーザー名): (発言)”</summary>
        DoubleQuotation
    }
    //-----------------------------------------------------------------------------------
    #endregion (QuoteType)
    //-------------------------------------------------------------------------------
    #region +TabSearchType 列挙体：作成タブの検索タイプです。
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 作成タブの検索タイプです。
    /// </summary>
    public enum TabSearchType : byte
    {

        /// <summary>キーワード検索</summary>
        Keyword,
        /// <summary>ユーザー</summary>
        User,
        /// <summary>リスト</summary>
        List
    }
    //-------------------------------------------------------------------------------
    #endregion (TabSearchType)

    //-------------------------------------------------------------------------
    #region +TabData 構造体：タブの情報
    //-------------------------------------------------------------------------------
    /// <summary>
    /// タブ情報
    /// </summary>
    [Serializable]
    public class TabData
    {
        /// <summary>タブの名前</summary>
        public string TabName;

        /// <summary>検索の種類</summary>
        public TabSearchType SearchType;
        /// <summary>(SearchType == List)のみ．タブオーナーのScreenName</summary>
        public string ListOwner;
        /// <summary>検索に関する文字列情報</summary>
        public string SearchWord;

        /// <summary>初めて呟きを取得する時の取得数</summary>
        public int FirstGetNum;
        /// <summary>更新時呟きを取得する時の取得数</summary>
        public int RenewGetNum;
        /// <summary>呟きの取得間隔(秒)</summary>
        public int GetInterval;
    }
    //-------------------------------------------------------------------------------
    #endregion (TabData)
    //-----------------------------------------------------------------------------------
    #region +UserData 構造体：ユーザーデータ
    //-------------------------------------------------------------------------------
    /// <summary>
    /// ユーザー名やOAuth認証のためのユーザートークンを格納する構造体です。
    /// </summary>
    [Serializable]
    public struct UserInfo
    {
        public string ScreenName;
        public long ID;

        public string AccessToken;
        public string AccessTokenSecret;
    }
    //-------------------------------------------------------------------------------
    #endregion (UserData)
}
