using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;
using TOPRO.PLC.Scheme;

namespace Topro.Extension.Plc.Scheme
{
    public abstract class BaseTopRoNetSchemeProvider : ITopRoNetSchemeProvider
    {
        protected readonly ITopRoSchemeInstance _instance;

        public BaseTopRoNetSchemeProvider(ITopRoSchemeInstance instance)
        {
            _instance = instance;
        }

        public PlcProtocolLevel PlcProtocolLevel { get; protected set; }

        /// <summary>
        /// 获取是否存在已连接的客户端
        /// </summary>
        /// <param name="netScheme"></param>
        /// <returns></returns>
        public abstract ITopRoNetScheme? GetScheme(ITopRoNetScheme netScheme);

        public virtual void RemoveScheme(ITopRoNetScheme netScheme)
        {
            _instance.Instance.Remove(netScheme);
        }

        public virtual void SetScheme(ITopRoNetScheme netScheme)
        {
            if (null != GetScheme(netScheme))
                return;

            _instance.Instance.Add(netScheme);
        }
    }
}
