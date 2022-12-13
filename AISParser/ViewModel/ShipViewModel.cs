using AISParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.ViewModel
{
    public class ShipViewModel : ViewModelBase
    {
        #region Field 
        private Ship _ship;
        #endregion

        #region Property
        internal Ship Ship {
            get
            {
                if (_ship == null)
                {
                    _ship = new Ship();
                }
                return _ship;
            }
            set
            {
                _ship = value;
                OnPropertyChanged("Nationality");
                OnPropertyChanged("AISType");
                OnPropertyChanged("MMSI");
                OnPropertyChanged("ShipName");
                OnPropertyChanged("UserShipName");
                OnPropertyChanged("Callsign");
                OnPropertyChanged("IMO");
                OnPropertyChanged("ShipLength");
                OnPropertyChanged("ShipWidth");
                OnPropertyChanged("TargetDraft");
                OnPropertyChanged("TypeOfShip");
                OnPropertyChanged("DestinationPort");
                OnPropertyChanged("ETA");
                OnPropertyChanged("PositionAccuracy");
                OnPropertyChanged("ReceivingStation");
                OnPropertyChanged("ReceivingDate");
                OnPropertyChanged("Longtitude");
                OnPropertyChanged("Latitude");
                OnPropertyChanged("SOG");
                OnPropertyChanged("COG");
                OnPropertyChanged("ShipHeadDirection");
                OnPropertyChanged("ROT");
                OnPropertyChanged("PositionAccuracyD");
                OnPropertyChanged("NaviStatus");
                OnPropertyChanged("TimeStamp");
                OnPropertyChanged("ShipDirection");

            }
        }
        public int MID
        {
            get
            {
                return _ship.MID;
            }
            set
            {
                _ship.MID = value;
            }
        }

        public string Status
        {
            get
            {
                return Ship.Status;
            }
            set
            {
                Ship.Status = value;
            }
        }
        public bool IsBaseStation
        {
            get
            {
                return Ship.IsBaseStation;
            }
            set
            {
                Ship.IsBaseStation = value;
            }
        }
        public bool IsAtoN
        {
            get
            {
                return Ship.IsAtoN;
            }
            set
            {
                Ship.IsAtoN = value;
            }
        }
        public bool IsAirCraft
        {
            get
            {
                return Ship.IsAirCraft;
            }
            set
            {
                Ship.IsAirCraft = value;
            }
        }
        public string Nationality {
            get
            {
                return Ship.Nationality;
            }
            set
            {
                Ship.Nationality = value;
                OnPropertyChanged("Nationality");
            }
        }
        public string AISType
        {
            get
            {
                return Ship.AISType;
            }
            set
            {
                Ship.AISType = value;
                OnPropertyChanged("AISType");
            }
        }
        public int MMSI
        {
            get
            {
                if(Ship.MMSI== 0)
                {
                    Ship.MMSI = 0;
                }
                return Ship.MMSI;
            }
            set
            {
                Ship.MMSI = value;
                OnPropertyChanged("MMSI");

            }
        }
        public string UserShipName
        {
            get
            {
                return Ship.UserShipName;
            }
            set
            {
                Ship.UserShipName = value;
                OnPropertyChanged("UserShipName");
            }
        }
        public string ShipName
        {
            get
            {
                return Ship.ShipName;
            }
            set
            {
                Ship.ShipName = value;
                OnPropertyChanged("ShipName");
            }
        }
        public string Callsign
        {
            get
            {
                return Ship.Callsign;
            }
            set
            {
                Ship.Callsign = value;
                OnPropertyChanged("Callsign");
            }
        }
        public int IMO
        {
            get
            {
                return Ship.IMO;
            }
            set
            {
                Ship.IMO = value;
                OnPropertyChanged("IMO");

            }
        }
        public float ShipLength
        {
            get
            {
                return Ship.ShipLength;
            }
            set
            {
                Ship.ShipLength = value;
                OnPropertyChanged("ShipLength");
            }
        }
        public float ShipWidth
        {
            get
            {
                return Ship.ShipWidth;
            }
            set
            {
                Ship.ShipWidth = value;
                OnPropertyChanged("ShipWidth");
            }
        }
        public float TargetDraft
        {
            get
            {
                return Ship.TargetDraft;
            }
            set
            {
                Ship.TargetDraft = value;
                OnPropertyChanged("TargetDraft");
            }
        }
        public string TypeOfShip
        {
            get
            {
                return Ship.TypeOfShip;
            }
            set
            {
                Ship.TypeOfShip = value;
                OnPropertyChanged("TypeOfShip");
            }
        }
        public string DestinationPort
        {
            get
            {
                return Ship.DestinationPort;
            }
            set
            {
                Ship.DestinationPort = value;
                OnPropertyChanged("DestinationPort");

            }
        }
        public string ETA
        {
            get
            {
                return Ship.ETA;
            }
            set
            {
                Ship.ETA = value;
                OnPropertyChanged("ETA");
            }
        }
        public string PositionAccuracy
        {
            get
            {
                return Ship.PositionAccuracy;
            }
            set
            {
                Ship.PositionAccuracy = value;
                OnPropertyChanged("PositionAccuracy");
            }
        }


        public string ReceivingStation
        {
            get
            {
                return Ship.ReceivingStation;
            }
            set
            {
                Ship.ReceivingStation = value;
                OnPropertyChanged("ReceivingStation");

            }
        }
        public string ReceivingDate
        {
            get
            {
                return Ship.ReceivingDate;
            }
            set
            {
                Ship.ReceivingDate = value;
                OnPropertyChanged("ReceivingDate");
            }
        }
        public string Longtitude
        {
            get
            {
                return Ship.Longtitude;
            }
            set
            {
                Ship.Longtitude = value;
                OnPropertyChanged("Longtitude");
            }
        }
        public string Latitude
        {
            get
            {
                return Ship.Latitude;
            }
            set
            {
                Ship.Latitude = value;
                OnPropertyChanged("Latitude");

            }
        }
        public float SOG
        {
            get
            {
                return Ship.SOG;
            }
            set
            {
                Ship.SOG = value;
                OnPropertyChanged("SOG");
            }
        }
        public float COG
        {
            get
            {
                return Ship.COG;
            }
            set
            {
                Ship.COG = value;
                OnPropertyChanged("COG");
            }
        }
        public string ShipHeadDirection
        {
            get
            {
                return Ship.ShipHeadDirection;
            }
            set
            {
                Ship.ShipHeadDirection = value;
                OnPropertyChanged("ShipHeadDirection");
            }
        }
        public string ROT
        {
            get
            {
                return Ship.ROT;
            }
            set
            {
                Ship.ROT = value;
                OnPropertyChanged("ROT");

            }
        }
        public string PositionAccuracyD
        {
            get
            {
                return Ship.PositionAccuracyD;
            }
            set
            {
                Ship.PositionAccuracyD = value;
                OnPropertyChanged("PositionAccuracyD");
            }
        }
        public string NaviStatus
        {
            get
            {
                return Ship.NaviStatus;
            }
            set
            {
                Ship.NaviStatus = value;
                OnPropertyChanged("NaviStatus");
            }
        }
        public string TimeStamp
        {
            get
            {
                return Ship.TimeStamp;
            }
            set
            {
                Ship.TimeStamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }
        public float ShipDirection
        {
            get
            {
                return Ship.ShipDirection;
            }
            set
            {
                Ship.ShipDirection = value;
                OnPropertyChanged("ShipDirection");

            }
        }
        #endregion

        #region Constructor
        public ShipViewModel(Ship ship)
        {
            this.Ship = ship;
        }
        public ShipViewModel()
        {
            
        }
        #endregion

        #region Method
        public void SetShip(Ship ship)
        {
            Ship= ship;
        }
        public Ship GetShip()
        {
            return Ship;
        }
        #endregion
    }
}
