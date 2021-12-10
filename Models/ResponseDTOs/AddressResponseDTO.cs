using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class AddressResponseDTO
    {
        public string Id { get; set; }
        public string Street { get; set; }
        public string Appartment { get; set; }

        public static explicit operator AddressResponseDTO(Address address)
        {
            if (address == null)
            {
                return null;
            }
            else
            {

                return new AddressResponseDTO()
                {
                    Id = address.Id,
                    Street = address.Street,
                    Appartment = address.Appartment
                };
            }
        }
    }
}
