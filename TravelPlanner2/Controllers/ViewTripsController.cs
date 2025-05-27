using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class ViewTripsController : Controller
    {
        // Database context to interact with the application's data
        private MyDBContext db = new MyDBContext();

        // GET: ViewTrips - Display a list of the current user's trips
        public ActionResult Index()
        {
            // Get the current user ID from session
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                // Redirect to sign-in if user is not logged in
                return RedirectToAction("SignIn", "Home");
            }

            // Retrieve and project trips into view models
            var trips = db.Trips
                .Where(t => t.UserId == currentUserId.Value)
                .ToList()
                .Select(t => new TripViewModel
                {
                    TripId = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Published = t.Published,
                    KmRange = t.kmRange,
                    CityNames = GetCitiesForTrip(t.Id),
                    Objectives = GetObjectivesForTrip(t.Id)
                })
                .ToList();

            return View(trips);
        }

        // Helper method to get distinct city names for a trip
        private List<string> GetCitiesForTrip(int tripId)
        {
            var cities = new List<string>();

            // Aggregate cities from various objective types
            cities.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => c.Buildings.City));
            cities.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => c.Culinary.City));
            cities.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => c.Nature.City));
            cities.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => c.Cultural.City));

            // Return distinct, non-empty city names
            return cities.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
        }

        // Helper method to get objectives (locations) for a trip
        private List<ObjectiveInfo> GetObjectivesForTrip(int tripId)
        {
            var objectives = new List<ObjectiveInfo>();

            // Collect buildings
            objectives.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Building",
                Name = c.Buildings.Name,
                Latitude = c.Buildings.Latitude,
                Longitude = c.Buildings.Longitude,
                Order = c.Order
            }));

            // Collect culinary spots
            objectives.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Culinary",
                Name = c.Culinary.Name,
                Latitude = c.Culinary.Latitude,
                Longitude = c.Culinary.Longitude,
                Order = c.Order
            }));

            // Collect nature spots
            objectives.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Nature",
                Name = c.Nature.Name,
                Latitude = c.Nature.Latitude,
                Longitude = c.Nature.Longitude,
                Order = c.Order
            }));

            // Collect cultural sites
            objectives.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Cultural",
                Name = c.Cultural.Name,
                Latitude = c.Cultural.Latitude,
                Longitude = c.Cultural.Longitude,
                Order = c.Order
            }));

            // Return objectives sorted by order
            return objectives.OrderBy(o => o.Order).ToList();
        }

        // POST: ViewTrips/RequestPublish - Submit a request to publish a trip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPublish(int tripId)
        {
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                // Redirect if not logged in
                return RedirectToAction("SignIn", "Home");
            }

            // Ensure trip belongs to current user
            var trip = db.Trips.FirstOrDefault(t => t.Id == tripId && t.UserId == currentUserId.Value);
            if (trip == null)
            {
                return HttpNotFound();
            }

            // Mark the trip as having requested publication
            trip.PublishRequested = true;
            db.SaveChanges();

            // Notify user of successful request
            TempData["Success"] = $"Request to publish trip \"{trip.Name}\" sent to admin.";
            return RedirectToAction("Index");
        }

        // View model to display trip info on the view
        public class TripViewModel
        {
            public int TripId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Published { get; set; }
            public double KmRange { get; set; }
            public List<string> CityNames { get; set; } = new List<string>();
            public List<ObjectiveInfo> Objectives { get; set; } = new List<ObjectiveInfo>();
        }

        // Class to hold details of an objective (location/stop)
        public class ObjectiveInfo
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
