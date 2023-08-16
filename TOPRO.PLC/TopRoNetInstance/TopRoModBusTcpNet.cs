using HslCommunication.ModBus;
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
    internal class TopRoModBusTcpNet : ModbusTcpNet, ITopRoModbusNetOperation
    {
        public PlcType PlcType {get;set;}
        public ProtocolType ProtocolType {get;set;}

        public TopRoModBusTcpNet()
        {
            PlcType = PlcType.Modbus;
            ProtocolType = ProtocolType.Modbus_Tcp;
        }
    }
}
