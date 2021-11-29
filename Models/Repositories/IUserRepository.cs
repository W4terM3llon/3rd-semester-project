using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task<Customer> GetAsync(string id);
        public Task<Customer> UpdateAsync(Register register);
        public Task<Customer> DeleteAsync(string id);
    }
}
