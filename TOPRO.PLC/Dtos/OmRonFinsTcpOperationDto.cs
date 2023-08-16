using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topro.Extension.Plc.Dtos
{
    public class OmRonFinsTcpOperationDto : DefaultOperationDto
    {
        /// <summary>
        /// 上位机节点
        /// </summary>
        public byte SA1 { get; set; }

        /// <summary>
        /// plc节点
        /// </summary>
        public byte DA1 { get; set; }

        /// <summary>
        /// plc单元号
        /// </summary>
        public byte DA2 { get; set; } = 0;
    }
}
