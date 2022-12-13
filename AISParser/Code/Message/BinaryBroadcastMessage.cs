using AISParser.Code.Message.InternationalFunctionMessage;
using AISParser.Code.Parser.Interface;
using AISParser.Code.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class BinaryBroadcastMessage :AISMessageBase
    {
        #region Property
        public byte RepeateIndicator { get; set; }
        public uint SourceId { get; set; }
        public byte Spare { get; set; }
        public ushort DAC { get; set; }
        public byte FI { get; set; }
        public InternationalFunctionMessageBase InternationalFunctionMessage { get; set; }
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            MessageId           = BitParser<MyByte>.Parsing(message, 0, 6).val;
            RepeateIndicator    = BitParser<MyByte>.Parsing(message, 6, 2).val;
            SourceId            = BitParser<MyUint>.Parsing(message, 8, 30).val;
            Spare               = BitParser<MyByte>.Parsing(message, 38, 2).val;
            DAC                 = BitParser<MyUshort>.Parsing(message, 40, 10).val;
            FI                  = BitParser<MyByte>.Parsing(message, 50, 6).val;
            #region DEBUG
#if DebugMessage

            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Source ID                    : " + Convert.ToString(SourceId, 10));
            Console.WriteLine("RepeateIndicator             : " + Convert.ToString(RepeateIndicator));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare));
            Console.WriteLine("DAC                          : " + Convert.ToString(DAC));
            Console.WriteLine("FI                           : " + Convert.ToString(FI));
#endif
            #endregion
            if (DAC == 1)
            {
                if (FI == 0)
                {
                    InternationalFunctionMessage = new AddressBinaryMessage((int)MessageId);
                    InternationalFunctionMessage.Parsing(message);
                }
                
            }

        }
        #endregion
    }
}
