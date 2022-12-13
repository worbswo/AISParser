using AISParser.Code.Message;
using AISParser.Code.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.ViewModel
{
    public class DetailWindowViewModel : ViewModelBase
    {

        #region Field
        private string _nationImageTxt;
        #endregion

        #region Property
        public string NationImageTxt
        {
            get
            {
                if(_nationImageTxt== null)
                {
                    _nationImageTxt = "kr.png";
                }
                return _nationImageTxt;
            }
            set
            {
                _nationImageTxt = value;
                OnPropertyChanged("NationImageTxt");
            }
        }
        public ShipViewModel ShipViewModel { get; set; } = new ShipViewModel();
        #endregion

        #region Constructor
        public void SetShip(ShipViewModel shipViewModel)
        {
            ShipViewModel.SetShip(shipViewModel.GetShip());
            NationImageTxt = AISMessageBase.NationNumber[ShipViewModel.GetShip().MID][1] + ".png";
        }
        #endregion

        #region Command

        #endregion 

        #region Method
        internal void Close(bool isClosing = false)
        {
            if (!isClosing)
            {
                base.Close();
            }
        }
        #endregion

    }
}
