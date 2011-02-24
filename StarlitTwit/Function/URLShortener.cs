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
