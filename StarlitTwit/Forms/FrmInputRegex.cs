﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace StarlitTwit
{
    public partial class FrmInputRegex : Form
    {
        private Regex _regex = null;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public FrmInputRegex()
        {
            InitializeComponent();
            lblInfo.Text = "";
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region RegexText プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string RegexText
        {
            get { return txtRegex.Text; }
            set { txtRegex.Text = value; }
        }
        #endregion (RegexText)

        //-------------------------------------------------------------------------------
        #region txtRegex_TextChanged
        //-------------------------------------------------------------------------------
        //
        private void txtRegex_TextChanged(object sender, EventArgs e)
        {
            bool success;
            if (!txtRegex.Text.EndsWith(@"\\") && txtRegex.Text.EndsWith(@"\")) {
                success = false;
            }
            else {
                try {
                    _regex = new Regex(txtRegex.Text);
                    success = true;
                }
                catch (ArgumentException) {
                    success = false;
                }
            }

            if (!success) {
                _regex = null;
                btnOK.Enabled = false;
                lblInfo.Text = "無効な正規表現";
                return;
            }

            btnOK.Enabled = true;

            if (txtCheck.Text.Length > 0) {
                CheckRegex();
            }
            else {
                lblInfo.Text = "";
            }
        }
        #endregion (txtRegex_TextChanged)
        //-------------------------------------------------------------------------------
        #region txtCheck_TextChanged
        //-------------------------------------------------------------------------------
        //
        private void txtCheck_TextChanged(object sender, EventArgs e)
        {
            if (txtCheck.Text.Length > 0 && _regex != null) {
                CheckRegex();
            }
            else {
                lblInfo.Text = "";
            }
        }
        #endregion (txtCheck_TextChanged)

        //-------------------------------------------------------------------------------
        #region -CheckRegex 正規表現チェック
        //-------------------------------------------------------------------------------
        //
        private void CheckRegex()
        {
            if (_regex.IsMatch(txtCheck.Text)) {
                lblInfo.Text = "合致";
            }
            else {
                lblInfo.Text = "不合致";
            }
        }
        #endregion (CheckRegex)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタン
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセルボタン
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)
    }
}
