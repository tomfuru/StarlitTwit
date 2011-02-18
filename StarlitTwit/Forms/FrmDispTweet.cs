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
        private readonly TwitData _startTwitdata = null;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmDispTweet(TwitData data, ImageList imageList)
        {
            InitializeComponent();

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
            tsslabel.Text = "リプライ取得中...";
            (new Action<long>(GetReplies)).BeginInvoke(_startTwitdata.Mention_StatusID, Utilization.InvokeCallback, null);
        }
        //-------------------------------------------------------------------------------
        #endregion (FrmReply_Shown)

        //-------------------------------------------------------------------------------
        #region -GetReplies (別スレッド：リプライ取得)
        //-------------------------------------------------------------------------------
        //
        private void GetReplies(long status_id)
        {
            try {
                while (status_id >= 0) {
                    TwitData data = null;
                    try {
                        data = FrmMain.Twitter.statuses_show(status_id);
                    }
                    catch (TwitterAPIException) {
                        this.Invoke(new Action(() => tsslabel.Text = "取得できなかった発言があります。"));
                        return;
                    }

                    this.Invoke(new Action(() => uctlDispTwit.AddData(new TwitData[] { data })));
                    status_id = data.Mention_StatusID;
                }
                this.Invoke(new Action(() => tsslabel.Text = "会話の取得が完了しました。"));
            }
            catch (InvalidOperationException) { }
        }
        //-------------------------------------------------------------------------------
        #endregion (GetReplies)
    }
}
