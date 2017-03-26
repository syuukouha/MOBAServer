using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    public class PlayerDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID;
        /// <summary>
        /// 角色名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv;
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp;
        /// <summary>
        /// 战斗力
        /// </summary>
        public int Power;
        /// <summary>
        /// 胜利场次
        /// </summary>
        public int WinCount;
        /// <summary>
        /// 失败场次
        /// </summary>
        public int LoseCount;
        /// <summary>
        /// 逃跑场次
        /// </summary>
        public int RunCount;

        /// <summary>
        /// 英雄ID列表
        /// </summary>
        public int[] HeroID;

        /// <summary>
        /// 好友
        /// </summary>
        public FriendDto[] Friends;

        public PlayerDto()
        {
            
        }
    }
}
