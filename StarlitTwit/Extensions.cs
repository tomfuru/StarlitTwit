using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public static class Extensions
    {
        //-------------------------------------------------------------------------------
        #region +[extension]object.WriteLineConsole コンソールに値を書き込み
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
        #region +[extension]object.WriteConsole コンソールに値書き込み
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
        #region +[extension]IEnumerable<T>.ForEach 各項目について処理
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

        //-------------------------------------------------------------------------------
        #region +[extension]Dictionary<K,V>.SafeRemove 項目がある場合のみ消去
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 項目がある場合のみ消去を行います。
        /// </summary>
        /// <typeparam name="TKey">キータイプ</typeparam>
        /// <typeparam name="TValue">値タイプ</typeparam>
        /// <param name="dic">this</param>
        /// <param name="keyitem">削除する項目</param>
        /// <returns></returns>
        public static bool SafeRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey keyitem)
        {
            if (dic.ContainsKey(keyitem)) {
                dic.Remove(keyitem);
                return true;
            }
            return false;
        }
        #endregion (SaveRemove)

        //-------------------------------------------------------------------------------
        #region +[extention]ComboBox.ObjectCollection.CombAddAvoidDup 重複を避けて追加します。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 重複を避けて項目を追加します。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item">追加する項目</param>
        public static void AddAvoidDup(this ComboBox.ObjectCollection collection, object item)
        {
            if (item != null && !collection.Contains(item)) { collection.Add(item); }
        }
        #endregion (AddAvoidDup)

        //-------------------------------------------------------------------------------
        #region +[extension]object.AsEnumerable 単数オブジェクトをIEnumerableとして扱います。
        //-------------------------------------------------------------------------------
        /// <summary>
        /// オブジェクトを単一オブジェクトのみを返すIEnumerableに変換します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this T obj)
        {
            yield return obj;
        }
        #endregion (object.AsEnumerable)
    }

    //-------------------------------------------------------------------------------
    #region (interface)IDeepCopyClonable
    //-------------------------------------------------------------------------------
    public interface IDeepCopyClonable<T>
    {
        /// <summary>
        /// deep copyによりCloneを作成します。
        /// </summary>
        /// <returns></returns>
        T DeepCopyClone();
    }
    #endregion ((interface)IDeepCopyClonable)
}
