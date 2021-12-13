using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class OrderRequestDTO
    {
        [Required(ErrorMessage = "OrderLines field is required")]
        public List<OrderLineRequestDTO> OrderLines { get; set; }
        [Required(ErrorMessage = "Customer field is required")]
        public string Customer { get; set; }
        [Required(ErrorMessage = "Restaurant field is required")]
        public string Restaurant { get; set; }
    }
}
