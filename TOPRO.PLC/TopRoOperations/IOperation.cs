using TOPRO.HSL;
using TOPRO.HSL.Core;
using TOPRO.HSL.Core.IMessage;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace TOPRO.PLC.TopRoOperations
{
    /// <summary>
    /// 操作接口【连接、关闭连接、读写】
    /// </summary>
    public interface IOperation : ITopRoPlcType
    {
        /// <summary>
        /// 连接id
        /// </summary>
        string? ConnectionId { get; }

        /// <summary>
        /// 值=1表示默认实例
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 是否长连接
        /// </summary>
        bool LongConnection { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        ConnectState ConnectState { get; set; }

        IReadWriteNet? ReadWriteNet { get; }

        /// <summary>
        /// 操作前执行
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Excuting(OperationDto input, out bool hasConnected);

        /// <summary>
        /// 操作后执行
        /// </summary>
        void Excuted();

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        OperateResult Connection();

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        OperateResult CloseConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">读取地址，格式为"M100","D100","W1A0"</param>
        ///    地址支持的列表如下：
        //     地址名称 – 示例 – 地址进制 –
        //     数据寄存器 – D1000,D2000 – 10 –
        //     链接寄存器 – W100,W1A0 – 16 –
        //     文件寄存器 – R100,R200 – 10 –
        //     变址寄存器 – Z100,Z200 – 10 –
        //     定时器的值 – T100,T200 – 10 –
        //     计数器的值 – C100,C200 – 10 –
        /// <returns></returns>
        OperateResult<T> Read<T>(string address, MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">读取地址，格式为"M100","D100","W1A0"</param>
        /// <param name="length">读取的数据长度，字最大值960，位最大值7168</param>
        ///    地址支持的列表如下：
        //     地址名称 – 示例 – 地址进制 –
        //     数据寄存器 – D1000,D2000 – 10 –
        //     链接寄存器 – W100,W1A0 – 16 –
        //     文件寄存器 – R100,R200 – 10 –
        //     变址寄存器 – Z100,Z200 – 10 –
        //     定时器的值 – T100,T200 – 10 –
        //     计数器的值 – C100,C200 – 10 –
        /// <returns></returns>
        OperateResult<T> Read<T>(string address, ushort length, MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">读取地址，格式为"M100","D100","W1A0"</param>
        /// <param name="length">读取的数据长度，字最大值960，位最大值7168</param>
        /// <param name="encode"></param>
        ///    地址支持的列表如下：
        //     地址名称 – 示例 – 地址进制 –
        //     数据寄存器 – D1000,D2000 – 10 –
        //     链接寄存器 – W100,W1A0 – 16 –
        //     文件寄存器 – R100,R200 – 10 –
        //     变址寄存器 – Z100,Z200 – 10 –
        //     定时器的值 – T100,T200 – 10 –
        //     计数器的值 – C100,C200 – 10 –
        /// <returns></returns>
        OperateResult<T> Read<T>(string address, ushort length, Encoding encode, MethodInfo method);

        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        OperateResult Write<T>(string address, T value, MethodInfo method);

        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="encode"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        OperateResult Write<T>(string address, T value,Encoding encode, MethodInfo method);
    }
}
