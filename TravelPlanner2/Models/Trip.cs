using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner2.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public double price { get; set; }
        [Required]
        public double timeRange { get; set; }
        [Required]
        public double kmRange { get; set; }
    }
}
