using Bll;
using Common.Cache;
using IBll;
using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MachineMvc.Controllers
{
    public class TeacherMachineroomController : Controller
    {
        //
        // GET: /TeacherMachineroom/
        private IMachineroomService machineroomService = new MachineroomService();

        

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(PagingInfo pi, Machineroom mr)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string uname = Request["uname"];
            //mr.Userinfo.Uname = String.IsNullOrWhiteSpace(mr.Userinfo.Uname) ? "" : mr.Userinfo.Uname;
            mr.Number = String.IsNullOrWhiteSpace(mr.Number) ? "" : mr.Number;
            mr.Name = String.IsNullOrWhiteSpace(mr.Name) ? "" : mr.Name;

            Expression<Func<Machineroom, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Name != null && u.Number != null && u.Userinfo.Uname != null && u.Number.Contains(mr.Number) && u.Name.Contains(mr.Name) && u.Userinfo.Uname.Contains(uname));

            int count = 0;
            var Machinelist = machineroomService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.Count, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(Machinelist, pi));
        }


        public ActionResult Edit(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Machineroom machineroom = machineroomService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.ID == idint).First() as Machineroom;
            if (machineroom == null)
            {
                return HttpNotFound();
            }
            return View(machineroom);
        }

        [HttpPost]
        public ActionResult Edit(Machineroom mr)
        {
            if (ModelState.IsValid)
            {
                //从缓存里面拿到userLoginId信息
                string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
                mr.UserinfoID = userInfo.ID;
                mr.Delflag = (int)DelflagEnum.Normal;
                machineroomService.Update(mr);
                return Json("ok");
            }
            return View(mr);
        }


        /// <summary>
        /// 根据机房号查找机房，最多只许查10条
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Getmachineroom(string Search = "")
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            var machinelist = machineroomService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.UserinfoID == userInfo.ID && u.Name != null && u.Number != null && (u.Number.Contains(Search) || u.Name.Contains(Search)))).OrderBy(u => u.ID).Take(12);
            return Json(machinelist);
        }

    }
}
