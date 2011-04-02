namespace StarlitTwit
{
    partial class FrmRenewProfileImage
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
            this.picbImage = new System.Windows.Forms.PictureBox();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdateImage = new System.Windows.Forms.Button();
            this.btnFileDialog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picbImage
            // 
            this.picbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbImage.Location = new System.Drawing.Point(10, 12);
            this.picbImage.Name = "picbImage";
            this.picbImage.Size = new System.Drawing.Size(50, 50);
            this.picbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbImage.TabIndex = 0;
            this.picbImage.TabStop = false;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(66, 12);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.ReadOnly = true;
            this.txtImagePath.Size = new System.Drawing.Size(196, 19);
            this.txtImagePath.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(205, 41);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdateImage
            // 
            this.btnUpdateImage.Enabled = false;
            this.btnUpdateImage.Location = new System.Drawing.Point(115, 41);
            this.btnUpdateImage.Name = "btnUpdateImage";
            this.btnUpdateImage.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateImage.TabIndex = 3;
            this.btnUpdateImage.Text = "更新";
            this.btnUpdateImage.UseVisualStyleBackColor = true;
            this.btnUpdateImage.Click += new System.EventHandler(this.btnUpdateImage_Click);
            // 
            // btnFileDialog
            // 
            this.btnFileDialog.Location = new System.Drawing.Point(257, 10);
            this.btnFileDialog.Name = "btnFileDialog";
            this.btnFileDialog.Size = new System.Drawing.Size(23, 23);
            this.btnFileDialog.TabIndex = 4;
            this.btnFileDialog.Text = "...";
            this.btnFileDialog.UseVisualStyleBackColor = true;
            this.btnFileDialog.Click += new System.EventHandler(this.btnFileDialog_Click);
            // 
            // FrmRenewProfileImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 76);
            this.ControlBox = false;
            this.Controls.Add(this.btnFileDialog);
            this.Controls.Add(this.btnUpdateImage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.picbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmRenewProfileImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "プロフィール画像更新";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRenewProfileImage_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbImage;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdateImage;
        private System.Windows.Forms.Button btnFileDialog;
    }
}