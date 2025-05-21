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
    public class ConnectionCulinariesController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: ConnectionCulinaries
        public ActionResult Index()
        {
            var connectionCulinaries = db.ConnectionCulinaries.Include(c => c.Culinary).Include(c => c.Trip);
            return View(connectionCulinaries.ToList());
        }

        // GET: ConnectionCulinaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionCulinary = db.ConnectionCulinaries
                .Include(cc => cc.Culinary)
                .Include(cc => cc.Trip)
                .FirstOrDefault(cc => cc.Id == id);
            if (connectionCulinary == null)
            {
                return HttpNotFound();
            }
            return View(connectionCulinary);
        }

        // GET: ConnectionCulinaries/Create
        public ActionResult Create()
        {
            ViewBag.CulinaryId = new SelectList(db.Culinaries, "Id", "Type");
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id");
            return View();
        }

        // POST: ConnectionCulinaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TripId,CulinaryId")] ConnectionCulinary connectionCulinary)
        {
            if (ModelState.IsValid)
            {
                db.ConnectionCulinaries.Add(connectionCulinary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CulinaryId = new SelectList(db.Culinaries, "Id", "Type", connectionCulinary.CulinaryId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCulinary.TripId);
            return View(connectionCulinary);
        }

        // GET: ConnectionCulinaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionCulinary connectionCulinary = db.ConnectionCulinaries.Find(id);
            if (connectionCulinary == null)
            {
                return HttpNotFound();
            }
            ViewBag.CulinaryId = new SelectList(db.Culinaries, "Id", "Type", connectionCulinary.CulinaryId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCulinary.TripId);
            return View(connectionCulinary);
        }

        // POST: ConnectionCulinaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TripId,CulinaryId")] ConnectionCulinary connectionCulinary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionCulinary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CulinaryId = new SelectList(db.Culinaries, "Id", "Type", connectionCulinary.CulinaryId);
            ViewBag.TripId = new SelectList(db.Trips, "Id", "Id", connectionCulinary.TripId);
            return View(connectionCulinary);
        }

        // GET: ConnectionCulinaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var connectionCulinary = db.ConnectionCulinaries
                .Include(cc => cc.Culinary)
                .Include(cc => cc.Trip)
                .FirstOrDefault(cc => cc.Id == id);
            if (connectionCulinary == null)
            {
                return HttpNotFound();
            }
            return View(connectionCulinary);
        }

        // POST: ConnectionCulinaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConnectionCulinary connectionCulinary = db.ConnectionCulinaries.Find(id);
            db.ConnectionCulinaries.Remove(connectionCulinary);
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
