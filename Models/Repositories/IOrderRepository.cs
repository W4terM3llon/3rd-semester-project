using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantSystem.Models.Requests;

namespace RestaurantSystem.Models.Repositories
{
    interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllAsync();
        public Task<Order> GetAsync(string id);
        public Task<Order> CreateAsync(Order order);
        public Task<Order> UpdateAsync(Order order);
        public Task<Order> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<Order> ConvertAlterOrderRequest(OrderRequest request);
    }
}
