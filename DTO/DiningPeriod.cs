using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class DiningPeriod
    {
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int TimeStartMinutes { get; set; } //start of booking in minutes in a day
        public int DurationMinutes { get; set; } //booking duration in minutes
        public Restaurant Restaurant { get; set; }
    }
}
