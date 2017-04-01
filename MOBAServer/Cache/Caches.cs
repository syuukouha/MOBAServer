using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBAServer.Cache
{
    public class Caches
    {
        public static AccountCache Account;
        public static PlayerCache Player;
        public static MatchCache Match;
        public static SelectCache Select;
        public static BattleCache Battle;

        static Caches()
        {
            Account = new AccountCache();
            Player = new PlayerCache();
            Match = new MatchCache();
            Select = new SelectCache();
            Battle = new BattleCache();
        }
    }
}
