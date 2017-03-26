using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Dto
{
    public class FriendDto
    {
        public int ID;
        public string Name;
        public bool IsOnline;

        public FriendDto()
        {
            
        }

        public FriendDto(int id, string name, bool isOnline)
        {
            this.ID = id;
            this.Name = name;
            this.IsOnline = isOnline;
        }
    }
}
