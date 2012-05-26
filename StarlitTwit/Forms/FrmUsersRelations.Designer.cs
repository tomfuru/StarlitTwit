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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstvCommonFriend = new StarlitTwit.ListViewEx();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstvCommonFollower = new StarlitTwit.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
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
            this.lblCommonFriendsNum.Location = new System.Drawing.Point(91, 280);
            this.lblCommonFriendsNum.Name = "lblCommonFriendsNum";
            this.lblCommonFriendsNum.Size = new System.Drawing.Size(49, 12);
            this.lblCommonFriendsNum.TabIndex = 6;
            this.lblCommonFriendsNum.Text = "(未取得)";
            // 
            // lblCommonFollowerNum
            // 
            this.lblCommonFollowerNum.AutoSize = true;
            this.lblCommonFollowerNum.Location = new System.Drawing.Point(91, 23);
            this.lblCommonFollowerNum.Name = "lblCommonFollowerNum";
            this.lblCommonFollowerNum.Size = new System.Drawing.Size(49, 12);
            this.lblCommonFollowerNum.TabIndex = 5;
            this.lblCommonFollowerNum.Text = "(未取得)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "共通フレンド数：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "共通フォロワー数：";
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCommonFriendsNum;
        private System.Windows.Forms.Label lblCommonFollowerNum;
        private System.Windows.Forms.Button btnAppendFriends;
        private System.Windows.Forms.Button btnAppendFollower;
        private System.Windows.Forms.ToolTip ttInfo;
    }
}