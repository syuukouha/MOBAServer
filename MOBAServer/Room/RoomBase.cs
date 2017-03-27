using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Threading;
using Photon.SocketServer;

namespace MOBAServer.Room
{
    /// <summary>
    /// 房间基类
    /// </summary>
    public class RoomBase<TClient> where TClient:ClientPeer
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 连接对象的集合
        /// </summary>
        public List<TClient> ClientList;

        /// <summary>
        /// 房间的容纳量
        /// </summary>
        public int Count;
        /// <summary>
        /// 定时器
        /// </summary>
        public Timer Timer;

        /// <summary>
        /// 定时任务的ID
        /// </summary>
        public Guid Guid;

        public RoomBase(int id, int count)
        {
            this.ID = id;
            this.Count = count;
            ClientList = new List<TClient>();
            Guid = Guid.NewGuid();
            Timer = new Timer();
            Timer.Start();
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="client"></param>
        protected bool EnterRoom(TClient client)
        {
            if (ClientList.Contains(client))
                return false;
            ClientList.Add(client);
            return true;
        }
        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="client"></param>
        protected bool ExitRoom(TClient client)
        {
            if (!ClientList.Contains(client))
                return false;
            ClientList.Remove(client);
            return true;
        }
        /// <summary>
        /// 开启定时任务
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="callback"></param>
        public void StartSchedule(DateTime utcTime, Action callback)
        {
            this.Guid = Timer.AddAction(utcTime, callback);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作码</param>
        /// <param name="parameters">参数</param>
        public void Broadcast(byte opCode, byte subCode, short returnCode, string message, params object[] parameters)
        {
            OperationResponse response = new OperationResponse();
            response.OperationCode = opCode;
            response.Parameters = new Dictionary<byte, object>();
            response[80] = subCode;
            for (int i = 0; i < parameters.Length; i++)
            {
                response[(byte)i] = parameters[i];
            }
            response.ReturnCode = returnCode;
            response.DebugMessage = message;
            foreach (TClient client in ClientList)
            {
                client.SendOperationResponse(response, new SendParameters());
            }
        }
    }
}
