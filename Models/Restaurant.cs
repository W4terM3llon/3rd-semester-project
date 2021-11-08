using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Address Address{ get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
    }
}
