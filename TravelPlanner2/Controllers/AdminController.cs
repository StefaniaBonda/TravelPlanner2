using System;
using System.Collections.Generic;
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
            // TODO: Fetch and return trips
            return View();
        }

        public ActionResult ManageUsers()
        {
            var sessionUser = Session["User"] as User;
            if (sessionUser == null || sessionUser.Role != "Admin")
            {
                return RedirectToAction("SignIn", "Home");
            }

            var users = db.Users.ToList();
            return View(users); // Returns Views/Admin/ManageUsers.cshtml
        }

    }

}