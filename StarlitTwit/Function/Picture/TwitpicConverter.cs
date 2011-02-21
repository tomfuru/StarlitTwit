using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace StarlitTwit
{
    class TwitpicConverter : IThumbnailConverter
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
}
