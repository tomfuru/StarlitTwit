using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace StarlitTwit
{
    //-------------------------------------------------------------------------------
    #region Config 設定に関するクラス
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 静的設定関連のメソッドを提供します．
    /// </summary>
    public static class Config
    {
        #region 補足されない例外の処理関連
        private const string UNCATCHED_EXCEPTION = "予想外の例外が発生しました\n";
        private static bool _bIsConfiged = false;
        #region +UnhandleExceptionConfiguration 補足されない例外のキャッチ設定
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 補足されない例外を拾いメッセージを表示するように設定します．1回しか呼び出しても効果がありません．
        /// </summary>
        public static void UnhandleExceptionConfiguration()
        {
            if (!_bIsConfiged) {
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Application_UnhandledException);
                _bIsConfiged = true;
            }
        }
        #endregion (UnhandleExceptionConfiguration)
        //-------------------------------------------------------------------------------
        #region -Application_UnhandledException
        //-------------------------------------------------------------------------------
        //
        private static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (ex != null) {
                Log.DebugLog(ex);
                Message.ShowWarningMessage(UNCATCHED_EXCEPTION, ex.Message);
            }
        }
        #endregion (Application_UnhandledException)
        #region -Application_ThreadException
        //-------------------------------------------------------------------------------
        //
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Log.DebugLog(ex);
            Message.ShowWarningMessage(UNCATCHED_EXCEPTION, ex.Message);
        }
        #endregion (Application_ThreadException)
        #endregion (補足されない例外の処理)
    }
    #endregion (Config)

    //-------------------------------------------------------------------------------
    #region (Class)Log ログ出力関連
    //-------------------------------------------------------------------------------
    /// <summary>
    /// ログ出力関連のメソッドを提供します．
    /// </summary>
    public static class Log
    {
        private const string LOG_DEFAULT_DIR_NAME = "Log";
        private delegate string MakeLogMessageDelegate(string dateString, string separator, string mainMessage);
        //-------------------------------------------------------------------------------
        #region +DebugLog デバッグ用ログ出力
        //-------------------------------------------------------------------------------
        private const string DEBUG_LOG = "Debug";
        #region 文字出力/ディレクトリ指定有(本体)
        /// <summary>
        /// デバッグログに出力を行います．
        /// </summary>
        /// <param name="message">出力する文字列</param>
        /// <param name="outDirPath">出力フォルダのパス</param>
        public static void DebugLog(string message, string outDirPath)
        {
            OutputLog(message, outDirPath, DEBUG_LOG, null);
        }
        #endregion (文字出力/ディレクトリ指定無)
        #region 例外出力/ディレクトリ指定有
        /// <summary>
        /// デバッグログに指定例外情報の出力を行います．
        /// </summary>
        /// <param name="ex">出力する例外</param>
        /// <param name="outDirPath">出力フォルダのパス</param>
        public static void DebugLog(Exception ex, string outDirPath)
        {
            DebugLog(MakeExceptionMessage(ex), outDirPath);
        }
        #endregion (DebugLog)
        #region 文字出力/ディレクトリ指定無
        /// <summary>
        /// デバッグログに出力を行います．出力先フォルダは実行ファイルディレクトリ内のLogフォルダです．
        /// </summary>
        /// <param name="message">出力する文字列</param>
        public static void DebugLog(string message)
        {
            string sExePath = Application.StartupPath;  // 実行ファイルパス
            DebugLog(message, Path.Combine(sExePath, LOG_DEFAULT_DIR_NAME));
        }
        #endregion (文字出力/ディレクトリ指定無)
        #region 例外出力/ディレクトリ指定無
        /// <summary>
        /// デバッグログに指定例外情報の出力を行います．出力先フォルダは実行ファイルディレクトリ内のLogフォルダです．
        /// </summary>
        /// <param name="ex">出力する例外</param>
        public static void DebugLog(Exception ex)
        {
            DebugLog(MakeExceptionMessage(ex));
        }
        #endregion (例外出力/ディレクトリ指定無)
        #endregion (DebugLog)
        //-------------------------------------------------------------------------------
        #region +OpeLog 実行作業記録用ログを出力
        //-------------------------------------------------------------------------------
        private const string OPERATION_LOG = "Operation";
        #region ディレクトリ指定有
        /// <summary>
        /// オペレーションログを出力します。
        /// </summary>
        /// <param name="message">出力する文字列</param>
        /// <param name="outDirPath">出力フォルダのパス</param>
        public static void OpeLog(string message, string outDirPath)
        {
            // メッセージ部分指定デリゲート
            MakeLogMessageDelegate mlmDelegate = new MakeLogMessageDelegate(
                delegate(string dateString, string separator, string mainMessage) {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dateString);
                    sb.Append(" : ");
                    sb.Append(message);

                    return sb.ToString();
                }
            );
            OutputLog(message, outDirPath, OPERATION_LOG, mlmDelegate);
        }
        #endregion (ディレクトリ指定有)
        #region ディレクトリ指定無
        /// <summary>
        /// オペレーションログを出力します。
        /// </summary>
        /// <param name="message">出力する文字列</param>
        public static void OpeLog(string message)
        {
            OpeLog(message, LOG_DEFAULT_DIR_NAME);
        }
        #endregion (ディレクトリ指定無)
        #endregion (OpeLog)

        //===============================================================================
        #region -MakeExceptionMessage 例外メッセージを説明する文字列を作成
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 例外メッセージを説明する文字列を作成します
        /// </summary>
        /// <param name="ex">文字列作成対象の例外</param>
        /// <returns>例外説明文字列</returns>
        private static string MakeExceptionMessage(Exception ex)
        {
            const char SPACE = ' ';
            StringBuilder sb = new StringBuilder();
            // 例外名
            sb.Append("例外名：");
            sb.Append(ex.ToString());
            // 例外説明
            sb.Append(SPACE);
            sb.AppendLine(ex.Message);
            // 場所説明
            sb.Append("発生場所");
            sb.Append(ex.Source);
            sb.Append(SPACE);
            sb.AppendFormat(ex.TargetSite.Name);
            // スタックトレース
            sb.Append("スタックトレース：");
            sb.Append(ex.StackTrace);

            return sb.ToString();
        }
        #endregion (MakeExceptionMessage)
        //-------------------------------------------------------------------------------
        #region -OutputLog ログ出力本体
        //-------------------------------------------------------------------------------
        /// <summary>ログ出力本体部分</summary>
        /// <param name="message">出力内容</param>
        /// <param name="outDirPath">ログ出力先ディレクトリパス</param>
        /// <param name="sLogFileName">(null可)ログファイル名先頭部分</param>
        /// <param name="mlmDelegate">(null可)出力を指定するデリゲート</param>
        private static void OutputLog(string message, string outDirPath, string sLogFileName, MakeLogMessageDelegate mlmDelegate)
        {
            const string LOG_EXTENSTION = ".log";
            const string FILENAME_DATETIME_FORMAT = "yyyyMMdd";
            const string INNER_DATETIME_FORMAT = "HH:mm:ss.ffff";
            const string SEPARATOR = "---------------------------------------------------------------------";
            const char SEPARATE_CHAR = '_';

            // ディレクトリなければ作成
            if (!Directory.Exists(outDirPath)) {
                Directory.CreateDirectory(outDirPath);
            }

            // 出力ファイル名生成
            DateTime dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(sLogFileName)) {       // ログファイル名先頭部分
                sb.Append(sLogFileName);
                sb.Append(SEPARATE_CHAR);
            }
            sb.Append(dt.ToString(FILENAME_DATETIME_FORMAT));
            sb.Append(LOG_EXTENSTION);
            string fileName = sb.ToString();

            // 時間情報
            string preMessage = dt.ToString(INNER_DATETIME_FORMAT);
            // 出力
            using (FileStream fs = new FileStream(Path.Combine(outDirPath, fileName), FileMode.Append, FileAccess.Write)) {
                using (StreamWriter sw = new StreamWriter(fs)) {
                    if (mlmDelegate != null) {
                        sw.WriteLine(mlmDelegate(preMessage, SEPARATOR, message));
                    }
                    else {
                        sw.WriteLine(preMessage + SEPARATOR);
                        sw.WriteLine(message);
                    }
                }
            }
        }
        #endregion (OutputLog)
    }
    #endregion (Log)

    //-------------------------------------------------------------------------------
    #region Message メッセージ表示関連
    //-------------------------------------------------------------------------------
    /// <summary>
    /// メッセージ表示関連のメソッドを提供します．
    /// </summary>
    public static class Message
    {
        private const string TITLE = "StarlitTwit";
        #region +ShowInfoMessage 通知メッセージを表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 通知メッセージを表示します．
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        public static void ShowInfoMessage(params string[] message)
        {
            MessageBox.Show(CombineStrings(message), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion (ShowInfoMessage)
        #region +ShowWarningMessage 警告メッセージ表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 警告メッセージを表示します．
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        public static void ShowWarningMessage(params string[] message)
        {
            MessageBox.Show(CombineStrings(message), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion (ShowWarningMessage)
        #region +ShowQuestionMessage 質問メッセージ表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 質問メッセージを表示します．
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        /// <returns>DialogResultのYesかNo</returns>
        public static DialogResult ShowQuestionMessage(params string[] message)
        {
            return MessageBox.Show(CombineStrings(message), TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        #endregion (ShowQuestionMessage)
        #region +ShowErrorMessage エラーメッセージ表示
        //-------------------------------------------------------------------------------
        /// <summary>
        /// エラーメッセージを表示します．
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        public static void ShowErrorMessage(params string[] message)
        {
            MessageBox.Show(CombineStrings(message), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion (ShowErrorMessage)
        //-------------------------------------------------------------------------------
        #region -CombineStrings 文字列を改行を区切り記号として結合
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 文字列を改行を区切り記号として結合します．
        /// </summary>
        /// <param name="messages">結合する文字列配列</param>
        /// <returns>結合した文字</returns>
        private static string CombineStrings(string[] messages)
        {
            if (messages.Length > 0) {
                return String.Join(Environment.NewLine, messages);
            }
            else {
                return string.Empty;
            }
        }
        #endregion (CombineStrings)
    }
    #endregion (Message)
}
