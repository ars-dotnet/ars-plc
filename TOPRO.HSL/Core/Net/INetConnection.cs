using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.HSL.Core.Net
{
    public interface INetConnection : IDefaultNetOperation
    {
        public string ConnectionId { get; }

        public OperateResult ConnectServer();

        public OperateResult ConnectServer(AlienSession session);

        public OperateResult ConnectClose();
    }
}
