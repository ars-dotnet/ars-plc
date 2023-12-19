using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;
using TOPRO.HSL;
using TOPRO.HSL.Core.Net;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 串口连接的操作父类
    /// </summary>
    public abstract class BaseSerialOperation : BaseOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.Serial;

        protected ISerialNetOperation? SerialNetOperation => (ISerialNetOperation)TopRoNetScheme.NetOperation;

        protected BaseSerialOperation(
            IEnumerable<IPlcProvider> plcProvider, 
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider) 
            : base(plcProvider, netSchemeProvider)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="input"></param>
        protected override void Init(OperationDto input)
        {
            SerialOperationDto dto = (SerialOperationDto)input;

            TopRoNetScheme = GetTopRoNetScheme(input, _provider.Resolve(input));

            SerialNetOperation?.SerialPortInni(sp =>
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

        /// <summary>
        /// 获取scheme
        /// </summary>
        /// <param name="input"></param>
        /// <param name="netOperation"></param>
        /// <returns></returns>
        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null)
        {
            SerialOperationDto dto = (SerialOperationDto)input;

            var scheme = new TopRoSerialScheme
            {
                NetOperation = netOperation,

                PortName = dto.PortName,
                BaudRate = dto.BaudRate,
                DataBits = dto.DataBits,
                StopBits = dto.StopBits,
                Parity = dto.Parity,

                PlcType = PlcType,
                ProtocolType = ProtocolType
            };

            return scheme;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override OperateResult Connection()
        {
            try
            {
                PlcPollly.ExceptionRetry(() => SerialNetOperation?.Open());

                ConnectState = ConnectState.Connected;

                _netSchemeProvider.SetScheme(TopRoNetScheme);
            }
            catch (Exception e)
            {
                return new OperateResult(1, e.Message);
            }

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public override OperateResult CloseConnection()
        {
            try
            {
                PlcPollly.ExceptionRetry(() => SerialNetOperation?.Close());

                _netSchemeProvider.RemoveScheme(TopRoNetScheme!);

                TopRoNetScheme = default;

                ConnectState = ConnectState.Disconnected;
            }
            catch (Exception e)
            {
                return new OperateResult(1, e.Message);
            }

            return OperateResult.CreateSuccessResult();
        }
    }
}
