using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class OrderResponseDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public List<OrderLineResponseDTO> OrderLines { get; set; }
        public OrderStageResponseDTO OrderStage { get; set; }
        public UserResponseDTO Customer { get; set; }
        public RestaurantResponseDTO Restaurant { get; set; }

        public static explicit operator OrderResponseDTO(Order order)
        {
            if (order == null)
            {
                return null;
            }
            else
            {

                return new OrderResponseDTO()
                {
                    Id = order.Id,
                    Date = order.Date,
                    OrderLines = order.OrderLines.Select(ol => (OrderLineResponseDTO)ol).ToList(),
                    OrderStage = (OrderStageResponseDTO)order.OrderStage,
                    Customer = (UserResponseDTO)order.Customer,
                    Restaurant = (RestaurantResponseDTO)order.Restaurant,
                };
            }
        }
    }
}
