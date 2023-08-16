using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslCommunication.Core.Net
{
    public interface IModbusReadWriteNet : IReadWriteNet
    {
        /// <summary>
        /// 读取线圈，需要指定起始地址
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <returns></returns>
        public OperateResult<bool> ReadCoil(string address);

        /// <summary>
        /// 批量的读取线圈，需要指定起始地址，读取长度
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public OperateResult<bool[]> ReadCoil(string address, ushort length);

        /// <summary>
        /// 离散读取
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public OperateResult<bool> ReadDiscrete(string address);

        /// <summary>
        /// 离散读取
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public OperateResult<bool[]> ReadDiscrete(string address, ushort length);

        /// <summary>
        /// 写一个线圈信息，指定是否通断
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperateResult WriteCoil(string address, bool value);

        /// <summary>
        /// 批量写线圈信息，指定是否通断
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public OperateResult WriteCoil(string address, bool[] values);
    }
}
