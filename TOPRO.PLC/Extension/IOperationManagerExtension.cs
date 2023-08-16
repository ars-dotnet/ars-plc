using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC.Extension
{
    public static class IOperationManagerExtension
    {
        /// <summary>
        /// 基本读
        /// 不支持modbus线圈读和离散读
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operationManager"></param>
        /// <param name="address">PLC地址</param>
        /// <param name="length">点位长度</param>
        /// <param name="encode">编码格式，只用于string类型读取，默认是ASCII</param>
        /// <returns></returns>
        public static OperateResult<T> Read<T>(this IOperationManager operationManager,string address, ushort length = 1, Encoding? encode = null)
        {
            return operationManager.Read<T>(
                new Dictionary<string, object> 
                { 
                    { nameof(address), address },
                    { nameof(length), length },
                    { nameof(encode), encode! }
                });
        }

        /// <summary>
        /// 基本写
        /// 不支持modbus线圈写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operationManager"></param>
        /// <param name="address">PLC地址</param>
        /// <param name="value">写入值</param>
        /// <param name="encode">编码格式，只用于string类型写入，默认是ASCII</param>
        /// <returns></returns>
        public static OperateResult Write<T>(this IOperationManager operationManager, string address, T value,Encoding? encode = null) 
        {
            return operationManager.Write<T>(
                new Dictionary<string, object> 
                { 
                    { nameof(address),address},
                    { nameof(value),value!}, 
                    { nameof(encode), encode! }
                });
        }

        /// <summary>
        /// 基本写
        /// 不支持modbus线圈写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operationManager"></param>
        /// <param name="address">PLC地址</param>
        /// <param name="value">写入值</param>
        /// <param name="valueType">值类型</param>
        /// <param name="encode">编码格式，只用于string类型写入，默认是ASCII</param>
        /// <returns></returns>
        public static OperateResult Write(this IOperationManager operationManager, string address, object value, Type valueType, Encoding? encode = null)
        {
            return operationManager.Write(
                valueType, 
                new Dictionary<string, object> 
                { 
                    { nameof(address), address },
                    { nameof(value), value },
                    { nameof(encode), encode! }
                });
        }
    }
}
