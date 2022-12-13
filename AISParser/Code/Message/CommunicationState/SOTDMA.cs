using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.CommunicationState
{
    public class SOTDMA
    {
        public byte SyncState { get; set; } = new byte();
        public byte SlotTimeout { get; set; } = new byte();
        public ushort Message { get; set; } = new ushort();
        public byte UTCHour { get; set; } = new byte();
        public byte UTCmin { get; set; } = new byte();
    }
}
