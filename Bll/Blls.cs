
 using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bll
{


	public partial class ComputerinfoService : BaseService<Computerinfo>, IComputerinfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.ComputerinfoDal;
        }
    }


	public partial class MachineroomService : BaseService<Machineroom>, IMachineroomService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.MachineroomDal;
        }
    }


	public partial class NewsinfoService : BaseService<Newsinfo>, INewsinfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.NewsinfoDal;
        }
    }


	public partial class ReservationService : BaseService<Reservation>, IReservationService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.ReservationDal;
        }
    }


	public partial class UserinfoService : BaseService<Userinfo>, IUserinfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.UserinfoDal;
        }
    }


}