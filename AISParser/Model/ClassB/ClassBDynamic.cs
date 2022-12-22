using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Model.ClassB
{
	public class ClassBDynamic : AISDynamic
	{
		public float SOG { get; set; }
		public float COG { get; set; }
		public string ShipHeadDirection { get; set; } = "";
		public string ROT { get; set; } = "";

	}
}
