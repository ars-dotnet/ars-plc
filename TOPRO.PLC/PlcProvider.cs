using TOPRO.HSL.Core;
using TOPRO.HSL.Core.IMessage;
using TOPRO.HSL.Core.Net;
using TOPRO.HSL.Profinet.Melsec;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.TopRoNetOperation;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.TopRoNetOperation;
using TOPRO.PLC.TopRoOperations;
using TOPRO.HSL.Inovance;

namespace TOPRO.PLC
{
    internal class DefaultPlcProvider: IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory; 
        public PlcProtocolLevel PlcProtocolLevel { get; }

        public DefaultPlcProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.DefaultTcp;
        }

        public INetOperation Resolve(OperationDto dto)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoDefaultNetOperation>>();

            DefaultOperationDto input = (DefaultOperationDto)dto;
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType && 
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            plc.IpAddress = input.IpAddress;
            plc.Port = input.Port;
            return plc;
        }
    }

    internal class SerialPlcProvider : IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public PlcProtocolLevel PlcProtocolLevel { get; }

        public SerialPlcProvider(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.Serial;
        }

        public INetOperation Resolve(OperationDto input)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoSerialNetOperation>>();
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            return plc;
        }
    }

    internal class ModbusPlcProvider : IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PlcProtocolLevel PlcProtocolLevel { get; }

        public ModbusPlcProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.ModBusTcp;
        }

        public INetOperation Resolve(OperationDto input)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoModbusNetOperation>>();

            ModbusOperationDto dto = (ModbusOperationDto)input;
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            plc.IpAddress = dto.IpAddress;
            plc.Port = dto.Port;
            plc.Station = dto.Station;
            plc.AddressStartWithZero = dto.AddressStartWithZero;
            plc.IsStringReverse = dto.IsStringReverse;
            plc.DataFormat = dto.DataFormat;
            return plc;
        }
    }

    internal class ModbusRtuPlcProvider : IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PlcProtocolLevel PlcProtocolLevel { get; }


        public ModbusRtuPlcProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.ModBusRtuorAscii;
        }

        public INetOperation Resolve(OperationDto input)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoModbusNetOperation>>();

            ModbusRtuOperationDto dto = (ModbusRtuOperationDto)input;
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            plc.Station = dto.Station;
            plc.AddressStartWithZero = dto.AddressStartWithZero;
            plc.IsStringReverse = dto.IsStringReverse;
            plc.DataFormat = dto.DataFormat;
            return plc;
        }
    }

    internal class OmRonPlcProvider : IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PlcProtocolLevel PlcProtocolLevel { get; }

        public OmRonPlcProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.OmRonFinsTcp;
        }

        public INetOperation Resolve(OperationDto input)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoOmRonNetOperation>>();

            OmRonFinsTcpOperationDto dto = (OmRonFinsTcpOperationDto)input;
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            plc.IpAddress = dto.IpAddress;
            plc.Port = dto.Port;
            plc.SA1 = dto.SA1;
            plc.DA1 = dto.DA1;
            plc.DA2 = dto.DA2;

            return plc;
        }
    }

    internal class InovancePlcProvider : IPlcProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PlcProtocolLevel PlcProtocolLevel { get; }

        public InovancePlcProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            PlcProtocolLevel = PlcProtocolLevel.InovanceTcp;
        }

        public INetOperation Resolve(OperationDto input)
        {
            using var scope = _scopeFactory.CreateScope();
            var netOperationPlcs = scope.ServiceProvider.GetRequiredService<IEnumerable<ITopRoInovanceNetOperation>>();

            InovanceOperationDto dto = (InovanceOperationDto)input;
            var plc = netOperationPlcs.FirstOrDefault(
                r =>
                    r.PlcType == input.PlcType &&
                    r.ProtocolType == input.ProtocolType);
            if (null == plc)
            {
                throw new Exception($"plctype:{input.PlcType},protocolType:{input.ProtocolType}实例未注册");
            }

            plc.IpAddress = dto.IpAddress;
            plc.Port = dto.Port;
            plc.Station = dto.Station;
            plc.AddressStartWithZero = dto.AddressStartWithZero;
            plc.IsStringReverse = dto.IsStringReverse;
            plc.DataFormat = dto.DataFormat;
            plc.Series = dto.Series;

            return plc;
        }
    }
}
