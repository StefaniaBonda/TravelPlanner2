using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class SaveTripController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: SaveTrip/SaveTrip
        public ActionResult SaveTrip()
        {
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                return RedirectToAction("CreateTrip", "Trips");
            }

            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                return RedirectToAction("SignImn", "Home");
            }

            var trip = db.Trips.Find(tripId);
            if (trip == null)
            {
                return HttpNotFound();
            }

            // Always recalculate kmRange before showing
            UpdateTripDistance(trip.Id);

            

            var viewModel = new SaveTripViewModel
            {
                kmRange = trip.kmRange,
                TripId = trip.Id,
                Name = trip.Name,
                Description = trip.Description,
                
            };

            return View(viewModel);
        }

        // POST: SaveTrip/SaveTrip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTrip(SaveTripViewModel model)
        {
            int? tripId = Session["TripId"] as int?;
            if (tripId == null || tripId != model.TripId)
            {
                return RedirectToAction("CreateTrip", "Trips");
            }

            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var trip = db.Trips.Find(tripId);
            if (trip == null)
            {
                return HttpNotFound();
            }

            // Always recalculate kmRange before saving
            UpdateTripDistance(trip.Id);
            db.Entry(trip).Reload(); // Refresh trip from DB

            trip.Name = model.Name;
            trip.Description = model.Description;
          

            db.Entry(trip).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("SaveTrip");
        }

        [HttpGet]
        public JsonResult GetOrderedCulinaryLocations(int tripId)
        {
            var locations = db.ConnectionCulinaries
                .Where(cc => cc.TripId == tripId)
                .OrderBy(cc => cc.Order)
                .Select(cc => new {
                    cc.Culinary.Name,
                    cc.Culinary.Description,
                    cc.Culinary.Latitude,
                    cc.Culinary.Longitude
                })
                .ToList();

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderedBuildingLocations(int tripId)
        {
            var locations = db.ConnectionBuildingss
                .Where(cc => cc.TripId == tripId)
                .OrderBy(cc => cc.Order)
                .Select(cc => new {
                    cc.Buildings.Name,
                    cc.Buildings.Description,
                    cc.Buildings.Latitude,
                    cc.Buildings.Longitude
                })
                .ToList();

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderedNatureLocations(int tripId)
        {
            var locations = db.ConnectionNatures
                .Where(cc => cc.TripId == tripId)
                .OrderBy(cc => cc.Order)
                .Select(cc => new {
                    cc.Nature.Name,
                    cc.Nature.Description,
                    cc.Nature.Latitude,
                    cc.Nature.Longitude
                })
                .ToList();

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderedCulturalLocations(int tripId)
        {
            var locations = db.ConnectionCulturals
                .Where(cc => cc.TripId == tripId)
                .OrderBy(cc => cc.Order)
                .Select(cc => new {
                    cc.Cultural.Name,
                    cc.Cultural.Description,
                    cc.Cultural.Latitude,
                    cc.Cultural.Longitude
                })
                .ToList();

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        // Helper method to calculate and update trip distance in the database
        private void UpdateTripDistance(int tripId)
        {
            try
            {
                var locations = new List<LocationWithCoordinates>();

                locations.AddRange(
                    db.ConnectionBuildingss
                        .Where(cb => cb.TripId == tripId)
                        .Select(cb => new LocationWithCoordinates
                        {
                            Latitude = cb.Buildings.Latitude,
                            Longitude = cb.Buildings.Longitude,
                            Order = cb.Order
                        })
                        .ToList()
                );
                locations.AddRange(
                    db.ConnectionCulinaries
                        .Where(cc => cc.TripId == tripId)
                        .Select(cc => new LocationWithCoordinates
                        {
                            Latitude = cc.Culinary.Latitude,
                            Longitude = cc.Culinary.Longitude,
                            Order = cc.Order
                        })
                        .ToList()
                );
                locations.AddRange(
                    db.ConnectionCulturals
                        .Where(cc => cc.TripId == tripId)
                        .Select(cc => new LocationWithCoordinates
                        {
                            Latitude = cc.Cultural.Latitude,
                            Longitude = cc.Cultural.Longitude,
                            Order = cc.Order
                        })
                        .ToList()
                );
                locations.AddRange(
                    db.ConnectionNatures
                        .Where(cn => cn.TripId == tripId)
                        .Select(cn => new LocationWithCoordinates
                        {
                            Latitude = cn.Nature.Latitude,
                            Longitude = cn.Nature.Longitude,
                            Order = cn.Order
                        })
                        .ToList()
                );

                locations = locations.OrderBy(l => l.Order).ToList();

                double totalDistance = 0;
                for (int i = 0; i < locations.Count - 1; i++)
                {
                    var from = locations[i];
                    var to = locations[i + 1];
                    totalDistance += CalculateDistanceKm(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
                }

                var trip = db.Trips.Find(tripId);
                if (trip != null)
                {
                    trip.kmRange = totalDistance;
                    db.Entry(trip).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating trip distance: {ex.Message}");
            }
        }

        // Helper method to calculate distance between two points in kilometers
        private double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        // Helper class for location coordinates
        private class LocationWithCoordinates
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int Order { get; set; }
        }

        // ViewModel
        public class SaveTripViewModel
        {
            public int TripId { get; set; }

            [StringLength(100)]
            public string Name { get; set; }

            [StringLength(500)]
            [AllowHtml]
            public string Description { get; set; }

            public bool IsFavorite { get; set; }
            public double kmRange { get; set; }
        }
    }
}
