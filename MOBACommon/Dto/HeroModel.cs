using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    /// <summary>
    /// 英雄模型
    /// </summary>
    public class HeroModel:SoldierModel
    {
        /// <summary>
        /// 当前魔法值
        /// </summary>
        public int CurrentMP { get; set; }
        /// <summary>
        /// 最大魔法值
        /// </summary>
        public int MaxMP { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// 钱
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 装备
        /// </summary>
        public int[] Equipments { get; set; }
        /// <summary>
        /// 技能
        /// </summary>
        public int[] Skills { get; set; }
        /// <summary>
        /// 加点
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// 补兵
        /// </summary>
        public int CS { get; set; }
        /// <summary>
        /// 击杀次数
        /// </summary>
        public int Kill { get; set; }
        /// <summary>
        /// 死亡次数
        /// </summary>
        public int Dead { get; set; }

        public HeroModel()
        {
            
        }

        public HeroModel(int id, int typeID, int teamID, string name, int maxHP, int attack, int defence,
            double attackDistance,int maxMP,int[] skills) : base(id, typeID, teamID, name, maxHP, attack, defence, attackDistance)
        {
            this.MaxMP = maxMP;
            this.CurrentMP = maxMP;
            this.Lv = 1;
            this.Exp = 0;
            this.Money = 500;
            this.Equipments = new int[6];
            this.Skills = skills;
            Point = 1;
            CS = 0;
            Kill = 0;
            Dead = 0;
        }
    }
}
