using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Booking
    {
        public int DBId { get; set; }
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public Table Table { get; set; }
        public DiningPeriod DiningPeriod { get; set; }
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
