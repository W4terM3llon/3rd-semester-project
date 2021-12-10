using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class OrderLineResponseDTO
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public DishResponseDTO Dish { get; set; }

        public static explicit operator OrderLineResponseDTO(OrderLine orderLine)
        {
            if (orderLine == null)
            {
                return null;
            }
            else
            {

                return new OrderLineResponseDTO()
                {
                    Id = orderLine.Id,
                    Quantity = orderLine.Quantity,
                    Dish = (DishResponseDTO)orderLine.Dish,
                };
            }
        }
    }
}