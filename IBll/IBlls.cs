
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
    }
	public partial interface IMachineroomService : IBaseService<Machineroom>
    {
    }
	public partial interface INewsinfoService : IBaseService<Newsinfo>
    {
    }
	public partial interface IReservationService : IBaseService<Reservation>
    {
    }
	public partial interface IUserinfoService : IBaseService<Userinfo>
    {
    }

}