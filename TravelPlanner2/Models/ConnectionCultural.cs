using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner2.Models
{
    public class ConnectionCultural
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }


        [ForeignKey("Cultural")]
        public int CulturalId { get; set; }
        public Cultural Cultural { get; set; }
    }
}
