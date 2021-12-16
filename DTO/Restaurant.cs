using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Restaurant
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool isTableBookingEnabled { get; set; }
        public bool isDeliveryAvailable { get; set; }
        public Address address { get; set; }
        public string everyDayUseAccountEmail { get; set; }
        public string everyDayUseAccountPassword { get; set; }
    }
}
