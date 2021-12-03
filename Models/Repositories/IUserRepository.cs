using Restaurant_system_new.Models.Requests;
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
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteAsync(string id);
        public Task<bool> IfExist(string id);
        public Task<User> ConvertAlterUserRequest(UserRequest request, string id);
    }
}
