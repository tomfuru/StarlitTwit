namespace StarlitTwit
{
    partial class RichTextBoxHash
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


        private void InitializeComponent()
        {
            //this.SuspendLayout();
            // 
            // RichTextBoxHash
            // 
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RichTextBoxHash_MouseClick);
            this.TextChanged += new System.EventHandler(this.RichTextBoxHash_TextChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RichTextBoxHash_MouseMove);
            this.ResumeLayout(false);
        }
    }
}