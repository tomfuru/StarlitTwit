namespace StarlitTwit
{
    partial class FrmUsersRelations
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnDisplayRelations = new System.Windows.Forms.Button();
            this.userSelector = new StarlitTwit.UserSelector();
            this.btnAppendFriends = new System.Windows.Forms.Button();
            this.btnAppendFollower = new System.Windows.Forms.Button();
            this.lblCommonFriendsNum = new System.Windows.Forms.Label();
            this.lblCommonFollowerNum = new System.Windows.Forms.Label();
            this.lstvCommonFriend = new StarlitTwit.ListViewEx();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuRowUpper = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiFollow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSepUnderFollow = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDispFriend = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDispFollower = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDisplayUserProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplayUserTweet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenBrowserUserHome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSepOverBlock = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnblock = new System.Windows.Forms.ToolStripMenuItem();
            this.lstvCommonFollower = new StarlitTwit.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.menuRowLower = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuRowUpper.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuRowLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnDisplayRelations);
            this.splitContainer1.Panel1.Controls.Add(this.userSelector);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAppendFriends);
            this.splitContainer1.Panel2.Controls.Add(this.btnAppendFollower);
            this.splitContainer1.Panel2.Controls.Add(this.lblCommonFriendsNum);
            this.splitContainer1.Panel2.Controls.Add(this.lblCommonFollowerNum);
            this.splitContainer1.Panel2.Controls.Add(this.lstvCommonFriend);
            this.splitContainer1.Panel2.Controls.Add(this.lstvCommonFollower);
            this.splitContainer1.Size = new System.Drawing.Size(784, 538);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnDisplayRelations
            // 
            this.btnDisplayRelations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayRelations.Enabled = false;
            this.btnDisplayRelations.Location = new System.Drawing.Point(6, 508);
            this.btnDisplayRelations.Name = "btnDisplayRelations";
            this.btnDisplayRelations.Size = new System.Drawing.Size(190, 23);
            this.btnDisplayRelations.TabIndex = 0;
            this.btnDisplayRelations.Text = "関係表示";
            this.btnDisplayRelations.UseVisualStyleBackColor = true;
            this.btnDisplayRelations.Click += new System.EventHandler(this.btnDisplayRelations_Click);
            // 
            // userSelector
            // 
            this.userSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userSelector.Location = new System.Drawing.Point(0, 0);
            this.userSelector.MyName = null;
            this.userSelector.Name = "userSelector";
            this.userSelector.Notifier = null;
            this.userSelector.Size = new System.Drawing.Size(196, 502);
            this.userSelector.TabIndex = 0;
            this.userSelector.SelectedUserNamesChanging += new System.EventHandler<StarlitTwit.SelectedUserNamesChangingEventArgs>(this.userSelector_SelectedUserNamesChanging);
            // 
            // btnAppendFriends
            // 
            this.btnAppendFriends.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppendFriends.Enabled = false;
            this.btnAppendFriends.Location = new System.Drawing.Point(301, 269);
            this.btnAppendFriends.Name = "btnAppendFriends";
            this.btnAppendFriends.Size = new System.Drawing.Size(70, 23);
            this.btnAppendFriends.TabIndex = 7;
            this.btnAppendFriends.Text = "追加取得";
            this.btnAppendFriends.UseVisualStyleBackColor = true;
            this.btnAppendFriends.Click += new System.EventHandler(this.btnAppendFriends_Click);
            // 
            // btnAppendFollower
            // 
            this.btnAppendFollower.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppendFollower.Enabled = false;
            this.btnAppendFollower.Location = new System.Drawing.Point(301, 12);
            this.btnAppendFollower.Name = "btnAppendFollower";
            this.btnAppendFollower.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAppendFollower.Size = new System.Drawing.Size(70, 23);
            this.btnAppendFollower.TabIndex = 1;
            this.btnAppendFollower.Text = "追加取得";
            this.btnAppendFollower.UseVisualStyleBackColor = true;
            this.btnAppendFollower.Click += new System.EventHandler(this.btnAppendFollower_Click);
            // 
            // lblCommonFriendsNum
            // 
            this.lblCommonFriendsNum.AutoSize = true;
            this.lblCommonFriendsNum.Location = new System.Drawing.Point(3, 280);
            this.lblCommonFriendsNum.Name = "lblCommonFriendsNum";
            this.lblCommonFriendsNum.Size = new System.Drawing.Size(49, 12);
            this.lblCommonFriendsNum.TabIndex = 6;
            this.lblCommonFriendsNum.Text = "(未設定)";
            // 
            // lblCommonFollowerNum
            // 
            this.lblCommonFollowerNum.AutoSize = true;
            this.lblCommonFollowerNum.Location = new System.Drawing.Point(3, 23);
            this.lblCommonFollowerNum.Name = "lblCommonFollowerNum";
            this.lblCommonFollowerNum.Size = new System.Drawing.Size(49, 12);
            this.lblCommonFollowerNum.TabIndex = 5;
            this.lblCommonFollowerNum.Text = "(未設定)";
            // 
            // lstvCommonFriend
            // 
            this.lstvCommonFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvCommonFriend.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lstvCommonFriend.ContextMenuStrip = this.menuRowUpper;
            this.lstvCommonFriend.FullRowSelect = true;
            this.lstvCommonFriend.GridLines = true;
            this.lstvCommonFriend.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvCommonFriend.Location = new System.Drawing.Point(3, 295);
            this.lstvCommonFriend.MultiSelect = false;
            this.lstvCommonFriend.Name = "lstvCommonFriend";
            this.lstvCommonFriend.OwnerDraw = true;
            this.lstvCommonFriend.Size = new System.Drawing.Size(368, 195);
            this.lstvCommonFriend.TabIndex = 2;
            this.lstvCommonFriend.UseCompatibleStateImageBehavior = false;
            this.lstvCommonFriend.View = System.Windows.Forms.View.Details;
            this.lstvCommonFriend.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lstvList_ColumnWidthChanging);
            this.lstvCommonFriend.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lstvList_DrawColumnHeader);
            this.lstvCommonFriend.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lstvList_DrawSubItem);
            this.lstvCommonFriend.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstvList_MouseMove);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "";
            this.columnHeader5.Width = 48;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "ユーザー名";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "名称";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "";
            this.columnHeader8.Width = 90;
            // 
            // menuRowUpper
            // 
            this.menuRowUpper.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFollow,
            this.tsmiRemove,
            this.tsSepUnderFollow,
            this.tsmiDispFriend,
            this.tsmiDispFollower,
            this.toolStripMenuItem2,
            this.tsmiDisplayUserProfile,
            this.tsmiDisplayUserTweet,
            this.tsmiOpenBrowserUserHome,
            this.tsSepOverBlock,
            this.tsmiBlock,
            this.tsmiUnblock});
            this.menuRowUpper.Name = "contextMenuStrip1";
            this.menuRowUpper.Size = new System.Drawing.Size(250, 220);
            this.menuRowUpper.Opening += new System.ComponentModel.CancelEventHandler(this.menuRow_Opening);
            // 
            // tsmiFollow
            // 
            this.tsmiFollow.Name = "tsmiFollow";
            this.tsmiFollow.Size = new System.Drawing.Size(249, 22);
            this.tsmiFollow.Text = "フォローする(&F)";
            this.tsmiFollow.Click += new System.EventHandler(this.tsmiFollow_Click);
            // 
            // tsmiRemove
            // 
            this.tsmiRemove.Name = "tsmiRemove";
            this.tsmiRemove.Size = new System.Drawing.Size(249, 22);
            this.tsmiRemove.Text = "フォローを解除する(&R)";
            this.tsmiRemove.Click += new System.EventHandler(this.tsmiRemove_Click);
            // 
            // tsSepUnderFollow
            // 
            this.tsSepUnderFollow.Name = "tsSepUnderFollow";
            this.tsSepUnderFollow.Size = new System.Drawing.Size(246, 6);
            // 
            // tsmiDispFriend
            // 
            this.tsmiDispFriend.Name = "tsmiDispFriend";
            this.tsmiDispFriend.Size = new System.Drawing.Size(249, 22);
            this.tsmiDispFriend.Text = "フォローしているユーザー(&D)";
            this.tsmiDispFriend.Click += new System.EventHandler(this.tsmiDispFriend_Click);
            // 
            // tsmiDispFollower
            // 
            this.tsmiDispFollower.Name = "tsmiDispFollower";
            this.tsmiDispFollower.Size = new System.Drawing.Size(249, 22);
            this.tsmiDispFollower.Text = "フォローされているユーザー(&E)";
            this.tsmiDispFollower.Click += new System.EventHandler(this.tsmiDispFollower_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(246, 6);
            // 
            // tsmiDisplayUserProfile
            // 
            this.tsmiDisplayUserProfile.Name = "tsmiDisplayUserProfile";
            this.tsmiDisplayUserProfile.Size = new System.Drawing.Size(249, 22);
            this.tsmiDisplayUserProfile.Text = "プロフィール(&P)";
            this.tsmiDisplayUserProfile.Click += new System.EventHandler(this.tsmiDisplayUserProfile_Click);
            // 
            // tsmiDisplayUserTweet
            // 
            this.tsmiDisplayUserTweet.Name = "tsmiDisplayUserTweet";
            this.tsmiDisplayUserTweet.Size = new System.Drawing.Size(249, 22);
            this.tsmiDisplayUserTweet.Text = "最近の発言(&S)";
            this.tsmiDisplayUserTweet.Click += new System.EventHandler(this.tsmiDisplayUserTweet_Click);
            // 
            // tsmiOpenBrowserUserHome
            // 
            this.tsmiOpenBrowserUserHome.Name = "tsmiOpenBrowserUserHome";
            this.tsmiOpenBrowserUserHome.Size = new System.Drawing.Size(249, 22);
            this.tsmiOpenBrowserUserHome.Text = "ホームをブラウザで開く(&B)";
            this.tsmiOpenBrowserUserHome.Click += new System.EventHandler(this.tsmiOpenBrowserUserHome_Click);
            // 
            // tsSepOverBlock
            // 
            this.tsSepOverBlock.Name = "tsSepOverBlock";
            this.tsSepOverBlock.Size = new System.Drawing.Size(246, 6);
            // 
            // tsmiBlock
            // 
            this.tsmiBlock.Name = "tsmiBlock";
            this.tsmiBlock.Size = new System.Drawing.Size(249, 22);
            this.tsmiBlock.Text = "ブロックする(&B)";
            this.tsmiBlock.Click += new System.EventHandler(this.tsmiBlock_Click);
            // 
            // tsmiUnblock
            // 
            this.tsmiUnblock.Name = "tsmiUnblock";
            this.tsmiUnblock.Size = new System.Drawing.Size(249, 22);
            this.tsmiUnblock.Text = "ブロックを解除する(&B)";
            this.tsmiUnblock.Click += new System.EventHandler(this.tsmiUnblock_Click);
            // 
            // lstvCommonFollower
            // 
            this.lstvCommonFollower.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvCommonFollower.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstvCommonFollower.ContextMenuStrip = this.menuRowUpper;
            this.lstvCommonFollower.FullRowSelect = true;
            this.lstvCommonFollower.GridLines = true;
            this.lstvCommonFollower.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvCommonFollower.Location = new System.Drawing.Point(3, 38);
            this.lstvCommonFollower.MultiSelect = false;
            this.lstvCommonFollower.Name = "lstvCommonFollower";
            this.lstvCommonFollower.OwnerDraw = true;
            this.lstvCommonFollower.Size = new System.Drawing.Size(368, 195);
            this.lstvCommonFollower.TabIndex = 1;
            this.lstvCommonFollower.UseCompatibleStateImageBehavior = false;
            this.lstvCommonFollower.View = System.Windows.Forms.View.Details;
            this.lstvCommonFollower.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lstvList_ColumnWidthChanging);
            this.lstvCommonFollower.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lstvList_DrawColumnHeader);
            this.lstvCommonFollower.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lstvList_DrawSubItem);
            this.lstvCommonFollower.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstvList_MouseMove);
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
            // columnHeader4
            // 
            this.columnHeader4.Text = "";
            this.columnHeader4.Width = 90;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslabel
            // 
            this.tsslabel.Name = "tsslabel";
            this.tsslabel.Size = new System.Drawing.Size(769, 18);
            this.tsslabel.Spring = true;
            this.tsslabel.Text = "...";
            this.tsslabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuRowLower
            // 
            this.menuRowLower.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3,
            this.toolStripSeparator1,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripSeparator2,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripSeparator3,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10});
            this.menuRowLower.Name = "contextMenuStrip1";
            this.menuRowLower.Size = new System.Drawing.Size(250, 220);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem1.Text = "フォローする(&F)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem3.Text = "フォローを解除する(&R)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(246, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem4.Text = "フォローしているユーザー(&D)";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem5.Text = "フォローされているユーザー(&E)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(246, 6);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem6.Text = "プロフィール(&P)";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem7.Text = "最近の発言(&S)";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem8.Text = "ホームをブラウザで開く(&B)";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(246, 6);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem9.Text = "ブロックする(&B)";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(249, 22);
            this.toolStripMenuItem10.Text = "ブロックを解除する(&B)";
            // 
            // FrmUsersRelations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 563);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FrmUsersRelations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ユーザー関係";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuRowUpper.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuRowLower.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserSelector userSelector;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslabel;
        private System.Windows.Forms.Button btnDisplayRelations;
        private ListViewEx lstvCommonFriend;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private ListViewEx lstvCommonFollower;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lblCommonFriendsNum;
        private System.Windows.Forms.Label lblCommonFollowerNum;
        private System.Windows.Forms.Button btnAppendFriends;
        private System.Windows.Forms.Button btnAppendFollower;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.ContextMenuStrip menuRowUpper;
        private System.Windows.Forms.ToolStripMenuItem tsmiFollow;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemove;
        private System.Windows.Forms.ToolStripSeparator tsSepUnderFollow;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispFriend;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispFollower;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserProfile;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayUserTweet;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenBrowserUserHome;
        private System.Windows.Forms.ToolStripSeparator tsSepOverBlock;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlock;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnblock;
        private System.Windows.Forms.ContextMenuStrip menuRowLower;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
    }
}