using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Extension;
using TOPRO.HSL.Inovance;
using TOPRO.PLC;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Extension;
using TOPRO.PLC.Scheme;
using Xunit;

namespace TOPRO.Test
{
    public class PlcTest : PlcBaseTest
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

        [Theory]
        [InlineData("192.168.1.110", 502, PlcType.Inovance, ProtocolType.Modbus_Tcp)]
        public void Test1(string ip, int port,
            PlcType plcType, ProtocolType protocolType) 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.ModbusConnectionAndInit(new InovanceOperationDto()
            {
                IpAddress = ip,
                Port = port,

                Station = 1,
                AddressStartWithZero = true,
                Series = InovanceSeries.AM,
                IsStringReverse = false,

                PlcType = plcType,
                ProtocolType = protocolType
            });
            Assert.True(res.IsSuccess);

            var a = _operationManager.Read<string>("MW9100",12).Content;

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 汇川PLC测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 502, PlcType.Inovance, ProtocolType.Modbus_Tcp,"MW",200)]
        public void TestInovance(
            string ip,int port,
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom) 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.InovanceConnectionAndInit(new InovanceOperationDto()
            {
                IpAddress = ip,
                Port = port,

                Station = 1,
                AddressStartWithZero = true,
                Series = InovanceSeries.AM,
                IsStringReverse = false,

                PlcType = plcType,
                ProtocolType = protocolType
            });
            Assert.True(res.IsSuccess);

            //单个读写
            TestSingle(_operationManager, dbNamePrefix, dbFrom);

            //批量读写
            TestMulti(_operationManager, dbNamePrefix, dbFrom + 100);

            //bool
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom}", true);
            Assert.True(res.IsSuccess);
            var booldata = _operationManager.Read<bool>($"{dbNamePrefix}{dbFrom}");
            Assert.True(true == booldata.Content);

            //bool[]
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom + 100}", new bool[] { true, true });
            Assert.True(res.IsSuccess);
            var booldatas = _operationManager.Read<bool[]>($"{dbNamePrefix}{dbFrom + 100}", 2);
            Assert.True(true == booldatas.Content[0]);
            Assert.True(true == booldatas.Content[1]);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 三菱PLC测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 6000, PlcType.MelSec, ProtocolType.MC_Qna_3E_Binary,"D",400)]
        public void TestMelSec(
            string ip, int port, 
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            });

            Assert.True(res.IsSuccess);

            //单个读写
            TestSingle(_operationManager, dbNamePrefix, dbFrom);

            //批量读写
            TestMulti(_operationManager, dbNamePrefix, dbFrom + 100);

            //三菱D地址不支持bool读
            //bool
            res = _operationManager.Write($"M{dbFrom}", true);
            Assert.True(res.IsSuccess);
            var booldata = _operationManager.Read<bool>($"M{dbFrom}");
            Assert.True(true == booldata.Content);

            //bool[]
            res = _operationManager.Write($"M{dbFrom + 100}", new bool[] { true, true });
            Assert.True(res.IsSuccess);
            var booldatas = _operationManager.Read<bool[]>($"M{dbFrom + 100}", 2);
            Assert.True(true == booldatas.Content[0]);
            Assert.True(true == booldatas.Content[1]);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 西门子读写测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 102, PlcType.Siemens, ProtocolType.S7_S1200,"M",600)]
        public void TestSiemens(
            string ip, int port, 
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection:false);

            Assert.True(res.IsSuccess);

            //单个读写
            TestSingle(_operationManager, dbNamePrefix, dbFrom);

            //批量读写
            TestMulti(_operationManager, dbNamePrefix, dbFrom + 100);

            //bool
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom}", true);
            Assert.True(res.IsSuccess);
            var booldata = _operationManager.Read<bool>($"{dbNamePrefix}{dbFrom}");
            Assert.True(true == booldata.Content);

            //bool[]
            //西门子bool[]写入只能写入第一位
            //西门子bool[]读取未实现
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom + 100}", new bool[] { true, true });
            Assert.True(res.IsSuccess);
            var booldatas = _operationManager.Read<bool[]>($"{dbNamePrefix}{dbFrom + 100}", 2);
            Assert.False(booldatas.IsSuccess);
            Assert.True(null == booldatas.Content);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// ModbusTcp读写测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 502, PlcType.Modbus, ProtocolType.Modbus_Tcp,"",800)]
        public void TestModbus(
            string ip, int port, 
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.ModbusConnectionAndInit(new ModbusOperationDto()
            {
                IpAddress = ip,
                Port = port,

                Station = 1,
                AddressStartWithZero = true,
                IsStringReverse = false,

                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);
            Assert.True(res.IsSuccess);

            //单个读写
            TestSingle(_operationManager, dbNamePrefix, dbFrom);

            //批量读写
            TestMulti(_operationManager, dbNamePrefix, dbFrom + 100);

            //bool
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom}", true);
            Assert.True(res.IsSuccess);
            var booldata = _operationManager.Read<bool>($"{dbNamePrefix}{dbFrom}");
            Assert.True(true == booldata.Content);

            //bool[]
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom + 100}", new bool[] { true, true });
            Assert.True(res.IsSuccess);
            var booldatas = _operationManager.Read<bool[]>($"{dbNamePrefix}{dbFrom + 100}", 2);
            Assert.True(true == booldatas.Content[0]);
            Assert.True(true == booldatas.Content[1]);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 欧姆龙读写
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 9600, PlcType.OmRon, ProtocolType.FinsTcp,"D",1000)]
        public void TestOmRon(
            string ip, int port, 
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);
            Assert.True(res.IsSuccess);

            //单个读写
            TestSingle(_operationManager, dbNamePrefix, dbFrom);

            //批量读写
            TestMulti(_operationManager, dbNamePrefix, dbFrom + 100);

            //bool
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom}", true);
            Assert.True(res.IsSuccess);
            var booldata = _operationManager.Read<bool>($"{dbNamePrefix}{dbFrom}");
            Assert.True(true == booldata.Content);

            //bool[]
            res = _operationManager.Write($"{dbNamePrefix}{dbFrom + 100}", new bool[] { true, true });
            Assert.True(res.IsSuccess);
            var booldatas = _operationManager.Read<bool[]>($"{dbNamePrefix}{dbFrom + 100}", 2);
            Assert.True(true == booldatas.Content[0]);
            Assert.True(true == booldatas.Content[1]);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 测试bit
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 9600, PlcType.OmRon, ProtocolType.FinsTcp)]
        public void TestBitData(string ip, int port, PlcType plcType, ProtocolType protocolType)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
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

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 测试type写
        /// 测试string类型采用GBK编码格式读写
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 9600, PlcType.OmRon, ProtocolType.FinsTcp)]
        public void TestTypeAll(string ip, int port, PlcType plcType, ProtocolType protocolType)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var rel = _operationManager.OmRonConnectionAndInit(new OmRonFinsTcpOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);

            Assert.True(rel.IsSuccess);

            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            rel = _operationManager.Write(
                "D120",
                new string[] { "你好", "test" }, 
                typeof(string[]),
                Encoding.GetEncoding("GBK"));

            Assert.True(rel.IsSuccess);

            var res = _operationManager.Read<string[]>("D120", 10, Encoding.GetEncoding("GBK"));
            Assert.True(res.IsSuccess);

            Assert.True("你好" == res.Content[0]);
            Assert.True("test" == res.Content[1]);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 测试长短连接
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 6000, PlcType.MelSec, ProtocolType.MC_Qna_3E_Binary)]
        public void TestMultipleManager(string ip, int port, PlcType plcType, ProtocolType protocolType)
        {
            using var scope1 = _serviceProvider.CreateScope();
            using var scope2 = _serviceProvider.CreateScope();
            var operation1 = scope1.ServiceProvider.GetRequiredService<IOperationManager>();

            //短连接，每次都会新建一个socket连接
            operation1.DefaultConnectionAndInit(new DefaultOperationDto
            {
                IpAddress = ip,
                Port = port,

                PlcType = plcType,
                ProtocolType = protocolType,

            },longConnection:false);

            var operation2 = scope2.ServiceProvider.GetRequiredService<IOperationManager>();

            Assert.True(operation1 != operation2);

            operation2.DefaultConnectionAndInit(new DefaultOperationDto
            {
                IpAddress = ip,
                Port = port,

                PlcType = plcType,
                ProtocolType = protocolType,
            }, longConnection: false);

            Assert.True(operation1.ConnectionId != operation2.ConnectionId);
            operation1.CloseConnection();
            operation2.CloseConnection();

            //长连接，相同配置规则不会重复建立连接
            operation1.DefaultConnectionAndInit(new DefaultOperationDto
            {
                IpAddress = ip,
                Port = port,

                PlcType = plcType,
                ProtocolType = protocolType,
            });
            operation2.DefaultConnectionAndInit(new DefaultOperationDto
            {
                IpAddress = ip,
                Port = port,

                PlcType = plcType,
                ProtocolType = protocolType,
            });

            Assert.True(operation1.ConnectionId == operation2.ConnectionId);

            operation1.CloseConnection();
            operation2.CloseConnection();
        }

        /// <summary>
        /// ModbusTcp读写测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 502, PlcType.Modbus, ProtocolType.Modbus_Tcp)]
        public void TestModbus123(
            string ip, int port,
            PlcType plcType, ProtocolType protocolType)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.ModbusConnectionAndInit(new ModbusOperationDto()
            {
                IpAddress = ip,
                Port = port,

                Station = 1,
                AddressStartWithZero = true,
                IsStringReverse = false,

                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);
            Assert.True(res.IsSuccess);

            //bool
            var data = _operationManager.Read<int>("3");

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 三菱PLC测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 6000, PlcType.MelSec, ProtocolType.MC_Qna_3E_Binary)]
        public void TestMelSecReadStringArray(
            string ip, int port,
            PlcType plcType, ProtocolType protocolType)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            });

            Assert.True(res.IsSuccess);

            //_operationManager.Write("D200", new string[] { "aabb1212","aabb1313"});

            //var datas = _operationManager.Read<string[]>("D200", 100);

            //每个字符串所占点位要有剩余，才能读出数组
            //或者字符串加固定符号结尾，比如,
            //var datass = _operationManager.Read<string[]>("D100", 14);

            var datasss = _operationManager.Read<string[]>("D100",30);

            _operationManager.CloseConnection();
        }

        [Fact]
        public void TestBits() 
        {
            bool[] data = new bool[16] 
            { 
                true, true, true, true,
                false,true, true,false,
                false,false, false,false,
                false,false, false,false
            };

            var a = data.ToShort();

            var data1 = new bool[] 
            { 
                true, true, true, true,
                false,true, true
            };

            var c = data1.ToShort();

            Assert.True(a == c);
            Assert.True(111 == a);

            var m = c.ToBinaryBits();

            Assert.True(data.SequenceEqual(m));
            Assert.False(data1.SequenceEqual(m));
        }
    }
}
