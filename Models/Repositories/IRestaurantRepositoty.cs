using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IRestaurantRepositoty
    {
        public Task<IEnumerable<Restaurant>> GetAllAsync();
        public Task<Restaurant> GetAsync(string id);
        public Task<Restaurant> CreateAsync(Restaurant user, string everyDayUseAccountEmail);
        public Task<Restaurant> UpdateAsync(Restaurant user);
        public Task<Restaurant> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
    }
}
