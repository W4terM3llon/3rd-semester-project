using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class DishRequestDTO
    {
        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price field is required")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Description field is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Likes field is required")]
        public int Likes { get; set; }
        [Required(ErrorMessage = "DishCategory field is required")]
        public string DishCategory { get; set; }
        [Required(ErrorMessage = "Restaurant field is required")]
        public string Restaurant { get; set; }
    }
}
