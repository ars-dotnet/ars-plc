using TOPRO.HSL;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;
using TOPRO.PLC;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;
using TOPRO.PLC.TopRoOperations;

namespace Topro.Extension.Plc.TopRoOperations
{
    /// <summary>
    /// omron操作类
    /// </summary>
    internal class OmRonFinsTcpNetOperation : BaseTcpOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.OmRonFinsTcp;

        public OmRonFinsTcpNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
             IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            :base(plcProvider,netSchemeProvider)
        {
            PlcType = PlcType.OmRon;
            ProtocolType = ProtocolType.FinsTcp;
        }

        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null)
        {
            OmRonFinsTcpOperationDto dto = (OmRonFinsTcpOperationDto)input;

            var scheme = new TopRoOmRonFinsTcpScheme
            {
                NetOperation = netOperation,

                IpAddress = dto.IpAddress,
                Port = dto.Port,

                SA1 = dto.SA1,
                DA1 = dto.DA1,
                DA2 = dto.DA2,

                PlcType = PlcType,
                ProtocolType = ProtocolType,
            };

            return scheme;
        }
    }
}
