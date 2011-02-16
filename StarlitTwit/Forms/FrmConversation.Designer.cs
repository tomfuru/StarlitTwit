namespace StarlitTwit
{
    partial class FrmDispTweet
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.uctlDispTwit = new StarlitTwit.UctlDispTwit();
            this.SuspendLayout();
            // 
            // uctlDispTwit
            // 
            this.uctlDispTwit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctlDispTwit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDispTwit.ImageList = null;
            this.uctlDispTwit.Location = new System.Drawing.Point(0, 0);
            this.uctlDispTwit.Name = "uctlDispTwit";
            this.uctlDispTwit.Size = new System.Drawing.Size(392, 266);
            this.uctlDispTwit.TabIndex = 0;
            // 
            // FrmReply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 266);
            this.Controls.Add(this.uctlDispTwit);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmReply";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "会話";
            this.Shown += new System.EventHandler(this.FrmReply_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private UctlDispTwit uctlDispTwit;
    }
}