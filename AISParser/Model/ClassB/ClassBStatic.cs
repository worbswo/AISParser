using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Model.ClassB
{
	public class ClassBStatic : AISStatic
	{
		public string Callsign { get; set; }
		public float ShipLength { get; set; }
		public float ShipWidth { get; set; }
		public string TypeOfShip { get; set; } = "";

	}
}
