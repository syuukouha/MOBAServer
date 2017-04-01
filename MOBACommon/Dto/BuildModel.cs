using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    /// <summary>
    /// 建筑物模型
    /// </summary>
    public class BuildModel:SoldierModel
    {
        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool IsAttack { get; set; }
        /// <summary>
        /// 是否重生
        /// </summary>
        public bool IsRebirth { get; set; }
        /// <summary>
        /// 重生时间
        /// </summary>
        public int RebirthTime { get; set; }


        public BuildModel()
        {
            
        }
        public BuildModel(int id, int typeID, int teamID, string name, int maxHP, int attack, int defence,
            double attackDistance, bool isAttack,bool isRebirth,int rebirthTime) : base(id, typeID, teamID, name, maxHP, attack, defence, attackDistance)
        {
            this.IsAttack = isAttack;
            this.IsRebirth = isRebirth;
            this.RebirthTime = rebirthTime;
        }
    }
}
