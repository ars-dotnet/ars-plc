using HslCommunication.Core;
using HslCommunication.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC.Scheme
{
    public interface ITopRoNetScheme
    {
        INetOperation NetOperation { get; }

        PlcType PlcType { get; }

        ProtocolType ProtocolType { get; }

        PlcProtocolLevel PlcProtocolLevel { get; }
    }

    public interface ITopRoDefaultScheme : ITopRoNetScheme
    {
        string IpAddress { get; }

        int Port { get; }
    }

    public interface ITopRoSerialScheme : ITopRoNetScheme
    {
        /// <summary>
        /// com口
        /// </summary>
        string PortName { get; }

        /// <summary>
        /// 波特率
        /// </summary>
        int BaudRate { get; }

        /// <summary>
        /// 数据位
        /// </summary>
        int DataBits { get; }

        /// <summary>
        /// 停止位
        /// </summary>
        int StopBits { get; }

        /// <summary>
        /// 奇偶
        /// </summary>
        int Parity { get; }
    }

    public interface ITopRoModBusScheme : ITopRoDefaultScheme
    {
        /// <summary>
        /// 站号
        /// </summary>
        byte Station { get; set; }

        /// <summary>
        /// 首地址从0开始
        /// </summary>
        bool AddressStartWithZero { get; set; }

        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        bool IsStringReverse { get; set; }

        DataFormat DataFormat { get; set; }
    }

    public interface ITopRoModBusRtuScheme : ITopRoModBusScheme, ITopRoSerialScheme
    {

    }
}
