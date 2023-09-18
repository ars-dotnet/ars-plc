using TOPRO.HSL.Core;
using TOPRO.HSL.Core.IMessage;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC
{
    public interface IPlcProvider
    {
        PlcProtocolLevel PlcProtocolLevel { get; }

        /// <summary>
        /// 获取最终执行的不同型号plc实例
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        INetOperation Resolve(OperationDto input);
    }
}
