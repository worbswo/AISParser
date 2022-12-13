using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class ShipStaticAndVoyageRelatedDataMessage : AISMessageBase
    {
        #region Property
        public byte AISVersionIndicator { get; set; }
        public uint IMONumber { get; set; }
        public byte[] CallSine { get; set; } = new byte[7];
        public byte[] Name { get; set; } = new byte[20];


        public byte TypeOfShipAndCargoType { get; set; }
        public int DimensionBow { get; set; }
        public int DimensionStern { get; set; }
        public int DimensionPort { get; set; }
        public int DimensionStarboard { get; set; }
        public int ShipWidth { get; set; }
        public int ShipLength { get; set; }
        public byte TypeOfElectronicPositionFixingDevice { get; set; }
        public int ETAHour { get; set; }
        public int ETADay { get; set; }
        public int ETAMonth { get; set; }
        public int ETAMinute { get; set; }
        public byte MaximumPresentStaticDraught {get;set;}
        public byte[] Destination { get; set; } = new byte[20];

        public bool DTE { get; set; }
        public bool Spare { get; set; }
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            AISVersionIndicator = BitParser<MyByte>.Parsing(message, 38, 2).val;
            IMONumber = BitParser<MyUint>.Parsing(message, 40, 30).val;
            for(int i = 0; i < 7; i++)
            {
                int startBit = 70 +(i*6);
                CallSine[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
            }
            for (int i = 0; i < 20; i++)
            {
                int startBit = 112 + (i * 6);
                Name[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
            }
            TypeOfShipAndCargoType                  = BitParser<MyByte>.Parsing(message, 232, 8).val;
            uint overallDimension                   = BitParser<MyUint>.Parsing(message, 240, 30).val;
            DimensionBow                            = (int)((overallDimension & 0b_0011_1111_1110_0000_0000_0000_0000_0000)>>21);
            DimensionStern                          = (int)((overallDimension & 0b_0000_0000_0001_1111_1111_0000_0000_0000)>>12);
            DimensionPort                           = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_1111_1100_0000)>>6);
            DimensionStarboard                      = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_0000_0011_1111));
            ShipLength                              = DimensionBow + DimensionStern;
            ShipWidth                               = DimensionPort + DimensionStarboard;
            TypeOfElectronicPositionFixingDevice    = BitParser<MyByte>.Parsing(message, 270, 4).val;
            uint ETA                                = BitParser<MyUint>.Parsing(message, 274, 20).val;
            ETAMonth                                = (int)((ETA & 0b_0000_0000_0000_1111_0000_0000_0000_0000) >> 16);
            ETADay                                  = (int)((ETA & 0b_0000_0000_0000_0000_1111_1000_0000_0000) >> 11);
            ETAHour                                 = (int)((ETA & 0b_0000_0000_0000_0000_0000_0111_1100_0000) >> 6);
            ETAMinute                               = (int)(ETA & 0b_0000_0000_0000_0000_0000_0000_0011_1111);
            MaximumPresentStaticDraught             = BitParser<MyByte>.Parsing(message, 294, 8).val;
            for (int i = 0; i < 20; i++)
            {
                int startBit = 302 + (i * 6);
                Destination[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
            }
            DTE     = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 422, 1).val);
            Spare   = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 423, 1).val);

            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                           : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator                       : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("User ID                              : " + Convert.ToString(UserId, 10));
            Console.WriteLine("IMONumber                            : " + Convert.ToString(IMONumber));
            char[] CallSineCh = new char[7];
            for(int i = 0; i < 7; i++)
            {
                int tmp = (int)CallSine[i];

                CallSineCh[i] = AISMessageBase.SixBitASCII[tmp];
            }
            Console.WriteLine("CallSine                             : " +new string(CallSineCh));

            char[] NameCh = new char[20];
            for (int i = 0; i < 20; i++)
            {
                int tmp = (int)Name[i];
             
                NameCh[i] = AISMessageBase.SixBitASCII[tmp];
            }
            Console.WriteLine("Name                                 : " + new string(NameCh));
            Console.WriteLine("TypeOfShipAndCargoType               : " + Convert.ToString(TypeOfShipAndCargoType));
            Console.WriteLine("Dimension to Bow                     : " + Convert.ToString(DimensionBow));
            Console.WriteLine("Dimension to Stern                   : " + Convert.ToString(DimensionStern));
            Console.WriteLine("Dimension to Port                    : " + Convert.ToString(DimensionPort));
            Console.WriteLine("Dimension to Starboard               : " + Convert.ToString(DimensionStarboard));

            Console.WriteLine("TypeOfElectronicpositionFixingDevice : " + Convert.ToString(TypeOfElectronicPositionFixingDevice));
            Console.WriteLine("ETAMonth                             : " + Convert.ToString(ETAMonth));
            Console.WriteLine("ETADay                               : " + Convert.ToString(ETADay));
            Console.WriteLine("ETAHour                              : " + Convert.ToString(ETAHour));
            Console.WriteLine("ETAMinute                            : " + Convert.ToString(ETAMinute));

            Console.WriteLine("MaximumPresentStaticDraught          : " + Convert.ToString(MaximumPresentStaticDraught));
            char[] DestinationCh = new char[20];
            for (int i = 0; i < 20; i++)
            {
                int tmp = (int)Destination[i];

                DestinationCh[i] = AISMessageBase.SixBitASCII[tmp];
            }
            Console.WriteLine("Destination                          : " + new string(DestinationCh));
            Console.WriteLine("DTE                                  : " + Convert.ToString(DTE));
            Console.WriteLine("Spare                                : " + Convert.ToString(Spare));



            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
            #endregion
        }
        #endregion
    }
}
