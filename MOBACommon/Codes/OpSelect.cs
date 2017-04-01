using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBACommon.Codes
{
    public class OpSelect
    {
        /// <summary>
        /// 进入
        /// </summary>
        public const byte Enter = 0;
        /// <summary>
        /// 选人
        /// </summary>
        public const byte Select = 1;
        /// <summary>
        /// 准备
        /// </summary>
        public const byte Ready = 2;
        /// <summary>
        /// 取消
        /// </summary>
        public const byte Cancel = 3;
        /// <summary>
        /// 获取房间信息
        /// </summary>
        public const byte GetInfo = 4;
        /// <summary>
        /// 聊天
        /// </summary>
        public const byte Chat = 5;
        /// <summary>
        /// 开始战斗
        /// </summary>
        public const byte StartBattle = 6;
    }
}
