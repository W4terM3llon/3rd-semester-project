using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class TableRequestDTO
    {
        public string SeatNumber { get; set; }
        public string Description { get; set; }
        public string Restaurant { get; set; }
    }
}
