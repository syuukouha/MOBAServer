using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Room
{
    public class MatchRoom : RoomBase<MOBAClient>
    {
        /// <summary>
        /// 队伍1玩家ID
        /// </summary>
        public List<int> RedTeamIdList;
        /// <summary>
        /// 队伍2玩家ID
        /// </summary>
        public List<int> BlueTeamIdList;

        public MatchRoom(int id, int count) : base(id, count)
        {
            int teamCount = count/2;
            RedTeamIdList = new List<int>(teamCount);
            BlueTeamIdList = new List<int>(teamCount);
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        public bool Enter(MOBAClient client, int playerID)
        {
            //判断容纳量是否大于数量
            if (RedTeamIdList.Capacity > RedTeamIdList.Count)
            {
                RedTeamIdList.Add(playerID);
                EnterRoom(client);
                return true;
            }
            else  if(BlueTeamIdList.Capacity > BlueTeamIdList.Count)
            {
                BlueTeamIdList.Add(playerID);
                EnterRoom(client);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="client"></param>
        /// <param name="playerID"></param>
        public bool Leave(MOBAClient client, int playerID)
        {
            //判断容纳量是否大于数量
            if (RedTeamIdList.Contains(playerID))
            {
                RedTeamIdList.Remove(playerID);
                ExitRoom(client);
                return true;
            }
            else if (BlueTeamIdList.Contains(playerID))
            {
                BlueTeamIdList.Remove(playerID);
                ExitRoom(client);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 房间是否满了=》如果满了 就开始选人
        /// </summary>
        /// <returns></returns>
        public bool RoomIsFull()
        {
            return RedTeamIdList.Capacity == RedTeamIdList.Count && BlueTeamIdList.Capacity == BlueTeamIdList.Count;
        } 

        /// <summary>
        /// 房间是否空了=》如果空了 就解散房间
        /// </summary>
        /// <returns></returns>
        public bool RoomIsEmpty()
        {
            return RedTeamIdList.Count == 0 && BlueTeamIdList.Count == 0;
        }
    }
}
