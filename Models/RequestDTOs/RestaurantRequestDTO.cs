using Restaurant_system_new.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class RestaurantRequestDTO
    {
        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "IsTableBookingEnabled field is required")]
        public bool IsTableBookingEnabled { get; set; }
        [Required(ErrorMessage = "IsDeliveryAvailable field is required")]
        public bool IsDeliveryAvailable { get; set; }
        [Required(ErrorMessage = "Address field is required")]
        public AddressRequestDTO Address { get; set; }
        [Required(ErrorMessage = "EveryDayUseAccountEmail field is required")]
        public string EveryDayUseAccountEmail { get; set; }
        [Required(ErrorMessage = "EveryDayUseAccountPassword field is required")]
        public string EveryDayUseAccountPassword { get; set; }
    }
}
