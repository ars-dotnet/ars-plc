using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.HSL.Core.Net
{
    public interface IModbusNetOperation : IDefaultNetOperation
    {
        public bool AddressStartWithZero { get; set; }

        public byte Station { get; set; }

        public DataFormat DataFormat { get; set; }

        public bool IsStringReverse { get; set; }
    }
}
