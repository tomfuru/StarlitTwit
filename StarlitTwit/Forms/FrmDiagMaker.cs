using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace StarlitTwit
{
    // TODO:"日替わり"かどうかの表示/StatusBarへの情報表示，データがwebから取れなかった時の処理
    public partial class FrmDiagMaker : Form
    {
        private const string URL_FORMAT = @"http://shindanmaker.com/{0}";

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmDiagMaker(int diagNo, string screen_name)
        {
            InitializeComponent();

            llbllink.Text = string.Format(URL_FORMAT, diagNo);

            txtName.Text = screen_name;
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GetHomeInfo();
        }
        #endregion (#[override]OnLoad)

        //-------------------------------------------------------------------------------
        #region btnClose_Click 閉じる
        //-------------------------------------------------------------------------------
        //
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnClose_Click)

        //-------------------------------------------------------------------------------
        #region txtName_KeyDown
        //-------------------------------------------------------------------------------
        //
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                btnDiag.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        #endregion (txtName_KeyDown)

        //-------------------------------------------------------------------------------
        #region txtName_TextChanged Text変更時
        //-------------------------------------------------------------------------------
        //
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnDiag.Enabled = (txtName.Text.Length > 0);
        }
        #endregion (txtName_TextChanged)

        //-------------------------------------------------------------------------------
        #region btnDiag_Click 診断する
        //-------------------------------------------------------------------------------
        //
        private void btnDiag_Click(object sender, EventArgs e)
        {
            GetResult();
        }
        #endregion (btnDiag_Click)

        //-------------------------------------------------------------------------------
        #region txtResult_TextChanged Text変更時
        //-------------------------------------------------------------------------------
        //
        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            btnTweet.Enabled = (txtResult.Text.Length > 0);
        }
        #endregion (txtResult_TextChanged)

        //-------------------------------------------------------------------------------
        #region btnTweet_Click 発言
        //-------------------------------------------------------------------------------
        //
        private void btnTweet_Click(object sender, EventArgs e)
        {
            Status_update(txtResult.Text);
        }
        #endregion (btnTweet_Click)

        //-------------------------------------------------------------------------------
        #region llbllink_LinkClicked リンククリック
        //-------------------------------------------------------------------------------
        //
        private void llbllink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilization.OpenBrowser(llbllink.Text, FrmMain.SettingsData.UseInternalWebBrowser, false);
        }
        #endregion (llbllink_LinkClicked)

        //-------------------------------------------------------------------------------
        #region -GetHomeInfo 入力フォームの情報を取得
        //-------------------------------------------------------------------------------
        //
        private void GetHomeInfo()
        {
            const string TITLE_BEGIN = "<title>";
            const string TITLE_END = "</title>";
            const string DESCRIPTION_BEGIN = @"<meta name=""description"" content=""";
            const string DESCTIPTION_END = @"- 診断メーカー"" />";
            const string INFO_REGEXPATTERN = @"<b>(.+?)</b>人が診断 結果パターン <b>(.+?)</b>通り";

            WebRequest req = WebRequest.Create(llbllink.Text);
            WebResponse res = req.GetResponse();
            using (Stream stream = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream)) {
                string str = sr.ReadToEnd();

                Match m_title = Regex.Match(str, string.Format("{0}(.*?){1}", TITLE_BEGIN, TITLE_END));
                if (m_title == null) { Debug.Assert(false); return; }
                this.Text = lblTitle.Text = m_title.Groups[1].Value.Trim(); // Title

                Match m_description = Regex.Match(str, string.Format("{0}(.*?){1}", DESCRIPTION_BEGIN,DESCTIPTION_END));
                if (m_description == null) { Debug.Assert(false); return; }
                
                Match m_info = Regex.Match(str, INFO_REGEXPATTERN);
                if (m_info == null) { Debug.Assert(false); return; }

                lblDescription.Text = string.Format("{0}\n{1}人が診断, 結果パターン{2}通り", 
                                                    m_description.Groups[1].Value.Trim(), 
                                                    m_info.Groups[1].Value.Trim(), 
                                                    m_info.Groups[2].Value.Trim());
            }
            res.Close();
        }
        #endregion (GetHomeInfo)

        //-------------------------------------------------------------------------------
        #region -GetResult 結果取得
        //-------------------------------------------------------------------------------
        //
        private void GetResult()
        {
            string boundary = Environment.TickCount.ToString();
            Encoding enc = Encoding.GetEncoding("shift_jis");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(llbllink.Text);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            string postData =
                string.Format(
@"--{0}
Content-Disposition: form-data; name=""u""

{1}
--{0}
Content-Disposition: form-data; name=""from""

--{0}--"
, boundary, txtName.Text);
             //string postData2 = "--" + boundary + "\r\n" +
             //                   "Content-Disposition: form-data; name=\"u\"\r\n\r\n" +
             //                   txtName.Text + "\r\n" +
             //                   "--" + boundary + "\r\n" +
             //                   "Content-Disposition: form-data; name=\"from\"\r\n\r\n" +
             //                   "--" + boundary + "--";

            byte[] data = enc.GetBytes(postData);

            req.ContentLength = data.Length;
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            using (Stream stream = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream)) {
                const string TEXT_BEGIN = "<textarea .*?>";
                const string TEXT_END = "</textarea>";
                string str = sr.ReadToEnd();

                Match m_text = Regex.Match(str, string.Format("{0}(.*){1}", TEXT_BEGIN, TEXT_END));
                if (m_text == null) { Debug.Assert(false); return; }
                txtResult.Text = m_text.Groups[1].Value.Trim();
            }
        }
        #endregion (GetResult)

        //-------------------------------------------------------------------------------
        #region -Status_update 発言 using TwitterClient
        //-------------------------------------------------------------------------------
        //
        private void Status_update(string message)
        {
            try {
                FrmMain.Twitter.statuses_update(message);
            }
            catch (TwitterAPIException) {

            }
        }
        #endregion (Status_update)
    }
}
