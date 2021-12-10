using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class UserResponseDTO
    {
        public string SystemId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressResponseDTO Address { get; set; }

        public static explicit operator UserResponseDTO(User user)
        {
            if (user == null)
            {
                return null;
            }
            else
            {

                return new UserResponseDTO()
                {
                    SystemId = user.SystemId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = (AddressResponseDTO)user.Address
                };
            }
        }
    }
}
