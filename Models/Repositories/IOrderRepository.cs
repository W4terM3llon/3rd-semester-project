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
        public Task<Order> PatchAsync(string orderId, string orderStageId);
        public Task<Order> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public bool ifOrderlinesUnique(List<OrderLineRequestDTO> orderLines);
        public bool ifOrderlinesQuantityQuantityCorrect(List<OrderLineRequestDTO> orderLines);
        public Task<Order> ConvertAlterOrderRequest(OrderRequestDTO request, string id);
        
    }
}
