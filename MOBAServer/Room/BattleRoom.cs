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
        private Dictionary<int,HeroModel> _redTeamHeroModels = new Dictionary<int, HeroModel>();
        //小兵
        private Dictionary<int,SoldierModel> _redTeamSoldierModels = new Dictionary<int, SoldierModel>();
        //防御塔
        private Dictionary<int, BuildModel> _redTeamBuildModels = new Dictionary<int, BuildModel>();

        #endregion

        #region BlueTeam
        //英雄
        private Dictionary<int, HeroModel> _blueTeamHeroModels = new Dictionary<int, HeroModel>();
        //小兵
        private Dictionary<int, SoldierModel> _blueTeamSoldierModels = new Dictionary<int, SoldierModel>();
        //防御塔
        private Dictionary<int, BuildModel> _blueTeamBuildModels = new Dictionary<int, BuildModel>();
        #endregion

        /// <summary>
        /// 取得所有的英雄
        /// </summary>
        public HeroModel[] HeroModels
        {
            get
            {
                List<HeroModel> heroModels = new List<HeroModel>();
                heroModels.AddRange(_redTeamHeroModels.Values);
                heroModels.AddRange(_blueTeamHeroModels.Values);
                return heroModels.ToArray();
            }
        }
        /// <summary>
        /// 取得所有的建筑
        /// </summary>
        public BuildModel[] BuildModels
        {
            get
            {
                List<BuildModel> buildModels = new List<BuildModel>();
                buildModels.AddRange(_redTeamBuildModels.Values);
                buildModels.AddRange(_blueTeamBuildModels.Values);
                return buildModels.ToArray();
            }
        }
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
        /// <param name="redTeamSelectModels"></param>
        /// <param name="blueTeamSelectModels"></param>
        public void Init(List<SelectModel> redTeamSelectModels, List<SelectModel> blueTeamSelectModels)
        {
            //初始化英雄数据
            foreach (SelectModel selectModel in redTeamSelectModels)
            {
                _redTeamHeroModels.Add(selectModel.PlayerID, GetHeroModel(selectModel, 1));
            }
            foreach (SelectModel selectModel in blueTeamSelectModels)
            {
                _blueTeamHeroModels.Add(selectModel.PlayerID, GetHeroModel(selectModel,2));
            }
            //初始化建筑物数据   红队：-10 蓝队：-20

            _redTeamBuildModels.Add(-10, GetBuildModel(-10, BuildData.Base, 1));
            _redTeamBuildModels.Add(-11, GetBuildModel(-11, BuildData.MilitaryCamp, 1));
            _redTeamBuildModels.Add(-12, GetBuildModel(-12, BuildData.MilitaryCamp, 1));
            _redTeamBuildModels.Add(-13, GetBuildModel(-13, BuildData.Tower, 1));
            _redTeamBuildModels.Add(-14, GetBuildModel(-14, BuildData.Tower, 1));

            _blueTeamBuildModels.Add(-20, GetBuildModel(-20, BuildData.Base, 2));
            _blueTeamBuildModels.Add(-21, GetBuildModel(-21, BuildData.MilitaryCamp, 2));
            _blueTeamBuildModels.Add(-22, GetBuildModel(-22, BuildData.MilitaryCamp, 2));
            _blueTeamBuildModels.Add(-23, GetBuildModel(-23, BuildData.Tower, 2));
            _blueTeamBuildModels.Add(-24, GetBuildModel(-24, BuildData.Tower, 2));


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
                _redTeamSoldierModels.Add(soldierModel.ID, soldierModel);
                soldiers.Add(soldierModel);

                soldierModel = new SoldierModel();
                _blueTeamSoldierModels.Add(soldierModel.ID, soldierModel);
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
            _redTeamHeroModels.Clear();
            _redTeamBuildModels.Clear();
            _redTeamSoldierModels.Clear();
            _blueTeamHeroModels.Clear();
            _blueTeamBuildModels.Clear();
            _blueTeamSoldierModels.Clear();
            LeaveClients.Clear();
            ClientList.Clear();
        }
    }
}
