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
        [DefaultValue(DEFAULT_SWITCHINTERVAL)]
        public int SwitchInterval { get; set; }
        /// <summary>永久でない時の最大表示時間(ミリ秒)</summary>
        [Category("動作")]
        [DefaultValue(DEFAULT_MAXDISPLAYTIME)]
        public int MaxDisplay { get; set; }
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
        private const int DEFAULT_SWITCHINTERVAL = 1500;
        private const int DEFAULT_MAXDISPLAYTIME = 5000;
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
            SwitchInterval = DEFAULT_SWITCHINTERVAL;
            MaxDisplay = DEFAULT_MAXDISPLAYTIME;
            base.Text = "";
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
                type = (times > 0) ? TextDataType.Some_Times : TextDataType.Permanent,
                restNum = times
            };

            lock (_objSync) {
                _firstQueue.Enqueue(text);
                if (_textDic.ContainsKey(text)) {
                    _textDic.Remove(text);
                }
                else { _textDic.Add(text, data); }
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
            try {
                string text;
                TextData textdata;
                while (true) {
                    lock (_objSync) {
                        if (_firstQueue.Count > 0) {
                            text = _firstQueue.Dequeue();
                        }
                        else if (_textQueue.Count > 0) {
                            text = _textQueue.Dequeue();
                        }
                        else { break; }                 // 要素もう無し，終了

                        if (!_textDic.ContainsKey(text)) { continue; }  // 削除済みの時
                        textdata = _textDic[text];
                    }

                    SetTextToLabel(text);

                    int standard = Environment.TickCount;
                    int now = standard;
                    while (true) {
                        if (_firstQueue.Count > 0) { break; }       // 初めての項目がきた場合は優先
                        Thread.Sleep(SLEEP_TIME);
                        now = Environment.TickCount;
                        if (now - standard >= SwitchInterval && _textQueue.Count > 0) { break; } // データがある場合はSwitchIntervalたったら抜ける
                        else if (textdata.type == TextDataType.Permanent) {    // Permanentの時にデータが消えてたら終わり
                            lock (_objSync) {
                                if (!_textDic.ContainsKey(text)) { break; }
                            }
                        }
                        else if (now - standard >= MaxDisplay) {               // Permanentでない時にMaxDisplay時間たったら終わり
                            textdata.restNum = 0;
                            break;
                        }
                    };

                    if (textdata.type == TextDataType.Permanent || --textdata.restNum >= 0) {
                        _textQueue.Enqueue(text);
                    }
                    else {
                        lock (_objSync) {
                            _textDic.SaveRemove(text);
                        }
                    }
                }
                SetTextToLabel("");
            }
            catch (NullReferenceException) { }
            catch (InvalidOperationException) { }
        }
        #endregion (SwitchText)

        //-------------------------------------------------------------------------------
        #region -SetTextToLabel テキストを設定
        //-------------------------------------------------------------------------------
        //
        private void SetTextToLabel(string text)
        {
            if (this.Parent.InvokeRequired) {
                this.Parent.Invoke(new Action(() => base.Text = text));
            }
            else { base.Text = text; }
        }
        #endregion (SetTextToLabel)
    }
}
