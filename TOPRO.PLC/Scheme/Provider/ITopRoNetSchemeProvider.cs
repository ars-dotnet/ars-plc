using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC.Scheme
{
    public interface ITopRoNetSchemeProvider
    {
        PlcProtocolLevel PlcProtocolLevel { get; }

        ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme);

        void SetScheme(ITopRoNetScheme netScheme);

        void RemoveScheme(ITopRoNetScheme netScheme);
    }
}
