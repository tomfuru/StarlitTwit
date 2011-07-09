namespace StarlitTwit
{
    partial class FrmEditList
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtListName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.rdbPublic = new System.Windows.Forms.RadioButton();
            this.rdbUnPublic = new System.Windows.Forms.RadioButton();
            this.btnCansel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "リスト名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "説明";
            // 
            // txtListName
            // 
            this.txtListName.Location = new System.Drawing.Point(68, 10);
            this.txtListName.Name = "txtListName";
            this.txtListName.Size = new System.Drawing.Size(179, 19);
            this.txtListName.TabIndex = 3;
            this.txtListName.TextChanged += new System.EventHandler(this.txtListName_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(68, 35);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(179, 45);
            this.txtDescription.TabIndex = 4;
            // 
            // rdbPublic
            // 
            this.rdbPublic.AutoSize = true;
            this.rdbPublic.Checked = true;
            this.rdbPublic.Location = new System.Drawing.Point(266, 13);
            this.rdbPublic.Name = "rdbPublic";
            this.rdbPublic.Size = new System.Drawing.Size(47, 16);
            this.rdbPublic.TabIndex = 5;
            this.rdbPublic.TabStop = true;
            this.rdbPublic.Text = "公開";
            this.rdbPublic.UseVisualStyleBackColor = true;
            // 
            // rdbUnPublic
            // 
            this.rdbUnPublic.AutoSize = true;
            this.rdbUnPublic.Location = new System.Drawing.Point(266, 36);
            this.rdbUnPublic.Name = "rdbUnPublic";
            this.rdbUnPublic.Size = new System.Drawing.Size(59, 16);
            this.rdbUnPublic.TabIndex = 6;
            this.rdbUnPublic.Text = "非公開";
            this.rdbUnPublic.UseVisualStyleBackColor = true;
            // 
            // btnCansel
            // 
            this.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCansel.Location = new System.Drawing.Point(250, 87);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(75, 23);
            this.btnCansel.TabIndex = 7;
            this.btnCansel.Text = "キャンセル";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(169, 87);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(12, 92);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(11, 12);
            this.lblWarning.TabIndex = 9;
            this.lblWarning.Text = "...";
            // 
            // FrmEditList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCansel;
            this.ClientSize = new System.Drawing.Size(330, 116);
            this.ControlBox = false;
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCansel);
            this.Controls.Add(this.rdbUnPublic);
            this.Controls.Add(this.rdbPublic);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtListName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmEditList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "リスト";
            this.Load += new System.EventHandler(this.FrmEditList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtListName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.RadioButton rdbPublic;
        private System.Windows.Forms.RadioButton rdbUnPublic;
        private System.Windows.Forms.Button btnCansel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblWarning;
    }
}