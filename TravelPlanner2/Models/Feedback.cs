using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using TravelPlanner2.Models;

public class Feedback
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Trip")]
    public int TripId { get; set; }
    public Trip Trip { get; set; }

    public string Comment { get; set; }
    public int Rating { get; set; } // 1 to 5 stars

    public DateTime CreatedAt { get; set; } = DateTime.Now; // Corrected initialization
}
