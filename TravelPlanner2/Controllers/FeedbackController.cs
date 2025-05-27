using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class FeedbackController : Controller
    {
        private MyDBContext db = new MyDBContext();

        [HttpPost]
        public ActionResult Submit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                TempData["Success"] = "Thank you for your feedback!";
                return RedirectToAction("About", "Home");
            }

            TempData["Error"] = "There was a problem with your submission.";
            return RedirectToAction("About", "Home");
        }

        // Admin or private viewing of feedbacks
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.OrderByDescending(f => f.SubmittedAt).ToList();
            return View(feedbacks);
        }
    }
}