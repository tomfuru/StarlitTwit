using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace StarlitTwit
{
    public static partial class PictureGetter
    {
        /// <summary>[static]読み込み可能な画像の拡張子</summary>
        private static readonly string[] IMAGE_EXTENSIONS;
        /// <summary>Thumbnailへのコンバータ</summary>
        private static readonly IThumbnailConverter[] CONVERTERS;

        //-------------------------------------------------------------------------------
        #region -(interface)IThumbnailConverter 画像サムネイルへのコンバータのインタフェース
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像へのURLをThumbnailに変換するためのメソッドを定義するクラスのインタフェースです。
        /// </summary>
        private interface IThumbnailConverter
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
        //-------------------------------------------------------------------------------
        #endregion (-(interface)IThumbnailConverter)

        //-------------------------------------------------------------------------------
        #region static コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static PictureGetter()
        {
            IMAGE_EXTENSIONS = GetImageCodecInfo();
            CONVERTERS = new IThumbnailConverter[] { 
                            new TwitpicConverter(),
                            new PhotozouConverter(),
                            new yFrogConverter(),
                            new img_lyConverter()
                        };
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +[static]IsPictureURL 画像URLかどうか判断します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像URLかどうか判断します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static bool IsPictureURL(string url)
        {
            return IMAGE_EXTENSIONS.Any(extention => url.EndsWith(extention))
                || CONVERTERS.Any(converter => converter.IsEffectiveURL(url));
        }
        #endregion (IsPictureURL)

        //-------------------------------------------------------------------------------
        #region +[static]ConvertURL URLを変換します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを画像のURLに変換します。
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static string ConvertURL(string url)
        {
            if (IMAGE_EXTENSIONS.Any(extention => url.EndsWith(extention))) { return url; }
            else {
                foreach (var converter in CONVERTERS) {
                    if (converter.IsEffectiveURL(url)) {
                        return converter.ConvertToThumbnailURL(url);
                    }
                }
            }
            return null;
        }
        #endregion (ConvertURL)

        //-------------------------------------------------------------------------------
        #region +[static]ConvertURLs URLを変換します。無効なURLは返りません。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// URLを変換します。無効なURLは返りません。
        /// </summary>
        /// <param name="urls">URL</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertURLs(IEnumerable<string> urls)
        {
            foreach (string url in urls) {
                string thumburl = ConvertURL(url);
                if (thumburl != null) { yield return thumburl; }
            }
        }
        #endregion (ConvertURLs)

        //-------------------------------------------------------------------------------
        #region -[static]GetImageCodecInfo 画像コーデック情報取得
        //-------------------------------------------------------------------------------
        //
        private static string[] GetImageCodecInfo()
        {
            List<string> extensionList = new List<string>();
            ImageCodecInfo[] infoarray = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo info in infoarray) {
                extensionList.AddRange(info.FilenameExtension.Split(';').Select((s) => s.Remove(0, 2)));
            }
            return extensionList.ToArray();
        }
        //-------------------------------------------------------------------------------
        #endregion (GetImageCodecInfo)
    }
}
