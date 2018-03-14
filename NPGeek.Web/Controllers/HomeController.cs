using NPGeek.Web.Models.DALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPGeek.Web.Controllers
{
    public class HomeController : Controller
    {

        private IParkDAL dal;

        public HomeController(IParkDAL dal)
        {
            this.dal = dal;
        }

        // GET: Home
        public ActionResult Index()
        {
            var park = dal.GetAllParks();

            return View("Index", park);
        }
    }
}