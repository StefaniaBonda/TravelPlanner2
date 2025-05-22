using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TravelPlanner2.Models;
using System.Data.Entity;

namespace TravelPlanner2.Controllers
{
    public class HomeController : Controller
    {
        private MyDBContext db = new MyDBContext();
        public ActionResult Index()
        {
            var trips = db.Trips.Include(t => t.User).ToList(); // Ensure related data is loaded
            return View(trips);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // Sign in
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                Session["UserEmail"] = user.Email;                
                Session["UserId"] = user.Id;
                Session["User"] = user;

                return RedirectToAction("UserDashboard", "Home");
            }
            else
            {               
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
        }

        // Sign up
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string name, string email, string password, string confirmPassword)
        {
            // Verificare ca parolele sa fie identice
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "The passwords don't match.");
                return View();
            }

            // Verificare daca useru-ul nu exista deja
            if (db.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                return View();
            }

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                Role = "User"
            };

            db.Users.Add(user);
            db.SaveChanges();

            ViewBag.Message = "Registration successful!";
            return View();
        }

        public ActionResult UserDashboard()
        {
            
            var email = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("SignIn");
            }

            
            var user = db.Users.FirstOrDefault(u => u.Email == email);

            

            if (user == null)
            {
                ViewBag.ErrorMessage = "User data could not be loaded.";
                return View("Error"); // Redirect to an error view
            }

            return View(user);
        }


       


    }
}