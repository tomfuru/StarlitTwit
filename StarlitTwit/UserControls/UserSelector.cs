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
    public partial class UserSelector : UserControl
    {
        //-------------------------------------------------------------------------------
        #region コンストラクタ 
        //-------------------------------------------------------------------------------
        //
        public UserSelector()
        {
            InitializeComponent();

            if (!FrmMain.Twitter.IsAuthenticated()) {
                btnAddFollowers.Enabled = btnAddFriends.Enabled = false;
            }
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region MyName プロパティ：
        //-------------------------------------------------------------------------------
        private string _myName = null;
        /// <summary>
        /// 
        /// </summary>
        public string MyName
        {
            get { return _myName; }
            set
            {
                if (_myName != null) {
                    chlsbUsers.Items.RemoveAt(0);
                }
                _myName = value;
                if (_myName != null) {
                    chlsbUsers.Items.Insert(0, _myName);
                }
            }
        }
        #endregion (MyName)

        public Action<Exception> ErrorHandler { get; private set; }

        //-------------------------------------------------------------------------------
        #region UserSelector_Resize
        //-------------------------------------------------------------------------------
        //
        private void UserSelector_Resize(object sender, EventArgs e)
        {
            const int PADDING = 3;
            const int HEIGHT = 40;
            int y = this.Height - HEIGHT - PADDING;
            int width = (this.Width - 4 * PADDING) / 3;

            this.btnAddFollowers.Location = new Point(PADDING, y);
            this.btnAddFriends.Location = new Point(this.Width / 2 - width / 2, y);
            this.btnInputName.Location = new Point(this.Width - width - PADDING, y);

            this.btnAddFollowers.Size = new Size(width, HEIGHT);
            this.btnAddFriends.Size = new Size(width, HEIGHT);
            this.btnInputName.Size = new Size(width, HEIGHT);
        }
        #endregion (UserSelector_Resize)

        //-------------------------------------------------------------------------------
        #region btnAddFollowers_Click フォロワー追加取得
        //-------------------------------------------------------------------------------
        //
        private void btnAddFollowers_Click(object sender, EventArgs e)
        {
            GetFollowers();
        }
        #endregion (btnAddFollowers_Click)
        //-------------------------------------------------------------------------------
        #region btnAddFriends_Click フレンド追加取得
        //-------------------------------------------------------------------------------
        //
        private void btnAddFriends_Click(object sender, EventArgs e)
        {
            GetFriends();
        }
        #endregion (btnAddFriends_Click)

        //-------------------------------------------------------------------------------
        #region txtInputName_Leave
        //-------------------------------------------------------------------------------
        //
        private void txtInputName_Leave(object sender, EventArgs e)
        {
            GetAndAddSpecifiedUserOfTextBox();
        }
        #endregion (txtInputName_Leave)
        //-------------------------------------------------------------------------------
        #region txtInputName_KeyDown
        //-------------------------------------------------------------------------------
        //
        private void txtInputName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) {
                txtInputName.Visible = false; // leaveイベント発生
            }
        }
        #endregion (txtInputName_KeyDown)

        //-------------------------------------------------------------------------------
        #region btnInputName_Click 名前入力
        //-------------------------------------------------------------------------------
        //
        private void btnInputName_Click(object sender, EventArgs e)
        {
            int index = chlsbUsers.Items.Add("");
            Rectangle r = chlsbUsers.GetItemRectangle(index);
            
            txtInputName.Location = new Point(r.X, r.Y + r.Height / 2 - txtInputName.Height / 2);
            txtInputName.Width = r.Width;
            txtInputName.Visible = true;
            txtInputName.Focus();
        }
        #endregion (btnInputName_Click)

        //-------------------------------------------------------------------------------
        #region +AddSpecifiedUser ユーザー名を指定して追加
        //-------------------------------------------------------------------------------
        //
        public void AddSpecifiedUser(string screen_name)
        {
            UserProfile prof = FrmMain.Twitter.users_show(screen_name: screen_name);
            AddUser(prof.ScreenName, true);
        }
        #endregion (AddSpecifiedUser)
        //-------------------------------------------------------------------------------
        #region -GetFollowers フォロワー追加取得
        //-------------------------------------------------------------------------------
        //
        private object _objlock_followers = new object();
        private long[] _followers = null;
        private long _follower_next_cursor = -1;
        private int _followers_current_cursor = -1;
        private void GetFollowers()
        {
            lock (_objlock_followers) {
                if (_followers == null || _followers_current_cursor == _followers.Length) {
                    var followers_data = FrmMain.Twitter.followers_ids(true, cursor: _follower_next_cursor);

                    _followers = followers_data.Data.ToArray();
                    _follower_next_cursor = followers_data.NextCursor;
                    _followers_current_cursor = 0;
                }

                int index = _followers_current_cursor;
                long[] ids = Utilization.SliceArray(_followers, ref index, 100);
                var profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                _followers_current_cursor = index;
                bool appendEnable = (_follower_next_cursor != 0 || _followers_current_cursor < _followers.Length);

                foreach (var prof in profiles) {
                    AddUser(prof.ScreenName, false);
                }
            }
        }
        #endregion (GetFollowers)
        //-------------------------------------------------------------------------------
        #region -GetFriends フレンド追加取得
        //-------------------------------------------------------------------------------
        //
        private object _objlock_friends = new object();
        private long[] _friends = null;
        private long _friend_next_cursor = -1;
        private int _friends_current_cursor = -1;
        private void GetFriends()
        {
            lock (_objlock_friends) {
                if (_friends == null || _friends_current_cursor == _friends.Length) {
                    var friends_data = FrmMain.Twitter.friends_ids(true, cursor: _follower_next_cursor);

                    _friends = friends_data.Data.ToArray();
                    _friend_next_cursor = friends_data.NextCursor;
                    _friends_current_cursor = 0;
                }

                int index = _friends_current_cursor;
                long[] ids = Utilization.SliceArray(_friends, ref index, 100);
                var profiles = Utilization.SortProfiles(FrmMain.Twitter.users_lookup(ids), ids);
                _friends_current_cursor = index;
                bool appendEnable = (_friend_next_cursor != 0 || _friends_current_cursor < _friends.Length);

                foreach (var prof in profiles) {
                    AddUser(prof.ScreenName, false);
                }
            }
        }
        #endregion (GetFriends)        

        //-------------------------------------------------------------------------------
        #region -AddUser ユーザー追加
        //-------------------------------------------------------------------------------
        //
        private void AddUser(string user, bool warning_in_case_of_dup)
        {
            lock (this) {
                if (warning_in_case_of_dup && chlsbUsers.Items.Contains(user)) {
                    Message.ShowWarningMessage(string.Format("名前 {0} は既に存在します。", user));
                    return;
                }

                chlsbUsers.Items.Add(user);
            }
        }
        #endregion (AddUser)

        //-------------------------------------------------------------------------------
        #region -GetAndAddSpecifiedUserOfTextBox 
        //-------------------------------------------------------------------------------
        //
        private void GetAndAddSpecifiedUserOfTextBox()
        {
            txtInputName.Visible = false;
            chlsbUsers.Items.RemoveAt(chlsbUsers.Items.Count - 1);
            if (txtInputName.Text.Length > 0) {
                try {
                    AddSpecifiedUser(txtInputName.Text);
                }
                catch (TwitterAPIException ex) {
                    if (ex.ErrorStatusCode == 404) { // Unknown User
                        Message.ShowWarningMessage("存在しないユーザーです。");
                    }
                    else {
                        if (ErrorHandler != null) { ErrorHandler(ex); }
                    }
                    return;
                }
            }
        }
        #endregion (GetAndAddSpecifiedUserOfTextBox)
    }
}
