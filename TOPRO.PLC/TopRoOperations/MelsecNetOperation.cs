﻿using TOPRO.HSL.Core;
using TOPRO.HSL.Core.IMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC.MelsecOperations
{
    /// <summary>
    /// 三菱默认操作类
    /// </summary>
    internal class MelsecNetOperation :BaseTcpOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.DefaultTcp;

        public MelsecNetOperation (
            IEnumerable<IPlcProvider> plcProvider,
            IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider) 
            : base(plcProvider,netSchemeProvider)
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.MC_Qna_3E_Binary;

            Order = 1;
        }
    }
}
