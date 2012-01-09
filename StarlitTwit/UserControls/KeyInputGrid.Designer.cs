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
            this.flpnlLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.flpnlRight = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.pnlScroll.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpnlLeft
            // 
            this.flpnlLeft.AutoSize = true;
            this.flpnlLeft.BackColor = System.Drawing.Color.White;
            this.flpnlLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpnlLeft.Location = new System.Drawing.Point(0, 0);
            this.flpnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.flpnlLeft.Name = "flpnlLeft";
            this.flpnlLeft.Size = new System.Drawing.Size(241, 253);
            this.flpnlLeft.TabIndex = 0;
            this.flpnlLeft.WrapContents = false;
            // 
            // flpnlRight
            // 
            this.flpnlRight.AutoSize = true;
            this.flpnlRight.BackColor = System.Drawing.Color.White;
            this.flpnlRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpnlRight.Location = new System.Drawing.Point(0, 0);
            this.flpnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.flpnlRight.Name = "flpnlRight";
            this.flpnlRight.Size = new System.Drawing.Size(116, 253);
            this.flpnlRight.TabIndex = 1;
            this.flpnlRight.WrapContents = false;
            // 
            // pnlScroll
            // 
            this.pnlScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlScroll.Controls.Add(this.pnlLeft);
            this.pnlScroll.Controls.Add(this.splitter1);
            this.pnlScroll.Controls.Add(this.pnlRight);
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(363, 253);
            this.pnlScroll.TabIndex = 4;
            this.pnlScroll.Resize += new System.EventHandler(this.pnlScroll_Resize);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.flpnlLeft);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(241, 253);
            this.pnlLeft.TabIndex = 3;
            this.pnlLeft.Resize += new System.EventHandler(this.pnlLeft_Resize);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(241, 0);
            this.splitter1.MinExtra = 100;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 253);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.flpnlRight);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(247, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(116, 253);
            this.pnlRight.TabIndex = 4;
            this.pnlRight.Resize += new System.EventHandler(this.flpnlRight_Resize);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Enabled = false;
            this.vScrollBar.Location = new System.Drawing.Point(363, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 253);
            this.vScrollBar.SmallChange = 10;
            this.vScrollBar.TabIndex = 5;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // KeyInputGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.pnlScroll);
            this.Name = "KeyInputGrid";
            this.Size = new System.Drawing.Size(380, 253);
            this.pnlScroll.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpnlLeft;
        private System.Windows.Forms.FlowLayoutPanel flpnlRight;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
    }
}
