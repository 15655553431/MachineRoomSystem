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
    public class TeacherComputerinfoController : Controller
    {
        //
        // GET: /TeacherComputerinfo/
        private IComputerinfoService computerinfoService = new ComputerinfoService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PagingInfo pi, Computerinfo ci)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string roomnumber = Request["MNumber"];
            ci.Number = String.IsNullOrWhiteSpace(ci.Number) ? "" : ci.Number;
            ci.Model = String.IsNullOrWhiteSpace(ci.Model) ? "" : ci.Model;
            ci.IP = String.IsNullOrWhiteSpace(ci.IP) ? "" : ci.IP;

            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            Expression<Func<Computerinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Machineroom.UserinfoID == userInfo.ID && u.Number != null && u.Model != null && u.IP != null && u.Machineroom.Number != null && u.Model.Contains(ci.Model) && u.IP.Contains(ci.IP) && u.Number.Contains(ci.Number) && u.Machineroom.Number.Contains(roomnumber));

            int count = 0;
            var computerlist = computerinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(computerlist, pi));
        }


        public ActionResult Edit(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Computerinfo computerinfo = computerinfoService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.ID == idint).First() as Computerinfo;
            if (computerinfo == null)
            {
                return HttpNotFound();
            }
            return View(computerinfo);
        }

        [HttpPost]
        public ActionResult Edit(Computerinfo ci)
        {
            if (ModelState.IsValid)
            {
                ci.Delflag = (int)DelflagEnum.Normal;
                computerinfoService.Update(ci);
                return Json("ok");
            }
            return View(ci);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Computerinfo ci)
        {
            if (ModelState.IsValid)
            {
                ci.Delflag = (int)DelflagEnum.Normal;
                ci.Status = (int)StatusEnum.Normal;
                computerinfoService.Add(ci);
                return Json("ok");
            }
            return View();
        }
    }
}
