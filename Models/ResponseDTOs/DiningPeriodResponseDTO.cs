using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class DiningPeriodResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TimeStartMinutes { get; set; }
        public int DurationMinutes { get; set; }
        public RestaurantResponseDTO Restaurant { get; set; }

        public static explicit operator DiningPeriodResponseDTO(DiningPeriod diningPeriod)
        {
            if (diningPeriod == null)
            {
                return null;
            }
            else
            {

                return new DiningPeriodResponseDTO()
                {
                    Id = diningPeriod.Id,
                    Name = diningPeriod.Name,
                    TimeStartMinutes = diningPeriod.TimeStartMinutes,
                    DurationMinutes = diningPeriod.DurationMinutes,
                    Restaurant = (RestaurantResponseDTO)diningPeriod.Restaurant
                };
            }
        }
    }
}
