using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class ViewTripsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        public ActionResult Index()
        {
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var trips = db.Trips
                .Where(t => t.UserId == currentUserId.Value)
                .ToList()
                .Select(t => new TripViewModel
                {
                    TripId = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Published = t.Published,
                    Price = t.price,
                    KmRange = t.kmRange,
                    TimeRange = t.timeRange,
                    CityNames = GetCitiesForTrip(t.Id)
                })
                .ToList();

            return View(trips);
        }

        private List<string> GetCitiesForTrip(int tripId)
        {
            var cities = new List<string>();

            var buildingCities = db.ConnectionBuildingss
                .Where(cb => cb.TripId == tripId)
                .Select(cb => cb.Buildings.City);

            var culinaryCities = db.ConnectionCulinaries
                .Where(cc => cc.TripId == tripId)
                .Select(cc => cc.Culinary.City);

            var natureCities = db.ConnectionNatures
                .Where(cn => cn.TripId == tripId)
                .Select(cn => cn.Nature.City);

            var culturalCities = db.ConnectionCulturals
                .Where(cc => cc.TripId == tripId)
                .Select(cc => cc.Cultural.City);

            cities.AddRange(buildingCities);
            cities.AddRange(culinaryCities);
            cities.AddRange(natureCities);
            cities.AddRange(culturalCities);

            return cities
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPublish(int tripId)
        {
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var trip = db.Trips.FirstOrDefault(t => t.Id == tripId && t.UserId == currentUserId.Value);
            if (trip == null)
            {
                return HttpNotFound();
            }

            TempData["Success"] = $"Request to publish trip \"{trip.Name}\" sent to admin.";
            return RedirectToAction("Index");
        }

        public class TripViewModel
        {
            public int TripId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Published { get; set; }
            public double Price { get; set; }
            public double KmRange { get; set; }
            public double TimeRange { get; set; }

            public List<string> CityNames { get; set; } = new List<string>();
        }
    }
}
