using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            //Application.Run(new Form1());


            TestClass c = new TestClass();
            c.testMain();
        }
    }

    class TestClass
    {
        public void testMain()
        {
            IEnumerable<int> enumerable = enum1();

            foreach (var item in enumerable) {
                Console.Write(item);
            }
        }

        private IEnumerable<int> enum1()
        {
            for (int i = 0; i < 30; i++)
			{
                yield return i;
			}
        }


    }
}
