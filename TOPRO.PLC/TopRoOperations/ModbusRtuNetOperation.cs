using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Dtos;
using TOPRO.HSL.Core;
using TOPRO.HSL.Core.Net;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// modbusrtu串口操作类
    /// </summary>
    internal class ModbusRtuNetOperation : BaseSerialOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.ModBusRtuorAscii;
        
        public ModbusRtuNetOperation(
            IEnumerable<IPlcProvider> plcProvider, 
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            : base(plcProvider, netSchemeProvider)
        {
            PlcType = PlcType.Modbus;
            ProtocolType = ProtocolType.Modbus_Rtu;

            Order = 1;
        }

        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null)
        {
            ModbusRtuOperationDto dto = (ModbusRtuOperationDto)input;

            var scheme = new TopRoModBusRtuScheme
            {
                NetOperation = netOperation,

                PortName = dto.PortName,
                BaudRate = dto.BaudRate,
                DataBits = dto.DataBits,
                StopBits = dto.StopBits,
                Parity = dto.Parity,
                Station = dto.Station,

                PlcType = PlcType,
                ProtocolType = ProtocolType
            };

            return scheme;
        }
    }
}
