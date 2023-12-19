using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Scheme;

namespace TOPRO.PLC.Scheme.Provider
{
    internal class TopRoNetInovanceSchemeProvider : TopRoNetModbusSchemeProvider
    {
        public TopRoNetInovanceSchemeProvider(ITopRoSchemeInstance instance) : base(instance)
        {
            PlcProtocolLevel = Enums.PlcProtocolLevel.InovanceTcp;
        }
    }
}
