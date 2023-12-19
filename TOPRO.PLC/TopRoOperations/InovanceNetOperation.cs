using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;
using TOPRO.HSL.Core.Net;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 汇川操作类
    /// </summary>
    internal class InovanceNetOperation : ModbusNetOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.InovanceTcp;

        public InovanceNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            : base(plcProvider, netSchemeProvider)
        {
            PlcType = PlcType.Inovance;

            //汇川走的modbus tcp连接
            ProtocolType = ProtocolType.Modbus_Tcp;
        }

        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null)
        {
            InovanceOperationDto dto = (InovanceOperationDto)input;
            var scheme = new TopRoInovanceScheme
            {
                NetOperation = netOperation,

                IpAddress = dto.IpAddress,
                Port = dto.Port,
                Station = dto.Station,

                PlcType = PlcType,
                ProtocolType = ProtocolType
            };

            return scheme;
        }
    }
}
