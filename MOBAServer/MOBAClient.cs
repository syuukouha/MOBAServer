using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBACommon.Codes;
using MOBAServer.Logic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
namespace MOBAServer
{
    public class MOBAClient:ClientPeer
    {
        /// <summary>
        /// 账号逻辑
        /// </summary>
        private IOpHandler accountOpHandler;
        public MOBAClient(InitRequest initRequest) : base(initRequest)
        {
            accountOpHandler = new AccountHandler();
        }
        /// <summary>
        /// 客户端请求
        /// </summary>
        /// <param name="operationRequest"></param>
        /// <param name="sendParameters"></param>
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            byte opCode = operationRequest.OperationCode;
            byte subCode = (byte) operationRequest[80];
            switch (opCode)
            {
                case OperationCode.AccountCode:
                    accountOpHandler.OnRequest(this, subCode, operationRequest);
                    break;
            }
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="reasonCode"></param>
        /// <param name="reasonDetail"></param>
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            accountOpHandler.OnDisConnect(this);
        }
    }
}
