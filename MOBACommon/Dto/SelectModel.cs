using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    public class SelectModel
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int PlayerID;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 选择英雄ID
        /// </summary>
        public int HeroID;
        /// <summary>
        /// 是否进入
        /// </summary>
        public bool IsEnter;
        /// <summary>
        /// 是否准备
        /// </summary>
        public bool IsReady;

        public SelectModel()
        {
            this.HeroID = -1;
            this.IsEnter = false;
            this.IsReady = false;
        }
        public SelectModel(int playerID, string playerName)
        {
            this.PlayerID = playerID;
            this.PlayerName = playerName;
            this.HeroID = -1;
            this.IsEnter = false;
            this.IsReady = false;
        }
    }
}
