using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using RestaurantSystem.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class TableFreePeriodsRepository : ITableFreePeriodsRepository
    {
        private readonly RestaurantSystemContext _context;

        public TableFreePeriodsRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync(string restaurantId, DateTime date)
        {
            //checking free periods for table and date
            var bookings = await _context.Booking.Include(booking => booking.Restaurant).Include(booking => booking.Table).Where(booking =>
                (booking.Restaurant.Id == restaurantId) && 
                (booking.Date.Year == date.Year && booking.Date.Month == date.Month && booking.Date.Day == date.Day)
                ).ToListAsync();

            var tables = await _context.Table.Include(table => table.Restaurant).Where(table => table.Restaurant.Id == restaurantId).ToListAsync();
            var diningPeriods = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).Where(diningPeriod => diningPeriod.Restaurant.Id == restaurantId).ToListAsync();

            Dictionary<string, List<DiningPeriod>> tableBookedPeriods = new Dictionary<string, List<DiningPeriod>>();
            foreach (var booking in bookings)
            {
                if (tableBookedPeriods.ContainsKey(booking.Table.Id))
                {
                    tableBookedPeriods[booking.Table.Id].Add(booking.DiningPeriod);
                }
                else 
                {
                    tableBookedPeriods.Add(booking.Table.Id, new List<DiningPeriod>() { booking.DiningPeriod });
                }
            }

            Dictionary<string, List<DiningPeriod>> tableFreePeriods = new Dictionary<string, List<DiningPeriod>>();
            foreach (var table in tables) {
                var freeDiningPeriods = new List<DiningPeriod>(diningPeriods);
                if (tableBookedPeriods.ContainsKey(table.Id))
                {
                    foreach (var period in tableBookedPeriods[table.Id])
                    {
                        freeDiningPeriods.Remove(period);
                    }
                }
                tableFreePeriods.Add(table.Id, freeDiningPeriods);
            }

            return tableFreePeriods;
        }
    }
}
