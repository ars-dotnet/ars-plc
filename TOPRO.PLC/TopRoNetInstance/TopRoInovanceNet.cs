using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.HSL.Inovance;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;

namespace TOPRO.PLC.TopRoNetInstance
{
    internal class TopRoInovanceNet : InovanceTcpNet, ITopRoInovanceNetOperation
    {
        public TopRoInovanceNet()
        {
            PlcType = PlcType.Inovance;
            ProtocolType = ProtocolType.Modbus_Tcp;
        }

        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }
    }
}
