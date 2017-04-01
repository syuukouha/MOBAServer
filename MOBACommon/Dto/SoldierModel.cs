using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    /// <summary>
    /// 小兵模型
    /// </summary>
    public class SoldierModel
    {
        /// <summary>
        /// 身份ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 队伍ID
        /// </summary>
        public int TeamID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrentHP { get; set; }
        /// <summary>
        /// 最大血量
        /// </summary>
        public int MaxHP { get; set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack { get; set; }
        /// <summary>
        /// 防御力
        /// </summary>
        public int Defence { get; set; }
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance { get; set; }

        public SoldierModel()
        {
            
        }

        public SoldierModel(int id,int typeID,int teamID,string name,int maxHP,int attack,int defence,double attackDistance)
        {
            this.ID = id;
            this.TypeID = typeID;
            this.TeamID = teamID;
            this.Name = name;
            this.MaxHP = maxHP;
            this.Attack = attack;
            this.Defence = defence;
            this.AttackDistance = attackDistance;
        }
    }
}
