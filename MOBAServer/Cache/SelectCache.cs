using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBACommon.Codes;
using MOBACommon.Dto;
using MOBAServer.Model;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class SelectCache:RoomCacheBase<SelectRoom>
    {
        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateSelectRoom(List<int> redTeam,List<int>blueTeam)
        {
            SelectRoom selectRoom;
            int count = redTeam.Count + blueTeam.Count;
            //先判断重用队列里面有没有房间
            if (rooms.TryDequeue(out selectRoom))
            {
                //有就初始化房间
                selectRoom.InitRoom(redTeam, blueTeam);
            }
            else
            {
                //没有就构建房间
                selectRoom = new SelectRoom(index++, count);
                //初始化房间
                selectRoom.InitRoom(redTeam, blueTeam);
            }
            //绑定玩家ID和房间ID
            foreach (int id in redTeam)
            {
                roomIDs.TryAdd(id, selectRoom.ID);
            }
            foreach (int id in blueTeam)
            {
                roomIDs.TryAdd(id, selectRoom.ID);
            }
            //绑定房间ID和房间模型
            Rooms.TryAdd(selectRoom.ID, selectRoom);

            //到这就房间创建完成，然后开启一个定时任务，通知玩家在10秒内同意进入房间，不同意房间就解散
            selectRoom.StartSchedule(DateTime.UtcNow.AddSeconds(10), () =>
            {
                //房间解散
                if (!selectRoom.IsAllEnter)
                {
                    DestorySelectRoom(selectRoom.ID);
                    selectRoom.Broadcast(OperationCode.SelectCode, OpSelect.Cancel, 0, "有玩家拒绝进入房间，解散当前房间",null);
                }
            } );

        }

        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="roomID"></param>
        public void DestorySelectRoom(int roomID)
        {
            //移除房间ID和房间模型的关系，如果找不到该房间就返回
            SelectRoom selectRoom;
            if (Rooms.TryRemove(roomID, out selectRoom))
                return;
            //移除玩家ID和房间ID的关系
            foreach (KeyValuePair<int, SelectModel> selectRoomRedTeamSelectModel in selectRoom._redTeamSelectModels)
            {
                roomIDs.Remove(selectRoomRedTeamSelectModel.Key);
            }
            foreach (KeyValuePair<int, SelectModel> selectRoomBlueTeamSelectModel in selectRoom._blueTeamSelectModels)
            {
                roomIDs.Remove(selectRoomBlueTeamSelectModel.Key);
            }
            //清空房间内的数据
            selectRoom._redTeamSelectModels.Clear();
            selectRoom._blueTeamSelectModels.Clear();
            selectRoom._enterCount = 0;
            selectRoom._readyCount = 0;
            selectRoom.ClientList.Clear();
            //入重用队列
            rooms.Enqueue(selectRoom);
        }
        /// <summary>
        /// 进入选人
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="client"></param>
        public SelectRoom Enter(int playerID, MOBAClient client)
        {
            //获取房间ID
            int roomID;
            if (!roomIDs.TryGetValue(playerID, out roomID))
                return null;
            //根据房间ID获取房间
            SelectRoom selectRoom;
            if(!Rooms.TryGetValue(roomID,out selectRoom))
                return null;
            selectRoom.EnterRoom(playerID, client);
            return selectRoom;
        }
    }
}
