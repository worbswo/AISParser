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
	sealed public class StandardClassBEquipment : AISMessageBase
    {

        #region Property
        public byte Spare { get; set; } 
        public ushort SOG { get; set; }
        public bool PositionAccuracy { get; set; }
        public ushort COG { get; set; } 
        public ushort TrueHeading { get; set; }
        public byte TimeStemp { get; set; }
        public byte Spare2 { get; set; }
        public bool ClassBUnitFlag { get; set; }
        public bool ClassBDisplayFlag { get; set; }
        public bool ClassBDSCFlag { get; set; }
        public bool ClassBBandFlag { get; set; }
        public bool ClassBMessage22Flag { get; set; }
        public bool ModeFlag { get; set; }
        public bool RAIMFlag { get; set; }
        public bool CommunicationStateSelectorFlag { get; set; }
        public SOTDMA SOTDMA { get; set; } = new SOTDMA();
        public ITDMA ITDMA { get; set; } = new ITDMA();
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            Spare                           = BitParser<MyByte>.Parsing(message, 38, 8).val;
            SOG                             = BitParser<MyUshort>.Parsing(message, 46, 10).val;
            PositionAccuracy                = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 56, 1).val);
            Longitude                       = BitParser<MyFloat>.Parsing(message, 57, 28).val;
            Latitude                        = BitParser<MyFloat>.Parsing(message, 85, 27).val;
            COG                             = BitParser<MyUshort>.Parsing(message, 112, 12).val;
            TrueHeading                     = BitParser<MyUshort>.Parsing(message, 124, 9).val;
            TimeStemp                       = BitParser<MyByte>.Parsing(message, 133, 6).val;
            Spare2                          = BitParser<MyByte>.Parsing(message, 139, 2).val;
            ClassBUnitFlag                  = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 141, 1).val);
            ClassBDisplayFlag               = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 142, 1).val);
            ClassBDSCFlag                   = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 143, 1).val);
            ClassBBandFlag                  = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 144, 1).val);
            ClassBMessage22Flag             = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 145, 1).val);
            ModeFlag                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 146, 1).val);
            RAIMFlag                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 147, 1).val);
            CommunicationStateSelectorFlag  = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 148, 1).val);
            if (!CommunicationStateSelectorFlag)
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
            Console.WriteLine("message ID                       : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator                   : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("User ID                          : " + Convert.ToString(UserId));
            Console.WriteLine("Spare                            : " + Convert.ToString(Spare));
            Console.WriteLine("SOG                              : " + Convert.ToString(SOG));
            Console.WriteLine("PositionAccuracy                 : " + Convert.ToString(PositionAccuracy));
            Console.WriteLine("Longitude                        : " + Convert.ToString(Longitude*AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Latitude                         : " + Convert.ToString(Latitude*AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("COG                              : " + Convert.ToString(COG));
            Console.WriteLine("TrueHeading                      : " + Convert.ToString(TrueHeading));
            Console.WriteLine("TimeStamp                        : " + Convert.ToString(TimeStemp));
            Console.WriteLine("Spare2                           : " + Convert.ToString(Spare2));
            Console.WriteLine("ClassBUnitFlag                   : " + Convert.ToString(ClassBUnitFlag));
            Console.WriteLine("ClassBDisplayFlag                : " + Convert.ToString(ClassBDisplayFlag));
            Console.WriteLine("ClassBMessage22Flag              : " + Convert.ToString(ClassBMessage22Flag));
            Console.WriteLine("ModeFlag                         : " + Convert.ToString(ModeFlag));
            Console.WriteLine("RAIMFlag                         : " + Convert.ToString(RAIMFlag));
            Console.WriteLine("CommunicationStateSelectorFlag   : " + Convert.ToString(CommunicationStateSelectorFlag));
            if (!CommunicationStateSelectorFlag)
            {
                Console.WriteLine("SyncState                        : " + Convert.ToString(SOTDMA.SyncState));
                Console.WriteLine("SlotTimeout                      : " + Convert.ToString(SOTDMA.SlotTimeout));
                if ((SOTDMA.SlotTimeout == 3 || SOTDMA.SlotTimeout == 5 || SOTDMA.SlotTimeout == 7))
                {
                    Console.WriteLine("Received stations                : " + Convert.ToString(SOTDMA.Message));
                }
                else if (SOTDMA.SlotTimeout == 2 || SOTDMA.SlotTimeout == 4 || SOTDMA.SlotTimeout == 6)
                {
                    Console.WriteLine("Slot number                      : " + Convert.ToString(SOTDMA.Message));
                }
                else if (SOTDMA.SlotTimeout == 1)
                {
                    Console.WriteLine("UTC hour                         : " + Convert.ToString(SOTDMA.UTCHour));
                    Console.WriteLine("UTC hour                         : " + Convert.ToString(SOTDMA.UTCmin));
                }
            }
            else
            {
                Console.WriteLine("SyncState                        : " + Convert.ToString(ITDMA.SyncState));
                Console.WriteLine("SlotIncrement                    : " + Convert.ToString(ITDMA.SlotIncrement));
                Console.WriteLine("NumberOfSlots                    : " + Convert.ToString(ITDMA.NumberOfSlots));
                Console.WriteLine("KeepFlag                         : " + Convert.ToString(ITDMA.KeepFlag));

            }
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
            #endregion
        }
        #endregion
    }
}
