using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public class TopRoNetModbusSchemeProvider : BaseTopRoNetSchemeProvider
    {
        public TopRoNetModbusSchemeProvider(ITopRoSchemeInstance instance) : base(instance)
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.ModBusTcp;
        }

        public override ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme)
        {
            ITopRoModBusScheme topRo = (ITopRoModBusScheme)netScheme;
            return _instance.Instance.
                Where(r => r.PlcProtocolLevel == PlcProtocolLevel).
                Select(r => (ITopRoModBusScheme)r).
                Where(r =>
                    r.IpAddress == topRo.IpAddress &&
                    r.Port == topRo.Port &&
                    r.Station == topRo.Station &&
                    r.PlcType == topRo.PlcType &&
                    r.ProtocolType == topRo.ProtocolType).
                    FirstOrDefault();
        }
    }
}
