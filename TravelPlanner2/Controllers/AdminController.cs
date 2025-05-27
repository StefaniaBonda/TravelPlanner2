using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class AdminController : Controller
    {
        private MyDBContext db = new MyDBContext();

        public ActionResult AdminDashboard()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null || sessionUser.Role != "Admin")
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
       

        public ActionResult PendingTrips()
        {
            var pendingTrips = db.Trips
                .Where(t => t.PublishRequested && !t.Published)
                .ToList();

            return View(pendingTrips);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovePublish(int tripId)
        {
            var trip = db.Trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null)
            {
                return HttpNotFound();
            }

            trip.Published = true;
            trip.PublishedDate = DateTime.Now;
            trip.PublishRequested = false;
            db.SaveChanges();

            TempData["Success"] = $"Trip \"{trip.Name}\" has been published.";
            return RedirectToAction("PendingTrips");
        }
      

        public ActionResult ManageUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }
    }

}

