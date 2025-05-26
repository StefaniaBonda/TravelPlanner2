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
        // GET: Admin/PublishRequests
        public ActionResult PublishRequests()
        {
            // TODO: Fetch and return publish requests
            return View();
        }

        // GET: Admin/ManageTrips
        public ActionResult ManageTrips()
        {
            var trips = db.Trips.ToList();
            return View(trips);
        }

        public ActionResult ManageUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }
    }

}

