using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using MOBACommon.Codes;
using MOBAServer.Cache;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    public class SelectHandler:SingleSend,IOpHandler
    {
        //缓存
        private SelectCache selectCache = Caches.Select;
        private PlayerCache playerCache = Caches.Player;

        public void OnRequest(MOBAClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpSelect.Enter:
                    OnEnter(client);
                    break;
            }
        }
        /// <summary>
        /// 进入选人
        /// </summary>
        /// <param name="client"></param>
        private void OnEnter(MOBAClient client)
        {
            int playerID = playerCache.GetID(client);
            SelectRoom selectRoom = selectCache.Enter(playerID, client);
            if (selectRoom == null)
                return;
            //进入成功
            //先给客户端发一个房间模型，再给房间内的客户端发一条消息：有人进入房间
            Send(client, OperationCode.SelectCode, OpSelect.GetInfo, 0, "获取房间信息",
                JsonMapper.ToJson(selectRoom._redTeamSelectModels.Values.ToArray()),
                JsonMapper.ToJson(selectRoom._blueTeamSelectModels.Values.ToArray()));
            selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Enter, 0, "有玩家进入房间", client, playerID);
        }

        public void OnDisConnect(MOBAClient client)
        {
            
        }
        /// <summary>
        /// 开始选人
        /// </summary>
        /// <param name="redTeam"></param>
        /// <param name="blueTeam"></param>
        public void StartSelect(List<int> redTeam, List<int> blueTeam)
        {
            //创建选人房间
            selectCache.CreateSelectRoom(redTeam, blueTeam);
        }

    }
}
