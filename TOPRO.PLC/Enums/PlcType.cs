using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC.Enums
{
    /// <summary>
    /// plc类型
    /// </summary>
    public enum PlcType
    {
        /// <summary>
        /// 三菱
        /// </summary>
        MelSec = 1,

        /// <summary>
        /// 西门子
        /// </summary>
        Siemens = 2,

        /// <summary>
        /// modbus
        /// </summary>
        Modbus = 3,

        /// <summary>
        /// 欧姆龙
        /// </summary>
        OmRon = 4,

        /// <summary>
        /// 汇川
        /// </summary>
        Inovance = 5,
    }
}
