﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TOPRO.HSL.Profinet.Siemens
{



    /// <summary>
    /// 西门子的PLC类型，目前支持的访问类型
    /// </summary>
    public enum SiemensPLCS
    {
        /// <summary>
        /// 1200系列
        /// </summary>
        S1200 = 1,
        /// <summary>
        /// 300系列
        /// </summary>
        S300 = 2,
        /// <summary>
        /// 1500系列PLC
        /// </summary>
        S1500 = 3,
        /// <summary>
        /// 200的smart系列
        /// </summary>
        S200Smart = 4,
    }


}
