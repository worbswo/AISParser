using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.TCP
{
    public class AISData
    {
        #region Property
        public byte[] Data { get; set; } = new byte[0];
        public int Length { get; set; } = new int();
        #endregion
    }
}
