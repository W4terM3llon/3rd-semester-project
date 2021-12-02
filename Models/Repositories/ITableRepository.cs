using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface ITableRepository
    {
        public Task<IEnumerable<Table>> GetAllAsync(string restaurantId);
        public Task<Table> GetAsync(string id);
        public Task<Table> CreateAsync(Table table);
        public Task<Table> UpdateAsync(Table table);
        public Task<Table> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
    }
}
