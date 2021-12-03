using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Models.Requests
{
    public class UserRequest
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressRequest AccountingAddress { get; set; }
    }
}
