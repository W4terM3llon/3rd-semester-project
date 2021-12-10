using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class RestaurantResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsTableBookingEnabled { get; set; }
        public bool IsDeliveryAvailable { get; set; }
        public AddressResponseDTO Address { get; set; }

        public static explicit operator RestaurantResponseDTO(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                return null;
            }
            else
            {

                return new RestaurantResponseDTO()
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    IsTableBookingEnabled = restaurant.IsTableBookingEnabled,
                    IsDeliveryAvailable = restaurant.IsDeliveryAvailable,
                    Address = (AddressResponseDTO)restaurant.Address,
                };
            }
        }
    }
}
