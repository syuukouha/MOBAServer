using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Threading;
using MOBAServer.Model;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class MatchCache:RoomCacheBase<MatchRoom>
    {
        #region 匹配
        /// <summary>
        /// 玩家进入匹配队列
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns>进入的房间</returns>
        public MatchRoom EnterMatch(MOBAClient client,int playerID)
        {
            MatchRoom room;
            //存在等待的房间
            foreach (MatchRoom matchRoom in Rooms.Values)
            {
                if (matchRoom.RoomIsFull())
                    continue;
                room = matchRoom;
                //玩家进入房间
                room.Enter(client, playerID);
                //绑定玩家和房间的映射
                roomIDs.TryAdd(playerID, room.ID);
                return room;
            }
            //不存在等待的房间
            //是否有重用的房间
            if (rooms.Count > 0)
            {
                rooms.TryDequeue(out room);
                //添加映射关系
                Rooms.TryAdd(room.ID, room);
                roomIDs.TryAdd(playerID, room.ID);
                //玩家进入房间
                room.Enter(client, playerID);
                return room;
            }
            room = new MatchRoom(index, 2);
            index++;
            //添加映射关系
            Rooms.TryAdd(room.ID, room);
            roomIDs.TryAdd(playerID, room.ID);
            //玩家进入房间
            room.Enter(client, playerID);
            return room;
        }
        /// <summary>
        /// 玩家离开匹配队列
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        /// <returns>离开是否成功</returns>
        public bool LeaveMatch(MOBAClient client, int playerID)
        {
            //安全检测
            if (!roomIDs.ContainsKey(playerID))
                return false;
            int roomID = roomIDs[playerID];
            MatchRoom room;
            //检测 （防止多线程造成不必要的错误）
            if (!Rooms.TryGetValue(roomID, out room))
                return false;
            room.Leave(client, playerID);
            if (room.RoomIsEmpty())
            {
                //移除映射关系
                roomIDs.Remove(playerID);
                Rooms.TryRemove(room.ID, out room);
                //移除定时任务
                if (!room.Guid.Equals(Guid.NewGuid()))
                    room.Timer.RemoveAction(room.Guid);
                //把房间入重用队列
                rooms.Enqueue(room);
            }
            return true;
        }
        /// <summary>
        /// 删除指定的房间
        /// </summary>
        /// <param name="room"></param>
        public void DeleteRoom(MatchRoom room)
        {
            //移除玩家ID和房间ID的映射
            foreach (int id in room.RedTeamIdList)
            {
                roomIDs.Remove(id);
            }
            foreach (int id in room.BlueTeamIdList)
            {
                roomIDs.Remove(id);
            }
            //移除房间ID和房间的映射
            Rooms.TryRemove(room.ID, out room);
            //清空房间信息
            room.RedTeamIdList.Clear();
            room.BlueTeamIdList.Clear();
            room.ClientList.Clear();
            //入重用队列
            rooms.Enqueue(room);
        }
        /// <summary>
        /// 玩家下线，离开匹配队列
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        public void OffLine(MOBAClient client,int playerID)
        {
            LeaveMatch(client, playerID);
        }
        #endregion
    }
}
