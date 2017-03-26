using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Threading;
using MOBAServer.Model;

namespace MOBAServer.Cache
{
    public class PlayerCache
    {
        #region 数据
        /// <summary>
        /// 账号ID对应的玩家ID
        /// </summary>
        //private Dictionary<int, int> accountPlayers = new Dictionary<int, int>();
        private SynchronizedDictionary<int, int> accountPlayers = new SynchronizedDictionary<int, int>();
        /// <summary>
        /// 玩家ID对应的玩家数据
        /// </summary>
        //private Dictionary<int,PlayerModel> playerModels = new Dictionary<int, PlayerModel>();
        private ConcurrentDictionary<int, PlayerModel> playerModels = new ConcurrentDictionary<int, PlayerModel>();
        /// <summary>
        /// 模拟主键ID
        /// </summary>
        private int index = 0;
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="accountID"></param>
        public void Create(string name, int accountID)
        {
            PlayerModel playerModel = new PlayerModel(index, name, accountID);
            accountPlayers.TryAdd(accountID, playerModel.Id);
            playerModels.TryAdd(playerModel.Id, playerModel);
            index++;
        }
        /// <summary>
        /// 判断是否存在角色
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public bool Has(int accountID)
        {
            return accountPlayers.ContainsKey(accountID);
        }
        /// <summary>
        /// 添加好友数据
        /// </summary>
        /// <param name="playerID"></param>
        public void AddFriend(int playerID,int friendID)
        {
            PlayerModel playerModel = playerModels[playerID];
            playerModel.FriendIdList.Add(friendID);
            PlayerModel friendModel = playerModels[friendID];
            friendModel.FriendIdList.Add(playerID);
        }
        /// <summary>
        /// 获取玩家ID
        /// </summary>
        /// <param name="accountID"></param>
        public int GetID(int accountID)
        {
            int playerID = -1;
            accountPlayers.TryGetValue(accountID, out playerID);
            return playerID;
        }
        /// <summary>
        /// 获取在线玩家ID
        /// </summary>
        /// <param name="client"></param>
        public int GetID(MOBAClient client)
        {
            return _onLinePlayers[client];
        }
        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public PlayerModel GetPlayerModel(int playerID)
        {
            return playerModels[playerID];
        }
        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PlayerModel GetPlayerModel(string name)
        {
            foreach (var playerModel in playerModels.Values)
            {
                if (playerModel.Name == name)
                    return playerModel;
            }
            return null;
        }
        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PlayerModel GetPlayerModel(MOBAClient client)
        {
            return playerModels[_onLinePlayers[client]];
        }
        /// <summary>
        /// 根据玩家ID获取对应的客户端
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public MOBAClient GetClient(int playerID)
        {
            foreach (MOBAClient client in _onLinePlayers.Keys)
            {
                if (_onLinePlayers[client].Equals(playerID))
                {
                    return client;
                }
            }
            return null;
        }
        #endregion
        #region 在线

        private Dictionary<MOBAClient, int> _onLinePlayers = new Dictionary<MOBAClient, int>();
        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        public void OnLine(MOBAClient client, int playerID)
        {
            _onLinePlayers.Add(client, playerID);
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void OffLine(MOBAClient client)
        {
            if (IsOnLine(client))
                if (_onLinePlayers.ContainsKey(client))
                    _onLinePlayers.Remove(client);
        }
        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        public bool IsOnLine(MOBAClient client)
        {
            return _onLinePlayers.ContainsKey(client);
        }
        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        /// <returns></returns>
        public bool IsOnLine(int playerID)
        {
            return _onLinePlayers.ContainsValue(playerID);
        }
        #endregion
    }
}
