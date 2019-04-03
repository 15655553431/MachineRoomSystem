using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    public enum StatusEnum
    {
        /// <summary>
        /// 正常使用 、已经预约未审核
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 电脑停用、已经预约审核不通过
        /// </summary>
        Block = 2,

        /// <summary>
        /// 离校、已经预约审核通过
        /// </summary>
        Leave = 3,

        /// <summary>
        /// 隐藏电脑、已经预约审核通过且过来使用了电脑
        /// </summary>
        Over = 4

        /// <summary>
        /// 、已经预约审核通过,但是并没有来的（这种情况应该是超时自动判断的）
        /// </summary>

    }
}
