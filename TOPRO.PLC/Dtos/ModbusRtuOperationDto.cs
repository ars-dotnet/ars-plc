using HslCommunication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;

namespace Topro.Extension.Plc.Dtos
{
    public class ModbusRtuOperationDto : OperationDto
    {
        /// <summary>
        /// com口
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public int StopBits { get; set; }

        /// <summary>
        /// 奇偶
        /// </summary>
        public int Parity { get; set; }

        /// <summary>
        /// 站号
        /// </summary>
        public byte Station { get; set; }

        /// <summary>
        /// 首地址从0开始
        /// </summary>
        public bool AddressStartWithZero { get; set; }

        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        public bool IsStringReverse { get; set; }

        public DataFormat DataFormat { get; set; } = DataFormat.ABCD;
    }
}
