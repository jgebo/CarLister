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
    public class CarsModelController : Controller
    {
        private HCL2Entities db = new HCL2Entities();

        // GET: Cars
        public ActionResult HCLList(int? id, string filter, int? page)
        //public ActionResult HCLList()
        {
            {
               
                var listMake = new List<HCLViewModel>();
                var listYear = new List<HCLViewModel>();
                var listModel = new List<HCLViewModel>();
                var shortList = new List<HCLViewModel>();
                var savedMake = "";
                var savedYear = "0";
                //var cars = db.HCL2.Include(c => c.make).Include(c => c.model_name).Include(c => c.model_trim).Include(c => c.model_year).Include(c => c.picurl).OrderBy(m => m.make);  //.Where(a => a.make == acura );
                var cars = db.HCL2.OrderBy(m => m.make);  //.Where(a => a.make == acura );
                
                foreach (var item in cars)
                {
                    shortList.Add(new HCLViewModel { id = item.id, make = item.make.ToUpper(), model = item.model_name + item.model_trim, year = item.model_year, picurl = item.picurl });
 
                    listModel.Add(new HCLViewModel { id = item.id, model = item.model_name + item.model_trim });

                    if(savedMake.Trim().ToUpper() != item.make.Trim().ToUpper())
                    {
                        savedMake = item.make.Trim().ToUpper();
                        listMake.Add(new HCLViewModel { id = item.id, make = item.make.ToUpper() });
                    }

                }

                savedYear = "0";
                var ycars = db.HCL2.OrderBy(y => y.model_year);  
                foreach (var yitem in ycars)
                {
                    if (savedYear != yitem.model_year.Trim())
                    {
                        if (yitem.model_year.Substring(0, 1).Trim() == "1" || yitem.model_year.Substring(0, 1).Trim() == "2")
                        {
                            savedYear = yitem.model_year.Trim();
                            listYear.Add(new HCLViewModel { id = yitem.id, year = yitem.model_year }); // removed non date rows, still has duplicates 
                        }
                    }
                }

                ViewBag.MakeId= new SelectList(listMake.OrderBy(m => m.make), "Id", "Make");
                ViewBag.ModelId = new SelectList(listModel.OrderBy(m => m.model), "Id", "Model");
                ViewBag.YearId = new SelectList(listYear.OrderBy(m => m.year), "Id", "Year");
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(shortList.ToPagedList(pageNumber, pageSize));
                //return View();
            }
        }

        //// GET: CarsModel/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Car car = db.Cars.Find(id);
        //    if (car == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(car);
        //}

        //// GET: CarsModel/Create
        //public ActionResult Create()
        //{
        //    ViewBag.MakeId = new SelectList(db.Makes.OrderBy(m => m.Name), "Id", "Name");
        //    ViewBag.ModelId = new SelectList(db.Models.OrderBy(m => m.Name), "Id", "Name");
        //    ViewBag.YearId = new SelectList(db.Years.OrderByDescending(y => y.Year1), "Id", "Year1");
        //    return View();
        //}

        //// POST: CarsModel/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Car car, HttpPostedFileBase fileUpload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var fileName = Path.GetFileName(fileUpload.FileName);
        //        car.PicPath = Path.Combine(Server.MapPath("~/Attachments/"), fileName);
        //        fileUpload.SaveAs(car.PicPath);
        //        car.PicUrl = "~/Attachments/" + fileName;
                
        //        db.Cars.Add(car);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MakeId = new SelectList(db.Makes, "Id", "Name", car.Model.MakeId);
        //    ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
        //    ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
        //    return View(car);
        //}

        //// GET: CarsModel/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Car car = db.Cars.Find(id);
        //    if (car == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
        //    ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
        //    return View(car);
        //}

        //// POST: CarsModel/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ModelId,Price,YearId,PicUrl,PicPath")] Car car)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(car).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ModelId = new SelectList(db.Models, "Id", "Name", car.ModelId);
        //    ViewBag.YearId = new SelectList(db.Years, "Id", "Id", car.YearId);
        //    return View(car);
        //}

        //// GET: CarsModel/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Car car = db.Cars.Find(id);
        //    if (car == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(car);
        //}

        //// POST: CarsModel/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Car car = db.Cars.Find(id);
        //    db.Cars.Remove(car);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult GetCarModels(string carMakeId)
        {
            var carList = db.HCL2
                            .Where(m => m.make == carMakeId)
                            .OrderBy(m => m.model_name)
                            .ToDictionary(key => key.id.ToString(), value => value.model_name);
            return Json(carList);
        }

        //public JsonResult GetCarModels(int carMakeId)
        //{
        //    var carList = db.HCL2
        //                    .Where(m => m.make == carMakeId)
        //                    .OrderBy(m => m.Name)
        //                    .ToDictionary(key => key.Id.ToString(), value => value.Name);
        //    return Json( carList );
        //}
             
    }
}
