using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Models.Requests
{
    public class AddressRequest
    {
        [Required(ErrorMessage = "Street field is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Appartment field is required")]
        public string Appartment { get; set; }
    }
}
