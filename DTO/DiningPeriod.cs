using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class DiningPeriod
    {
        public string id { get; set; }
        public string name { get; set; }
        public int timeStartMinutes { get; set; }
        public int durationMinutes { get; set; }
        public Restaurant restaurant { get; set; }
    }
}
