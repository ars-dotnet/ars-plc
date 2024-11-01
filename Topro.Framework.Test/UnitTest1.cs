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
                Rack = 0, //PLC的机架号，针对S7-400的PLC设置的
                Slot = 0, //PLC的槽号，针对S7-400的PLC设置的
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