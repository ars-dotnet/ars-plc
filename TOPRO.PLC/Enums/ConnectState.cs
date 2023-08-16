using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC.Enums
{
    public enum ConnectState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        None = 0,

        /// <summary>
        /// 连接
        /// </summary>
        Connected = 1,

        /// <summary>
        /// 断开
        /// </summary>
        Disconnected = 2,
    }
}
