using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Services
{
    public class PermissionValidation : IPermissionValidation
    {
        private readonly RestaurantSystemContext _context;
        private readonly UserManager<User> _userManager;

        public PermissionValidation(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> isManagerRestaurantOwnerAsync(string restaurantId, string currentUserSystemId)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == restaurantId);
            if (restaurant == null) 
            {
                return false;
            }

            var currentUser = await _userManager.Users.FirstOrDefaultAsync(user => user.SystemId == currentUserSystemId);
            var manager = restaurant.Manager;
            if (manager.SystemId != currentUser.SystemId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> isEveryDayUseAccountRestaurantsOwnershipAsync(string restaurantId, string currentUserSystemId)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.EveryDayUseAccount).FirstOrDefaultAsync(restaurant => restaurant.Id == restaurantId);
            if (restaurant == null)
            {
                return false;
            }

            var currentUser = await _userManager.Users.FirstOrDefaultAsync(user => user.SystemId == currentUserSystemId);
            var everyDayUseAccount = restaurant.EveryDayUseAccount;
            if (everyDayUseAccount.SystemId != currentUser.SystemId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> isManagerTableOwnerAsync(string tableId, string currentUserSystemId)
        {
            var table = await _context.Table.Include(table => table.Restaurant).FirstOrDefaultAsync(table => table.Id == tableId);
            return await isManagerRestaurantOwnerAsync(table.Restaurant.Id, currentUserSystemId);
        }

        public async Task<bool> isManagerDishOwnerAsync(string dishId, string currentUserSystemId)
        {
            var dish = await _context.Dish.Include(dish => dish.Restaurant).FirstOrDefaultAsync(dish => dish.Id == dishId);
            return await isManagerRestaurantOwnerAsync(dish.Restaurant.Id, currentUserSystemId);
        }

        public async Task<bool> isManagerDiningPeriodOwnerAsync(string diningPeriodId, string currentUserSystemId)
        {
            var diningPeriod = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == diningPeriodId);
            return await isManagerRestaurantOwnerAsync(diningPeriod.Restaurant.Id, currentUserSystemId);
        }

        public async Task<bool> isManagerBookingOwnerAsync(string bookingId, string currentUserSystemId)
        {
            var booking = await _context.Booking.Include(booking => booking.Restaurant).FirstOrDefaultAsync(booking => booking.Id == bookingId);
            return await isManagerRestaurantOwnerAsync(booking.Restaurant.Id, currentUserSystemId);
        }

        public async Task<bool> isCustomerBookingOwnerAsync(string bookingId, string currentUserSystemId)
        {
            var booking = await _context.Booking.Include(booking => booking.User).FirstOrDefaultAsync(booking => booking.Id == bookingId);
            return booking.User.SystemId == currentUserSystemId;
        }
        public async Task<bool> isManagerOrderOwnerAsync(string orderId, string currentUserSystemId)
        {
            var order = await _context.Order.Include(order => order.Customer).FirstOrDefaultAsync(order => order.Id == orderId);
            return await isManagerRestaurantOwnerAsync(order.Restaurant.Id, currentUserSystemId);
        }
        public async Task<bool> isCustomerOrderOwnerAsync(string orderId, string currentUserSystemId)
        {
            var order = await _context.Order.Include(order => order.Customer).FirstOrDefaultAsync(order => order.Id == orderId);
            return order.Customer.SystemId == currentUserSystemId;
        }

        public async Task<bool> isUserTheSameAsync(string userId, string currentUserEmail)
        {
            return await _context.User.AnyAsync(user=> user.Email == currentUserEmail && user.SystemId == userId);
        }

        public async Task<bool> isDishRestaurantOwnershipAsync(string dishId, string restaurantId)
        {
            return await _context.Dish.Include(dish => dish.Restaurant).AnyAsync(dish => dish.Restaurant.Id == restaurantId && dish.Id == dishId);
        }

        public async Task<bool> isTableRestaurantOwnershipAsync(string tableId, string restaurantId)
        {
            return await _context.Table.Include(table => table.Restaurant).AnyAsync(table => table.Restaurant.Id == restaurantId && table.Id == tableId) ;
        }

        public async Task<bool> isDiningPeriodRestaurantOwnershipAsync(string diningPeriodId, string restaurantId)
        {
            return await _context.DiningPeriod.Include(dp => dp.Restaurant).AnyAsync(dp => dp.Restaurant.Id == restaurantId && dp.Id == diningPeriodId);
        }
    }
}
