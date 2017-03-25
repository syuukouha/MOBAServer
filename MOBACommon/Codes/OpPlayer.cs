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
        /// 添加好友
        /// </summary>
        public const byte AddFriend = 3;
    }
}
