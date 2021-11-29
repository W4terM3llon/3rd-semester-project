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
    public class PermissionValidation
    {
        private readonly RestaurantSystemContext _context;
        private readonly UserManager<User> _userManager;

        public PermissionValidation(RestaurantSystemContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> isManagerRestaurantOwnerAsync(string restaurantId, string currentUserEmail)
        {
            var restaurant = await _context.Restaurant.Include(restaurant => restaurant.Manager).FirstOrDefaultAsync(restaurant => restaurant.Id == restaurantId);

            var currentUser = await _userManager.FindByEmailAsync(currentUserEmail);
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

        public async Task<bool> isManagerTableOwnerAsync(string tableId, string currentUserEmail)
        {
            var table = await _context.Table.Include(table => table.Restaurant).FirstOrDefaultAsync(table => table.Id == tableId);
            return await isManagerRestaurantOwnerAsync(table.Restaurant.Id, currentUserEmail);
        }

        public async Task<bool> isManagerDishOwnerAsync(string dishId, string currentUserEmail)
        {
            var dish = await _context.Dish.Include(dish => dish.Restaurant).FirstOrDefaultAsync(dish => dish.Id == dishId);
            return await isManagerRestaurantOwnerAsync(dish.Restaurant.Id, currentUserEmail);
        }
        /*
        public async Task<bool> isManagerDishCategoryOwnerAsync(string dishCategoryId, string currentUserEmail)
        {
            var dishCategory = await _context.DishCategory.Include(dishCategory => dishCategory.Restaurant).FirstOrDefaultAsync(dishCategory => dishCategory.Id == dishCategoryId);
            return await isManagerRestaurantOwnerAsync(dishCategory.Restaurant.Id, currentUserEmail);
        }*/

        public async Task<bool> isManagerDiningPeriodOwnerAsync(string diningPeriodId, string currentUserEmail)
        {
            var diningPeriod = await _context.DiningPeriod.Include(diningPeriod => diningPeriod.Restaurant).FirstOrDefaultAsync(diningPeriod => diningPeriod.Id == diningPeriodId);
            return await isManagerRestaurantOwnerAsync(diningPeriod.Restaurant.Id, currentUserEmail);
        }

        public async Task<bool> isManagerBookingOwnerAsync(string bookingId, string currentUserEmail)
        {
            var booking = await _context.Booking.Include(booking => booking.Restaurant).FirstOrDefaultAsync(booking => booking.Id == bookingId);
            return await isManagerRestaurantOwnerAsync(booking.Restaurant.Id, currentUserEmail);
        }

        public async Task<bool> isCustomerBookingOwnerAsync(string bookingId, string currentUserEmail)
        {
            var booking = await _context.Booking.Include(booking => booking.User).FirstOrDefaultAsync(booking => booking.Id == bookingId);
            return booking.User.Email == currentUserEmail;
        }
        public async Task<bool> isManagerOrderOwnerAsync(string orderId, string currentUserEmail)
        {
            var order = await _context.Order.Include(order => order.Customer).FirstOrDefaultAsync(order => order.Id == orderId);
            return await isManagerRestaurantOwnerAsync(order.Restaurant.Id, currentUserEmail);
        }
        public async Task<bool> isCustomerOrderOwnerAsync(string orderId, string currentUserEmail)
        {
            var order = await _context.Order.Include(order => order.Customer).FirstOrDefaultAsync(order => order.Id == orderId);
            return order.Customer.Email == currentUserEmail;
        }

        public async Task<bool> isUserTheSameAsync(string userId, string currentUserEmail)
        {
            return await _context.Customer.AnyAsync(user=> user.Email == currentUserEmail && user.SystemId == userId);
        }
    }
}
