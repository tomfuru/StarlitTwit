using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace StarlitTwit
{
    public partial class FrmDispTweet : Form
    {
        private Thread _getThread = null;
        private readonly TwitData _startTwitdata = null;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispTweet(TwitData data,ImageList imageList)
        {
            InitializeComponent();

            _getThread = new Thread(new ParameterizedThreadStart(GetReplies));
            _getThread.IsBackground = true;

            _startTwitdata = data;
            uctlDispTwit.ImageList = imageList;
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region UctlDispTwit プロパティ：表示ユーザーコントロールを取得します
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 表示ユーザーコントロールを取得します
        /// </summary>
        public UctlDispTwit UctlDispTwit
        {
            get { return uctlDispTwit; }
        }
        #endregion (UctlDispTwit)

        //-------------------------------------------------------------------------------
        #region FrmReply_Shown フォーム表示時
        //-------------------------------------------------------------------------------
        //
        private void FrmReply_Shown(object sender, EventArgs e)
        {
            uctlDispTwit.AddData(new TwitData[] { _startTwitdata });
            _getThread.Start(_startTwitdata.Mention_StatusID);
        }
        //-------------------------------------------------------------------------------
        #endregion (FrmReply_Shown)

        //-------------------------------------------------------------------------------
        #region -GetReplies (別スレッド：リプライ取得)
        //-------------------------------------------------------------------------------
        //
        private void GetReplies(object arg)
        {
            try {
                long statusid = (long)arg;
                while (statusid >= 0) {
                    TwitData data = FrmMain.Twitter.statuses_show(statusid);
                    uctlDispTwit.Invoke(new Action(() =>
                    {
                        uctlDispTwit.AddData(new TwitData[] { data });
                    }));
                    statusid = data.Mention_StatusID;
                }
            }
            catch (InvalidOperationException) { }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetReplies)
    }
}
