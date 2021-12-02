using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Customers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address AccountingAddress { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Order> Orders { get; set; }
}
}
