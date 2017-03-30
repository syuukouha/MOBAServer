using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Config
{
    /// <summary>
    /// 英雄数据
    /// </summary>
    public class HeroData
    {
        private static Dictionary<int,HeroModel> _heroModels = new Dictionary<int, HeroModel>();
        static HeroData()
        {
            //英雄ID范围：1-99
            //技能ID范围：英雄ID+001 002 003 004 {1001，1002,1003,1004}
            CreateHero(1, "瞎子", 60, 20, 300, 100, 10, 3, 50, 10, 1, new[] {1001, 1002, 1003, 1004});
            CreateHero(2, "狐狸", 50, 10, 100, 300, 8, 2, 40, 10, 5, new[] {2001, 2002, 2003, 2004});
        }

        public static HeroModel GetHeroModel(int heroID)
        {
            HeroModel heroModel = null;
            _heroModels.TryGetValue(heroID, out heroModel);
            return heroModel;
        }
        /// <summary>
        /// 创建英雄
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="growUpHP"></param>
        /// <param name="mp"></param>
        /// <param name="growUpMP"></param>
        /// <param name="baseAttack"></param>
        /// <param name="baseDefense"></param>
        /// <param name="growUpAttack"></param>
        /// <param name="growUpDefense"></param>
        /// <param name="attackDistance"></param>
        /// <param name="skillID"></param>
        /// <returns></returns>
        private static void CreateHero(int id, string name, int hp, int growUpHP, int mp, int growUpMP, int baseAttack,
            int baseDefense, int growUpAttack, int growUpDefense, double attackDistance, int[] skillID)
        {
            HeroModel heroModel = new HeroModel(id, name, hp, growUpHP, mp, growUpMP, baseAttack,
                baseDefense, growUpAttack, growUpDefense, attackDistance, skillID);
            //保存英雄数据
            _heroModels.Add(heroModel.ID, heroModel);
        }
    }
    /// <summary>
    /// 英雄的数据模型
    /// </summary>
    public class HeroModel
    {
        /// <summary>
        /// 英雄编号
        /// </summary>
        public int ID;

        /// <summary>
        /// 英雄名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 生命值
        /// </summary>
        public int HP;
        /// <summary>
        /// 成长生命值
        /// </summary>
        public int GrowUpHP;
        /// <summary>
        /// 魔法值
        /// </summary>
        public int MP;
        /// <summary>
        /// 成长魔法值
        /// </summary>
        public int GrowUpMP;
        /// <summary>
        /// 基础攻击力
        /// </summary>
        public int BaseAttack;
        /// <summary>
        /// 基础防御力
        /// </summary>
        public int BaseDefense;
        /// <summary>
        /// 成长攻击力
        /// </summary>
        public int GrowUpAttack;
        /// <summary>
        /// 成长防御力
        /// </summary>
        public int GrowUpDefense;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance;
        /// <summary>
        /// 技能ID
        /// </summary>
        public int[] SkillID;


        public HeroModel()
        {
            
        }

        public HeroModel(int id, string name, int hp, int growUpHP, int mp, int growUpMP, int baseAttack,
            int baseDefense, int growUpAttack, int growUpDefense, double attackDistance, int[] skillID)
        {
            this.ID = id;
            this.Name = name;
            this.HP = hp;
            this.GrowUpHP = growUpHP;
            this.MP = mp;
            this.GrowUpMP = growUpMP;
            this.BaseAttack = baseAttack;
            this.BaseDefense = baseDefense;
            this.GrowUpAttack = growUpAttack;
            this.GrowUpDefense = growUpDefense;
            this.AttackDistance = attackDistance;
            this.SkillID = skillID;
        }

    }
}
