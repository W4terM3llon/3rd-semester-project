using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User> GetAsync(string id);
        public Task<User> UpdateAsync(Register register);
        public Task<User> DeleteAsync(string id);
    }
}
