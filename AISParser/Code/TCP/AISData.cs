using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.TCP
{
    public class AISData
    {
        #region Property
        internal byte[] Data { get; set; } = new byte[0];
        public int Length { get; set; } = new int();
		#endregion

		#region Constructor
		public AISData(byte[] date,int size)
		{
			Data = date;
			Length = size;	
		}
		#endregion
		public byte this[int index]
		{
			get
			{
				return Data[index];
			}
			set
			{
				Data[index] = value;
			}
		}
	}
}
