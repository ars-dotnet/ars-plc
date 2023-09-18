using TOPRO.HSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    internal class TopRoModBusScheme : TopRoDefaultScheme, ITopRoModBusScheme
    {
        public TopRoModBusScheme()
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.ModBusTcp;
        }

        public byte Station { get; set; }
        public bool AddressStartWithZero { get; set; }
        public bool IsStringReverse { get; set; }
        public DataFormat DataFormat { get; set; }
    }
}
