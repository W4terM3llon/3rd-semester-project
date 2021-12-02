using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantSystem.Models.Requests;

namespace RestaurantSystem.Models.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllAsync(string restaurantId, DateTime date, string userId);
        public Task<Order> GetAsync(string id);
        public Task<Order> CreateAsync(Order order);
        public Task<Order> UpdateAsync(Order order);
        public Task<Order> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<Order> ConvertAlterOrderRequest(OrderRequest request);
    }
}
