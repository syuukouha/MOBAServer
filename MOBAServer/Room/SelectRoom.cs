using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBACommon.Dto;
using MOBAServer.Cache;
using MOBAServer.Model;

namespace MOBAServer.Room
{
    public class SelectRoom:RoomBase<MOBAClient>
    {
        /// <summary>
        /// 队伍1玩家ID对应的选择模型
        /// </summary>
        public Dictionary<int, SelectModel> _redTeamSelectModels;
        /// <summary>
        /// 队伍2玩家ID对应的选择模型
        /// </summary>
        public Dictionary<int, SelectModel> _blueTeamSelectModels;
        /// <summary>
        /// 进入的数量
        /// </summary>
        public int _enterCount;
        /// <summary>
        /// 准备的数量
        /// </summary>
        public int _readyCount;

        //是否全部进入房间
        public bool IsAllEnter
        {
            get { return _enterCount == Count; }
        }
        //是否全部准备
        public bool IsAllReady
        {
            get { return _readyCount == _enterCount; }
        }

        public SelectRoom(int id, int count) : base(id, count)
        {
            _redTeamSelectModels = new Dictionary<int, SelectModel>();
            _blueTeamSelectModels = new Dictionary<int, SelectModel>();
            _enterCount = 0;
            _readyCount = 0;
        }
        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="redTeam"></param>
        /// <param name="blueTeam"></param>
        public void InitRoom(List<int> redTeam, List<int> blueTeam)
        {
            //初始化房间
            PlayerModel playerModel;
            SelectModel selectModel;
            foreach (int id in redTeam)
            {
                playerModel = Caches.Player.GetPlayerModel(id);
                selectModel = new SelectModel(playerModel.Id, playerModel.Name);
                //添加到字典
                _redTeamSelectModels.Add(playerModel.Id, selectModel);
            }
            foreach (int id in blueTeam)
            {
                playerModel = Caches.Player.GetPlayerModel(id);
                selectModel = new SelectModel(playerModel.Id,playerModel.Name);
                //添加到字典
                _blueTeamSelectModels.Add(playerModel.Id, selectModel);
            }
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        public void EnterRoom(int playerID,MOBAClient client)
        {
            if (_redTeamSelectModels.ContainsKey(playerID))
            {
                _redTeamSelectModels[playerID].IsEnter = true;
            }else if (_blueTeamSelectModels.ContainsKey(playerID))
            {
                _blueTeamSelectModels[playerID].IsEnter = true;
            }
            else
            {
                //失败
                return;
            }
            //添加客户端
            ClientList.Add(client);
            //更新进入人数
            _enterCount++;
        }
        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="heroID"></param>
        public bool Select(int playerID, int heroID)
        {
            //判断队友有没有选择
            if (_redTeamSelectModels.ContainsKey(playerID))
            {
                foreach (var value in _redTeamSelectModels.Values)
                {
                    if (value.HeroID == heroID)
                        return false;
                }
                //可以选择
                _redTeamSelectModels[playerID].HeroID = heroID;
            }
            if (_blueTeamSelectModels.ContainsKey(playerID))
            {
                foreach (var value in _blueTeamSelectModels.Values)
                {
                    if (value.HeroID == heroID)
                        return false;
                }
                _blueTeamSelectModels[playerID].HeroID = heroID;
            }
            return true;
        }
        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public bool Ready(int playerID)
        {
            if (_redTeamSelectModels.ContainsKey(playerID))
            {
                SelectModel selectModel = _redTeamSelectModels[playerID];
                //检测有没有选英雄
                if (selectModel.HeroID == -1)
                    return false;
                selectModel.IsReady = true;
                //更新准备的人数
                _readyCount++;
            }else if (_blueTeamSelectModels.ContainsKey(playerID))
            {
                SelectModel selectModel = _blueTeamSelectModels[playerID];
                //检测有没有选英雄
                if (selectModel.HeroID == -1)
                    return false;
                selectModel.IsReady = true;
                //更新准备的人数
                _readyCount++;
            }
            return true;
        }
    }

}
