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
    public abstract class BaseOperation : IOperation
    {
        public int Order { get; protected set; }

        public bool LongConnection { get; set; } = true;

        public ConnectState ConnectState { get; set; }

        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }

        protected ITopRoNetScheme TopRoNetScheme { get; set; }

        public IReadWriteNet ReadWriteNet => (IReadWriteNet)TopRoNetScheme.NetOperation;

        protected INetConnection NetConnection => (INetConnection)TopRoNetScheme.NetOperation;

        protected abstract PlcProtocolLevel PlcProtocolLevel { get; }

        protected readonly IPlcProvider _provider;

        protected readonly ITopRoNetSchemeProvider _netSchemeProvider;

        public BaseOperation(
             IEnumerable<IPlcProvider> plcProvider,
             IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
        {
            _provider = plcProvider.FirstOrDefault(r => r.PlcProtocolLevel == PlcProtocolLevel) 
                ?? throw new NullReferenceException($"{PlcProtocolLevel}:plc提供者实例不存在");
            _netSchemeProvider = netSchemeProvider.FirstOrDefault(r => r.PlcProtocolLevel == PlcProtocolLevel)
                ?? throw new NullReferenceException($"{PlcProtocolLevel}:scheme提供者实例不存在");
        }

        /// <summary>
        /// 操作前执行
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public virtual void Excuting(OperationDto input,out bool hasConnected)
        {
            DefaultOperationDto dto = (DefaultOperationDto)input;
            if (!System.Net.IPAddress.TryParse(dto.IpAddress, out var _))
            {
                throw new Exception("Ip地址输入不正确！");
            }

            hasConnected = false;
            if (LongConnection)
            {
                ITopRoNetScheme? scheme = 
                    _netSchemeProvider.GetScheme(
                            new TopRoDefaultScheme
                            {
                                IpAddress = dto.IpAddress,
                                Port = dto.Port,
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

            if(!LongConnection) 
            {
                Init(input);
            }
        }

        protected virtual void Init(OperationDto input) 
        {
            INetOperation opt = _provider.Resolve(input);

            DefaultOperationDto dto = (DefaultOperationDto)input;
            TopRoNetScheme = new TopRoDefaultScheme
            {
                NetOperation = opt,
                IpAddress = dto.IpAddress,
                Port = dto.Port,
                PlcType = PlcType,
                ProtocolType = ProtocolType
            };
        }

        /// <summary>
        /// 操作后执行
        /// </summary>
        public virtual void Excuted()
        {
            if (!LongConnection) 
            {
                CloseConnection();
            }
        }

        public virtual OperateResult Connection() 
        {
            var res = PlcPollly.PlcRetry(() => NetConnection.ConnectServer());
            if (res.IsSuccess) 
            {
                _netSchemeProvider.SetScheme(TopRoNetScheme);
                ConnectState = ConnectState.Connected;
            }

            return res;
        }

        public virtual OperateResult CloseConnection()
        {
            var res =  PlcPollly.PlcRetry(() => NetConnection.ConnectClose());
            if (res.IsSuccess) 
            {
                _netSchemeProvider.RemoveScheme(TopRoNetScheme);
                ConnectState = ConnectState.Disconnected;
            }

            return res;
        }

        #region 读取
        public virtual OperateResult<T> Read<T>(string address, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return OperateResult.CreateFailedResult<T>(new OperateResult("未连接"));

            return PlcPollly.PlcRetry(() => (OperateResult<T>)method.Invoke(ReadWriteNet, new object[] { address })!);
        }

        public virtual OperateResult<T> Read<T>(string address, ushort length, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return OperateResult.CreateFailedResult<T>(new OperateResult("未连接"));

            return PlcPollly.PlcRetry(() => (OperateResult<T>)method.Invoke(ReadWriteNet, new object[] { address, length })!);
        }

        public virtual OperateResult<T> Read<T>(string address, ushort length,Encoding encode, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return OperateResult.CreateFailedResult<T>(new OperateResult("未连接"));

            return PlcPollly.PlcRetry(() => (OperateResult<T>)method.Invoke(ReadWriteNet, new object[] { address, length,encode })!);
        }

        #endregion

        #region 写入
        public virtual OperateResult Write<T>(string address, T value, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return new OperateResult("未连接");

            return PlcPollly.PlcRetry(() => (OperateResult)method.Invoke(ReadWriteNet, new object[] { address, value! })!);
        }

        public virtual OperateResult Write<T>(string address, T value,Encoding encode, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return new OperateResult("未连接");

            return PlcPollly.PlcRetry(() => (OperateResult)method.Invoke(ReadWriteNet, new object[] { address, value!, encode })!);
        }

        #endregion
    }
}
