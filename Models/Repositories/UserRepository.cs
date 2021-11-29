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

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customer.Include(user => user.AccountingAddress).ToListAsync();
            
        }

        public async Task<Customer> GetAsync(string id)
        {
            var user = await _context.Customer.Include(user => user.AccountingAddress).FirstOrDefaultAsync(user => user.SystemId == id);
            return user;
        }

        public async Task<Customer> UpdateAsync(Register register)
        {
            if (await IfExist(register.SystemId))
            {
                Customer user = await GetAsync(register.SystemId);
                user.Email = register.Email;
                user.UserName = register.Email;
                user.FirstName = register.FirstName;
                user.LastName = register.LastName;
                user.AccountingAddress.Street = register.AccountingAddress.Street;
                user.AccountingAddress.Appartment = register.AccountingAddress.Appartment;
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

        public async Task<Customer> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var user = await GetAsync(id);
                var address = user.AccountingAddress;
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
            return await _context.Customer.AnyAsync(user => user.SystemId == id);
        }
    }
}
