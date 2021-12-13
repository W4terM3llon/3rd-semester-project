using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Dish
    {
        [Key]
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public DishCategory DishCategory {get; set;}
        public Restaurant Restaurant { get; set; }
    }
}