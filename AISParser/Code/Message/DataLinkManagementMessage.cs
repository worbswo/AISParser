using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
	sealed public class DataLinkManagementMessage : AISMessageBase
    {
        #region Property
        public byte Spare { get; set; } 
        public ushort[] OffsetNumber { get; set; }=new ushort[4];
        public byte[] NumberOfSlot { get;set; }=new byte[4];
        public byte[] TimeOut { get; set; } = new byte[4];
        public ushort[] SlotNumber { get; set; }= new ushort[4];
        public byte Spare2 { get; set; }
        #endregion
        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            Repeatindicator = BitParser<MyByte>.Parsing(message, 6, 2).val;
            UserId          = BitParser<MyUint>.Parsing(message, 8, 30).val;
            Spare           = BitParser<MyByte>.Parsing(message,38,2).val;
            int startBit = 40;
            for(int i=0;i<4;i++)
            {
                OffsetNumber[i] = BitParser<MyUshort>.Parsing(message, startBit, 12).val;
                NumberOfSlot[i] = BitParser<MyByte>.Parsing(message, startBit, 4).val;
                TimeOut[i]      = BitParser<MyByte>.Parsing(message, startBit, 3).val;
                SlotNumber[i]   = BitParser<MyUshort>.Parsing(message, startBit, 11).val;
                startBit += 30;
            }
            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator               : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("Source station ID            : " + Convert.ToString(UserId, 10));
            for(int i = 0; i < 4; i++)
            {
                Console.WriteLine("Offsetnumber" + (i + 1).ToString() + "            : " + Convert.ToString(OffsetNumber[i]));
                Console.WriteLine("NumberOfSlot" + (i + 1).ToString() + "            : " + Convert.ToString(NumberOfSlot[i]));
                Console.WriteLine("TimeOut     " + (i + 1).ToString() + "            : " + Convert.ToString(TimeOut[i]));
                Console.WriteLine("SlotNumber  " + (i + 1).ToString() + "            : " + Convert.ToString(SlotNumber[i]));

            }
#endif
            #endregion
        }

        #endregion
    }
}
