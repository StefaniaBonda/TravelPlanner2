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
    public class CulinariesController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: Culinaries
        public ActionResult Index()
        {
            return View(db.Culinaries.ToList());
        }

        // GET: Culinaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culinary culinary = db.Culinaries.Find(id);
            if (culinary == null)
            {
                return HttpNotFound();
            }
            return View(culinary);
        }

        // GET: Culinaries/Create
        public ActionResult Create()
        {
            return View(new Culinary());
        }

        // POST: Culinaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Culinary culinary)
        {
            if (ModelState.IsValid)
            {
                db.Culinaries.Add(culinary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(culinary);
        }

        // GET: Culinaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culinary culinary = db.Culinaries.Find(id);
            if (culinary == null)
            {
                return HttpNotFound();
            }
            return View(culinary);
        }

        // POST: Culinaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Name,Description,Latitude,Longitude,Price,Country,City")] Culinary culinary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culinary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(culinary);
        }

        // GET: Culinaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culinary culinary = db.Culinaries.Find(id);
            if (culinary == null)
            {
                return HttpNotFound();
            }
            return View(culinary);
        }

        // POST: Culinaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Culinary culinary = db.Culinaries.Find(id);
            db.Culinaries.Remove(culinary);
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
