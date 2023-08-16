using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;

namespace Topro.Extension.Plc.Dtos
{
    public class DefaultOperationDto : OperationDto
    {
        public string IpAddress { get; set; }

        public int Port { get; set; }
    }
}
