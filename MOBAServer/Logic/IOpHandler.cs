using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    public interface IOpHandler
    {
        /// <summary>
        /// 收到请求时的处理
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="subCode">子操作</param>
        /// <param name="request">请求</param>
        void OnRequest(MOBAClient client, byte subCode, OperationRequest request);
        /// <summary>
        /// 断开连接时的处理
        /// </summary>
        /// <param name="client">客户端</param>
        void OnDisConnect(MOBAClient client);
    }
}
