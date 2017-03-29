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
        private AccountHandler accountHandler;
        /// <summary>
        /// 角色
        /// </summary>
        private PlayerHandler playerHandler;
        /// <summary>
        /// 选人
        /// </summary>
        private SelectHandler selectHandler;

        public MOBAClient(InitRequest initRequest) : base(initRequest)
        {
            accountHandler = new AccountHandler();
            playerHandler = new PlayerHandler();
            selectHandler = new SelectHandler();
            playerHandler.StartSelectAction = selectHandler.StartSelect;
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
                    accountHandler.OnRequest(this, subCode, operationRequest);
                    break;
                case OperationCode.PlayerCode:
                    playerHandler.OnRequest(this, subCode, operationRequest);
                    break;
                case OperationCode.SelectCode:
                    selectHandler.OnRequest(this, subCode, operationRequest);
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
            //倒序
            selectHandler.OnDisConnect(this);
            playerHandler.OnDisConnect(this);
            accountHandler.OnDisConnect(this);
        }
    }
}
