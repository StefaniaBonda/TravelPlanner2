using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class GuesttController : Controller
    {
        private MyDBContext db = new MyDBContext();

        public ActionResult GuestView()
        {
            var trips = db.Trips
            .Include("User") // sau .Include(t => t.User) dacă folosești EF Core
            .Where(t => t.Published)
            .ToList()
                 .Select(t => new TripViewModel
                {
                    TripId = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    KmRange = t.kmRange,
                    CityNames = GetCitiesForTrip(t.Id),
                    Objectives = GetObjectivesForTrip(t.Id),
                    PublishedBy = t.User?.Name ?? "Unknown",
                     PublishedDate = t.PublishedDate
                })
                .ToList();

            return View(trips);
        }

        private List<string> GetCitiesForTrip(int tripId)
        {
            var cities = new List<string>();

            cities.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => c.Buildings.City));
            cities.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => c.Culinary.City));
            cities.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => c.Nature.City));
            cities.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => c.Cultural.City));

            return cities.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
        }

        private List<ObjectiveInfo> GetObjectivesForTrip(int tripId)
        {
            var objectives = new List<ObjectiveInfo>();

            objectives.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Building",
                Name = c.Buildings.Name,
                Latitude = c.Buildings.Latitude,
                Longitude = c.Buildings.Longitude,
                Order = c.Order
            }));

            objectives.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Culinary",
                Name = c.Culinary.Name,
                Latitude = c.Culinary.Latitude,
                Longitude = c.Culinary.Longitude,
                Order = c.Order
            }));

            objectives.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Nature",
                Name = c.Nature.Name,
                Latitude = c.Nature.Latitude,
                Longitude = c.Nature.Longitude,
                Order = c.Order
            }));

            objectives.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Cultural",
                Name = c.Cultural.Name,
                Latitude = c.Cultural.Latitude,
                Longitude = c.Cultural.Longitude,
                Order = c.Order
            }));

            return objectives.OrderBy(o => o.Order).ToList();
        }

        public class TripViewModel
        {
            public int TripId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double KmRange { get; set; }
            public List<string> CityNames { get; set; } = new List<string>();
            public List<ObjectiveInfo> Objectives { get; set; } = new List<ObjectiveInfo>();
            public string PublishedBy { get; set; }
            public DateTime? PublishedDate { get; set; }
        }

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