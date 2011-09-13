using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StarlitTwit
{
    public class HistoryListManager<T>
    {
        private List<T> _historyList = new List<T>();

        //-------------------------------------------------------------------------------
        #region KeepHistoryNum プロパティ：履歴保持数
        //-------------------------------------------------------------------------------
        private int _keepHistoryNum;
        /// <summary>
        /// 履歴保持数
        /// </summary>
        public int KeepHistoryNum
        {
            get { return _keepHistoryNum; }
            set
            {
                Debug.Assert(value > 0, "値は1以上でなければならない");
                _keepHistoryNum = value;
                OnKeepHistoryNumChanged();
            }
        }
        #endregion (KeepHistoryNum)

        //-------------------------------------------------------------------------------
        #region -OnKeepHistoryNumChanged 履歴保持数変更時
        //-------------------------------------------------------------------------------
        //
        private void OnKeepHistoryNumChanged()
        {
            if (_keepHistoryNum < _historyList.Count) {
                _historyList.RemoveRange(_keepHistoryNum, _historyList.Count - _keepHistoryNum);
            }
        }
        #endregion (OnKeepHistoryNumChanged)

        //-------------------------------------------------------------------------------
        #region +AddHistory 履歴追加
        //-------------------------------------------------------------------------------
        //
        public bool AddHistory(T value)
        {
            bool existValue;
            int index = _historyList.IndexOf(value);
            if (existValue = (index > -1)) { _historyList.RemoveAt(index); }
            else if (_historyList.Count == _keepHistoryNum) { _historyList.RemoveAt(_keepHistoryNum - 1); }

            _historyList.Insert(0, value);
            return existValue;
        }
        #endregion (AddHistory)
        //-------------------------------------------------------------------------------
        #region +GetHistories 履歴一覧取得
        //-------------------------------------------------------------------------------
        //
        public T[] GetHistories()
        {
            return _historyList.ToArray();
        }
        #endregion (GetHistories)
    }
}
