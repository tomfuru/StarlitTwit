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
            this.llblWeb = new System.Windows.Forms.LinkLabel();
            this.lblListedNum = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnImageChange = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiOperation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOperation_Follow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOperation_UnFollow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOperation_MakeUserTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOperation_Block = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOperation_UnBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplay = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplay_Friends = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplay_Follower = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDisplay_Statuses = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDisplay_OwnList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplay_BelongList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisplay_SubscriptList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenew = new System.Windows.Forms.ToolStripMenuItem();
            this.rtxtDescription = new StarlitTwit.RichTextBoxEx();
            this.picbIcon = new StarlitTwit.PictureBoxEx();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRenew
            // 
            this.btnRenew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenew.Location = new System.Drawing.Point(94, 456);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(75, 23);
            this.btnRenew.TabIndex = 7;
            this.btnRenew.Text = "更新";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(175, 456);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 54);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "ユーザーID：";
            // 
            // lblUserID
            // 
            this.lblUserID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(136, 55);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUserID.Size = new System.Drawing.Size(11, 12);
            this.lblUserID.TabIndex = 7;
            this.lblUserID.Text = "...";
            // 
            // lblRegisterTime
            // 
            this.lblRegisterTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRegisterTime.AutoSize = true;
            this.lblRegisterTime.Location = new System.Drawing.Point(136, 76);
            this.lblRegisterTime.Name = "lblRegisterTime";
            this.lblRegisterTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRegisterTime.Size = new System.Drawing.Size(11, 12);
            this.lblRegisterTime.TabIndex = 9;
            this.lblRegisterTime.Text = "...";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 76);
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
            this.label2.Location = new System.Drawing.Point(12, 188);
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
            this.lblScreenName.Location = new System.Drawing.Point(92, 188);
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
            this.label5.Location = new System.Drawing.Point(12, 210);
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
            this.label6.Location = new System.Drawing.Point(12, 232);
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
            this.label7.Location = new System.Drawing.Point(12, 254);
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
            this.label8.Location = new System.Drawing.Point(12, 276);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "自己紹介";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(77, 207);
            this.txtName.MaxLength = 20;
            this.txtName.Name = "txtName";
            this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtName.Size = new System.Drawing.Size(169, 19);
            this.txtName.TabIndex = 1;
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation.Location = new System.Drawing.Point(77, 230);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLocation.Size = new System.Drawing.Size(169, 19);
            this.txtLocation.TabIndex = 2;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(76, 253);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUrl.Size = new System.Drawing.Size(169, 19);
            this.txtUrl.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 386);
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
            this.lblLastStatus.Location = new System.Drawing.Point(12, 400);
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
            this.lblDescriptionRest.Location = new System.Drawing.Point(71, 280);
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
            this.lblLastStatusTime.Location = new System.Drawing.Point(75, 386);
            this.lblLastStatusTime.Name = "lblLastStatusTime";
            this.lblLastStatusTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLastStatusTime.Size = new System.Drawing.Size(11, 12);
            this.lblLastStatusTime.TabIndex = 19;
            this.lblLastStatusTime.Text = "...";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(136, 100);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(67, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "フォロワー数：";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 100);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(58, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "フォロー数：";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(136, 122);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(74, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "お気に入り数：";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 122);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 24;
            this.label13.Text = "発言数：";
            // 
            // lblFollowerNum
            // 
            this.lblFollowerNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFollowerNum.AutoSize = true;
            this.lblFollowerNum.Location = new System.Drawing.Point(216, 100);
            this.lblFollowerNum.Name = "lblFollowerNum";
            this.lblFollowerNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollowerNum.Size = new System.Drawing.Size(11, 12);
            this.lblFollowerNum.TabIndex = 25;
            this.lblFollowerNum.Text = "...";
            // 
            // lblFavoriteNum
            // 
            this.lblFavoriteNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFavoriteNum.AutoSize = true;
            this.lblFavoriteNum.Location = new System.Drawing.Point(216, 122);
            this.lblFavoriteNum.Name = "lblFavoriteNum";
            this.lblFavoriteNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFavoriteNum.Size = new System.Drawing.Size(11, 12);
            this.lblFavoriteNum.TabIndex = 26;
            this.lblFavoriteNum.Text = "...";
            // 
            // lblFollowingNum
            // 
            this.lblFollowingNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFollowingNum.AutoSize = true;
            this.lblFollowingNum.Location = new System.Drawing.Point(92, 100);
            this.lblFollowingNum.Name = "lblFollowingNum";
            this.lblFollowingNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollowingNum.Size = new System.Drawing.Size(11, 12);
            this.lblFollowingNum.TabIndex = 28;
            this.lblFollowingNum.Text = "...";
            // 
            // lblStatusNum
            // 
            this.lblStatusNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusNum.AutoSize = true;
            this.lblStatusNum.Location = new System.Drawing.Point(92, 122);
            this.lblStatusNum.Name = "lblStatusNum";
            this.lblStatusNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatusNum.Size = new System.Drawing.Size(11, 12);
            this.lblStatusNum.TabIndex = 29;
            this.lblStatusNum.Text = "...";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 166);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label12.Size = new System.Drawing.Size(67, 12);
            this.label12.TabIndex = 30;
            this.label12.Text = "タイムゾーン：";
            // 
            // lblTimeZone
            // 
            this.lblTimeZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTimeZone.AutoSize = true;
            this.lblTimeZone.Location = new System.Drawing.Point(92, 166);
            this.lblTimeZone.Name = "lblTimeZone";
            this.lblTimeZone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTimeZone.Size = new System.Drawing.Size(11, 12);
            this.lblTimeZone.TabIndex = 31;
            this.lblTimeZone.Text = "...";
            // 
            // lblProtected
            // 
            this.lblProtected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProtected.AutoSize = true;
            this.lblProtected.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblProtected.Location = new System.Drawing.Point(75, 30);
            this.lblProtected.Name = "lblProtected";
            this.lblProtected.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblProtected.Size = new System.Drawing.Size(101, 12);
            this.lblProtected.TabIndex = 32;
            this.lblProtected.Text = "◆非公開ユーザー";
            // 
            // lblFollow
            // 
            this.lblFollow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFollow.AutoSize = true;
            this.lblFollow.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFollow.Location = new System.Drawing.Point(193, 30);
            this.lblFollow.Name = "lblFollow";
            this.lblFollow.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFollow.Size = new System.Drawing.Size(57, 12);
            this.lblFollow.TabIndex = 33;
            this.lblFollow.Text = "フォロー中";
            // 
            // llblWeb
            // 
            this.llblWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llblWeb.AutoEllipsis = true;
            this.llblWeb.Location = new System.Drawing.Point(75, 254);
            this.llblWeb.Name = "llblWeb";
            this.llblWeb.Size = new System.Drawing.Size(171, 16);
            this.llblWeb.TabIndex = 5;
            this.llblWeb.TabStop = true;
            this.llblWeb.Text = "...";
            this.llblWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblWeb_LinkClicked);
            // 
            // lblListedNum
            // 
            this.lblListedNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblListedNum.AutoSize = true;
            this.lblListedNum.Location = new System.Drawing.Point(92, 144);
            this.lblListedNum.Name = "lblListedNum";
            this.lblListedNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblListedNum.Size = new System.Drawing.Size(11, 12);
            this.lblListedNum.TabIndex = 36;
            this.lblListedNum.Text = "...";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 144);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 35;
            this.label15.Text = "リスト：";
            // 
            // btnImageChange
            // 
            this.btnImageChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImageChange.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnImageChange.Location = new System.Drawing.Point(14, 79);
            this.btnImageChange.Name = "btnImageChange";
            this.btnImageChange.Size = new System.Drawing.Size(48, 18);
            this.btnImageChange.TabIndex = 0;
            this.btnImageChange.Text = "変更";
            this.btnImageChange.UseVisualStyleBackColor = true;
            this.btnImageChange.Click += new System.EventHandler(this.btnImageChange_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOperation,
            this.tsmiDisplay,
            this.tsmiRenew});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(257, 26);
            this.menuStrip1.TabIndex = 39;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiOperation
            // 
            this.tsmiOperation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOperation_Follow,
            this.tsmiOperation_UnFollow,
            this.toolStripMenuItem2,
            this.tsmiOperation_MakeUserTab,
            this.toolStripMenuItem3,
            this.tsmiOperation_Block,
            this.tsmiOperation_UnBlock});
            this.tsmiOperation.Name = "tsmiOperation";
            this.tsmiOperation.Size = new System.Drawing.Size(63, 22);
            this.tsmiOperation.Text = "操作(&O)";
            // 
            // tsmiOperation_Follow
            // 
            this.tsmiOperation_Follow.Name = "tsmiOperation_Follow";
            this.tsmiOperation_Follow.Size = new System.Drawing.Size(238, 22);
            this.tsmiOperation_Follow.Text = "フォローする(&F)";
            this.tsmiOperation_Follow.Click += new System.EventHandler(this.tsmiOperation_Follow_Click);
            // 
            // tsmiOperation_UnFollow
            // 
            this.tsmiOperation_UnFollow.Name = "tsmiOperation_UnFollow";
            this.tsmiOperation_UnFollow.Size = new System.Drawing.Size(238, 22);
            this.tsmiOperation_UnFollow.Text = "フォロー解除する(&F)";
            this.tsmiOperation_UnFollow.Click += new System.EventHandler(this.tsmiOperation_UnFollow_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(235, 6);
            // 
            // tsmiOperation_MakeUserTab
            // 
            this.tsmiOperation_MakeUserTab.Name = "tsmiOperation_MakeUserTab";
            this.tsmiOperation_MakeUserTab.Size = new System.Drawing.Size(238, 22);
            this.tsmiOperation_MakeUserTab.Text = "ユーザーのタブを作成する(&T)";
            this.tsmiOperation_MakeUserTab.Click += new System.EventHandler(this.tsmiOperation_MakeUserTab_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(235, 6);
            // 
            // tsmiOperation_Block
            // 
            this.tsmiOperation_Block.Name = "tsmiOperation_Block";
            this.tsmiOperation_Block.Size = new System.Drawing.Size(238, 22);
            this.tsmiOperation_Block.Text = "ブロックする(&B)";
            this.tsmiOperation_Block.Click += new System.EventHandler(this.tsmiOperation_Block_Click);
            // 
            // tsmiOperation_UnBlock
            // 
            this.tsmiOperation_UnBlock.Name = "tsmiOperation_UnBlock";
            this.tsmiOperation_UnBlock.Size = new System.Drawing.Size(238, 22);
            this.tsmiOperation_UnBlock.Text = "ブロックを解除する(&B)";
            this.tsmiOperation_UnBlock.Click += new System.EventHandler(this.tsmiOperation_UnBlock_Click);
            // 
            // tsmiDisplay
            // 
            this.tsmiDisplay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDisplay_Friends,
            this.tsmiDisplay_Follower,
            this.toolStripSeparator1,
            this.tsmiDisplay_Statuses,
            this.toolStripMenuItem1,
            this.tsmiDisplay_OwnList,
            this.tsmiDisplay_BelongList,
            this.tsmiDisplay_SubscriptList});
            this.tsmiDisplay.Name = "tsmiDisplay";
            this.tsmiDisplay.Size = new System.Drawing.Size(63, 22);
            this.tsmiDisplay.Text = "表示(&D)";
            // 
            // tsmiDisplay_Friends
            // 
            this.tsmiDisplay_Friends.Name = "tsmiDisplay_Friends";
            this.tsmiDisplay_Friends.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_Friends.Text = "フレンド表示(&F)";
            this.tsmiDisplay_Friends.Click += new System.EventHandler(this.tsmiDisplay_Friends_Click);
            // 
            // tsmiDisplay_Follower
            // 
            this.tsmiDisplay_Follower.Name = "tsmiDisplay_Follower";
            this.tsmiDisplay_Follower.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_Follower.Text = "フォロワー表示(&O)";
            this.tsmiDisplay_Follower.Click += new System.EventHandler(this.tsmiDisplay_Follower_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // tsmiDisplay_Statuses
            // 
            this.tsmiDisplay_Statuses.Name = "tsmiDisplay_Statuses";
            this.tsmiDisplay_Statuses.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_Statuses.Text = "最近の発言表示(&S)";
            this.tsmiDisplay_Statuses.Click += new System.EventHandler(this.tsmiDisplay_Statuses_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(199, 6);
            // 
            // tsmiDisplay_OwnList
            // 
            this.tsmiDisplay_OwnList.Name = "tsmiDisplay_OwnList";
            this.tsmiDisplay_OwnList.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_OwnList.Text = "所有リスト表示(&O)";
            this.tsmiDisplay_OwnList.Click += new System.EventHandler(this.tsmiDisplay_OwnList_Click);
            // 
            // tsmiDisplay_BelongList
            // 
            this.tsmiDisplay_BelongList.Name = "tsmiDisplay_BelongList";
            this.tsmiDisplay_BelongList.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_BelongList.Text = "所属リスト表示(&B)";
            this.tsmiDisplay_BelongList.Click += new System.EventHandler(this.tsmiDisplay_BelongList_Click);
            // 
            // tsmiDisplay_SubscriptList
            // 
            this.tsmiDisplay_SubscriptList.Name = "tsmiDisplay_SubscriptList";
            this.tsmiDisplay_SubscriptList.Size = new System.Drawing.Size(202, 22);
            this.tsmiDisplay_SubscriptList.Text = "フォローリスト表示(&S)";
            this.tsmiDisplay_SubscriptList.Click += new System.EventHandler(this.tsmiDisplay_SubscriptList_Click);
            // 
            // tsmiRenew
            // 
            this.tsmiRenew.Name = "tsmiRenew";
            this.tsmiRenew.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.tsmiRenew.Size = new System.Drawing.Size(68, 22);
            this.tsmiRenew.Text = "更新(F5)";
            this.tsmiRenew.Click += new System.EventHandler(this.tsmiRenew_Click);
            // 
            // rtxtDescription
            // 
            this.rtxtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtDescription.DetectUrls = false;
            this.rtxtDescription.Location = new System.Drawing.Point(14, 295);
            this.rtxtDescription.Name = "rtxtDescription";
            this.rtxtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtxtDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtxtDescription.Size = new System.Drawing.Size(231, 88);
            this.rtxtDescription.TabIndex = 6;
            this.rtxtDescription.Text = "";
            this.rtxtDescription.TextChanged += new System.EventHandler(this.rtxtDescription_TextChanged);
            // 
            // picbIcon
            // 
            this.picbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picbIcon.ImageListWrapper = null;
            this.picbIcon.Location = new System.Drawing.Point(14, 33);
            this.picbIcon.Name = "picbIcon";
            this.picbIcon.Size = new System.Drawing.Size(48, 48);
            this.picbIcon.TabIndex = 12;
            this.picbIcon.TabStop = false;
            // 
            // FrmProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 486);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnImageChange);
            this.Controls.Add(this.lblListedNum);
            this.Controls.Add(this.label15);
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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label lblListedNum;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnImageChange;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_OwnList;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_BelongList;
        private System.Windows.Forms.ToolStripMenuItem tsmiRenew;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_Friends;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_Follower;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation_Follow;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation_UnFollow;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation_MakeUserTab;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation_Block;
        private System.Windows.Forms.ToolStripMenuItem tsmiOperation_UnBlock;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_Statuses;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay_SubscriptList;
    }
}