using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class BuildingsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        public ActionResult Index()
        {
            return View(db.Buildingss.ToList());
        }

        // Buildings details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buildings buildings = db.Buildingss.Find(id);
            if (buildings == null)
            {
                return HttpNotFound();
            }
            return View(buildings);
        }

        // Buildings Create
        public ActionResult Create()
        {
            return View(new Buildings());
        }

        // POST: Buildings Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Buildings buildings)
        {
            if (ModelState.IsValid)
            {
                db.Buildingss.Add(buildings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(buildings);
        }

        // Buildings edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buildings buildings = db.Buildingss.Find(id);
            if (buildings == null)
            {
                return HttpNotFound();
            }
            return View(buildings);
        }

        // POST Buildings edit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Buildings buildings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buildings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buildings);
        }

        // GET Buildings delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buildings buildings = db.Buildingss.Find(id);
            if (buildings == null)
            {
                return HttpNotFound();
            }
            return View(buildings);
        }

        // POST Buildings delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Buildings buildings = db.Buildingss.Find(id);
            db.Buildingss.Remove(buildings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
