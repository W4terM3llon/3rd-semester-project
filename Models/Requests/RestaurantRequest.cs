using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class RestaurantRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public Address Address { get; set; }
        public string EveryDayUseAccountEmail { get; set; }
    }
}
