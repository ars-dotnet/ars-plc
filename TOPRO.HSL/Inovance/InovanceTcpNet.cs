using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.HSL.ModBus;
using TOPRO.HSL.Core;

namespace TOPRO.HSL.Inovance
{
    //
    // 摘要:
    //     汇川的网口通信协议，A适用于AM400、 AM400_800、 AC800、H3U, XP, H5U 等系列底层走的是MODBUS-TCP协议，地址说明参见标记
    //     Huichuan's serial communication protocol is applicable to AM400, AM400_800, AC800
    //     and other series. The bottom layer is MODBUS-RTU protocol. For the address description,
    //     please refer to the mark
    //
    // 言论：
    //     AM400_800 的元件有 Q 区，I 区，M 区这三种，分别都可以按位，按字节，按字和按双字进行访问，在本组件的条件下，仅支持按照位，字访问。
    //     位地址支持 Q, I, M 地址类型，字地址支持 SM, SD，支持对字地址的位访问，例如 ReadBool("SD0.5"); H3U 系列控制器支持
    //     M/SM/S/T/C/X/Y 等 bit 型变量（也称线圈） 的访问、 D/SD/R/T/C 等 word 型变量的访问；
    //     H5U 系列控制器支持 M/B/S/X/Y 等 bit 型变量（也称线圈） 的访问、 D/R 等 word 型变量的访问；内部 W 元件，不支持通信访问。
    public class InovanceTcpNet : ModbusTcpNet
    {
        //
        // 摘要:
        //     获取或设置汇川的系列，默认为AM系列
        public InovanceSeries Series { get; set; }

        //
        // 摘要:
        //     实例化一个默认的对象
        public InovanceTcpNet()
        {
            Series = InovanceSeries.AM;
            base.DataFormat = DataFormat.CDAB;
        }

        //
        // 摘要:
        //     通过指定站号，ip地址，端口号来实例化一个新的对象
        //
        // 参数:
        //   ipAddress:
        //     Ip地址
        //
        //   port:
        //     端口号
        //
        //   station:
        //     站号信息
        public InovanceTcpNet(string ipAddress, int port = 502, byte station = 1)
            : base(ipAddress, port, station)
        {
            Series = InovanceSeries.AM;
            base.DataFormat = DataFormat.CDAB;
        }

        //
        // 摘要:
        //     通过指定站号，IP地址，端口以及PLC的系列来实例化一个新的对象
        //     Instantiate a new object by specifying the station number and PLC series
        //
        // 参数:
        //   series:
        //     PLC的系列
        //
        //   ipAddress:
        //     Ip地址
        //
        //   port:
        //     端口号
        //
        //   station:
        //     站号信息
        public InovanceTcpNet(InovanceSeries series, string ipAddress, int port = 502, byte station = 1)
            : base(ipAddress, port, station)
        {
            Series = series;
            base.DataFormat = DataFormat.CDAB;
        }

        public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
        {
            return InovanceHelper.PraseInovanceAddress(Series, address, modbusCode);
        }

        public override string ToString()
        {
            return $"InovanceTcpNet<{Series}>[{IpAddress}:{Port}]";
        }
    }
}
