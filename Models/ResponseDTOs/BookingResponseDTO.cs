using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantSystem.Models
{
    public class BookingResponseDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public TableResponseDTO Table { get; set; }
        public DiningPeriodResponseDTO DiningPeriod { get; set; }
        public UserResponseDTO User { get; set; }
        public RestaurantResponseDTO Restaurant { get; set; }

        public static explicit operator BookingResponseDTO(Booking booking)
        {
            if (booking == null)
            {
                return null;
            }
            else
            {

                return new BookingResponseDTO()
                {
                    Id = booking.Id,
                    Date = booking.Date,
                    Table = (TableResponseDTO)booking.Table,
                    DiningPeriod = (DiningPeriodResponseDTO)booking.DiningPeriod,
                    User = (UserResponseDTO)booking.User,
                    Restaurant = (RestaurantResponseDTO)booking.Restaurant
                };
            }
        }
    }
}
