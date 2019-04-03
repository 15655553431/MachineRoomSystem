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
    public partial class ComputerinfoService : BaseService<Computerinfo>, IComputerinfoService
    {

        public bool RCExist(int row, int col, int id)
        {
            IMachineroomService machineroomService = new MachineroomService();
            Machineroom mr = machineroomService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.ID == id).First();
            if (mr.Crow < row || mr.Ccol < col)
            {
                return false;
            }
            if (GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.Row == row && u.Col == col && u.MachineroomID == mr.ID).Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
