using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC
{
    public interface ITopRoPlcType
    {
        PlcType PlcType { get; set; }

        ProtocolType ProtocolType { get; set; }
    }
}
