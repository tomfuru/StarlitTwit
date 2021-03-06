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
    /// 時間指定発言取得の情報を入力するフォームです。
    /// </summary>
    public partial class FrmGetTweet : Form
    {
        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmGetTweet()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region EnableDateTimeFrom プロパティ：
        //-------------------------------------------------------------------------------
        private bool _enableDTFrom = true;
        /// <summary>
        /// From が有効かどうかを取得または設定
        /// </summary>
        public bool EnableDateTimeFrom
        {
            get { return _enableDTFrom; }
            set { _enableDTFrom = value; }
        }
        #endregion (EnableDateTimeFrom)
        #region DateTimeFrom プロパティ：時刻(From)
        //-------------------------------------------------------------------------------
        private DateTime _dtFrom = DateTime.Now;
        /// <summary>
        /// 検索する開始時刻を取得または設定します。
        /// </summary>
        public DateTime DateTimeFrom
        {
            get { return _dtFrom; }
            set { _dtFrom = value; }
        }
        #endregion (DateTimeFrom)
        //-------------------------------------------------------------------------------
        #region EnableDateTimeTo プロパティ：
        //-------------------------------------------------------------------------------
        private bool _enableDTTo = true;
        /// <summary>
        /// To が有効かどうかを取得または設定
        /// </summary>
        public bool EnableDateTimeTo
        {
            get { return _enableDTTo; }
            set { _enableDTTo = value; }
        }
        #endregion (EnableDateTimeTo)
        #region DateTimeTo プロパティ：時刻(To)
        //-------------------------------------------------------------------------------
        private DateTime _dtTo = DateTime.Now;
        /// <summary>
        /// 検索する終了時刻を取得または設定します。
        /// </summary>
        public DateTime DateTimeTo
        {
            get { return _dtTo; }
            set { _dtTo = value; }
        }
        #endregion (DateTimeTo)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad ロード時
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (chbFromEnable.Checked = EnableDateTimeFrom) {
                dtpFrom.Value = this.DateTimeFrom;
            }
            dtpFrom.Enabled = chbFromEnable.Checked;
            if (chbToEnable.Checked = EnableDateTimeTo) {
                dtpTo.Value = this.DateTimeTo;
            }
            dtpTo.Enabled = chbToEnable.Checked;
        }
        #endregion (OnLoad)

        //-------------------------------------------------------------------------------
        #region chbFromEnable_CheckedChanged Fromチェックボックス
        //-------------------------------------------------------------------------------
        //
        private void chbFromEnable_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = chbFromEnable.Checked;
        }
        #endregion (chbFromEnable_CheckedChanged)
        //-------------------------------------------------------------------------------
        #region chbToEnable_CheckedChanged Toチェックボックス
        //-------------------------------------------------------------------------------
        //
        private void chbToEnable_CheckedChanged(object sender, EventArgs e)
        {
            dtpTo.Enabled = chbToEnable.Checked;
        }
        #endregion (chbToEnable_CheckedChanged)

        //-------------------------------------------------------------------------------
        #region btnReverse_Click 反転ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnReverse_Click(object sender, EventArgs e)
        {
            { // チェックボックス交換
                bool tmp = chbFromEnable.Checked;
                chbFromEnable.Checked = chbToEnable.Checked;
                chbToEnable.Checked = tmp;
            }

            { // 交換
                DateTime tmp = dtpFrom.Value;
                dtpFrom.Value = dtpTo.Value;
                dtpTo.Value = tmp;
            }
        }
        #endregion (btnReverse_Click)

        //-------------------------------------------------------------------------------
        #region btnGet_Click 取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.EnableDateTimeFrom = chbFromEnable.Checked;
            this.EnableDateTimeTo = chbToEnable.Checked;
            this.DateTimeFrom = dtpFrom.Value;
            this.DateTimeTo = dtpTo.Value;
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnGet_Click)

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
