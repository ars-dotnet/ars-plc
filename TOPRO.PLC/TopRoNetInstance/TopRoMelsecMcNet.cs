using HslCommunication.Profinet.Melsec;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC.TopRoNetInstance
{
    /// <summary>
    /// 三菱MCQna3E二进制协议实例
    /// </summary>
    internal class TopRoMelsecMcNet : MelsecMcNet, ITopRoDefaultNetOperation
    {
        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }

        public TopRoMelsecMcNet()
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.MC_Qna_3E_Binary;
        }
    }
}
