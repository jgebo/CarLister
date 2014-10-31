using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarLister.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Car Lister application.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Car Lister.";

            return View();
        }
    }
}