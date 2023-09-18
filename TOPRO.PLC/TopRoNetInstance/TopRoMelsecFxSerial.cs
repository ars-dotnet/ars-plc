using TOPRO.HSL.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC.TopRoNetInstance
{
    internal class TopRoMelsecFxSerial : MelsecFxSerial, ITopRoSerialNetOperation
    {
        public PlcType PlcType {get;set;}
        public ProtocolType ProtocolType {get;set;}

        public TopRoMelsecFxSerial()
        {
            PlcType = PlcType.MelSec;
            ProtocolType = ProtocolType.Serial;
        }
    }
}
