using AISParser.Model;
using AISParser.Model.AirCraft;
using AISParser.Model.AtoN;
using AISParser.Model.ClassA;
using AISParser.Model.ClassB;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.ViewModel
{
	sealed public class ShipViewModel : ViewModelBase
	{
		#region Field 
		private AISBase _aISData;
		#endregion

		#region Property
		internal AISBase AISData {
			get
			{
				if (_aISData == null)
				{
					_aISData = new AISBase();
				}
				return _aISData;
			}
			set
			{
				_aISData = value;
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
				return _aISData.MID;
			}
			set
			{
				_aISData.MID = value;
			}
		}

		public string Status
		{
			get
			{
				return AISData.Status;
			}
			set
			{
				AISData.Status = value;
			}
		}
		public bool IsBaseStation
		{
			get
			{
				if (AISData.AISType == "Base Station")
				{
					return true;
				}
				else
				{
					return false;
				}

			}

		}
		public bool IsAtoN
		{
			get
			{
				if (AISData.AISType == "Aids to Navigation")
				{
					return true;
				}
				else
				{
					return false;
				}

			}
		}
		public bool IsAirCraft
		{
			get
			{
				if (AISData.AISType == "AirCraft")
				{
					return true;
				}
				else
				{
					return false;
				}

			}
		}
		public string Nationality {
			get
			{
				return AISData.Nationality;
			}
			set
			{
				AISData.Nationality = value;
				OnPropertyChanged("Nationality");
			}
		}
		public string AISType
		{
			get
			{
				return AISData.AISType;
			}
			set
			{
				AISData.AISType = value;
				OnPropertyChanged("AISType");
			}
		}
		public int MMSI
		{
			get
			{
				if (AISData.MMSI == 0)
				{
					AISData.MMSI = 0;
				}
				return AISData.MMSI;
			}
			set
			{
				AISData.MMSI = value;
				OnPropertyChanged("MMSI");

			}
		}
		public string UserShipName
		{
			get
			{
				if (AISData.AISStatic == null)
				{
					return "";
				}
				else
				{
					return AISData.AISStatic.UserShipName;

				}
			}
			set
			{
				if (AISData.AISStatic == null)
				{

				}
				else
					AISData.AISStatic.UserShipName = value;
				OnPropertyChanged("UserShipName");
			}
		}
		public string ShipName
		{
			get
			{
				if (AISData.AISStatic == null)
				{
					return "";
				}
				if (AISData.AISType == "Aids to Navigation")
				{
					return (AISData.AISStatic as AtoNStatic).ShipName;
				} else if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).ShipName;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISStatic as ClassBStatic).ShipName;
				}
				else
				{
					return "";
				}
			}
			set
			{
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Aids to Navigation")
				{
					(AISData.AISStatic as AtoNStatic).ShipName = value;
				}
				else if (AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).ShipName = value;
				}
				else if (AISData.AISType == "Class B")
				{
					(AISData.AISStatic as ClassBStatic).ShipName = value;
				}
				else
				{
				}
				OnPropertyChanged("ShipName");
			}
		}
		public string Callsign
		{
			get
			{
				if (AISData.AISStatic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).Callsign;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISStatic as ClassBStatic).Callsign;
				}
				else
				{
					return "";
				}
			}
			set
			{
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).Callsign = value;
				}
				else if (AISData.AISType == "Class B")
				{
					(AISData.AISStatic as ClassBStatic).Callsign = value;
				}
				OnPropertyChanged("Callsign");
			}
		}
		public int IMO
		{
			get
			{
				if (AISData.AISStatic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).IMO;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).IMO = value;
				}
				OnPropertyChanged("IMO");

			}
		}
		public float ShipLength
		{
			get
			{
				if (AISData.AISStatic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).ShipLength;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISStatic as ClassBStatic).ShipLength;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).ShipLength = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISStatic as ClassBStatic).ShipLength = value;
				}
				OnPropertyChanged("ShipLength");
			}
        }
        public float ShipWidth
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).ShipWidth;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISStatic as ClassBStatic).ShipWidth;
				}
				else
				{
					return 0;
				}
			}
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).ShipWidth = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISStatic as ClassBStatic).ShipWidth = value;
				}
				OnPropertyChanged("ShipWidth");
            }
        }
        public float TargetDraft
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).TargetDraft;
				}
				else
				{
					return 0;
				}
			}
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).TargetDraft = value;
				}
				OnPropertyChanged("TargetDraft");
            }
        }
        public string TypeOfShip
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).TypeOfShip;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISStatic as ClassBStatic).TypeOfShip;
				}
				else
				{
					return "";
				}
			}
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).TypeOfShip = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISStatic as ClassBStatic).TypeOfShip = value;
				}
				OnPropertyChanged("TypeOfShip");
            }
        }
        public string DestinationPort
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).DestinationPort;
				}
				else
				{
					return "";
				}
			}
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).DestinationPort = value;
				}
				OnPropertyChanged("DestinationPort");

            }
        }
        public string ETA
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISStatic as ClassAStatic).ETA;
				}
				else
				{
					return "";
				}
			}
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else if (AISData.AISType == "Class A")
				{
					(AISData.AISStatic as ClassAStatic).ETA = value;
				}
				OnPropertyChanged("ETA");
            }
        }
        public string PositionAccuracy
        {
            get
            {
				if (AISData.AISStatic == null)
				{
					return "";
				}
				return AISData.PositionAccuracy;
            }
            set
            {
				if (AISData.AISStatic == null)
				{

				}
				else
					AISData.PositionAccuracy = value;
                OnPropertyChanged("PositionAccuracy");
            }
        }

        public string ReceivingDate
        {
            get
            {
			
				return AISData.ReceivingDate;
            }
            set
            {
				
					AISData.ReceivingDate = value;
                OnPropertyChanged("ReceivingDate");
            }
        }
        public string Longtitude
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return "";
				}
				return AISData.AISDynamic.Longtitude;
            }
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISDynamic == null)
				{

				}
				else
					AISData.AISDynamic.Longtitude = value;
                OnPropertyChanged("Longtitude");
            }
        }
        public string Latitude
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return "";
				}
				return AISData.AISDynamic.Latitude;
            }
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISDynamic == null)
				{

				}
				else
					AISData.AISDynamic.Latitude = value;
                OnPropertyChanged("Latitude");

            }
        }
        public float SOG
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISDynamic as ClassADynamic).SOG;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISDynamic as ClassBDynamic).SOG;
				}
				else if (AISData.AISType == "AirCraft")
				{
					return (AISData.AISDynamic as AirCraftDynamic).SOG;
				}
				else
				{
					return 0;
				}
			}
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISDynamic == null)
				{
					
				}
				else if (AISData.AISType == "Class A")
				{
					(AISData.AISDynamic as ClassADynamic).SOG = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISDynamic as ClassBDynamic).SOG = value;
				}
				else if (AISData.AISType == "AirCraft")
				{
					(AISData.AISDynamic as AirCraftDynamic).SOG = value;
				}
				OnPropertyChanged("SOG");
            }
        }
        public float COG
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return 0;
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISDynamic as ClassADynamic).COG;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISDynamic as ClassBDynamic).COG;
				}
				else if (AISData.AISType == "AirCraft")
				{
					return (AISData.AISDynamic as AirCraftDynamic).COG;
				}
				else
				{
					return 0;
				}
			}
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISDynamic as ClassADynamic).COG = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISDynamic as ClassBDynamic).COG = value;
				}
				else if (AISData.AISType == "AirCraft")
				{
					(AISData.AISDynamic as AirCraftDynamic).COG = value;
				}
				OnPropertyChanged("COG");
            }
        }
        public string ShipHeadDirection
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISDynamic as ClassADynamic).ShipHeadDirection;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISDynamic as ClassBDynamic).ShipHeadDirection;
				}
				else
				{
					return "";
				}
			}
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISDynamic as ClassADynamic).ShipHeadDirection = value;
				}
				else if (AISData.AISType == "Class B")
				{

					(AISData.AISDynamic as ClassBDynamic).ShipHeadDirection = value;
				}
				OnPropertyChanged("ShipHeadDirection");
            }
        }
        public string ROT
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISDynamic as ClassADynamic).ROT;
				}
				else if (AISData.AISType == "Class B")
				{
					return (AISData.AISDynamic as ClassBDynamic).ROT;
				}
				else
				{
					return "";
				}
			}
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if(AISData.AISType == "Class A")
				{
					(AISData.AISDynamic as ClassADynamic).ROT = value;
				}
				else if (AISData.AISType == "Class B")
				{
					(AISData.AISDynamic as ClassBDynamic).ROT = value;
				}
				OnPropertyChanged("ROT");

            }
        }
        public string PositionAccuracyD
        {
            get
            {
				if (AISData.AISDynamic == null)
				{
					return "";
				}
				
				
					return AISData.AISDynamic.PositionAccuracyD;
				
				
				
			}
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else
					AISData.AISDynamic.PositionAccuracyD = value;
				OnPropertyChanged("PositionAccuracyD");
            }
        }
        public string NaviStatus
        {
            get
            {
				if(AISData.AISDynamic == null)
				{
					return "";
				}
				if (AISData.AISType == "Class A")
				{
					return (AISData.AISDynamic as ClassADynamic).NaviStatus;
				}
				else
				{
					return "";
				}
            }
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else if (AISData.AISType == "Class A")
				{
					(AISData.AISDynamic as ClassADynamic).NaviStatus = value;
				}
				OnPropertyChanged("NaviStatus");
            }
        }
        public string TimeStamp
        {
            get
            {
				if(AISData.AISDynamic == null)
				{
					return "";
				}
                return AISData.AISDynamic.TimeStamp;
            }
            set
            {
				if (AISData.AISDynamic == null)
				{
				}
				else
					AISData.AISDynamic.TimeStamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }
       
        #endregion

        #region Constructor
        public ShipViewModel(AISBase aisData)
        {
            this.AISData = aisData;
        }
        public ShipViewModel()
        {
            
        }
        #endregion

        #region Method
        public void SetShip(AISBase aisData)
        {
            AISData= aisData;
        }
        public AISBase GetShip()
        {
            return AISData;
        }
        #endregion
    }
}
