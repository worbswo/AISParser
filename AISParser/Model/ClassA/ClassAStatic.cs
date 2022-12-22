using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Model.ClassA
{
	public class ClassAStatic : AISStatic
	{
		public int IMO { get; set; }
		public string Callsign { get; set; }
		public float ShipLength { get; set; }
		public float ShipWidth { get; set; }
		public string TypeOfShip { get; set; } = "";
		public float TargetDraft { get; set; }
		public string DestinationPort { get; set; } = "";
		public string ETA { get; set; } = "";
	}
}
