using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StarlitTwit;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml;
using System.Xml.Linq;

namespace UserStreamTest
{
    public partial class Form1 : Form
    {
        SettingsData data;
        Twitter twitter;

        CancellationTokenSource cts = null;

        public Form1()
        {
            InitializeComponent();

            data = SettingsData.Restore();
            twitter = new Twitter();
            if (data.UserInfoList.Count > 0) {
                twitter.AccessToken = data.UserInfoList[0].AccessToken;
                twitter.AccessTokenSecret = data.UserInfoList[0].AccessTokenSecret;
            }
            else {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cts == null) {
                //cts = twitter.userstream_statuses_sample(Action);
                cts = twitter.userstream_user(ActionU, false);
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cts != null) {
                cts.Cancel();
                cts = null;
                button3.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void Action(string str)
        {
            XmlNode node = JsonConvert.DeserializeXmlNode(str, "status");

            XElement el = XmlNodeToXElement(node);

            string filename = string.Format("Xml/{0}.xml", DateTime.Now.ToString("yyMMddHHmmssffff"));
            using (StreamWriter writer = new StreamWriter(filename)) {
                writer.Write(el.ToString());
            }

            this.Invoke(new Action(() =>
            {
                bool scr = richTextBox1.SelectionStart == richTextBox1.TextLength;
                richTextBox1.AppendText(str);
                richTextBox1.AppendText("\n");
                if (scr) {
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    richTextBox1.ScrollToCaret();
                }
            }));
        }

        private void ActionU(UserStreamItemType type, object data)
        {
            StringBuilder sb = new StringBuilder("・");
            switch (type) {
                case UserStreamItemType.unknown:
                    sb.Append(string.Format("UnknownData(file:{0})", (string)data));
                    break;
                case UserStreamItemType.friendlist:
                    sb.Append(string.Format("FriendList (Num:{0})", ((IEnumerable<long>)data).Count()));
                    break;
                case UserStreamItemType.status:
                    TwitData t = (TwitData)data;
                    sb.Append(string.Format("{0} Status by {1}", t.Time.ToString(Utilization.STR_DATETIMEFORMAT)
                                                               , t.UserScreenName));
                    break;
                case UserStreamItemType.delete:
                    sb.Append(string.Format("Delete status_id:{0}", (long)data));
                    break;
                case UserStreamItemType.eventdata:
                    UserStreamEventData d = (UserStreamEventData)data;
                    switch (d.Type) {
                        case UserStreamEventType.favorite:
                            sb.Append(string.Format(string.Format("{0} {1} fav {2} 's tweet", 
                                                        d.Time.ToString(Utilization.STR_DATETIMEFORMAT),
                                                        d.SourceUser.ScreenName, d.TargetUser.ScreenName)));
                            break;
                        case UserStreamEventType.unfavorite:
                            sb.Append(string.Format(string.Format("{0} {1} unfav {2} 's tweet", 
                                                        d.Time.ToString(Utilization.STR_DATETIMEFORMAT),
                                                        d.SourceUser.ScreenName, d.TargetUser.ScreenName)));
                            break;
                        case UserStreamEventType.follow:
                            sb.Append(string.Format(string.Format("{0} {1} follow {2}", 
                                                        d.Time.ToString(Utilization.STR_DATETIMEFORMAT),
                                                        d.SourceUser.ScreenName, d.TargetUser.ScreenName)));
                            break;
                        case UserStreamEventType.block:
                            sb.Append(string.Format(string.Format("{0} {1} block {2}", 
                                                        d.Time.ToString(Utilization.STR_DATETIMEFORMAT),
                                                        d.SourceUser.ScreenName, d.TargetUser.ScreenName)));
                            break;
                        case UserStreamEventType.unblock:
                            sb.Append(string.Format(string.Format("{0} {1} unblock {2}", 
                                                        d.Time.ToString(Utilization.STR_DATETIMEFORMAT),
                                                        d.SourceUser.ScreenName, d.TargetUser.ScreenName)));
                            break;

                    }
                    break;
            }

            this.Invoke(new Action(() =>
            {
                richTextBox1.AppendText(sb.ToString());
                richTextBox1.AppendText("\n");
                richTextBox1.Invalidate();
            }));
        }

        //-------------------------------------------------------------------------------
        #region -ConvertToStreamItem XElementをUserStreamのアイテムに変換します。
        //-------------------------------------------------------------------------------
        //
        private object ConvertToStreamItem(XElement el)
        {
            if (el.Element("event") != null) {
                // status

            }
            else {
                // event

            }

            return null;
        }
        #endregion (ConvertToStreamItem)

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    string filename = ofd.FileName;
                    using (StreamWriter sw = new StreamWriter(filename, false)) {
                        sw.Write(richTextBox1.Text);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------
        #region -XmlNodeToXElement XmlNode->XElement
        //-------------------------------------------------------------------------------
        //
        private XElement XmlNodeToXElement(XmlNode node)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xw = doc.CreateWriter()) {
                node.WriteTo(xw);
            }
            return doc.Root;
        }
        #endregion (-XmlNodeToXElement)
    }
}
