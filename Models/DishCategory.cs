using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class DishCategory
    {  
        [Key]
        public int DbId { get; set; }
        [StringLength(30)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
