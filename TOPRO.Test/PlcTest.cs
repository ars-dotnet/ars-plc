using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Extension;
using TOPRO.HSL;
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
        [InlineData("127.0.0.1", 502, PlcType.Inovance, ProtocolType.Modbus_Tcp)]
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
                Series = InovanceSeries.H3U,
                IsStringReverse = false,

                PlcType = plcType,
                ProtocolType = protocolType
            });
            Assert.True(res.IsSuccess);

            res = _operationManager.Write("D8000", 123.45f);

            var a = _operationManager.Read<float>("D8000").Content;

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 汇川PLC测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 502, PlcType.Inovance, ProtocolType.Modbus_Tcp, "MW", 200)]
        public void TestInovance(
            string ip, int port,
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
        [InlineData("127.0.0.1", 6000, PlcType.MelSec, ProtocolType.MC_Qna_3E_Binary, "D", 400)]
        public void TestMelSec(
            string ip, int port,
            PlcType plcType, ProtocolType protocolType,
            string dbNamePrefix, int dbFrom)
        {
            using var _operationManager = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            });

            Assert.True(res.IsSuccess);

            res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
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

            _instance.Exist();

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 西门子读写测试
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 102, PlcType.Siemens, ProtocolType.S7_S1200, "M", 600)]
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
        [InlineData("127.0.0.1", 502, PlcType.Modbus, ProtocolType.Modbus_Tcp, "", 800)]
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

                PlcType = PlcType.Modbus,
                ProtocolType = ProtocolType.Modbus_Tcp
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
        [InlineData("127.0.0.1", 9600, PlcType.OmRon, ProtocolType.FinsTcp, "D", 1000)]
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

            }, longConnection: false);

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

            _operationManager.Write("D200", new string[] { "aabb00121200","aabb00131300"});
            _operationManager.Write("D300", "aabb00121200");

            //每个字符串所占点位要有剩余，才能读出数组
            //或者字符串加固定符号结尾，比如,
            //var datass = _operationManager.Read<IEnumerable<string>>("D200", 30); //数组没有实现
            var datass = _operationManager.Read<string[]>("D200", 30);

            var datas = _operationManager.Read<string>("D300",10);

            //var datasss = _operationManager.Read<int[]>("D100",10).Content;

            //var res1 = _operationManager.Write<int>("D100", 65588);
            //Assert.True(res1.IsSuccess);

            //var data = _operationManager.Read<int>("D100").Content;
            //Assert.True(data == 65588);

            //res1 = _operationManager.Write<int>("D100", 0);
            //Assert.True(res1.IsSuccess);

            //data = _operationManager.Read<int>("D100").Content;
            //Assert.True(data == 0);

            _operationManager.CloseConnection();

            _operationManager.CloseConnection();

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 测试西门子读写bit
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="plcType"></param>
        /// <param name="protocolType"></param>
        [Theory]
        [InlineData("127.0.0.1", 102, PlcType.Siemens, ProtocolType.S7_S300)]
        public void TestSiemensBit(string ip, int port,
            PlcType plcType, ProtocolType protocolType) 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);

            Assert.True(res.IsSuccess);

            res =_operationManager.Write<short>("DB150.DBB286", 123);

            Assert.True(res.IsSuccess);

            var data = _operationManager.Read<short>("DB150.DBB286", 1);

            Assert.True(data.Content == 123);

            //写bit位
            //   （DB1.DBX0.8 - DB1.DBX0.15）是右边的8位，从右向左算；
            //   （DB1.DBX0.0 - DB1.DBX0.7）是左边的8位，从右向左算；

            // 00000000 01111011 => 00000000 01111010 
            res = _operationManager.Write("DB150.DBB286.8", false);
            Assert.True(res.IsSuccess);
            data = _operationManager.Read<short>("DB150.DBB286", 1);
            var c = data.Content.ToBinaryBits();

            // 00000000 01111010 => 00000000 01111110
            res = _operationManager.Write("DB150.DBB286.10", true);
            Assert.True(res.IsSuccess);
            data = _operationManager.Read<short>("DB150.DBB286", 1);
            c = data.Content.ToBinaryBits();

            //00000000 01111110 => 00000001 01111110 
            res = _operationManager.Write("DB150.DBB286.0", true);
            data = _operationManager.Read<short>("DB150.DBB286", 1);
            c = data.Content.ToBinaryBits();

            //00000001 01111110 => 00001001 01111110
            res = _operationManager.Write("DB150.DBB286.3", true);
            data = _operationManager.Read<short>("DB150.DBB286", 1);
            c = data.Content.ToBinaryBits();
        }

        /// <summary>
        /// 测试西门子读写string
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="plcType"></param>
        /// <param name="protocolType"></param>
        [Theory]
        [InlineData("127.0.0.1", 102, PlcType.Siemens, ProtocolType.S7_S300)]
        public void TestSiemensxString(string ip, int port,
            PlcType plcType, ProtocolType protocolType)
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();
            var res = _operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = plcType,
                ProtocolType = protocolType
            }, longConnection: false);

            Assert.True(res.IsSuccess);

            //res = _operationManager.Write<string>("DB100.DBB100", "aabb1212");

            //Assert.True(res.IsSuccess);

            //var data = _operationManager.Read<string>("DB100.DBB100", 10);

            //Assert.True(data.Content == "aabb1212");

            var xx = _operationManager.Read<short>("DB100.DBB200", 1);
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

            var mmm = a.ToBinaryBits();

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

        /// <summary>
        /// 三菱PLC测试读写Bit位
        /// </summary>
        [Theory]
        [InlineData("127.0.0.1", 6000, PlcType.MelSec, ProtocolType.MC_Qna_3E_Binary)]
        public void TestMelSecReadBit(
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

            //写bit位
            //res = _operationManager.Write("D200.1",true);
            //res = _operationManager.Write("D200.1", false);
            //Assert.True(res.IsSuccess);

            //res = _operationManager.Write("D200.1", new bool[] { true,false,true});
            //Assert.True(res.IsSuccess);

            ////读bit位
            //var data = _operationManager.Read<bool>("D200.1");
            //Assert.True(data.IsSuccess);
            //Assert.True(data.Content == true);

            //var datas = _operationManager.Read<bool[]>("D200.1",6);

            //Assert.True(datas.IsSuccess);
            //Assert.True(datas.Content[0] == true);
            //Assert.True(datas.Content[2] == true);

            //_operationManager.Write("D100", 100);
            //datas = _operationManager.Read<bool[]>("D100.0", 7);
            //Assert.True(datas.IsSuccess);
            //Assert.True(datas.Content[0] == false);
            //Assert.True(datas.Content[1] == false);
            //Assert.True(datas.Content[2] == true);
            //Assert.True(datas.Content[5] == true);
            //Assert.True(datas.Content[6] == true);

            var xx = _operationManager.Read<short>("D100", 1);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// modbus读取bit位
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="plcType"></param>
        /// <param name="protocolType"></param>
        [Theory]
        [InlineData("127.0.0.1", 502, PlcType.Modbus, ProtocolType.Modbus_Tcp)]
        public void TestModBusReadBit(
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

            //var res1 = _operationManager.Write("100", 100);
            //Assert.True(res1.IsSuccess);

            //var datas = _operationManager.Read<byte[]>("100", 1);

            var datas = _operationManager.Read<short>("100", 1);

            _operationManager.CloseConnection();
        }

        /// <summary>
        /// 测试并发连接、关闭
        /// </summary>
        [Fact]
        public async void TestManyConnect() 
        {
            using var _operationManager = _serviceProvider.GetRequiredService<IOperationManager>();

            //连接
            IEnumerable<Task<(IOperationManager, OperateResult)>> tasks = new List<Task<(IOperationManager, OperateResult)>>()
            {
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
                ConnectAsync("127.0.0.1",6000),
            };

            var mm = await Task.WhenAll(tasks);

            var instance = _serviceProvider.GetRequiredService<ITopRoSchemeInstance>();

            //关闭
            var taskss = mm.Select(r => CloseAsync(r.Item1));

            var nn = await Task.WhenAll(taskss);
        }

        private async Task<(IOperationManager, OperateResult)> ConnectAsync(string ip,int port) 
        {
            //await Task.Delay(new Random().Next(10, 200));

            var provider = _serviceProvider.CreateScope().ServiceProvider;

            var operationManager = provider.GetRequiredService<IOperationManager>();

            var res = operationManager.DefaultConnectionAndInit(new DefaultOperationDto()
            {
                IpAddress = ip,
                Port = port,
                PlcType = PlcType.MelSec,
                ProtocolType = ProtocolType.MC_Qna_3E_Binary
            });

            return (operationManager,res);
        }

        private async Task<OperateResult> CloseAsync(IOperationManager operationManager) 
        {
            await Task.Delay(new Random().Next(10, 200));

            return operationManager.CloseConnection();
        }
    }
}
