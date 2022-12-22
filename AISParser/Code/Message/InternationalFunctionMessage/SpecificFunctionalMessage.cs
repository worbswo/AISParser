using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.InternationalFunctionMessage
{
	sealed public class SpecificFunctionalMessage : InternationalFunctionMessageBase
    {
        #region Property
        public ushort RequestedDACCode { get; set; }
        public byte RequestedFICode { get; set; }
        #endregion
        #region Method
        public void Parsing(byte[] message)
        {
            RequestedDACCode = BitParser<MyUshort>.Parsing(message, 88, 10).val;
            RequestedFICode = BitParser<MyByte>.Parsing(message, 98, 6).val;

            #region DEBUG
#if DebugMessage
            Console.WriteLine("RequestedFICode              : " + Convert.ToString(RequestedFICode));
            Console.WriteLine("RequestedDACCode             : " + Convert.ToString(RequestedDACCode));
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");

#endif
            #endregion
        }
        #endregion
    }
}
