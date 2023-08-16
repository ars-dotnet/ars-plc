using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslCommunication.Core.Net
{
    public interface ISerialNetOperation : INetOperation
    {
        /// <summary>
        /// 初始化串口信息，9600波特率，8位数据位，1位停止位，无奇偶校验
        /// </summary>
        /// <param name="portName"></param>
        public void SerialPortInni(string portName);

        /// <summary>
        /// 根据自定义初始化方法进行初始化串口信息
        /// </summary>
        /// <param name="initi"></param>
        public void SerialPortInni(Action<SerialPort> initi);

        /// <summary>
        /// 打开一个新的串行端口连接
        /// </summary>
        public void Open();

        /// <summary>
        /// 关闭端口连接
        /// </summary>
        public void Close();
    }
}
