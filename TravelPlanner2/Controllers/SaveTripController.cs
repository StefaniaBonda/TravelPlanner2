using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity; // Asigură-te că e System.Data.Entity pentru EF6
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
            // Obține tripId-ul din sesiune
            int? tripId = Session["TripId"] as int?;
            if (tripId == null)
            {
                // Redirecționează către pagina de creare trip dacă nu există unul activ
                return RedirectToAction("CreateTrip", "Trips");
            }

            // Obține ID-ul utilizatorului curent din sesiune
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                // Dacă utilizatorul nu este logat, redirecționează la login
                return RedirectToAction("Login", "Account");
            }

            // Caută trip-ul în baza de date
            var trip = db.Trips.Find(tripId);
            if (trip == null)
            {
                return HttpNotFound();
            }

            // Verifică dacă trip-ul este deja favorit pentru utilizatorul curent
            bool isFavorited = db.Favorites.Any(f => f.UserId == currentUserId.Value && f.TripId == trip.Id);

            // Creează ViewModel-ul
            var viewModel = new SaveTripViewModel
            {
                TripId = trip.Id,
                Name = trip.Name,
                Description = trip.Description,
                IsFavorite = isFavorited,
            };

            return View(viewModel);
        }

        // POST: SaveTrip/SaveTrip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTrip(SaveTripViewModel model)
        {
            // Obține tripId-ul din sesiune (sau folosește model.TripId, dacă îl ai)
            int? tripId = Session["TripId"] as int?;
            if (tripId == null || tripId != model.TripId) //Verificare suplimentara
            {
                return RedirectToAction("CreateTrip", "Trips"); //Sau o pagina de eroare
            }

            // Obține ID-ul utilizatorului curent din sesiune
            int? currentUserId = Session["UserId"] as int?;
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Găsește trip-ul în baza de date
            var trip = db.Trips.Find(tripId);
            if (trip == null)
            {
                return HttpNotFound();
            }

            // Actualizează proprietățile
            trip.Name = model.Name;
            trip.Description = model.Description;

            // Gestionează Favorite
            var existingFavorite = db.Favorites.FirstOrDefault(f => f.UserId == currentUserId.Value && f.TripId == trip.Id);
            if (model.IsFavorite)
            {
                if (existingFavorite == null)
                {
                    var newFavorite = new Favorite
                    {
                        UserId = currentUserId.Value,
                        TripId = trip.Id
                    };
                    db.Favorites.Add(newFavorite);
                }
            }
            else
            {
                if (existingFavorite != null)
                {
                    db.Favorites.Remove(existingFavorite);
                }
            }

            db.Entry(trip).State = EntityState.Modified;
            db.SaveChanges();

            // Redirecționează înapoi la pagina de detalii
            return RedirectToAction("SaveTrip"); //Sau o alta actiune, daca vrei
        }

        //Model
        public class SaveTripViewModel
        {
            public int TripId { get; set; }

            [StringLength(100)]
            public string Name { get; set; }

            [StringLength(500)]
            [AllowHtml]
            public string Description { get; set; }

            public bool IsFavorite { get; set; }
        }
    }
}