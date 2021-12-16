using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Booking
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public Table table { get; set; }
        public DiningPeriod diningPeriod { get; set; }
        public User user { get; set; }
        public Restaurant restaurant { get; set; }
    }
}
