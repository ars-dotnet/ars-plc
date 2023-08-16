using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;

namespace TOPRO.PLC.TopRoNetInstance
{
    internal class TopRoModBusAsciiNet : ModbusAscii, ITopRoModbusNetOperation
    {
        public PlcType PlcType {get;set;}
        public ProtocolType ProtocolType {get;set;}
        public string IpAddress {get;set;}
        public int Port {get;set;}

        public TopRoModBusAsciiNet()
        {
            PlcType = PlcType.Modbus;
            ProtocolType = ProtocolType.Modbus_ASCII;
        }
    }
}
