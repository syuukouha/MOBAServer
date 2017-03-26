using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using MOBAServer.Cache;
using MOBAServer.Model;
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
                case OpPlayer.CreatePlayer:
                    OnCreatePlayer(client, request[0].ToString());
                    break;
                case OpPlayer.Online:
                    OnPlayerOnline(client);
                    break;
            }
        }


        public void OnDisConnect(MOBAClient client)
        {
            playerCache.OffLine(client);
        }
        /// <summary>
        /// 获取角色信息的处理
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
        /// <summary>
        /// 创建角色的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestParameters"></param>
        private void OnCreatePlayer(MOBAClient client, string name)
        {
            int accountID = accountCache.GetID(client);
            if (playerCache.Has(accountID))
                return;
            playerCache.Create(name, accountID);
            Send(client, OperationCode.PlayerCode, OpPlayer.CreatePlayer, 0, "创建成功");
        }
        /// <summary>
        /// 玩家上线的处理
        /// </summary>
        /// <param name="client"></param>
        private void OnPlayerOnline(MOBAClient client)
        {
            int accountID = accountCache.GetID(client);
            int playerID = playerCache.GetID(accountID);
            if (playerCache.IsOnLine(client))
                return;
            playerCache.OnLine(client, playerID);
            PlayerModel playerModel = playerCache.GetPlayerModel(playerID);
            PlayerDto playerDto = new PlayerDto()
            {
                ID = playerModel.Id,
                Exp = playerModel.Exp,
                Lv = playerModel.Lv,
                Name = playerModel.Name,
                Power = playerModel.Power,
                WinCount = playerModel.WinCount,
                LoseCount = playerModel.LoseCount,
                RunCount = playerModel.RunCount,
                FriendIDList = playerModel.FriendIdList,
                HeroIDList = playerModel.HeroIdList
            };
            Send(client, OperationCode.PlayerCode, OpPlayer.Online, 0, "上线成功", JsonMapper.ToJson(playerDto));
        }

    }
}
