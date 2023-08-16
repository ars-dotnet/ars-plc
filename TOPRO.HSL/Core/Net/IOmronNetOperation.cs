using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslCommunication.Core.Net
{
    public interface IOmronNetOperation : IDefaultNetOperation
    {
        byte SA1 { get; set; }

        byte DA1 { get; set; }

        byte DA2 { get; set; }
    }
}
