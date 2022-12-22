using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code
{
	sealed public class MySortDescription
    {
        #region Property

        Dictionary<string, SortDescription> Sort { get; set; }

		public SortDescription this[string index]
		{
			get
			{
				return Sort[index];
			}
			set
			{
				if (Sort.ContainsKey(index))
				{
					Sort[index] = value;
				}
			}
		}

		#endregion

		#region Constructor

		public MySortDescription(ListSortDirection listSortDirection)
        {
            Sort = new Dictionary<string, SortDescription>()
            {
                { "ReceivingDate",  new SortDescription("ReceivingDate", listSortDirection) },
                { "MMSI",           new SortDescription("MMSI", listSortDirection) },
                { "AISType",        new SortDescription("AISType", listSortDirection) },
                { "Status",         new SortDescription("Status", listSortDirection) },
                { "ShipName",       new SortDescription("ShipName", listSortDirection) },
                { "UserShipName",   new SortDescription("UserShipName", listSortDirection) },
                { "Latitude",       new SortDescription("Latitude", listSortDirection) },
                { "Longtitude",     new SortDescription("Longtitude", listSortDirection) },
                { "SOG",            new SortDescription("SOG", listSortDirection) },
                { "COG",            new SortDescription("COG", listSortDirection) },
            };
        }
      
        #endregion
    }
}
