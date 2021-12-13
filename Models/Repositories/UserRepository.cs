using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant_system_new.Models.Requests;
using RestaurantSystem.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        User user = await _context.User.Include(user => user.Address).FirstOrDefaultAsync(user => user.SystemId == userData.SystemId);
                        user.FirstName = userData.FirstName;
                        user.LastName = userData.LastName;
                        user.Address.Street = userData.Address.Street;
                        user.Address.Appartment = userData.Address.Appartment;
                        user.PhoneNumber = userData.PhoneNumber;

                        await _userManager.UpdateAsync(user);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return user;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
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
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var user = await _context.User.Include(user => user.Address).FirstOrDefaultAsync(user => user.SystemId == id);
                        var address = user.Address;
                        await _userManager.DeleteAsync(user);
                        _context.Address.Remove(address);
                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return user;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
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

        public User ConvertAlterUserRequest(UserRequestDTO request, string id)
        {
            var user = new User()
            {
                SystemId = id != null ? id : new Random().Next(1, 1000).ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = new Address()
                {
                    Street = request.Address.Street,
                    Appartment = request.Address.Appartment
                }
            };

            return user;
        }
    }
}
