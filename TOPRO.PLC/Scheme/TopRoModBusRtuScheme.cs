using TOPRO.HSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    internal class TopRoModBusRtuScheme : TopRoDefaultScheme, ITopRoModBusRtuScheme
    {
        public TopRoModBusRtuScheme()
        {
            PlcProtocolLevel = TOPRO.PLC.Enums.PlcProtocolLevel.ModBusRtuorAscii;
        }

        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public int StopBits { get; set; }

        public int Parity { get; set; }

        public byte Station { get; set; }
        public bool AddressStartWithZero { get; set; }
        public bool IsStringReverse { get; set; }
        public DataFormat DataFormat { get; set; }
    }
}
