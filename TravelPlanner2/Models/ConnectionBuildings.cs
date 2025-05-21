using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TravelPlanner2.Models;

public class ConnectionBuildings
{
    [Key]
    public int Id { get; set; }
    public int Order { get; set; }
    [ForeignKey("Trip")]
     
    public int TripId { get; set; }
    public Trip Trip { get; set; } // Corrected property name to match ForeignKey attribute
    [ForeignKey("Buildings")]
    public int BuildingsId { get; set; }
    public Buildings Buildings { get; set; } // Corrected property name to match ForeignKey attribute
}
