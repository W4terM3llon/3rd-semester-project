using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_system_new.Models.Requests;
using RestaurantSystem.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantSystemContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.User.Include(user => user.Address).ToListAsync();
            
        }

        public async Task<User> GetAsync(string id)
        {
            var user = await _context.User.Include(user => user.Address).FirstOrDefaultAsync(user => user.SystemId == id);
            return user;
        }

        public async Task<User> UpdateAsync(User userData)
        {
            if (await IfExist(userData.SystemId))
            {
                User user = await GetAsync(userData.SystemId);
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.Address.Street = userData.Address.Street;
                user.Address.Appartment = userData.Address.Appartment;
                user.PhoneNumber = userData.PhoneNumber;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            else 
            {
                return null;
            }
        }

        public async Task<User> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var user = await GetAsync(id);
                var address = user.Address;
                await _userManager.DeleteAsync(user);
                _context.Address.Remove(address);
                await _context.SaveChangesAsync();
                return user;
            }
            else 
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.User.AnyAsync(user => user.SystemId == id);
        }

        public async Task<User> ConvertAlterUserRequest(UserRequest request, string id)
        {
            var user = new User()
            {
                SystemId = id != null ? id : new Random().Next(1, 1000).ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = new Address()
                {
                    Street = request.AccountingAddress.Street,
                    Appartment = request.AccountingAddress.Appartment
                }
            };

            return user;
        }
    }
}
