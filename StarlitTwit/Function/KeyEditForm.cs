using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    /// <summary>
    /// キー入力フォームです。
    /// </summary>
    public partial class KeyEditForm : Form
    {
        #region 定数
        //-------------------------------------------------------------------------------
        private const string CTRL = "Ctrl";
        private const string SHIFT = "Shift";
        private const string ALT = "Alt";
        //-------------------------------------------------------------------------------
        #endregion (定数)

        //-------------------------------------------------------------------------------
        #region メンバ変数
        //-------------------------------------------------------------------------------
        private KeyData _keyData;
        //-------------------------------------------------------------------------------
        #endregion (メンバ変数)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">初期入力のKeyData</param>
        public KeyEditForm(KeyData data)
        {
            InitializeComponent();
            txtKey.Text = (data == null) ? "" : data.ToString();
            _keyData = data;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region KeyEditForm_Load ロード時イベント
        //-------------------------------------------------------------------------------
        //
        private void KeyEditForm_Load(object sender, EventArgs e)
        {
            txtKey.Focus();
        }
        #endregion (KeyEditForm_Load)

        //-------------------------------------------------------------------------------
        #region KeyEditForm_FormClosed クローズ後イベント
        //-------------------------------------------------------------------------------
        //
        private void KeyEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        #endregion (KeyEditForm_FormClosed)

        //-------------------------------------------------------------------------------
        #region txtKey_KeyDown テキスト入力時イベント：入力禁止
        //-------------------------------------------------------------------------------
        //
        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            txtKey.ImeMode = ImeMode.Off; // 全角入力禁止のため

            //Console.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.ProcessKey) {
                _keyData = null;
            }
            else {
                KeyData keyData = new KeyData();
                keyData.Ctrl = e.Control;
                keyData.Alt = e.Alt;
                keyData.Shift = e.Shift;
                keyData.Key = e.KeyCode;
                _keyData = keyData;
            }
            if (_keyData == null) {
                btnOK.Enabled = true;
                txtKey.Text = "";
            }
            else {
                btnOK.Enabled = !_keyData.IsModifyOnly();
                txtKey.Text = _keyData.ToString(true);
            }
            e.SuppressKeyPress = true;
        }
        #endregion (txtKey_KeyDown)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタンクリック時
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)

        //-------------------------------------------------------------------------------
        #region KeyData プロパティ：入力キーデータ取得
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 入力されたキーデータを取得します。
        /// </summary>
        public KeyData KeyData
        {
            get { return _keyData; }
        }
        #endregion (KeyData)
    }
}
