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
    public class ConnectionBuildingsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: ConnectionBuildings
        public ActionResult Index()
        {
            var connectionBuildingss = db.ConnectionBuildingss.Include(c => c.Buildings).Include(c => c.Trip);
            return View(connectionBuildingss.ToList());
        }

        // GET: ConnectionBuildings/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var connectionBuildings = db.ConnectionBuildingss
                .Include(cb => cb.Buildings)
                .Include(cb => cb.Trip)
                .FirstOrDefault(cb => cb.Id == id);

            if (connectionBuildings == null)
            {
                return HttpNotFound();
            }

            return View(connectionBuildings);
        }


        // GET: ConnectionBuildings/Create
        public ActionResult Create()
        {
            ViewBag.BuildingsId = new SelectList(db.Buildingss, "Id", "Type");
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id");
            return View();
        }

        // POST: ConnectionBuildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TripId,BuildingsId")] ConnectionBuildings connectionBuildings)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionBuildingss.Add(connectionBuildings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingsId = new SelectList(db.Buildingss, "Id", "Type", connectionBuildings.BuildingsId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionBuildings.TripId);
            return View(connectionBuildings);
        }

        // GET: ConnectionBuildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionBuildings connectionBuildings = db.ConnectionBuildingss.Find(id);
            if (connectionBuildings == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingsId = new SelectList(db.Buildingss, "Id", "Type", connectionBuildings.BuildingsId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionBuildings.TripId);
            return View(connectionBuildings);
        }

        // POST: ConnectionBuildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TripId,BuildingsId")] ConnectionBuildings connectionBuildings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionBuildings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingsId = new SelectList(db.Buildingss, "Id", "Type", connectionBuildings.BuildingsId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionBuildings.TripId);
            return View(connectionBuildings);
        }

        // GET: ConnectionBuildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionBuildings = db.ConnectionBuildingss
             .Include(cb => cb.Buildings)
             .Include(cb => cb.Trip)
             .FirstOrDefault(cb => cb.Id == id);

            if (connectionBuildings == null)
            {
                return HttpNotFound();
            }

            return View(connectionBuildings);
        }

        // POST: ConnectionBuildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionBuildings connectionBuildings = db.ConnectionBuildingss.Find(id);
            db.ConnectionBuildingss.Remove(connectionBuildings);
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
