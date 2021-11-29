using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IDiningPeriodRepository
    {
        public Task<IEnumerable<DiningPeriod>> GetAllAsync();
        public Task<DiningPeriod> GetAsync(string id);
        public Task<DiningPeriod> CreateAsync(DiningPeriod diningPeriodRequest);
        public Task<DiningPeriod> UpdateAsync(DiningPeriod diningPeriodRequest);
        public Task<DiningPeriod> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
    }
}
