using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;

namespace Topro.Extension.Plc.Dtos
{
    public class SerialOperationDto : OperationDto
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
    }
}
