using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public class TopRoNetDefaultSchemeProvider : BaseTopRoNetSchemeProvider
    {
        public TopRoNetDefaultSchemeProvider(
            ITopRoSchemeInstance instance)
            : base(instance)
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.DefaultTcp;
        }

        public override ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme)
        {
            ITopRoDefaultScheme topRo = (ITopRoDefaultScheme)netScheme;
            return _instance.Instance.
                Where(r => r.PlcProtocolLevel == PlcProtocolLevel).
                Select(r => (ITopRoDefaultScheme)r).
                Where(r =>
                    r.IpAddress == topRo.IpAddress &&
                    r.Port == topRo.Port &&
                    r.PlcType == topRo.PlcType &&
                    r.ProtocolType == topRo.ProtocolType).
                    FirstOrDefault();
        }
    }
}
