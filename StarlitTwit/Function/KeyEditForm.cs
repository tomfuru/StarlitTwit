﻿using System;
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
            txtKey.Text = MakeKeyDataString(data);
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
            txtKey.Text = MakeKeyDataString(_keyData);
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
        #region -MakeKeyDataString KeyDataを文字列にします
        //-------------------------------------------------------------------------------
        /// <summary>
        /// KeyDataを文字列にします
        /// </summary>
        /// <param name="data">文字列形式にするKeyData</param>
        /// <returns>文字列</returns>
        private string MakeKeyDataString(KeyData data)
        {
            if (data == null) {
                btnOK.Enabled = true;
                return ""; 
            }

            Keys k = data.Key;
            StringBuilder sb = new StringBuilder();
            if (data.Ctrl) {
                if (sb.Length != 0) { sb.Append(" + "); }
                sb.Append(CTRL);
                if (k == Keys.ControlKey) { k = Keys.None; }
            }
            if (data.Shift) {
                if (sb.Length != 0) { sb.Append(" + "); }
                sb.Append(SHIFT);
                if (k == Keys.ShiftKey) { k = Keys.None; }
            }
            if (data.Alt) {
                if (sb.Length != 0) { sb.Append(" + "); }
                sb.Append(ALT);
                if (k == Keys.Menu) { k = Keys.None; }
            }

            if (sb.Length != 0) { sb.Append(" + "); }
            if (btnOK.Enabled = (k != Keys.None)) { sb.Append(k.ToString()); }

            return sb.ToString();
        }
        #endregion (MakeKeyDataString)

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
