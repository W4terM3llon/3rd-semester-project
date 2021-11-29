using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IEnumerable<Table>> GetAllAsync(TableRequest tableQuery)
        {
            var tables = await _context.Table.Include(table => table.Restaurant).Where(table=>
            (table.Id==tableQuery.Id || tableQuery.Id == null) &&
            (table.SeatNumber == tableQuery.SeatNumber || tableQuery.SeatNumber == null) &&
            (table.Description == tableQuery.Description || tableQuery.Description == null) &&
            (table.Restaurant.Id == tableQuery.Restaurant || tableQuery.Restaurant == null)
            ).ToListAsync();
            return tables;
        }
        public async Task<Table> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var table = await _context.Table.FirstOrDefaultAsync(table => table.Id == id);
                return table;
            }
            else
            {
                return null;
            }
        }
        public async Task<Table> UpdateAsync(Table table)
        {
            if (await IfExist(table.Id))
            {
                var contextTable = await _context.Table.FirstOrDefaultAsync(table => table.Id== table.Id);

                contextTable.SeatNumber = table.SeatNumber;
                contextTable.Description = table.Description;

                await _context.SaveChangesAsync();
                return contextTable;
            }
            else
            {
                return null;
            }
        }
        public async Task<Table> CreateAsync(Table table)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == table.Restaurant.Id);

            if (restaurant == null)
            {
                return null;
            }

            await _context.Table.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<Table> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var table = await _context.Table.Include(table => table.Restaurant).FirstOrDefaultAsync(table => table.Id == id);

                _context.Table.Remove(table);
                await _context.SaveChangesAsync();
                return table;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
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

        public async Task<Table> ConvertAlterTableRequest(TableRequest request)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            if (restaurant == null)
            {
                return null;
            }

            var table = new Table()
            {
                Id = new Random().Next(1, 1000).ToString(),
                SeatNumber = request.SeatNumber,
                Description = request.Description,
                Restaurant = restaurant
            };
            return table;
        }
    }
}
