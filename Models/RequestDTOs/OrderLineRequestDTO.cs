using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// to be deleted

namespace RestaurantSystem.Models.Requests
{
    public class OrderLineRequestDTO
    {
        [Required(ErrorMessage = "Quantity field is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Dish field is required")]
        public string Dish { get; set; }
    }
}
