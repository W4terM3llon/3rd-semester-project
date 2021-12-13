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
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public Address Address { get; set; }
        public User Manager { get; set; }
        [ForeignKey("RestaurantEveryDayUseAccount")]
        public User EveryDayUseAccount { get; set; }
}
}
