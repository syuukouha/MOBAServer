using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Model
{
    public class PlayerModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        private int id;
        /// <summary>
        /// 角色名字
        /// </summary>
        private string name;
        /// <summary>
        /// 等级
        /// </summary>
        private int lv;
        /// <summary>
        /// 经验
        /// </summary>
        private int exp;
        /// <summary>
        /// 战斗力
        /// </summary>
        private int power;
        /// <summary>
        /// 胜利场次
        /// </summary>
        private int winCount;
        /// <summary>
        /// 失败场次
        /// </summary>
        private int loseCount;
        /// <summary>
        /// 逃跑场次
        /// </summary>
        private int runCount;
        /// <summary>
        /// 英雄ID列表
        /// </summary>
        private string heroIDList;
        /// <summary>
        /// 好友ID列表
        /// </summary>
        private string friendIDList;
        private int accountID;

        #region Property
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Lv
        {
            get { return lv; }
            set { lv = value; }
        }

        public int Exp
        {
            get { return exp; }
            set { exp = value; }
        }

        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        public int WinCount
        {
            get { return winCount; }
            set { winCount = value; }
        }

        public int LoseCount
        {
            get { return loseCount; }
            set { loseCount = value; }
        }

        public int RunCount
        {
            get { return runCount; }
            set { runCount = value; }
        }

        public string HeroIdList
        {
            get { return heroIDList; }
            set { heroIDList = value; }
        }

        public string FriendIdList
        {
            get { return friendIDList; }
            set { friendIDList = value; }
        }

        public int AccountId
        {
            get { return accountID; }
            set { accountID = value; }
        } 
        #endregion

        public PlayerModel()
        {
            
        }
        public PlayerModel(int id,string name,int accountID)
        {
            Id = id;
            Name = name;
            AccountId = accountID;
            Lv = 1;
            Exp = 0;
            Power = 2000;
            WinCount = 0;
            LoseCount = 0;
            RunCount = 0;
            HeroIdList = "0,1";
            FriendIdList = string.Empty;
        }
    }
}
