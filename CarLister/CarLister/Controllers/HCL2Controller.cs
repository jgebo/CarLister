using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarLister.Models;
using PagedList;
using System.Threading.Tasks;

namespace CarLister.Controllers
{
    public class HCL2Controller : Controller
    {
        private static HCL2Entities db = new HCL2Entities();
        private static string sYear;
        private static string sMake;
        private static string sModel;
        private static IEnumerable<HCL2> cars = db.HCL2                                               
                                                .OrderBy(c => c.model_year)
                                                .ThenBy(c => c.make)
                                                .ThenBy(c => c.model_name)
                                                .ThenBy(c => c.model_trim)
                                                .ToList();
                                               

        // GET: Cars
        [HttpGet]
        public ActionResult Index()
        {
            CarViewModel model = new CarViewModel();
            cars = db.HCL2.ToList();
            sYear = "--Select Year";
            sYear = "--Select Make";
            sYear = "--Select Model";

            model.yearList = new SelectList(db.HCL2
                        .GroupBy(m => m.model_year)
                        .Select(f => f.FirstOrDefault())
                        .OrderByDescending(m => m.model_year), "model_year", "model_year", sYear);
            model.makeList = new SelectList(db.HCL2
                        .GroupBy(m => m.make)
                        .Select(f => f.FirstOrDefault())
                        .OrderByDescending(m => m.make), "make", "make", sMake);
             model.modelList = new SelectList(db.HCL2
                        .GroupBy(m => m.model_name)
                        .Select(f => f.FirstOrDefault())
                        .OrderByDescending(m => m.model_name), "model_name", "model_name", sModel);

            model.carsList = db.HCL2.OrderByDescending(c => c.model_year)
                                    .ThenBy(c => c.make)
                                    .ThenBy(c => c.model_name)
                                    .ThenBy(c => c.model_trim)
                                    .ToPagedList(1, 10);
            return this.View(model);


        }
        public ActionResult CarsList(CarViewModel model, string param1, string param2, string param3, int? id, int? page)
       
        {
            if (param1 != null)
            {
                cars = db.HCL2.Where(c => c.model_year == param1);
                sYear = param1;
                if (param2 != null)
                {
                    cars = db.HCL2.Where(c => c.make == param2);
                    sMake = param2;
                    if (param3 != null)
                    {
                        cars = db.HCL2.Where(c => c.model_name == param3);
                        sModel = param3;
                    }
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            model.carsList = cars.OrderBy(m => m.make).ToPagedList(pageNumber, pageSize);
            return PartialView("CarsList", model);

        }

        // GET: HCL2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HCL2 hCL2 = db.HCL2.Find(id);
            if (hCL2 == null)
            {
                return HttpNotFound();
            }
            return View(hCL2);
        }


        public JsonResult Selector(string filterType, string filterValue)
        {
            switch (filterType)
            {
                case "Year":
                    sYear = filterValue;
                    var select = (db.HCL2
                       .Where(m => m.model_year == sYear)
                       .GroupBy(m => m.make)
                       .Select(f => f.FirstOrDefault())
                       .OrderBy(m => m.make)
                       .ToDictionary(key => key.make, value => value.make));
                    return Json(db.HCL2
                        .Where(m => m.model_year == sYear)
                        .GroupBy(m => m.make)
                        .Select(f => f.FirstOrDefault())
                        .OrderBy(m => m.make)
                        .ToDictionary(key => key.make, value => value.make));
                case "Make":
                    sMake = filterValue;
                    return Json(db.HCL2
                        .Where(m => m.model_year == sYear && m.make == sMake)
                        .OrderBy(m => m.model_name)
                        .ThenBy(m => m.model_trim)
                        .ToDictionary(key => key.model_name + " " + key.model_trim,
                                      value => value.model_name + " " + value.model_trim));
                case "Model":
                    sModel = filterValue;
                    return Json("");
                default:
                    return Json("Error");
            }
        }
    }
}
