using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.HSL.Core.Net
{
    public interface IDefaultNetOperation : INetOperation
    {
        public string IpAddress { get; set; }

        public int Port { get; set; }
    }
}
