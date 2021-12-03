using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IRestaurantRepository
    {
        public Task<IEnumerable<Restaurant>> GetAllAsync();
        public Task<Restaurant> GetAsync(string id);
        public Task<Restaurant> CreateAsync(Restaurant user, string everyDayUseAccountEmail, string everyDayUseAccountPassword);
        public Task<Restaurant> UpdateAsync(Restaurant user);
        public Task<Restaurant> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<Restaurant> ConvertAlterRestaurantRequest(RestaurantRequest request, string currentUserEmail, string id);
    }
}
