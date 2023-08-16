using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Scheme;
using Topro.Extension.Plc.TopRoNetInstance;
using Topro.Extension.Plc.TopRoNetOperation;
using Topro.Extension.Plc.TopRoOperations;
using TOPRO.PLC.MelsecOperations;
using TOPRO.PLC.Scheme;
using TOPRO.PLC.TopRoNetInstance;
using TOPRO.PLC.TopRoNetOperation;
using TOPRO.PLC.TopRoOperations;

namespace TOPRO.PLC.Extension
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPlcCore(this IServiceCollection services)
        {
            #region  PlcOperation
            services.TryAddEnumerable(new[]
            {
                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(MelsecNetOperation),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(MelsecFxSerialNetOperation),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(SiemensS7NetOperation),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(ModbusNetOperation),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(ModbusRtuNetOperation),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(IOperation),
                    typeof(OmRonFinsTcpNetOperation),
                    ServiceLifetime.Scoped),
            });
            #endregion

            #region PlcNet
            services.TryAddEnumerable(new[]
            {
                new ServiceDescriptor(
                    typeof(ITopRoSerialNetOperation),
                    typeof(TopRoMelsecFxSerial),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoModbusNetOperation),
                    typeof(TopRoModBusTcpNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoModbusNetOperation),
                    typeof(TopRoModBusRtuNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoModbusNetOperation),
                    typeof(TopRoModBusAsciiNet),
                    ServiceLifetime.Scoped),


                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoMelsecMcAsciiNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoMelsecMcNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoMelsecA1ENet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoSiemensS7S1200Net),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoSiemensS7S1500Net),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoSiemensS7S200SmartNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoSiemensS7S300Net),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoDefaultNetOperation),
                    typeof(TopRoSiemensSiemensFetchWriteNet),
                    ServiceLifetime.Scoped),

                new ServiceDescriptor(
                    typeof(ITopRoOmRonNetOperation),
                    typeof(TopRoOmronFinsTcpNet),
                    ServiceLifetime.Scoped),
            });
            #endregion

            #region PlcNetSchemeProvider
            services.TryAddEnumerable(new[]
            {
                new ServiceDescriptor(
                    typeof(ITopRoNetSchemeProvider),
                    typeof(TopRoNetDefaultSchemeProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(ITopRoNetSchemeProvider),
                    typeof(TopRoNetSerialSchemeProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(ITopRoNetSchemeProvider),
                    typeof(TopRoNetModbusSchemeProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(ITopRoNetSchemeProvider),
                    typeof(TopRoNetModbusRtuSchemeProvider),
                    ServiceLifetime.Singleton),
                new ServiceDescriptor(
                    typeof(ITopRoNetSchemeProvider),
                    typeof(TopRoNetOmRonSchemeProvider),
                    ServiceLifetime.Singleton),
            });
            #endregion

            #region PlcProvider
            services.TryAddEnumerable(new[]
            {
                new ServiceDescriptor(
                    typeof(IPlcProvider),
                    typeof(DefaultPlcProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(IPlcProvider),
                    typeof(SerialPlcProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(IPlcProvider),
                    typeof(ModbusPlcProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(IPlcProvider),
                    typeof(ModbusRtuPlcProvider),
                    ServiceLifetime.Singleton),

                new ServiceDescriptor(
                    typeof(IPlcProvider),
                    typeof(OmRonPlcProvider),
                    ServiceLifetime.Singleton),
            });
            #endregion

            services.AddSingleton<ITopRoSchemeInstance, TopRoSchemeInstance>();
            services.AddScoped<IOperationManager, OperationManager>();
            return services;
        }
    }
}
