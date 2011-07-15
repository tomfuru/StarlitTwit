using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StarlitTwit
{
    /// <summary>履歴管理クラス</summary>
    public class HistoryManager<T>
    {
        //-------------------------------------------------------------------------------
        #region Variable
        //-------------------------------------------------------------------------------
        //private int _current = -1;
        //private List<T> _historyList = new List<T>();
        /// <summary>履歴リスト．インデックスが大きいほど古い</summary>
        private LinkedList<T> _historyList = new LinkedList<T>();
        /// <summary>現在位置ノード</summary>
        private LinkedListNode<T> _currentNode = null;
        //-------------------------------------------------------------------------------
        #endregion (Variable)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public HistoryManager(T firstValue, int keepNum = 5)
        {
            _keepHistoryNum = keepNum;

            AddHistory(firstValue);
        }
        #endregion (Constructor)

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
            set {
                Debug.Assert(value > 0, "値は1以上でなければならない");
                _keepHistoryNum = value + 1;
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
            //for (int i = _historyList.Count - 1; i >= _keepHistoryNum; i--) {
            //    _historyList.RemoveAt(i);
            //}

            for (int i = _historyList.Count - 1; i >= _keepHistoryNum; i--) {
                _historyList.RemoveLast();
            }
        }
        #endregion (OnKeepHistoryNumChanged)

        //-------------------------------------------------------------------------------
        #region +CanUndo Undo可能かどうか
        //-------------------------------------------------------------------------------
        //
        public bool CanUndo()
        {
            //return _current < _historyList.Count - 1;
            return _currentNode.Next != null;
        }
        #endregion (CanUndo)
        //-------------------------------------------------------------------------------
        #region +CanRedo Redo可能かどうか
        //-------------------------------------------------------------------------------
        //
        public bool CanRedo()
        {
            //return _current > 0;
            return _currentNode.Previous != null;
        }
        #endregion (CanRedo)

        //-------------------------------------------------------------------------------
        #region +AddHistory 履歴追加
        //-------------------------------------------------------------------------------
        //
        public void AddHistory(T value)
        {
            //for (int i = _current - 1; i >= 0; i--) {
            //    _historyList.RemoveAt(i);
            //}
            //_current = 0;
            //_historyList.Insert(0, value);
            //if (_historyList.Count > _keepHistoryNum) {
            //    _historyList.RemoveAt(_historyList.Count - 1);
            //}

            var node = _currentNode;
            while (_historyList.First != _currentNode) {
                _historyList.RemoveFirst();
            }
            _historyList.AddFirst(value);
            _currentNode = _historyList.First;
            if (_historyList.Count > _keepHistoryNum) {
                _historyList.RemoveLast();
            }
        }
        #endregion (AddHistory)

        //-------------------------------------------------------------------------------
        #region +Undo Undo処理
        //-------------------------------------------------------------------------------
        //
        public T Undo()
        {
            Debug.Assert(CanUndo(), "Undo不可");

            //return _historyList[++_current];
            _currentNode = _currentNode.Next;
            return _currentNode.Value;
        }
        #endregion (Undo)
        //-------------------------------------------------------------------------------
        #region +Redo Redo処理
        //-------------------------------------------------------------------------------
        //
        public T Redo()
        {
            Debug.Assert(CanRedo(), "Redo不可");

            //return _historyList[--_current];
            _currentNode = _currentNode.Previous;
            return _currentNode.Value;
        }
        #endregion (Redo)
    }
}
