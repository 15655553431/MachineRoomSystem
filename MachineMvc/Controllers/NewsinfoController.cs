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
    public class NewsinfoController : Controller
    {
        private INewsinfoService newsinfoService = new NewsinfoService();
        //
        // GET: /Newsinfo/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NewsShow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PagingInfo pi, Newsinfo ni)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            //string uname = Request["uname"];
            //mr.Userinfo.Uname = String.IsNullOrWhiteSpace(mr.Userinfo.Uname) ? "" : mr.Userinfo.Uname;
            //ni.Number = String.IsNullOrWhiteSpace(mr.Number) ? "" : mr.Number;
            //ni.Name = String.IsNullOrWhiteSpace(mr.Name) ? "" : mr.Name;

            Expression<Func<Newsinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal);

            int count = 0;
            var newsinfolist = newsinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, false).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(newsinfolist, pi));
        }

        [HttpPost]
        public ActionResult ShowNewsList()
        {
            DateTime nowdate = DateTime.Now;
            Expression<Func<Newsinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Showtime <= nowdate && u.Endtime >= nowdate);
            var newsinfolist = newsinfoService.GetEntities(wherelambda).ToList();

            return Json(newsinfolist);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Newsinfo ni, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //这里注意必须使用 FormCollection form  获得富文本编辑器的内容
                //[ValidateInput(false)] 还需要加上这个，因为富文本编辑器传过来的是html代码。不这样会报错
                string editorValue = form["editorValue"];
                ni.Content = editorValue;
                ni.Delflag = (int)DelflagEnum.Normal;
                ni.Inputdate = DateTime.Now;
                //从缓存里面拿到userLoginId信息
                string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
                ni.UserinfoID = userInfo.ID;
                newsinfoService.Add(ni);
                return Json("ok");
            }
            return View();
        }

        public ActionResult Details(int id = 0)
        {
            Newsinfo newsinfo = newsinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.ID == id)).First();
            if (newsinfo == null)
            {
                return HttpNotFound();
            }
            return View(newsinfo);
        }


        public ActionResult Edit(int id = 0)
        {
            Newsinfo newsinfo = newsinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.ID == id)).First();
            if (newsinfo == null)
            {
                return HttpNotFound();
            }
            return View(newsinfo);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Newsinfo ni, FormCollection form)
        {

            if (ModelState.IsValid)
            {
                Newsinfo newsinfo = newsinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.ID == ni.ID)).First();
                //这里注意必须使用 FormCollection form  获得富文本编辑器的内容
                //[ValidateInput(false)] 还需要加上这个，因为富文本编辑器传过来的是html代码。不这样会报错
                string editorValue = form["editorValue"];
                newsinfo.Content = editorValue;
                newsinfo.Inputdate = DateTime.Now;
                newsinfo.Showtime = ni.Showtime;
                newsinfo.Endtime = ni.Endtime;
                newsinfo.Title = ni.Title;
                newsinfo.Other = ni.Other;
                //从缓存里面拿到userLoginId信息
                string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
                newsinfo.UserinfoID = userInfo.ID;
                newsinfoService.Update(newsinfo);
                return Json("ok");
            }
            return View();
        }





    }
}
