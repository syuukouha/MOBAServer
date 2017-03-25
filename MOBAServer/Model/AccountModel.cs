using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Model
{
    /// <summary>
    /// 账号数据模型
    /// </summary>
    public class AccountModel
    {
        private int id;
        private string account;
        private string password;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public AccountModel()
        {
            
        }

        public AccountModel(int id, string account, string password)
        {
            this.Id = id;
            this.Account = account;
            this.Password = password;
        }
    }
}
