using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{

	sealed public class InterrogationMessage :AISMessageBase
    {
        #region Property
        public uint[] DestinationId { get; set; } = new uint[2];
        public byte[] Dest1MessageId { get; set; } = new byte[2];
        public ushort[] Dest1SlotOffset { get; set; } = new ushort[2];
        public byte Dest2MessageId { get; set; } 
        public ushort Dest2SlotOffset { get; set; }

        public byte[] Spare { get; set; } = new byte[4];
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            Spare[0]            = BitParser<MyByte>.Parsing(message, 38, 2).val;
            DestinationId[0]    = BitParser<MyUint>.Parsing(message, 40, 30).val;
            int startBit = 70;
            for(int i = 0; i < 2; i++)
            {
                startBit += 20 * i;
                Dest1MessageId[0]   = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                Dest1SlotOffset[0]  = BitParser<MyUshort>.Parsing(message, startBit+6, 12).val;
                Spare[1 + i]        = BitParser<MyByte>.Parsing(message, startBit+18, 2).val;
            }
            DestinationId[1]    = BitParser<MyUint>.Parsing(message, 110, 30).val;
            Dest2MessageId      = BitParser<MyByte>.Parsing(message, 140, 6).val;
            Dest2SlotOffset     = BitParser<MyUshort>.Parsing(message,146, 12).val;
            Spare[3]            = BitParser<MyByte>.Parsing(message, 158, 2).val;

            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator               : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("Soruce ID                    : " + Convert.ToString(UserId, 10));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[0], 10));
            Console.WriteLine("DestinationId 1              : " + Convert.ToString(DestinationId[0], 10));
            Console.WriteLine("Message ID 1.1               : " + Convert.ToString(Dest1MessageId[0], 10));
            Console.WriteLine("Slot Offset 1.1              : " + Convert.ToString(Dest1SlotOffset[0], 10));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[1], 10));
            Console.WriteLine("Message ID 1.2               : " + Convert.ToString(Dest1MessageId[1], 10));
            Console.WriteLine("Slot Offset 1.2              : " + Convert.ToString(Dest1SlotOffset[1], 10));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[2], 10));
            Console.WriteLine("DestinationId 2              : " + Convert.ToString(DestinationId[1], 10));
            Console.WriteLine("Message ID 2.1               : " + Convert.ToString(Dest2MessageId, 10));
            Console.WriteLine("Slot Offset 2.1              : " + Convert.ToString(Dest2SlotOffset, 10));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[3], 10));
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");

#endif
            #endregion
        }
        #endregion
    }
}
