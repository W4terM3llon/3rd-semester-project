using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Table
    {
        [Key]
        [JsonIgnore]
        public int DbId { get; set; }
        public string Id { get; set; }
        public Restaurant Restaurant { get; set; }
        public string SeatNumber { get; set; }
        public string Description { get; set; }
        public List<DiningPeriod> AvailablePeriods { get; set; } //many to many
    }
}
