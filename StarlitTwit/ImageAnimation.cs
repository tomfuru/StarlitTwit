using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;

namespace StarlitTwit
{
    /// <summary>
    /// 画像アニメーションを管理するクラスです。
    /// </summary>
    public class ImageAnimation : IDisposable
    {
        //-------------------------------------------------------------------------------
        #region Variables
        //-------------------------------------------------------------------------------
        /// <summary>現在のループカウント</summary>
        private int _nowLoopCount = 0;
        /// <summary>現在のフレームカウント</summary>
        private int _nowFrameCount = 0;
        /// <summary>アニメーション中か</summary>
        private bool _animating = false;
        /// <summary>タイマー</summary>
        private Timer _timer;

        //-------------------------------------------------------------------------------
        /// <summary>アニメーションするイメージ</summary>
        private readonly Image _image;
        /// <summary>フレームディメンション</summary>
        private readonly FrameDimension FrameDimension;
        /// <summary>ループ回数</summary>
        public readonly int MaxLoopCount;
        /// <summary>フレーム数</summary>
        public readonly int MaxFrameCount;
        /// <summary>フレームのディレイ(10ms単位)</summary>
        public readonly int[] FrameDelays;
        /// <summary>無限ループかどうか</summary>
        public readonly bool LoopInfinity;
        /// <summary>フレーム操作時にロックするオブジェクト</summary>
        private readonly object _lockFrame = new object();
        //-------------------------------------------------------------------------------
        #endregion (Variables)

        //-------------------------------------------------------------------------------
        #region Property
        //-------------------------------------------------------------------------------
        #region Image プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// アニメーションするイメージを取得します。
        /// </summary>
        public Image Image
        {
            get { return _image; }
        }
        #endregion (Image)
        //-------------------------------------------------------------------------------
        #region NowLoopCount プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 現在何回目のループか
        /// </summary>
        public int NowLoopCount
        {
            get { return _nowLoopCount + 1; }
        }
        #endregion (NowLoopCount)
        //-------------------------------------------------------------------------------
        #region NowFrameCount プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 次表示されるのは何フレーム目か
        /// </summary>
        public int NowFrameCount
        {
            get { return _nowFrameCount + 1; }
        }
        #endregion (NowFrameCount)
        //-------------------------------------------------------------------------------
        #region Animating プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// アニメーション中かどうか
        /// </summary>
        public bool Animating
        {
            get { return _animating; }
        }
        #endregion (Animating)
        //-------------------------------------------------------------------------------
        #endregion (Property)

        //-------------------------------------------------------------------------------
        #region public events
        //-------------------------------------------------------------------------------
        public event EventHandler FrameUpdated;
        //-------------------------------------------------------------------------------
        #endregion (public events)

