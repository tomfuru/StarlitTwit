namespace StarlitTwit
{
    partial class UserSelector
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
            this.btnAddFollowers = new System.Windows.Forms.Button();
            this.btnInputName = new System.Windows.Forms.Button();
            this.chlsbUsers = new System.Windows.Forms.CheckedListBox();
            this.btnAddFriends = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddFollowers
            // 
            this.btnAddFollowers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFollowers.Location = new System.Drawing.Point(3, 279);
            this.btnAddFollowers.Name = "btnAddFollowers";
            this.btnAddFollowers.Size = new System.Drawing.Size(70, 40);
            this.btnAddFollowers.TabIndex = 0;
            this.btnAddFollowers.Text = "フォロワー\r\n追加取得";
            this.btnAddFollowers.UseVisualStyleBackColor = true;
            this.btnAddFollowers.Click += new System.EventHandler(this.btnAddFollowers_Click);
            // 
            // btnInputName
            // 
            this.btnInputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInputName.Location = new System.Drawing.Point(149, 279);
            this.btnInputName.Name = "btnInputName";
            this.btnInputName.Size = new System.Drawing.Size(70, 40);
            this.btnInputName.TabIndex = 1;
            this.btnInputName.Text = "名前入力";
            this.btnInputName.UseVisualStyleBackColor = true;
            this.btnInputName.Click += new System.EventHandler(this.btnInputName_Click);
            // 
            // chlsbUsers
            // 
            this.chlsbUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chlsbUsers.FormattingEnabled = true;
            this.chlsbUsers.Location = new System.Drawing.Point(3, 3);
            this.chlsbUsers.Name = "chlsbUsers";
            this.chlsbUsers.Size = new System.Drawing.Size(215, 270);
            this.chlsbUsers.TabIndex = 2;
            this.chlsbUsers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlsbUsers_ItemCheck);
            // 
            // btnAddFriends
            // 
            this.btnAddFriends.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFriends.Location = new System.Drawing.Point(76, 279);
            this.btnAddFriends.Name = "btnAddFriends";
            this.btnAddFriends.Size = new System.Drawing.Size(70, 40);
            this.btnAddFriends.TabIndex = 3;
            this.btnAddFriends.Text = "フレンド\r\n追加取得";
            this.btnAddFriends.UseVisualStyleBackColor = true;
            this.btnAddFriends.Click += new System.EventHandler(this.btnAddFriends_Click);
            // 
            // UserSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddFriends);
            this.Controls.Add(this.chlsbUsers);
            this.Controls.Add(this.btnInputName);
            this.Controls.Add(this.btnAddFollowers);
            this.Name = "UserSelector";
            this.Size = new System.Drawing.Size(222, 322);
            this.Resize += new System.EventHandler(this.UserSelector_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddFollowers;
        private System.Windows.Forms.Button btnInputName;
        private System.Windows.Forms.CheckedListBox chlsbUsers;
        private System.Windows.Forms.Button btnAddFriends;
    }
}
