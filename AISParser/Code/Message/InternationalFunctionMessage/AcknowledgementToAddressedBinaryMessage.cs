using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.InternationalFunctionMessage
{
    public class AcknowledgementToAddressedBinaryMessage : InternationalFunctionMessageBase
    {
        #region Property
        public ushort DACCodeOfReceivedFunctianlaMessage { get; set; }
        public byte FICodeOfReceivedFunctianlaMessage { get; set; }
        public ushort TextSequenceNumber { get; set; }
        public bool AIResponse { get; set; }

        #endregion

        #region Method
        public  void Parsing(byte[] message)
        {
            DACCodeOfReceivedFunctianlaMessage  = BitParser<MyUshort>.Parsing(message,88,10).val;
            FICodeOfReceivedFunctianlaMessage   = BitParser<MyByte>.Parsing(message, 98, 6).val;
            TextSequenceNumber                  = BitParser<MyUshort>.Parsing(message,104,11).val;
            AIResponse                          = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 114, 1).val);
#if DebugMessage
            Console.WriteLine("DACCode                      : " + Convert.ToString(DACCodeOfReceivedFunctianlaMessage));
            Console.WriteLine("FICode                       : " + Convert.ToString(FICodeOfReceivedFunctianlaMessage));
            Console.WriteLine("TextSequenceNumber           : " + Convert.ToString(TextSequenceNumber));
            Console.WriteLine("AIResponse                   : " + Convert.ToString(AIResponse));

            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
        }
        #endregion
    }
}
