using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace StarlitTwit
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string settingPath = Utilization.GetDefaultSettingsDataFilePath();
            Mutex mutex = new Mutex(false, settingPath.Replace('\\','_'));
            if (!mutex.WaitOne(0, false)) {
                // 同じ設定ファイルを使って起動しているプロセスがある
                Message.ShowWarningMessage("既に同じ設定ファイルで起動しています。");
                return;
            }

            try {
                Config.UnhandleExceptionConfiguration();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
            catch (Exception) { throw; }
            finally { mutex.ReleaseMutex(); }
        }
    }
}
