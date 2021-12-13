using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Order
    {
        [Key]
        public int DbId { get; set; }
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public OrderStage OrderStage { get; set; }
        public User Customer { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}
