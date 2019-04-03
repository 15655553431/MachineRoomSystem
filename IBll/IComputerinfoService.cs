using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public partial interface IComputerinfoService : IBaseService<Computerinfo>
    {
        /// <summary>
        /// 根据给定的行列信息和机房id，若该行列范围正确，而且当前位置为空可放置电脑返回true，否则false
        /// </summary>
        /// <param name="row">所在行</param>
        /// <param name="col">所在列</param>
        /// <param name="id">所在机房id</param>
        /// <returns></returns>
        bool RCExist(int row, int col,int id);
    }
}
