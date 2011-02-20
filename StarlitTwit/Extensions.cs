using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    public static class Extensions
    {
        //-------------------------------------------------------------------------------
        #region +[extension]WriteLineConsole コンソールに値を書き込み
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このオブジェクトの値をコンソールに書き込みます。
        /// </summary>
        /// <param name="obj">このオブジェクト</param>
        /// <param name="preStr">オブジェクトの値の前に書く文字列</param>
        /// <param name="aftStr">オブジェクトの値の後に書く文字列</param>
        public static void WriteLineConsole(this object obj, string preStr = "", string aftStr = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preStr);
            sb.Append(obj);
            sb.Append(aftStr);
            Console.WriteLine(sb.ToString());
        }
        /// <summary>
        /// このオブジェクトの値をコンソールに書き込みます。
        /// </summary>
        /// <param name="obj">このオブジェクト</param>
        public static void WriteLineConsole(this object obj)
        {
            Console.WriteLine(obj);
        }
        //-------------------------------------------------------------------------------
        #endregion (WriteLineConsole)
        //-------------------------------------------------------------------------------
        #region +[extension]WriteConsole コンソールに値書き込み
        //-------------------------------------------------------------------------------
        /// <summary>
        /// このオブジェクトの値をコンソールに書き込みます。
        /// </summary>
        /// <param name="obj">このオブジェクト</param>
        /// <param name="preStr">オブジェクトの値の前に書く文字列</param>
        /// <param name="aftStr">オブジェクトの値の後に書く文字列</param>
        public static void WriteConsole(this object obj, string preStr = "", string aftStr = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preStr);
            sb.Append(obj);
            sb.Append(aftStr);
            Console.Write(sb.ToString());
        }
        /// <summary>
        /// このオブジェクトの値をコンソールに書き込みます。
        /// </summary>
        /// <param name="obj">このオブジェクト</param>
        public static void WriteConsole(this object obj)
        {
            Console.Write(obj);
        }
        //-------------------------------------------------------------------------------
        #endregion (WriteConsole)
        //-------------------------------------------------------------------------------
        #region +[extension]ForEach 各項目について処理
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 各項目について処理を行います。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="act"></param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> act)
        {
            foreach (T item in collection) { act(item); }
        }
        #endregion (ForEach<T>)
    }
}
