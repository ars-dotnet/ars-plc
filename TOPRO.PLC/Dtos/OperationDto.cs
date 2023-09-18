using TOPRO.HSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC.Dtos
{
    public class OperationDto : ITopRoPlcType
    {
        public PlcType PlcType { get; set; }

        public ProtocolType ProtocolType { get; set; }
    }
}
