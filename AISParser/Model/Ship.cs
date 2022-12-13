using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Model
{
    public class Ship
    {
        #region Property
        public int MID { get; set; } = 999;
        public string Status { get; set; } = "";
        public bool IsBaseStation { get; set; }
        public bool IsAtoN { get; set; }
        public bool IsAirCraft { get; set; }
        public string Nationality { get; set; } = "";
        public string AISType { get; set; } = "";
        public int MMSI { get; set; }
        public string ShipName { get; set; } = "";
        public string UserShipName { get; set; } = "";
        public string Callsign { get; set; }
        public int IMO { get; set; }
        public float ShipLength { get; set; }
        public float ShipWidth { get; set; }
        public float TargetDraft { get; set; }
        public string TypeOfShip { get; set; } = "";
        public string DestinationPort { get; set; } = "";
        public string ETA { get; set; } = "";
        public string PositionAccuracy { get; set; } = "";


        public string ReceivingStation { get; set; } = "";
        public string ReceivingDate { get; set; } = "";
        public string Longtitude { get; set; } = "";
        public string Latitude { get; set; } = "";
        public float SOG { get; set; }
        public float COG { get; set; }
        public string ShipHeadDirection { get; set; } = "";
        public string ROT { get; set; } = "";
        public string PositionAccuracyD { get; set; } = "";
        public string NaviStatus { get; set; } = "";
        public string TimeStamp { get; set; } = "";
        public float ShipDirection { get; set; }
        #endregion
    }
}
