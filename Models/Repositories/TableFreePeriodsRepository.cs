using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_system_new.Models.Repositories
{
    public class TableFreePeriodsRepository : ITableFreePeriodsRepository
    {
        private readonly RestaurantSystemContext _context;

        public TableFreePeriodsRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync(string restaurant, DateTime date)
        {
            var bookings = await _context.Booking.Include(booking => booking.Restaurant).Include(booking => booking.Table).Where(booking =>
                (booking.Restaurant.Id == restaurant) && 
                (booking.Date.Year == date.Year && booking.Date.Month == date.Month && booking.Date.Day == date.Day)
                ).ToListAsync();

            var tables = await _context.Table.Include(table => table.Restaurant).Where(table => table.Restaurant.Id == restaurant).ToListAsync();
            var diningPeriods = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).Where(diningPeriod => diningPeriod.Restaurant.Id == restaurant).ToListAsync();

            Dictionary<string, DiningPeriod> tableBookedPeriods = new Dictionary<string, DiningPeriod>();
            foreach (var booking in bookings)
            {
                tableBookedPeriods.Add(booking.Table.Id, booking.DiningPeriod);
            }

            Dictionary<string, List<DiningPeriod>> tableFreePeriods = new Dictionary<string, List<DiningPeriod>>();
            foreach (var table in tables) {
                var freeDiningPeriods = diningPeriods;
                if (tableBookedPeriods.ContainsKey(table.Id))
                {
                    freeDiningPeriods.Remove(tableBookedPeriods[table.Id]);
                }
                tableFreePeriods.Add(table.Id, freeDiningPeriods);
            }

            return tableFreePeriods;
        }
    }
}
