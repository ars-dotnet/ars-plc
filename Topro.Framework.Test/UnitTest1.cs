using HslCommunication;
using HslCommunication.Profinet.Siemens;
using NUnit.Framework;

namespace Topro.Framework.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            SiemensS7Net siemensS7Net = new SiemensS7Net(SiemensPLCS.S300)
            {
                IpAddress = "127.0.0.1",
                Port = 102,
                Rack = 0, //PLC�Ļ��ܺţ����S7-400��PLC���õ�
                Slot = 0, //PLC�Ĳۺţ����S7-400��PLC���õ�
            };

            OperateResult connect = siemensS7Net.ConnectServer();
            Assert.True(connect.IsSuccess);

            connect = siemensS7Net.Write("M100",(short)123);
            Assert.True(connect.IsSuccess);

            var data = siemensS7Net.ReadInt16("M100");

            Assert.True(data.IsSuccess);
            Assert.True(data.Content == 123);

            siemensS7Net.ConnectClose();
        }
    }
}