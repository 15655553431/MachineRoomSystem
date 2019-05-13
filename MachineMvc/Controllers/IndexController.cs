using Common.Cache;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineMvc.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            List<Meansinfo> milist = new List<Meansinfo>();
            //id,name,layer,pid,type,style
            milist.Add(new Meansinfo(1, "账号管理", "", 1, -1, 1, "&#xe616;"));
            milist.Add(new Meansinfo(2, "管理员账号", "/Userinfo/AdminIndex", 2, 1, 1, "&#xe616;"));
            milist.Add(new Meansinfo(3, "教职工账号", "/Userinfo/TeacherIndex", 2, 1, 1, "&#xe616;"));
            milist.Add(new Meansinfo(4, "学生账号", "/Userinfo/StudentIndex", 2, 1, 1, "&#xe616;"));

            milist.Add(new Meansinfo(5, "机房管理", "", 1, -1, 1, "&#xe613;"));
            milist.Add(new Meansinfo(6, "机房信息", "/Machineroom/Index", 2, 5, 1, "&#xe616;"));


            milist.Add(new Meansinfo(7, "电脑管理", "", 1, -1, 1, "&#xe620;"));
            milist.Add(new Meansinfo(8, "电脑信息", "/Computerinfo/Index", 2, 7, 1, "&#xe616;"));

            milist.Add(new Meansinfo(9, "新闻管理", "", 1, -1, 1, "&#xe622;"));
            milist.Add(new Meansinfo(10, "新闻信息", "/Newsinfo/Index", 2, 9, 1, "&#xe616;"));

            milist.Add(new Meansinfo(11, "预约管理", "", 1, -1, 1, "&#xe60d;"));
            milist.Add(new Meansinfo(12, "等待审核", "/Reservation/AdminResAwait", 2, 11, 1, "&#xe616;"));
            milist.Add(new Meansinfo(13, "审核通过", "/Reservation/AdminResPassed", 2, 11, 1, "&#xe616;"));
            milist.Add(new Meansinfo(14, "审核历史", "/Reservation/AdminResHistory", 2, 11, 1, "&#xe616;"));
            milist.Add(new Meansinfo(14, "预约统计", "/Reservation/AdminResChart", 2, 11, 1, "&#xe616;"));

            milist.Add(new Meansinfo(15, "我的桌面", "/Reservation/ShowLayout", 0, -1, 1, "&#xe616;"));


            //教师页面
            milist.Add(new Meansinfo(16, "个人管理", "", 1, -1, 2, "&#xe613;"));
            milist.Add(new Meansinfo(17, "个人信息", "/Userinfo/Details", 2, 16, 2, "&#xe616;"));

            milist.Add(new Meansinfo(40, "机房信息", "", 1, -1, 2, "&#xe613;"));
            milist.Add(new Meansinfo(41, "机房新闻", "/Newsinfo/NewsShow", 2, 40, 2, "&#xe616;"));

            milist.Add(new Meansinfo(18, "机房管理", "", 1, -1, 2, "&#xe613;"));
            milist.Add(new Meansinfo(19, "机房预约", "/TeacherMachineroom/Index", 2, 18, 2, "&#xe616;"));


            milist.Add(new Meansinfo(20, "电脑预约", "", 1, -1, 2, "&#xe620;"));
            milist.Add(new Meansinfo(21, "自主预约", "/StudentComputerinfo/Index", 2, 20, 2, "&#xe616;"));

            milist.Add(new Meansinfo(22, "预约管理", "", 1, -1, 2, "&#xe60d;"));
            milist.Add(new Meansinfo(23, "等待审核", "/Reservation/StudentResAwait", 2, 22, 2, "&#xe616;"));
            milist.Add(new Meansinfo(24, "审核通过", "/Reservation/StudentResPassed", 2, 22, 2, "&#xe616;"));
            milist.Add(new Meansinfo(25, "审核历史", "/Reservation/StudentResHistory", 2, 22, 2, "&#xe616;"));

            milist.Add(new Meansinfo(26, "我的桌面", "/Reservation/ShowLayout", 0, -1, 2, "&#xe616;"));




            //学生页面
            milist.Add(new Meansinfo(27, "个人管理", "", 1, -1, 3, "&#xe613;"));
            milist.Add(new Meansinfo(28, "个人信息", "/Userinfo/Details", 2, 27, 3, "&#xe616;"));

            milist.Add(new Meansinfo(38, "机房信息", "", 1, -1, 3, "&#xe613;"));
            milist.Add(new Meansinfo(39, "机房新闻", "/Newsinfo/NewsShow", 2, 38, 3, "&#xe616;"));

            milist.Add(new Meansinfo(29, "电脑预约", "", 1, -1, 3, "&#xe613;"));
            milist.Add(new Meansinfo(30, "自主预约", "/StudentComputerinfo/Index", 2, 29, 3, "&#xe616;"));

            milist.Add(new Meansinfo(33, "预约管理", "", 1, -1, 3, "&#xe60d;"));
            milist.Add(new Meansinfo(34, "等待审核", "/Reservation/StudentResAwait", 2, 33, 3, "&#xe616;"));
            milist.Add(new Meansinfo(35, "审核通过", "/Reservation/StudentResPassed", 2, 33, 3, "&#xe616;"));
            milist.Add(new Meansinfo(36, "审核历史", "/Reservation/StudentResHistory", 2, 33, 3, "&#xe616;"));

            milist.Add(new Meansinfo(37, "我的桌面", "/Reservation/ShowLayout", 0, -1, 3, "&#xe616;"));






            string userloginid = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo loginui = CacheHelper.GetCache<Userinfo>(userloginid);

            int logintype = 0;
            if (loginui != null)
            {
                logintype = Convert.ToInt32(loginui.Type);
            }

            return View(Tuple.Create(milist.Where(u => u.MType == logintype),loginui));
        }

    }
}
