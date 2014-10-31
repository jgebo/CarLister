using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarLister.Models;
using System.IO;
using PagedList.Mvc;
using PagedList;

namespace CarLister.Controllers
{
    public class CarsController : Controller
    {
        private CarsEntities db = new CarsEntities();

        private static bool SortDirection;
        private static string SortProperty;
        private static IEnumerable<Car> cars; // stsatic model for compounding



        //// GET: Cars Cool List
        ///// <summary>
        ///// Generates view using mosiac template for list  - Cool View
        ///// </summary>
        ///// <returns>ActionResult</returns>
        //public ActionResult CoolList()
        //{
        //    var cars = db.Cars.Include(c => c.Model).Include(c => c.Year);
        //    return View(cars.ToList());
        //}

        // GET: Cars Uncool List
        /// <summary>
        /// Generates standard view of car list - Uncool View 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult UncoolList(int? id, string filter, int? page)
        {
            var cars = db.Cars.Include(c => c.Model.Make).Include(c => c.Model).Include(c => c.Year);
           
            switch(filter)
            {
                case "Year":
                    cars = cars.Where(c => c.Year.Id == id);
                    break;
                case "Make":
                    cars = cars.Where(c => c.Model.Make.Id == id);
                    break;
                case "Model":
                    cars = cars.Where(c => c.Model.Id == id);
                    break;
                default:
                    break;
            }

            switch(SortProperty)
            {
                case "Year":
                    if(!SortDirection)
                      {
                        cars = cars.OrderBy(c => c.Year.Year1);
                    }
                    else
                    {
                        cars = cars.OrderByDescending(c => c.Year.Year1);
                    }
                    break;
                case "Make":
                    if (!SortDirection)
                    {
                        cars = cars.OrderBy(c => c.Model.Make.Name);
                    }
                    else
                    {
                        cars = cars.OrderByDescending(c => c.Model.Make.Name);
                    }
                    break;
                case "Model":
                    if (!SortDirection)
                    {
                        cars = cars.OrderBy(c => c.Model.Name);
                    }
                    else
                    {
                        cars = cars.OrderByDescending(c => c.Model.Name);
                    }
                    break;
                case "Price":
                    if (!SortDirection)
                    {
                        cars = cars.OrderBy(c => c.Price);
                    }
                    else
                    {
                        cars = cars.OrderByDescending(c =>  c.Price);
                    }
                    break;

                default:
                    cars = cars.OrderBy(c => c.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(cars.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.MakeId = new SelectList(db.Makes.OrderBy(m => m.Name), "Id", "Name");
            ViewBag.ModelId = new SelectList(db.Models.OrderBy(m => m.Name), "Id", "Name");
            ViewBag.YearId = new SelectList(db.Years.OrderByDescending(y => y.Year1), "Id", "Year1");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                car.PicPath = Path.Combine(Server.MapPath("~/Attachments/"), fileName);
                fileUpload.SaveAs(car.PicPath);
                car.PicUrl = "~/Attachments/" + fileName;

                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("UncoolList");
            }

            ViewBag.MakeId = new SelectList(db.Makes, "Id", "Name", car.Model.MakeId);
            ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ModelId,Price,YearId,PicUrl,PicPath")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("UncoolList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public JsonResult GetCarModels(int carMakeId)
        {
            var carList = db.Models
                            .Where(m => m.MakeId == carMakeId)
                            .OrderBy(m => m.Name)
                            .ToDictionary(key => key.Id.ToString(), value => value.Name);
            return Json(carList);
        }
        public ActionResult Sort(string property, IEnumerable<Car> model)
        {
            cars = model;
            if (SortProperty == property)
            {
                // toggle direction
                SortDirection = !SortDirection;
            }
            else
            {
                // initial direction (ascending)
                SortDirection = false;
            }

            SortProperty = property;
            return RedirectToAction("UncoolList");

        }
    }
}
