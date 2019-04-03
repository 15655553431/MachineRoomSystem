
 using IDal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDal
{

	public partial class ComputerinfoDal:BaseDal<Computerinfo>,IComputerinfoDal
    {
       
    }

	public partial class MachineroomDal:BaseDal<Machineroom>,IMachineroomDal
    {
       
    }

	public partial class NewsinfoDal:BaseDal<Newsinfo>,INewsinfoDal
    {
       
    }

	public partial class ReservationDal:BaseDal<Reservation>,IReservationDal
    {
       
    }

	public partial class UserinfoDal:BaseDal<Userinfo>,IUserinfoDal
    {
       
    }


}