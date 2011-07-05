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
            this.lstvList = new StarlitTwit.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 251);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(392, 23);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // tsslLabel
            // 
            this.tsslLabel.Name = "tsslLabel";
            this.tsslLabel.Size = new System.Drawing.Size(377, 18);
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
            // lstvList
            // 
            this.lstvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstvList.FullRowSelect = true;
            this.lstvList.GridLines = true;
            this.lstvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvList.Location = new System.Drawing.Point(12, 16);
            this.lstvList.MultiSelect = false;
            this.lstvList.Name = "lstvList";
            this.lstvList.Size = new System.Drawing.Size(368, 204);
            this.lstvList.TabIndex = 5;
            this.lstvList.UseCompatibleStateImageBehavior = false;
            this.lstvList.View = System.Windows.Forms.View.Details;
            this.lstvList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstvList_MouseMove);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "リスト名";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "人数";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "フォロワー数";
            this.columnHeader4.Width = 71;
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
            // ttInfo
            // 
            this.ttInfo.AutoPopDelay = 10000;
            this.ttInfo.InitialDelay = 500;
            this.ttInfo.ReshowDelay = 100;
            // 
            // FrmDispLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 274);
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
        private System.Windows.Forms.ToolTip ttInfo;
    }
}