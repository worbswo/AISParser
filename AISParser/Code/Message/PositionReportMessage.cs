using AISParser.Code.Message.CommunicationState;
using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class PositionReportMessage : AISMessageBase
    {

        #region Property
        public byte NavigationalStatus { get; set; } = new byte();
        public sbyte RateOfTurnROTAIS { get; set; } = new sbyte();
        public ushort SOG { get; set; } = new ushort();
        public bool PositionAccuracy { get; set; } = new bool();
        public ushort COG { get; set; } = 0;

        public ushort TrueHeading { get; set; } = new ushort();
        public byte TimeStemp { get; set; } = new byte();
        public byte SpecialmanoeuvreIndicator { get; set; } = new byte();
        public byte Spare { get; set; } = new byte();
        public bool RAIMFlag { get; set; } = new bool();

        SOTDMA SOTDMA { get; set; } = new SOTDMA();
        ITDMA ITDMA { get; set; } = new ITDMA();
        #endregion

        #region Method
        /// <summary>
        /// AIS 메세지 파싱
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
        
            NavigationalStatus = BitParser<MyByte>.Parsing(message, 38, 4).val;
            RateOfTurnROTAIS            = BitParser<MySbyte>.Parsing(message, 42, 8).val;
            SOG                         = BitParser<MyUshort>.Parsing(message, 50, 10).val;
            PositionAccuracy            = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 60, 1).val);
            Longitude                   = BitParser<MyFloat>.Parsing(message, 61, 28).val;
            Latitude                    = BitParser<MyFloat>.Parsing(message, 89, 27).val;
            COG                         = BitParser<MyUshort>.Parsing(message, 116, 12).val;
            TrueHeading                 = BitParser<MyUshort>.Parsing(message, 128, 9).val;
            TimeStemp                   = BitParser<MyByte>.Parsing(message, 137, 6).val;
            SpecialmanoeuvreIndicator   = BitParser<MyByte>.Parsing(message, 143, 2).val;
            Spare                       = BitParser<MyByte>.Parsing(message, 145, 3).val;
            RAIMFlag                    = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 148, 1).val);
            if (MessageId ==1 || MessageId == 2)
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
           
            else if (MessageId == 3)
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
            Console.WriteLine("Naviagational status         : " +Convert.ToString(NavigationalStatus));
            Console.WriteLine("RateOfTurnROTAIS             : " + Convert.ToString(RateOfTurnROTAIS));
            Console.WriteLine("SOG                          : " + Convert.ToString(SOG));
            Console.WriteLine("PositionAccuracy             : " + Convert.ToString(PositionAccuracy));
            Console.WriteLine("Longitude                    : " + Convert.ToString(Longitude* AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Latitude                     : " + Convert.ToString(Latitude* AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("COG                          : " + Convert.ToString(COG));
            Console.WriteLine("TrueHeading                  : " + Convert.ToString(TrueHeading));
            Console.WriteLine("TimeStemp                    : " + Convert.ToString(TimeStemp));
            Console.WriteLine("SpecialmanoeuvreIndicator    : " + Convert.ToString(SpecialmanoeuvreIndicator));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare));
            Console.WriteLine("RAIMFlag                     : " + Convert.ToString(RAIMFlag));
            if (MessageId ==1|| MessageId == 2)
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
            }else if(MessageId == 3)
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
