using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        public ActionResult Create()
        {
            return View(new User());
        }

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

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

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

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

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
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Profile()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var user = db.Users.Find(sessionUser.Id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        public ActionResult EditProfile()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var user = db.Users.Find(sessionUser.Id);
            if (user == null)
                return HttpNotFound();

            ViewBag.AvatarOptions = GetAvatarOptions();

            return View("EditProfile", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "Id,Name,Email,Password,Role")] User updatedUser, string SelectedAvatar)
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var userInDb = db.Users.Find(sessionUser.Id);
            if (userInDb == null)
                return HttpNotFound();

            string currentPassword = Request["CurrentPassword"];

            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                updatedUser.Password = userInDb.Password;
            }

            if (updatedUser.Password != userInDb.Password)
            {
                if (string.IsNullOrEmpty(currentPassword) || currentPassword != userInDb.Password)
                {
                    ModelState.AddModelError("", "Incorrect current password. Changes not saved.");
                    ViewBag.AvatarOptions = GetAvatarOptions();
                    return View("EditProfile", updatedUser);
                }
            }

            userInDb.Name = updatedUser.Name;
            userInDb.Email = updatedUser.Email;
            userInDb.Password = updatedUser.Password;

            if (!string.IsNullOrEmpty(SelectedAvatar))
            {
                userInDb.ProfilePicturePath = SelectedAvatar;
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"[VALIDATION ERROR] Property: {error.PropertyName} - Error: {error.ErrorMessage}");
                    }
                }

                throw;
            }
            Session["User"] = userInDb;
            return RedirectToAction("Profile");
        }

        private List<string> GetAvatarOptions()
        {
            var directoryPath = Server.MapPath("~/Uploads/ProfilePictures");
            var filePaths = Directory.GetFiles(directoryPath);
            return filePaths.Select(f => "~/Uploads/ProfilePictures/" + Path.GetFileName(f)).ToList();
        }
    }
}
