using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBACommon.Codes;
using MOBAServer.Cache;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    class PlayerHandler:SingleSend,IOpHandler
    {
        /// <summary>
        /// 账号的缓存
        /// </summary>
        private AccountCache accountCache = Caches.Account;
        /// <summary>
        /// 角色的缓存
        /// </summary>
        private PlayerCache playerCache = Caches.Player;
        public void OnRequest(MOBAClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpPlayer.GetPlayerInfo:
                    OnGetInfo(client);
                    break;
            }
        }

        public void OnDisConnect(MOBAClient client)
        {
            
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void OnGetInfo(MOBAClient client)
        {
            int accountID = accountCache.GetID(client);
            if (accountID == -1)
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.GetPlayerInfo, -1, "非法登录");
                return;
            }
            else if (playerCache.Has(accountID))
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.GetPlayerInfo, 0, "角色存在");
                return;
            }
            else
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.GetPlayerInfo, -2, "角色不存在");
                return;
            }
        }
    }
}
