using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace StarlitTwit
{
    /// <summary>複数項目の表示ができるToolStripStatusLabelです。</summary>
    /// <remarks>
    /// ・Permanentな項目のみの時
    ///  - Some Timesな項目が来たら，すぐ切り替える
    /// その他は全てIntervalを待って切り替える
    /// </remarks>
    public class ToolStripStatusLabelEx : ToolStripStatusLabel
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>複数ラベルがある時の切り替え間隔(ミリ秒)</summary>
        [Category("動作")]
        [DefaultValue(1000)]
        public int SwitchInterval { get; set; }
        /// <summary>最初のキュー</summary>
        private Queue<string> _firstQueue = new Queue<string>();
        /// <summary>テキストデータリスト</summary>
        private Queue<string> _textQueue = new Queue<string>();
        /// <summary>テキスト辞書</summary>
        private Dictionary<string, TextData> _textDic = new Dictionary<string, TextData>();
        /// <summary>別スレッド</summary>
        private Thread _thread = null;
        /// <summary>同期用オブジェクト</summary>
        private object _objSync = new object();
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------
        [Browsable(false)]
        public new string Text
        {
            get { return base.Text; }
        }
        //-------------------------------------------------------------------------------
        #endregion (Properties)

        //-------------------------------------------------------------------------------
        #region Constants
        //-------------------------------------------------------------------------------
        private const int SLEEP_TIME = 20;
        //-------------------------------------------------------------------------------
        #endregion (Constants)

        //-------------------------------------------------------------------------------
        #region TextData 構造体：登録テキストデータ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        private class TextData
        {
            /// <summary>種類</summary>
            public TextDataType type;
            /// <summary>残り回数</summary>
            public int restNum;
        }
        //-------------------------------------------------------------------------------
        #endregion (TextData)
        //-------------------------------------------------------------------------------
        #region TextDataType 列挙体：登録テキストデータタイプ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 登録されたテキストのデータ種類です。
        /// </summary>
        private enum TextDataType
        {
            /// <summary>永久</summary>
            Permanent,
            /// <summary>何度か</summary>
            Some_Times
        }
        //-------------------------------------------------------------------------------
        #endregion (TextDataType)

        //-------------------------------------------------------------------------------
        #region コンストラクタ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// ToolStripStatusLabelExのインスタンスを初期化します。
        /// </summary>
        public ToolStripStatusLabelEx()
            : base()
        {
            SwitchInterval = 1000;
        }
        #endregion (コンストラクタ)

        //-------------------------------------------------------------------------------
        #region +SetText ラベルをセット
        //-------------------------------------------------------------------------------
        #region (string)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストをセットします。Removeされるまでラベルは表示され続けます。
        /// </summary>
        /// <param name="text">テキスト</param>
        public void SetText(string text)
        {
            SetText(text, 0);
        }
        //-------------------------------------------------------------------------------
        #endregion ((string))
        #region (string, int)
        //-------------------------------------------------------------------------------
        /// <summary>
        /// テキストをセットします。指定回数表示された後は表示されません。
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="times">表示回数, 0以下を指定すると無限</param>
        public void SetText(string text, int times)
        {
            TextData data = new TextData() {
                type = TextDataType.Permanent,
                restNum = times
            };

            lock (_objSync) {
                _firstQueue.Enqueue(text);
                _textDic.Add(text, data);
                StartThreadIfNotActive();
            }
        }
        //-------------------------------------------------------------------------------
        #endregion ((string, int))
        #endregion (SetText)
        //-------------------------------------------------------------------------------
        #region +RemoveText ラベルを消去
        //-------------------------------------------------------------------------------
        /// <summary>
        /// セットされたテキストを消去します。
        /// </summary>
        /// <param name="text"></param>
        public void RemoveText(string text)
        {
            lock (_objSync) {
                _textDic.SaveRemove(text);
            }
        }
        #endregion (RemoveText)

        //-------------------------------------------------------------------------------
        #region -StartThreadIfNotActive Threadが動いていない時，スレッドをスタートさせます
        //-------------------------------------------------------------------------------
        //
        private void StartThreadIfNotActive()
        {
            if (_thread == null || !_thread.IsAlive) {
                _thread = new Thread(SwitchText);
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
        #endregion (StartThreadIfNotActive)

        //-------------------------------------------------------------------------------
        #region -SwitchText テキストのスイッチングを行う（別スレッド）
        //-------------------------------------------------------------------------------
        //
        private void SwitchText()
        {
            string text;
            TextData textdata;
            while (true) {
                bool isFirst;
                lock (_objSync) {
                    if (_firstQueue.Count > 0) {
                        isFirst = true;
                        text = _firstQueue.Dequeue();
                    }
                    else if (_textQueue.Count > 0) {
                        isFirst = false;
                        text = _textQueue.Dequeue();
                    }
                    else {                                          // 要素もう無し，終了
                        base.Text = "";
                        return;
                    }

                    if (!_textDic.ContainsKey(text)) { continue; }  // 削除済みの時
                    textdata = _textDic[text];
                }

                base.Text = text;

                int standard = Environment.TickCount;
                int now = standard;
                do {
                    if (!isFirst && _firstQueue.Count > 0) { break; } // 初めての項目がきた場合は優先
                    Thread.Sleep(SLEEP_TIME);
                    now = Environment.TickCount;
                } while (now - standard < SwitchInterval);

                if (textdata.type == TextDataType.Permanent || --textdata.restNum >= 0) {
                    _textQueue.Enqueue(text);
                }
                else {
                    lock (_objSync) {
                        _textDic.SaveRemove(text);
                    }
                }
            }
        }
        #endregion (SwitchText)

    }
}
