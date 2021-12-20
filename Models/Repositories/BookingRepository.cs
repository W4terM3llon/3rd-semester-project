using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using RestaurantSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        public async Task<IEnumerable<Booking>> GetAllAsync(string restaurantId, DateTime date, string userId)
        {
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).ThenInclude(user => user.Address).Include(booking => booking.DiningPeriod).Include(booking => booking.Restaurant).ThenInclude(restaurant => restaurant.Address).Where(booking =>
            (booking.Restaurant.Id == restaurantId || restaurantId == null) &&
            (booking.User.SystemId == userId || userId == null) &&
            ((booking.Date.Year == date.Year && booking.Date.Month == date.Month && booking.Date.Day == date.Day) || date == DateTime.MinValue)
            ).ToListAsync();

        }

        public async Task<Booking> GetAsync(string id)
        {
            return await _context.Booking.Include(booking => booking.Table).Include(booking => booking.User).ThenInclude(user => user.Address)
                .Include(booking => booking.DiningPeriod).Include(booking => booking.Restaurant).ThenInclude(restaurant => restaurant.Address)
                .FirstOrDefaultAsync(booking => booking.Id == id);
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    var date = new DateTime(booking.Date.Year, booking.Date.Month, booking.Date.Day);

                    var contextBooking = await GetAsync(booking.Id);
                    contextBooking.Date = date;
                    contextBooking.DiningPeriod = booking.DiningPeriod;
                    contextBooking.Table = booking.Table;

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return booking;
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    await _context.Booking.AddAsync(booking);
                    int sth = await _context.SaveChangesAsync();
                    transaction.Commit();

                    return booking;
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Booking> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        var booking = await GetAsync(id);
                        _context.Booking.Remove(booking);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return booking;
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
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

        public bool IfBookingDateCorrect(DateTime dateTime, int diningPeriodStartMinutes)
        {
            DateTime currentDate = DateTime.Now;
            if (dateTime.Date.CompareTo(currentDate.Date) >= 0 && diningPeriodStartMinutes > currentDate.Date.Hour * 60 + currentDate.Date.Minute)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public async Task<Booking> ConvertAlterBookingRequest(BookingRequestDTO request, string id)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == request.Table);
            var user = await _context.User.FirstOrDefaultAsync(user => user.SystemId == request.User);
            var diningPeriod = await _context.DiningPeriod.FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == request.DiningPeriod);
            var date = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day);

            if (restaurant == null || table == null || user == null || diningPeriod == null)
            {
                return null;
            }

            var booking = new Booking()
            {
                Id = id != null ? id : IdGenerator.GenerateId(),
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
