﻿using Restaurant_system_new.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class Register
    {
        public string SystemId { get; set; }
        [Required(ErrorMessage = "Email field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "PhoneNumber field is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "FirstName field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName field is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "AccountingAddress is required")]
        public AddressRequest AccountingAddress { get; set; }
    }
}
