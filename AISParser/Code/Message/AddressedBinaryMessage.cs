using AISParser.Code.Parser.Interface;
using AISParser.Code.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AISParser.Code.Message.InternationalFunctionMessage;

namespace AISParser.Code.Message
{
    public class AddressedBinaryMessage :AISMessageBase
    {
        #region Property
        public byte RepeateIndicator { get; set; }
        public uint SourceId { get; set; }
        public byte SequenceNumber { get; set; }
        public uint DestinationId { get; set; }
        public bool RetransmitFlag { get; set; }
        public bool Spare { get; set; }
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
            SequenceNumber      = BitParser<MyByte>.Parsing(message, 38, 2).val;
            DestinationId       = BitParser<MyUint>.Parsing(message, 40, 30).val;
            RetransmitFlag      = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 70, 1).val);
            Spare               = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 71, 1).val);
            DAC                 = BitParser<MyUshort>.Parsing(message, 72, 10).val;
            FI                  = BitParser<MyByte>.Parsing(message, 82, 6).val;

            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Source ID                    : " + Convert.ToString(SourceId, 10));
            Console.WriteLine("RepeateIndicator             : " + Convert.ToString(RepeateIndicator));
            Console.WriteLine("SequenceNumber               : " + Convert.ToString(SequenceNumber));
            Console.WriteLine("DestinationId                : " + Convert.ToString(DestinationId));
            Console.WriteLine("RetransmitFlag               : " + Convert.ToString(RetransmitFlag));
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
                }else if (FI == 2)
                {
                    InternationalFunctionMessage = new SpecificFunctionalMessage();
                    InternationalFunctionMessage.Parsing(message);
                }
                else if (FI == 3)
                {
                    InternationalFunctionMessage = new CapabilityInterrogation();
                    InternationalFunctionMessage.Parsing(message);
                }
                else if (FI == 4)
                {
                    InternationalFunctionMessage = new CapabilityReply();
                    InternationalFunctionMessage.Parsing(message);
                }
                else if (FI == 5)
                {
                    InternationalFunctionMessage = new AcknowledgementToAddressedBinaryMessage();
                    InternationalFunctionMessage.Parsing(message);
                }
            }
         
        }

        #endregion
    }
}
