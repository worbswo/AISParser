using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Message.InternationalFunctionMessage
{
    public interface InternationalFunctionMessageBase
    {
        void Parsing(byte[] message);
    }
}
