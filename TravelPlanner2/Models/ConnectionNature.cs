using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner2.Models
{
    public class ConnectionNature
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; } 

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }


        [ForeignKey("Nature")]
        public int NatureId { get; set; }
        public Nature Nature { get; set; }
    }
}
