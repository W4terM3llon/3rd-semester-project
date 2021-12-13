using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class OrderLine
    {
        [Key]
        public int DbId { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }
        public Dish Dish { get; set; }
    }
}