using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    internal class TopRoDefaultScheme : BaseTopRoNetScheme,ITopRoDefaultScheme
    {
        public TopRoDefaultScheme()
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.DefaultTcp;
        }

        public string IpAddress { get; set; }

        public int Port { get; set; }
    }
}
