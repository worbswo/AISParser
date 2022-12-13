using AISParser.Code.Message.CommunicationState;
using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class StandardSearchAndResuceAircraftMessage :AISMessageBase
    {
        #region Property
        public ushort Altitude { get; set; }
        public bool PositionAccuracy { get; set; }
        public ushort SOG { get; set; }
        public ushort COG { get; set; }
        public byte TimeStemp { get; set; }
        public bool AltitudeSensor { get; set; }
        public byte[] Spare { get; set; } = new byte[2];
        public bool DTE { get; set; }
        public bool AssignedModeFlag { get; set; }
        public bool RAIMFlag { get; set; }
        public bool CommunicationStateSelectFlag { get; set; }
        public SOTDMA SOTDMA { get; set; } = new SOTDMA();
        public ITDMA ITDMA { get; set; }=new ITDMA();
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            Altitude                        = BitParser<MyUshort>.Parsing(message, 38, 12).val;
            SOG                             = BitParser<MyUshort>.Parsing(message, 50, 10).val;
            PositionAccuracy                = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 60, 1).val);
            Longitude                       = BitParser<MyFloat>.Parsing(message, 61, 28).val;
            Latitude                        = BitParser<MyFloat>.Parsing(message, 89, 27).val;
            COG                             = BitParser<MyUshort>.Parsing(message, 116, 12).val;
            TimeStemp                       = BitParser<MyByte>.Parsing(message, 128, 6).val;
            AltitudeSensor                  = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 134, 1).val);
            Spare[0]                        = BitParser<MyByte>.Parsing(message, 135, 7).val;
            DTE                             = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 142, 1).val);
            Spare[1]                        = BitParser<MyByte>.Parsing(message, 143, 3).val;
            AssignedModeFlag                = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 146, 1).val);
            RAIMFlag                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 147, 1).val);
            CommunicationStateSelectFlag    = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 148, 1).val);

            if (!CommunicationStateSelectFlag)
            {
                SOTDMA.SyncState    = BitParser<MyByte>.Parsing(message, 149, 2).val;
                SOTDMA.SlotTimeout  = BitParser<MyByte>.Parsing(message, 151, 3).val;
                SOTDMA.Message      = BitParser<MyUshort>.Parsing(message, 154, 14).val;


                if (SOTDMA.SlotTimeout == 1)
                {
                    SOTDMA.UTCHour  = (byte)((SOTDMA.Message & 0b_001_1110_0000_0000) >> 9);
                    SOTDMA.UTCmin   = (byte)((SOTDMA.Message & 0b_0000_0001_1111_1100) >> 2);
                }
            }
            else 
            {
                ITDMA.SyncState     = BitParser<MyByte>.Parsing(message, 149, 2).val;
                ITDMA.SlotIncrement = BitParser<MyUshort>.Parsing(message, 151, 13).val;
                ITDMA.NumberOfSlots = BitParser<MyByte>.Parsing(message, 164, 3).val;
                ITDMA.KeepFlag      = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 167, 1).val);
            }
            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator               : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("User ID                      : " + Convert.ToString(UserId, 10));
            Console.WriteLine("SOG                          : " + Convert.ToString(SOG));
            Console.WriteLine("PositionAccuracy             : " + Convert.ToString(PositionAccuracy));
            Console.WriteLine("Longitude                    : " + Convert.ToString(Longitude * AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Latitude                     : " + Convert.ToString(Latitude * AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("COG                          : " + Convert.ToString(COG));
            Console.WriteLine("TimeStemp                    : " + Convert.ToString(TimeStemp));
            Console.WriteLine("AltitudeSensor               : " + Convert.ToString(AltitudeSensor));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[0]));
            Console.WriteLine("DTE                          : " + Convert.ToString(DTE));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare[1]));
            Console.WriteLine("AssignedModeFlag             : " + Convert.ToString(AssignedModeFlag));
            Console.WriteLine("RAIMFlag                     : " + Convert.ToString(RAIMFlag));
            Console.WriteLine("CommunicationStateSelectFlag : " + Convert.ToString(CommunicationStateSelectFlag));
            if (MessageId == 1 || MessageId == 2)
            {
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
            }
            else if (MessageId == 3)
            {
                Console.WriteLine("SyncState                    : " + Convert.ToString(ITDMA.SyncState));
                Console.WriteLine("SlotIncrement                : " + Convert.ToString(ITDMA.SlotIncrement));
                Console.WriteLine("NumberOfSlots                : " + Convert.ToString(ITDMA.NumberOfSlots));
                Console.WriteLine("KeepFlag                     : " + Convert.ToString(ITDMA.KeepFlag));

            }
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
            #endregion
        }
        #endregion
    }
}