        //-------------------------------------------------------------------------------
        #region Constants
        //-------------------------------------------------------------------------------
        const int FRAME_DELAY = 0x5100;
        const int FRAME_NUM = 0x5101;
        //-------------------------------------------------------------------------------
        #endregion (Constants)

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public ImageAnimation(Image img)
        {
            _image = img;
            FrameDimension = new FrameDimension(img.FrameDimensionsList[0]);
            MaxFrameCount = img.GetFrameCount(FrameDimension);
            PropertyItem pItemFrameDelay = img.GetPropertyItem(FRAME_DELAY);
            PropertyItem pItemFrameNum = img.GetPropertyItem(FRAME_NUM);
            FrameDelays = new int[MaxFrameCount];

            for (int i = 0; i < MaxFrameCount; i++) {
                FrameDelays[i] = BitConverter.ToInt32(pItemFrameDelay.Value, 4 * i);
            }
            MaxLoopCount = BitConverter.ToInt16(pItemFrameNum.Value, 0);

            LoopInfinity = (MaxLoopCount == 0);

            _timer = new Timer(Timer_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
            try {
                _image.SelectActiveFrame(FrameDimension, 0);
            }
            catch (InvalidOperationException/* ex*/) {
                //Log.DebugLog(ex);
                //Debug.Assert(false, "Image.SelectActiveFrame失敗");
            }
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region Timer_Elapsed タイマーの時間経過時イベント
        //-------------------------------------------------------------------------------
        //
        private void Timer_Elapsed(object o)
        {
            NextFrame();
        }
        #endregion (Timer_Elapsed)

        //-------------------------------------------------------------------------------
        #region +StartAnimation アニメーションスタート
        //-------------------------------------------------------------------------------
        /// <summary>
        /// アニメーションをスタートします。
        /// </summary>
        public void StartAnimation()
        {
            if (_animating) { return; }

            lock (_lockFrame) {
                _timer.Change(FrameDelays[0] * 10, Timeout.Infinite);
                _animating = true;
            }
        }
        #endregion (StartAnimation)
        //-------------------------------------------------------------------------------
        #region +StopAnimation アニメーションストップ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// アニメーションをストップします。
        /// </summary>
        public void StopAnimation()
        {
            if (!_animating) { return; }

            lock (_lockFrame) {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _animating = false;
            }
        }
        #endregion (StopAnimation)
        //-------------------------------------------------------------------------------
        #region +ResetAnimation アニメーションリセット
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リセットして最初からアニメーションを再生します。
        /// </summary>
        public void ResetAnimation()
        {
            lock (_lockFrame) {
                if (_animating) { _timer.Change(FrameDelays[0] * 10, Timeout.Infinite); }
                try {
                    _image.SelectActiveFrame(FrameDimension, 0);
                }
                catch (InvalidOperationException ex) {
                    Log.DebugLog(ex);
                    Debug.Assert(false, "Image.SelectActiveFrame失敗");
                }

                _nowFrameCount = _nowLoopCount = 0;

                if (FrameUpdated != null) { FrameUpdated(this, EventArgs.Empty); }
            }
        }
        #endregion (ResetAnimation)

        //-------------------------------------------------------------------------------
        #region -NextFrame 次のフレームへ
        //-------------------------------------------------------------------------------
        //
        private void NextFrame()
        {
            lock (_lockFrame) {
                // 終わりか判別
                if (!_animating || (!LoopInfinity && NowLoopCount == MaxLoopCount && NowFrameCount == MaxFrameCount)) {
                    StopAnimation();
                }

                // フレームを進める
                if (NowFrameCount == MaxFrameCount) {
                    _nowFrameCount = 0;
                    unchecked { _nowLoopCount++; }
                }
                else { _nowFrameCount++; }

                _timer.Change(FrameDelays[_nowFrameCount] * 10, Timeout.Infinite);
                try {
                    _image.SelectActiveFrame(FrameDimension, _nowFrameCount);
                }
                catch (InvalidOperationException/* ex*/) {
                    //Log.DebugLog(ex);
                    //Debug.Assert(false, "Image.SelectActiveFrame失敗");
                    return;
                }
            }
            if (FrameUpdated != null) { FrameUpdated(this, EventArgs.Empty); }
        }
        #endregion (NextFrame)

        //-------------------------------------------------------------------------------
        #region IDisposable.Dispose 破棄
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リソースを破棄します。イメージは破棄しません。
        /// </summary>
        public void Dispose()
        {
            lock (_lockFrame) { _timer.Dispose(); }
        }
        #endregion (IDisposable.Dispose)
        //-------------------------------------------------------------------------------
        #region +Dispose 破棄
        //-------------------------------------------------------------------------------
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        /// <param name="disposeImage">画像を破棄する場合はtrue,しない場合はfalse</param>
        public void Dispose(bool disposeImage)
        {
            lock (_lockFrame) {
                if (disposeImage) { _image.Dispose(); }
                Dispose();
            }
        }
        #endregion (Dispose)
    }
}
