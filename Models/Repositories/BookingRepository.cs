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
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).Include(booking => booking.DiningPeriod).ToListAsync();

        }

        public async Task<Booking> GetAsync(string id)
        {
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).Include(booking => booking.DiningPeriod).FirstOrDefaultAsync(booking => booking.Id == id);
        }

        public async Task<BookingRequest> UpdateAsync(BookingRequest bookingRequest)
        {
            if (await IfExist(bookingRequest.Id) && await IfRequestDataCorrect(bookingRequest) && !await IfBookingOverlap(bookingRequest))
            {
                var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == bookingRequest.Table);
                var user = await _context.User.FirstOrDefaultAsync(user => user.SystemId == bookingRequest.User);
                var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == bookingRequest.DiningPeriod);
                var day = new DateTime(bookingRequest.Day.Year, bookingRequest.Day.Month, bookingRequest.Day.Day);

                var contextBooking = await GetAsync(bookingRequest.Id);

                contextBooking.Table = table;
                contextBooking.User = user;
                contextBooking.DiningPeriod = diningPeriod;
                contextBooking.Day= day;

                await _context.SaveChangesAsync();
                return bookingRequest;
            }
            else
            {
                return null;
            }

        }

        public async Task<BookingRequest> CreateAsync(BookingRequest bookingRequest)
        {
            if (await IfRequestDataCorrect(bookingRequest) && !await IfBookingOverlap(bookingRequest))
            {
                var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == bookingRequest.Table);
                var user = await _context.User.FirstOrDefaultAsync(user => user.SystemId == bookingRequest.User);
                var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == bookingRequest.DiningPeriod);
                var day = new DateTime(bookingRequest.Day.Year, bookingRequest.Day.Month, bookingRequest.Day.Day);
                var booking = new Booking
                {
                    Id = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                    Table = table,
                    User = user,
                    DiningPeriod = diningPeriod,
                    Day = day
                };

                await _context.Booking.AddAsync(booking);
                await _context.SaveChangesAsync();
                return bookingRequest;
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
                booking.Table.AvailablePeriods.Add(booking.DiningPeriod);
                await _context.SaveChangesAsync();
                return booking;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> IfExist(string id)
        {
            return await _context.Booking.AnyAsync(booking => booking.Id == id);
        }

        private async Task<bool> IfRequestDataCorrect(BookingRequest bookingRequest) 
        {
            var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(period => period.Id == bookingRequest.DiningPeriod);
            if (diningPeriod == null ||
                !await _context.Table.AnyAsync(table => table.Id == bookingRequest.Table && table.AvailablePeriods.Contains(diningPeriod)) ||
                !await _context.User.AnyAsync(user => user.SystemId == bookingRequest.User))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IfBookingOverlap(BookingRequest bookingRequest)
        {
            var day = new DateTime(bookingRequest.Day.Year, bookingRequest.Day.Month, bookingRequest.Day.Day);
            if (await _context.Booking.AnyAsync(booking => (booking.Table.Id == bookingRequest.Table && booking.DiningPeriod.Id == bookingRequest.DiningPeriod && booking.Day.Equals(day))))
            {
                return true;
            }
            return false;
        }
    }
}
