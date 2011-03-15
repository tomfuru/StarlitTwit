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
                cts = twitter.userstream_statuses_sample(Action);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cts != null) {
                cts.Cancel();
                cts = null;
            }
        }

        private void Action(string str)
        {
            XmlNode node = JsonConvert.DeserializeXmlNode(str,"status");

            XElement el = XmlNodeToXElement(node);

            string filename = string.Format("Xml/{0}.xml",DateTime.Now.ToString("yyMMddHHmmssffff"));
            using (StreamWriter writer = new StreamWriter(filename)) {
                writer.Write(el.ToString());
            }

            this.Invoke(new Action(() =>
            {
                richTextBox1.AppendText(str);
                richTextBox1.AppendText("\n");
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    string filename = ofd.FileName;
                    using (StreamWriter sw = new StreamWriter(filename,false)) {
                        sw.Write(richTextBox1.Text);
                    }
                }
            }
        }

        private XElement XmlNodeToXElement(XmlNode node)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xw = doc.CreateWriter()) {
                node.WriteTo(xw);
            }
            return doc.Root;
        }
    }
}
