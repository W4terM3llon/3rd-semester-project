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
        public int Quantity { get; set; }
        public string Dish { get; set; }
    }
}
