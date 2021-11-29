using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class BookingRequest
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Table { get; set; }
        public string DiningPeriod { get; set; }
        public string User { get; set; }
        public string Restaurant { get; set; }
    }
}
