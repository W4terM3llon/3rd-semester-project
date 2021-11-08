using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    interface IDiningPeriodRepository
    {
        public Task<IEnumerable<DiningPeriodRequest>> GetAllAsync();
        public Task<DiningPeriodRequest> GetAsync(string id);
        public Task<DiningPeriodRequest> CreateAsync(DiningPeriodRequest diningPeriodRequest);
        public Task<DiningPeriodRequest> UpdateAsync(DiningPeriodRequest diningPeriodRequest);
        public Task<DiningPeriodRequest> DeleteAsync(string id);
    }
}
