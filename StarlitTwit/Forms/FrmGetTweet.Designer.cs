﻿namespace StarlitTwit
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
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
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
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(32, 6);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(183, 19);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(32, 67);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(183, 19);
            this.dtpTo.TabIndex = 4;
            // 
            // btnReverse
            // 
            this.btnReverse.Location = new System.Drawing.Point(163, 35);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(52, 23);
            this.btnReverse.TabIndex = 2;
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
            this.chbToEnable.TabIndex = 3;
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
            this.chbFromEnable.TabIndex = 0;
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
            this.label1.TabIndex = 7;
            this.label1.Text = "～";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 84);
            this.label3.TabIndex = 8;
            this.label3.Text = "※多くのAPIを消費します\r\nHome:過去800件まで、API消費最大4\r\nReply:過去800件まで、API消費最大4\r\nHistory,User:過去32" +
                "00件まで、API消費最大16\r\nDM:過去200件まで、API消費最大2\r\nList:過去800件まで、API消費最大4\r\nSearch:過去1500件まで、" +
                "API消費最大15";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(47, 95);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(69, 23);
            this.btnGet.TabIndex = 5;
            this.btnGet.Text = "取得";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnCansel
            // 
            this.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCansel.Location = new System.Drawing.Point(146, 95);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(69, 23);
            this.btnCansel.TabIndex = 6;
            this.btnCansel.Text = "キャンセル";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // FrmGetTweet
            // 
            this.AcceptButton = this.btnGet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCansel;
            this.ClientSize = new System.Drawing.Size(461, 130);
            this.ControlBox = false;
            this.Controls.Add(this.btnCansel);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chbFromEnable);
            this.Controls.Add(this.chbToEnable);
            this.Controls.Add(this.btnReverse);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmGetTweet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "時間指定ツイート取得";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
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