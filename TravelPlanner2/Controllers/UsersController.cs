﻿using System;
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
    public class UsersController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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


        // Action for the View Profile button
        public ActionResult Profile()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var user = db.Users.Find(sessionUser.Id);
            if (user == null)
                return HttpNotFound();

            return View(user); // Create Views/Users/Profile.cshtml
        }

        public ActionResult EditProfile()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var user = db.Users.Find(sessionUser.Id);
            if (user == null)
                return HttpNotFound();

            return View("EditProfile", user); // Reuse your existing Edit.cshtml view
        }

        // POST: Users/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "Id,Name,Email,Password,Role")] User updatedUser)
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var userInDb = db.Users.Find(sessionUser.Id);
            if (userInDb == null)
                return HttpNotFound();

            string currentPassword = Request["CurrentPassword"];

            // If password was changed, check current password
            if (updatedUser.Password != userInDb.Password)
            {
                if (string.IsNullOrEmpty(currentPassword) || currentPassword != userInDb.Password)
                {
                    ModelState.AddModelError("", "Incorrect current password. Changes not saved.");
                    return View("EditProfile", updatedUser);
                }
            }

            // Update other fields
            userInDb.Name = updatedUser.Name;
            userInDb.Email = updatedUser.Email;

            if (updatedUser.Password != userInDb.Password)
                userInDb.Password = updatedUser.Password;

            db.SaveChanges();
            Session["User"] = userInDb;

            return RedirectToAction("Profile");
        }

        public ActionResult AdminDashboard()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null || sessionUser.Role != "Admin")
            {
                return RedirectToAction("SignIn", "Home");
            }


            return RedirectToAction("AdminDashboard", "Admin");
        }



    }
}
