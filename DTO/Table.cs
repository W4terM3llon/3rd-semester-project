using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.DTO
{
    public class Table
    {
        public int DbId { get; set; }
        public string Id { get; set; }
        public string SeatNumber { get; set; }
        public string Description { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
