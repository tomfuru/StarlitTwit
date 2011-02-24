using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace StarlitTwit
{
    public class PhotozouConverter : IThumbnailConverter
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
}
