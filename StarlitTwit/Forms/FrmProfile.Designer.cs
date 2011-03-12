namespace StarlitTwit
{
    partial class FrmProfile
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
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lblRegisterTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblScreenName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLastStatus = new System.Windows.Forms.Label();
            this.lblDescriptionRest = new System.Windows.Forms.Label();
            this.lblLastStatusTime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblFollowerNum = new System.Windows.Forms.Label();
            this.lblFavoriteNum = new System.Windows.Forms.Label();
            this.lblFollowingNum = new System.Windows.Forms.Label();
            this.lblStatusNum = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblTimeZone = new System.Windows.Forms.Label();
            this.lblProtected = new System.Windows.Forms.Label();
            this.lblFollow = new System.Windows.Forms.Label();
            this.rtxtDescription = new StarlitTwit.RichTextBoxEx();
            this.picbIcon = new StarlitTwit.PictureBoxEx();
            this.llblWeb = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRenew
            // 
            this.btnRenew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenew.Location = new System.Drawing.Point(94, 416);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(75, 23);
            this.btnRenew.TabIndex = 4;
            this.btnRenew.Text = "更新";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(175, 416);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 33);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "ユーザーID：";
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(136, 34);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUserID.Size = new System.Drawing.Size(11, 12);
            this.lblUserID.TabIndex = 7;
            this.lblUserID.Text = "...";
            // 
            // lblRegisterTime
            // 
            this.lblRegisterTime.AutoSize = true;
            this.lblRegisterTime.Location = new System.Drawing.Point(136, 55);
            this.lblRegisterTime.Name = "lblRegisterTime";
            this.lblRegisterTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRegisterTime.Size = new System.Drawing.Size(11, 12);
            this.lblRegisterTime.TabIndex = 9;
            this.lblRegisterTime.Text = "...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 55);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "登録日時：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 147);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "ユーザー名：";
            // 
            // lblScreenName
            // 
            this.lblScreenName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenName.AutoSize = true;
            this.lblScreenName.Location = new System.Drawing.Point(92, 147);
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblScreenName.Size = new System.Drawing.Size(11, 12);
            this.lblScreenName.TabIndex = 11;
            this.lblScreenName.Text = "...";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 170);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "名称：";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 193);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "位置情報：";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 216);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(32, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "Web：";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 239);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "自己紹介";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(77, 167);
            this.txtName.MaxLength = 20;
            this.txtName.Name = "txtName";
            this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtName.Size = new System.Drawing.Size(169, 19);
            this.txtName.TabIndex = 0;
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation.Location = new System.Drawing.Point(77, 190);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLocation.Size = new System.Drawing.Size(169, 19);
            this.txtLocation.TabIndex = 1;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(76, 213);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUrl.Size = new System.Drawing.Size(169, 19);
            this.txtUrl.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 346);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "最新の発言";
            // 
            // lblLastStatus
            // 
            this.lblLastStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastStatus.AutoEllipsis = true;
            this.lblLastStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLastStatus.Location = new System.Drawing.Point(12, 360);
            this.lblLastStatus.Name = "lblLastStatus";
            this.lblLastStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLastStatus.Size = new System.Drawing.Size(233, 48);
            this.lblLastStatus.TabIndex = 17;
            this.lblLastStatus.Text = "...";
            // 
            // lblDescriptionRest
            // 
            this.lblDescriptionRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescriptionRest.AutoSize = true;
            this.lblDescriptionRest.Location = new System.Drawing.Point(71, 240);
            this.lblDescriptionRest.Name = "lblDescriptionRest";
            this.lblDescriptionRest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDescriptionRest.Size = new System.Drawing.Size(11, 12);
            this.lblDescriptionRest.TabIndex = 18;
            this.lblDescriptionRest.Text = "...";
            // 
            // lblLastStatusTime
            // 
            this.lblLastStatusTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastStatusTime.AutoSize = true;
            this.lblLastStatusTime.Location = new System.Drawing.Point(75, 346);
            this.lblLastStatusTime.Name = "lblLastStatusTime";
            this.lblLastStatusTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLastStatusTime.Size = new System.Drawing.Size(11, 12);
            this.lblLastStatusTime.TabIndex = 19;
            this.lblLastStatusTime.Text = "...";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 78);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(67, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "フォロワー数：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(136, 80);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(58, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "フォロー数：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 101);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(74, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "お気に入り数：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(136, 101);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 24;
            this.label13.Text = "発言数：";
            // 
            // lblFollowerNum
            // 
            this.lblFollowerNum.AutoSize = true;
            this.lblFollowerNum.Location = new System.Drawing.Point(92, 78);
            this.lblFollowerNum.Name = "lblFollowerNum";
            this.lblFollowerNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollowerNum.Size = new System.Drawing.Size(11, 12);
            this.lblFollowerNum.TabIndex = 25;
            this.lblFollowerNum.Text = "...";
            // 
            // lblFavoriteNum
            // 
            this.lblFavoriteNum.AutoSize = true;
            this.lblFavoriteNum.Location = new System.Drawing.Point(92, 101);
            this.lblFavoriteNum.Name = "lblFavoriteNum";
            this.lblFavoriteNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFavoriteNum.Size = new System.Drawing.Size(11, 12);
            this.lblFavoriteNum.TabIndex = 26;
            this.lblFavoriteNum.Text = "...";
            // 
            // lblFollowingNum
            // 
            this.lblFollowingNum.AutoSize = true;
            this.lblFollowingNum.Location = new System.Drawing.Point(197, 80);
            this.lblFollowingNum.Name = "lblFollowingNum";
            this.lblFollowingNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollowingNum.Size = new System.Drawing.Size(11, 12);
            this.lblFollowingNum.TabIndex = 28;
            this.lblFollowingNum.Text = "...";
            // 
            // lblStatusNum
            // 
            this.lblStatusNum.AutoSize = true;
            this.lblStatusNum.Location = new System.Drawing.Point(197, 101);
            this.lblStatusNum.Name = "lblStatusNum";
            this.lblStatusNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatusNum.Size = new System.Drawing.Size(11, 12);
            this.lblStatusNum.TabIndex = 29;
            this.lblStatusNum.Text = "...";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 124);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label12.Size = new System.Drawing.Size(67, 12);
            this.label12.TabIndex = 30;
            this.label12.Text = "タイムゾーン：";
            // 
            // lblTimeZone
            // 
            this.lblTimeZone.AutoSize = true;
            this.lblTimeZone.Location = new System.Drawing.Point(92, 124);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTimeZone.Size = new System.Drawing.Size(11, 12);
            this.lblTimeZone.TabIndex = 31;
            this.lblTimeZone.Text = "...";
            // 
            // lblProtected
            // 
            this.lblProtected.AutoSize = true;
            this.lblProtected.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblProtected.Location = new System.Drawing.Point(75, 9);
            this.lblProtected.Name = "lblProtected";
            this.lblProtected.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblProtected.Size = new System.Drawing.Size(101, 12);
            this.lblProtected.TabIndex = 32;
            this.lblProtected.Text = "◆非公開ユーザー";
            // 
            // lblFollow
            // 
            this.lblFollow.AutoSize = true;
            this.lblFollow.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFollow.Location = new System.Drawing.Point(193, 9);
            this.lblFollow.Name = "lblFollow";
            this.lblFollow.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollow.Size = new System.Drawing.Size(57, 12);
            this.lblFollow.TabIndex = 33;
            this.lblFollow.Text = "フォロー中";
            // 
            // rtxtDescription
            // 
            this.rtxtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtDescription.DetectUrls = false;
            this.rtxtDescription.Location = new System.Drawing.Point(14, 255);
            this.rtxtDescription.Name = "rtxtDescription";
            this.rtxtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtxtDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtxtDescription.Size = new System.Drawing.Size(231, 88);
            this.rtxtDescription.TabIndex = 3;
            this.rtxtDescription.Text = "";
            this.rtxtDescription.TextChanged += new System.EventHandler(this.rtxtDescription_TextChanged);
            // 
            // picbIcon
            // 
            this.picbIcon.ImageListWrapper = null;
            this.picbIcon.Location = new System.Drawing.Point(14, 12);
            this.picbIcon.Name = "picbIcon";
            this.picbIcon.Size = new System.Drawing.Size(48, 48);
            this.picbIcon.TabIndex = 12;
            this.picbIcon.TabStop = false;
            // 
            // llblWeb
            // 
            this.llblWeb.AutoSize = true;
            this.llblWeb.Location = new System.Drawing.Point(75, 216);
            this.llblWeb.Name = "llblWeb";
            this.llblWeb.Size = new System.Drawing.Size(11, 12);
            this.llblWeb.TabIndex = 34;
            this.llblWeb.TabStop = true;
            this.llblWeb.Text = "...";
            this.llblWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblWeb_LinkClicked);
            // 
            // FrmProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 446);
            this.Controls.Add(this.llblWeb);
            this.Controls.Add(this.lblFollow);
            this.Controls.Add(this.lblProtected);
            this.Controls.Add(this.lblTimeZone);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblStatusNum);
            this.Controls.Add(this.lblFollowingNum);
            this.Controls.Add(this.lblFavoriteNum);
            this.Controls.Add(this.lblFollowerNum);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblLastStatusTime);
            this.Controls.Add(this.lblDescriptionRest);
            this.Controls.Add(this.lblLastStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtxtDescription);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.picbIcon);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblScreenName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRegisterTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRenew);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmProfile_FormClosing);
            this.Load += new System.EventHandler(this.FrmProfile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Label lblRegisterTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblScreenName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private PictureBoxEx picbIcon;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtUrl;
        private StarlitTwit.RichTextBoxEx rtxtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLastStatus;
        private System.Windows.Forms.Label lblDescriptionRest;
        private System.Windows.Forms.Label lblLastStatusTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblFollowerNum;
        private System.Windows.Forms.Label lblFavoriteNum;
        private System.Windows.Forms.Label lblFollowingNum;
        private System.Windows.Forms.Label lblStatusNum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTimeZone;
        private System.Windows.Forms.Label lblProtected;
        private System.Windows.Forms.Label lblFollow;
        private System.Windows.Forms.LinkLabel llblWeb;
    }
}