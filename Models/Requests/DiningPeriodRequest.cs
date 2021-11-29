using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class DiningPeriodRequest
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public int TimeStartMinutes { get; set; }
        public int DurationMinutes { get; set; }
        public string Restaurant { get; set; }
    }
}
