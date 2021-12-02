using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IBookingRepository
    {
        public Task<IEnumerable<Booking>> GetAllAsync(string restaurantId, DateTime date, string userId);
        public Task<Booking> GetAsync(string id);
        public Task<Booking> CreateAsync(Booking bookingRequest);
        public Task<Booking> UpdateAsync(Booking bookingRequest);
        public Task<Booking> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<Booking> ConvertAlterBookingRequest(BookingRequest request);
        public Task<bool> IfTimeAvailable(Booking bookingPretender);
    }
}
