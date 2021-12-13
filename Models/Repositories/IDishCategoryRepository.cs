using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IDishCategoryRepository
    {
        public Task<IEnumerable<DishCategory>> GetAllAsync();
        public Task<DishCategory> GetAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<DishCategory> ConvertAlterDishRequest(DishCategoryRequestDTO request);
    }
}
