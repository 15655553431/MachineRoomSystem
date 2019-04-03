using Bll;
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
    public class ComputerinfoController : Controller
    {
        //
        // GET: /Computerinfo/
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

            Expression<Func<Computerinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Normal && u.Number != null && u.Model != null && u.IP != null && u.Machineroom.Number != null && u.Model.Contains(ci.Model) && u.IP.Contains(ci.IP) && u.Number.Contains(ci.Number) && u.Machineroom.Number.Contains(roomnumber));

            int count = 0;
            var computerlist = computerinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(computerlist, pi));
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
                if (computerinfoService.RCExist(ci.Row, ci.Col, ci.MachineroomID))
                {
                    ci.Delflag = (int)DelflagEnum.Normal;
                    ci.Status = (int)StatusEnum.Normal;
                    computerinfoService.Add(ci);
                }
                return Json("ok");
            }
            return View();
        }


        public ActionResult Details(int id = 0)
        {
            Computerinfo computerinfo = computerinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.ID == id)).First();
            if (computerinfo == null)
            {
                return HttpNotFound();
            }
            return View(computerinfo);
        }


        public ActionResult StudentDetails(int id = 0)
        {
            Computerinfo computerinfo = computerinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.ID == id)).First();
            if (computerinfo == null)
            {
                return HttpNotFound();
            }
            return View(computerinfo);
        }


        public ActionResult Edit(string id = null)
        {
            int idint = Convert.ToInt32(id);
            Computerinfo computerinfo = computerinfoService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Normal && u.ID == idint).First() as Computerinfo;
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
                Computerinfo computerinfo = computerinfoService.GetEntities(u => u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Normal && u.ID == ci.ID).First() as Computerinfo;
                if (ci.Row != computerinfo.Row || ci.Col != computerinfo.Col)
                {
                    if (!computerinfoService.RCExist(ci.Row, ci.Col, ci.MachineroomID))
                    {
                        return Json("err");
                    }
                }
                ci.Delflag = (int)DelflagEnum.Normal;
                computerinfoService.Update(ci);
                return Json("ok");
            }
            return View(ci);
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            int idint = Convert.ToInt32(id);

            Computerinfo computerinfo = computerinfoService.GetEntities(u => u.ID == idint && u.Delflag == (int)DelflagEnum.Normal).First() as Computerinfo;
            if (computerinfo != null)
            {
                computerinfoService.Delete(computerinfo);
                return Json("ok");
            }
            else
            {
                return Json("err");
            }
        }


        /// <summary>
        /// 根据机房ID。获得当前机房的布局信息,根据排数，进行分页展示
        /// 不存在或没有记录的电脑，显示为：此位空置
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowLayout(PagingInfo pi, string MachineroomID = null)
        {
            int midint = Convert.ToInt32(MachineroomID);
            pi.PageIndex = pi.PageIndex == 0 ? 1 : pi.PageIndex;
            pi.PageSize = pi.PageSize == 0 ? 3 : pi.PageSize;
            List<Computerinfo> computerlist = new List<Computerinfo>();
            computerlist = computerinfoService.GetEntities(u => (u.Delflag == (int)DelflagEnum.Normal && u.MachineroomID == midint)).ToList();
            if (computerlist.Count() > 0)
            {
                Machineroom mroom = computerlist.First().Machineroom;
                pi.TotalItems = mroom.Ccol;

                List<Computerinfo> showcomputerlist = new List<Computerinfo>();

                int starti = (pi.PageIndex - 1) * pi.PageSize + 1;
                int endi = starti + pi.PageSize - 1;

                if (starti > mroom.Ccol)
                {
                    starti = 1;
                    endi = 3;
                }
                endi = endi > mroom.Ccol ? mroom.Ccol : endi;

                for (int i = starti; i <= endi; i++)
                {
                    for (int j = 1; j <= mroom.Crow; j++)
                    {
                        var temp = computerlist.Where(u => u.Row == i && u.Col == j);
                        if (temp.Count() > 0)
                        {
                            showcomputerlist.Add(temp.First());
                        }
                        else
                        {
                            Computerinfo citemp = new Computerinfo();
                            citemp.ID = 0;
                            citemp.Number = "此位空置";
                            citemp.Row = i;
                            citemp.Col = j;
                            showcomputerlist.Add(citemp);
                        }
                    }
                }

                return Json(Tuple.Create(mroom, pi, showcomputerlist));
            }
            else
            {
                return Json("null");
            }
        }


        /// <summary>
        /// 根据id显示电脑信息,电脑预约界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StudentRes(int id = 0)
        {
            Computerinfo computerinfo = computerinfoService.GetEntities(u => u.ID == id && u.Delflag == (int)DelflagEnum.Normal).First() as Computerinfo;
            return View(computerinfo);
        }



    }
}
