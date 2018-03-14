using NPGeek.Web.Models.DALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPGeek.Web.Controllers
{
    public class FavoriteParksController : Controller
    {
        private ISurveyDAL dal;
        public FavoriteParksController(ISurveyDAL dal)
        {
            this.dal = dal;
        }

        // GET: FavoriteParks
        public ActionResult Survey()
        {
            return View();
        }

        public ActionResult FavoriteParks()
        {
            var parks = dal.GetFavoriteParks();

            return View("FavoriteParks", parks);
        }
    }
}