using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> UpdateAsync(Register register)
        {
            if (await IfExist(register.SystemId))
            {
                User user = await GetAsync(register.SystemId);
                user.Email = register.Email;
                user.UserName = register.Email;
                user.FirstName = register.FirstName;
                user.LastName = register.LastName;
                user.Address.Street = register.AccountingAddress.Street;
                user.Address.Appartment = register.AccountingAddress.Appartment;
                user.PhoneNumber = register.PhoneNumber;

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

        private async Task<bool> IfExist(string id) 
        {
            return await _context.User.AnyAsync(user => user.SystemId == id);
        }
    }
}
