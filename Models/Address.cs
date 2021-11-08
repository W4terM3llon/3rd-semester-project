using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Address
    {
        [Key]
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Street { get; set; }
        public string Appartment { get; set; }
    }
}
