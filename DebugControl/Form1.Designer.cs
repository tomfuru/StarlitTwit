namespace DebugControl
{
    partial class Form1
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
            this.tabControlEx1 = new StarlitTwit.TabControlEx();
            this.tabPageEx1 = new StarlitTwit.TabPageEx();
            this.tabPageEx2 = new StarlitTwit.TabPageEx();
            this.tabControlEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.AllowDrop = true;
            this.tabControlEx1.Controls.Add(this.tabPageEx1);
            this.tabControlEx1.Controls.Add(this.tabPageEx2);
            this.tabControlEx1.ItemSize = new System.Drawing.Size(0, 15);
            this.tabControlEx1.Location = new System.Drawing.Point(44, 70);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.tabControlEx1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControlEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StarlitTwit.TabControlEx tabControlEx1;
        private StarlitTwit.TabPageEx tabPageEx1;
        private StarlitTwit.TabPageEx tabPageEx2;

    }
}

