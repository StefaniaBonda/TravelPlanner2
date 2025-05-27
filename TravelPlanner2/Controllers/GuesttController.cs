using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class GuesttController : Controller
    {
        // Database context for accessing data
        private MyDBContext db = new MyDBContext();

        // Displays a list of published trips to guest users
        public ActionResult GuestView()
        {
            // Fetch trips including associated User, filter published trips, then map to TripViewModel
            var trips = db.Trips
                .Include("User") // Include user information for each trip
                .Where(t => t.Published) // Only include published trips
                .ToList()
                .Select(t => new TripViewModel
                {
                    TripId = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    KmRange = t.kmRange,
                    CityNames = GetCitiesForTrip(t.Id), // Get list of cities for the trip
                    Objectives = GetObjectivesForTrip(t.Id), // Get objectives (places) for the trip
                    PublishedBy = t.User?.Name ?? "Unknown", // Use user's name or "Unknown" if null
                    PublishedDate = t.PublishedDate
                })
                .ToList();

            return View(trips); // Return the list of TripViewModels to the view
        }

        // Retrieves a list of distinct city names associated with a specific trip
        private List<string> GetCitiesForTrip(int tripId)
        {
            var cities = new List<string>();

            // Gather city names from different types of trip connections
            cities.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => c.Buildings.City));
            cities.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => c.Culinary.City));
            cities.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => c.Nature.City));
            cities.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => c.Cultural.City));

            // Return distinct, non-empty city names
            return cities.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
        }

        // Retrieves a list of objectives (places of interest) for a specific trip
        private List<ObjectiveInfo> GetObjectivesForTrip(int tripId)
        {
            var objectives = new List<ObjectiveInfo>();

            // Add building objectives
            objectives.AddRange(db.ConnectionBuildingss.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Building",
                Name = c.Buildings.Name,
                Latitude = c.Buildings.Latitude,
                Longitude = c.Buildings.Longitude,
                Order = c.Order
            }));

            // Add culinary objectives
            objectives.AddRange(db.ConnectionCulinaries.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Culinary",
                Name = c.Culinary.Name,
                Latitude = c.Culinary.Latitude,
                Longitude = c.Culinary.Longitude,
                Order = c.Order
            }));

            // Add nature objectives
            objectives.AddRange(db.ConnectionNatures.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Nature",
                Name = c.Nature.Name,
                Latitude = c.Nature.Latitude,
                Longitude = c.Nature.Longitude,
                Order = c.Order
            }));

            // Add cultural objectives
            objectives.AddRange(db.ConnectionCulturals.Where(c => c.TripId == tripId).Select(c => new ObjectiveInfo
            {
                Type = "Cultural",
                Name = c.Cultural.Name,
                Latitude = c.Cultural.Latitude,
                Longitude = c.Cultural.Longitude,
                Order = c.Order
            }));

            // Return the objectives ordered by their specified order
            return objectives.OrderBy(o => o.Order).ToList();
        }

        // ViewModel for presenting trip data to the view
        public class TripViewModel
        {
            public int TripId { get; set; } // Unique ID of the trip
            public string Name { get; set; } // Name of the trip
            public string Description { get; set; } // Description of the trip
            public double KmRange { get; set; } // Distance covered in km
            public List<string> CityNames { get; set; } = new List<string>(); // Cities included in the trip
            public List<ObjectiveInfo> Objectives { get; set; } = new List<ObjectiveInfo>(); // List of objectives (points of interest)
            public string PublishedBy { get; set; } // Name of the user who published the trip
            public DateTime? PublishedDate { get; set; } // Date the trip was published
        }

        // Model representing information about each objective in a trip
        public class ObjectiveInfo
        {
            public string Type { get; set; } // Type of objective (e.g., Building, Nature)
            public string Name { get; set; } // Name of the objective
            public int Order { get; set; } // Display order of the objective
            public double Latitude { get; set; } // Geographic latitude
            public double Longitude { get; set; } // Geographic longitude
        }
    }
}
