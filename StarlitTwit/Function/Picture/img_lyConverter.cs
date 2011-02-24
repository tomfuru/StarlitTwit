using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    public class img_lyConverter : IThumbnailConverter
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
}
