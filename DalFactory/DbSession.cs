
 using EFDal;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalFactory
{
    public class DbSession:IDbSession
    {


		public IComputerinfoDal ComputerinfoDal
        {
            get { return StaticDalFactory.GetComputerinfoDal(); }
        }


		public IMachineroomDal MachineroomDal
        {
            get { return StaticDalFactory.GetMachineroomDal(); }
        }


		public INewsinfoDal NewsinfoDal
        {
            get { return StaticDalFactory.GetNewsinfoDal(); }
        }


		public IReservationDal ReservationDal
        {
            get { return StaticDalFactory.GetReservationDal(); }
        }


		public IUserinfoDal UserinfoDal
        {
            get { return StaticDalFactory.GetUserinfoDal(); }
        }

		/// <summary>
        /// 拿到当前的EF的上下文，然后进行 把修改的实体进行一个整体提交
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return DbContextFactory.GetCurrentDbContext().SaveChanges();
        }
	}
}