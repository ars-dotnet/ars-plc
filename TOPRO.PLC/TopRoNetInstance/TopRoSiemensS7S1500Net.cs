using TOPRO.HSL.Profinet.Siemens;
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
    internal class TopRoSiemensS7S1500Net : SiemensS7Net, ITopRoDefaultNetOperation
    {
        public PlcType PlcType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public TopRoSiemensS7S1500Net() : base(SiemensPLCS.S1500)
        {
            PlcType = PlcType.Siemens;
            ProtocolType = ProtocolType.S7_S1500;
        }
    }
}
