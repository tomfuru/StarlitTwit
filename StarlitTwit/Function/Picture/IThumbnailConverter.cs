using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    /// <summary>
    /// 画像へのURLをThumbnailに変換するためのメソッドを定義するクラスのインタフェースです。
    /// </summary>
    public interface IThumbnailConverter
    {
        /// <summary>
        /// URLをThumbnailURLに変換できるかどうか
        /// </summary>
        /// <returns>できるかどうか</returns>
        bool IsEffectiveURL(string url);

        /// <summary>
        /// URLをThumbailのURLに変換します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>ThumbnailのURL</returns>
        string ConvertToThumbnailURL(string url);
    }
}
