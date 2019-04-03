
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{


	public partial interface IComputerinfoDal:IBaseDal<Computerinfo>
    {
    }



	public partial interface IMachineroomDal:IBaseDal<Machineroom>
    {
    }



	public partial interface INewsinfoDal:IBaseDal<Newsinfo>
    {
    }



	public partial interface IReservationDal:IBaseDal<Reservation>
    {
    }



	public partial interface IUserinfoDal:IBaseDal<Userinfo>
    {
    }



}