using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    public class yFrogConverter : IThumbnailConverter
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
}
