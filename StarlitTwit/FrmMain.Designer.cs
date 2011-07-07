namespace StarlitTwit
{
    partial class FrmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmi_ファイル = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiファイル_設定 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiファイル_Sep = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiファイル_終了 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_機能 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi更新 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSpecifyTime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearTweets = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAPIRestriction = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi認証 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi画面設定 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiタブの位置 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsComboTabAlignment = new System.Windows.Forms.ToolStripComboBox();
            this.tsmi_プロフィール = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiプロフィール更新 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi自分のプロフィール = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi自分のリスト = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiフォロー中のリスト = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi自分のお気に入り = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi自分のリツイート = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiフォロワーのリツイート = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi自分がされたリツイート = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiブロックユーザーリスト = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUserStream = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUserStreamStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUserStreamEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUserStreamLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_子画面 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi子画面_nothing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSep子画面 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi全小画面を消去 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tssLabel = new StarlitTwit.ToolStripStatusLabelEx();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsslRestAPI = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiTab_MakeTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTab_EditTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTab_DeleteTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tasktray = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuTasktray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiTasktray_Display = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTasktray_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContainer2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCon2_MakeTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShortenType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmibit_ly = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmij_mp = new System.Windows.Forms.ToolStripMenuItem();
            this.splContainer = new System.Windows.Forms.SplitContainer();
            this.llblList = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserStreamInfo = new System.Windows.Forms.Label();
            this.btnURLShorten = new StarlitTwit.SplitButton();
            this.lblStatuses = new System.Windows.Forms.Label();
            this.lblStatusesl = new System.Windows.Forms.Label();
            this.llblFollower = new System.Windows.Forms.LinkLabel();
            this.lblFollowerl = new System.Windows.Forms.Label();
            this.llblFollowing = new System.Windows.Forms.LinkLabel();
            this.lblFollowingl = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnStateReset = new System.Windows.Forms.Button();
            this.lblTweetStatus = new System.Windows.Forms.Label();
            this.lblRest = new System.Windows.Forms.Label();
            this.btnTwit = new System.Windows.Forms.Button();
            this.rtxtTwit = new StarlitTwit.RichTextBoxEx();
            this.tabTwitDisp = new StarlitTwit.TabControlEx();
            this.tabpgHome = new StarlitTwit.TabPageEx();
            this.uctlDispHome = new StarlitTwit.UctlDispTwit();
            this.tabpgReply = new StarlitTwit.TabPageEx();
            this.uctlDispReply = new StarlitTwit.UctlDispTwit();
            this.tabpgHistory = new StarlitTwit.TabPageEx();
            this.uctlDispHistory = new StarlitTwit.UctlDispTwit();
            this.tabpgDirect = new StarlitTwit.TabPageEx();
            this.uctlDispDirect = new StarlitTwit.UctlDispTwit();
            this.imageListWrapper = new StarlitTwit.ImageListWrapper();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuTab.SuspendLayout();
            this.menuTasktray.SuspendLayout();
            this.menuContainer2.SuspendLayout();
            this.menuShortenType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splContainer)).BeginInit();
            this.splContainer.Panel1.SuspendLayout();
            this.splContainer.Panel2.SuspendLayout();
            this.splContainer.SuspendLayout();
            this.tabTwitDisp.SuspendLayout();
            this.tabpgHome.SuspendLayout();
            this.tabpgReply.SuspendLayout();
            this.tabpgHistory.SuspendLayout();
            this.tabpgDirect.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_ファイル,
            this.tsmi_機能,
            this.tsmi_プロフィール,
            this.tsmiUserStream,
            this.tsmi_子画面});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(492, 26);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmi_ファイル
            // 
            this.tsmi_ファイル.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiファイル_設定,
            this.tsmiファイル_Sep,
            this.tsmiファイル_終了});
            this.tsmi_ファイル.Name = "tsmi_ファイル";
            this.tsmi_ファイル.Size = new System.Drawing.Size(85, 22);
            this.tsmi_ファイル.Text = "ファイル(&F)";
            // 
            // tsmiファイル_設定
            // 
            this.tsmiファイル_設定.Name = "tsmiファイル_設定";
            this.tsmiファイル_設定.Size = new System.Drawing.Size(119, 22);
            this.tsmiファイル_設定.Text = "設定(&C)";
            this.tsmiファイル_設定.Click += new System.EventHandler(this.tsmiファイル_設定_Click);
            // 
            // tsmiファイル_Sep
            // 
            this.tsmiファイル_Sep.Name = "tsmiファイル_Sep";
            this.tsmiファイル_Sep.Size = new System.Drawing.Size(116, 6);
            // 
            // tsmiファイル_終了
            // 
            this.tsmiファイル_終了.Name = "tsmiファイル_終了";
            this.tsmiファイル_終了.Size = new System.Drawing.Size(119, 22);
            this.tsmiファイル_終了.Text = "終了(&Q)";
            this.tsmiファイル_終了.Click += new System.EventHandler(this.tsmiファイル_終了_Click);
            // 
            // tsmi_機能
            // 
            this.tsmi_機能.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi更新,
            this.tsmiSpecifyTime,
            this.tsmiClearTweets,
            this.toolStripMenuItem2,
            this.tsmiAPIRestriction,
            this.tsmi認証,
            this.toolStripMenuItem3,
            this.tsmi画面設定});
            this.tsmi_機能.Name = "tsmi_機能";
            this.tsmi_機能.Size = new System.Drawing.Size(62, 22);
            this.tsmi_機能.Text = "機能(&A)";
            // 
            // tsmi更新
            // 
            this.tsmi更新.Enabled = false;
            this.tsmi更新.Name = "tsmi更新";
            this.tsmi更新.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.tsmi更新.Size = new System.Drawing.Size(227, 22);
            this.tsmi更新.Text = "更新(&R)";
            this.tsmi更新.Click += new System.EventHandler(this.tsmi更新_Click);
            // 
            // tsmiSpecifyTime
            // 
            this.tsmiSpecifyTime.Enabled = false;
            this.tsmiSpecifyTime.Name = "tsmiSpecifyTime";
            this.tsmiSpecifyTime.Size = new System.Drawing.Size(227, 22);
            this.tsmiSpecifyTime.Text = "時刻を指定して発言取得(&G)";
            this.tsmiSpecifyTime.Click += new System.EventHandler(this.tsmiSpecifyTime_Click);
            // 
            // tsmiClearTweets
            // 
            this.tsmiClearTweets.Enabled = false;
            this.tsmiClearTweets.Name = "tsmiClearTweets";
            this.tsmiClearTweets.Size = new System.Drawing.Size(227, 22);
            this.tsmiClearTweets.Text = "発言のクリア(&C)";
            this.tsmiClearTweets.Click += new System.EventHandler(this.tsmiClearTweets_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(224, 6);
            // 
            // tsmiAPIRestriction
            // 
            this.tsmiAPIRestriction.Enabled = false;
            this.tsmiAPIRestriction.Name = "tsmiAPIRestriction";
            this.tsmiAPIRestriction.Size = new System.Drawing.Size(227, 22);
            this.tsmiAPIRestriction.Text = "API使用回数制限情報(&A)";
            this.tsmiAPIRestriction.Click += new System.EventHandler(this.tsmiAPIRestriction_Click);
            // 
            // tsmi認証
            // 
            this.tsmi認証.Name = "tsmi認証";
            this.tsmi認証.Size = new System.Drawing.Size(227, 22);
            this.tsmi認証.Text = "認証(&O)";
            this.tsmi認証.Click += new System.EventHandler(this.tsmi認証_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(224, 6);
            // 
            // tsmi画面設定
            // 
            this.tsmi画面設定.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiタブの位置});
            this.tsmi画面設定.Name = "tsmi画面設定";
            this.tsmi画面設定.Size = new System.Drawing.Size(227, 22);
            this.tsmi画面設定.Text = "画面設定(&C)";
            // 
            // tsmiタブの位置
            // 
            this.tsmiタブの位置.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsComboTabAlignment});
            this.tsmiタブの位置.Name = "tsmiタブの位置";
            this.tsmiタブの位置.Size = new System.Drawing.Size(154, 22);
            this.tsmiタブの位置.Text = "タブの位置(&T)";
            // 
            // tsComboTabAlignment
            // 
            this.tsComboTabAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsComboTabAlignment.Name = "tsComboTabAlignment";
            this.tsComboTabAlignment.Size = new System.Drawing.Size(121, 26);
            this.tsComboTabAlignment.SelectedIndexChanged += new System.EventHandler(this.tsComboTabAlignment_SelectedIndexChanged);
            // 
            // tsmi_プロフィール
            // 
            this.tsmi_プロフィール.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiプロフィール更新,
            this.tsmi自分のプロフィール,
            this.toolStripMenuItem5,
            this.tsmi自分のリスト,
            this.tsmiフォロー中のリスト,
            this.toolStripMenuItem6,
            this.tsmi自分のお気に入り,
            this.toolStripMenuItem8,
            this.tsmi自分のリツイート,
            this.tsmiフォロワーのリツイート,
            this.tsmi自分がされたリツイート,
            this.toolStripMenuItem7,
            this.tsmiブロックユーザーリスト});
            this.tsmi_プロフィール.Enabled = false;
            this.tsmi_プロフィール.Name = "tsmi_プロフィール";
            this.tsmi_プロフィール.Size = new System.Drawing.Size(109, 22);
            this.tsmi_プロフィール.Text = "プロフィール(&P)";
            // 
            // tsmiプロフィール更新
            // 
            this.tsmiプロフィール更新.Enabled = false;
            this.tsmiプロフィール更新.Name = "tsmiプロフィール更新";
            this.tsmiプロフィール更新.Size = new System.Drawing.Size(261, 22);
            this.tsmiプロフィール更新.Text = "フォロー数・発言数を更新する(&P)";
            this.tsmiプロフィール更新.Click += new System.EventHandler(this.tsmiプロフィール更新_Click);
            // 
            // tsmi自分のプロフィール
            // 
            this.tsmi自分のプロフィール.Enabled = false;
            this.tsmi自分のプロフィール.Name = "tsmi自分のプロフィール";
            this.tsmi自分のプロフィール.Size = new System.Drawing.Size(261, 22);
            this.tsmi自分のプロフィール.Text = "自分のプロフィール(&M)";
            this.tsmi自分のプロフィール.Click += new System.EventHandler(this.tsmi自分のプロフィール_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(258, 6);
            // 
            // tsmi自分のリスト
            // 
            this.tsmi自分のリスト.Enabled = false;
            this.tsmi自分のリスト.Name = "tsmi自分のリスト";
            this.tsmi自分のリスト.Size = new System.Drawing.Size(261, 22);
            this.tsmi自分のリスト.Text = "自分のリスト(&L)";
            this.tsmi自分のリスト.Click += new System.EventHandler(this.tsmi自分のリスト_Click);
            // 
            // tsmiフォロー中のリスト
            // 
            this.tsmiフォロー中のリスト.Enabled = false;
            this.tsmiフォロー中のリスト.Name = "tsmiフォロー中のリスト";
            this.tsmiフォロー中のリスト.Size = new System.Drawing.Size(261, 22);
            this.tsmiフォロー中のリスト.Text = "フォロー中のリスト(&S)";
            this.tsmiフォロー中のリスト.Click += new System.EventHandler(this.tsmiフォロー中のリスト_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(258, 6);
            // 
            // tsmi自分のお気に入り
            // 
            this.tsmi自分のお気に入り.Enabled = false;
            this.tsmi自分のお気に入り.Name = "tsmi自分のお気に入り";
            this.tsmi自分のお気に入り.Size = new System.Drawing.Size(261, 22);
            this.tsmi自分のお気に入り.Text = "自分のお気に入り(&F)";
            this.tsmi自分のお気に入り.Click += new System.EventHandler(this.tsmi自分のお気に入り_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(258, 6);
            // 
            // tsmi自分のリツイート
            // 
            this.tsmi自分のリツイート.Enabled = false;
            this.tsmi自分のリツイート.Name = "tsmi自分のリツイート";
            this.tsmi自分のリツイート.Size = new System.Drawing.Size(261, 22);
            this.tsmi自分のリツイート.Text = "自分のリツイート(&R)";
            this.tsmi自分のリツイート.Click += new System.EventHandler(this.tsmi自分のリツイート_Click);
            // 
            // tsmiフォロワーのリツイート
            // 
            this.tsmiフォロワーのリツイート.Enabled = false;
            this.tsmiフォロワーのリツイート.Name = "tsmiフォロワーのリツイート";
            this.tsmiフォロワーのリツイート.Size = new System.Drawing.Size(261, 22);
            this.tsmiフォロワーのリツイート.Text = "フォロワーのリツイート(&T)";
            this.tsmiフォロワーのリツイート.Click += new System.EventHandler(this.tsmiフォロワーのリツイート_Click);
            // 
            // tsmi自分がされたリツイート
            // 
            this.tsmi自分がされたリツイート.Enabled = false;
            this.tsmi自分がされたリツイート.Name = "tsmi自分がされたリツイート";
            this.tsmi自分がされたリツイート.Size = new System.Drawing.Size(261, 22);
            this.tsmi自分がされたリツイート.Text = "自分がされたリツイート(&O)";
            this.tsmi自分がされたリツイート.Click += new System.EventHandler(this.tsmi自分がされたリツイート_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(258, 6);
            // 
            // tsmiブロックユーザーリスト
            // 
            this.tsmiブロックユーザーリスト.Enabled = false;
            this.tsmiブロックユーザーリスト.Name = "tsmiブロックユーザーリスト";
            this.tsmiブロックユーザーリスト.Size = new System.Drawing.Size(261, 22);
            this.tsmiブロックユーザーリスト.Text = "ブロックユーザー(&B)";
            this.tsmiブロックユーザーリスト.Click += new System.EventHandler(this.tsmiブロックユーザーリスト_Click);
            // 
            // tsmiUserStream
            // 
            this.tsmiUserStream.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUserStreamStart,
            this.tsmiUserStreamEnd,
            this.toolStripMenuItem4,
            this.tsmiUserStreamLog});
            this.tsmiUserStream.Enabled = false;
            this.tsmiUserStream.Name = "tsmiUserStream";
            this.tsmiUserStream.Size = new System.Drawing.Size(110, 22);
            this.tsmiUserStream.Text = "UserStream(&U)";
            this.tsmiUserStream.DropDownOpening += new System.EventHandler(this.tsmiUserStream_DropDownOpening);
            // 
            // tsmiUserStreamStart
            // 
            this.tsmiUserStreamStart.Enabled = false;
            this.tsmiUserStreamStart.Name = "tsmiUserStreamStart";
            this.tsmiUserStreamStart.Size = new System.Drawing.Size(118, 22);
            this.tsmiUserStreamStart.Text = "開始(&S)";
            this.tsmiUserStreamStart.Click += new System.EventHandler(this.tsmiUserStreamStart_Click);
            // 
            // tsmiUserStreamEnd
            // 
            this.tsmiUserStreamEnd.Enabled = false;
            this.tsmiUserStreamEnd.Name = "tsmiUserStreamEnd";
            this.tsmiUserStreamEnd.Size = new System.Drawing.Size(118, 22);
            this.tsmiUserStreamEnd.Text = "終了(&E)";
            this.tsmiUserStreamEnd.Click += new System.EventHandler(this.tsmiUserStreamEnd_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(115, 6);
            // 
            // tsmiUserStreamLog
            // 
            this.tsmiUserStreamLog.Enabled = false;
            this.tsmiUserStreamLog.Name = "tsmiUserStreamLog";
            this.tsmiUserStreamLog.Size = new System.Drawing.Size(118, 22);
            this.tsmiUserStreamLog.Text = "ログ(&L)";
            this.tsmiUserStreamLog.Click += new System.EventHandler(this.tsmiUserStreamLog_Click);
            // 
            // tsmi_子画面
            // 
            this.tsmi_子画面.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi子画面_nothing,
            this.tsSep子画面,
            this.tsmi全小画面を消去});
            this.tsmi_子画面.Enabled = false;
            this.tsmi_子画面.Name = "tsmi_子画面";
            this.tsmi_子画面.Size = new System.Drawing.Size(74, 22);
            this.tsmi_子画面.Text = "子画面(&C)";
            this.tsmi_子画面.DropDownOpening += new System.EventHandler(this.tsmi_子画面_DropDownOpening);
            // 
            // tsmi子画面_nothing
            // 
            this.tsmi子画面_nothing.Enabled = false;
            this.tsmi子画面_nothing.Name = "tsmi子画面_nothing";
            this.tsmi子画面_nothing.Size = new System.Drawing.Size(179, 22);
            this.tsmi子画面_nothing.Text = "(なし)";
            // 
            // tsSep子画面
            // 
            this.tsSep子画面.Name = "tsSep子画面";
            this.tsSep子画面.Size = new System.Drawing.Size(176, 6);
            // 
            // tsmi全小画面を消去
            // 
            this.tsmi全小画面を消去.Name = "tsmi全小画面を消去";
            this.tsmi全小画面を消去.Size = new System.Drawing.Size(179, 22);
            this.tsmi全小画面を消去.Text = "全子画面を消去(&D)";
            this.tsmi全小画面を消去.Click += new System.EventHandler(this.tsmi全子画面を消去_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLabel,
            this.toolStripSeparator1,
            this.tsslRestAPI});
            this.statusStrip.Location = new System.Drawing.Point(0, 643);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(492, 23);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tssLabel
            // 
            this.tssLabel.AutoSize = false;
            this.tssLabel.Name = "tssLabel";
            this.tssLabel.Size = new System.Drawing.Size(371, 18);
            this.tssLabel.Spring = true;
            this.tssLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // tsslRestAPI
            // 
            this.tsslRestAPI.AutoSize = false;
            this.tsslRestAPI.Name = "tsslRestAPI";
            this.tsslRestAPI.Size = new System.Drawing.Size(100, 18);
            this.tsslRestAPI.Text = "...";
            // 
            // menuTab
            // 
            this.menuTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTab_MakeTab,
            this.tsmiTab_EditTab,
            this.tsmiTab_DeleteTab});
            this.menuTab.Name = "menuTab";
            this.menuTab.Size = new System.Drawing.Size(157, 70);
            this.menuTab.Opening += new System.ComponentModel.CancelEventHandler(this.menuTab_Opening);
            // 
            // tsmiTab_MakeTab
            // 
            this.tsmiTab_MakeTab.Name = "tsmiTab_MakeTab";
            this.tsmiTab_MakeTab.Size = new System.Drawing.Size(156, 22);
            this.tsmiTab_MakeTab.Text = "タブの作成(&M)";
            this.tsmiTab_MakeTab.Click += new System.EventHandler(this.tsmiTab_MakeTab_Click);
            // 
            // tsmiTab_EditTab
            // 
            this.tsmiTab_EditTab.Name = "tsmiTab_EditTab";
            this.tsmiTab_EditTab.Size = new System.Drawing.Size(156, 22);
            this.tsmiTab_EditTab.Text = "タブの編集(&E)";
            this.tsmiTab_EditTab.Click += new System.EventHandler(this.tsmiTab_EditTab_Click);
            // 
            // tsmiTab_DeleteTab
            // 
            this.tsmiTab_DeleteTab.Name = "tsmiTab_DeleteTab";
            this.tsmiTab_DeleteTab.Size = new System.Drawing.Size(156, 22);
            this.tsmiTab_DeleteTab.Text = "タブの削除(&D)";
            this.tsmiTab_DeleteTab.Click += new System.EventHandler(this.tsmiTab_DeleteTab_Click);
            // 
            // tasktray
            // 
            this.tasktray.ContextMenuStrip = this.menuTasktray;
            this.tasktray.Icon = ((System.Drawing.Icon)(resources.GetObject("tasktray.Icon")));
            this.tasktray.Text = "StarlitTwit";
            this.tasktray.Visible = true;
            // 
            // menuTasktray
            // 
            this.menuTasktray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTasktray_Display,
            this.toolStripMenuItem1,
            this.tsmiTasktray_Exit});
            this.menuTasktray.Name = "menuTasktray";
            this.menuTasktray.Size = new System.Drawing.Size(120, 54);
            // 
            // tsmiTasktray_Display
            // 
            this.tsmiTasktray_Display.Name = "tsmiTasktray_Display";
            this.tsmiTasktray_Display.Size = new System.Drawing.Size(119, 22);
            this.tsmiTasktray_Display.Text = "表示(&D)";
            this.tsmiTasktray_Display.Click += new System.EventHandler(this.tsmiTasktray_Display_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(116, 6);
            // 
            // tsmiTasktray_Exit
            // 
            this.tsmiTasktray_Exit.Name = "tsmiTasktray_Exit";
            this.tsmiTasktray_Exit.Size = new System.Drawing.Size(119, 22);
            this.tsmiTasktray_Exit.Text = "終了(&E)";
            this.tsmiTasktray_Exit.Click += new System.EventHandler(this.tsmiTasktray_Exit_Click);
            // 
            // menuContainer2
            // 
            this.menuContainer2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCon2_MakeTab});
            this.menuContainer2.Name = "menuContainer2";
            this.menuContainer2.Size = new System.Drawing.Size(157, 26);
            // 
            // tsmiCon2_MakeTab
            // 
            this.tsmiCon2_MakeTab.Name = "tsmiCon2_MakeTab";
            this.tsmiCon2_MakeTab.Size = new System.Drawing.Size(156, 22);
            this.tsmiCon2_MakeTab.Text = "タブの作成(&M)";
            this.tsmiCon2_MakeTab.Click += new System.EventHandler(this.tsmiTab_MakeTab_Click);
            // 
            // menuShortenType
            // 
            this.menuShortenType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmibit_ly,
            this.tsmij_mp});
            this.menuShortenType.Name = "menuShortenType";
            this.menuShortenType.ShowCheckMargin = true;
            this.menuShortenType.ShowImageMargin = false;
            this.menuShortenType.Size = new System.Drawing.Size(106, 48);
            this.menuShortenType.Opening += new System.ComponentModel.CancelEventHandler(this.menuShortenType_Opening);
            // 
            // tsmibit_ly
            // 
            this.tsmibit_ly.Name = "tsmibit_ly";
            this.tsmibit_ly.Size = new System.Drawing.Size(105, 22);
            this.tsmibit_ly.Tag = StarlitTwit.URLShortenType.bit_ly;
            this.tsmibit_ly.Text = "bit.ly";
            this.tsmibit_ly.Click += new System.EventHandler(this.btnURLShorten_MenuClick);
            // 
            // tsmij_mp
            // 
            this.tsmij_mp.Name = "tsmij_mp";
            this.tsmij_mp.Size = new System.Drawing.Size(105, 22);
            this.tsmij_mp.Tag = StarlitTwit.URLShortenType.j_mp;
            this.tsmij_mp.Text = "j.mp";
            this.tsmij_mp.Click += new System.EventHandler(this.btnURLShorten_MenuClick);
            // 
            // splContainer
            // 
            this.splContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splContainer.Location = new System.Drawing.Point(0, 26);
            this.splContainer.Margin = new System.Windows.Forms.Padding(0);
            this.splContainer.Name = "splContainer";
            this.splContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splContainer.Panel1
            // 
            this.splContainer.Panel1.Controls.Add(this.llblList);
            this.splContainer.Panel1.Controls.Add(this.label1);
            this.splContainer.Panel1.Controls.Add(this.lblUserStreamInfo);
            this.splContainer.Panel1.Controls.Add(this.btnURLShorten);
            this.splContainer.Panel1.Controls.Add(this.lblStatuses);
            this.splContainer.Panel1.Controls.Add(this.lblStatusesl);
            this.splContainer.Panel1.Controls.Add(this.llblFollower);
            this.splContainer.Panel1.Controls.Add(this.lblFollowerl);
            this.splContainer.Panel1.Controls.Add(this.llblFollowing);
            this.splContainer.Panel1.Controls.Add(this.lblFollowingl);
            this.splContainer.Panel1.Controls.Add(this.lblUserName);
            this.splContainer.Panel1.Controls.Add(this.btnStateReset);
            this.splContainer.Panel1.Controls.Add(this.lblTweetStatus);
            this.splContainer.Panel1.Controls.Add(this.lblRest);
            this.splContainer.Panel1.Controls.Add(this.btnTwit);
            this.splContainer.Panel1.Controls.Add(this.rtxtTwit);
            this.splContainer.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splContainer_Panel1_MouseMove);
            this.splContainer.Panel1MinSize = 80;
            // 
            // splContainer.Panel2
            // 
            this.splContainer.Panel2.Controls.Add(this.tabTwitDisp);
            this.splContainer.Panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.splContainer_Panel2_MouseClick);
            this.splContainer.Size = new System.Drawing.Size(492, 617);
            this.splContainer.SplitterDistance = 98;
            this.splContainer.SplitterWidth = 2;
            this.splContainer.TabIndex = 0;
            this.splContainer.TabStop = false;
            // 
            // llblList
            // 
            this.llblList.AutoSize = true;
            this.llblList.Enabled = false;
            this.llblList.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblList.LinkColor = System.Drawing.Color.Black;
            this.llblList.Location = new System.Drawing.Point(350, 22);
            this.llblList.Name = "llblList";
            this.llblList.Size = new System.Drawing.Size(11, 12);
            this.llblList.TabIndex = 15;
            this.llblList.TabStop = true;
            this.llblList.Text = "-";
            this.llblList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblList_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "List:";
            // 
            // lblUserStreamInfo
            // 
            this.lblUserStreamInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserStreamInfo.AutoSize = true;
            this.lblUserStreamInfo.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUserStreamInfo.Location = new System.Drawing.Point(362, 4);
            this.lblUserStreamInfo.Name = "lblUserStreamInfo";
            this.lblUserStreamInfo.Size = new System.Drawing.Size(14, 12);
            this.lblUserStreamInfo.TabIndex = 13;
            this.lblUserStreamInfo.Text = "...";
            // 
            // btnURLShorten
            // 
            this.btnURLShorten.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnURLShorten.ClickedImage = "Clicked";
            this.btnURLShorten.ContextMenuStrip = this.menuShortenType;
            this.btnURLShorten.DisabledImage = "Disabled";
            this.btnURLShorten.Enabled = false;
            this.btnURLShorten.FocusedImage = "Focused";
            this.btnURLShorten.HoverImage = "Hover";
            this.btnURLShorten.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnURLShorten.ImageKey = "Normal";
            this.btnURLShorten.Location = new System.Drawing.Point(409, 55);
            this.btnURLShorten.Name = "btnURLShorten";
            this.btnURLShorten.NormalImage = "Normal";
            this.btnURLShorten.Size = new System.Drawing.Size(75, 20);
            this.btnURLShorten.TabIndex = 4;
            this.btnURLShorten.Text = "URL短縮";
            this.btnURLShorten.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnURLShorten.UseVisualStyleBackColor = true;
            this.btnURLShorten.ButtonClick += new System.EventHandler(this.btnURLShorten_ButtonClick);
            // 
            // lblStatuses
            // 
            this.lblStatuses.AutoSize = true;
            this.lblStatuses.Location = new System.Drawing.Point(271, 22);
            this.lblStatuses.Name = "lblStatuses";
            this.lblStatuses.Size = new System.Drawing.Size(11, 12);
            this.lblStatuses.TabIndex = 1;
            this.lblStatuses.Text = "-";
            // 
            // lblStatusesl
            // 
            this.lblStatusesl.AutoSize = true;
            this.lblStatusesl.Location = new System.Drawing.Point(221, 22);
            this.lblStatusesl.Name = "lblStatusesl";
            this.lblStatusesl.Size = new System.Drawing.Size(52, 12);
            this.lblStatusesl.TabIndex = 0;
            this.lblStatusesl.Text = "Statuses:";
            // 
            // llblFollower
            // 
            this.llblFollower.AutoSize = true;
            this.llblFollower.Enabled = false;
            this.llblFollower.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblFollower.LinkColor = System.Drawing.Color.Black;
            this.llblFollower.Location = new System.Drawing.Point(168, 22);
            this.llblFollower.Name = "llblFollower";
            this.llblFollower.Size = new System.Drawing.Size(11, 12);
            this.llblFollower.TabIndex = 1;
            this.llblFollower.TabStop = true;
            this.llblFollower.Text = "-";
            this.llblFollower.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblFollower_LinkClicked);
            // 
            // lblFollowerl
            // 
            this.lblFollowerl.AutoSize = true;
            this.lblFollowerl.Location = new System.Drawing.Point(118, 22);
            this.lblFollowerl.Name = "lblFollowerl";
            this.lblFollowerl.Size = new System.Drawing.Size(50, 12);
            this.lblFollowerl.TabIndex = 12;
            this.lblFollowerl.Text = "Follower:";
            // 
            // llblFollowing
            // 
            this.llblFollowing.AutoSize = true;
            this.llblFollowing.Enabled = false;
            this.llblFollowing.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblFollowing.LinkColor = System.Drawing.Color.Black;
            this.llblFollowing.Location = new System.Drawing.Point(65, 23);
            this.llblFollowing.Name = "llblFollowing";
            this.llblFollowing.Size = new System.Drawing.Size(11, 12);
            this.llblFollowing.TabIndex = 0;
            this.llblFollowing.TabStop = true;
            this.llblFollowing.Text = "-";
            this.llblFollowing.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblFollowing_LinkClicked);
            // 
            // lblFollowingl
            // 
            this.lblFollowingl.AutoSize = true;
            this.lblFollowingl.Location = new System.Drawing.Point(10, 22);
            this.lblFollowingl.Name = "lblFollowingl";
            this.lblFollowingl.Size = new System.Drawing.Size(55, 12);
            this.lblFollowingl.TabIndex = 11;
            this.lblFollowingl.Text = "Following:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(11, 4);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(49, 12);
            this.lblUserName.TabIndex = 10;
            this.lblUserName.Text = "(未認証)";
            // 
            // btnStateReset
            // 
            this.btnStateReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStateReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStateReset.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.btnStateReset.Location = new System.Drawing.Point(4, 76);
            this.btnStateReset.Name = "btnStateReset";
            this.btnStateReset.Size = new System.Drawing.Size(17, 17);
            this.btnStateReset.TabIndex = 5;
            this.btnStateReset.Text = "×";
            this.btnStateReset.UseVisualStyleBackColor = true;
            this.btnStateReset.Visible = false;
            this.btnStateReset.Click += new System.EventHandler(this.btnStateReset_Click);
            // 
            // lblTweetStatus
            // 
            this.lblTweetStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTweetStatus.AutoSize = true;
            this.lblTweetStatus.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTweetStatus.ForeColor = System.Drawing.Color.Red;
            this.lblTweetStatus.Location = new System.Drawing.Point(21, 82);
            this.lblTweetStatus.Name = "lblTweetStatus";
            this.lblTweetStatus.Size = new System.Drawing.Size(14, 12);
            this.lblTweetStatus.TabIndex = 4;
            this.lblTweetStatus.Text = "...";
            // 
            // lblRest
            // 
            this.lblRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRest.AutoSize = true;
            this.lblRest.Location = new System.Drawing.Point(380, 79);
            this.lblRest.Name = "lblRest";
            this.lblRest.Size = new System.Drawing.Size(23, 12);
            this.lblRest.TabIndex = 2;
            this.lblRest.Text = "140";
            // 
            // btnTwit
            // 
            this.btnTwit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTwit.Location = new System.Drawing.Point(409, 34);
            this.btnTwit.Name = "btnTwit";
            this.btnTwit.Size = new System.Drawing.Size(75, 20);
            this.btnTwit.TabIndex = 3;
            this.btnTwit.Text = "つぶやく";
            this.btnTwit.UseVisualStyleBackColor = true;
            this.btnTwit.Click += new System.EventHandler(this.btnTwit_Click);
            // 
            // rtxtTwit
            // 
            this.rtxtTwit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtTwit.DetectUrls = false;
            this.rtxtTwit.Location = new System.Drawing.Point(3, 37);
            this.rtxtTwit.Name = "rtxtTwit";
            this.rtxtTwit.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtTwit.Size = new System.Drawing.Size(400, 39);
            this.rtxtTwit.TabIndex = 2;
            this.rtxtTwit.Text = "";
            this.rtxtTwit.TextChanged += new System.EventHandler(this.rtxtTwit_TextChanged);
            this.rtxtTwit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtxtTwit_KeyDown);
            // 
            // tabTwitDisp
            // 
            this.tabTwitDisp.AllowDrop = true;
            this.tabTwitDisp.Controls.Add(this.tabpgHome);
            this.tabTwitDisp.Controls.Add(this.tabpgReply);
            this.tabTwitDisp.Controls.Add(this.tabpgHistory);
            this.tabTwitDisp.Controls.Add(this.tabpgDirect);
            this.tabTwitDisp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTwitDisp.ItemSize = new System.Drawing.Size(0, 15);
            this.tabTwitDisp.Location = new System.Drawing.Point(0, 0);
            this.tabTwitDisp.Margin = new System.Windows.Forms.Padding(0);
            this.tabTwitDisp.MinMovableIndex = 4;
            this.tabTwitDisp.Multiline = true;
            this.tabTwitDisp.Name = "tabTwitDisp";
            this.tabTwitDisp.Padding = new System.Drawing.Point(0, 0);
            this.tabTwitDisp.SelectedIndex = 0;
            this.tabTwitDisp.SelectedTab = this.tabpgHome;
            this.tabTwitDisp.ShowToolTips = true;
            this.tabTwitDisp.Size = new System.Drawing.Size(488, 513);
            this.tabTwitDisp.TabIndex = 0;
            this.tabTwitDisp.TabExchanged += new System.EventHandler<StarlitTwit.TabMoveEventArgs>(this.tabTwitDisp_TabMoved);
            this.tabTwitDisp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabTwitDisp_MouseDown);
            // 
            // tabpgHome
            // 
            this.tabpgHome.Controls.Add(this.uctlDispHome);
            this.tabpgHome.Location = new System.Drawing.Point(4, 19);
            this.tabpgHome.Name = "tabpgHome";
            this.tabpgHome.Size = new System.Drawing.Size(480, 490);
            this.tabpgHome.TabIndex = 3;
            this.tabpgHome.UseVisualStyleBackColor = true;
            // 
            // uctlDispHome
            // 
            this.uctlDispHome.AutoScroll = true;
            this.uctlDispHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctlDispHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDispHome.ImageListWrapper = null;
            this.uctlDispHome.Location = new System.Drawing.Point(0, 0);
            this.uctlDispHome.Margin = new System.Windows.Forms.Padding(0);
            this.uctlDispHome.Name = "uctlDispHome";
            this.uctlDispHome.Size = new System.Drawing.Size(480, 490);
            this.uctlDispHome.TabIndex = 2;
            // 
            // tabpgReply
            // 
            this.tabpgReply.Controls.Add(this.uctlDispReply);
            this.tabpgReply.Location = new System.Drawing.Point(4, 19);
            this.tabpgReply.Name = "tabpgReply";
            this.tabpgReply.Size = new System.Drawing.Size(480, 492);
            this.tabpgReply.TabIndex = 1;
            this.tabpgReply.UseVisualStyleBackColor = true;
            // 
            // uctlDispReply
            // 
            this.uctlDispReply.AutoScroll = true;
            this.uctlDispReply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctlDispReply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDispReply.ImageListWrapper = null;
            this.uctlDispReply.Location = new System.Drawing.Point(0, 0);
            this.uctlDispReply.Margin = new System.Windows.Forms.Padding(0);
            this.uctlDispReply.Name = "uctlDispReply";
            this.uctlDispReply.Size = new System.Drawing.Size(480, 492);
            this.uctlDispReply.TabIndex = 1;
            // 
            // tabpgHistory
            // 
            this.tabpgHistory.Controls.Add(this.uctlDispHistory);
            this.tabpgHistory.Location = new System.Drawing.Point(4, 19);
            this.tabpgHistory.Name = "tabpgHistory";
            this.tabpgHistory.Size = new System.Drawing.Size(480, 492);
            this.tabpgHistory.TabIndex = 0;
            this.tabpgHistory.UseVisualStyleBackColor = true;
            // 
            // uctlDispHistory
            // 
            this.uctlDispHistory.AutoScroll = true;
            this.uctlDispHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctlDispHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDispHistory.ImageListWrapper = null;
            this.uctlDispHistory.Location = new System.Drawing.Point(0, 0);
            this.uctlDispHistory.Margin = new System.Windows.Forms.Padding(0);
            this.uctlDispHistory.Name = "uctlDispHistory";
            this.uctlDispHistory.Size = new System.Drawing.Size(480, 492);
            this.uctlDispHistory.TabIndex = 0;
            // 
            // tabpgDirect
            // 
            this.tabpgDirect.Controls.Add(this.uctlDispDirect);
            this.tabpgDirect.Location = new System.Drawing.Point(4, 19);
            this.tabpgDirect.Name = "tabpgDirect";
            this.tabpgDirect.Size = new System.Drawing.Size(480, 492);
            this.tabpgDirect.TabIndex = 4;
            this.tabpgDirect.UseVisualStyleBackColor = true;
            // 
            // uctlDispDirect
            // 
            this.uctlDispDirect.AutoScroll = true;
            this.uctlDispDirect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctlDispDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctlDispDirect.ImageListWrapper = null;
            this.uctlDispDirect.Location = new System.Drawing.Point(0, 0);
            this.uctlDispDirect.Margin = new System.Windows.Forms.Padding(0);
            this.uctlDispDirect.Name = "uctlDispDirect";
            this.uctlDispDirect.Size = new System.Drawing.Size(480, 492);
            this.uctlDispDirect.TabIndex = 2;
            // 
            // imageListWrapper
            // 
            // 
            // 
            // 
            this.imageListWrapper.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListWrapper.ImageList.ImageSize = new System.Drawing.Size(48, 48);
            this.imageListWrapper.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 666);
            this.Controls.Add(this.splContainer);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = global::StarlitTwit.Properties.Resources.icon;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmMain";
            this.Text = "StarlitTwit";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuTab.ResumeLayout(false);
            this.menuTasktray.ResumeLayout(false);
            this.menuContainer2.ResumeLayout(false);
            this.menuShortenType.ResumeLayout(false);
            this.splContainer.Panel1.ResumeLayout(false);
            this.splContainer.Panel1.PerformLayout();
            this.splContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splContainer)).EndInit();
            this.splContainer.ResumeLayout(false);
            this.tabTwitDisp.ResumeLayout(false);
            this.tabpgHome.ResumeLayout(false);
            this.tabpgReply.ResumeLayout(false);
            this.tabpgHistory.ResumeLayout(false);
            this.tabpgDirect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTwit;
        private TabControlEx tabTwitDisp;
        private TabPageEx tabpgHistory;
        private System.Windows.Forms.SplitContainer splContainer;
        private UctlDispTwit uctlDispHistory;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ファイル;
        private System.Windows.Forms.ToolStripMenuItem tsmi_機能;
        private System.Windows.Forms.ToolStripMenuItem tsmi認証;
        private TabPageEx tabpgReply;
        private UctlDispTwit uctlDispReply;
        private TabPageEx tabpgHome;
        private UctlDispTwit uctlDispHome;
        private System.Windows.Forms.ToolStripMenuItem tsmiファイル_終了;
        private System.Windows.Forms.ToolStripMenuItem tsmiファイル_設定;
        private System.Windows.Forms.ToolStripSeparator tsmiファイル_Sep;
        private System.Windows.Forms.Label lblRest;
        private System.Windows.Forms.StatusStrip statusStrip;
        private ToolStripStatusLabelEx tssLabel;
        private System.Windows.Forms.ToolStripMenuItem tsmi更新;
        private System.Windows.Forms.Label lblTweetStatus;
        private TabPageEx tabpgDirect;
        private UctlDispTwit uctlDispDirect;
        private System.Windows.Forms.ContextMenuStrip menuTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiTab_MakeTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiTab_DeleteTab;
        private System.Windows.Forms.Button btnStateReset;
        private System.Windows.Forms.ToolStripStatusLabel tsslRestAPI;
        private System.Windows.Forms.ToolStripMenuItem tsmiTab_EditTab;
        internal System.Windows.Forms.NotifyIcon tasktray;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip menuTasktray;
        private System.Windows.Forms.ToolStripMenuItem tsmiTasktray_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiTasktray_Exit;
        private System.Windows.Forms.ContextMenuStrip menuContainer2;
        private System.Windows.Forms.ToolStripMenuItem tsmiCon2_MakeTab;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.LinkLabel llblFollowing;
        private System.Windows.Forms.Label lblFollowingl;
        private System.Windows.Forms.LinkLabel llblFollower;
        private System.Windows.Forms.Label lblFollowerl;
        private System.Windows.Forms.ToolStripMenuItem tsmi_プロフィール;
        private System.Windows.Forms.Label lblStatuses;
        private System.Windows.Forms.Label lblStatusesl;
        private System.Windows.Forms.ToolStripMenuItem tsmiプロフィール更新;
        private SplitButton btnURLShorten;
        private System.Windows.Forms.ContextMenuStrip menuShortenType;
        private System.Windows.Forms.ToolStripMenuItem tsmibit_ly;
        private System.Windows.Forms.ToolStripMenuItem tsmij_mp;
        private System.Windows.Forms.ToolStripMenuItem tsmiSpecifyTime;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearTweets;
        private System.Windows.Forms.ToolStripMenuItem tsmi_子画面;
        private System.Windows.Forms.ToolStripMenuItem tsmi子画面_nothing;
        private ImageListWrapper imageListWrapper;
        private StarlitTwit.RichTextBoxEx rtxtTwit;
        private System.Windows.Forms.ToolStripMenuItem tsmi自分のプロフィール;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tsmi画面設定;
        private System.Windows.Forms.ToolStripMenuItem tsmiタブの位置;
        private System.Windows.Forms.ToolStripComboBox tsComboTabAlignment;
        private System.Windows.Forms.ToolStripMenuItem tsmiAPIRestriction;
        private System.Windows.Forms.ToolStripMenuItem tsmiUserStream;
        private System.Windows.Forms.ToolStripMenuItem tsmiUserStreamStart;
        private System.Windows.Forms.ToolStripMenuItem tsmiUserStreamEnd;
        private System.Windows.Forms.ToolStripMenuItem tsmiUserStreamLog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.Label lblUserStreamInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmi自分のリスト;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem tsmiフォロー中のリスト;
        private System.Windows.Forms.LinkLabel llblList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem tsmiブロックユーザーリスト;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem tsmi自分のリツイート;
        private System.Windows.Forms.ToolStripMenuItem tsmiフォロワーのリツイート;
        private System.Windows.Forms.ToolStripMenuItem tsmi自分がされたリツイート;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem tsmi自分のお気に入り;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripSeparator tsSep子画面;
        private System.Windows.Forms.ToolStripMenuItem tsmi全小画面を消去;

    }
}

