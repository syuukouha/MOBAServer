using System;
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
        private SynchronizedDictionary<int, PlayerModel> playerModels = new SynchronizedDictionary<int, PlayerModel>();
        /// <summary>
        /// 主键ID
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
        /// 获取玩家数据
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public PlayerModel GetPlayerModel(int playerID)
        {
            return playerModels[playerID];
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
                _onLinePlayers.Remove(client);
        }

        public bool IsOnLine(MOBAClient client)
        {
            return _onLinePlayers.ContainsKey(client);
        }
        #endregion
    }
}
