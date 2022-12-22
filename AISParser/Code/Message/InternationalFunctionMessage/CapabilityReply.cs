using AISParser.Code.Parser.Interface;
using AISParser.Code.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.InternationalFunctionMessage
{
    sealed public class CapabilityReply : InternationalFunctionMessageBase
    {
        #region Property
        public ushort DACCode { get; set; }
        #endregion
        #region Method
        public  void Parsing(byte[] message)
        {
            DACCode = BitParser<MyUshort>.Parsing(message, 88, 10).val;

            #region DEBUG
#if DebugMessage

            Console.WriteLine("DACCode                      : " + Convert.ToString(DACCode));
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");

#endif
            #endregion
        }
        #endregion
    }
}
