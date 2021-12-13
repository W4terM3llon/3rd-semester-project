using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<IEnumerable<Table>> GetAllAsync(string restaurantId)
        {
            var tables = await _context.Table.Include(table => table.Restaurant).ThenInclude(restaurant => restaurant.Address).Where(table=>
            (table.Restaurant.Id == restaurantId || restaurantId == null)
            ).ToListAsync();
            return tables;
        }
        public async Task<Table> GetAsync(string id)
        {
            if (await IfExist(id))
            {
                var table = await _context.Table.Include(table => table.Restaurant).ThenInclude(restaurant => restaurant.Address).FirstOrDefaultAsync(table => table.Id == id);
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
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var contextTable = await _context.Table.FirstOrDefaultAsync(table => table.Id == table.Id);

                        contextTable.SeatNumber = table.SeatNumber;
                        contextTable.Description = table.Description;
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return contextTable;
                    }
                    catch (Exception ex)
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
        public async Task<Table> CreateAsync(Table table)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(restaurant => restaurant.Id == table.Restaurant.Id);

            if (restaurant == null)
            {
                return null;
            }

            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    await _context.Table.AddAsync(table);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return table;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Table> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var table = await _context.Table.Include(table => table.Restaurant).FirstOrDefaultAsync(table => table.Id == id);

                        _context.Table.Remove(table);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return table;
                    }
                    catch (Exception ex)
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
            return await _context.Table.AnyAsync(table => table.Id == id);
        }

        public async Task<Table> ConvertAlterTableRequest(TableRequestDTO request, string id)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == request.Restaurant);
            if (restaurant == null)
            {
                return null;
            }

            var table = new Table()
            {
                Id = id != null ? id : new Random().Next(1, 1000).ToString(),
                SeatNumber = request.SeatNumber,
                Description = request.Description,
                Restaurant = restaurant
            };
            return table;
        }
    }
}
