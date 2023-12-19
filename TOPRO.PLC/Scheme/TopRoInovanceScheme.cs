using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Scheme;

namespace TOPRO.PLC.Scheme
{
    internal class TopRoInovanceScheme : TopRoModBusScheme
    {
        public TopRoInovanceScheme() : base()
        {
            PlcProtocolLevel = Enums.PlcProtocolLevel.InovanceTcp;
        }
    }
}
