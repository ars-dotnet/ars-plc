
using Microsoft.Extensions.DependencyInjection;
using Topro.Extension.Plc.Dtos;
using TOPRO.PLC;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Extension;

//引用TOPRO.PLC.dll和TOPRO.HSL.dll时
//项目要添加一个对Polly包的引用
//  <ItemGroup>
//    < PackageReference Include = "Polly" Version = "7.0.0" />
//  </ ItemGroup >

IServiceCollection service = new  ServiceCollection();

//注册PLC相关服务
service.AddPlcCore();

var serviceProvider = service.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();

IOperationManager operationManager = scope.ServiceProvider.GetRequiredService<IOperationManager>();
 
var res = operationManager.DefaultConnectionAndInit(new DefaultOperationDto
{
    IpAddress = "127.0.0.1",
    Port = 6000,
    PlcType = PlcType.MelSec,
    ProtocolType = ProtocolType.MC_Qna_3E_Binary
});

Console.WriteLine("连接PLC:" + res.Message);

////res = operationManager.Write<short>("D100", 123);

////Console.WriteLine("写PLC点位:" + res.Message);

////var read = operationManager.Read<short>("D100");

////Console.WriteLine("读取PLC点位:" + read.Message + ",值:" + read.Content);

var datass = operationManager.Read<string[]>("D100", 30);

operationManager.CloseConnection();

Console.Read();
