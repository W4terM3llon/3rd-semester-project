using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    [Table("RestaurantManager")]
    public class RestaurantManager :  User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Restaurant> ManagedRestaurants { get; set; }
    }
}
