using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace MOBAServer
{
    public class SingleSend
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client">接受的客户端</param>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作码</param>
        /// <param name="parameters">参数</param>
        public virtual void Send(MOBAClient client,byte opCode, byte subCode,short returnCode,string message, params object[] parameters)
        {
            OperationResponse response = new OperationResponse();
            response.OperationCode = opCode;
            response.Parameters = new Dictionary<byte, object>();
            response[80] = subCode;
            for (int i = 0; i < parameters.Length; i++)
            {
                response[(byte) i] = parameters[i];
            }
            response.ReturnCode = returnCode;
            response.DebugMessage = message;
            client.SendOperationResponse(response, new SendParameters());
        }
    }
}
