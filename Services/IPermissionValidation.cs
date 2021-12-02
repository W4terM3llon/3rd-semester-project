using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSystem.Services
{
    public interface IPermissionValidation
    {
        public Task<bool> isManagerRestaurantOwnerAsync(string restaurantId, string currentUserEmail);
        public Task<bool> isEveryDayUseAccountRestaurantsOwnershipAsync(string restaurantId, string currentUserEmail);
        public Task<bool> isManagerTableOwnerAsync(string tableId, string currentUserEmail);
        public Task<bool> isManagerDishOwnerAsync(string dishId, string currentUserEmail);
        public Task<bool> isManagerDiningPeriodOwnerAsync(string diningPeriodId, string currentUserEmail);
        public Task<bool> isManagerBookingOwnerAsync(string bookingId, string currentUserEmail);
        public Task<bool> isCustomerBookingOwnerAsync(string bookingId, string currentUserEmail);
        public Task<bool> isManagerOrderOwnerAsync(string orderId, string currentUserEmail);
        public Task<bool> isCustomerOrderOwnerAsync(string orderId, string currentUserEmail);
        public Task<bool> isUserTheSameAsync(string userId, string currentUserEmail);
    }
}
