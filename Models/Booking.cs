using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Booking
    {
        [Key]
        [JsonIgnore]
        public int DBId { get; set; }
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public Table Table { get; set; }
        public DiningPeriod DiningPeriod { get; set; }
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
