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
        // Database context to access the application's data
        private MyDBContext db = new MyDBContext();

        // GET: Users - Display list of all users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5 - Show details of a specific user
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // GET: Users/Create - Render user creation form
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: Users/Create - Handle form submission to create a new user
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

        // GET: Users/Edit/5 - Render form to edit an existing user
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Users/Edit - Handle form submission to update user details
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

        // GET: Users/Delete/5 - Confirm deletion of a user
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Users/DeleteConfirmed - Final deletion of a user
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Clean up database resources when the controller is disposed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        // GET: Users/Profile - Show current user's profile
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

        // GET: Users/EditProfile - Show form to edit the current user's profile
        public ActionResult EditProfile()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null)
                return RedirectToAction("SignIn", "Home");

            var user = db.Users.Find(sessionUser.Id);
            if (user == null)
                return HttpNotFound();

            ViewBag.AvatarOptions = GetAvatarOptions(); // Provide available avatar images

            return View("EditProfile", user);
        }

        // POST: Users/EditProfile - Save changes to current user's profile
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

            // Preserve existing password if new one is not provided
            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                updatedUser.Password = userInDb.Password;
            }

            // Validate password change
            if (updatedUser.Password != userInDb.Password)
            {
                if (string.IsNullOrEmpty(currentPassword) || currentPassword != userInDb.Password)
                {
                    ModelState.AddModelError("", "Incorrect current password. Changes not saved.");
                    ViewBag.AvatarOptions = GetAvatarOptions();
                    return View("EditProfile", updatedUser);
                }
            }

            // Update user fields
            userInDb.Name = updatedUser.Name;
            userInDb.Email = updatedUser.Email;
            userInDb.Password = updatedUser.Password;

            // Update profile picture if a new avatar was selected
            if (!string.IsNullOrEmpty(SelectedAvatar))
            {
                userInDb.ProfilePicturePath = SelectedAvatar;
            }

            // Save changes to database and handle any validation errors
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

                throw; // Re-throw exception after logging
            }

            // Update session with new user data
            Session["User"] = userInDb;
            return RedirectToAction("Profile");
        }

        // Helper method to get all available avatar image file paths
        private List<string> GetAvatarOptions()
        {
            var directoryPath = Server.MapPath("~/Uploads/ProfilePictures");
            var filePaths = Directory.GetFiles(directoryPath);
            return filePaths.Select(f => "~/Uploads/ProfilePictures/" + Path.GetFileName(f)).ToList();
        }
    }
}
