using IBll;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public partial class ReservationService : BaseService<Reservation>, IReservationService
    {
        public bool SetStatus(int status, List<int> idlist)
        {
            foreach (int id in idlist)
            {
                Reservation entity = GetEntities(u => u.ID == id).First();
                entity.Status = status;
                CurrentDal.Update(entity);
            }

            return DbSession.SaveChanges() > 0;
        }


        public bool SetStatusOver(List<int> idlist)
        {
            foreach (int id in idlist)
            {
                Reservation entity = GetEntities(u => u.ID == id).First();
                if (entity.Sdate != DateTime.Now.Date || entity.Status != (int)StatusEnum.Leave)
                {
                    return false;
                }
                entity.Status = (int)StatusEnum.Over;
                CurrentDal.Update(entity);
            }

            return DbSession.SaveChanges() > 0;
        }


        public bool DeleteRes(List<int> idlist)
        {
            foreach (int id in idlist)
            {
                Reservation entity = GetEntities(u => u.ID == id).First();
                if (entity.Status == (int)StatusEnum.Normal)
                {
                    CurrentDal.Delete(entity);
                }
            }

            return DbSession.SaveChanges() > 0;
        }
    }
}
