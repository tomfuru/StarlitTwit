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
    /// ImageListのWrapperです
    /// </summary>
    public class ImageListWrapper : Component
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>内部のImageListを取得します。</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageList ImageList { get; private set; }
        /// <summary>画像取得スレッド</summary>
        private Thread _thread = null;
        /// <summary>URLキュー</summary>
        private List<string> _urlList = new List<string>();
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
                    _thread.IsBackground = false;
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
                if (!ImageList.Images.ContainsKey(url)) {
                    Image img = Utilization.GetImageFromURL(url);
                    if (img != null) { ImageList.Images.Add(url, img); }
                }
            }
        }
        #endregion (GetImages)
    }
}
