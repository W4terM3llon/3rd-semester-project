using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Requests
{
    public class BookingRequestDTO
    {
        [Required(ErrorMessage = "Date field is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Table field is required")]
        public string Table { get; set; }
        [Required(ErrorMessage = "DiningPeriod field is required")]
        public string DiningPeriod { get; set; }
        [Required(ErrorMessage = "User field is required")]
        public string User { get; set; }
        [Required(ErrorMessage = "Restaurant field is required")]
        public string Restaurant { get; set; }
    }
}
