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
        private KeyData _keydata = null;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public KeyInputRight(KeyData keydata)
        {
            InitializeComponent();

            _keydata = keydata;
            SetLabelText();

            lblKey.MouseClick += new MouseEventHandler(control_MouseClick);
            btnEdit.MouseClick += new MouseEventHandler(control_MouseClick);
        }
        #endregion (Constructor)

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
            lblKey.Text = (_keydata == null) ? "" : _keydata.ToString();
        }
        #endregion (SetLabelText)

        //-------------------------------------------------------------------------------
        #region +GetKeyData KeyData取得
        //-------------------------------------------------------------------------------
        //
        public KeyData GetKeyData()
        {
            return _keydata;
        }
        #endregion (GetKeyData)

        //-------------------------------------------------------------------------------
        #region btnEdit_Click ...ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (KeyEditForm frm = new KeyEditForm(_keydata)) {
                if (frm.ShowDialog() == DialogResult.OK) {
                    _keydata = frm.KeyData;
                    SetLabelText();
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
}
