using HslCommunication.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC.Scheme
{
    public interface ITopRoSchemeInstance
    {
        HashSet<ITopRoNetScheme> Instance { get; }

        void Exist();
    }

    public class TopRoSchemeInstance : ITopRoSchemeInstance 
    {
        private volatile HashSet<ITopRoNetScheme> netSchemes;
        public TopRoSchemeInstance()
        {
            netSchemes = new HashSet<ITopRoNetScheme>();
        }

        public HashSet<ITopRoNetScheme> Instance => netSchemes;

        public void Exist()
        {
            if (Instance.Any()) 
            {
                PlcProtocolLevel[] levels = new[] { PlcProtocolLevel.Serial , PlcProtocolLevel.ModBusRtuorAscii };
                var serial = Instance.
                    Where(r => levels.Contains(r.PlcProtocolLevel)).
                    Select(r => (ITopRoSerialScheme)r);
                if (serial.Any()) 
                {
                    foreach (var se in serial) 
                    {
                        ((ISerialNetOperation)se.NetOperation).Close();
                    }

                    Instance.RemoveWhere(r => levels.Contains(r.PlcProtocolLevel));
                }

                if (Instance.Any())
                {
                    foreach (var i in Instance) 
                    {
                        ((INetConnection)i).ConnectClose();
                    }

                    Instance.Clear();
                } 
            }
        }
    }
}
