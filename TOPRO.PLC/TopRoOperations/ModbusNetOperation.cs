﻿using TOPRO.HSL.Core;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// modbustcp操作类
    /// </summary>
    internal class ModbusNetOperation : BaseTcpOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.ModBusTcp;

        public ModbusNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider) 
            : base(plcProvider, netSchemeProvider)
        {
            PlcType = PlcType.Modbus;
            ProtocolType = ProtocolType.Modbus_Tcp;
        }

        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null) 
        {
            ModbusOperationDto dto = (ModbusOperationDto)input;
            var scheme = new TopRoModBusScheme
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
