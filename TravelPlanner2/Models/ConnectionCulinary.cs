using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner2.Models
{
    public class ConnectionCulinary
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; } 

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }


        [ForeignKey("Culinary")]
        public int CulinaryId { get; set; }
        public Culinary Culinary { get; set; }
    }
}
