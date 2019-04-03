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
    public class StudentComputerinfoController : Controller
    {
        //
        // GET: /StudentComputerinfo/
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

            Expression<Func<Computerinfo, bool>> wherelambda = u => (u.Delflag == (int)DelflagEnum.Normal && u.Status == (int)StatusEnum.Normal && u.Number != null && u.Model != null && u.Machineroom.Number != null && u.Model.Contains(ci.Model) && u.Number.Contains(ci.Number) && u.Machineroom.Number.Contains(roomnumber));

            int count = 0;
            var computerlist = computerinfoService.GetPageEntities<int>(pi.PageSize, pi.PageIndex, out count, wherelambda, u => u.ID, true).ToList();
            pi.TotalItems = count;

            return Json(Tuple.Create(computerlist, pi));
        }

    }
}
