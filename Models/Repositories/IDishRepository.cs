using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IDishRepository
    {
        public Task<IEnumerable<Dish>> GetAllAsync(string restaurantId);
        public Task<Dish> GetAsync(string id);
        public Task<Dish> CreateAsync(Dish dish);
        public Task<Dish> UpdateAsync(Dish dish);
        public Task<Dish> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<Dish> ConvertAlterDishRequest(DishRequest request, string id);
    }
}
