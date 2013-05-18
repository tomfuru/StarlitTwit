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
        public event EventHandler<SelectedUserNamesChangingEventArgs> SelectedUserNamesChanging;

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        //
        public UserSelector()
        {
            InitializeComponent();
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region #[override]OnLoad
        //-------------------------------------------------------------------------------
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (FrmMain.Twitter != null) {
                if (!FrmMain.Twitter.IsAuthenticated) {
                    btnAddFollowers.Enabled = btnAddFriends.Enabled = false;
                }
                else {
                    AddUser(FrmMain.Twitter.ScreenName, false);
                }
            }
        }
        #endregion (#[override]OnLoad)

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

        //-------------------------------------------------------------------------------
        #region SelectedUserNames プロパティ：
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> SelectedUserNames
        {
            get { return chlsbUsers.CheckedItems.Cast<string>(); }
        }
        #endregion (SelectedUserNames)

        //-------------------------------------------------------------------------------
        #region SelectedUserNum プロパティ：
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public int SelectedUserNum
        {
            get { return chlsbUsers.SelectedIndices.Count; }
        }
        #endregion (SelectedUserNum)

        public Action<string> Notifier { get; set; }

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
        #region btnInputName_Click 名前入力
        //-------------------------------------------------------------------------------
        //
        private void btnInputName_Click(object sender, EventArgs e)
        {
            using (FrmInputName frm = new FrmInputName()) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    GetAndAddSpecifiedUserOfTextBox(frm.UserName);
                }
            }
        }
        #endregion (btnInputName_Click)

        //-------------------------------------------------------------------------------
        #region chlsbUsers_ItemCheck アイテムのチェックが変更された場合
        //-------------------------------------------------------------------------------
        //
        private void chlsbUsers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (SelectedUserNamesChanging != null) {
                int num_selected = chlsbUsers.CheckedIndices.Count;
                if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked) {
                    num_selected--;
                }
                else if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked) {
                    num_selected++;
                }

                SelectedUserNamesChanging(this, new SelectedUserNamesChangingEventArgs(num_selected));
            }
        }
        #endregion (chlsbUsers_ItemCheck)

        //-------------------------------------------------------------------------------
        #region +AddSpecifiedUser ユーザー名を指定して追加
        //-------------------------------------------------------------------------------
        //
        public bool AddSpecifiedUser(string screen_name)
        {
            UserProfile prof = FrmMain.Twitter.users_show(screen_name: screen_name);
            return AddUser(prof.ScreenName, true);
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
            if (Notifier != null) { Notifier("フォロワー取得中..."); }
            lock (_objlock_followers) {
                try {
                    if (_followers == null || _followers_current_cursor == _followers.Length) {
                        var followers_data = FrmMain.Twitter.followers_ids(cursor: _follower_next_cursor);

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

                    btnAddFollowers.Enabled = appendEnable;
                    if (Notifier != null) { Notifier((appendEnable) ? "フォロワーを追加しました。" : "全てのフォロワーを追加しました。"); }
                }
                catch (TwitterAPIException ex) {
                    if (Notifier != null) { Notifier(Twitter.MakeTwitterAPIExceptionStr(ex)); }
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
            if (Notifier != null) { Notifier("フレンド取得中..."); }
            lock (_objlock_friends) {
                try {
                    if (_friends == null || _friends_current_cursor == _friends.Length) {
                        var friends_data = FrmMain.Twitter.friends_ids(cursor: _friend_next_cursor);

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
                    btnAddFriends.Enabled = appendEnable;
                    if (Notifier != null) { Notifier((appendEnable) ? "フレンドを追加しました。" : "全てのフレンドを追加しました。"); }
                }
                catch (TwitterAPIException ex) {
                    if (Notifier != null) { Notifier(Twitter.MakeTwitterAPIExceptionStr(ex)); }
                }
            }
        }
        #endregion (GetFriends)

        //-------------------------------------------------------------------------------
        #region -AddUser ユーザー追加
        //-------------------------------------------------------------------------------
        //
        private bool AddUser(string user, bool warning_in_case_of_dup)
        {
            lock (this) {
                if (chlsbUsers.Items.Contains(user)) {
                    if (warning_in_case_of_dup) {
                        Message.ShowWarningMessage(string.Format("名前 {0} は既に存在します。", user));
                    }
                    return false;
                }

                chlsbUsers.Items.Add(user);
                return true;
            }
        }
        #endregion (AddUser)

        //-------------------------------------------------------------------------------
        #region -GetAndAddSpecifiedUserOfTextBox
        //-------------------------------------------------------------------------------
        //
        private void GetAndAddSpecifiedUserOfTextBox(string name)
        {
            try {
                bool successed = AddSpecifiedUser(name);
                if (successed && Notifier != null) { Notifier("ユーザを追加しました"); }
            }
            catch (TwitterAPIException ex) {
                if (ex.ErrorStatusCode == 404) { // Unknown User
                    Message.ShowWarningMessage("存在しないユーザーです。");
                }
                else {
                    if (Notifier != null) { Notifier(Twitter.MakeTwitterAPIExceptionStr(ex)); }
                }
                return;
            }

        }
        #endregion (GetAndAddSpecifiedUserOfTextBox)
    }

    public class SelectedUserNamesChangingEventArgs : EventArgs
    {
        public int SelectedItemsNum { get; private set; }
        public SelectedUserNamesChangingEventArgs(int num_selected)
        {
            SelectedItemsNum = num_selected;
        }
    }
}
