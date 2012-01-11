using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class KeyInputRight : UserControl
    {
        public KeyData Keydata { get; private set; }
        public int GroupID { get; private set; }
        public event EventHandler<KeyDataChangingEventArgs> KeyDataChanging;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public KeyInputRight(KeyData keydata, int groupID)
        {
            InitializeComponent();

            Keydata = keydata;
            GroupID = groupID;
            SetLabelText();

            lblKey.MouseClick += new MouseEventHandler(control_MouseClick);
            btnEdit.MouseClick += new MouseEventHandler(control_MouseClick);
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region IsWarning プロパティ：
        //-------------------------------------------------------------------------------
        private bool _isWarning;
        /// <summary>
        /// 警告状態かどうか
        /// </summary>
        public bool IsWarning
        {
            get { return _isWarning; }
            set
            {
                _isWarning = value;
                lblKey.ForeColor = (_isWarning) ? Color.Red : Color.Black;
            }
        }
        #endregion (IsWarning)

        //-------------------------------------------------------------------------------
        #region control_MouseClick マウスクリック時
        //-------------------------------------------------------------------------------
        //
        private void control_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }
        #endregion (control_MouseClick)

        //-------------------------------------------------------------------------------
        #region -SetLabelText ラベルテキスト設定
        //-------------------------------------------------------------------------------
        //
        private void SetLabelText()
        {
            lblKey.Text = (Keydata == null) ? "" : Keydata.ToString();
        }
        #endregion (SetLabelText)

        //-------------------------------------------------------------------------------
        #region btnEdit_Click ...ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (KeyEditForm frm = new KeyEditForm(Keydata)) {
                if (frm.ShowDialog() == DialogResult.OK) {
                    var ce = new KeyDataChangingEventArgs(frm.KeyData, Keydata);
                    if (KeyDataChanging != null) { KeyDataChanging(this, ce); }
                    if (!ce.Cancel) {
                        Keydata = frm.KeyData;
                        SetLabelText();
                    }
                }
            }
        }
        #endregion (btnEdit_Click)

        //-------------------------------------------------------------------------------
        #region ...ボタンのVisible変更のためのイベント
        //-------------------------------------------------------------------------------
        private void KeyInputRight_MouseMove(object sender, MouseEventArgs e)
        {
            btnEdit.Visible = this.ClientRectangle.Contains(e.Location);
        }

        private void lblKey_MouseEnter(object sender, EventArgs e)
        {
            btnEdit.Visible = true;
        }

        private void lblKey_MouseLeave(object sender, EventArgs e)
        {
            Point mousep = btnEdit.PointToClient(Control.MousePosition);
            if (!btnEdit.ClientRectangle.Contains(mousep)) {
                btnEdit.Visible = false;
            }
        }

        private void btnEdit_MouseEnter(object sender, EventArgs e)
        {
            btnEdit.Visible = true;
        }

        private void btnEdit_MouseLeave(object sender, EventArgs e)
        {
            btnEdit.Visible = false;
        }
        //-------------------------------------------------------------------------------
        #endregion (...ボタンのVisible変更のためのイベント)

        //-------------------------------------------------------------------------------
        #region btnEdit_Paint ...の描画
        //-------------------------------------------------------------------------------
        //
        private void btnEdit_Paint(object sender, PaintEventArgs e)
        {
            string str = "...";
            //Size s = TextRenderer.MeasureText(str, btnEdit.Font);
            e.Graphics.DrawString(str, btnEdit.Font, Brushes.Black, 4, 0);
        }
        #endregion (btnEdit_Paint)
    }

    //-------------------------------------------------------------------------------
    #region (class)KeyDataChangingEventArgs
    //-------------------------------------------------------------------------------
    public class KeyDataChangingEventArgs : CancelEventArgs
    {
        public KeyData NewKeyData { get; private set; }
        public KeyData PreviousKeyData { get; private set; }

        public KeyDataChangingEventArgs(KeyData newkeydata, KeyData prevkeydata)
            : base()
        {
            NewKeyData = (newkeydata == null) ? null : newkeydata.DeepCopyClone();
            PreviousKeyData = (prevkeydata == null) ? null : prevkeydata.DeepCopyClone();
        }
    }
    //-------------------------------------------------------------------------------
    #endregion ((class)KeyDataChangingEventArgs)
}
