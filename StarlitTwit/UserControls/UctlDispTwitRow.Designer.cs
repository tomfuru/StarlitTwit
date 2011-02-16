namespace StarlitTwit
{
    partial class UctlDispTwitRow
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
            this.lblName = new System.Windows.Forms.Label();
            this.picbIcon = new System.Windows.Forms.PictureBox();
            this.lblTweet = new System.Windows.Forms.Label();
            this.uctlline = new StarlitTwit.UctlLine();
            this.rtxtGet = new StarlitTwit.RichTextBoxHash();
            this.tooltipReply = new StarlitTwit.PermanentToolTip(this.components);
            this.tooltipPicture = new StarlitTwit.ToolTipPicture(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.Location = new System.Drawing.Point(52, 2);
            this.lblName.MaximumSize = new System.Drawing.Size(305, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(302, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
            this.lblName.UseMnemonic = false;
            this.lblName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Label_DoubleClick);
            // 
            // picbIcon
            // 
            this.picbIcon.Location = new System.Drawing.Point(2, 2);
            this.picbIcon.Name = "picbIcon";
            this.picbIcon.Size = new System.Drawing.Size(48, 48);
            this.picbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbIcon.TabIndex = 1;
            this.picbIcon.TabStop = false;
            this.picbIcon.Visible = false;
            // 
            // lblTweet
            // 
            this.lblTweet.AutoSize = true;
            this.lblTweet.BackColor = System.Drawing.Color.Transparent;
            this.lblTweet.Location = new System.Drawing.Point(52, 14);
            this.lblTweet.MaximumSize = new System.Drawing.Size(305, 100);
            this.lblTweet.Name = "lblTweet";
            this.lblTweet.Size = new System.Drawing.Size(299, 36);
            this.lblTweet.TabIndex = 2;
            this.lblTweet.Text = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT" +
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT";
            this.lblTweet.UseMnemonic = false;
            this.lblTweet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Label_DoubleClick);
            // 
            // uctlline
            // 
            this.uctlline.BackColor = System.Drawing.Color.Black;
            this.uctlline.Length = 359;
            this.uctlline.Location = new System.Drawing.Point(0, 51);
            this.uctlline.Name = "uctlline";
            this.uctlline.Size = new System.Drawing.Size(359, 1);
            this.uctlline.TabIndex = 4;
            // 
            // rtxtGet
            // 
            this.rtxtGet.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtGet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtGet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtxtGet.Location = new System.Drawing.Point(54, 17);
            this.rtxtGet.Name = "rtxtGet";
            this.rtxtGet.ReadOnly = true;
            this.rtxtGet.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtGet.Size = new System.Drawing.Size(100, 19);
            this.rtxtGet.TabIndex = 3;
            this.rtxtGet.TabStop = false;
            this.rtxtGet.Text = "";
            this.rtxtGet.Visible = false;
            this.rtxtGet.TweetItemClick += new System.EventHandler<StarlitTwit.TweetItemClickEventArgs>(this.rtxtGet_TweetItemClick);
            this.rtxtGet.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtxtGet_LinkClicked);
            this.rtxtGet.Leave += new System.EventHandler(this.rtxtGet_Leave);
            this.rtxtGet.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.rtxtGet_MouseWheel);
            // 
            // tooltipReply
            // 
            this.tooltipReply.AutoPopDelay = 99999;
            this.tooltipReply.InitialDelay = 100;
            this.tooltipReply.ReshowDelay = 100;
            this.tooltipReply.Popup += new System.Windows.Forms.PopupEventHandler(this.tooltipReply_Popup);
            // 
            // tooltipPicture
            // 
            this.tooltipPicture.AutoPopDelay = 5000;
            this.tooltipPicture.ImageURLs = null;
            this.tooltipPicture.InitialDelay = 1000;
            this.tooltipPicture.MaximumSize = new System.Drawing.Size(500, 500);
            this.tooltipPicture.OwnerDraw = true;
            this.tooltipPicture.ReshowDelay = 100;
            this.tooltipPicture.SwitchInterval = 2000;
            this.tooltipPicture.PrePopup += new System.EventHandler<System.ComponentModel.CancelEventArgs>(this.tooltipPicture_PrePopup);
            // 
            // UctlDispTwitRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.uctlline);
            this.Controls.Add(this.rtxtGet);
            this.Controls.Add(this.lblTweet);
            this.Controls.Add(this.picbIcon);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UctlDispTwitRow";
            this.Size = new System.Drawing.Size(359, 53);
            this.Load += new System.EventHandler(this.UctlDispTwitRow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox picbIcon;
        private System.Windows.Forms.Label lblTweet;
        private StarlitTwit.RichTextBoxHash rtxtGet;
        private UctlLine uctlline;
        private ToolTipPicture tooltipPicture;
        private PermanentToolTip tooltipReply;
    }
}
