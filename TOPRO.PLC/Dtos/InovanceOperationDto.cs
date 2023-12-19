using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using TOPRO.HSL.Core;
using TOPRO.HSL.Inovance;

namespace TOPRO.PLC.Dtos
{
    public class InovanceOperationDto : ModbusOperationDto
    {
        public InovanceOperationDto() : base()
        {
            DataFormat = DataFormat.CDAB;
        }

        /// <summary>
        /// 汇川的系列
        /// </summary>
        public InovanceSeries Series { get; set; } = InovanceSeries.AM;
    }
}
