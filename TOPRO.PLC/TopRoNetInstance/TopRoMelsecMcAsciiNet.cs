using TOPRO.HSL.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC.TopRoNetInstance
{
    /// <summary>
    /// 三菱MCQna3E ASSCII实例
    /// </summary>
    internal class TopRoMelsecMcAsciiNet : MelsecMcAsciiNet, ITopRoDefaultNetOperation
    {
        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }

        public TopRoMelsecMcAsciiNet()
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.MC_Qna_3E_ASCII;
        }
    }
}
