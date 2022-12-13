using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message
{
    public class AidsToNavigationMessage :AISMessageBase
    {
        #region Property
        public byte TypeOfAidsToNavigation {get; set; }
        public byte[] NameOfAidsToNavigation { get; set; }  =new byte[20];
        public bool PositionAccuracy { get; set; }
        public int DimensionBow { get; set; }
        public int DimensionStern { get; set; }
        public int DimensionPort { get; set; }
        public int DimensionStarboard { get; set; }
        public int ShipWidth { get; set; }
        public int ShipLength { get; set; }
        public byte TypeOfElectronicPositionFixingDevice { get; set; }
        public byte TimeStemp { get; set; }
        public bool OffPostionIndicator { get; set; }
        public byte AtoNStatus { get; set; }
        public bool RAIMFlag { get; set; }
        public bool VirtualAtoNFlag { get; set; }
        public bool AssignedModeFlag { get; set; }
        public bool Spare { get; set; }
        public byte[] NameToAidToNavigationExtension { get; set; } = new byte[14];
        #endregion

        #region Method
        public override void Parsing(byte[] message)
        {
            base.Parsing(message);
            TypeOfAidsToNavigation = BitParser<MyByte>.Parsing(message, 38, 5).val;
            for (int i = 0; i < 20; i++)
            {
                int startBit = 43 + (i * 6);
                NameOfAidsToNavigation[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
            }
            PositionAccuracy                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 163, 1).val);
            Longitude                               = BitParser<MyFloat>.Parsing(message, 164, 28).val;
            Latitude                                = BitParser<MyFloat>.Parsing(message, 192, 27).val;
            uint overallDimension                   = BitParser<MyUint>.Parsing(message, 219, 30).val;
            DimensionBow                            = (int)((overallDimension & 0b_0011_1111_1110_0000_0000_0000_0000_0000) >> 21);
            DimensionStern                          = (int)((overallDimension & 0b_0000_0000_0001_1111_1111_0000_0000_0000) >> 12);
            DimensionPort                           = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_1111_1100_0000) >> 6);
            DimensionStarboard                      = (int)((overallDimension & 0b_0000_0000_0000_0000_0000_0000_0011_1111));
            ShipLength                              = DimensionBow + DimensionStern;
            ShipWidth                               = DimensionPort + DimensionStarboard;
            TypeOfElectronicPositionFixingDevice    = BitParser<MyByte>.Parsing(message, 249, 4).val;
            TimeStemp                               = BitParser<MyByte>.Parsing(message, 253, 6).val;
            OffPostionIndicator                     = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 259, 1).val);
            AtoNStatus                              = BitParser<MyByte>.Parsing(message, 260, 8).val;
            RAIMFlag                                = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 268, 1).val);
            VirtualAtoNFlag                         = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 269, 1).val);
            AssignedModeFlag                        = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 270, 1).val);
            Spare                                   = Convert.ToBoolean(BitParser<MyByte>.Parsing(message, 271, 1).val);
            for (int i = 0; i < 14; i++)
            {
                int startBit = 272 + (i * 6);
                NameToAidToNavigationExtension[i] = BitParser<MyByte>.Parsing(message, startBit, 6).val;
            }

            #region DEBUG
#if DebugMessage
            Console.WriteLine("message ID                   : " + Convert.ToString(MessageId, 10));
            Console.WriteLine("User ID                      : " + Convert.ToString(UserId, 10));
            Console.WriteLine("Repeatindcator               : " + Convert.ToString(Repeatindicator));
            char[] NameOfAidsToNavigationCh = new char[20];
            for (int i = 0; i < 20; i++)
            {
                int tmp = (int)NameOfAidsToNavigation[i];

                NameOfAidsToNavigationCh[i] = AISMessageBase.SixBitASCII[tmp];
            }
            Console.WriteLine("NameOfAidsToNavigation       : " + new string(NameOfAidsToNavigationCh));
            Console.WriteLine("PositionAccuracy             : " + Convert.ToString(PositionAccuracy));
            Console.WriteLine("Longitude                    : " + Convert.ToString(Longitude*AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Latitude                     : " + Convert.ToString(Latitude*AISMessageBase.LongitudeLatitudeRatio));
            Console.WriteLine("Dimension                    : ");
            Console.WriteLine("Dimension to Bow             : " + Convert.ToString(DimensionBow));
            Console.WriteLine("Dimension to Stern           : " + Convert.ToString(DimensionStern));
            Console.WriteLine("Dimension to Port            : " + Convert.ToString(DimensionPort));
            Console.WriteLine("Dimension to Starboard       : " + Convert.ToString(DimensionStarboard));
            Console.WriteLine("TypeOfElectronic             : ");
            Console.WriteLine("PositionFixingDevice         : " + Convert.ToString(TypeOfElectronicPositionFixingDevice));
            Console.WriteLine("TimeStamp                    : " + Convert.ToString(TimeStemp));
            Console.WriteLine("OffPostionIndicator          : " + Convert.ToString(OffPostionIndicator));
            Console.WriteLine("AtoNStatus                   : " + Convert.ToString(AtoNStatus));
            Console.WriteLine("RAIMFlag                     : " + Convert.ToString(RAIMFlag));
            Console.WriteLine("Spare                        : " + Convert.ToString(Spare));
            char[] NameToAidToNavigationExtensionCh = new char[14];
            for (int i = 0; i < 14; i++)
            {
                int tmp = (int)NameToAidToNavigationExtension[i];

                NameToAidToNavigationExtensionCh[i] = AISMessageBase.SixBitASCII[tmp];
            }
            Console.WriteLine("NameToAidToNavigationE       : " + new string(NameToAidToNavigationExtensionCh));
            Console.WriteLine("------------------------------------------------------------------ ");
            Console.WriteLine("------------------------------------------------------------------ ");
#endif
            #endregion

        }
        #endregion
    }
}
