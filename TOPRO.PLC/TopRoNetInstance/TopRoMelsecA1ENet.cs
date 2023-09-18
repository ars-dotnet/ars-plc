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
    internal class TopRoMelsecA1ENet : MelsecA1ENet, ITopRoDefaultNetOperation
    {
        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }

        public TopRoMelsecA1ENet()
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.MC_A_1E_Binary;
        }
    }
}
