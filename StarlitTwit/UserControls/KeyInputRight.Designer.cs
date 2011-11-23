namespace StarlitTwit
{
    partial class KeyInputRight
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
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblKey = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(48, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(19, 18);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "...";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnEdit.MouseEnter += new System.EventHandler(this.btnEdit_MouseEnter);
            this.btnEdit.MouseLeave += new System.EventHandler(this.btnEdit_MouseLeave);
            // 
            // lblKey
            // 
            this.lblKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKey.AutoEllipsis = true;
            this.lblKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKey.Location = new System.Drawing.Point(0, 0);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(67, 16);
            this.lblKey.TabIndex = 3;
            this.lblKey.Text = "...";
            this.lblKey.MouseEnter += new System.EventHandler(this.lblKey_MouseEnter);
            this.lblKey.MouseLeave += new System.EventHandler(this.lblKey_MouseLeave);
            // 
            // KeyInputRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lblKey);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "KeyInputRight";
            this.Size = new System.Drawing.Size(67, 16);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.KeyInputRight_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblKey;
    }
}
