namespace TwitterClient
{
    partial class FrmMakeTab
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCansel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTabName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbSearchType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlKeyword = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.pnlList = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbList = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numTimeline_Interval = new TwitterClient.NumericUpDownExtended();
            this.numTimeline_Add = new TwitterClient.NumericUpDownExtended();
            this.numTimeline_First = new TwitterClient.NumericUpDownExtended();
            this.lblListOwner = new System.Windows.Forms.Label();
            this.pnlKeyword.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_Add)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_First)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(104, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCansel
            // 
            this.btnCansel.Location = new System.Drawing.Point(189, 137);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(75, 23);
            this.btnCansel.TabIndex = 1;
            this.btnCansel.Text = "キャンセル";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "タブ表示名";
            // 
            // txtTabName
            // 
            this.txtTabName.Location = new System.Drawing.Point(76, 107);
            this.txtTabName.Name = "txtTabName";
            this.txtTabName.Size = new System.Drawing.Size(123, 19);
            this.txtTabName.TabIndex = 4;
            this.txtTabName.TextChanged += new System.EventHandler(this.txtTabName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(206, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "追加取得数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(206, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "取得間隔(秒)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(206, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "初期取得数";
            // 
            // cmbSearchType
            // 
            this.cmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchType.FormattingEnabled = true;
            this.cmbSearchType.Location = new System.Drawing.Point(81, 21);
            this.cmbSearchType.Name = "cmbSearchType";
            this.cmbSearchType.Size = new System.Drawing.Size(106, 20);
            this.cmbSearchType.TabIndex = 26;
            this.cmbSearchType.SelectedIndexChanged += new System.EventHandler(this.cmbSearchType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "検索方法";
            // 
            // pnlKeyword
            // 
            this.pnlKeyword.Controls.Add(this.label3);
            this.pnlKeyword.Controls.Add(this.txtKeyword);
            this.pnlKeyword.Location = new System.Drawing.Point(19, 43);
            this.pnlKeyword.Name = "pnlKeyword";
            this.pnlKeyword.Size = new System.Drawing.Size(168, 62);
            this.pnlKeyword.TabIndex = 28;
            this.pnlKeyword.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "検索語(ハッシュタグも可)";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(3, 24);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(162, 19);
            this.txtKeyword.TabIndex = 0;
            this.txtKeyword.TextChanged += new System.EventHandler(this.txtkeyword_TextChanged);
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.label8);
            this.pnlUser.Controls.Add(this.label4);
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Location = new System.Drawing.Point(283, 11);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(168, 62);
            this.pnlUser.TabIndex = 29;
            this.pnlUser.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "※名前ではありません";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "検索ユーザー名";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(3, 24);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(162, 19);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.lblListOwner);
            this.pnlList.Controls.Add(this.label9);
            this.pnlList.Controls.Add(this.cmbList);
            this.pnlList.Location = new System.Drawing.Point(283, 98);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(168, 62);
            this.pnlList.TabIndex = 30;
            this.pnlList.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "リスト";
            // 
            // cmbList
            // 
            this.cmbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbList.FormattingEnabled = true;
            this.cmbList.Location = new System.Drawing.Point(3, 24);
            this.cmbList.Name = "cmbList";
            this.cmbList.Size = new System.Drawing.Size(162, 20);
            this.cmbList.TabIndex = 0;
            this.cmbList.SelectedIndexChanged += new System.EventHandler(this.cmbList_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(255, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "※リストを選択するとAPIを使用してリストを取得します";
            // 
            // numTimeline_Interval
            // 
            this.numTimeline_Interval.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimeline_Interval.Location = new System.Drawing.Point(208, 108);
            this.numTimeline_Interval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numTimeline_Interval.Name = "numTimeline_Interval";
            this.numTimeline_Interval.Size = new System.Drawing.Size(56, 19);
            this.numTimeline_Interval.TabIndex = 23;
            // 
            // numTimeline_Add
            // 
            this.numTimeline_Add.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimeline_Add.Location = new System.Drawing.Point(208, 71);
            this.numTimeline_Add.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numTimeline_Add.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numTimeline_Add.Name = "numTimeline_Add";
            this.numTimeline_Add.Size = new System.Drawing.Size(56, 19);
            this.numTimeline_Add.TabIndex = 21;
            this.numTimeline_Add.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numTimeline_First
            // 
            this.numTimeline_First.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimeline_First.Location = new System.Drawing.Point(208, 34);
            this.numTimeline_First.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numTimeline_First.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numTimeline_First.Name = "numTimeline_First";
            this.numTimeline_First.Size = new System.Drawing.Size(56, 19);
            this.numTimeline_First.TabIndex = 20;
            this.numTimeline_First.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblListOwner
            // 
            this.lblListOwner.AutoSize = true;
            this.lblListOwner.Location = new System.Drawing.Point(38, 9);
            this.lblListOwner.Name = "lblListOwner";
            this.lblListOwner.Size = new System.Drawing.Size(11, 12);
            this.lblListOwner.TabIndex = 6;
            this.lblListOwner.Text = "...";
            // 
            // FrmMakeTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 165);
            this.ControlBox = false;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlUser);
            this.Controls.Add(this.pnlKeyword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSearchType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numTimeline_Interval);
            this.Controls.Add(this.numTimeline_Add);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numTimeline_First);
            this.Controls.Add(this.txtTabName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCansel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmMakeTab";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "タブ作成";
            this.Load += new System.EventHandler(this.FrmMakeTab_Load);
            this.pnlKeyword.ResumeLayout(false);
            this.pnlKeyword.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_Add)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeline_First)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCansel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTabName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private TwitterClient.NumericUpDownExtended numTimeline_Interval;
        private TwitterClient.NumericUpDownExtended numTimeline_Add;
        private System.Windows.Forms.Label label5;
        private TwitterClient.NumericUpDownExtended numTimeline_First;
        private System.Windows.Forms.ComboBox cmbSearchType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlKeyword;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.ComboBox cmbList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblListOwner;
    }
}