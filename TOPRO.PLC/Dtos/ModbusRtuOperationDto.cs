﻿using TOPRO.HSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Dtos;

namespace Topro.Extension.Plc.Dtos
{
    public class ModbusRtuOperationDto : SerialOperationDto
    {
        /// <summary>
        /// 站号
        /// </summary>
        public byte Station { get; set; }

        /// <summary>
        /// 首地址从0开始
        /// </summary>
        public bool AddressStartWithZero { get; set; }

        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        public bool IsStringReverse { get; set; }

        public DataFormat DataFormat { get; set; } = DataFormat.ABCD;
    }
}
