using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.Room;
using MOBACommon.Dto;

namespace MOBAServer.Cache
{
    public class BattleCache:RoomCacheBase<BattleRoom>
    {
        /// <summary>
        /// 创建战斗房间
        /// </summary>
        /// <param name="redSelectRooms"></param>
        /// <param name="blueSelectRooms"></param>
        public void CreateBattleRoom(List<SelectModel> redRoom, List<SelectModel> blueRoom)
        {
            BattleRoom battleRoom;
            //检测有没有可重用的房间
            if(!rooms.TryDequeue(out battleRoom))
                return;
            //有就初始化房间数据
            battleRoom.Init(redRoom, blueRoom);
            //添加映射关系
            foreach (SelectModel selectModel in redRoom)
            {
                roomIDs.TryAdd(selectModel.PlayerID, battleRoom.ID);
            }
            foreach (SelectModel selectModel in blueRoom)
            {
                roomIDs.TryAdd(selectModel.PlayerID, battleRoom.ID);
            }
            Rooms.TryAdd(battleRoom.ID, battleRoom);
            //所有玩家都进入之后 再发送
        }
        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="roomID"></param>
        public void DestroyBattleRoom(int roomID)
        {
            //移除房间ID和房间模型的关系，如果找不到该房间就返回
            BattleRoom battleRoom;
            if (!Rooms.TryRemove(roomID, out battleRoom))
                return;
            //移除玩家ID和房间ID的关系
            foreach (KeyValuePair<int, HeroModel> redTeamHeroModel in battleRoom.RedTeamHeroModels)
            {
                roomIDs.Remove(redTeamHeroModel.Key);
            }
            foreach (KeyValuePair<int, HeroModel> blueTeamHeroModel in battleRoom.BlueTeamHeroModels)
            {
                roomIDs.Remove(blueTeamHeroModel.Key);
            }
            //清空房间内的数据
            battleRoom.Clear();
            //入重用队列
            rooms.Enqueue(battleRoom);
        }
        /// <summary>
        /// 进入战斗
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public BattleRoom Enter(MOBAClient client, int playerID)
        {
            BattleRoom battleRoom = GetRoom(playerID);
            if (battleRoom == null)
                return null;
            battleRoom.Enter(client);
            return battleRoom;
        }
        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        public void OffLine(MOBAClient client, int playerID)
        {
            //获取房间
            BattleRoom battleRoom = GetRoom(playerID);
            if (battleRoom == null)
                return;
            //离开房间
            battleRoom.Leave(client);
            if(!battleRoom.IsAllLeave)
                return;
            //如果全部人都退出了 销毁房间
            DestroyBattleRoom(battleRoom.ID);
        }
    }
}
