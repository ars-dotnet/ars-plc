using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TOPRO.HSL;
using TOPRO.HSL.Core;
using TOPRO.HSL.Core.Net;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 操作父类
    /// </summary>
    public abstract class BaseOperation : IOperation
    {
        public BaseOperation(
             IEnumerable<IPlcProvider> plcProvider,
             IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
        {
            _provider = plcProvider.FirstOrDefault(r => r.PlcProtocolLevel == PlcProtocolLevel)
                ?? throw new NullReferenceException($"{PlcProtocolLevel}:plc提供者实例不存在");

            _netSchemeProvider = netSchemeProvider.FirstOrDefault(r => r.PlcProtocolLevel == PlcProtocolLevel)
                ?? throw new NullReferenceException($"{PlcProtocolLevel}:scheme提供者实例不存在");
        }

        protected abstract PlcProtocolLevel PlcProtocolLevel { get; }

        protected readonly IPlcProvider _provider;

        protected readonly ITopRoNetSchemeProvider _netSchemeProvider;

        /// <summary>
        /// 值=1表示默认实例
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// 是否长连接
        /// </summary>
        public bool LongConnection { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public ConnectState ConnectState { get; set; }

        /// <summary>
        /// PLC类型
        /// </summary>
        public PlcType PlcType { get; set; }

        /// <summary>
        /// PLC协议类型
        /// </summary>
        public ProtocolType ProtocolType { get; set; }

        /// <summary>
        /// scheme
        /// </summary>
        protected ITopRoNetScheme TopRoNetScheme { get; set; }

        /// <summary>
        /// 读写scheme
        /// </summary>
        public IReadWriteNet ReadWriteNet => (IReadWriteNet)TopRoNetScheme.NetOperation;

        /// <summary>
        /// 连接scheme
        /// </summary>
        protected INetConnection NetConnection => (INetConnection)TopRoNetScheme.NetOperation;

        /// <summary>
        /// 连接id
        /// </summary>
        public string? ConnectionId => NetConnection?.ConnectionId;

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <returns></returns>
        public abstract OperateResult Connection();

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public abstract OperateResult CloseConnection();

        /// <summary>
        /// 操作前执行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hasConnected"></param>
        public virtual void Excuting(OperationDto input, out bool hasConnected) 
        {
            Check(input);

            hasConnected = false;

            if (LongConnection)
            {
                ITopRoNetScheme? scheme =
                    _netSchemeProvider.GetScheme(GetTopRoNetScheme(input));

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

            if (!LongConnection)
            {
                Init(input);
            }
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

        public virtual OperateResult<T> Read<T>(string address, ushort length, Encoding encode, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return OperateResult.CreateFailedResult<T>(new OperateResult("未连接"));

            return PlcPollly.PlcRetry(() => (OperateResult<T>)method.Invoke(ReadWriteNet, new object[] { address, length, encode })!);
        }
        #endregion

        #region 写入
        public virtual OperateResult Write<T>(string address, T value, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return new OperateResult("未连接");

            return PlcPollly.PlcRetry(() => (OperateResult)method.Invoke(ReadWriteNet, new object[] { address, value! })!);
        }

        public virtual OperateResult Write<T>(string address, T value, Encoding encode, MethodInfo method)
        {
            if (ConnectState != ConnectState.Connected)
                return new OperateResult("未连接");

            return PlcPollly.PlcRetry(() => (OperateResult)method.Invoke(ReadWriteNet, new object[] { address, value!, encode })!);
        }
        #endregion


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="input"></param>
        protected virtual void Check(OperationDto input) 
        {
            return;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="input"></param>
        protected virtual void Init(OperationDto input) 
        {
            TopRoNetScheme = GetTopRoNetScheme(input, _provider.Resolve(input));
        }

        /// <summary>
        /// 获取scheme
        /// </summary>
        /// <param name="input"></param>
        /// <param name="netOperation"></param>
        /// <returns></returns>
        protected abstract ITopRoNetScheme GetTopRoNetScheme(OperationDto input, INetOperation? netOperation = null);
    }
}
