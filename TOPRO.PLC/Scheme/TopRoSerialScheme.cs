using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public class TopRoSerialScheme : BaseTopRoNetScheme, ITopRoSerialScheme
    {
        public TopRoSerialScheme()
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.Serial;
        }

        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public int StopBits { get; set; }

        public int Parity { get; set; }
    }
}
