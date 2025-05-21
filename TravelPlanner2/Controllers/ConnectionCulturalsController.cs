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
    public class ConnectionCulturalsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: ConnectionCulturals
        public ActionResult Index()
        {
            var connectionCulturals = db.ConnectionCulturals.Include(c => c.Cultural).Include(c => c.Trip);
            return View(connectionCulturals.ToList());
        }

        // GET: ConnectionCulturals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionCultural = db.ConnectionCulturals
                .Include(cc => cc.Cultural)
                .Include(cc => cc.Trip)
                .FirstOrDefault(cc => cc.Id == id);
            if (connectionCultural == null)
            {
                return HttpNotFound();
            }
            return View(connectionCultural);
        }

        // GET: ConnectionCulturals/Create
        public ActionResult Create()
        {
            ViewBag.CulturalId = new SelectList(db.Culturals, "Id", "Type");
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id");
            return View();
        }

        // POST: ConnectionCulturals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TripId,CulturalId")] ConnectionCultural connectionCultural)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionCulturals.Add(connectionCultural);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CulturalId = new SelectList(db.Culturals, "Id", "Type", connectionCultural.CulturalId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCultural.TripId);
            return View(connectionCultural);
        }

        // GET: ConnectionCulturals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionCultural connectionCultural = db.ConnectionCulturals.Find(id);
            if (connectionCultural == null)
            {
                return HttpNotFound();
            }
            ViewBag.CulturalId = new SelectList(db.Culturals, "Id", "Type", connectionCultural.CulturalId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCultural.TripId);
            return View(connectionCultural);
        }

        // POST: ConnectionCulturals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TripId,CulturalId")] ConnectionCultural connectionCultural)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionCultural).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CulturalId = new SelectList(db.Culturals, "Id", "Type", connectionCultural.CulturalId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCultural.TripId);
            return View(connectionCultural);
        }

        // GET: ConnectionCulturals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionCultural = db.ConnectionCulturals
                .Include(cc => cc.Cultural)
                .Include(cc => cc.Trip)
                .FirstOrDefault(cc => cc.Id == id);
            if (connectionCultural == null)
            {
                return HttpNotFound();
            }
            return View(connectionCultural);
        }

        // POST: ConnectionCulturals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionCultural connectionCultural = db.ConnectionCulturals.Find(id);
            db.ConnectionCulturals.Remove(connectionCultural);
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
