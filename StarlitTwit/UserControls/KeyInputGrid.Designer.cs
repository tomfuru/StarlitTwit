namespace StarlitTwit
{
    partial class KeyInputGrid
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
            this.splContainter = new System.Windows.Forms.SplitContainer();
            this.flpnlLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.flpnlRight = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splContainter)).BeginInit();
            this.splContainter.Panel1.SuspendLayout();
            this.splContainter.Panel2.SuspendLayout();
            this.splContainter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splContainter
            // 
            this.splContainter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splContainter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splContainter.Location = new System.Drawing.Point(0, 0);
            this.splContainter.Name = "splContainter";
            // 
            // splContainter.Panel1
            // 
            this.splContainter.Panel1.Controls.Add(this.flpnlLeft);
            // 
            // splContainter.Panel2
            // 
            this.splContainter.Panel2.Controls.Add(this.flpnlRight);
            this.splContainter.Size = new System.Drawing.Size(380, 253);
            this.splContainter.SplitterDistance = 252;
            this.splContainter.TabIndex = 1;
            // 
            // flpnlLeft
            // 
            this.flpnlLeft.AutoScroll = true;
            this.flpnlLeft.BackColor = System.Drawing.SystemColors.Control;
            this.flpnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpnlLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpnlLeft.Location = new System.Drawing.Point(0, 0);
            this.flpnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flpnlLeft.Name = "flpnlLeft";
            this.flpnlLeft.Size = new System.Drawing.Size(250, 251);
            this.flpnlLeft.TabIndex = 0;
            this.flpnlLeft.Resize += new System.EventHandler(this.flpnlLeft_Resize);
            // 
            // flpnlRight
            // 
            this.flpnlRight.AutoScroll = true;
            this.flpnlRight.BackColor = System.Drawing.SystemColors.Control;
            this.flpnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpnlRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpnlRight.Location = new System.Drawing.Point(0, 0);
            this.flpnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.flpnlRight.Name = "flpnlRight";
            this.flpnlRight.Size = new System.Drawing.Size(122, 251);
            this.flpnlRight.TabIndex = 1;
            this.flpnlRight.Resize += new System.EventHandler(this.flpnlRight_Resize);
            // 
            // KeyInputGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splContainter);
            this.Name = "KeyInputGrid";
            this.Size = new System.Drawing.Size(380, 253);
            this.splContainter.Panel1.ResumeLayout(false);
            this.splContainter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splContainter)).EndInit();
            this.splContainter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splContainter;
        private System.Windows.Forms.FlowLayoutPanel flpnlLeft;
        private System.Windows.Forms.FlowLayoutPanel flpnlRight;
    }
}
