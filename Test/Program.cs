using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace Test
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new FormFlowTest());
            //test.t();
            //Application.Run(new Form1());
            Application.Run(new Form2());

            //new TestClass().testMain();
        }
    }

    static class TTTest
    {
        public static void t()
        {
            string str = "";

            using (StringReader reader = new StringReader(str)) {
                XElement el = XElement.Load(reader);
                Type type = typeof(StarlitTwit.Twitter);
                //type.InvokeMember("ConvertToUserProfile", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, StarlitTwit.FrmMain.Twitter, new object[] { el });
                //MethodInfo[] info = type.GetMethods(BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                MethodInfo minfo = type.GetMethod("ConvertToUserProfile", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                minfo.Invoke(new StarlitTwit.Twitter(), new object[] { el });
            }
        }
    }

    class TestClass
    {
        public void testMain()
        {
            //TaskCompletionSource<Tuple<IEnumerable<TwitData>>> tcs2 = new TaskCompletionSource<Tuple<IEnumerable<TwitData>>>();
            //tcs2.
            //TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            //t = Task.Factory.StartNew(() => GetUserTweets(UserScreenName, -1), cts.Token);
            //var t2 = Task<Tuple<IEnumerable<TwitData>, string>>.Factory.StartNew(null);

            //-------------------------------------------------------------------------------

            //WindowsFormsSynchronizationContext.Current;
            SynchronizationContext sc = SynchronizationContext.Current;


            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.Task.Start();

            //tcs.SetException(

            CancellationTokenSource cts = new CancellationTokenSource();
            //cts.Token.


            Task t = Task.Factory.StartNew(() => enum1());






            //-------------------------------------------------------------------------------
            IEnumerable<int> enumerable = enum1();

            foreach (var item in enumerable) {
                Console.Write(item);
            }
        }

        private IEnumerable<int> enum1()
        {
            for (int i = 0; i < 30; i++) {
                yield return i;
            }
        }


    }
}
