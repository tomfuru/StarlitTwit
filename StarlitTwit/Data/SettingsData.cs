﻿using System;
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
    public class SettingsData : SaveDataClassBase<SettingsData>
    {
        //-------------------------------------------------------------------------------
        #region 定数
        //-------------------------------------------------------------------------------
        /// <summary>名前のデフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFORMAT = @"$Locked$$ScreenName$/$Name$ [$DateTime(MM/dd HH:mm:ss)$] (from $Source$)$Favorited$%(Retweeted by $RTCount$ people)%";
        /// <summary>名前のリツイート時デフォルトフォーマット</summary>
        public const string DEFAULT_NAMEFOEMAT_RETWEET = @"$ScreenName$/$Name$(RT:$Retweeter$[$DateTime(HH:mm:ss)$])[$RTDateTime(MM/dd HH:mm:ss)$] (from $Source$)$Favorited$%(Retweeted by $RTCount$ people)%";
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
        #region UserStream関係
        //-------------------------------------------------------------------------------
        /// <summary>UserStreamのスタートアップ</summary>
        public bool UserStreamStartUp = false;
        /// <summary>UserStreamのAll_Repliesオプション</summary>
        public bool UserStreamAllReplies = false;
        /// <summary>UserStream開始と同時にログ画面表示</summary>
        public bool UserStreamAutoOpenLog = false;
        /// <summary>お気に入り追加時にPopup通知</summary>
        public bool UserStream_ShowPopup_Favorite = true;
        /// <summary>お気に入り削除時にPopup通知</summary>
        public bool UserStream_ShowPopup_Unfavorite = true;
        /// <summary>フォローイベント時にPopup通知</summary>
        public bool UserStream_ShowPopup_Follow = true;
        /// <summary>ブロック時にPopup通知</summary>
        public bool UserStream_ShowPopup_Block = true;
        /// <summary>ブロック解除時にPopup通知</summary>
        public bool UserStream_ShowPopup_Unblock = true;
        /// <summary>リストメンバー追加時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListMemberAdd = true;
        /// <summary>リストメンバー削除時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListMemberRemoved = true;
        /// <summary>リスト作成時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListCreated = true;
        /// <summary>リスト更新時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListUpdated = true;
        /// <summary>リスト削除時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListDestroyed = true;
        /// <summary>リストフォロー時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListSubscribed = true;
        /// <summary>リストフォロー解除時にPopup通知</summary>
        public bool UserStream_ShowPopup_ListUnsubscribed = true;
        /// <summary>プロフィールアップデート時にPopup通知</summary>
        public bool UserStream_ShowPopup_UserUpdate = true;
        /// <summary>自分の発言がRetweetされた時にPopup通知</summary>
        public bool UserStream_ShowPopup_Retweet = true;
        //-------------------------------------------------------------------------------
        #endregion (UserStream関係)
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
        /// <summary>Youtubeサムネイル</summary>
        public YoutubeThumbnailType ThumbType_youtube;
        /// <summary>ニコニコ動画サムネイル</summary>
        public NicovideoThumbnailType ThumbType_nicovideo;
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
        /// <summary>instagramサムネイル</summary>
        public instagramThumbnailType ThumbType_instagram;
        //-------------------------------------------------------------------------------
        #endregion (画像サムネイル関係)
        //-------------------------------------------------------------------------------
        #region その他
        //-------------------------------------------------------------------------------
        /// <summary>フィルターデータ</summary>
        public StatusFilterInfo[] Filters = new StatusFilterInfo[0];
        /// <summary>フォロー・フォロー解除時に確認ダイアログを表示する</summary>
        public bool ConfirmDialogFollow = true;
        /// <summary>お気に入り追加・削除時に確認ダイアログを表示する</summary>
        public bool ConfirmDialogFavorite = true;
        /// <summary>ブロック・ブロック解除時に確認ダイアログを表示する</summary>
        public bool ConfirmDialogBlock = true;
        /// <summary>タブデータ</summary>
        public SerializableDictionary<string, TabData> TabDataDic = new SerializableDictionary<string,TabData>();
        /// <summary>キー</summary>
        public List<UserAuthInfo> UserInfoList = new List<UserAuthInfo>();
        //-------------------------------------------------------------------------------
        #endregion (その他)
        //-------------------------------------------------------------------------------
        #endregion (メンバー変数)

        //-------------------------------------------------------------------------------
        #region +[override]Save 保存
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このインスタンスをファイルに保存します。失敗した時は別の場所にファイルを保存するかどうか尋ねます。
        /// </summary>
        public override void Save(string filePath)
        {
            const string MESSAGE_1 = "設定の保存ができませんでした。\n設定を別の名前で保存しますか？";
            const string MESSAGE_2 = "設定を別の名前で保存しますか？";
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));
            if (!this.SaveBase(filePath)) {
                string dispMessage = MESSAGE_1;
                do {
                    if (Message.ShowQuestionMessage(dispMessage) == DialogResult.Yes) {
                        using (SaveFileDialog sfd = new SaveFileDialog()) {
                            sfd.FileName = string.Format("{0}_tmp.dat" ,Path.GetFileNameWithoutExtension(filePath));
                            sfd.Filter = "StarlitTwit設定ファイル(*.dat)|*.dat";
                            if (sfd.ShowDialog() == DialogResult.OK) {
                                if (!this.SaveBase(sfd.FileName)) {
                                    dispMessage = MESSAGE_1;
                                    continue; 
                                }
                                break;
                            }
                            else { dispMessage = MESSAGE_2; }
                        }
                    }
                    else { break; }
                } while (true);
            }
        }
        #endregion (Save)
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
    //-----------------------------------------------------------------------------------
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

    //-----------------------------------------------------------------------------------
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
}
