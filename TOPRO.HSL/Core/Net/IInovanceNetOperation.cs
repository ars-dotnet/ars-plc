using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.HSL.Inovance;

namespace TOPRO.HSL.Core.Net
{
    public interface IInovanceNetOperation : IModbusNetOperation
    {
        /// <summary>
        /// 汇川系列
        /// </summary>
        public InovanceSeries Series { get; set; }
    }
}
