using TOPRO.HSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 西门子S7操作类
    /// </summary>
    public class SiemensS7NetOperation : BaseTcpOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.DefaultTcp;

        public SiemensS7NetOperation(
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            : base(plcProvider, netSchemeProvider)
        {
            PlcType = PlcType.Siemens;
            ProtocolType = ProtocolType.S7_S1500;
            Order = 1;
        }
    }
}
