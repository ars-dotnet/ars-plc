using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public class TopRoNetOmRonSchemeProvider : BaseTopRoNetSchemeProvider
    {
        public TopRoNetOmRonSchemeProvider(ITopRoSchemeInstance instance)
            : base(instance)
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.OmRonFinsTcp;
        }

        public override ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme)
        {
            TopRoOmRonFinsTcpScheme topRo = (TopRoOmRonFinsTcpScheme)netScheme;
            return _instance.Instance.
                Where(r => r.PlcProtocolLevel == PlcProtocolLevel).
                Select(r => (TopRoOmRonFinsTcpScheme)r).
                Where(r =>
                    r.IpAddress == topRo.IpAddress &&
                    r.Port == topRo.Port &&
                    r.SA1 == topRo.SA1 &&
                    r.DA1 == topRo.DA1 &&
                    r.DA2 == topRo.DA2 &&
                    r.PlcType == topRo.PlcType &&
                    r.ProtocolType == topRo.ProtocolType).
                    FirstOrDefault();
        }
    }
}
