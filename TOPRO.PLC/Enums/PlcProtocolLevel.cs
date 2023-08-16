using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC.Enums
{
    public enum PlcProtocolLevel
    {
        DefaultTcp = 0,

        Serial = 1,

        ModBusTcp = 2,

        ModBusRtuorAscii = 3,

        OmRonFinsTcp = 4,
    }
}
