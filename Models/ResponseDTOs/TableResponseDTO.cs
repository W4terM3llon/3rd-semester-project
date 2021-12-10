using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class TableResponseDTO
    {
        public string Id { get; set; }
        public string SeatNumber { get; set; }
        public string Description { get; set; }
        public RestaurantResponseDTO Restaurant { get; set; }

        public static explicit operator TableResponseDTO(Table table)
        {
            if (table == null)
            {
                return null;
            }
            else
            {

                return new TableResponseDTO()
                {
                    Id = table.Id,
                    SeatNumber = table.SeatNumber,
                    Description = table.Description,
                    Restaurant = (RestaurantResponseDTO)table.Restaurant
                };
            }
        }
    }
}
