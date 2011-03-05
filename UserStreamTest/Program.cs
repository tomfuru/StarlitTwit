using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using StarlitTwit;

namespace UserStreamTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingsData data = SettingsData.Restore();

            Twitter twitter = new Twitter();
            
            

            twitter.userstream_statuses_sample();
        }
    }
}
