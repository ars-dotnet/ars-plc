using TOPRO.HSL.Core;
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
    internal class ModbusNetOperation : BaseOperation
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

        public override void Excuting(OperationDto input, out bool hasConnected)
        {
            ModbusOperationDto dto = (ModbusOperationDto)input;
            if (!System.Net.IPAddress.TryParse(dto.IpAddress, out var _))
            {
                throw new Exception("Ip地址输入不正确！");
            }

            hasConnected = false;
            if (LongConnection)
            {
                ITopRoNetScheme? scheme =
                    _netSchemeProvider.GetScheme(
                        new TopRoModBusScheme
                        {
                            IpAddress = dto.IpAddress,
                            Port = dto.Port,
                            Station = dto.Station,
                            PlcType = PlcType,
                            ProtocolType = ProtocolType
                        });

                if (null != scheme)
                {
                    TopRoNetScheme = scheme;
                    hasConnected = true;
                }
                else
                {
                    Init(input);
                }
            }
            else 
            {
                Init(input);
            }
        }

        protected override void Init(OperationDto input)
        {
            INetOperation opt = _provider.Resolve(input);
            ModbusOperationDto dto = (ModbusOperationDto)input;

            TopRoNetScheme = new TopRoModBusScheme
            {
                NetOperation = opt,
                IpAddress = dto.IpAddress,
                Port = dto.Port,
                Station = dto.Station,
                PlcType = PlcType,
                ProtocolType = ProtocolType
            };
        }
    }
}
