namespace StarlitTwit
{
    partial class RichTextBoxHash
    {
        /// <summary> 
        /// �K�v�ȃf�U�C�i�[�ϐ��ł��B
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// �g�p���̃��\�[�X�����ׂăN���[���A�b�v���܂��B
        /// </summary>
        /// <param name="disposing">�}�l�[�W ���\�[�X���j�������ꍇ true�A�j������Ȃ��ꍇ�� false �ł��B</param>
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