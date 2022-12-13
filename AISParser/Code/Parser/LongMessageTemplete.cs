using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Parser
{
    /// <summary>
    /// 연속된 AIS 메세지를 저장하기 위한 클래스
    /// </summary>
    public class LongMessageTemplete
    {
        public string Identify { get; set; }
        public byte[] SixBitMessages { get; set; }
        public int SubMessageId { get; set; }
        public List<byte[]> SixBitMessage { get; set; } = new List<byte[]>();
        public List<int> SixBitMessageCount { get; set; } = new List<int>();
        public bool[] isMessagesChecked { get; set; }

 
    }
}
