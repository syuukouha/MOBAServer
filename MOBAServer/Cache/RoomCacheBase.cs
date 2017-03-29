using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Threading;
using MOBAServer.Room;

namespace MOBAServer.Cache
{
    public class RoomCacheBase<TRoom> where TRoom:RoomBase<MOBAClient>
    {
        #region 数据
        /// <summary>
        /// 房间ID对应的房间数据
        /// </summary>
        protected ConcurrentDictionary<int, TRoom> Rooms = new ConcurrentDictionary<int, TRoom>();
        /// <summary>
        /// 玩家ID对应的房间ID
        /// </summary>
        protected SynchronizedDictionary<int, int> roomIDs = new SynchronizedDictionary<int, int>();

        /// <summary>
        /// 重用的队列
        /// </summary>
        protected ConcurrentQueue<TRoom> rooms = new ConcurrentQueue<TRoom>();

        /// <summary>
        /// 主键ID
        /// </summary>
        protected int index = 0;
        #endregion
    }
}
