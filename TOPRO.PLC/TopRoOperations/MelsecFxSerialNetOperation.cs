using TOPRO.HSL;
using TOPRO.HSL.Core;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 三菱串口操作类
    /// </summary>
    internal class MelsecFxSerialNetOperation : BaseSerialOperation
    {
        public MelsecFxSerialNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider) 
            : base(plcProvider,netSchemeProvider)
        {
            PlcType = PlcType.MelSec;

            ProtocolType = ProtocolType.Serial;

            Order = 2;
        }
    }
}
