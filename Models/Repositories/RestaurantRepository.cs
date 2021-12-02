using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RestaurantSystem.Models.Repositories
{
    class RestaurantRepository : IRestaurantRepositoty
    {
        private readonly RestaurantSystemContext _context;
        private readonly UserManager<User> _userManager;

        public RestaurantRepository(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurant.Include(restaurant => restaurant.Address).ToListAsync();

        }

        public async Task<Restaurant> GetAsync(string id)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Address).FirstOrDefaultAsync(restaurant => restaurant.Id == id);
            return restaurant;
        }

        public async Task<Restaurant> UpdateAsync(Restaurant newRestaurantData)
        {
            if (await IfExist(newRestaurantData.Id))
            {
                var contextRestaurant = await GetAsync(newRestaurantData.Id);
                contextRestaurant.Name = newRestaurantData.Name;
                contextRestaurant.IsTableBookingEnabled = newRestaurantData.IsTableBookingEnabled;
                contextRestaurant.IsDeliveryAvailable = newRestaurantData.IsDeliveryAvailable;
                contextRestaurant.Address.Street = newRestaurantData.Address.Street;
                contextRestaurant.Address.Appartment = newRestaurantData.Address.Appartment;
                await _context.SaveChangesAsync();
                return newRestaurantData;
            }
            else
            {
                return null;
            }

        }

        public async Task<Restaurant> CreateAsync(Restaurant restaurant, string everyDayUseAccountEmail)
        {
            restaurant.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator
            restaurant.Address.Id = new Random().Next(1, 1000).ToString(); //Replace by real id generator

            //create EveryDayUseAccount
            var everyDayUseAccount = new User()
            {
                FirstName= "Helper",
                LastName = "Account",
                Address = null,
                Email = everyDayUseAccountEmail,
                UserName = everyDayUseAccountEmail,
                SystemId = new Random().Next(1, 1000).ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(everyDayUseAccount, "123!@#aAA");
            if (!result.Succeeded)
            {
                return null;
            }
            await _userManager.AddToRoleAsync(everyDayUseAccount, "RestaurantEveryDayUse");

            restaurant.EveryDayUseAccount = everyDayUseAccount;

            await _context.Restaurant.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Address).Include(restaurant => restaurant.EveryDayUseAccount).FirstOrDefaultAsync(restaurant => restaurant.Id == id);
                var address = restaurant.Address;
                var everyDayUseAccount = restaurant.EveryDayUseAccount;
                _context.Restaurant.Remove(restaurant);
                _context.Address.Remove(address);
                _context.User.Remove(everyDayUseAccount);
                await _context.SaveChangesAsync();
                return restaurant;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IfExist(string id)
        {
            return await _context.Restaurant.AnyAsync(Restaurant => Restaurant.Id == id);
        }

        public async Task<Restaurant> ConvertAlterRestaurantRequest(RestaurantRequest request, string currentUserEmail)
        {
            var manager = await _context.User.FirstOrDefaultAsync(manager => manager.Email == currentUserEmail);
            var restaurant = new Restaurant()
            {
                Id = request.Id,
                Name = request.Name,
                IsTableBookingEnabled = request.IsTableBookingEnabled,
                IsDeliveryAvailable = request.IsDeliveryAvailable,
                Address = new Address 
                {
                       Street = request.Address.Street,
                       Appartment = request.Address.Appartment
                },
                Manager = manager
            };
            return restaurant;
        }
    }
}
