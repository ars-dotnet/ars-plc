using HslCommunication.Profinet.Siemens;
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
    internal class TopRoSiemensS7S200SmartNet : SiemensS7Net, ITopRoDefaultNetOperation
    {
        public PlcType PlcType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public TopRoSiemensS7S200SmartNet() : base(SiemensPLCS.S200Smart)
        {
            PlcType = PlcType.Siemens;
            ProtocolType = ProtocolType.S7_S200Smart;
        }
    }
}
