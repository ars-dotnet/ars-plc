using TOPRO.HSL;
using TOPRO.HSL.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using Topro.Extension.Plc.Scheme;
using TOPRO.PLC;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;
using TOPRO.PLC.TopRoOperations;

namespace Topro.Extension.Plc.TopRoOperations
{
    internal class OmRonFinsTcpNetOperation : BaseOperation
    {
        protected override PlcProtocolLevel PlcProtocolLevel => PlcProtocolLevel.OmRonFinsTcp;

        public OmRonFinsTcpNetOperation(
            IEnumerable<IPlcProvider> plcProvider,
             IEnumerable<ITopRoNetSchemeProvider> netSchemeProvider)
            :base(plcProvider,netSchemeProvider)
        {
            PlcType = PlcType.OmRon;
            ProtocolType = ProtocolType.FinsTcp;
        }

        public override void Excuting(OperationDto input, out bool hasConnected)
        {
            OmRonFinsTcpOperationDto dto = (OmRonFinsTcpOperationDto)input;
            hasConnected = false;

            if (LongConnection)
            {
                ITopRoNetScheme? scheme = 
                    _netSchemeProvider.GetScheme(
                        new TopRoOmRonFinsTcpScheme
                        {
                            IpAddress = dto.IpAddress,
                            Port = dto.Port,
                            SA1 = dto.SA1,
                            DA1 = dto.DA1,
                            DA2 = dto.DA2,
                            PlcType = PlcType,
                            ProtocolType = ProtocolType,
                        });

                if (null != scheme)
                {
                    TopRoNetScheme = scheme;
                    hasConnected = true;
                }
                else
                {
                    Init(input);
                }
            }
            else
            {
                Init(input);
            }
        }

        protected override void Init(OperationDto input)
        {
            INetOperation opt = _provider.Resolve(input);

            OmRonFinsTcpOperationDto dto = (OmRonFinsTcpOperationDto)input;
            TopRoNetScheme = new TopRoOmRonFinsTcpScheme
            {
                NetOperation = opt,
                IpAddress = dto.IpAddress,
                Port = dto.Port,
                PlcType = PlcType,
                ProtocolType = ProtocolType,

                SA1 = dto.SA1,
                DA1 = dto.DA1,
                DA2 = dto.DA2,
            };
        }
    }
}
