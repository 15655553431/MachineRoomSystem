﻿using Common.Cache;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineMvc.Models
{
    public class LoginCheckFilterAttribute : ActionFilterAttribute
    {
        //使用拦截器校验用户是否登录

        public bool IsCheck { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (IsCheck)
            {
                //校验用户是否已经登录
                //使用memcached+cookie代替session

                //如果没有记录的id，直接跳转到登陆界面
                if (filterContext.HttpContext.Request.Cookies["userLoginId"] == null)
                {        
                    filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                    return;
                }

                //从缓存里面拿到userLoginId信息
                string userLoginId = filterContext.HttpContext.Request.Cookies["userLoginId"].Value.ToString();

                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

                if (userInfo == null)
                {
                    //这里还有一种情况也需要返回登陆界面，就是用户没有操作权限，当前请求越权了
                    //后面有空需要研究一下，如何判断用户请求的页面越权
                    //用户长时间不操作，超时了
                    filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                    return;
                }
                else
                {
                    //滑动窗口机制（就是延长缓存时间,重新延长20分钟）
                    CacheHelper.SetCache(userLoginId, userInfo, DateTime.Now.AddMinutes(20));
                }

            }

        }
    }
}