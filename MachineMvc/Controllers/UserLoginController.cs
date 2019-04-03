using Bll;
using Common.Cache;
using Common.Md5;
using Common.VCode;
using IBll;
using MachineMvc.Models;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineMvc.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class UserLoginController : Controller
    {
        //
        // GET: /UserLogin/
        public IUserinfoService userinfoService=new UserinfoService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowVCode()
        {
            VCodeHelper validateCode = new VCodeHelper();
            string strCode = validateCode.RandomCode(4);

            //把验证码放到session中
            Session["VCode"] = strCode;

            byte[] imgBytes = validateCode.CreatVCodeImgs(strCode);

            return File(imgBytes, "image/jpeg");
        }

        public ActionResult ProcessLogin()
        {
            //1 处理验证码

            string strCode = Request["vcode"];
            //拿到session中的验证码做对比
            string sessionCode = Session["VCode"] as string;

            //取过验证码，马上置空
            Session["VCode"] = null;

            //在测试阶段可以注释这段，就不需要填写验证码了
            if (string.IsNullOrEmpty(sessionCode) || strCode != sessionCode)
            {
                // "alert('ok！');", "text/javascript"
                return Json("验证码错误！");
            }


            //2 处理用户名和密码
            string login = Request["login"];
            string pwd = Request["pwd"].GetMd5();

            var userinfo = userinfoService.GetEntities(u => u.Login == login && u.Pwd == pwd && u.Delflag == (int)DelflagEnum.Normal).FirstOrDefault();

            if (userinfo == null)
            {
                return Json("用户名或密码错误!");
            }


            //登录成功，把用户信息放到Cache里,记录登陆时间

            userinfo.Logindate = DateTime.Now;
            userinfoService.Update(userinfo);
            //1,立即分配一个标志，guid，作为key存入mm，同时把这个key存入客户端cookie
            string userLoginId = Guid.NewGuid().ToString();

            //把用户数据写入缓存
            CacheHelper.AddCache(userLoginId, userinfo, DateTime.Now.AddMinutes(20));

            //往客户端写入cookie
            Response.Cookies["userLoginId"].Value = userLoginId;


            //正确跳转到首页
            //return RedirectToAction("Test","Index");
            
            return Json("ok");
            
        }


        [LoginCheckFilterAttribute(IsCheck = true)]
        public ActionResult ActionUrl()
        {
            //上面先校验登陆信息再跳转
            return RedirectToAction( "Index","Index");
        }

        public ActionResult ExitLogin()
        {
            string userloginid = Request.Cookies["userLoginId"].Value.ToString();
            //登陆信息置空
            if (!string.IsNullOrWhiteSpace(userloginid))
            {
                CacheHelper.RemCache(userloginid);
            }
            
            return RedirectToAction("Index");
        } 

    }
}
