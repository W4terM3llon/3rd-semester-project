using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
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

        public RestaurantRepository(RestaurantSystemContext context)
        {
            _context = context;
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

        public async Task<Restaurant> CreateAsync(Restaurant restaurant)
        {
            await _context.Restaurant.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> DeleteAsync(string id)
        {
            if (await IfExist(id))
            {
                var restaurant = await GetAsync(id);
                var address = restaurant.Address;
                _context.Restaurant.Remove(restaurant);
                _context.Address.Remove(address);
                await _context.SaveChangesAsync();
                return restaurant;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> IfExist(string id)
        {
            return await _context.Restaurant.AnyAsync(Restaurant => Restaurant.Id == id);
        }
    }
}
