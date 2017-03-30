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
    class PlayerHandler:SingleSend,IOpHandler
    {
        public Action<List<int>, List<int>> StartSelectAction;

        /// <summary>
        /// 账号的缓存
        /// </summary>
        private AccountCache accountCache = Caches.Account;
        /// <summary>
        /// 角色的缓存
        /// </summary>
        private PlayerCache playerCache = Caches.Player;

        /// <summary>
        /// 匹配的缓存
        /// </summary>
        private MatchCache matchCache = Caches.Match;



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
                case OpPlayer.AddFriend:
                    OnAddFriend(client, request[0].ToString());
                    break;
                case OpPlayer.AddFriendToClient:
                    OnAddFriendToClient(client, (bool) request[0], (int) request[1]);
                    break;
                case OpPlayer.MatchStart:
                    OnMatchStart(client,(int)request[0]);
                    break;
                case OpPlayer.MatchStop:
                    OnMatchStop(client, (int)request[0]);
                    break;

            }
        }


        
        public void OnDisConnect(MOBAClient client)
        {
            //下线的时候，通知在线好友，显示离线状态
            //***这里要通过客户端获取到玩家账号ID，再用账号ID获取到玩家ID，否则会报空
            int accountID = accountCache.GetID(client);
            int playerID = playerCache.GetID(accountID);

            PlayerModel playerModel = playerCache.GetPlayerModel(playerID);
            if (playerModel != null)
            {
                foreach (int friendID in playerModel.FriendIdList)
                {
                    if (!playerCache.IsOnLine(friendID))    //因为GetPlayerModel，GetClient是获取在线玩家的数据
                        continue;
                    MOBAClient friendClient = playerCache.GetClient(friendID);
                    Send(friendClient, OperationCode.PlayerCode, OpPlayer.FriendOnlineState, 1, "好友玩家下线", playerModel.Id);
                }
            }
            matchCache.OffLine(client, playerID);
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
            //防止重复在线
            if (playerCache.IsOnLine(client))
                return;
            //上线
            playerCache.OnLine(client, playerID);
            //上线的时候，通知在线好友，显示在线状态
            PlayerModel playerModel = playerCache.GetPlayerModel(playerID);
            if (playerModel != null)
            {
                foreach (int friendID in playerModel.FriendIdList)
                {
                    if (!playerCache.IsOnLine(friendID))    //因为GetPlayerModel，GetClient是获取在线玩家的数据
                        continue;
                    MOBAClient friendClient = playerCache.GetClient(friendID);
                    Send(friendClient, OperationCode.PlayerCode, OpPlayer.FriendOnlineState, 0, "好友玩家上线", playerModel.Id);
                }
            }
            PlayerDto playerDto = ToDto(playerCache.GetPlayerModel(playerID));

            //发送
            Send(client, OperationCode.PlayerCode, OpPlayer.Online, 0, "上线成功", JsonMapper.ToJson(playerDto));
        }

        /// <summary>
        /// 添加好友的处理
        /// </summary>
        /// <param name="name"></param>
        private void OnAddFriend(MOBAClient client,string name)
        {
            //获取添加好友的数据模型
            PlayerModel friendModel = playerCache.GetPlayerModel(name);
            //获取当前玩家的数据模型
            PlayerModel playerModel = playerCache.GetPlayerModel(client);

            if (friendModel == null)
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.AddFriend, -1, "没有此玩家");
                return;
            }
            //如果添加的是自己则返回
            if (playerCache.GetPlayerModel(client).Id.Equals(friendModel.Id))
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.AddFriend, -3, "不能添加自己为好友");
                return;
            }
            //如果添加的是已经是好友的玩家
            int playerID = playerCache.GetID(client);
            if (friendModel.FriendIdList.Contains(playerID))
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.AddFriend, -4, "此玩家已经是你的好友");
                return;
            }
            //如果能获取到数据模型，先判断他是否在线
            bool isOnline = playerCache.IsOnLine(friendModel.Id);
            //不在线 回传不在线
            if (!isOnline)
            {
                Send(client, OperationCode.PlayerCode, OpPlayer.AddFriend, -1, "此玩家不在线");
            }
            //在线 给模型对应的客户端发消息
            MOBAClient friendClient = playerCache.GetClient(friendModel.Id);

            Send(friendClient, OperationCode.PlayerCode, OpPlayer.AddFriendToClient, 0, "是否添加好友?",
                JsonMapper.ToJson(ToDto(playerModel)));
        }

        /// <summary>
        /// 添加好友结果的处理
        /// </summary>
        /// <param name="result"></param>
        private void OnAddFriendToClient(MOBAClient client,bool result,int requestID)
        {
            MOBAClient requestClient = playerCache.GetClient(requestID);

            if (result)
            {
                //同意了，保存数据
                int playerID = playerCache.GetID(client);
                playerCache.AddFriend(playerID, requestID);
                Send(client, OperationCode.PlayerCode, OpPlayer.AddFriendToClient, 1, "添加成功",
                    JsonMapper.ToJson(ToDto(playerCache.GetPlayerModel(client))));
                Send(requestClient, OperationCode.PlayerCode, OpPlayer.AddFriendToClient, 1, "添加成功",
                    JsonMapper.ToJson(ToDto(playerCache.GetPlayerModel(requestClient))));
            }
            else
            {
                //拒绝了，就回传给原来的客户端，告诉他被拒绝了
                Send(requestClient, OperationCode.PlayerCode, OpPlayer.AddFriendToClient, -1, "此玩家拒绝你的请求");
            }
        }
        /// <summary>
        /// 开始匹配的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isSingle"></param>
        /// <param name="playerID"></param>
        private void OnMatchStart(MOBAClient client, int playerID)
        {
            //非法操作检测
            if (playerCache.GetID(client) != playerID)
                return;
            MatchRoom room = matchCache.EnterMatch(client, playerID);
            Send(client, OperationCode.PlayerCode, OpPlayer.MatchStart, 0, "开始匹配成功");
            //如果房间满了，就开始选人
            if (room.RoomIsFull())
            {
                //通知房间内所有人进入选人界面
                room.Broadcast(OperationCode.PlayerCode, OpPlayer.MatchComplete, 1, "是否进入选人界面(10秒)",null);
                StartSelectAction(room.RedTeamIdList, room.BlueTeamIdList);
                //删除房间
                matchCache.DeleteRoom(room);
            }
        }
        /// <summary>
        /// 离开匹配的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        private void OnMatchStop(MOBAClient client, int playerID)
        {
            bool result = matchCache.LeaveMatch(client, playerID);
            if (result)
                Send(client, OperationCode.PlayerCode, OpPlayer.MatchStop, 0, "离开匹配成功");

        }

        /// <summary>
        /// 赋值Dto
        /// </summary>
        /// <param name="playerModel"></param>
        /// <returns></returns>
        private PlayerDto ToDto(PlayerModel playerModel)
        {
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
                HeroID = new int[playerModel.HeroIdList.Count],
                Friends = new FriendDto[playerModel.FriendIdList.Count]
            };
            //英雄ID列表
            for (int i = 0; i < playerModel.HeroIdList.Count; i++)
            {
                playerDto.HeroID[i] = playerModel.HeroIdList[i];
            }
            //好友ID列表
            FriendDto[] friendDtos = new FriendDto[playerModel.FriendIdList.Count];
            for (int i = 0; i < playerModel.FriendIdList.Count; i++)
            {
                int id = playerModel.FriendIdList[i];
                PlayerModel friendModel = playerCache.GetPlayerModel(id);
                bool isOnline = playerCache.IsOnLine(id);
                friendDtos[i] = new FriendDto(id, friendModel.Name, isOnline);
            }
            //赋值给playerDto
            playerDto.Friends = friendDtos;
            return playerDto;
        }

    }
}
