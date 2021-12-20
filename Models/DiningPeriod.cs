using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class DiningPeriod
    {
        [Key]
        public int DbId { get; set; }
        [StringLength(30)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int TimeStartMinutes { get; set; } //start of booking in minutes in a day
        public int DurationMinutes { get; set; } //booking duration in minutes
        public Restaurant Restaurant { get; set; }
    }
}
