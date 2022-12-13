using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.CommunicationState
{
    public class ITDMA
    {
        #region Property
        public byte SyncState { get; set; } = new byte();
        public ushort SlotIncrement { get; set; } = new ushort();
        public byte NumberOfSlots { get; set; } = new byte();
        public bool KeepFlag { get; set; } = new bool();
        #endregion
    }
}
