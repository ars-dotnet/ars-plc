using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public class TopRoNetModbusRtuSchemeProvider : BaseTopRoNetSchemeProvider
    {
        public TopRoNetModbusRtuSchemeProvider(
            ITopRoSchemeInstance instance)
            : base(instance)
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.ModBusRtuorAscii;
        }

        public override ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme)
        {
            ITopRoModBusRtuScheme topRo = (ITopRoModBusRtuScheme)netScheme;
            return _instance.Instance.
                Where(r => r.PlcProtocolLevel == PlcProtocolLevel).
                Select(r => (ITopRoModBusRtuScheme)r).
                Where(r =>
                    r.Station == topRo.Station &&
                    r.PortName == topRo.PortName &&
                    r.BaudRate == topRo.BaudRate &&
                    r.DataBits == topRo.DataBits &&
                    r.StopBits == topRo.StopBits &&
                    r.Parity == topRo.Parity &&
                    r.PlcType == topRo.PlcType &&
                    r.ProtocolType == topRo.ProtocolType).
                    FirstOrDefault();
        }
    }
}
