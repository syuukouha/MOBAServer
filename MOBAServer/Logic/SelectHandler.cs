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
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    public class SelectHandler:SingleSend,IOpHandler
    {
        /// <summary>
        /// 开始对战的事件
        /// </summary>
        public Action<List<SelectModel>, List<SelectModel>> StartBattleAction;
        //缓存
        private SelectCache selectCache = Caches.Select;
        private PlayerCache playerCache = Caches.Player;
        private AccountCache accountCache = Caches.Account;


        public void OnRequest(MOBAClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpSelect.Enter:
                    OnEnter(client);
                    break;
                case OpSelect.Select:
                    OnSelect(client, (int) request[0]);
                    break;
                case OpSelect.Ready:
                    OnReady(client);
                    break;
                case OpSelect.Chat:
                    OnChat(client, request[0].ToString());
                    break;
            }
        }

        private void OnChat(MOBAClient client, string text)
        {
            //给当前客户端所在的房间内的所有人发一条消息
            PlayerModel playerModel = playerCache.GetPlayerModel(client);
            if (playerModel == null)
                return;
            SelectRoom selectRoom = selectCache.GetRoom(playerModel.Id);
            if (selectRoom != null)
            {
                string str = string.Format("{0}:{1}", playerModel.Name, text);
                selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Chat, 0, "有玩家发言了", null, str);
            }
        }

        /// <summary>
        /// 玩家确认选择（准备）
        /// </summary>
        /// <param name="client"></param>
        private void OnReady(MOBAClient client)
        {
            int playerID = playerCache.GetID(client);
            SelectRoom selectRoom = selectCache.Ready(playerID);
            if (selectRoom == null)
            {
                //返回准备失败给客户端
                Send(client, OperationCode.SelectCode, OpSelect.Ready, -1, "准备失败");
                return;
            }
            //通知房间所有人此玩家准备
            selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Ready, 0, "有人准备了", null, playerID);

            //检测 是否全部人都准备，都准备就进入战斗
            if (selectRoom.IsAllReady)
            {
                StartBattleAction(selectRoom.RedTeamSelectModels.Values.ToList(),
                    selectRoom.BlueTeamSelectModels.Values.ToList());
                //给客户端发送消息:准备战斗 切换场景
                selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.StartBattle, 0, "准备进入战斗场景", null);

                //销毁当前房间
                selectCache.DestorySelectRoom(selectRoom.ID);
            }
        }

        /// <summary>
        /// 玩家选人
        /// </summary>
        /// <param name="client"></param>
        private void OnSelect(MOBAClient client,int heroID)
        {
            int playerID = playerCache.GetID(client);
            SelectRoom selectRoom = selectCache.Select(playerID, heroID);
            if (selectRoom == null)
            {
                Send(client, OperationCode.SelectCode, OpSelect.Select, -1, "选择失败");
                return;
            }
            //给房间里面的所有人发一条消息：谁选了什么英雄(playerID，heroID)
            selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Select, 0, "有人选择了英雄", null, playerID, heroID);
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
                JsonMapper.ToJson(selectRoom.RedTeamSelectModels.Values.ToArray()),
                JsonMapper.ToJson(selectRoom.BlueTeamSelectModels.Values.ToArray()));
            selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Enter, 0, "有玩家进入房间", client, playerID);
            //检测是否全部都进入房间
            if (selectRoom.IsAllEnter)
            {
                //如果全部都进入房间就开启一个定时任务，通知玩家在60秒内准备，不准备房间就解散
                selectRoom.StartSchedule(DateTime.UtcNow.AddSeconds(60), () =>
                {
                    //房间解散
                    if (!selectRoom.IsAllReady)
                    {
                        selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Cancel, 1, "有玩家60秒内未准备，解散当前房间", null);
                        selectCache.DestorySelectRoom(selectRoom.ID);
                    }
                });
                //给所有人发送全部玩家进入房间的消息
                selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Enter, 1, "全部玩家进入房间", null, playerID);
            }
        }

        public void OnDisConnect(MOBAClient client)
        {
            //下线  *** 这里要通过客户端获取到玩家账号ID，再用账号ID获取到玩家ID，否则会报空
            int accountID = accountCache.GetID(client);
            int playerID = playerCache.GetID(accountID);
            selectCache.OffLine(client, playerID);
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
