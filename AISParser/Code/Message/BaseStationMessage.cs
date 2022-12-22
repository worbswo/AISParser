using AISParser.Code.Message.CommunicationState;
using AISParser.Code.Parser.Interface;
using AISParser.Code.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
	sealed public class BaseStationMessage :AISMessageBase
    {
        #region Property
        public ushort UTCYear { get; set; }
        public byte UTCMonth { get; set; }
        public byte UTCDay { get; set; }
        public byte UTCHour{ get; set;}
        public byte UTCMinute { get; set; }
        public byte UTCSecond { get; set; }
        public bool PositionAccuracy { get; set; }

        public byte TypeOfElectronicPositionFixingDevice { get; set; }
        public bool TransmissionControlForLongRangeBroadcastMessage { get; set; }
        public ushort Spare { get; set; }
        public bool RAINFlag { get; set; }
        public SOTDMA SOTDMA { get; set; } = new SOTDMA();
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            UTCYear                                         = BitParser<MyUshort>.Parsing(message, 38, 14).val;
            UTCMonth                                        = BitParser<MyByte>.Parsing(message, 52, 4).val;
            UTCDay                                          = BitParser<MyByte>.Parsing(message, 56, 5).val;
            UTCHour                                         = BitParser<MyByte>.Parsing(message, 61, 5).val;
            UTCMinute                                       = BitParser<MyByte>.Parsing(message, 66, 6).val;
            UTCSecond                                       = BitParser<MyByte>.Parsing(message, 72, 6).val;
            PositionAccuracy                                = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 78, 1).val);
            Longitude                                       = BitParser<MyFloat>.Parsing(message, 79, 28).val;
            Latitude                                        = BitParser<MyFloat>.Parsing(message, 107, 27).val;
            TypeOfElectronicPositionFixingDevice            = BitParser<MyByte>.Parsing(message, 134, 4).val;
            TransmissionControlForLongRangeBroadcastMessage = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 138, 1).val);
            Spare                                           = BitParser<MyUshort>.Parsing(message, 139, 9).val;
            RAINFlag                                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 148, 1).val);
            SOTDMA.SyncState                                = BitParser<MyByte>.Parsing(message, 149, 2).val;
            SOTDMA.SlotTimeout                              = BitParser<MyByte>.Parsing(message, 151, 3).val;
            SOTDMA.Message                                  = BitParser<MyUshort>.Parsing(message, 154, 14).val;
            if (SOTDMA.SlotTimeout == 1)
            {
                SOTDMA.UTCHour  = (byte)((SOTDMA.Message & 0b_001_1110_0000_0000) >> 9);
                SOTDMA.UTCmin   = (byte)((SOTDMA.Message & 0b_0000_0001_1111_1100) >> 2);
            }

            #region Debug
#if DebugMessage
            Console.WriteLine("message ID                   : " +Convert.ToString(MessageId,10));
            Console.WriteLine("User ID                      : " + Convert.ToString(UserId, 10));
            Console.WriteLine("Repeatindcator               : " +Convert.ToString(Repeatindicator));
            Console.WriteLine("UTCYear                      : " + Convert.ToString(UTCYear));
            Console.WriteLine("UTCMonth                     : " + Convert.ToString(UTCMonth));
            Console.WriteLine("UTCDay                       : " + Convert.ToString(UTCDay));
            Console.WriteLine("UTCHour                      : " + Convert.ToString(UTCHour));
            Console.WriteLine("UTCMinute                    : " + Convert.ToString(UTCMinute));
            Console.WriteLine("UTCSecond                    : " + Convert.ToString(UTCSecond));
            Console.WriteLine("PositionAccauracy            : " + Convert.ToString(PositionAccuracy));
            Console.WriteLine("Longitude                    : " + Convert.ToString(Longitude*AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Latitude                     : " + Convert.ToString(Latitude* AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("TypeOfElectronic             : " + Convert.ToString(TypeOfElectronicPositionFixingDevice));
            Console.WriteLine("PositionFixingDevice         : ");
            Console.WriteLine("TransmissionControlFor       : " + Convert.ToString(TransmissionControlForLongRangeBroadcastMessage));
            Console.WriteLine("LongRangeBroadcastMessage    : ");
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare));
            Console.WriteLine("RAINFlag                     : " + Convert.ToString(RAINFlag));
            Console.WriteLine("SyncState                    : " + Convert.ToString(SOTDMA.SyncState));
            Console.WriteLine("SlotTimeout                  : " + Convert.ToString(SOTDMA.SlotTimeout));
            if ((SOTDMA.SlotTimeout == 3 || SOTDMA.SlotTimeout == 5 || SOTDMA.SlotTimeout == 7))
            {
                Console.WriteLine("Received stations            : " + Convert.ToString(SOTDMA.Message));
            }
            else if (SOTDMA.SlotTimeout == 2 || SOTDMA.SlotTimeout == 4 || SOTDMA.SlotTimeout == 6)
            {
                Console.WriteLine("Slot number                  : " + Convert.ToString(SOTDMA.Message));
            }
            else if (SOTDMA.SlotTimeout == 1)
            {
                Console.WriteLine("UTC hour                     : " + Convert.ToString(SOTDMA.UTCHour));
                Console.WriteLine("UTC hour                     : " + Convert.ToString(SOTDMA.UTCmin));
            }
            else
            {
                Console.WriteLine("Slot number                  : " + Convert.ToString(SOTDMA.Message));
            }
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");


#endif
            #endregion
        }
        #endregion
    }
}
