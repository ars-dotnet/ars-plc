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
    internal class MelsecFxSerialNetOperation : BaseOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.Serial;

        protected ISerialNetOperation SerialNetOperation => (ISerialNetOperation)TopRoNetScheme.NetOperation;

        public MelsecFxSerialNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider) 
            : base(plcProvider,netSchemeProvider)
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.Serial;
            Order = 2;
        }

        public override void Excuting(OperationDto input, out bool hasConnected)
        {
            SerialOperationDto dto = (SerialOperationDto)input;
            hasConnected = false;
            if (LongConnection)
            {
                ITopRoNetScheme? scheme = 
                    _netSchemeProvider.GetScheme(
                        new TopRoSerialScheme
                        {
                            PortName = dto.PortName,
                            BaudRate = dto.BaudRate,
                            DataBits = dto.DataBits,
                            StopBits = dto.StopBits,
                            Parity = dto.Parity,

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
            SerialOperationDto dto = (SerialOperationDto)input;
            TopRoNetScheme = new TopRoSerialScheme
            {
                NetOperation = opt,
                PortName = dto.PortName,
                BaudRate = dto.BaudRate,
                DataBits = dto.DataBits,
                StopBits = dto.StopBits,
                Parity = dto.Parity,
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

        public override void Excuted()
        {
            if (!LongConnection) 
            {
                CloseConnection();
            }
        }

        public override OperateResult Connection()
        {
            try
            {
                PlcPollly.ExceptionRetry(() => SerialNetOperation.Open());

                ConnectState = ConnectState.Connected;
                _netSchemeProvider.SetScheme(TopRoNetScheme);
            }
            catch (Exception e) 
            {
                return new OperateResult(1, e.Message);
            }

            return OperateResult.CreateSuccessResult();
        }

        public override OperateResult CloseConnection()
        {
            try
            {
                PlcPollly.ExceptionRetry(() => SerialNetOperation.Close());

                ConnectState = ConnectState.Disconnected;
                _netSchemeProvider.RemoveScheme(TopRoNetScheme);
            }
            catch (Exception e) 
            {
                return new OperateResult(1,e.Message);
            }

            return OperateResult.CreateSuccessResult();
        }
    }
}
