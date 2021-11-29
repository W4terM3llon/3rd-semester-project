using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class OrderStage
    {
        [Key]
        [JsonIgnore]
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

    }
}