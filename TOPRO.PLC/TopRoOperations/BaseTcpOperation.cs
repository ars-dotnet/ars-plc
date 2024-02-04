using TOPRO.HSL;
using TOPRO.HSL.Core;
using TOPRO.HSL.Core.IMessage;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Concurrent;
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
    /// TCP连接的操作父类
    /// </summary>
    public abstract class BaseTcpOperation : BaseOperation
    {
        public BaseTcpOperation(
             IEnumerable<IPlcProvider> plcProvider,
             IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            : base(plcProvider, netSchemeProvider)
        {

        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="Exception"></exception>
        protected override void Check(OperationDto input) 
        {
            DefaultOperationDto dto = (DefaultOperationDto)input;
            if (!System.Net.IPAddress.TryParse(dto.IpAddress, out var _))
            {
                throw new Exception("Ip地址输入不正确！");
            }
        }

        /// <summary>
        /// 获取scheme
        /// </summary>
        /// <param name="input"></param>
        /// <param name="netOperation"></param>
        /// <returns></returns>
        protected override ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null) 
        {
            DefaultOperationDto dto = (DefaultOperationDto)input;

            TopRoDefaultScheme scheme = new TopRoDefaultScheme
            {
                NetOperation = netOperation,

                IpAddress = dto.IpAddress,
                Port = dto.Port,

                PlcType = PlcType,
                ProtocolType = ProtocolType
            };

            return scheme;
        }

        public override OperateResult Connection() 
        {
            var res = PlcPollly.PlcRetry(() => NetConnection.ConnectServer());
            if (res.IsSuccess) 
            {
                _netSchemeProvider.SetScheme(TopRoNetScheme);

                ConnectState = ConnectState.Connected;
            }

            return res;
        }

        public override OperateResult CloseConnection()
        {
            var res =  PlcPollly.PlcRetry(() => NetConnection.ConnectClose());
            if (res.IsSuccess) 
            {
                _netSchemeProvider.RemoveScheme(TopRoNetScheme);

                ConnectState = ConnectState.Disconnected;
            }

            return res;
        }
    }
}
