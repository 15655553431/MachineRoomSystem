using Common.Md5;
using DalFactory;
using IBll;
using IDal;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public partial class UserinfoService : BaseService<Userinfo>, IUserinfoService
    {
        
        public bool LoginExist(string login)
        {
            return GetEntities(u => u.Login == login && u.Delflag == (int)DelflagEnum.Normal).Count() > 0;
        }


    

        public bool EditPwd(int id, string newpwd)
        {
            Userinfo ui = CurrentDal.GetEntities(u => u.ID==id).First();
            if (ui != null)
            {
                ui.Pwd = newpwd.GetMd5();
                if (Update(ui))
                {
                    return true;
                }
                return false;
            }
            return false;
        }


        public bool LoginExist(int id, string login)
        {
            return GetEntities(u => u.Login == login && u.ID != id && u.Delflag == (int)DelflagEnum.Normal).Count() > 0;
        }
    }
}
