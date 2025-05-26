using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelPlanner2.Models;

namespace TravelPlanner2.Controllers
{
    public class TripsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // GET: Trips/CreateTrip
        public ActionResult CreateTrip()
        {
            var userId = Session["UserId"];

            // Validate UserId
            if (userId == null || !(userId is int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid UserId in session.");
            }
            var newTrip = new Trip
            {
                UserId = (int)userId,
                price = 0,
                timeRange = 0,
                kmRange = 0,
            };

            db.Trips.Add(newTrip);
            db.SaveChanges();
            Session["TripId"] = newTrip.Id;
            return View();
        }

        [HttpGet]
        public JsonResult GetTripId()
        {
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(tripId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddLocation(Location data)
        {
            if (data == null || string.IsNullOrEmpty(data.Type))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Date invalide");

            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No active trip found");

            switch (data.Type)
            {
                case "Buildings":
                    var existingBuilding = db.Buildingss.FirstOrDefault(b =>
                    b.Name == data.Name &&
                    b.City == data.City &&
                    b.Country == data.Country &&
                    Math.Abs(b.Latitude - data.Latitude) < 0.0001 &&
                    Math.Abs(b.Longitude - data.Longitude) < 0.0001);

                    if (existingBuilding == null)
                    {
                        existingBuilding = new Buildings
                        {
                            Type = "Buildings",
                            Name = data.Name,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            Description = data.Description,
                            Country = data.Country,
                            City = data.City,
                            Price = data.Price

                        };
                        db.Buildingss.Add(existingBuilding);
                        db.SaveChanges();

                    }

                    var alreadyLinkedB = db.ConnectionBuildingss.Any(cb =>
                    cb.TripId == tripId.Value && cb.BuildingsId == existingBuilding.Id);

                    if (!alreadyLinkedB)
                    {
                        var connB = new ConnectionBuildings
                        {
                            TripId = tripId.Value,
                            BuildingsId = existingBuilding.Id,
                        };
                        db.ConnectionBuildingss.Add(connB);
                    }

                    break;

                case "Culinary":
                    var existingCulinary = db.Culinaries.FirstOrDefault(c =>
                        c.Name == data.Name &&
                        c.City == data.City &&
                        c.Country == data.Country &&
                        Math.Abs(c.Latitude - data.Latitude) < 0.0001 &&
                        Math.Abs(c.Longitude - data.Longitude) < 0.0001);

                    if (existingCulinary == null)
                    {
                        existingCulinary = new Culinary
                        {
                            Type = "Culinary",
                            Name = data.Name,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            Description = data.Description,
                            Country = data.Country,
                            City = data.City,
                            Price = data.Price
                        };
                        db.Culinaries.Add(existingCulinary);
                        db.SaveChanges();
                    }

                    var alreadyLinkedC = db.ConnectionCulinaries.Any(cc =>
                        cc.TripId == tripId.Value && cc.CulinaryId == existingCulinary.Id);

                    if (!alreadyLinkedC)
                    {
                        var connCul = new ConnectionCulinary
                        {
                            TripId = tripId.Value,
                            CulinaryId = existingCulinary.Id
                        };
                        db.ConnectionCulinaries.Add(connCul);
                    }

                    break;


                case "Cultural":
                    var existingCultural = db.Culturals.FirstOrDefault(c =>
                        c.Name == data.Name &&
                        c.City == data.City &&
                        c.Country == data.Country &&
                        Math.Abs(c.Latitude - data.Latitude) < 0.0001 &&
                        Math.Abs(c.Longitude - data.Longitude) < 0.0001);

                    if (existingCultural == null)
                    {
                        existingCultural = new Cultural
                        {
                            Type = "Cultural",
                            Name = data.Name,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            Description = data.Description,
                            Country = data.Country,
                            City = data.City,
                            Price = data.Price
                        };
                        db.Culturals.Add(existingCultural);
                        db.SaveChanges();
                    }

                    var alreadyLinkedCu = db.ConnectionCulturals.Any(cc =>
                        cc.TripId == tripId.Value && cc.CulturalId == existingCultural.Id);

                    if (!alreadyLinkedCu)
                    {
                        var connCulCul = new ConnectionCultural
                        {
                            TripId = tripId.Value,
                            CulturalId = existingCultural.Id
                        };
                        db.ConnectionCulturals.Add(connCulCul);
                    }
                    break;

                case "Nature":
                    var existingNature = db.Natures.FirstOrDefault(n =>
                        n.Name == data.Name &&
                        n.City == data.City &&
                        n.Country == data.Country &&
                        Math.Abs(n.Latitude - data.Latitude) < 0.0001 &&
                        Math.Abs(n.Longitude - data.Longitude) < 0.0001);

                    if (existingNature == null)
                    {
                        existingNature = new Nature
                        {
                            Type = "Nature",
                            Name = data.Name,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            Description = data.Description,
                            Country = data.Country,
                            City = data.City,
                            Price = data.Price
                        };
                        db.Natures.Add(existingNature);
                        db.SaveChanges();
                    }

                    var alreadyLinkedN = db.ConnectionNatures.Any(cn =>
                        cn.TripId == tripId.Value && cn.NatureId == existingNature.Id);

                    if (!alreadyLinkedN)
                    {
                        var connNat = new ConnectionNature
                        {
                            TripId = tripId.Value,
                            NatureId = existingNature.Id
                        };
                        db.ConnectionNatures.Add(connNat);
                    }
                    break;

                default:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Type not recognized");
            }
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Location added successfully");
        }

        public ActionResult ViewTrip()
        {
            // Check if a trip is in session
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                return RedirectToAction("CreateTrip");
            }

            // Calculate and update kmRange in the database
            UpdateTripDistance(tripId.Value);

            return View();
        }

        // GET: GetTripLocations
        [HttpGet]
        public JsonResult GetTripLocations()
        {
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var locations = new List<object>();

            // Fetch Buildings for this trip
            var buildingLocations = db.ConnectionBuildingss
                .Where(cb => cb.TripId == tripId)
                .Select(cb => new
                {
                    Type = "Buildings",
                    Name = cb.Buildings.Name,
                    Latitude = cb.Buildings.Latitude,
                    Longitude = cb.Buildings.Longitude,
                    City = cb.Buildings.City,
                    Country = cb.Buildings.Country,
                    Description = cb.Buildings.Description
                });

            // Fetch Culinary locations for this trip
            var culinaryLocations = db.ConnectionCulinaries
                .Where(cc => cc.TripId == tripId)
                .Select(cc => new
                {
                    Type = "Culinary",
                    Name = cc.Culinary.Name,
                    Latitude = cc.Culinary.Latitude,
                    Longitude = cc.Culinary.Longitude,
                    City = cc.Culinary.City,
                    Country = cc.Culinary.Country,
                    Description = cc.Culinary.Description
                });

            // Fetch Cultural locations for this trip
            var culturalLocations = db.ConnectionCulturals
                .Where(cc => cc.TripId == tripId)
                .Select(cc => new
                {
                    Type = "Cultural",
                    Name = cc.Cultural.Name,
                    Latitude = cc.Cultural.Latitude,
                    Longitude = cc.Cultural.Longitude,
                    City = cc.Cultural.City,
                    Country = cc.Cultural.Country,
                    Description = cc.Cultural.Description
                });

            // Fetch Nature locations for this trip
            var natureLocations = db.ConnectionNatures
                .Where(cn => cn.TripId == tripId)
                .Select(cn => new
                {
                    Type = "Nature",
                    Name = cn.Nature.Name,
                    Latitude = cn.Nature.Latitude,
                    Longitude = cn.Nature.Longitude,
                    City = cn.Nature.City,
                    Country = cn.Nature.Country,
                    Description = cn.Nature.Description
                });

            // Combine all locations
            locations.AddRange(buildingLocations);
            locations.AddRange(culinaryLocations);
            locations.AddRange(culturalLocations);
            locations.AddRange(natureLocations);

            // Check if we have a custom order saved in session
            var savedOrder = Session[$"TripLocationOrder_{tripId}"] as List<LocationOrderItem>;
            if (savedOrder != null && savedOrder.Any())
            {
                var orderedLocations = new List<object>();

                // First add locations in the saved order
                foreach (var orderItem in savedOrder)
                {
                    var location = locations.FirstOrDefault(l =>
                        IsMatchingLocation(l, orderItem.Type, orderItem.Name, orderItem.City));

                    if (location != null)
                    {
                        orderedLocations.Add(location);
                    }
                }

                // Then add any remaining locations that weren't in the saved order
                foreach (var location in locations)
                {
                    if (!orderedLocations.Contains(location))
                    {
                        orderedLocations.Add(location);
                    }
                }

                return Json(orderedLocations, JsonRequestBehavior.AllowGet);
            }

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        private bool IsMatchingLocation(dynamic location, string type, string name, string city)
        {
            return location.Type == type &&
                   location.Name == name &&
                   location.City == city;
        }

        // Action to continue the existing trip
        public ActionResult ContinueTrip()
        {
            // Check if a trip is already in session
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                // If no trip in session, redirect to create a new trip
                return RedirectToAction("CreateTrip");
            }

            // Render the CreateTrip view with the existing trip
            return View("CreateTrip");
        }

        // New method to save location order
        [HttpPost]
        public ActionResult SaveLocationOrder(List<LocationOrderItem> orderData)
        {
            // Validate input
            if (orderData == null || !orderData.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid location order data");
            }

            // Get the current trip ID from session
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No active trip found");
            }

            try
            {
                for (int i = 0; i < orderData.Count; i++)
                {
                    var item = orderData[i];

                    switch (item.Type)
                    {
                        case "Buildings":
                            var cb = db.ConnectionBuildingss.FirstOrDefault(x =>
                                x.TripId == tripId && x.Buildings.Name == item.Name && x.Buildings.City == item.City);
                            if (cb != null)
                                cb.Order = i;
                            break;

                        case "Culinary":
                            var cc = db.ConnectionCulinaries.FirstOrDefault(x =>
                                x.TripId == tripId && x.Culinary.Name == item.Name && x.Culinary.City == item.City);
                            if (cc != null)
                                cc.Order = i;
                            break;

                        case "Cultural":
                            var ccul = db.ConnectionCulturals.FirstOrDefault(x =>
                                x.TripId == tripId && x.Cultural.Name == item.Name && x.Cultural.City == item.City);
                            if (ccul != null)
                                ccul.Order = i;
                            break;

                        case "Nature":
                            var cn = db.ConnectionNatures.FirstOrDefault(x =>
                                x.TripId == tripId && x.Nature.Name == item.Name && x.Nature.City == item.City);
                            if (cn != null)
                                cn.Order = i;
                            break;
                    }
                }

                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Helper method to calculate and update trip distance in the database
        private void UpdateTripDistance(int tripId)
        {
            try
            {
                // Gather all locations for this trip, ordered by their Order property
                var locations = new List<LocationWithCoordinates>();

                // Add all locations from all types, but keep their Order property
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

                // Sort all locations by Order
                locations = locations.OrderBy(l => l.Order).ToList();

                // Calculate total distance
                double totalDistance = 0;
                for (int i = 0; i < locations.Count - 1; i++)
                {
                    var from = locations[i];
                    var to = locations[i + 1];
                    totalDistance += CalculateDistanceKm(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
                }

                // Update the trip's kmRange
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
    }

    // Model for location order items
    public class LocationOrderItem
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
