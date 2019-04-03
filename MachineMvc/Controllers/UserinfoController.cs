using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using MachineMvc.Models;
using IBll;
using Bll;
using Model.Enum;
using Common.Md5;
using System.Collections;
using System.Linq.Expressions;
using Common.Cache;

namespace MachineMvc.Controllers
{

    public class UserinfoController : Controller
    {
        private DataModelContainer db = new DataModelContainer();
        private IUserinfoService userinfoService = new UserinfoService();
        //
        // GET: /Userinfo/

        public ActionResult AdminIndex(PagingInfo pi,Userinfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            ui.Number = String.IsNullOrWhiteSpace(ui.Number) ? "" : ui.Number;
            ui.Uname = String.IsNullOrWhiteSpace(ui.Uname) ? "" : ui.Uname;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login)?"":ui.Login;

            Expression<Func<Userinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Type == (int)TypeEnum.Administrator && u.Number != null && u.Uname != null && u.Login != null && u.Number.Contains(ui.Number) && u.Uname.Contains(ui.Uname) && u.Login.Contains(ui.Login));

            int count = 0;
            var adminlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<Userinfo>;
            pi.TotalItems = count;

            return View(Tuple.Create(adminlist,pi,ui));
        }


        public ActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminCreate(Userinfo userinfo)
        {
            if (userinfoService.LoginExist(userinfo.Login))
            {
                return View(userinfo);
            }
            if (ModelState.IsValid)
            {
                userinfo.Delflag = (int)DelflagEnum.Normal;
                userinfo.Status = (int)StatusEnum.Normal;
                userinfo.Signdate = DateTime.Now;
                userinfo.Pwd = "123456".GetMd5();
                userinfo.Type = (int)TypeEnum.Administrator;
                userinfoService.Add(userinfo);
                return Json("ok");
            }

            return View(userinfo);
        }

        [HttpPost]
        public ActionResult AdminDelete(string id)
        {
            int idint = Convert.ToInt32(id);
            
            Userinfo userinfo = userinfoService.GetEntities(u => u.ID == idint).First() as Userinfo;
            if (userinfo != null)
            {
                userinfoService.Delete(userinfo);
                return Json("ok");
            }
            else
            {
                return Json("err");
            }
        }

        public ActionResult AdminEdit(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Userinfo userinfo = userinfoService.GetEntities(u => u.ID == idint && u.Delflag == (int)DelflagEnum.Normal).First() as Userinfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }


        [HttpPost]
        public ActionResult AdminEdit(Userinfo userinfo)
        {
            if (userinfoService.LoginExist(userinfo.ID, userinfo.Login))
            {
                return View(userinfo);
            }
            userinfo.Delflag = (int)DelflagEnum.Normal;
            if (ModelState.IsValid)
            {
                userinfoService.Update(userinfo);
                return Json("ok");
            }
            return View(userinfo);
        }

        public ActionResult UserinfoDetails(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Userinfo userinfo = userinfoService.GetEntities(u => u.ID == idint).First() as Userinfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        public ActionResult EditPwd(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Userinfo userinfo = userinfoService.GetEntities(u => u.ID == idint).First() as Userinfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        [HttpPost]
        public ActionResult EditPwd(Userinfo userinfo)
        {
            if (userinfoService.EditPwd(userinfo.ID, userinfo.Pwd))
            {
                return Json("ok");
            }
            return View(userinfo);
        }


        public ActionResult TeacherIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherIndex(PagingInfo pi, Userinfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            ui.Number = String.IsNullOrWhiteSpace(ui.Number) ? "" : ui.Number;
            ui.Uname = String.IsNullOrWhiteSpace(ui.Uname) ? "" : ui.Uname;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<Userinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Type == (int)TypeEnum.Teacher && u.Number != null && u.Uname != null && u.Login != null && u.Number.Contains(ui.Number) && u.Uname.Contains(ui.Uname) && u.Login.Contains(ui.Login));

            int count = 0;
            var adminlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<Userinfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(adminlist, pi));
        }

        public ActionResult TeacherCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherCreate(Userinfo userinfo)
        {
            if (userinfoService.LoginExist(userinfo.Login))
            {
                return View(userinfo);
            }
            if (ModelState.IsValid)
            {
                userinfo.Delflag = (int)DelflagEnum.Normal;
                userinfo.Status = (int)StatusEnum.Normal;
                userinfo.Signdate = DateTime.Now;
                userinfo.Pwd = "123456".GetMd5();
                userinfo.Type = (int)TypeEnum.Teacher;
                userinfoService.Add(userinfo);
                return Json("ok");
            }

            return View(userinfo);
        }


        public ActionResult StudentIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentIndex(PagingInfo pi, Userinfo ui)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            ui.Number = String.IsNullOrWhiteSpace(ui.Number) ? "" : ui.Number;
            ui.Uname = String.IsNullOrWhiteSpace(ui.Uname) ? "" : ui.Uname;
            ui.Login = String.IsNullOrWhiteSpace(ui.Login) ? "" : ui.Login;

            Expression<Func<Userinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Type == (int)TypeEnum.Student && u.Number != null && u.Uname != null && u.Login != null && u.Number.Contains(ui.Number) && u.Uname.Contains(ui.Uname) && u.Login.Contains(ui.Login));

            int count = 0;
            var adminlist = userinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true) as IEnumerable<Userinfo>;
            pi.TotalItems = count;

            return Json(Tuple.Create(adminlist, pi));
        }


        
        public ActionResult StudentCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentCreate(Userinfo userinfo)
        {
            if (userinfoService.LoginExist(userinfo.Login))
            {
                return View(userinfo);
            }
            if (ModelState.IsValid)
            {
                userinfo.Delflag = (int)DelflagEnum.Normal;
                userinfo.Status = (int)StatusEnum.Normal;
                userinfo.Signdate = DateTime.Now;
                userinfo.Pwd = "123456".GetMd5();
                userinfo.Type = (int)TypeEnum.Student;
                userinfoService.Add(userinfo);
                return Json("ok");
            }

            return View(userinfo);
        }



        /// <summary>
        /// 用来查找用户用的,排除了学生。只取前20条
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserinfo(Userinfo ui)
        {
            ui.Uname = String.IsNullOrWhiteSpace(ui.Uname) ? "" : ui.Uname;
            var userinfolist = userinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.Uname != null && u.Uname.Contains(ui.Uname) && u.Type != (int)TypeEnum.Student)).OrderBy(u => u.ID).Take(12);
            return Json(userinfolist);
        }


        public ActionResult Details()
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
            return View(userInfo);
        }
        public ActionResult UserinfoEditPwd(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Userinfo userinfo = userinfoService.GetEntities(u => u.ID == idint).First() as Userinfo;
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        [HttpPost]
        public ActionResult UserinfoEditPwd(Userinfo userinfo)
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
            if (userinfoService.EditPwd(userInfo.ID, userinfo.Pwd))
            {
                return Json("ok");
            }
            return View(userinfo);
        }

        public ActionResult Edit()
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
            return View(userInfo);
        }

        [HttpPost]
        public ActionResult Edit(Userinfo userinfo)
        {
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;


            if (userinfoService.LoginExist(userInfo.ID, userinfo.Login))
            {
                return View(userinfo);
            }

            userInfo.Login = userinfo.Login;
            userInfo.Phone = userinfo.Phone;
            userInfo.Address = userinfo.Address;
            userInfo.Other = userinfo.Other;
            
            if (ModelState.IsValid)
            {
                userinfoService.Update(userInfo);
                //更新登陆缓存信息
                CacheHelper.SetCache(userLoginId, userInfo, DateTime.Now.AddMinutes(20));
                return Json("ok");
            }


            return View(userInfo);
        }

    }
}