namespace TwitterClient
{
    partial class FrmGetTweet
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
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromTime = new System.Windows.Forms.DateTimePicker();
            this.dtpToTime = new System.Windows.Forms.DateTimePicker();
            this.btnReverse = new System.Windows.Forms.Button();
            this.chbToEnable = new System.Windows.Forms.CheckBox();
            this.chbFromEnable = new System.Windows.Forms.CheckBox();
            this.lbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnCansel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "";
            this.dtpFromDate.Location = new System.Drawing.Point(32, 6);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(114, 19);
            this.dtpFromDate.TabIndex = 0;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "";
            this.dtpToDate.Location = new System.Drawing.Point(32, 67);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(114, 19);
            this.dtpToDate.TabIndex = 1;
            // 
            // dtpFromTime
            // 
            this.dtpFromTime.CustomFormat = "";
            this.dtpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFromTime.Location = new System.Drawing.Point(164, 6);
            this.dtpFromTime.Name = "dtpFromTime";
            this.dtpFromTime.ShowUpDown = true;
            this.dtpFromTime.Size = new System.Drawing.Size(74, 19);
            this.dtpFromTime.TabIndex = 2;
            // 
            // dtpToTime
            // 
            this.dtpToTime.CustomFormat = "";
            this.dtpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpToTime.Location = new System.Drawing.Point(164, 67);
            this.dtpToTime.Name = "dtpToTime";
            this.dtpToTime.ShowUpDown = true;
            this.dtpToTime.Size = new System.Drawing.Size(74, 19);
            this.dtpToTime.TabIndex = 3;
            // 
            // btnReverse
            // 
            this.btnReverse.Location = new System.Drawing.Point(174, 35);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(52, 23);
            this.btnReverse.TabIndex = 4;
            this.btnReverse.Text = "反転";
            this.btnReverse.UseVisualStyleBackColor = true;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // chbToEnable
            // 
            this.chbToEnable.AutoSize = true;
            this.chbToEnable.Checked = true;
            this.chbToEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbToEnable.Location = new System.Drawing.Point(11, 70);
            this.chbToEnable.Name = "chbToEnable";
            this.chbToEnable.Size = new System.Drawing.Size(15, 14);
            this.chbToEnable.TabIndex = 5;
            this.chbToEnable.UseVisualStyleBackColor = true;
            this.chbToEnable.CheckedChanged += new System.EventHandler(this.chbToEnable_CheckedChanged);
            // 
            // chbFromEnable
            // 
            this.chbFromEnable.AutoSize = true;
            this.chbFromEnable.Checked = true;
            this.chbFromEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbFromEnable.Location = new System.Drawing.Point(11, 9);
            this.chbFromEnable.Name = "chbFromEnable";
            this.chbFromEnable.Size = new System.Drawing.Size(15, 14);
            this.chbFromEnable.TabIndex = 5;
            this.chbFromEnable.UseVisualStyleBackColor = true;
            this.chbFromEnable.CheckedChanged += new System.EventHandler(this.chbFromEnable_CheckedChanged);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(21, 43);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(35, 12);
            this.lbl.TabIndex = 6;
            this.lbl.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 84);
            this.label3.TabIndex = 9;
            this.label3.Text = "※多くのAPIを消費します\r\nHome:過去800件まで、API消費最大4\r\nReply:過去800件まで、API消費最大4\r\nHistory,User:過去32" +
                "00件まで、API消費最大16\r\nDM:過去200件まで、API消費最大2\r\nList:過去800件まで、API消費最大4\r\nSearch:過去1500件まで、" +
                "API消費最大15";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(77, 95);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(69, 23);
            this.btnGet.TabIndex = 10;
            this.btnGet.Text = "取得";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnCansel
            // 
            this.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCansel.Location = new System.Drawing.Point(169, 95);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(69, 23);
            this.btnCansel.TabIndex = 11;
            this.btnCansel.Text = "キャンセル";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // FrmGetTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCansel;
            this.ClientSize = new System.Drawing.Size(479, 130);
            this.ControlBox = false;
            this.Controls.Add(this.btnCansel);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chbFromEnable);
            this.Controls.Add(this.chbToEnable);
            this.Controls.Add(this.btnReverse);
            this.Controls.Add(this.dtpToTime);
            this.Controls.Add(this.dtpFromTime);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmGetTweet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "時間指定ツイート取得";
            this.Load += new System.EventHandler(this.FrmGetTweet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromTime;
        private System.Windows.Forms.DateTimePicker dtpToTime;
        private System.Windows.Forms.Button btnReverse;
        private System.Windows.Forms.CheckBox chbToEnable;
        private System.Windows.Forms.CheckBox chbFromEnable;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnCansel;
    }
}