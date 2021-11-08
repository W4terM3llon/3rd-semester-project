using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IBookingRepository
    {
        public Task<IEnumerable<Booking>> GetAllAsync();
        public Task<Booking> GetAsync(string id);
        public Task<BookingRequest> CreateAsync(BookingRequest bookingRequest);
        public Task<BookingRequest> UpdateAsync(BookingRequest bookingRequest);
        public Task<Booking> DeleteAsync(string id);
    }
}
