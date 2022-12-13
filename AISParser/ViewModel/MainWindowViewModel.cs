using AISParser.Code;
using AISParser.Code.Command;
using AISParser.Code.Message;
using AISParser.Code.Parser;
using AISParser.Model;
using AISParser.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace AISParser.ViewModel
{
    
    public class MainWindowViewModel :ViewModelBase
    {

    #region Field 
        private bool _isBaseStation = false;
        private bool _isAtoN = false;
        private bool _isAirCraft = false;
        private bool _isClassA = false;
        private bool _isClassB = false;

        private string _searchTxt;
        private string _targetText;

        private int _selectedIndex;

        private ICommand _doubleClickedCommand;
        private ICommand _searchCommand;
        private ICommand _keyDownCommand;
        private ICommand _sortCommand;
        private ICommand _listViewItemMouseDownCommand;
        private ShipViewModel _shipViewModel;

        private Dictionary<string, string> _dicHeaderName = new Dictionary<string, string>();
    #endregion

    #region Property

        internal DetailWindowViewModel DetailWindowViewModel { get; set; } = new DetailWindowViewModel();
        internal DetailWindow DetailWindow { get; set; } = new DetailWindow();
        internal Dictionary<int, ShipViewModel> DicShip { get; set; } = new Dictionary<int, ShipViewModel>(); //선박 모델
        Dictionary<string, Predicate<ShipViewModel>> Filters { get; set; } = new Dictionary<string, Predicate<ShipViewModel>>(); //리스트뷰 필터
        Thread receiveMessageThread { get; set; }
        AISMessageBase AISMessageBase { get; set; }

        private Dictionary<string, string> OriginHeaderName { get; set; } = new Dictionary<string, string>()
        {
            {"ReceivingDate","수신 일시"},    {"MMSI","물표 ID" },      {"AISType","타입" },
            {"Status","상태" },               {"ShipName","선명" },     {"UserShipName","사용자선명" },
            {"Latitude","위도" },             {"Longtitude","경도" },   {"SOG","속도" },
            {"COG","코너"}
        }; // 리스트뷰의 목록이름
        #region SortDescription
        MySortDescription SortAC { get; set; } = new MySortDescription(ListSortDirection.Ascending);
        MySortDescription SortDC { get; set; } = new MySortDescription(ListSortDirection.Descending);

       

        #endregion
        internal bool isSearch { get; set; } = false;
        internal bool isItem { get; set; } = false;
        public Dictionary<string, string> DicHeaderName
        {
            get
            {
                return _dicHeaderName;
            }
            set
            {
                _dicHeaderName = value;
                NotifyPropertyChanged("DicHeaderName");
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
           set
            {
                _selectedIndex= value;
            }
        }
        public string SearchTxt
        {
            get { return _searchTxt; }
            set
            {
                _searchTxt = value;
                OnPropertyChanged("SearchTxt");
            }
        }
        public string TargetText
        {
            get { return _targetText; }
            set
            {
                _targetText = value;
                OnPropertyChanged("TargetText");
            }
        }
        public bool IsClassA
        {
            get
            {
                return _isClassA;
            }
            set
            {
                if (_isClassA != value)
                {
                    _isClassA = value;
                    if (_isClassA)
                    {
                        IsClassAFilter();
                    }
                    else
                    {
                        RemoveFilter("ClassA");
                    }
                }
                TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

            }
        }
        public bool IsClassB
        {
            get
            {
                return _isClassB;
            }
            set
            {
                if (_isClassB != value)
                {
                    _isClassB = value;
                    if (_isClassB)
                    {
                        IsClassBFilter();
                    }
                    else
                    {
                        RemoveFilter("ClassB");
                    }
                }
                TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

            }
        }
        public bool IsAtoN
        {
            get
            {
                return _isAtoN;
            }
            set
            {
                if (_isAtoN != value)
                {
                    _isAtoN = value;
                    if (_isAtoN)
                    {
                        IsAtoNFilter();
                    }
                    else
                    {
                        RemoveFilter("AtoN");
                    }
                }
                TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

            }
        }
        public bool IsAirCraft
        {
            get
            {
                return _isAirCraft;
            }
            set
            {
                if (_isAirCraft != value)
                {
                    _isAirCraft = value;
                    if (_isAirCraft)
                    {
                        IsAirCraftFilter();
                    }
                    else
                    {
                        RemoveFilter("AirCraft");
                    }
                    TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

                }

            }
        }
        public bool IsBaseStation
        {
            get
            {
                return _isBaseStation;
            }
            set
            {
                if (_isBaseStation != value)
                {
                    _isBaseStation = value;
                    if (_isBaseStation)
                    {
                       IsBaseStationFilter();

                    }
                    else
                    {
                        RemoveFilter("BaseStation");
                    }
                    TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

                }
            }
        }
      

        internal MessageParser MessageParser { get; set; }
        private ObservableCollection<ShipViewModel> ShipCollection { get; set; }
        private CollectionViewSource ShipCollectionViewSource { get; set; }
        public ICollectionView ShipCollectionView
        {
            get { return ShipCollectionViewSource.View; }
            set
            {
                OnPropertyChanged("ShipCollectionView");
            }
        }

        /// <summary>
        /// MainView의 ListBox에서 선택된 아이템
        /// </summary>
        public ShipViewModel SelectedItem
        {
            get
            {

                return _shipViewModel;
            }
            set
            {
                if (_shipViewModel != value && value != null)
                {
                    _shipViewModel = value;

                    OnPropertyChanged("SelectedItem");
                }
            }
        }
     #endregion

    #region Construct
        public MainWindowViewModel()
        {

            AISMessageBase = new AISMessageBase();

            ShipCollection = new ObservableCollection<ShipViewModel>();
            ShipCollectionViewSource = new CollectionViewSource();
            ShipCollectionViewSource.Source = this.ShipCollection;
            ShipCollectionViewSource.View.Filter = Filter;

            MessageParser = new MessageParser();
            receiveMessageThread = new Thread(receivedMessage);
            receiveMessageThread.Start();

            IsClassA=true;
            IsClassB = true;
            IsBaseStation=true;
            IsAirCraft=true;
            IsAtoN=true;

            DetailWindow.DataContext = DetailWindowViewModel;
            DetailWindowViewModel.RequestClose += (o, e) => { DetailWindow.Close(); };
            DetailWindow.Closing += (o, e) =>
            {
                (e as CancelEventArgs).Cancel = true;
                DetailWindow.Hide();
            };
            DetailWindow.Hide();

            foreach(var item in OriginHeaderName)
            {
                DicHeaderName.Add(item.Key, item.Value);
            }
        }

    #endregion

    #region Command
        public ICommand ListViewItemMouseDownCommand
        {
            get
            {
                if (_listViewItemMouseDownCommand == null)
                {
                    _listViewItemMouseDownCommand = new RelayCommand(ListViewItemMouseDownMethod, null);
                }
                return _listViewItemMouseDownCommand;
            }
        }
        public ICommand SortCommand
        {
            get
            {
                if (_sortCommand == null)
                {
                    _sortCommand = new RelayCommand(SortMethod, null);
                }
                return _sortCommand;
            }
        }
        public ICommand KeyDownCommand
        {
            get
            {
                if (_keyDownCommand == null)
                {
                    _keyDownCommand = new RelayCommand(KeyDownMethod, null);
                }
                return _keyDownCommand;
            }
        }
        public ICommand DoubleClickedCommand
        {
            get
            {
                if (_doubleClickedCommand == null)
                {
                    _doubleClickedCommand = new RelayCommand(DoubleClickMethod, null);
                }
                return _doubleClickedCommand;
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(SearchMethod, null);
                }
                return _searchCommand;
            }
        }
    #endregion

    #region Method

        /// <summary>
        /// 파싱된 메시지를 받아서 데이터를 변환
        /// </summary>
        private void receivedMessage()
        {
            while (true)
            {
                AISMessageBase = MessageParser.Parsing();

                if (AISMessageBase == null)
                {
                    Thread.Sleep(1);

                    continue;
                }
                if ((int)AISMessageBase.MessageId == 1 || (int)AISMessageBase.MessageId == 2 || (int)AISMessageBase.MessageId == 3)
                {
                    DispatcherService.Invoke(PositionReprotTypeConvert);
                }
                else if ((int)AISMessageBase.MessageId == 4)
                {
                    DispatcherService.Invoke(BaseStationConverter);
                }
                else if ((int)AISMessageBase.MessageId == 5)
                {
                    DispatcherService.Invoke(ShipStaticAndVoyageRelatedDataConverter);
                }
                else if ((int)AISMessageBase.MessageId == 9)
                {
                    DispatcherService.Invoke(StandardClassBEquipmentPositionConvert);
                }
                else if ((int)AISMessageBase.MessageId == 18)
                {
                    DispatcherService.Invoke(StandardClassBEquipmentPositionConvert);
                }
                else if ((int)AISMessageBase.MessageId == 21)
                {
                    DispatcherService.Invoke(AtoNMessageConvert);
                }
                else if ((int)AISMessageBase.MessageId == 24)
                {
                    DispatcherService.Invoke(StaticDataConvert);
                }
                DispatcherService.Invoke(() => {
                    TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

                });

                Thread.Sleep(1);
            }

        }

        #region CommandMethod
        /// <summary>
        /// list View header 클릭 시 정렬하는 함수
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <param name="param">헤더 이름</param>
        public void SortMethod(object obj, object args,object param)
        {
            string header = (string)param;
            isItem = false;
            if (ShipCollectionViewSource.View.SortDescriptions.Contains(SortAC[header]))
            {
                ShipCollectionViewSource.View.SortDescriptions.Remove(SortAC[header]);
                ShipCollectionViewSource.View.SortDescriptions.Add(SortDC[header]);
                DicHeaderName[header] = OriginHeaderName[header] + " ▲";

            }
            else if (ShipCollectionViewSource.View.SortDescriptions.Contains(SortDC[header]))
            {
                ShipCollectionViewSource.View.SortDescriptions.Remove(SortDC[header]);
                DicHeaderName[header] = OriginHeaderName[header];


            }
            else
            {
                ShipCollectionViewSource.View.SortDescriptions.Add(SortAC[header]);
                DicHeaderName[header] = OriginHeaderName[header] + " ▼";

            }
            DicHeaderName = DicHeaderName;
        }

        /// <summary>
        /// Enter Key를 입력 했을때 검색
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args">event</param>
        public void KeyDownMethod(object obj, object args,object param)
        {
            KeyEventArgs e = args as KeyEventArgs;
            if (e != null)
            {
                if (e.Key == Key.Enter)
                {
                    SearchMethod(obj);
                }
                if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                {
                    if (e.Key == Key.C)
                    {
                        CopyListBox(param as ListView);

                    }
                }
            }
        }
        public void CopyListBox(ListView list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("수신 일시" + "\t");
            sb.Append("MMSI" + "\t");
            sb.Append("AIS 타입" + "\t");
            sb.Append("상태" + "\t");
            sb.Append("선명" + "\t");
            sb.Append("사용자 선명" + "\t");
            sb.Append("위도" + "\t");
            sb.Append("경도" + "\t");
            sb.Append("속도" + "\t");
            sb.Append("코너");
            sb.AppendLine();
            foreach (var item in list.SelectedItems)
            {
                ShipViewModel l = (item as ShipViewModel);
               

                if (l != null)
                {
                    sb.Append(l.ReceivingDate.ToString() + "\t");
                    sb.Append(l.MMSI.ToString() + "\t");
                    sb.Append(l.AISType.ToString() + "\t");
                    sb.Append(l.Status.ToString() + "\t");
                    sb.Append(l.ShipName + "\t");
                    sb.Append(l.UserShipName+ "\t");
                    sb.Append(l.Latitude + "\t");
                    sb.Append(l.Longtitude.ToString() + "\t");
                    sb.Append(l.SOG.ToString() + "\t");
                    sb.Append(l.COG.ToString());

                }
                sb.AppendLine();
            }
            Clipboard.SetDataObject(sb.ToString().Trim());

        }

        public void ListViewItemMouseDownMethod(object sender, object args)
        {
            isItem = true;
        }
        public void DoubleClickMethod(object sender, object args)
        {
            if (!isItem) return;
            if (SelectedItem == null) return;
            DetailWindowViewModel.SetShip(SelectedItem);
            if (DetailWindow.Visibility == Visibility.Hidden)
            {
                DetailWindow.Show();
            }
            else
            {
                DetailWindow.WindowState = WindowState.Normal;
                DetailWindow.Activate(); 
                DetailWindow.Topmost= true;
                DetailWindow.Topmost = false;
                DetailWindow.Focus();
            }
        }

        public void SearchMethod(object sender)
        {

            if (SearchTxt == ""|| SearchTxt==null)
            {
                isSearch = false;
            }
            else
            {
                isSearch=true;

            }
            ShipCollectionViewSource.View.Refresh();
            TargetText = ShipCollectionViewSource.View.Cast<ShipViewModel>().Count().ToString();

        }
        #endregion

        #region Filter
        /// <summary>
        /// 모든 필터를 합침
        /// </summary>
        /// <param name="obj"></param> 
        /// <returns></returns>
        private bool Filter(object obj)
        {
            ShipViewModel c = (ShipViewModel)obj;
            return Filters.Values
                .Aggregate(false,
                    (prevValue, predicate) => prevValue || predicate(c));
        }
        /// <summary>
        /// 필터를 삭제함
        /// </summary>
        /// <param name="filterName">삭제할 필터 이름</param>
        public void RemoveFilter(string filterName)
        {
            if (Filters.Remove(filterName))
            {
                ShipCollectionViewSource.View.Refresh();
            }
        }
        /// <summary>
        /// 새로운 필터를 추가하고 리프레쉬한다
        /// </summary>
        /// <param name="name">추가할 필터 이름</param>
        /// <param name="predicate">필터 구현</param>
        private void AddFilterAndRefresh(string name, Predicate<ShipViewModel> predicate)
        {
            Filters.Add(name, predicate);
            ShipCollectionViewSource.View.Refresh();
        }
        /// <summary>
        /// 메시지가 Class B타입인지 확인
        /// </summary>
        public void IsClassBFilter()
        {
            AddFilterAndRefresh("ClassB",
                shipViewModel =>
                {
                    if (shipViewModel.AISType == "Class B")
                    {
                        if (!SearchFilter(shipViewModel)){
                            return false;
                        }
                        return true;
                    }

                    return false;
                });
        }
        /// <summary>
        /// 검색된 문자가 포함된 AIS 정보 검색
        /// </summary>
        public bool SearchFilter(ShipViewModel obj)
        {
            if (!isSearch)
            {
                return true;
            }
            if (SelectedIndex == 0)
            {
                
                if (obj.MMSI.ToString() == null)
                {
                    return false;
                }
                 if (obj.MMSI.ToString().Contains(SearchTxt))
                {
                    return true;
                }
            }else if(SelectedIndex == 1)
            {
                if ((obj).ShipName == null)
                {
                    return false;
                }
                if ((obj).ShipName.Contains(SearchTxt))
                {
                    return true;
                }
            }
            else if(SelectedIndex == 2)
            {
                if ((obj).Callsign == null)
                {
                    return false;
                }
                if ((obj).Callsign.Contains(SearchTxt))
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 메시지가 ClassA 타입인지 확인
        /// </summary>
        public void IsClassAFilter()
        {
            AddFilterAndRefresh("ClassA",
                shipViewModel =>
                {
                    if (shipViewModel.AISType == "Class A")
                    {
                        if (!SearchFilter(shipViewModel))
                        {
                            return false;
                        }
                        return true;
                    }

                    return false;
                });
        }

        /// <summary>
        /// IsBaseStation이 True인 List만 출력
        /// </summary>
        /// <returns></returns>
        public void IsBaseStationFilter()
        {
            AddFilterAndRefresh("BaseStation",
               shipViewModel =>
               {
                   if (!SearchFilter(shipViewModel))
                   {
                       return false;
                   }
                   return shipViewModel.IsBaseStation;
               });
        }
        /// <summary>
        /// IsAtoN이 True인 List만 출력
        /// </summary>
        /// <returns></returns>
        public void IsAtoNFilter()
        {
            AddFilterAndRefresh("AtoN",
               shipViewModel =>
               {
                   if (!SearchFilter(shipViewModel))
                   {
                       return false;
                   }
                   return shipViewModel.IsAtoN;
               });
        }/// <summary>
         /// IsAirCraft이 True인 List만 출력
         /// </summary>
         /// <returns></returns>
        public void IsAirCraftFilter()
        {
            AddFilterAndRefresh("AirCraft",
               shipViewModel =>
               {
                   if (!SearchFilter(shipViewModel))
                   {
                       return false;
                   }
                   return shipViewModel.IsAirCraft;
               });
        }
        #endregion

        #region Converter
        public string ConvertCoord(float coord,bool isLong=true)
        {
            string result = "";
            int coord1 = (int)coord;
            int coord2 = (int)(coord - coord1) * 60;
            float coor3 = ((coord - coord1) * 60 - coord2) * 60;
            float coord4 = coord2 + (float)Math.Round(coor3 / 60f, 3);
            if (isLong)
            {
                result = String.Format("{0}˚{1:00.00000}' E", coord1, coord4);
            }
            else
            {
                result = String.Format("{0}˚{1:00.00000}' N", coord1, coord4);
            }
            return result;
        }
        
        /// <summary>
        /// 파싱한 messageType 1,2,3 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void PositionReprotTypeConvert()
        {
            PositionReportMessage message = AISMessageBase as PositionReportMessage;
            
            string  positionAccuracy    = message.PositionAccuracy ? "high (<10m)" : "low(>10m)";
            float   longtitude          = message.Longitude * AISMessageBase.LongitudeLatitudeRatio;
            float   latitude            = message.Latitude * AISMessageBase.LongitudeLatitudeRatio;
            
            if (!DicShip.ContainsKey((int)message.UserId))
            {

                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    AISType             = "Class A",
                    MMSI                = (int)message.UserId,
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    COG                 = message.COG / 10f,
                    PositionAccuracyD   = positionAccuracy,
                    ROT                 = AISMessageBase.ConvertROT(message.RateOfTurnROTAIS),
                    ShipHeadDirection   = message.TrueHeading.ToString(),
                    Longtitude          = ConvertCoord(longtitude),
                    Latitude            = ConvertCoord(latitude,false),
                    TimeStamp           = message.TimeStemp.ToString(),
                    SOG                 = message.SOG / 10f,
                    NaviStatus          = AISMessageBase.NaviSatus[(int)message.NavigationalStatus],
                    ReceivingDate       = message.ReceivingDate
                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));
                
            }
            else
            {
                DicShip[(int)message.UserId].AISType             = "Class A";
                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].COG                 = message.COG / 10f;
                DicShip[(int)message.UserId].PositionAccuracyD   = positionAccuracy;
                DicShip[(int)message.UserId].ROT                 = AISMessageBase.ConvertROT(message.RateOfTurnROTAIS);
                DicShip[(int)message.UserId].ShipHeadDirection   = message.TrueHeading.ToString();
                DicShip[(int)message.UserId].Longtitude          = ConvertCoord(longtitude);
                DicShip[(int)message.UserId].Latitude            = ConvertCoord(latitude,false);
                DicShip[(int)message.UserId].TimeStamp           = message.TimeStemp.ToString();
                DicShip[(int)message.UserId].SOG                 = message.SOG / 10f;
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                DicShip[(int)message.UserId].NaviStatus          = AISMessageBase.NaviSatus[(int)message.NavigationalStatus];
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
        /// <summary>
        /// 파싱한 messageType 5 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void ShipStaticAndVoyageRelatedDataConverter()
        {
            ShipStaticAndVoyageRelatedDataMessage message = AISMessageBase as ShipStaticAndVoyageRelatedDataMessage;

           
            if (!DicShip.ContainsKey((int)message.UserId))
            {
                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    AISType             = "Class A",
                    MMSI                = (int)message.UserId,
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    IMO                 = (int)message.IMONumber,
                    Callsign            = AISMessageBase.Convert6bitAscII(message.CallSine, 7),
                    ShipName            = AISMessageBase.Convert6bitAscII(message.Name, 20),
                    DestinationPort     = AISMessageBase.Convert6bitAscII(message.Destination, 20),
                    ShipLength          = message.ShipLength,
                    ShipWidth           = message.ShipWidth,
                    TypeOfShip          = AISMessageBase.ConvertCargoType((int)message.TypeOfShipAndCargoType),
                    PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice],
                    ReceivingDate       = message.ReceivingDate,
                    TargetDraft         = (float)message.MaximumPresentStaticDraught / 10f,
                    ETA                 = String.Format("{0:00}.{1:00} {2:00}:{3:00}", message.ETAMonth, message.ETADay, message.ETAHour, message.ETAMinute)
                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));

            }
            else
            {
                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].AISType             = "Class A";
                DicShip[(int)message.UserId].IMO                 = (int)message.IMONumber;
                DicShip[(int)message.UserId].Callsign            = AISMessageBase.Convert6bitAscII(message.CallSine, 7);
                DicShip[(int)message.UserId].ShipName            = AISMessageBase.Convert6bitAscII(message.Name, 20);
                DicShip[(int)message.UserId].DestinationPort     = AISMessageBase.Convert6bitAscII(message.Destination, 20);
                DicShip[(int)message.UserId].ShipLength          = message.ShipLength;
                DicShip[(int)message.UserId].ShipWidth           = message.ShipWidth;
                DicShip[(int)message.UserId].TypeOfShip          = AISMessageBase.ConvertCargoType((int)message.TypeOfShipAndCargoType);
                DicShip[(int)message.UserId].PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice];
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                DicShip[(int)message.UserId].TargetDraft         = (float)message.MaximumPresentStaticDraught/10f;
                DicShip[(int)message.UserId].ETA                 = String.Format("{0:00}.{1:00} {2:00}:{3:00}", message.ETAMonth, message.ETADay, message.ETAHour, message.ETAMinute);
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
        /// <summary>
        ///  파싱한 messageType 4 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void BaseStationConverter()
        {
            BaseStationMessage message = (AISMessageBase as BaseStationMessage);

            string positionAccuracy = message.PositionAccuracy ? "high (<10m)" : "low(>10m)";
            float longtitude        = message.Longitude * AISMessageBase.LongitudeLatitudeRatio;
            float latitude          = message.Latitude * AISMessageBase.LongitudeLatitudeRatio;

            if (!DicShip.ContainsKey((int)message.UserId))
            {
                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    MMSI                = (int)message.UserId,
                    AISType             = "Base Station",
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    PositionAccuracyD   = positionAccuracy,
                    PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice],
                    Longtitude          = ConvertCoord(longtitude),
                    Latitude            = ConvertCoord(latitude,false),
                    IsBaseStation       = true,
                    ReceivingDate       = message.ReceivingDate,
                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));

            }
            else
            {

                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].AISType             = "Base Station";
                DicShip[(int)message.UserId].PositionAccuracyD   = positionAccuracy;
                DicShip[(int)message.UserId].PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice];
                DicShip[(int)message.UserId].Longtitude          = ConvertCoord(longtitude);
                DicShip[(int)message.UserId].Latitude            = ConvertCoord(latitude,false);
                DicShip[(int)message.UserId].IsBaseStation       = true;
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
        /// <summary>
        /// 파싱한 messageType 9 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void StandardSearchAndRescueAircraftConvert()
        {
            StandardSearchAndResuceAircraftMessage message = AISMessageBase as StandardSearchAndResuceAircraftMessage;

            string positionAccuracy = message.PositionAccuracy ? "high (<10m)" : "low(>10m)";
            float longtitude        = message.Longitude * AISMessageBase.LongitudeLatitudeRatio;
            float latitude          = message.Latitude * AISMessageBase.LongitudeLatitudeRatio;

            if (!DicShip.ContainsKey((int)message.UserId))
            {
                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    AISType             = "AirCraft",
                    MMSI                = (int)message.UserId,
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    COG                 = message.COG / 10f,
                    PositionAccuracyD   = positionAccuracy,
                    Longtitude          = ConvertCoord(longtitude),
                    Latitude            = ConvertCoord(latitude,false),
                    TimeStamp           = message.TimeStemp.ToString(),
                    SOG                 = message.SOG / 10f,
                    ReceivingDate       = message.ReceivingDate,
                    IsAirCraft          = true
                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));
            }
            else
            {
                DicShip[(int)message.UserId].AISType             = "AirCraft";
                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].COG                 = message.COG / 10f;
                DicShip[(int)message.UserId].PositionAccuracyD   = positionAccuracy;
                DicShip[(int)message.UserId].Longtitude          = ConvertCoord(longtitude);
                DicShip[(int)message.UserId].Latitude            = ConvertCoord(latitude,false);
                DicShip[(int)message.UserId].TimeStamp           = message.TimeStemp.ToString();
                DicShip[(int)message.UserId].SOG                 = message.SOG / 10f;
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                DicShip[(int)message.UserId].IsAirCraft          = true;
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
      
        /// <summary>
        /// 파싱한 messageType 21 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void AtoNMessageConvert()
        {
            AidsToNavigationMessage message = AISMessageBase as AidsToNavigationMessage;

            string positionAccuracy = message.PositionAccuracy ? "high (<10m)" : "low(>10m)";
            float longtitude        = message.Longitude * AISMessageBase.LongitudeLatitudeRatio;
            float latitude          = message.Latitude * AISMessageBase.LongitudeLatitudeRatio;
            if (!DicShip.ContainsKey((int)message.UserId))
            {
               
                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    MMSI                = (int)message.UserId,
                    PositionAccuracyD   = positionAccuracy,
                    AISType             = "Aids to Navigation",
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    ShipName            = AISMessageBase.Convert6bitAscII(message.NameOfAidsToNavigation, 20) + AISMessageBase.Convert6bitAscII(message.NameToAidToNavigationExtension, 14),
                    PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice],
                    Longtitude          = ConvertCoord(longtitude),
                    Latitude            = ConvertCoord(latitude,false),
                    TimeStamp           = message.TimeStemp.ToString(),
                    ReceivingDate       = message.ReceivingDate,
                    ShipLength          = message.ShipLength,
                    ShipWidth           = message.ShipWidth,
                    IsAtoN              = true

                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));
            }
            else
            {
                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].ShipName            = AISMessageBase.Convert6bitAscII(message.NameOfAidsToNavigation, 20) + AISMessageBase.Convert6bitAscII(message.NameToAidToNavigationExtension, 14);
                DicShip[(int)message.UserId].AISType             = "Aids to Navigation";
                DicShip[(int)message.UserId].PositionAccuracyD   = positionAccuracy;
                DicShip[(int)message.UserId].PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPositionFixingDevice];
                DicShip[(int)message.UserId].Longtitude          = ConvertCoord(longtitude);
                DicShip[(int)message.UserId].Latitude            = ConvertCoord(latitude,false);
                DicShip[(int)message.UserId].TimeStamp           = message.TimeStemp.ToString();
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                DicShip[(int)message.UserId].IsAtoN              = true;
                DicShip[(int)message.UserId].ShipLength          = message.ShipLength;
                DicShip[(int)message.UserId].ShipWidth           = message.ShipWidth;
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
        /// <summary>
        ///  파싱한 messageType 18 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void StandardClassBEquipmentPositionConvert()
        {
            StandardClassBEquipment message = AISMessageBase as StandardClassBEquipment;

            string positionAccuracy = message.PositionAccuracy ? "high (<10m)" : "low(>10m)";
            float longtitude        = message.Longitude * AISMessageBase.LongitudeLatitudeRatio;
            float latitude          = message.Latitude * AISMessageBase.LongitudeLatitudeRatio;

            if (!DicShip.ContainsKey((int)message.UserId))
            {
            
                DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                {
                    MMSI                = (int)message.UserId,
                    AISType             = "Class B",
                    Nationality         = AISMessageBase.NationNumber[message.MID][0],
                    MID                 = (int)message.MID,
                    PositionAccuracyD   = positionAccuracy,
                    Longtitude          = ConvertCoord(longtitude),
                    Latitude            = ConvertCoord(latitude,false),
                    SOG                 = message.SOG / 10f,
                    COG                 = message.COG / 10f,
                    ShipHeadDirection   = message.TrueHeading.ToString(),
                    TimeStamp           = message.TimeStemp.ToString(),
                    ReceivingDate       = message.ReceivingDate
                }));
                ShipCollection.Add((DicShip[(int)message.UserId]));
            }
            else
            {
                DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                DicShip[(int)message.UserId].AISType             = "Class B";
                DicShip[(int)message.UserId].PositionAccuracyD   = positionAccuracy;
                DicShip[(int)message.UserId].Longtitude          = ConvertCoord(longtitude);
                DicShip[(int)message.UserId].Latitude            = ConvertCoord(latitude,false);
                DicShip[(int)message.UserId].SOG                 = message.SOG / 10f;
                DicShip[(int)message.UserId].COG                 = message.COG / 10f;
                DicShip[(int)message.UserId].ShipHeadDirection   = message.TrueHeading.ToString();
                DicShip[(int)message.UserId].TimeStamp           = message.TimeStemp.ToString();
                DicShip[(int)message.UserId].ReceivingDate       = message.ReceivingDate;
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }
        }
        /// <summary>
        /// 파싱한 messageType 24 을 뷰를 위한 데이터로 변환
        /// </summary>
        public void StaticDataConvert()
        {
            StaticDataMessage message = AISMessageBase as StaticDataMessage;
            
            int partNumber = (int)message.PartNumber;
            if (!DicShip.ContainsKey((int)message.UserId))
            {
                if (partNumber == 0)
                {
                 
                    DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                    {
                        MMSI        = (int)message.UserId,
                        Nationality = AISMessageBase.NationNumber[message.MID][0],
                        ShipName    = AISMessageBase.Convert6bitAscII(message.Name, 20)
                    }));
                    ShipCollection.Add((DicShip[(int)message.UserId]));
                }
                else
                {
                    
                    DicShip.Add((int)message.UserId, new ShipViewModel(new Ship()
                    {
                        MMSI                = (int)message.UserId,
                        Nationality         = AISMessageBase.NationNumber[message.MID][0],
                        MID                 = (int)message.MID,
                        Callsign            = AISMessageBase.Convert6bitAscII(message.CallSign, 7),
                        TypeOfShip          = AISMessageBase.ConvertCargoType((int)message.TypeOfShipAndCargoType),
                        ShipLength          = message.ShipLength,
                        ShipWidth           = message.ShipWidth,
                        PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPostition]

                    }));
                    ShipCollection.Add((DicShip[(int)message.UserId]));
                }
            }
            else
            {
                if (partNumber == 0)
                {

                    DicShip[(int)message.UserId].MMSI        = (int)message.UserId;
                    DicShip[(int)message.UserId].ShipName    = AISMessageBase.Convert6bitAscII(message.Name, 20);
                    
                }
                else
                {

                    DicShip[(int)message.UserId].MMSI                = (int)message.UserId;
                    DicShip[(int)message.UserId].Callsign            = AISMessageBase.Convert6bitAscII(message.CallSign, 7);
                    DicShip[(int)message.UserId].TypeOfShip          = AISMessageBase.ConvertCargoType((int)message.TypeOfShipAndCargoType);
                    DicShip[(int)message.UserId].ShipLength          = message.ShipLength;
                    DicShip[(int)message.UserId].ShipWidth           = message.ShipWidth;
                    DicShip[(int)message.UserId].PositionAccuracy    = AISMessageBase.PositionFixingDevice[(int)message.TypeOfElectronicPostition];
                   
                }
                if (SelectedItem != null)
                {
                    if (message.UserId == SelectedItem.MMSI)
                    {
                        SelectedItem.SetShip(DicShip[(int)message.UserId].GetShip());
                        DetailWindowViewModel.SetShip(DicShip[(int)message.UserId]);
                    }
                }
            }

        }
        #endregion


        internal void Close(bool isClosing = false)
        {
            if (!isClosing)
            {
                MessageParser.Close();
                receiveMessageThread.Abort();
                DetailWindow.Owner = App.Current.MainWindow;
            }
        }
     #endregion

    }
}
