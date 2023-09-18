using TOPRO.HSL.Core.Net;
using TOPRO.HSL.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.TopRoNetOperation;
using TOPRO.PLC.Enums;

namespace Topro.Extension.Plc.TopRoNetInstance
{
    internal class TopRoOmronFinsTcpNet : OmronFinsNet, ITopRoOmRonNetOperation
    {
        public PlcType PlcType { get; set; }
        public ProtocolType ProtocolType { get; set; }

        public TopRoOmronFinsTcpNet()
        {
            PlcType = PlcType.OmRon;
            ProtocolType = ProtocolType.FinsTcp;
        }
    }
}
