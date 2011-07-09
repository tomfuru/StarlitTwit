namespace Test
{
    partial class Form3
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControlEx1 = new StarlitTwit.TabControlEx();
            this.tabPageEx1 = new StarlitTwit.TabPageEx();
            this.tabPageEx2 = new StarlitTwit.TabPageEx();
            this.tabControlEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 19);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 77);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 19);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 77);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.AllowDrop = true;
            this.tabControlEx1.Controls.Add(this.tabPageEx1);
            this.tabControlEx1.Controls.Add(this.tabPageEx2);
            this.tabControlEx1.ItemSize = new System.Drawing.Size(0, 15);
            this.tabControlEx1.Location = new System.Drawing.Point(45, 80);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.SelectedTab = this.tabPageEx1;
            this.tabControlEx1.Size = new System.Drawing.Size(200, 100);
            this.tabControlEx1.TabIndex = 0;
            // 
            // tabPageEx1
            // 
            this.tabPageEx1.Location = new System.Drawing.Point(4, 19);
            this.tabPageEx1.Name = "tabPageEx1";
            this.tabPageEx1.Size = new System.Drawing.Size(192, 77);
            this.tabPageEx1.TabIndex = 0;
            this.tabPageEx1.UseVisualStyleBackColor = true;
            // 
            // tabPageEx2
            // 
            this.tabPageEx2.Location = new System.Drawing.Point(4, 19);
            this.tabPageEx2.Name = "tabPageEx2";
            this.tabPageEx2.Size = new System.Drawing.Size(192, 77);
            this.tabPageEx2.TabIndex = 1;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.tabControlEx1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.tabControlEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private StarlitTwit.TabControlEx tabControlEx1;
        private StarlitTwit.TabPageEx tabPageEx1;
        private StarlitTwit.TabPageEx tabPageEx2;

    }
}