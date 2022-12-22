using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Model
{
	public class AISBase
	{
		public int MID { get; set; } = 999;
		public string Status { get; set; } = "";
		public string Nationality { get; set; } = "";
		public string AISType { get; set; } = "";
		public int MMSI { get; set; }
		public string PositionAccuracy { get; set; } = "";
		public string ReceivingDate { get; set; } = "";

		public AISDynamic AISDynamic { get; set; }
		public AISStatic AISStatic { get; set; }

	}
}
