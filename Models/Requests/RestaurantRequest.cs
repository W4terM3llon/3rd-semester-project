using Restaurant_system_new.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class RestaurantRequest
    {
        public string Name { get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public AddressRequest Address { get; set; }
        public string EveryDayUseAccountEmail { get; set; }
        public string EveryDayUseAccountPassword { get; set; }
    }
}
