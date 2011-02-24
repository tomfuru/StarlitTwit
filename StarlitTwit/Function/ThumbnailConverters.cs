using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace StarlitTwit
{
    partial class PictureGetter
    {
        //-------------------------------------------------------------------------------
        #region Twitpic Converter
        //-------------------------------------------------------------------------------
        private class TwitpicConverter : IThumbnailConverter
        {

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                const string TWITPIC = @"twitpic.com";

                string hostname = Utilization.GetHostName(url);
                if (hostname == null) { return false; }

                IPHostEntry entry = Dns.GetHostEntry(hostname);
                IPHostEntry twitpicentry = Dns.GetHostEntry(TWITPIC);
                return (entry.AddressList.Except(twitpicentry.AddressList).Count() == 0);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                string[] urlpartials = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                return urlpartials[0] + "//" + urlpartials[1] + "/show/thumb/" + urlpartials[2];
            }
        }
        #endregion (TwitpicConverter)

        //-------------------------------------------------------------------------------
        #region Photozou Converter
        //-------------------------------------------------------------------------------
        private class PhotozouConverter : IThumbnailConverter
        {
            private const string DOMAIN = @"http://photozou.jp/";
            private const string INFOAPI_URL = @"http://api.photozou.jp/rest/photo_info";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                string id = url.Split('/').Last();
                string apiurl = INFOAPI_URL + @"?photo_id=" + id;
                try {
                    WebRequest req = WebRequest.Create(apiurl);
                    WebResponse res = req.GetResponse();
                    using (Stream stream = res.GetResponseStream()) {
                        XElement el = XElement.Load(stream);
                        return el.Element("info").Element("photo").Element("thumbnail_image_url").Value;
                    }
                }
                catch (Exception) { return null; }
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (Photozou Converter)

        //-------------------------------------------------------------------------------
        #region yFrog Converter
        //-------------------------------------------------------------------------------
        private class yFrogConverter : IThumbnailConverter
        {
            private const string DOMAIN = @"http://yfrog.com/";
            private const string THUMBNAIL = ".th.jpg";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                return url + THUMBNAIL;
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (yFrog Converter)

        //-------------------------------------------------------------------------------
        #region img.ly Converter
        //-------------------------------------------------------------------------------
        private class img_lyConverter : IThumbnailConverter
        {
            private const string DOMAIN = @"http://img.ly/";

            bool IThumbnailConverter.IsEffectiveURL(string url)
            {
                return url.StartsWith(DOMAIN);
            }

            string IThumbnailConverter.ConvertToThumbnailURL(string url)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DOMAIN);
                sb.Append("show/thumb/");
                sb.Append(url.Split('/').Last());
                return sb.ToString();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion (img.ly Converter)
    }
}
