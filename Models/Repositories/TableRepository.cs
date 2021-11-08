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
    public class TableRepository : ITableRepository
    {

        private readonly RestaurantSystemContext _context;

        public TableRepository(RestaurantSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TableRequest>> GetAllAsync()
        {
            var tables = await _context.Table.Include(table => table.Restaurant).Include(table => table.AvailablePeriods).ToListAsync();
            var tableRequests = tables.ConvertAll(x => new TableRequest { Id = x.Id, SeatNumber = x.SeatNumber, Description = x.Description, Restaurant = x.Restaurant.Id, AvailablePeriods = x.AvailablePeriods.ConvertAll(period => period.Id) });
            return tableRequests;
        }
        public async Task<TableRequest> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var table = await _context.Table.Include(table => table.Restaurant).Include(table => table.AvailablePeriods).FirstOrDefaultAsync(table => table.Id == id);
                var tableRequest = new TableRequest { Id = table.Id, SeatNumber = table.SeatNumber, Description = table.Description, Restaurant = table.Restaurant.Id, AvailablePeriods = table.AvailablePeriods.ConvertAll(period => period.Id) };
                return tableRequest;
            }
            else
            {
                return null;
            }
        }
        public async Task<TableRequest> UpdateAsync(TableRequest tableRequest)
        {
            if (await IfExist(tableRequest.Id) && await IfDiningPeriodsExist(tableRequest.AvailablePeriods))
            {
                var contextTable = await _context.Table.Include(table => table.AvailablePeriods).FirstOrDefaultAsync(table => table.Id==tableRequest.Id);

                contextTable.SeatNumber = tableRequest.SeatNumber;
                contextTable.Description = tableRequest.Description;
                contextTable.AvailablePeriods = tableRequest.AvailablePeriods.Select(diningPeriodId => _context.DiningPeriod.FirstOrDefault(period => period.Id == diningPeriodId)).ToList();

                await _context.SaveChangesAsync();
                return tableRequest;
            }
            else
            {
                return null;
            }
        }
        public async Task<TableRequest> CreateAsync(TableRequest tableRequest)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == tableRequest.Restaurant);

            if (restaurant == null || !await IfDiningPeriodsExist(tableRequest.AvailablePeriods))
            {
                return null;
            }

            var table = new Table
            {
                Id = new Random().Next(1, 1000).ToString(), //Replace by real id generator
                SeatNumber = tableRequest.SeatNumber,
                Description = tableRequest.Description,
                Restaurant = restaurant,
                AvailablePeriods = tableRequest.AvailablePeriods.Select(diningPeriodId => _context.DiningPeriod.FirstOrDefault(period => period.Id == diningPeriodId)).ToList()
            };

            await _context.Table.AddAsync(table);
            await _context.SaveChangesAsync();
            tableRequest.Id = table.Id;
            return tableRequest;
        }

        public async Task<TableRequest> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var tableRequest = await GetAsync(id);
                var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == id);
                _context.Table.Remove(table);
                await _context.SaveChangesAsync();
                return tableRequest;
            }
            else
            {
                return null;
            }
        }
        private async Task<bool> IfExist(string id)
        {
            return await _context.Table.AnyAsync(table => table.Id == id);
        }

        private async Task<bool> IfDiningPeriodsExist(List<String> AvailablePeriods) {
            foreach (string diningPeriod in AvailablePeriods)
            {
                if (!await _context.DiningPeriod.AnyAsync(period => period.Id == diningPeriod))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
