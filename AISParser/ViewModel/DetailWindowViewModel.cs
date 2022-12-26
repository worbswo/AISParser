using AISParser.Code.Message;
using AISParser.Code.Parser;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.ViewModel
{
	sealed public class DetailWindowViewModel : ViewModelBase
    {

        #region Field
        #endregion

        #region Property
        public string NationImageTxt
        {
            get
            {
				return GetValue<string>("NationImageTxt","kr.png");
            }
            set
            {
				SetValue<string>("NationImageTxt", value);
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
