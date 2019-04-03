using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public partial interface IReservationService : IBaseService<Reservation>
    {
        /// <summary>
        /// 根据给定的预约状态信息，和id集合，批量设置状态，事务操作
        /// </summary>
        /// <param name="status">需要设置的状态</param>
        /// <param name="idlist">ID集合</param>
        /// <returns></returns>
        bool SetStatus(int status,List<int> idlist);

        /// <summary>
        /// 根据给定的id集合，批量设置预约人已经到场，会判断预约时间是否是今天，事务操作
        /// </summary>
        /// <param name="idlist">ID集合</param>
        /// <returns></returns>
        bool SetStatusOver(List<int> idlist);


        /// <summary>
        /// 根据给定的id集合，批量删除预约信息
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        bool DeleteRes(List<int> idlist);
    }
}
