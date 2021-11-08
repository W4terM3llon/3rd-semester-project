using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface ITableRepository
    {
        public Task<IEnumerable<TableRequest>> GetAllAsync();
        public Task<TableRequest> GetAsync(string id);
        public Task<TableRequest> CreateAsync(TableRequest table);
        public Task<TableRequest> UpdateAsync(TableRequest table);
        public Task<TableRequest> DeleteAsync(string id);
    }
}
