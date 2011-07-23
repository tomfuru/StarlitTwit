using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Drawing;

namespace StarlitTwit
{
    /// <summary>
    /// ImageListのWrapperです。画像取得・参照等はこのクラスのインスタンスのメソッドを利用し，ImageListプロパティを通じて行ってはならない。
    /// </summary>
    public class ImageListWrapper : Component
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>
        /// <para>内部のImageListを取得します。</para>
        /// <para>【注意】画像の追加・参照等はこのインスタンスを通して行わない。</para>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageList ImageList { get; private set; }
        /// <summary>画像取得スレッド</summary>
        private Thread _thread = null;
        /// <summary>URLキュー</summary>
        private List<string> _urlList = new List<string>();
        /// <summary>ImageList操作時ロックする</summary>
        private object _objLock = new object();
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public ImageListWrapper()
            : base()
        {
            ImageList = new ImageList();
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +GetImage 画像取得
        //-------------------------------------------------------------------------------
        //
        public Image GetImage(string key)
        {
            lock (_objLock) {
                return ImageList.Images[key];
            }
        }
        #endregion (GetImage)
        //-------------------------------------------------------------------------------
        #region +ImageAdd 画像追加
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を追加します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="image">画像</param>
        public void ImageAdd(string key, Image image)
        {
            lock (_objLock) {
                ImageList.Images.Add(key, image);
            }
        }
        #endregion (ImageAdd)
        //-------------------------------------------------------------------------------
        #region +ImageContainsKey 画像リストに指定キー項目があるか
        //-------------------------------------------------------------------------------
        //
        public bool ImageContainsKey(string key)
        {
            lock (_objLock) {
                return ImageList.Images.ContainsKey(key);
            }
        }
        #endregion (ImageContainsKey)

        //-------------------------------------------------------------------------------
        #region +RequestAddImages イメージの取得要請
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 画像を取得するように要請します。
        /// </summary>
        /// <param name="urls">URL</param>
        public void RequestAddImages(IEnumerable<string> urls)
        {
            lock (_urlList) {
                _urlList.AddRange(urls.Distinct().Where((url) => !_urlList.Contains(url)));

                if (_thread == null || !_thread.IsAlive) {
                    _thread = new Thread(GetImages);
                    _thread.IsBackground = true;
                    _thread.Start();
                }
            }
        }
        #endregion (RequestAddImages)

        //-------------------------------------------------------------------------------
        #region -GetImages 画像取得（別スレッド）
        //-------------------------------------------------------------------------------
        //
        private void GetImages()
        {
            while (true) {
                string url;
                lock (_urlList) {
                    if (_urlList.Count == 0) { return; }
                    url = _urlList[0];
                    _urlList.RemoveAt(0);
                }
                if (!ImageContainsKey(url)) {
                    Image img = Utilization.GetImageFromURL(url);
                    
                    if (img != null) { ImageAdd(url, img); }
                }
            }
        }
        #endregion (GetImages)
    }
}
