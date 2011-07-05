namespace StarlitTwit
{
    partial class FrmDispLists
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.menuRow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMakeListTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDispListStatuses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDispListUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDispListSubscriber = new System.Windows.Forms.ToolStripMenuItem();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.btnAddNewList = new System.Windows.Forms.Button();
            this.lstvList = new StarlitTwit.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiListSubscribe = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiListUnSubscribe = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSepListEdit = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip.SuspendLayout();
            this.menuRow.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 252);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(392, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // tsslLabel
            // 
            this.tsslLabel.Name = "tsslLabel";
            this.tsslLabel.Size = new System.Drawing.Size(377, 17);
            this.tsslLabel.Spring = true;
            this.tsslLabel.Text = "...";
            this.tsslLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(279, 1);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(11, 12);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "...";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(305, 226);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAppend.Enabled = false;
            this.btnAppend.Location = new System.Drawing.Point(12, 226);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 6;
            this.btnAppend.Text = "追加取得";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // menuRow
            // 
            this.menuRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditList,
            this.tsmiDeleteList,
            this.tsSepListEdit,
            this.tsmiMakeListTab,
            this.tsmiDispListStatuses,
            this.toolStripMenuItem1,
            this.tsmiDispListUsers,
            this.tsmiDispListSubscriber,
            this.toolStripMenuItem2,
            this.tsmiListSubscribe,
            this.tsmiListUnSubscribe});
            this.menuRow.Name = "menuRow";
            this.menuRow.Size = new System.Drawing.Size(206, 220);
            this.menuRow.Opening += new System.ComponentModel.CancelEventHandler(this.menuRow_Opening);
            // 
            // tsmiMakeListTab
            // 
            this.tsmiMakeListTab.Name = "tsmiMakeListTab";
            this.tsmiMakeListTab.Size = new System.Drawing.Size(205, 22);
            this.tsmiMakeListTab.Text = "リストのタブを追加(&T)";
            this.tsmiMakeListTab.Click += new System.EventHandler(this.tsmiMakeListTab_Click);
            // 
            // tsmiDispListStatuses
            // 
            this.tsmiDispListStatuses.Name = "tsmiDispListStatuses";
            this.tsmiDispListStatuses.Size = new System.Drawing.Size(205, 22);
            this.tsmiDispListStatuses.Text = "リストの発言を表示(&S)";
            this.tsmiDispListStatuses.Click += new System.EventHandler(this.tsmiDispListStatuses_Click);
            // 
            // tsmiDispListUsers
            // 
            this.tsmiDispListUsers.Name = "tsmiDispListUsers";
            this.tsmiDispListUsers.Size = new System.Drawing.Size(205, 22);
            this.tsmiDispListUsers.Text = "リスト内のユーザーを表示(&U)";
            this.tsmiDispListUsers.Click += new System.EventHandler(this.tsmiDispListUsers_Click);
            // 
            // tsmiDispListSubscriber
            // 
            this.tsmiDispListSubscriber.Name = "tsmiDispListSubscriber";
            this.tsmiDispListSubscriber.Size = new System.Drawing.Size(205, 22);
            this.tsmiDispListSubscriber.Text = "リストのフォロワーを表示(&F)";
            this.tsmiDispListSubscriber.Click += new System.EventHandler(this.tsmiDispListSubscriber_Click);
            // 
            // btnAddNewList
            // 
            this.btnAddNewList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNewList.Location = new System.Drawing.Point(93, 226);
            this.btnAddNewList.Name = "btnAddNewList";
            this.btnAddNewList.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewList.TabIndex = 9;
            this.btnAddNewList.Text = "新規リスト";
            this.btnAddNewList.UseVisualStyleBackColor = true;
            this.btnAddNewList.Click += new System.EventHandler(this.btnAddNewList_Click);
            // 
            // lstvList
            // 
            this.lstvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstvList.ContextMenuStrip = this.menuRow;
            this.lstvList.FullRowSelect = true;
            this.lstvList.GridLines = true;
            this.lstvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvList.Location = new System.Drawing.Point(12, 16);
            this.lstvList.MultiSelect = false;
            this.lstvList.Name = "lstvList";
            this.lstvList.OwnerDraw = true;
            this.lstvList.Size = new System.Drawing.Size(368, 204);
            this.lstvList.TabIndex = 5;
            this.lstvList.UseCompatibleStateImageBehavior = false;
            this.lstvList.View = System.Windows.Forms.View.Details;
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
            this.columnHeader2.Text = "リスト名";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "人数";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "ﾌｫﾛﾜｰ数";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 64;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(202, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(202, 6);
            // 
            // tsmiListSubscribe
            // 
            this.tsmiListSubscribe.Name = "tsmiListSubscribe";
            this.tsmiListSubscribe.Size = new System.Drawing.Size(205, 22);
            this.tsmiListSubscribe.Text = "リストをフォロー(&F)";
            this.tsmiListSubscribe.Click += new System.EventHandler(this.tsmiListSubscribe_Click);
            // 
            // tsmiListUnSubscribe
            // 
            this.tsmiListUnSubscribe.Name = "tsmiListUnSubscribe";
            this.tsmiListUnSubscribe.Size = new System.Drawing.Size(205, 22);
            this.tsmiListUnSubscribe.Text = "リストをフォロー解除(&U)";
            this.tsmiListUnSubscribe.Click += new System.EventHandler(this.tsmiListUnSubscribe_Click);
            // 
            // tsmiEditList
            // 
            this.tsmiEditList.Name = "tsmiEditList";
            this.tsmiEditList.Size = new System.Drawing.Size(205, 22);
            this.tsmiEditList.Text = "リストを編集(&E)";
            this.tsmiEditList.Click += new System.EventHandler(this.tsmiEditList_Click);
            // 
            // tsmiDeleteList
            // 
            this.tsmiDeleteList.Name = "tsmiDeleteList";
            this.tsmiDeleteList.Size = new System.Drawing.Size(205, 22);
            this.tsmiDeleteList.Text = "リストを削除(&D)";
            this.tsmiDeleteList.Click += new System.EventHandler(this.tsmiDeleteList_Click);
            // 
            // tsSepListEdit
            // 
            this.tsSepListEdit.Name = "tsSepListEdit";
            this.tsSepListEdit.Size = new System.Drawing.Size(202, 6);
            // 
            // FrmDispLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 274);
            this.Controls.Add(this.btnAddNewList);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lstvList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDispLists";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "...";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuRow.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslLabel;
        private System.Windows.Forms.Label lblCount;
        private StarlitTwit.ListViewEx lstvList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip menuRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiMakeListTab;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.Button btnAddNewList;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispListStatuses;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispListUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiDispListSubscriber;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiListSubscribe;
        private System.Windows.Forms.ToolStripMenuItem tsmiListUnSubscribe;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditList;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteList;
        private System.Windows.Forms.ToolStripSeparator tsSepListEdit;
    }
}