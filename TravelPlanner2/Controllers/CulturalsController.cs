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
    public class CulturalsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: Culturals
        public ActionResult Index()
        {
            return View(db.Culturals.ToList());
        }

        // GET: Culturals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cultural cultural = db.Culturals.Find(id);
            if (cultural == null)
            {
                return HttpNotFound();
            }
            return View(cultural);
        }

        // GET: Culturals/Create
        public ActionResult Create()
        {
            return View(new Cultural());
        }

        // POST: Culturals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Cultural cultural)
        {
            if (ModelState.IsValid)
            {
                db.Culturals.Add(cultural);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cultural);
        }

        // GET: Culturals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cultural cultural = db.Culturals.Find(id);
            if (cultural == null)
            {
                return HttpNotFound();
            }
            return View(cultural);
        }

        // POST: Culturals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Cultural cultural)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cultural).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cultural);
        }

        // GET: Culturals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cultural cultural = db.Culturals.Find(id);
            if (cultural == null)
            {
                return HttpNotFound();
            }
            return View(cultural);
        }

        // POST: Culturals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cultural cultural = db.Culturals.Find(id);
            db.Culturals.Remove(cultural);
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
