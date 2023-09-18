using TOPRO.HSL.Core;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC.Scheme
{
    public class BaseTopRoNetScheme : ITopRoNetScheme
    {
        public INetOperation NetOperation { get; set; }

        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }

        public PlcProtocolLevel PlcProtocolLevel { get; set; }
    }
}
