using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Table
    {
        public string id { get; set; }
        public string seatNumber { get; set; }
        public string description { get; set; }
        public Restaurant restaurant { get; set; }
    }
}
