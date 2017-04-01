using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Config
{
    /// <summary>
    /// 建筑数据
    /// </summary>
    public class BuildData
    {
        /// <summary>
        /// 基地
        /// </summary>
        public const int Base = 1;
        /// <summary>
        /// 兵营
        /// </summary>
        public const int MilitaryCamp = 2;
        /// <summary>
        /// 防御塔
        /// </summary>
        public const int Tower = 3;

        /// <summary>
        /// 类型和数据模型的映射
        /// </summary>
        static Dictionary<int, BuildDataModel> buildDataModels = new Dictionary<int, BuildDataModel>();

        static BuildData()
        {
            CreateBuildDataModel(Base, 5000, -1, 100, -1, false, false, -1, "基地");
            CreateBuildDataModel(MilitaryCamp, 3000, -1, 100, -1, false, true, 300, "兵营");
            CreateBuildDataModel(MilitaryCamp, 3000, -1, 100, -1, false, true, 300, "兵营");
            CreateBuildDataModel(Tower, 5000, 200, 50, 5, true, false, -1, "炮塔");
            CreateBuildDataModel(Tower, 5000, 200, 50, 5, true, false, -1, "炮塔");

        }

        private static void CreateBuildDataModel(int typeID, int hp, int attack, int defence, double attackDisatance,
            bool isAttack, bool isRebirth, int rebirthTime, string name)
        {
            BuildDataModel towerDataModel = new BuildDataModel(typeID, hp, attack, defence, attackDisatance, isAttack,
                isRebirth, rebirthTime, name);
            buildDataModels.Add(towerDataModel.TypeID, towerDataModel);
        }
        /// <summary>
        /// 根据类型ID获取数据模型
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public static BuildDataModel GetBuildDataModel(int typeID)
        {
            BuildDataModel buildDataModel;
            buildDataModels.TryGetValue(typeID, out buildDataModel);
            return buildDataModel;
        }
    }

    /// <summary>
    /// 建筑数据模型
    /// </summary>
    public class BuildDataModel
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 血量
        /// </summary>
        public int HP { get; set; }
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
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public BuildDataModel(int typeID,int hp,int attack,int defence,double attackDisatance,bool isAttack,bool isRebirth,int rebirthTime,string name)
        {
            this.TypeID = typeID;
            this.HP = hp;
            this.Attack = attack;
            this.Defence = defence;
            this.AttackDistance = attackDisatance;
            this.IsAttack = isAttack;
            this.IsRebirth = isRebirth;
            this.RebirthTime = rebirthTime;
            this.Name = name;
        }
    }
}
