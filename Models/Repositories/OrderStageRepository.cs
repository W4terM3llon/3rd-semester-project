using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class OrderStageRepository : IOrderStageRepository
    {

        private readonly RestaurantSystemContext _context;

        public OrderStageRepository(RestaurantSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderStage>> GetAllAsync()
        {
            return await _context.OrderStage.ToListAsync();
        }

        public async Task<OrderStage> GetAsync(string id)
        {
            var orderStage = await _context.OrderStage.FirstOrDefaultAsync(orderStage => orderStage.Id == id);
            return orderStage;
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.OrderStage.AnyAsync(e => e.Id == id);
        }
    }
}
