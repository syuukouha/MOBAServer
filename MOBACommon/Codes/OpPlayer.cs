using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Codes
{
    /// <summary>
    /// 角色操作码
    /// </summary>
    public class OpPlayer
    {
        /// <summary>
        /// 获取角色信息
        /// </summary>
        public const byte GetPlayerInfo = 0;
        /// <summary>
        /// 创建角色
        /// </summary>
        public const byte CreatePlayer = 1;
        /// <summary>
        /// 上线
        /// </summary>
        public const byte Online = 2;
        /// <summary>
        /// 添加好友(向服务器请求)
        /// </summary>
        public const byte AddFriend = 3;
        /// <summary>
        /// 添加好友（服务器给客户端请求）
        /// </summary>
        public const byte AddFriendToClient = 4;
        /// <summary>
        /// 好友在线状态变更
        /// </summary>
        public const byte FriendOnlineState = 5;
        /// <summary>
        /// 匹配开始
        /// </summary>
        public const byte MatchStart = 6;
        /// <summary>
        /// 匹配完成
        /// </summary>
        public const byte MatchComplete = 7;
        /// <summary>
        /// 匹配终止
        /// </summary>
        public const byte MatchStop = 8;

    }
}
