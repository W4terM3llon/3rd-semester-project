using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IDiningPeriodRepository
    {
        public Task<IEnumerable<DiningPeriod>> GetAllAsync(string restaurantId);
        public Task<DiningPeriod> GetAsync(string id);
        public Task<DiningPeriod> CreateAsync(DiningPeriod diningPeriodRequest);
        public Task<DiningPeriod> UpdateAsync(DiningPeriod diningPeriodRequest);
        public Task<DiningPeriod> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<bool> IfPeriodsOverlap(DiningPeriod diningPeriod);
        public bool IfDiningTimeCorrent(DiningPeriod diningPeriod);
        public Task<DiningPeriod> ConvertAlterDiningPeriodRequest(DiningPeriodRequestDTO request, string id);
    }
}
