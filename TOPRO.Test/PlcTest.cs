using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Extension;
using TOPRO.PLC;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Extension;
using TOPRO.PLC.Scheme;

namespace TOPRO.Test
{
    public class PlcTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITopRoSchemeInstance _instance;
        public PlcTest()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();

            //注册PLC相关服务
            serviceDescriptors.AddPlcCore();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();

            _instance = _serviceProvider.GetRequiredService<ITopRoSchemeInstance>();
            //程序退出事件
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                try
                {
                    _instance.Exist();

                    //Environment.Exit(0);
                }
                catch (Exception ex)
                {

                }
            };
        }

        /// <summary>
        /// 三菱PLC测试
        /// </summary>
        [Fact]
        public void TestMelSec() 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 6000,
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary
            });

            Assert.True(rel.IsSuccess);

            var writeres = _operationManager.Write("D100",(short)123);
            Assert.True(writeres.IsSuccess);
            writeres = _operationManager.Write("D110", new short[] { 223,666 });
            Assert.True(writeres.IsSuccess);

            var readres = _operationManager.Read<short>("D100");
            Assert.True(readres.Content == 123);

            var readres1 = _operationManager.Read<short[]>("D110", 3);
            Assert.True(readres1.Content[0] == 223);
            Assert.True(readres1.Content[1] == 666);
            Assert.True(readres1.Content[2] == 0);
        }

        /// <summary>
        /// 西门子读写测试
        /// </summary>
        [Fact]
        public void TestSiemens()
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 102,
                PlcType = PlcType.Siemens,
                ProtocolType = ProtocolType.S7_S1200
            }, longConnection:false);

            Assert.True(rel.IsSuccess);

            var writeres = _operationManager.Write("M100", (double)123.99);
            Assert.True(writeres.IsSuccess);
            writeres = _operationManager.Write("M110", new double[] { 223.99, 666.99 });
            Assert.True(writeres.IsSuccess);

            var readres = _operationManager.Read<double>("M100");
            Assert.True(readres.Content == 123.99);

            var readres1 = _operationManager.Read<double[]>("M110", 3);
            Assert.True(readres1.Content[0] == 223.99);
            Assert.True(readres1.Content[1] == 666.99);
            Assert.True(readres1.Content[2] == 0);
            //主动释放连接,长连接也会释放
            //如果不主动释放，则在using代码块结束时自动释放，长连接不会释放
            _operationManager.CloseConnection();

            //这里会存储最新的连接，第一个连接已经被新连接覆盖掉了
            rel = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 103,
                PlcType = PlcType.Siemens,
                ProtocolType = ProtocolType.S7_S1200
            }, longConnection: false);
            Assert.True(rel.IsSuccess);
            writeres = _operationManager.Write("M100", (double)123.9999);
            Assert.True(writeres.IsSuccess);
            readres = _operationManager.Read<double>("M100");
            Assert.True(readres.Content == 123.9999);
        }

        /// <summary>
        /// ModbusTcp读写测试
        /// </summary>
        [Fact]
        public void TestModbus()
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.ModbusConnectionAndInit(new ModbusOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 502,
                Station = 1,
                AddressStartWithZero = true,
                IsStringReverse = false,
                PlcType = PlcType.Modbus,
                ProtocolType = ProtocolType.Modbus_Tcp
            }, longConnection: false);
            Assert.True(rel.IsSuccess);

            rel = _operationManager.Write("100", new bool[] { true, true });
            Assert.True(rel.IsSuccess);

            var res = _operationManager.Read<bool[]>("100",2);
            Assert.True(res.IsSuccess);
            Assert.True(true == res.Content[0]);
            Assert.True(true == res.Content[1]);
        }

        /// <summary>
        /// 欧姆龙读写
        /// </summary>
        [Fact]
        public void TestOmRon() 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 9600,
                PlcType = PlcType.OmRon,
                ProtocolType = ProtocolType.FinsTcp
            }, longConnection: false);
            Assert.True(rel.IsSuccess);

            rel = _operationManager.Write("D100",new string[] { "ars","test"});
            Assert.True(rel.IsSuccess);

            var res = _operationManager.Read<string[]>("D100",10);
            Assert.True(res.IsSuccess);
            Assert.True("ars" == res.Content[0]);
            Assert.True("test" == res.Content[1]);
        }

        /// <summary>
        /// 测试bit
        /// </summary>
        [Fact]
        public void TestBitData() 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 9600,
                PlcType = PlcType.OmRon,
                ProtocolType = ProtocolType.FinsTcp
            }, longConnection: false);
            Assert.True(rel.IsSuccess);

            bool[] data = new bool[] { true, false, true };

            rel = _operationManager.Write("D100", data.ToShort());
            Assert.True(rel.IsSuccess);

            var res = _operationManager.Read<short>("D100");
            Assert.True(res.IsSuccess);

            data = res.Content.ToBinaryBits();
            
            Assert.True(true == data[0]);
            Assert.True(false == data[1]);
            Assert.True(true == data[2]);
        }

        /// <summary>
        /// 测试type写
        /// 测试string类型采用GBK编码格式读写
        /// </summary>
        [Fact]
        public void TestTypeAll()
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = "127.0.0.1",
                Port = 9600,
                PlcType = PlcType.OmRon,
                ProtocolType = ProtocolType.FinsTcp
            }, longConnection: false);
            Assert.True(rel.IsSuccess);

            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            rel = _operationManager.Write("D120", new string[] { "你好", "test" }, typeof(string[]),Encoding.GetEncoding("GBK"));
            Assert.True(rel.IsSuccess);

            var res = _operationManager.Read<string[]>("D120", 10, Encoding.GetEncoding("GBK"));
            Assert.True(res.IsSuccess);

            Assert.True("你好" == res.Content[0]);
            Assert.True("test" == res.Content[1]);
        }

        [Fact]
        public void TestMultipleManager() 
        {
            using var scope1 = _serviceProvider.CreateScope();
            using var scope2 = _serviceProvider.CreateScope();
            var operation1 = scope1.ServiceProvider.GetRequiredService<IOperationManager>();
            var operation2 = scope2.ServiceProvider.GetRequiredService<IOperationManager>();

            Assert.True(operation1 != operation2);

            //短连接，每次都会新建一个socket连接
            operation1.DefaultConnectionAndInit(new DefaultOperationDto
            {
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary,
                IpAddress = "127.0.0.1",
                Port = 5007
            },longConnection:false);
            operation2.DefaultConnectionAndInit(new DefaultOperationDto
            {
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary,
                IpAddress = "127.0.0.1",
                Port = 5007
            }, longConnection: false);

            operation1.CloseConnection();
            operation2.CloseConnection();

            //长连接，相同配置规则不会重复建立连接
            operation1.DefaultConnectionAndInit(new DefaultOperationDto
            {
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary,
                IpAddress = "127.0.0.1",
                Port = 5007
            });
            operation2.DefaultConnectionAndInit(new DefaultOperationDto
            {
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary,
                IpAddress = "127.0.0.1",
                Port = 5007
            });
        }
    }
}
