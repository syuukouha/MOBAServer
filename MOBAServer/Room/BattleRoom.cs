using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBACommon.Config;
using MOBACommon.Dto;

namespace MOBAServer.Room
{
    /// <summary>
    /// 对战房间
    /// </summary>
    public class BattleRoom:RoomBase<MOBAClient>
    {
        #region RedTeam
        //英雄
        public Dictionary<int,HeroModel> RedTeamHeroModels = new Dictionary<int, HeroModel>();
        //小兵
        public Dictionary<int,SoldierModel> RedTeamSoldierModels = new Dictionary<int, SoldierModel>();
        //防御塔
        public Dictionary<int, BuildModel> RedTeamBuildModels = new Dictionary<int, BuildModel>();

        #endregion

        #region BlueTeam
        //英雄
        public Dictionary<int, HeroModel> BlueTeamHeroModels = new Dictionary<int, HeroModel>();
        //小兵
        public Dictionary<int, SoldierModel> BlueTeamSoldierModels = new Dictionary<int, SoldierModel>();
        //防御塔
        public Dictionary<int, BuildModel> BlueTeamBuildModels = new Dictionary<int, BuildModel>();
        #endregion

        /// <summary>
        /// 是否全部进入
        /// </summary>
        public bool IsAllEnter
        {
            get { return ClientList.Count >= Count; }
        }
        /// <summary>
        /// 是否全部进入
        /// </summary>
        public bool IsAllLeave
        {
            get { return ClientList.Count <= 0; }
        }

        /// <summary>
        /// 逃跑的客户端
        /// </summary>
        public List<MOBAClient> LeaveClients = new List<MOBAClient>();

        public BattleRoom(int id, int count) : base(id, count)
        {

        }
        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="redTeam"></param>
        /// <param name="blueTeam"></param>
        public void Init(List<SelectModel> redTeam, List<SelectModel> blueTeam)
        {
            //玩家数量
            this.Count = redTeam.Count + blueTeam.Count;
            //初始化英雄数据
            foreach (SelectModel selectModel in redTeam)
            {
                RedTeamHeroModels.Add(selectModel.PlayerID, GetHeroModel(selectModel, 1));
            }
            foreach (SelectModel selectModel in blueTeam)
            {
                RedTeamHeroModels.Add(selectModel.PlayerID, GetHeroModel(selectModel,2));
            }
            //初始化建筑物数据   红队：-10 蓝队：-20

            RedTeamBuildModels.Add(-10, GetBuildModel(-10, BuildData.Base, 1));
            RedTeamBuildModels.Add(-11, GetBuildModel(-11, BuildData.MilitaryCamp, 1));
            RedTeamBuildModels.Add(-12, GetBuildModel(-12, BuildData.MilitaryCamp, 1));
            RedTeamBuildModels.Add(-13, GetBuildModel(-13, BuildData.Tower, 1));
            RedTeamBuildModels.Add(-14, GetBuildModel(-14, BuildData.Tower, 1));

            BlueTeamBuildModels.Add(-20, GetBuildModel(-20, BuildData.Base, 2));
            BlueTeamBuildModels.Add(-21, GetBuildModel(-21, BuildData.MilitaryCamp, 2));
            BlueTeamBuildModels.Add(-22, GetBuildModel(-22, BuildData.MilitaryCamp, 2));
            BlueTeamBuildModels.Add(-23, GetBuildModel(-23, BuildData.Tower, 2));
            BlueTeamBuildModels.Add(-24, GetBuildModel(-24, BuildData.Tower, 2));


            //初始化小兵数据


        }
        /// <summary>
        /// 获取英雄数据
        /// </summary>
        /// <param name="selectModel"></param>
        /// <param name="teamID"></param>
        /// <returns></returns>
        private HeroModel GetHeroModel(SelectModel selectModel,int teamID)
        {
            //先从静态配置表里面 获取到英雄的数据
            HeroDataModel heroDataModel = HeroData.GetHeroDataModel(selectModel.HeroID);
            //构建英雄数据
            HeroModel heroModel = new HeroModel(selectModel.PlayerID, heroDataModel.ID, teamID, heroDataModel.Name,
                heroDataModel.HP, heroDataModel.BaseAttack, heroDataModel.BaseDefense, heroDataModel.AttackDistance,
                heroDataModel.MP, heroDataModel.SkillID);
            return heroModel;
        }
        /// <summary>
        /// 获取建筑物数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeID"></param>
        /// <param name="teamID"></param>
        /// <returns></returns>
        private BuildModel GetBuildModel(int id,int typeID,int teamID)
        {
            //先从静态配置表里面 获取到英雄的数据
            BuildDataModel buildDataModel = BuildData.GetBuildDataModel(typeID);
            //构建英雄数据
            BuildModel buildModel = new BuildModel(id, typeID, teamID, buildDataModel.Name, buildDataModel.HP,
                buildDataModel.Attack, buildDataModel.Defence, buildDataModel.AttackDistance, buildDataModel.IsAttack,
                buildDataModel.IsRebirth, buildDataModel.RebirthTime);
            return buildModel;
        }

        private int soldierID = -1000;
        public int SoldierId
        {
            get { return soldierID; }
            set { soldierID = value; }
        }
        /// <summary>
        /// 开启定时任务：30秒之后产生小兵
        /// </summary>
        private void SpawnSoldier()
        {
            this.StartSchedule(DateTime.UtcNow.AddSeconds(30), delegate
            {
                 List<SoldierModel> soldiers = new List<SoldierModel>();
                //产生小兵
                SoldierModel soldierModel = new SoldierModel();
                //TODO
                //添加映射
                RedTeamSoldierModels.Add(soldierModel.ID, soldierModel);
                soldiers.Add(soldierModel);

                soldierModel = new SoldierModel();
                BlueTeamSoldierModels.Add(soldierModel.ID, soldierModel);
                soldiers.Add(soldierModel);

                //自身调用自身，无限递归
                SpawnSoldier();
            });
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        public void Enter(MOBAClient client)
        {
            if (!ClientList.Contains(client))
                ClientList.Add(client);
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        public void Leave(MOBAClient client)
        {
            if (ClientList.Contains(client))
                ClientList.Remove(client);
            if (!LeaveClients.Contains(client))
                LeaveClients.Add(client);
        }
        /// <summary>
        /// 清空房间数据
        /// </summary>
        public void Clear()
        {
            RedTeamHeroModels.Clear();
            RedTeamBuildModels.Clear();
            RedTeamSoldierModels.Clear();
            BlueTeamHeroModels.Clear();
            BlueTeamBuildModels.Clear();
            BlueTeamSoldierModels.Clear();
            LeaveClients.Clear();
            ClientList.Clear();
        }
    }
}
