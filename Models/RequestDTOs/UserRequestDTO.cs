using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Models.Requests
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "PhoneNumber field is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "FirstName field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName field is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address field is required")]
        public AddressRequestDTO Address { get; set; }
    }
}
