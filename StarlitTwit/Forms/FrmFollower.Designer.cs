namespace StarlitTwit
{
    partial class FrmFollower
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
            if (disposing && (_imageAnimation != null)) { // リソース
                _imageAnimation.Dispose(true);
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuRow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiFollow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDispFollowing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDispFollower = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDisplayUserProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplayUserTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowserUserHome = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAppend = new System.Windows.Forms.Button();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.lstvList = new StarlitTwit.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuRow.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuRow
            // 
            this.menuRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFollow,
            this.tsmiRemove,
            this.toolStripMenuItem1,
            this.tsmiDispFollowing,
            this.tsmiDispFollower,
            this.toolStripMenuItem2,
            this.tsmiDisplayUserProfile,
            this.tsmiDisplayUserTweet,
            this.tsmiOpenBrowserUserHome});
            this.menuRow.Name = "contextMenuStrip1";
            this.menuRow.Size = new System.Drawing.Size(261, 170);
            this.menuRow.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // tsmiFollow
            // 
            this.tsmiFollow.Name = "tsmiFollow";
            this.tsmiFollow.Size = new System.Drawing.Size(260, 22);
            this.tsmiFollow.Text = "フォローする(&F)";
            this.tsmiFollow.Click += new System.EventHandler(this.tsmiFollow_Click);
            // 
            // tsmiRemove
            // 
            this.tsmiRemove.Name = "tsmiRemove";
            this.tsmiRemove.Size = new System.Drawing.Size(260, 22);
            this.tsmiRemove.Text = "フォローを解除する(&R)";
            this.tsmiRemove.Click += new System.EventHandler(this.tsmiRemove_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(257, 6);
            // 
            // tsmiDispFollowing
            // 
            this.tsmiDispFollowing.Name = "tsmiDispFollowing";
            this.tsmiDispFollowing.Size = new System.Drawing.Size(260, 22);
            this.tsmiDispFollowing.Text = "フレンドを見る";
            this.tsmiDispFollowing.Click += new System.EventHandler(this.tsmiDispFollowing_Click);
            // 
            // tsmiDispFollower
            // 
            this.tsmiDispFollower.Name = "tsmiDispFollower";
            this.tsmiDispFollower.Size = new System.Drawing.Size(260, 22);
            this.tsmiDispFollower.Text = "フォロワーを見る";
            this.tsmiDispFollower.Click += new System.EventHandler(this.tsmiDispFollower_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(257, 6);
            // 
            // tsmiDisplayUserProfile
            // 
            this.tsmiDisplayUserProfile.Name = "tsmiDisplayUserProfile";
            this.tsmiDisplayUserProfile.Size = new System.Drawing.Size(260, 22);
            this.tsmiDisplayUserProfile.Text = "このユーザーのプロフィールを表示する(&P)";
            this.tsmiDisplayUserProfile.Click += new System.EventHandler(this.tsmiDisplayUserProfile_Click);
            // 
            // tsmiDisplayUserTweet
            // 
            this.tsmiDisplayUserTweet.Name = "tsmiDisplayUserTweet";
            this.tsmiDisplayUserTweet.Size = new System.Drawing.Size(260, 22);
            this.tsmiDisplayUserTweet.Text = "このユーザーの発言を表示する(&D)";
            this.tsmiDisplayUserTweet.Click += new System.EventHandler(this.tsmiDisplayUserTweet_Click);
            // 
            // tsmiOpenBrowserUserHome
            // 
            this.tsmiOpenBrowserUserHome.Name = "tsmiOpenBrowserUserHome";
            this.tsmiOpenBrowserUserHome.Size = new System.Drawing.Size(260, 22);
            this.tsmiOpenBrowserUserHome.Text = "このユーザーのホームをブラウザで見る(&B)";
            this.tsmiOpenBrowserUserHome.Click += new System.EventHandler(this.tsmiOpenBrowserUserHome_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAppend.Enabled = false;
            this.btnAppend.Location = new System.Drawing.Point(12, 243);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 1;
            this.btnAppend.Text = "追加取得";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // ttInfo
            // 
            this.ttInfo.AutoPopDelay = 10000;
            this.ttInfo.InitialDelay = 500;
            this.ttInfo.ReshowDelay = 100;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 269);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(411, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslabel
            // 
            this.tsslabel.Name = "tsslabel";
            this.tsslabel.Size = new System.Drawing.Size(396, 17);
            this.tsslabel.Spring = true;
            this.tsslabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(324, 243);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(298, 1);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(11, 12);
            this.lblCount.TabIndex = 4;
            this.lblCount.Text = "...";
            // 
            // lstvList
            // 
            this.lstvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstvList.ContextMenuStrip = this.menuRow;
            this.lstvList.FullRowSelect = true;
            this.lstvList.GridLines = true;
            this.lstvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvList.Location = new System.Drawing.Point(12, 16);
            this.lstvList.MultiSelect = false;
            this.lstvList.Name = "lstvList";
            this.lstvList.OwnerDraw = true;
            this.lstvList.Size = new System.Drawing.Size(387, 221);
            this.lstvList.TabIndex = 0;
            this.lstvList.UseCompatibleStateImageBehavior = false;
            this.lstvList.View = System.Windows.Forms.View.Details;
            this.lstvList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lstvList_ColumnWidthChanging);
            this.lstvList.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lstvList_DrawColumnHeader);
            this.lstvList.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lstvList_DrawSubItem);
            this.lstvList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstvList_MouseMove);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 48;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ユーザー名";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "名称";
            this.columnHeader3.Width = 100;
            // 
            // FrmFollower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 291);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lstvList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAppend);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFollower";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "...";
            this.Load += new System.EventHandler(this.FrmFollower_Load);
            this.menuRow.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StarlitTwit.ListViewEx lstvList;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslabel;
        private System.Windows.Forms.ContextMenuStrip menuRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiFollow;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowserUserHome;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserProfile;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispFollower;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispFollowing;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}