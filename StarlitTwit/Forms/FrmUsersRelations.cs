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
    public partial class FrmUsersRelations : Form
    {
        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public FrmUsersRelations()
        {
            InitializeComponent();

            tsslabel.Text = "";
            userSelector.Notifier = NotifierMethod;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region NotifierMethod 通知メソッド中身
        //-------------------------------------------------------------------------------
        //
        private void NotifierMethod(string str)
        {
            tsslabel.Text = str;
        }
        #endregion (NotifierMethod)

        //-------------------------------------------------------------------------------
        #region userSelector_SelectedUserNamesChanging
        //-------------------------------------------------------------------------------
        private void userSelector_SelectedUserNamesChanging(object sender, SelectedUserNamesChangingEventArgs e)
        {
            btnDisplayRelations.Enabled = (e.SelectedItemsNum > 1);
        }
        //-------------------------------------------------------------------------------
        #endregion (userSelector_SelectedUserNamesChanging)

        //-------------------------------------------------------------------------------
        #region btnDisplayRelations_Click 関係表示ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnDisplayRelations_Click(object sender, EventArgs e)
        {
            //userSelector.SelectedUserNames
        }
        #endregion (btnDisplayRelations_Click)
        
    }
}
