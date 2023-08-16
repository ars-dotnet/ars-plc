using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslCommunication.Core.Net
{
    public interface INetConnection : IDefaultNetOperation
    {
        public OperateResult ConnectServer();

        public OperateResult ConnectServer(AlienSession session);

        public OperateResult ConnectClose();
    }
}
