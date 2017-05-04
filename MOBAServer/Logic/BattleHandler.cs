using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using MOBAServer.Cache;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    public class BattleHandler:IOpHandler
    {
        //缓存
        private PlayerCache playerCache = Caches.Player;
        private AccountCache accountCache = Caches.Account;
        private BattleCache battleCache = Caches.Battle;

        public void OnRequest(MOBAClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpBattle.Enter:
                    OnEnter(client, (int) request[0]);
                    break;
                case OpBattle.Walk:
                    OnWalk(client, (float)request[0], (float)request[1], (float)request[2]);
                    break;

            }
        }
        /// <summary>
        /// 玩家移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        private void OnWalk(MOBAClient client, float x, float y, float z)
        {
            int playerID = playerCache.GetID(client);
            BattleRoom battleRoom = battleCache.GetRoom(playerID);
            if (battleRoom == null)
                return;
            //给每一个客户端发送谁移动到哪的信息
            battleRoom.Broadcast(OperationCode.BattleCode, OpBattle.Walk, 0, "有玩家移动", null, playerID, x, y, z);
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        private void OnEnter(MOBAClient client, int playerID)
        {
            BattleRoom battleRoom = battleCache.Enter(client, playerID);
            if (battleRoom == null)
                return;
            //首先要判断是否全部进入了
            if (!battleRoom.IsAllEnter)
                return;
            //给每一个客户端发送战斗房间的信息
            battleRoom.Broadcast(OperationCode.BattleCode, OpBattle.GetInfo, 0, "加载战斗场景数据", null,
                JsonMapper.ToJson(battleRoom.HeroModels),
                JsonMapper.ToJson(battleRoom.BuildModels));

        }

        public void OnDisConnect(MOBAClient client)
        {
            //下线  *** 这里要通过客户端获取到玩家账号ID，再用账号ID获取到玩家ID，否则会报空
            int accountID = accountCache.GetID(client);
            int playerID = playerCache.GetID(accountID);
            battleCache.OffLine(client, playerID);
        }
        /// <summary>
        /// 开始战斗
        /// </summary>
        /// <param name="redTeamModels"></param>
        /// <param name="blueTeamModels"></param>
        public void StartBattle(List<SelectModel> redTeamModels, List<SelectModel> blueTeamModels)
        {
            battleCache.CreateBattleRoom(redTeamModels, blueTeamModels);
        }
    }
}
