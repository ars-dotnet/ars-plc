using TOPRO.HSL;
using TOPRO.HSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC
{
    internal class OperationManager : IOperationManager
    {
        public IEnumerable<IOperation> Operations;
        public OperationManager(IEnumerable<IOperation> operations)
        {
            this.Operations = operations;
        }

        private IOperation? _operation;

        /// <summary>
        /// 连接id
        /// </summary>
        public string? ConnectionId => _operation?.ConnectionId;

        /// <summary>
        /// 默认tcp初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public OperateResult DefaultConnectionAndInit(DefaultOperationDto input, bool longConnection = true)
        {
            return ConnectionAndInit(input, longConnection, 1);
        }

        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public OperateResult SerialConnectionAndInit(SerialOperationDto input, bool longConnection = true)
        {
            return ConnectionAndInit(input, longConnection);
        }

        /// <summary>
        /// ModbusTcp初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public OperateResult ModbusConnectionAndInit(ModbusOperationDto input, bool longConnection = true)
        {
            return ConnectionAndInit(input, longConnection, 1);
        }

        /// <summary>
        /// ModbusRtu - Ascii初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        public OperateResult ModbusRtuOrAsciiConnectionAndInit(ModbusRtuOperationDto input, bool longConnection = true)
        {
            return ConnectionAndInit(input, longConnection, 1);
        }

        /// <summary>
        /// 欧姆龙初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection"></param>
        /// <returns></returns>
        public OperateResult OmRonConnectionAndInit(OmRonFinsTcpOperationDto input, bool longConnection = true)
        {
            return ConnectionAndInit(input, longConnection, 1);
        }

        /// <summary>
        /// 汇川初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection"></param>
        /// <returns></returns>
        public OperateResult InovanceConnectionAndInit(InovanceOperationDto input, bool longConnection = true)
        {
            return ModbusConnectionAndInit(input, longConnection);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public OperateResult CloseConnection()
        {
            return _operation?.CloseConnection() ?? OperateResult.CreateSuccessResult();
        }

        private OperateResult ConnectionAndInit(OperationDto input, bool longConnection, int? order = null)
        {
            var opt = Operations.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == opt && order.HasValue)
            {
                opt = Operations.FirstOrDefault(
                    r =>
                        r.PlcType == input.PlcType &&
                        r.Order == order);
            }
            if (null == opt)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}操作类未注册");
            }

            opt.PlcType = input.PlcType;
            opt.ProtocolType = input.ProtocolType;
            opt.LongConnection = longConnection;

            _operation = opt;
            _operation.Excuting(input, out bool hasConnected);

            if (hasConnected)
            {
                _operation.ConnectState = ConnectState.Connected;
                return OperateResult.CreateSuccessResult();
            }

            return _operation.Connection();
        }

        #region Read
        public OperateResult<T> Read<T>(Dictionary<string, object> pams)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(string[]))
            {
                return ReadString<T>(ActionNames.GetReadMethodName(typeof(T)), pams);
            }
            else
            {
                return Read<T>(ActionNames.GetReadMethodName(typeof(T)), pams);
            }
        }

        public OperateResult<T> Read<T>(string optName, Dictionary<string, object> pams)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(string[]))
            {
                throw new ArgumentException("读取string类型的参数请调用ReadString方法");
            }
            if (!pams.TryGetValue("address", out object? address) || null == address)
            {
                throw new ArgumentNullException("address");
            }
            pams.TryGetValue("length", out object? length);

            if (null == length || (ushort.TryParse(length.ToString(), out var len) && len == 1))
                return _operation?.Read<T>(
                    address.ToString()!,
                    GetMethodInfo<T>(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string) })
                    ) ?? new OperateResult<T>("未调用连接初始化方法");
            else
                return _operation?.Read<T>(
                    address.ToString()!,
                    ushort.Parse(length.ToString()!),
                    GetMethodInfo<T>(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string), typeof(ushort) })
                    ) ?? new OperateResult<T>("未调用连接初始化方法");
        }

        public OperateResult<T> ReadString<T>(string optName, Dictionary<string, object> pams)
        {
            if (!pams.TryGetValue("address", out object? address) || null == address)
            {
                throw new ArgumentNullException("address");
            }
            pams.TryGetValue("length", out object? length);
            if (null == length)
            {
                throw new ArgumentNullException("length");
            }

            Encoding? encoding = null;
            pams.TryGetValue("encode", out object? encode);
            if (null != encode)
            {
                encoding = (encode as Encoding);
            }

            OperateResult<string>? res = null;
            if (null == encoding)
            {
                res = _operation?.Read<string>(
                    address.ToString()!,
                    ushort.Parse(length.ToString()!),
                    GetMethodInfo<string>(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string), typeof(ushort) })
                    ) ?? new OperateResult<string>("未调用连接初始化方法");
            }
            else
            {
                res = _operation?.Read<string>(
                    address.ToString()!,
                    ushort.Parse(length.ToString()!),
                    encoding,
                    GetMethodInfo<string>(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string), typeof(ushort), typeof(Encoding) })
                    ) ?? new OperateResult<string>("未调用连接初始化方法");
            }

            if (typeof(T) == typeof(string))
            {
                var data = (T)Convert.ChangeType(res.Content?.Replace("\0", ""), typeof(T))!;
                return OperateResult.CreateSuccessResult(data);
            }
            else
            {
                var arrays = res.Content?
                        .Split(new char[] { '\0', ',' })
                        .Where(r => !string.IsNullOrEmpty(r))
                        .ToArray();

                var data = (T)Convert.ChangeType(arrays, typeof(T))!;
                return OperateResult.CreateSuccessResult(data);
            }
        }
        #endregion

        #region Write
        public OperateResult Write<T>(Dictionary<string, object> pams)
        {
            return Write(typeof(T), pams);
        }

        public OperateResult Write(Type datatype, Dictionary<string, object> pams)
        {
            if (datatype == typeof(string) || datatype == typeof(string[]))
            {
                return WriteString(ActionNames.GetWriteMethodName(datatype), datatype, pams);
            }
            else
            {
                return Write(ActionNames.GetWriteMethodName(datatype), datatype, pams);
            }
        }

        public OperateResult Write(string optName, Type datatype, Dictionary<string, object> pams)
        {
            if (datatype == typeof(string) || datatype == typeof(string[]))
            {
                throw new ArgumentException("写入string类型的参数请调用WriteString方法");
            }
            if (!pams.TryGetValue("address", out object? address) || null == address)
            {
                throw new ArgumentNullException("address");
            }
            if (!pams.TryGetValue("value", out object? value) || null == value)
            {
                throw new ArgumentNullException("value");
            }

            return _operation?.Write(
                address.ToString()!,
                Convert.ChangeType(value, datatype),
                GetMethodInfo(
                    _operation.ReadWriteNet.GetType(),
                    optName,
                    new Type[] { typeof(string), datatype },
                    datatype)
                ) ?? new OperateResult("未调用连接初始化方法");
        }

        public OperateResult WriteString(string optName, Type datatype, Dictionary<string, object> pams)
        {
            if (datatype != typeof(string) && datatype != typeof(string[]))
            {
                throw new ArgumentException("泛型只支持string和string[]");
            }
            if (!pams.TryGetValue("address", out object? address) || null == address)
            {
                throw new ArgumentNullException("address");
            }
            if (!pams.TryGetValue("value", out object? value) || null == value)
            {
                throw new ArgumentNullException("value");
            }

            Encoding? encoding = null;
            pams.TryGetValue("encode", out object? encode);
            if (null != encode)
            {
                encoding = encode as Encoding;
            }

            if (datatype == typeof(string[]))
            {
                string[] values = (string[])value;
                value = string.Join(",", values);
            }

            if (null == encoding)
            {
                return _operation?.Write(
                    address.ToString()!,
                    (string)value,
                    GetMethodInfo(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string), typeof(string) },
                        typeof(string))
                    ) ?? new OperateResult("未调用连接初始化方法");
            }
            else
            {
                return _operation?.Write(
                    address.ToString()!,
                    (string)value,
                    encoding,
                    GetMethodInfo(
                        _operation.ReadWriteNet.GetType(),
                        optName,
                        new Type[] { typeof(string), typeof(string), typeof(Encoding) },
                        typeof(string))
                    ) ?? new OperateResult("未调用连接初始化方法");
            }
        }
        #endregion

        public void Dispose()
        {
            _operation?.Excuted();
        }

        private MethodInfo GetMethodInfo<T>(Type type, string methodName, Type[] paramsTypes)
        {
            return GetMethodInfo(type, methodName, paramsTypes, typeof(T));
        }

        private MethodInfo GetMethodInfo(Type type, string methodName, Type[] paramsTypes, Type genericType)
        {
            var method = type.GetMethod(methodName, paramsTypes);
            if (null == method)
                throw new Exception($"{type.Name}中不存在方法:{methodName}");
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(genericType);

            return method;
        }
    }
}
