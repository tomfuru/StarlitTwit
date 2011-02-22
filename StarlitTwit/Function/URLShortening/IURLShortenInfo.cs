using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
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
}
