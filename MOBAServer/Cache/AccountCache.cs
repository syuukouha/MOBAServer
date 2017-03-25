using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOBAServer.Model;

namespace MOBAServer.Cache
{
    #region 数据
    /// <summary>
    /// 账号缓存层
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 账号和模型的映射
        /// </summary>
        private Dictionary<string, AccountModel> accountModels = new Dictionary<string, AccountModel>();
        /// <summary>
        /// 匹配账号密码是否存在且正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Match(string account, string password)
        {
            if (!accountModels.ContainsKey(account))
                return false;
            return accountModels[account].Password == password;
        }
        //模拟ID
        private int id = 0;

        /// <summary>
        /// 添加账号信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Add(string account, string password)
        {
            if (Has(account))
                return false;
            //添加
            accountModels[account] = new AccountModel(id, account, password);
            id++;
            return true;
        }
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool Has(string account)
        {
            return accountModels.ContainsKey(account);
        }
        #endregion

        #region 在线玩家
        private Dictionary<MOBAClient,string> clients = new Dictionary<MOBAClient,string>();

        public bool IsOnLine(string account)
        {
            return clients.ContainsValue(account);
        }
        //添加在线客户端
        public bool OnLineClient(string account, MOBAClient client)
        {
            if (IsOnLine(account))
                return false;
            clients[client] = account;
            return true;
        }
        //删除在线客户端
        public void OffLineClient(MOBAClient client)
        {
            if (clients.ContainsKey(client))
                clients.Remove(client);
        }
        #endregion
        /// <summary>
        /// 根据连接对象获取账号ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns>有就返回，没有返回-1</returns>
        public int GetID(MOBAClient client)
        {
            if (!clients.ContainsKey(client))
                return -1;
            string account = clients[client];
            if (!accountModels.ContainsKey(account))
                return -1;
            return accountModels[account].Id;
        }
    }
}
