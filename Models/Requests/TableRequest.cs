using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class TableRequest
    {
        public string Id { get; set; }
        public string Restaurant { get; set; }
        public string SeatNumber { get; set; }
        public string Description { get; set; }
        public List<String> AvailablePeriods { get; set; }
    }
}
