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
    public partial class FrmDispLists : Form
    {
        //-------------------------------------------------------------------------------
        #region EFormType 列挙体：フォームの種類
        //-------------------------------------------------------------------------------
        /// <summary>
        /// フォームがどのような目的で使われるかを表します。
        /// </summary>
        public enum EFormType
        {
            /// <summary>自分のリスト</summary>
            MyList,
            /// <summary>指定ユーザーのリスト</summary>
            UserList,
            /// <summary>自分が所属しているリスト</summary>
            MyBelongedList,
            /// <summary>指定ユーザーが所属しているリスト</summary>
            UserBelongedList,
            /// <summary>自分がフォローしているリスト</summary>
            MySubscribingList,
            /// <summary>指定ユーザーがフォローしているリスト</summary>
            UserSubscribingList
        }
        //-------------------------------------------------------------------------------
        #endregion (EFormType)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public FrmDispLists()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region イベント
        //-------------------------------------------------------------------------------
        #region btnClose_Click 閉じるボタン
        //-------------------------------------------------------------------------------
        //
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion (btnClose_Click)
        //-------------------------------------------------------------------------------
        #region btnAppend_Click 追加取得ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnAppend_Click(object sender, EventArgs e)
        {

        }
        #endregion (btnAppend_Click)
        //-------------------------------------------------------------------------------
        #endregion (イベント)

        //-------------------------------------------------------------------------------
        #region メソッド
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------
        #endregion (メソッド)
    }
}
