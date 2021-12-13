using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class DishCategoryRequestDTO
    {
        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Restaurant field is required")]
        public string Restaurant { get; set; }
    }
}
