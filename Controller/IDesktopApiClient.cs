using DesktopClient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Controller
{
    interface IDesktopApiClient
    {
        public JWTToken Login(string email, string password);
        public int Register(User user);
        public int AddRest(Restaurant restaurant, string jwtToken);
        public List<Restaurant> GetRestaurants(string jwtToken);
        public int CreateDish(Dish dish, string jwtToken);
        public List<DishCategory> GetDishCategories(string jwtToken);
        public List<Dish> GetDishes(string jwtToken, string restId);
        public int UpdateDish(Dish dish, string jwtToken);
        public int DeleteDish(string dishId, string jwtToken);
        public List<Table> GetTables(string restId, string jwtToken);
        public int CreateTable(Table table, string jwtToken);
        public int UpdateTable(Table table, string jwtToken);
        public int DeleteTable(string tableId, string jwtToken);
        public int DeletePeriod(string periodId, string jwtToken);
        public List<DiningPeriod> GetPeriods(string jwtToken, string restId);
        public int UpdatePeriod(DiningPeriod period, string jwtToken);
        public int CreatePeriod(DiningPeriod period, string jwtToken);
        public List<Order> GetOrders(string restaurantId, string date, string bearerToken);
        public List<OrderStage> GetOrderStages(string bearerToken);
        public bool PatchOrderStage(string orderId, string orderStageId, string bearerToken);
        public List<Booking> GetBookings(string jwtToken, string restId, string date);
    }
}
