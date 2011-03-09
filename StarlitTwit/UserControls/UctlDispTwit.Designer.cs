namespace StarlitTwit
{
    partial class UctlDispTwit
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuRow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiReply = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiQuote = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRetweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDirectMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSepConversation = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDispConversation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnfavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSepFavorite = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSepDelete = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOpenBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowser_UserHome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowser_ThisTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowser_ReplyTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDisplayUserProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplayUserTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMakeUserTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMakeUserListTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSepMoreTweet = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiMoreRecently = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOlder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSpecifyTime = new System.Windows.Forms.ToolStripMenuItem();
            this.vscrbar = new System.Windows.Forms.VScrollBar();
            this.pnlTweets = new StarlitTwit.PanelEx();
            this.menuRow.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuRow
            // 
            this.menuRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReply,
            this.tsmiQuote,
            this.tsmiRetweet,
            this.tsmiDirectMessage,
            this.tsmiSepConversation,
            this.tsmiDispConversation,
            this.toolStripSeparator2,
            this.tsmiFavorite,
            this.tsmiUnfavorite,
            this.tsmiSepFavorite,
            this.tsmiDelete,
            this.tsmiSepDelete,
            this.tsmiOpenBrowser,
            this.toolStripMenuItem2,
            this.tsmiDisplayUserProfile,
            this.tsmiDisplayUserTweet,
            this.tsmiMakeUserTab,
            this.tsmiMakeUserListTab,
            this.tsmiSepMoreTweet,
            this.tsmiMoreRecently,
            this.tsmiOlder,
            this.tsmiSpecifyTime});
            this.menuRow.Name = "menuRow";
            this.menuRow.Size = new System.Drawing.Size(244, 414);
            this.menuRow.Opening += new System.ComponentModel.CancelEventHandler(this.menuRow_Opening);
            // 
            // tsmiReply
            // 
            this.tsmiReply.Name = "tsmiReply";
            this.tsmiReply.Size = new System.Drawing.Size(243, 22);
            this.tsmiReply.Text = "返信(&R)";
            this.tsmiReply.Click += new System.EventHandler(this.tsmiReply_Click);
            // 
            // tsmiQuote
            // 
            this.tsmiQuote.Name = "tsmiQuote";
            this.tsmiQuote.Size = new System.Drawing.Size(243, 22);
            this.tsmiQuote.Text = "引用(&Q)";
            this.tsmiQuote.Click += new System.EventHandler(this.tsmiQuote_Click);
            // 
            // tsmiRetweet
            // 
            this.tsmiRetweet.Name = "tsmiRetweet";
            this.tsmiRetweet.Size = new System.Drawing.Size(243, 22);
            this.tsmiRetweet.Text = "リツイート(&T)";
            this.tsmiRetweet.Click += new System.EventHandler(this.tsmiRetweet_Click);
            // 
            // tsmiDirectMessage
            // 
            this.tsmiDirectMessage.Name = "tsmiDirectMessage";
            this.tsmiDirectMessage.Size = new System.Drawing.Size(243, 22);
            this.tsmiDirectMessage.Text = "ダイレクトメッセージ(&D)";
            this.tsmiDirectMessage.Click += new System.EventHandler(this.tsmiDirectMessage_Click);
            // 
            // tsmiSepConversation
            // 
            this.tsmiSepConversation.Name = "tsmiSepConversation";
            this.tsmiSepConversation.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiDispConversation
            // 
            this.tsmiDispConversation.Name = "tsmiDispConversation";
            this.tsmiDispConversation.Size = new System.Drawing.Size(243, 22);
            this.tsmiDispConversation.Text = "会話を表示(&C)";
            this.tsmiDispConversation.Click += new System.EventHandler(this.tsmiDispConversation_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiFavorite
            // 
            this.tsmiFavorite.Name = "tsmiFavorite";
            this.tsmiFavorite.Size = new System.Drawing.Size(243, 22);
            this.tsmiFavorite.Text = "お気に入りに追加(&F)";
            this.tsmiFavorite.Click += new System.EventHandler(this.tsmiFavorite_Click);
            // 
            // tsmiUnfavorite
            // 
            this.tsmiUnfavorite.Name = "tsmiUnfavorite";
            this.tsmiUnfavorite.Size = new System.Drawing.Size(243, 22);
            this.tsmiUnfavorite.Text = "お気に入りから削除(&F)";
            this.tsmiUnfavorite.Click += new System.EventHandler(this.tsmiUnfavorite_Click);
            // 
            // tsmiSepFavorite
            // 
            this.tsmiSepFavorite.Name = "tsmiSepFavorite";
            this.tsmiSepFavorite.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(243, 22);
            this.tsmiDelete.Text = "削除(&D)";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // tsmiSepDelete
            // 
            this.tsmiSepDelete.Name = "tsmiSepDelete";
            this.tsmiSepDelete.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiOpenBrowser
            // 
            this.tsmiOpenBrowser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenBrowser_UserHome,
            this.tsmiOpenBrowser_ThisTweet,
            this.tsmiOpenBrowser_ReplyTweet});
            this.tsmiOpenBrowser.Name = "tsmiOpenBrowser";
            this.tsmiOpenBrowser.Size = new System.Drawing.Size(243, 22);
            this.tsmiOpenBrowser.Text = "ブラウザで開く";
            // 
            // tsmiOpenBrowser_UserHome
            // 
            this.tsmiOpenBrowser_UserHome.Name = "tsmiOpenBrowser_UserHome";
            this.tsmiOpenBrowser_UserHome.Size = new System.Drawing.Size(168, 22);
            this.tsmiOpenBrowser_UserHome.Text = "このユーザーのホーム";
            this.tsmiOpenBrowser_UserHome.Click += new System.EventHandler(this.tsmiOpenBrowser_UserHome_Click);
            // 
            // tsmiOpenBrowser_ThisTweet
            // 
            this.tsmiOpenBrowser_ThisTweet.Name = "tsmiOpenBrowser_ThisTweet";
            this.tsmiOpenBrowser_ThisTweet.Size = new System.Drawing.Size(168, 22);
            this.tsmiOpenBrowser_ThisTweet.Text = "このツイート";
            this.tsmiOpenBrowser_ThisTweet.Click += new System.EventHandler(this.tsmiOpenBrowser_ThisTweet_Click);
            // 
            // tsmiOpenBrowser_ReplyTweet
            // 
            this.tsmiOpenBrowser_ReplyTweet.Name = "tsmiOpenBrowser_ReplyTweet";
            this.tsmiOpenBrowser_ReplyTweet.Size = new System.Drawing.Size(168, 22);
            this.tsmiOpenBrowser_ReplyTweet.Text = "リプライ先ツイート";
            this.tsmiOpenBrowser_ReplyTweet.Click += new System.EventHandler(this.tsmiOpenBrowser_ReplyTweet_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiDisplayUserProfile
            // 
            this.tsmiDisplayUserProfile.Name = "tsmiDisplayUserProfile";
            this.tsmiDisplayUserProfile.Size = new System.Drawing.Size(243, 22);
            this.tsmiDisplayUserProfile.Text = "このユーザーのプロフィールを表示する";
            this.tsmiDisplayUserProfile.Click += new System.EventHandler(this.tsmiDisplayUserProfile_Click);
            // 
            // tsmiDisplayUserTweet
            // 
            this.tsmiDisplayUserTweet.Name = "tsmiDisplayUserTweet";
            this.tsmiDisplayUserTweet.Size = new System.Drawing.Size(243, 22);
            this.tsmiDisplayUserTweet.Text = "このユーザーの発言を表示する";
            this.tsmiDisplayUserTweet.Click += new System.EventHandler(this.tsmiDisplayUserTweet_Click);
            // 
            // tsmiMakeUserTab
            // 
            this.tsmiMakeUserTab.Name = "tsmiMakeUserTab";
            this.tsmiMakeUserTab.Size = new System.Drawing.Size(243, 22);
            this.tsmiMakeUserTab.Text = "このユーザーのタブを作成する";
            this.tsmiMakeUserTab.Click += new System.EventHandler(this.tsmiMakeUserTab_Click);
            // 
            // tsmiMakeUserListTab
            // 
            this.tsmiMakeUserListTab.Name = "tsmiMakeUserListTab";
            this.tsmiMakeUserListTab.Size = new System.Drawing.Size(243, 22);
            this.tsmiMakeUserListTab.Text = "このユーザーのリストのタブを作成する";
            this.tsmiMakeUserListTab.Click += new System.EventHandler(this.tsmiMakeUserListTab_Click);
            // 
            // tsmiSepMoreTweet
            // 
            this.tsmiSepMoreTweet.Name = "tsmiSepMoreTweet";
            this.tsmiSepMoreTweet.Size = new System.Drawing.Size(240, 6);
            // 
            // tsmiMoreRecently
            // 
            this.tsmiMoreRecently.Name = "tsmiMoreRecently";
            this.tsmiMoreRecently.Size = new System.Drawing.Size(243, 22);
            this.tsmiMoreRecently.Text = "これより新しいツイートを取得する";
            this.tsmiMoreRecently.Click += new System.EventHandler(this.tsmiMoreRecentData_Click);
            // 
            // tsmiOlder
            // 
            this.tsmiOlder.Name = "tsmiOlder";
            this.tsmiOlder.Size = new System.Drawing.Size(243, 22);
            this.tsmiOlder.Text = "これより古いツイートを取得する";
            this.tsmiOlder.Click += new System.EventHandler(this.tsmiOlderData_Click);
            // 
            // tsmiSpecifyTime
            // 
            this.tsmiSpecifyTime.Name = "tsmiSpecifyTime";
            this.tsmiSpecifyTime.Size = new System.Drawing.Size(243, 22);
            this.tsmiSpecifyTime.Text = "時刻を指定して発言取得(&G)";
            this.tsmiSpecifyTime.Click += new System.EventHandler(this.tsmiSpecifyTime_Click);
            // 
            // vscrbar
            // 
            this.vscrbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vscrbar.Enabled = false;
            this.vscrbar.LargeChange = 1;
            this.vscrbar.Location = new System.Drawing.Point(348, 0);
            this.vscrbar.Maximum = 0;
            this.vscrbar.Name = "vscrbar";
            this.vscrbar.Size = new System.Drawing.Size(17, 281);
            this.vscrbar.TabIndex = 1;
            this.vscrbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vscrbar_Scroll);
            this.vscrbar.ValueChanged += new System.EventHandler(this.vscrbar_ValueChanged);
            // 
            // pnlTweets
            // 
            this.pnlTweets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTweets.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTweets.ContextMenuStrip = this.menuRow;
            this.pnlTweets.Location = new System.Drawing.Point(0, 0);
            this.pnlTweets.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTweets.Name = "pnlTweets";
            this.pnlTweets.Size = new System.Drawing.Size(348, 281);
            this.pnlTweets.TabIndex = 0;
            this.pnlTweets.TabStop = true;
            this.pnlTweets.SizeChanged += new System.EventHandler(this.UctlDispTwit_ClientSizeChanged);
            this.pnlTweets.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlflow_MouseClick);
            this.pnlTweets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlflow_MouseDown);
            this.pnlTweets.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlflow_MouseMove);
            this.pnlTweets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlflow_MouseUp);
            this.pnlTweets.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pnlflow_MouseWheel);
            // 
            // UctlDispTwit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.vscrbar);
            this.Controls.Add(this.pnlTweets);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UctlDispTwit";
            this.Size = new System.Drawing.Size(365, 281);
            this.ClientSizeChanged += new System.EventHandler(this.UctlDispTwit_ClientSizeChanged);
            this.menuRow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StarlitTwit.PanelEx pnlTweets;
        private System.Windows.Forms.ContextMenuStrip menuRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoreRecently;
        private System.Windows.Forms.ToolStripMenuItem tsmiOlder;
        private System.Windows.Forms.ToolStripMenuItem tsmiReply;
        private System.Windows.Forms.ToolStripMenuItem tsmiQuote;
        private System.Windows.Forms.ToolStripSeparator tsmiSepDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiRetweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispConversation;
        private System.Windows.Forms.VScrollBar vscrbar;
        private System.Windows.Forms.ToolStripSeparator tsmiSepFavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiMakeUserTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator tsmiSepMoreTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiMakeUserListTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser_UserHome;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser_ThisTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser_ReplyTweet;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnfavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiDirectMessage;
        private System.Windows.Forms.ToolStripSeparator tsmiSepConversation;
        private System.Windows.Forms.ToolStripMenuItem tsmiSpecifyTime;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserProfile;
    }
}
