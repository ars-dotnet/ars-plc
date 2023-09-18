using TOPRO.HSL.Profinet.Siemens;
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
    internal class TopRoSiemensSiemensFetchWriteNet : SiemensFetchWriteNet, ITopRoDefaultNetOperation
    {
        public PlcType PlcType {get;set;}
        public ProtocolType ProtocolType {get;set;}

        public TopRoSiemensSiemensFetchWriteNet()
        {
            PlcType = PlcType.Siemens;
            ProtocolType = ProtocolType.Fetch_Write;
        }
    }
}
