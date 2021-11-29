using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Restaurant
    {
        [Key]
        [JsonIgnore]
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public Address Address { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; }
        [JsonIgnore]
        public List<Dish> Dishes { get; set; }
        [JsonIgnore]
        public List<Table> Tables { get; set; }
        [JsonIgnore]
        public List<Booking> Bookings { get; set; }
        [JsonIgnore]
        public RestaurantManager Manager { get; set; }
        [JsonIgnore]
        [ForeignKey("RestaurantEveryDayUseAccount")]
        public RestaurantEveryDayUseAccount EveryDayUseAccount { get; set; }
}
}
