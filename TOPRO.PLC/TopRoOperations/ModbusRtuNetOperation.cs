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
    internal class ModbusRtuNetOperation : MelsecFxSerialNetOperation
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

        public override void Excuting(OperationDto input, out bool hasConnected)
        {
            ModbusRtuOperationDto dto = (ModbusRtuOperationDto)input;

            hasConnected = false;
            if (LongConnection)
            {
                ITopRoNetScheme? scheme = 
                    _netSchemeProvider.GetScheme(
                        new TopRoModBusRtuScheme
                        {
                            PortName = dto.PortName,
                            BaudRate = dto.BaudRate,
                            DataBits = dto.DataBits,
                            StopBits = dto.StopBits,
                            Parity = dto.Parity,

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
            ModbusRtuOperationDto dto = (ModbusRtuOperationDto)input;
            TopRoNetScheme = new TopRoModBusRtuScheme
            {
                NetOperation = opt,
                PortName = dto.PortName,
                BaudRate = dto.BaudRate,
                DataBits = dto.DataBits,
                StopBits = dto.StopBits,
                Parity = dto.Parity,
                Station = dto.Station,
                PlcType = PlcType,
                ProtocolType = ProtocolType
            };

            SerialNetOperation.SerialPortInni(sp =>
            {
                sp.PortName = dto.PortName;
                sp.BaudRate = dto.BaudRate;
                sp.DataBits = dto.DataBits;
                sp.StopBits = dto.StopBits == 0
                    ? System.IO.Ports.StopBits.None
                    : dto.StopBits == 1
                        ? System.IO.Ports.StopBits.One
                        : System.IO.Ports.StopBits.Two;
                sp.Parity = dto.Parity == 0
                    ? System.IO.Ports.Parity.None
                    : dto.Parity == 1
                        ? System.IO.Ports.Parity.Odd
                        : System.IO.Ports.Parity.Even;
            });
        }
    }
}
