using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topro.Extension.Plc.Scheme
{
    internal class TopRoOmRonFinsTcpScheme : TopRoDefaultScheme
    {
        public TopRoOmRonFinsTcpScheme()
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.OmRonFinsTcp;
        }

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
        public byte DA2 { get; set; }
    }
}
