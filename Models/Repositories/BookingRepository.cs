using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    class BookingRepository : IBookingRepository
    {
        private readonly RestaurantSystemContext _context;

        public BookingRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).Include(booking => booking.DiningPeriod).Include(booking => booking.Table).ToListAsync();

        }

        public async Task<Booking> GetAsync(string id)
        {
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).Include(booking => booking.DiningPeriod).Include(booking => booking.Table).FirstOrDefaultAsync(booking => booking.Id == id);
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            if (await IfExist(booking.Id) && await IfTimeAvailable(booking))
            {
                var date = new DateTime(booking.Date.Year, booking.Date.Month, booking.Date.Day);

                var contextBooking = await GetAsync(booking.Id);
                contextBooking.Date = date;
                contextBooking.DiningPeriod = booking.DiningPeriod;
                contextBooking.Table = booking.Table;

                await _context.SaveChangesAsync();
                return booking;
            }
            else
            {
                return null;
            }

        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            if (await IfTimeAvailable(booking))
            {
                await _context.Booking.AddAsync(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            else
            {
                return null;
            }
        }

        public async Task<Booking> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var booking = await GetAsync(id);
                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.Booking.AnyAsync(booking => booking.Id == id);
        }

        private async Task<bool> IfTimeAvailable(Booking bookingPretender)
        {
            if (await _context.Booking.AnyAsync(booking => booking.Date == bookingPretender.Date && booking.Table == bookingPretender.Table && booking.DiningPeriod == bookingPretender.DiningPeriod))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<Booking> ConvertAlterBookingRequest(BookingRequest request)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == request.Table);
            var user = await _context.Customer.FirstOrDefaultAsync(user => user.SystemId == request.User);
            var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == request.DiningPeriod);
            var date = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day);

            if (restaurant == null || table == null || user == null || diningPeriod == null)
            {
                return null;
            }

            var booking = new Booking()
            {
                Id = new Random().Next(1, 1000).ToString(),
                Table = table,
                User =  user,
                DiningPeriod = diningPeriod,
                Date = date,
                Restaurant = restaurant
            };
            return booking;
        }
    }
}
