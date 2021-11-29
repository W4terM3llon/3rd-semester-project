using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IDiscountRepository
    {
        public Task<IEnumerable<Discount>> GetAllAsync();
        public Task<Discount> GetAsync(string id);
       // public Task<Discount> CreateAsync();
       // public Task<Discount> UpdateAsync();
        public Task<Discount> DeleteAsync(string id);
    }
}
