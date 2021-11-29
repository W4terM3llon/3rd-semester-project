using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    [Table("RestaurantEveryDayUseAccount")]
    public class RestaurantEveryDayUseAccount : User
    {
        public Restaurant Restaurant { get; set; }
    }
}
