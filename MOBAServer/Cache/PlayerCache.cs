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
        #endregion

    }
}
