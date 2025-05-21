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
    public class ConnectionNaturesController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: ConnectionNatures
        public ActionResult Index()
        {
            var connectionNatures = db.ConnectionNatures.Include(c => c.Nature).Include(c => c.Trip);
            return View(connectionNatures.ToList());
        }

        // GET: ConnectionNatures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionNature = db.ConnectionNatures
                .Include(cn => cn.Nature)
                .Include(cn => cn.Trip)
                .FirstOrDefault(cn => cn.Id == id);
            if (connectionNature == null)
            {
                return HttpNotFound();
            }
            return View(connectionNature);
        }

        // GET: ConnectionNatures/Create
        public ActionResult Create()
        {
            ViewBag.NatureId = new SelectList(db.Natures, "Id", "Type");
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id");
            return View();
        }

        // POST: ConnectionNatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TripId,NatureId")] ConnectionNature connectionNature)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionNatures.Add(connectionNature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NatureId = new SelectList(db.Natures, "Id", "Type", connectionNature.NatureId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionNature.TripId);
            return View(connectionNature);
        }

        // GET: ConnectionNatures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionNature connectionNature = db.ConnectionNatures.Find(id);
            if (connectionNature == null)
            {
                return HttpNotFound();
            }
            ViewBag.NatureId = new SelectList(db.Natures, "Id", "Type", connectionNature.NatureId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionNature.TripId);
            return View(connectionNature);
        }

        // POST: ConnectionNatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TripId,NatureId")] ConnectionNature connectionNature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionNature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NatureId = new SelectList(db.Natures, "Id", "Type", connectionNature.NatureId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionNature.TripId);
            return View(connectionNature);
        }

        // GET: ConnectionNatures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var connectionNature = db.ConnectionNatures
                .Include(cn => cn.Nature)
                .Include(cn => cn.Trip)
                .FirstOrDefault(cn => cn.Id == id);
            if (connectionNature == null)
            {
                return HttpNotFound();
            }
            return View(connectionNature);
        }

        // POST: ConnectionNatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionNature connectionNature = db.ConnectionNatures.Find(id);
            db.ConnectionNatures.Remove(connectionNature);
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
