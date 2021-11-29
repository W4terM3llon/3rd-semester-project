using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantSystem.Models
{
    public class Discount
    {
        [Key]
        [JsonIgnore]
        public int DbId { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool isDisposable { get; set; }
    }
}