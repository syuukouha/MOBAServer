using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using MOBAServer.Cache;
using Photon.SocketServer;

namespace MOBAServer.Logic
{
    /// <summary>
    /// 账号逻辑处理
    /// </summary>
    public class AccountHandler:SingleSend,IOpHandler
    {
        /// <summary>
        /// 账号的缓存
        /// </summary>
        private AccountCache accountCache = Caches.Account;
        public void OnRequest(MOBAClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpAccount.Login:
                    AccountDto dto = JsonMapper.ToObject<AccountDto>(request[0].ToString());
                    OnLogin(client, dto.Account, dto.Password);
                    break;
                case OpAccount.Register:
                    string account = request[0].ToString();
                    string password = request[1].ToString();
                    OnRegister(client, account, password);
                    break;
            }
        }

        public void OnDisConnect(MOBAClient client)
        {
            accountCache.OffLineClient(client);
        }

        private void OnLogin(MOBAClient client, string account, string password)
        {
            //无效检测
            if (account == null || password == null)
                return;
            //验证在线
            if (accountCache.IsOnLine(account))
            {
                Send(client, OperationCode.AccountCode, OpAccount.Login, -1, "玩家在线");
                return;
            }
            if (accountCache.Match(account, password))
            {
                accountCache.OnLineClient(account, client);
                Send(client, OperationCode.AccountCode, OpAccount.Login, 0, "登录成功");
            }
            else
            {
                Send(client, OperationCode.AccountCode, OpAccount.Login, -2, "账号或密码错误");
            }
        }
        private void OnRegister(MOBAClient client, string account, string password)
        {
            //无效检测
            if (account == null || password == null)
                return;
            //重复检测
            if (accountCache.Has(account))
            {
                Send(client, OperationCode.AccountCode, OpAccount.Register, -1, "账号已存在");
                return;
            }
            accountCache.Add(account, password);
            Send(client, OperationCode.AccountCode, OpAccount.Register, 0, "注册成功");
        }
    }
}
