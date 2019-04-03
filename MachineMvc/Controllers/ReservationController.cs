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
    public class ReservationController : Controller
    {

        private IReservationService reservationService = new ReservationService();

        private IComputerinfoService computerinfoService = new ComputerinfoService();

        private IMachineroomService machineroomService = new MachineroomService();
        //
        // GET: /Reservation/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Details(int id)
        {
            return View(reservationService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.ID == id).First());
        }

        /// <summary>
        /// 显示机器布局界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowLayout()
        {
            return View();
        }


        /// <summary>
        /// 管理员查询所有等待审核的预约信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminResAwait()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminResAwait(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Normal && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }

        /// <summary>
        /// 管理员查看审核通过的
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminResPassed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminResPassed(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Leave && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }


        /// <summary>
        /// 管理员查看历史审核的
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminResHistory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminResHistory(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && (u.Sdate <= dts || u.Status == (int)StatusEnum.Block || u.Status == (int)StatusEnum.Over) && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }


        /// <summary>
        /// 根据预约信息，增加预约记录。预约成功返回ok，错误返回err
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StudentReservation(Reservation res)
        {
            string resultstr = "err";
            //先查询是否已经有预约信息，如果没有被预约，再插入
            List<Reservation> temp = reservationService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.ComputerinfoID == res.ComputerinfoID && u.Sdate == res.Sdate && u.DateFlag == res.DateFlag).ToList();
            if (temp.Count() == 0)
            {
                res.Rdate = DateTime.Now;
                res.Edate = res.Sdate;
                res.Status = (int)StatusEnum.Normal;
                res.Delflag = (int)DelflagEnum.Normal;
                //从缓存里面拿到userLoginId信息
                string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
                res.UserinfoID = userInfo.ID;
                reservationService.Add(res);
                resultstr = "ok";
            }

            return Json(resultstr);
        }

        /// <summary>
        /// 根据电脑id和日期，返回可以预约的时段
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDateFlag(int id, DateTime date)
        {
            //查询该机房是否有预约信息
            Computerinfo ci = computerinfoService.GetEntities(u => u.ID == id).FirstOrDefault();
            Computerinfo ciall = computerinfoService.GetEntities(u => u.MachineroomID == ci.MachineroomID && u.Col == 0 && u.Row == 0 && u.Status == (int)StatusEnum.Over).FirstOrDefault();
            List<Reservation> reslist = reservationService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && (u.ComputerinfoID == id || u.ComputerinfoID == ciall.ID) && u.Sdate == date && u.Status != (int)StatusEnum.Block)).ToList();
            List<string> reslut = new List<string>();
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Morning).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Morning).ToString() + ",上午第1、2节课");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Forenoon).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Forenoon).ToString() + ",上午第3、4节课");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Afternoon).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Afternoon).ToString() + ",下午课程时段");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Night).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Night).ToString() + ",晚自习时段");
            }
            if (reslut.Count() == 0)
            {
                return Json("null");
            }
            return Json(reslut);
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="idstr"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetStatus(int status, string checkid = "")
        {
            string[] idnum = checkid.Split(',');
            List<int> idlist = new List<int>();
            foreach (string idstr in idnum)
            {
                idlist.Add(Convert.ToInt32(idstr));
            }
            if (idlist.Count() > 0)
            {
                if (reservationService.SetStatus(status, idlist))
                {
                    return Json("ok");
                }
                else
                {
                    return Json("err");
                }
            }
            return Json("ok");
        }


        /// <summary>
        /// 批量取消预约
        /// </summary>
        /// <param name="checkid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRes(string checkid = "")
        {
            string[] idnum = checkid.Split(',');
            List<int> idlist = new List<int>();
            foreach (string idstr in idnum)
            {
                idlist.Add(Convert.ToInt32(idstr));
            }

            if (idlist.Count() > 0)
            {
                if (reservationService.DeleteRes(idlist))
                {
                    return Json("ok");
                }
                else
                {
                    return Json("err");
                }
            }

            return Json("ok");

        }

        /// <summary>
        /// 批量确认，预约信息中预约人已经到了。先验证预约日期是否是今天，然后才能改变状态。
        /// </summary>
        /// <param name="checkid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetStatusOver(string checkid = "")
        {
            string[] idnum = checkid.Split(',');
            List<int> idlist = new List<int>();
            foreach (string idstr in idnum)
            {
                idlist.Add(Convert.ToInt32(idstr));
            }
            if (idlist.Count() > 0)
            {
                if (reservationService.SetStatusOver(idlist))
                {
                    return Json("ok");
                }
                else
                {
                    return Json("err");
                }
            }
            return Json("ok");
        }




        /// <summary>
        /// 教职工查询所有等待审核的预约信息
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherResAwait()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherResAwait(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Computerinfo.Machineroom.UserinfoID == userInfo.ID && u.Status == (int)StatusEnum.Normal && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }

        /// <summary>
        /// 教职工查看审核通过的
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherResPassed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherResPassed(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Computerinfo.Machineroom.UserinfoID == userInfo.ID && u.Status == (int)StatusEnum.Leave && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }


        /// <summary>
        /// 教职工查看历史审核的
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherResHistory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherResHistory(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Computerinfo.Machineroom.UserinfoID == userInfo.ID && (u.Sdate <= dts || u.Status == (int)StatusEnum.Block || u.Status == (int)StatusEnum.Over) && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }




        /// <summary>
        /// 学生查询所有等待审核的预约信息
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentResAwait()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentResAwait(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.UserinfoID == userInfo.ID && u.Status == (int)StatusEnum.Normal && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }

        /// <summary>
        /// 学生查看审核通过的
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentResPassed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentResPassed(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.UserinfoID == userInfo.ID && u.Status == (int)StatusEnum.Leave && u.Sdate > dts && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }


        /// <summary>
        ///学生查看历史审核的
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentResHistory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentResHistory(PagingInfo pi)
        {
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 5 : pi.PageSize;
            string snumber = Request["SNumber"];
            string cnumber = Request["CNumber"];
            string uname = Request["UName"];
            //从缓存里面拿到userLoginId信息
            string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
            Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;

            DateTime dts = DateTime.Now.AddDays(-1);
            Expression<Func<Reservation, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.UserinfoID == userInfo.ID && (u.Sdate <= dts || u.Status == (int)StatusEnum.Block || u.Status == (int)StatusEnum.Over) && u.Userinfo.Number.Contains(snumber) && u.Computerinfo.Machineroom.Userinfo.Uname.Contains(uname) && u.Computerinfo.Number.Contains(cnumber));

            int count = 0;
            var reservationlist = reservationService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(reservationlist, pi));
        }


        /// <summary>
        /// 预约机房专用的方法
        /// </summary>
        /// <param name="mr"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MachineroomRes(Reservation res)
        {
            //这里传入的预约信息中的ComputerinfoID，其实是机房的id，并不是电脑id
            string resultstr = "err";
            Machineroom mr = machineroomService.GetEntities(u => u.ID == res.ComputerinfoID).FirstOrDefault() as Machineroom;

            //先查询该机房是否有隐藏的电脑信息
            List<Computerinfo> cilist = computerinfoService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.MachineroomID == mr.ID && u.Col == 0 && u.Row == 0).ToList();
            if (cilist.Count == 0)
            {
                //一台隐藏的没有，那就添加一台
                Computerinfo ciadd = new Computerinfo();
                ciadd.MachineroomID = mr.ID;
                ciadd.Number = mr.Number + "-ALL";
                ciadd.Money = 0;
                ciadd.Memory = mr.Name + "(所有电脑)";
                ciadd.IP = "机房网关：" + mr.IPinfo;
                ciadd.MAC = mr.Address;
                ciadd.Model = mr.Name + "(所有电脑)";
                ciadd.System = mr.Name + "(所有电脑)";
                ciadd.RN = mr.Name + "(所有电脑)";
                ciadd.Row = 0;
                ciadd.Col = 0;
                ciadd.Brand = mr.Name + "(所有电脑)";
                ciadd.CPU = mr.Name + "(所有电脑)";
                ciadd.Delflag = (int)DelflagEnum.Normal;
                ciadd.Status = (int)StatusEnum.Over;
                ciadd.Hard = mr.Name + "(所有电脑)";
                ciadd.Facturedate = DateTime.Now;
                computerinfoService.Add(ciadd);
            }
            else if (cilist.Count > 1)
            {
                //隐藏电脑只能有一台，如果有多台，全部删除，返回err
                return Json(resultstr);
            }

            //先查询是否已经有预约信息(查询整个机房，只要有一台被预约了，就不行)，如果没有被预约，再插入
            List<Reservation> temp = reservationService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.Computerinfo.MachineroomID == mr.ID && u.Sdate == res.Sdate && u.DateFlag == res.DateFlag).ToList();
            if (temp.Count() == 0)
            {
                Computerinfo ci = computerinfoService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.MachineroomID == mr.ID && u.Col == 0 && u.Row == 0).FirstOrDefault();
                res.ComputerinfoID = ci.ID;
                res.Rdate = DateTime.Now;
                res.Edate = res.Sdate;
                res.Status = (int)StatusEnum.Normal;
                res.Delflag = (int)DelflagEnum.Normal;
                //从缓存里面拿到userLoginId信息
                string userLoginId = Request.Cookies["userLoginId"].Value.ToString();
                Userinfo userInfo = CacheHelper.GetCache(userLoginId) as Userinfo;
                res.UserinfoID = userInfo.ID;
                reservationService.Add(res);
                resultstr = "ok";
            }

            return Json(resultstr);

        }


        /// <summary>
        /// 根据机房id和日期，返回可以预约的时段
        /// 用于预约机房使用
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMachineFlag(int id, DateTime date)
        {
            List<Reservation> reslist = reservationService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.Computerinfo.MachineroomID == id && u.Sdate == date && u.Status != (int)StatusEnum.Block)).ToList();
            List<string> reslut = new List<string>();
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Morning).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Morning).ToString() + ",上午第1、2节课");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Forenoon).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Forenoon).ToString() + ",上午第3、4节课");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Afternoon).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Afternoon).ToString() + ",下午课程时段");
            }
            if (reslist.Where(u => u.DateFlag == (int)DateFlagEnum.Night).Count() == 0)
            {
                reslut.Add(((int)DateFlagEnum.Night).ToString() + ",晚自习时段");
            }
            if (reslut.Count() == 0)
            {
                return Json("null");
            }
            return Json(reslut);
        }

    }
}
