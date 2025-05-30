﻿using System;
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

        [StringLength(100)]
        public string Name { get; set; } 

        [StringLength(500)]
        public string Description { get; set; } 

        public bool Published { get; set; } = false;
        public bool PublishRequested { get; set; }

        public DateTime? PublishedDate { get; set; }


    }
}
