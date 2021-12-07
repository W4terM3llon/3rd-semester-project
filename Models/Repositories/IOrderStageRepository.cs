using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IOrderStageRepository
    {
        public Task<OrderStage> GetAsync(string id);
        public Task<IEnumerable<OrderStage>> GetAllAsync();
        public Task<bool> IfExist(string id);

    }
}
