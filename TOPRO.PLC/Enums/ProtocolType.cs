using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC.Enums
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// 从模块的不同，可以分为C帧和E帧，即QJ71C24模块使用的帧，QJ71E71模块使用的帧。
        /// 帧：数据通讯报文。通讯的数据格式
        /// C24代表串口通信模块，E71代表以太网通信模块。如只使用了网络，则只要看E71，即3E帧/4E帧。
        /// 两种帧格式均可ASCII或二进制通信。看起来C24的C即COM,E71的E为Ehternet
        /// </summary>
        #region 三菱协议

        /// <summary>
        /// 三菱Q系列MELSEC通讯协议
        /// </summary>
        MC_Qna_3E_ASCII = 1,

        MC_Qna_3E_Binary = 2,

        MC_A_1E_Binary = 3,

        Serial = 4,

        #endregion 三菱协议

        #region 西门子协议

        S7_S1200 = 21,

        S7_S300 = 22,

        S7_S1500 = 23,

        S7_S200Smart = 24,

        Fetch_Write = 25,

        #endregion 西门子协议

        #region Modbus协议

        Modbus_Tcp = 41,

        Modbus_Rtu = 42,

        Modbus_ASCII = 43,

        #endregion Modbus协议

        #region OmRon协议

        FinsTcp = 51,

        #endregion OmRon协议
    }
}
