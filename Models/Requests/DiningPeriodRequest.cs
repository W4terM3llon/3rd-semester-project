using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class DiningPeriodRequest
    {
        public string Id { get; set; }
        public int TimeStartMinutes { get; set; } //start of booking in minutes in a day
        public int DurationMinutes { get; set; } //booking duration in minutes
        public string Name { get; set; }
        public string Restaurant { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
