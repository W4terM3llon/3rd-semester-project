using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class OrderStageResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static explicit operator OrderStageResponseDTO(OrderStage orderStage)
        {
            if (orderStage == null)
            {
                return null;
            }
            else
            {

                return new OrderStageResponseDTO()
                {
                    Id = orderStage.Id,
                    Name = orderStage.Name,
                };
            }
        }
    }
}