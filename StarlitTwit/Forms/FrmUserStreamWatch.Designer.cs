namespace StarlitTwit
{
    partial class FrmUserStreamWatch
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
            this.btnClose = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.chbAutoScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(228, 231);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 12;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(319, 220);
            this.listBox.TabIndex = 2;
            // 
            // chbAutoScroll
            // 
            this.chbAutoScroll.AutoSize = true;
            this.chbAutoScroll.Checked = true;
            this.chbAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbAutoScroll.Location = new System.Drawing.Point(12, 231);
            this.chbAutoScroll.Name = "chbAutoScroll";
            this.chbAutoScroll.Size = new System.Drawing.Size(94, 16);
            this.chbAutoScroll.TabIndex = 3;
            this.chbAutoScroll.Text = "自動スクロール";
            this.chbAutoScroll.UseVisualStyleBackColor = true;
            // 
            // FrmUserStreamWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 261);
            this.Controls.Add(this.chbAutoScroll);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.btnClose);
            this.Name = "FrmUserStreamWatch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "UserStream ログ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUserStreamWatch_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.CheckBox chbAutoScroll;
    }
}