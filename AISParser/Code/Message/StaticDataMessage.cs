using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class StaticDataMessage :AISMessageBase
    {
        #region Property
        public byte PartNumber { get; set; }    
        public byte[] Name { get; set; } = new byte[20];

        public byte TypeOfShipAndCargoType { get; set; }
        public byte[] VendorID { get; set; } = new byte[7];
        public byte[] CallSign { get; set; } = new byte[7];
        public int DimensionBow { get; set; }
        public int DimensionStern { get; set; }
        public int DimensionPort { get; set; }
        public int DimensionStarboard { get; set; }
        public int ShipWidth { get; set; }
        public int ShipLength { get; set; }
        public byte TypeOfElectronicPostition { get; set; }
        public byte Spare { get; set; }
       
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            PartNumber = BitParser<MyByte>.Parsing(message, 38, 2).val;
            if(PartNumber == 0 )
            {
                for (int i = 0; i < 20; i++)
                {
                    int startBit = 40 + (i * 6);
                    Name[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                }
            }
            else if(PartNumber == 1 )
            {
                TypeOfShipAndCargoType = BitParser<MyByte>.Parsing(message, 40, 8).val;
                for (int i = 0; i < 7; i++)
                {
                    int startBit = 48 + (i * 6);
                    VendorID[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                }
                for (int i = 0; i < 7; i++)
                {
                    int startBit = 90 + (i * 6);
                    CallSign[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
                }
                uint overallDimension       = BitParser<MyUint>.Parsing(message, 132, 30).val;
                DimensionBow                = (int)((overallDimension & 0b_0011_1111_1110_0000_0000_0000_0000_0000) >> 21);
                DimensionStern              = (int)((overallDimension & 0b_0000_0000_0001_1111_1111_0000_0000_0000) >> 12);
                DimensionPort               = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_1111_1100_0000) >> 6);
                DimensionStarboard          = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_0000_0011_1111));
                ShipLength                  = DimensionBow + DimensionStern;
                ShipWidth                   = DimensionPort + DimensionStarboard;
                TypeOfElectronicPostition   = BitParser<MyByte>.Parsing(message, 162, 4).val;
                Spare                       = BitParser<MyByte>.Parsing(message, 166, 2).val;

            }

            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                       : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("Repeatindcator                   : " + Convert.ToString(Repeatindicator, 10));
            Console.WriteLine("User ID                          : " + Convert.ToString(UserId));
            Console.WriteLine("PartNumber                       : " + Convert.ToString(PartNumber));
            if (PartNumber == 0)
            {
                char[] NameCh = new char[20];
                for (int i = 0; i < 20; i++)
                {
                    int tmp = (int)Name[i];

                    NameCh[i] = AISMessageBase.SixBitASCII[tmp];
                }
                Console.WriteLine("Name                             : " + new string(NameCh));
            }else if(PartNumber == 1)
            {
                Console.WriteLine("TypeOfShipAndCargoType           : " + Convert.ToString(TypeOfShipAndCargoType));
                char[] VendorIDCh = new char[7];
                for (int i = 0; i < 7; i++)
                {
                    int tmp = (int)VendorID[i];

                    VendorIDCh[i] = AISMessageBase.SixBitASCII[tmp];
                }
                Console.WriteLine("VendorID                         : " + new string(VendorIDCh)); 
                char[] CallSignCh = new char[7];
                for (int i = 0; i < 7; i++)
                {
                    int tmp = (int)CallSign[i];

                    CallSignCh[i] = AISMessageBase.SixBitASCII[tmp];
                }
                Console.WriteLine("CallSign                         : " + new string(CallSignCh));
                Console.WriteLine("Dimension to Bow                 : " + Convert.ToString(DimensionBow));
                Console.WriteLine("Dimension to Stern               : " + Convert.ToString(DimensionStern));
                Console.WriteLine("Dimension to Port                : " + Convert.ToString(DimensionPort));
                Console.WriteLine("Dimension to Starboard           : " + Convert.ToString(DimensionStarboard));
                Console.WriteLine("TypeOfElectronicPostition        : " + Convert.ToString(TypeOfElectronicPostition));
                Console.WriteLine("Spare                            : " + Convert.ToString(Spare));
            }
#endif
            #endregion
        }
        #endregion
    }
}
