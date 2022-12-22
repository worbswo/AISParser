using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.InternationalFunctionMessage
{
	sealed public class AddressBinaryMessage : InternationalFunctionMessageBase
    {
        #region Property
        public int MessageId { get; set; }
        public bool AcknowledgeRequriedFlag { get; set; }
        public ushort TextSequenceNumber { get; set; }
        public byte[] TextString { get; set; }
        #endregion

        #region Concstructor
        public AddressBinaryMessage(int messageId)
        {
            MessageId= messageId;
        }
        #endregion

        #region Method

        public void Parsing(byte[] message)
        {

            if (MessageId == 6)
            {
                AcknowledgeRequriedFlag = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 88, 1).val);
                TextSequenceNumber = BitParser<MyUshort>.Parsing(message, 89, 11).val;
                TextString = new byte[151];
                for (int i = 0; i < 151; i++)
                {
                    int startBit = 100 + (i * 6);
                    TextString[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                }
            }
            else if (MessageId == 8)
            {
                AcknowledgeRequriedFlag = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 56, 1).val);
                TextSequenceNumber = BitParser<MyUshort>.Parsing(message, 57, 11).val;
                TextString = new byte[156];
                for (int i = 0; i < 156; i++)
                {
                    int startBit = 68 + (i * 6);
                    TextString[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                }
            }
            #region DEBUG
#if DebugMessage
            if (MessageId == 6)
            {
                Console.WriteLine("AcknowledgeRequriedFlag      : " + Convert.ToString(AcknowledgeRequriedFlag));
                Console.WriteLine("TextSequenceNumber           : " + Convert.ToString(TextSequenceNumber));
                char[] TextStringCh = new char[151];
                for (int i = 0; i < 151; i++)
                {
                    int tmp = (int)TextString[i];

                    TextStringCh[i] = AISMessageBase.SixBitASCII[tmp];
                }
                Console.WriteLine("TextStringCh                 : " + new string(TextStringCh));

            }
            else if (MessageId == 8)
            {
                
                Console.WriteLine("AcknowledgeRequriedFlag      : " + Convert.ToString(AcknowledgeRequriedFlag));
                Console.WriteLine("TextSequenceNumber           : " + Convert.ToString(TextSequenceNumber));
                char[] TextStringCh = new char[156];
                for (int i = 0; i < 156; i++)
                {
                    int tmp = (int)TextString[i];

                    TextStringCh[i] = AISMessageBase.SixBitASCII[tmp];
                }
                Console.WriteLine("TextStringCh                 : " + new string(TextStringCh));
            } 
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
            #endregion
        }
        #endregion
    }
}
