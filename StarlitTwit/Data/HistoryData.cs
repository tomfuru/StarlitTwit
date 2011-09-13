using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarlitTwit
{
    public class HistoryData : SaveDataClassBase<HistoryData>
    {
        /// <summary>発言履歴</summary>
        public string[] Tweet = new string[0];
        /// <summary>フッタ履歴</summary>
        public string[] Footer = new string[0];
        /// <summary>ヘッダ履歴</summary>
        public string[] Header = new string[0];
    }
}
