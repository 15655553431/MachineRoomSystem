using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 预约时段的枚举类
    /// </summary>
    public enum DateFlagEnum
    {
        /// <summary>
        /// 上午第1、2节课
        /// </summary>
        Morning = 1,

        /// <summary>
        /// 上午第3、4节课
        /// </summary>
        Forenoon = 2,

        /// <summary>
        /// 下午时段
        /// </summary>
        Afternoon = 3,

        /// <summary>
        /// 晚自习时段
        /// </summary>
        Night = 4


    }
}
