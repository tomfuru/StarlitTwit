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
            this.tsmiUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_DisplayProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_DisplayTweets = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_MakeUserTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_MakeListTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_OpenBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUser_Clipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsComboUser = new System.Windows.Forms.ToolStripComboBox();
            this.tsmiHashtag = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHashtag_MakeTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHashtag_Clipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsComboHashtag = new System.Windows.Forms.ToolStripComboBox();
            this.tsmiURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiURL_OpenExternalBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiURL_OpenInternalBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiURL_Clipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsComboURL = new System.Windows.Forms.ToolStripComboBox();
            this.tsmiOpenBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowser_ThisTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowser_ReplyTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDispRetweeter = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tsmiUser,
            this.tsmiHashtag,
            this.tsmiURL,
            this.tsmiOpenBrowser,
            this.toolStripMenuItem1,
            this.tsmiDispRetweeter,
            this.tsmiSepMoreTweet,
            this.tsmiMoreRecently,
            this.tsmiOlder,
            this.tsmiSpecifyTime});
            this.menuRow.Name = "menuRow";
            this.menuRow.Size = new System.Drawing.Size(223, 414);
            this.menuRow.Opening += new System.ComponentModel.CancelEventHandler(this.menuRow_Opening);
            // 
            // tsmiReply
            // 
            this.tsmiReply.Name = "tsmiReply";
            this.tsmiReply.Size = new System.Drawing.Size(241, 22);
            this.tsmiReply.Text = "返信(&R)";
            this.tsmiReply.Click += new System.EventHandler(this.tsmiReply_Click);
            // 
            // tsmiQuote
            // 
            this.tsmiQuote.Name = "tsmiQuote";
            this.tsmiQuote.Size = new System.Drawing.Size(241, 22);
            this.tsmiQuote.Text = "引用(&Q)";
            this.tsmiQuote.Click += new System.EventHandler(this.tsmiQuote_Click);
            // 
            // tsmiRetweet
            // 
            this.tsmiRetweet.Name = "tsmiRetweet";
            this.tsmiRetweet.Size = new System.Drawing.Size(241, 22);
            this.tsmiRetweet.Text = "リツイート(&T)";
            this.tsmiRetweet.Click += new System.EventHandler(this.tsmiRetweet_Click);
            // 
            // tsmiDirectMessage
            // 
            this.tsmiDirectMessage.Name = "tsmiDirectMessage";
            this.tsmiDirectMessage.Size = new System.Drawing.Size(241, 22);
            this.tsmiDirectMessage.Text = "ダイレクトメッセージ(&D)";
            this.tsmiDirectMessage.Click += new System.EventHandler(this.tsmiDirectMessage_Click);
            // 
            // tsmiSepConversation
            // 
            this.tsmiSepConversation.Name = "tsmiSepConversation";
            this.tsmiSepConversation.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiDispConversation
            // 
            this.tsmiDispConversation.Name = "tsmiDispConversation";
            this.tsmiDispConversation.Size = new System.Drawing.Size(241, 22);
            this.tsmiDispConversation.Text = "会話を表示(&C)";
            this.tsmiDispConversation.Click += new System.EventHandler(this.tsmiDispConversation_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiFavorite
            // 
            this.tsmiFavorite.Name = "tsmiFavorite";
            this.tsmiFavorite.Size = new System.Drawing.Size(241, 22);
            this.tsmiFavorite.Text = "お気に入りに追加(&F)";
            this.tsmiFavorite.Click += new System.EventHandler(this.tsmiFavorite_Click);
            // 
            // tsmiUnfavorite
            // 
            this.tsmiUnfavorite.Name = "tsmiUnfavorite";
            this.tsmiUnfavorite.Size = new System.Drawing.Size(241, 22);
            this.tsmiUnfavorite.Text = "お気に入りから削除(&F)";
            this.tsmiUnfavorite.Click += new System.EventHandler(this.tsmiUnfavorite_Click);
            // 
            // tsmiSepFavorite
            // 
            this.tsmiSepFavorite.Name = "tsmiSepFavorite";
            this.tsmiSepFavorite.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(241, 22);
            this.tsmiDelete.Text = "削除(&D)";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // tsmiSepDelete
            // 
            this.tsmiSepDelete.Name = "tsmiSepDelete";
            this.tsmiSepDelete.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiUser
            // 
            this.tsmiUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUser_DisplayProfile,
            this.tsmiUser_DisplayTweets,
            this.tsmiUser_MakeUserTab,
            this.tsmiUser_MakeListTab,
            this.tsmiUser_OpenBrowser,
            this.tsmiUser_Clipboard,
            this.toolStripMenuItem3,
            this.tsComboUser});
            this.tsmiUser.Name = "tsmiUser";
            this.tsmiUser.Size = new System.Drawing.Size(241, 22);
            this.tsmiUser.Text = "ユーザー(&U)";
            // 
            // tsmiUser_DisplayProfile
            // 
            this.tsmiUser_DisplayProfile.Name = "tsmiUser_DisplayProfile";
            this.tsmiUser_DisplayProfile.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_DisplayProfile.Text = "プロフィール(&P)";
            this.tsmiUser_DisplayProfile.Click += new System.EventHandler(this.tsmiUser_DisplayProfile_Click);
            this.tsmiUser_DisplayProfile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiUser_DisplayTweets
            // 
            this.tsmiUser_DisplayTweets.Name = "tsmiUser_DisplayTweets";
            this.tsmiUser_DisplayTweets.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_DisplayTweets.Text = "最近の発言(&S)";
            this.tsmiUser_DisplayTweets.Click += new System.EventHandler(this.tsmiUser_DisplayTweets_Click);
            this.tsmiUser_DisplayTweets.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiUser_MakeUserTab
            // 
            this.tsmiUser_MakeUserTab.Name = "tsmiUser_MakeUserTab";
            this.tsmiUser_MakeUserTab.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_MakeUserTab.Text = "タブを作成(&T)";
            this.tsmiUser_MakeUserTab.Click += new System.EventHandler(this.tsmiUser_MakeUserTab_Click);
            this.tsmiUser_MakeUserTab.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiUser_MakeListTab
            // 
            this.tsmiUser_MakeListTab.Name = "tsmiUser_MakeListTab";
            this.tsmiUser_MakeListTab.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_MakeListTab.Text = "リストのタブを作成(&L)";
            this.tsmiUser_MakeListTab.Click += new System.EventHandler(this.tsmiUser_MakeListTab_Click);
            this.tsmiUser_MakeListTab.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiUser_OpenBrowser
            // 
            this.tsmiUser_OpenBrowser.Name = "tsmiUser_OpenBrowser";
            this.tsmiUser_OpenBrowser.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_OpenBrowser.Text = "ホームをブラウザで開く(&B)";
            this.tsmiUser_OpenBrowser.Click += new System.EventHandler(this.tsmiUser_OpenBrowser_Click);
            this.tsmiUser_OpenBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiUser_Clipboard
            // 
            this.tsmiUser_Clipboard.Name = "tsmiUser_Clipboard";
            this.tsmiUser_Clipboard.Size = new System.Drawing.Size(189, 22);
            this.tsmiUser_Clipboard.Text = "クリップボードにコピー(&C)";
            this.tsmiUser_Clipboard.Click += new System.EventHandler(this.tsmiUser_Clipboard_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(186, 6);
            // 
            // tsComboUser
            // 
            this.tsComboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsComboUser.Name = "tsComboUser";
            this.tsComboUser.Size = new System.Drawing.Size(121, 20);
            // 
            // tsmiHashtag
            // 
            this.tsmiHashtag.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHashtag_MakeTab,
            this.tsmiHashtag_Clipboard,
            this.toolStripMenuItem4,
            this.tsComboHashtag});
            this.tsmiHashtag.Name = "tsmiHashtag";
            this.tsmiHashtag.Size = new System.Drawing.Size(222, 22);
            this.tsmiHashtag.Text = "ハッシュタグ(&H)";
            // 
            // tsmiHashtag_MakeTab
            // 
            this.tsmiHashtag_MakeTab.Name = "tsmiHashtag_MakeTab";
            this.tsmiHashtag_MakeTab.Size = new System.Drawing.Size(182, 22);
            this.tsmiHashtag_MakeTab.Text = "タブを作成(&T)";
            this.tsmiHashtag_MakeTab.Click += new System.EventHandler(this.tsmiHashtag_MakeTab_Click);
            this.tsmiHashtag_MakeTab.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiHashtag_Clipboard
            // 
            this.tsmiHashtag_Clipboard.Name = "tsmiHashtag_Clipboard";
            this.tsmiHashtag_Clipboard.Size = new System.Drawing.Size(182, 22);
            this.tsmiHashtag_Clipboard.Text = "クリップボードにコピー(&C)";
            this.tsmiHashtag_Clipboard.Click += new System.EventHandler(this.tsmiHashtag_Clipboard_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(179, 6);
            // 
            // tsComboHashtag
            // 
            this.tsComboHashtag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsComboHashtag.Name = "tsComboHashtag";
            this.tsComboHashtag.Size = new System.Drawing.Size(121, 20);
            // 
            // tsmiURL
            // 
            this.tsmiURL.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiURL_OpenExternalBrowser,
            this.tsmiURL_OpenInternalBrowser,
            this.tsmiURL_Clipboard,
            this.toolStripMenuItem5,
            this.tsComboURL});
            this.tsmiURL.Name = "tsmiURL";
            this.tsmiURL.Size = new System.Drawing.Size(241, 22);
            this.tsmiURL.Text = "URL(&P)";
            // 
            // tsmiURL_OpenExternalBrowser
            // 
            this.tsmiURL_OpenExternalBrowser.Name = "tsmiURL_OpenExternalBrowser";
            this.tsmiURL_OpenExternalBrowser.Size = new System.Drawing.Size(182, 22);
            this.tsmiURL_OpenExternalBrowser.Text = "外部ブラウザで開く(&E)";
            this.tsmiURL_OpenExternalBrowser.Click += new System.EventHandler(this.tsmiURL_OpenExternalBrowser_Click);
            this.tsmiURL_OpenExternalBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiURL_OpenInternalBrowser
            // 
            this.tsmiURL_OpenInternalBrowser.Name = "tsmiURL_OpenInternalBrowser";
            this.tsmiURL_OpenInternalBrowser.Size = new System.Drawing.Size(182, 22);
            this.tsmiURL_OpenInternalBrowser.Text = "内部ブラウザで開く(&I)";
            this.tsmiURL_OpenInternalBrowser.Click += new System.EventHandler(this.tsmiURL_OpenInternalBrowser_Click);
            this.tsmiURL_OpenInternalBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tsmiEntityItem_MouseMove);
            // 
            // tsmiURL_Clipboard
            // 
            this.tsmiURL_Clipboard.Name = "tsmiURL_Clipboard";
            this.tsmiURL_Clipboard.Size = new System.Drawing.Size(182, 22);
            this.tsmiURL_Clipboard.Text = "クリップボードにコピー(&C)";
            this.tsmiURL_Clipboard.Click += new System.EventHandler(this.tsmiURL_Clipboard_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(179, 6);
            // 
            // tsComboURL
            // 
            this.tsComboURL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsComboURL.Name = "tsComboURL";
            this.tsComboURL.Size = new System.Drawing.Size(121, 20);
            // 
            // tsmiOpenBrowser
            // 
            this.tsmiOpenBrowser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenBrowser_ThisTweet,
            this.tsmiOpenBrowser_ReplyTweet});
            this.tsmiOpenBrowser.Name = "tsmiOpenBrowser";
            this.tsmiOpenBrowser.Size = new System.Drawing.Size(241, 22);
            this.tsmiOpenBrowser.Text = "ツイートをブラウザで開く(&B)";
            // 
            // tsmiOpenBrowser_ThisTweet
            // 
            this.tsmiOpenBrowser_ThisTweet.Name = "tsmiOpenBrowser_ThisTweet";
            this.tsmiOpenBrowser_ThisTweet.Size = new System.Drawing.Size(167, 22);
            this.tsmiOpenBrowser_ThisTweet.Text = "このツイート(&T)";
            this.tsmiOpenBrowser_ThisTweet.Click += new System.EventHandler(this.tsmiOpenBrowser_ThisTweet_Click);
            // 
            // tsmiOpenBrowser_ReplyTweet
            // 
            this.tsmiOpenBrowser_ReplyTweet.Name = "tsmiOpenBrowser_ReplyTweet";
            this.tsmiOpenBrowser_ReplyTweet.Size = new System.Drawing.Size(167, 22);
            this.tsmiOpenBrowser_ReplyTweet.Text = "リプライ先ツイート(&R)";
            this.tsmiOpenBrowser_ReplyTweet.Click += new System.EventHandler(this.tsmiOpenBrowser_ReplyTweet_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiDispRetweeter
            // 
            this.tsmiDispRetweeter.Name = "tsmiDispRetweeter";
            this.tsmiDispRetweeter.Size = new System.Drawing.Size(241, 22);
            this.tsmiDispRetweeter.Text = "リツイートしたユーザー(&R)";
            this.tsmiDispRetweeter.Click += new System.EventHandler(this.tsmiDispRetweeter_Click);
            // 
            // tsmiSepMoreTweet
            // 
            this.tsmiSepMoreTweet.Name = "tsmiSepMoreTweet";
            this.tsmiSepMoreTweet.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiMoreRecently
            // 
            this.tsmiMoreRecently.Name = "tsmiMoreRecently";
            this.tsmiMoreRecently.Size = new System.Drawing.Size(222, 22);
            this.tsmiMoreRecently.Text = "これより新しいツイートを取得(&N)";
            this.tsmiMoreRecently.Click += new System.EventHandler(this.tsmiMoreRecentData_Click);
            // 
            // tsmiOlder
            // 
            this.tsmiOlder.Name = "tsmiOlder";
            this.tsmiOlder.Size = new System.Drawing.Size(222, 22);
            this.tsmiOlder.Text = "これより古いツイートを取得(&O)";
            this.tsmiOlder.Click += new System.EventHandler(this.tsmiOlderData_Click);
            // 
            // tsmiSpecifyTime
            // 
            this.tsmiSpecifyTime.Name = "tsmiSpecifyTime";
            this.tsmiSpecifyTime.Size = new System.Drawing.Size(241, 22);
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
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator tsmiSepMoreTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser_ThisTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowser_ReplyTweet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnfavorite;
        private System.Windows.Forms.ToolStripMenuItem tsmiDirectMessage;
        private System.Windows.Forms.ToolStripSeparator tsmiSepConversation;
        private System.Windows.Forms.ToolStripMenuItem tsmiSpecifyTime;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_DisplayProfile;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_DisplayTweets;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_MakeUserTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_MakeListTab;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripComboBox tsComboUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiHashtag;
        private System.Windows.Forms.ToolStripMenuItem tsmiHashtag_MakeTab;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripComboBox tsComboHashtag;
        private System.Windows.Forms.ToolStripMenuItem tsmiURL;
        private System.Windows.Forms.ToolStripMenuItem tsmiURL_OpenExternalBrowser;
        private System.Windows.Forms.ToolStripMenuItem tsmiURL_OpenInternalBrowser;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripComboBox tsComboURL;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_OpenBrowser;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispRetweeter;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiURL_Clipboard;
        private System.Windows.Forms.ToolStripMenuItem tsmiHashtag_Clipboard;
        private System.Windows.Forms.ToolStripMenuItem tsmiUser_Clipboard;
    }
}
