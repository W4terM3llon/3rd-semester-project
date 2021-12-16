using DesktopClient.DTO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopClient.Controller
{
    class DesktopApiClient : IDesktopApiClient
    {
        public string BaseUri { get; private set; }
        
        private RestClient RestClient { get; set; }

        public DesktopApiClient(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
        }
        public JWTToken Login(string email, string password)
        {
            var request = new RestRequest("login", Method.POST);
            var body = new
            {
                email = email,
                password = password
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            JWTToken jwtToken = JsonSerializer.Deserialize<JWTToken>(jsonString);
            return jwtToken;
        }

        public int Register(User user)
        {
            var request = new RestRequest("register", Method.POST);
            var body = new
            {
                email = user.email,
                password = user.password,
                phoneNumber = user.phoneNumber,
                firstName = user.firstName,
                lastName = user.lastName,
                address = new
                {
                    street = user.address.street,
                    appartment = user.address.appartment
                }
            };
            request.AddJsonBody(body);

            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public int AddRest(Restaurant restaurant, string jwtToken)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                name = restaurant.name,
                isTableBookingEnabled = restaurant.isTableBookingEnabled,
                isDeliveryAvailable = restaurant.isDeliveryAvailable,
                address = new
                {
                    street = restaurant.address.street,
                    appartment = restaurant.address.appartment
                },
                everyDayUseAccountEmail = restaurant.everyDayUseAccountEmail,
                everyDayUseAccountPassword = restaurant.everyDayUseAccountPassword
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);

            return (int)result.StatusCode;
        }

        public List<Restaurant> GetRestaurants(string jwtToken)
        {
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<Restaurant> restaurantList = JsonSerializer.Deserialize<List<Restaurant>>(jsonString);

            return restaurantList;
        }

        public int CreateDish(Dish dish, string jwtToken)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                name = dish.name,
                price = dish.price,
                description = dish.description,
                likes = 0,
                dishCategory = dish.dishCategory.id,
                restaurant = dish.restaurant.id
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public List<DishCategory> GetDishCategories(string jwtToken)
        {
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<DishCategory> dishCategoryList = JsonSerializer.Deserialize<List<DishCategory>>(jsonString);

            return dishCategoryList;
        }

        public List<Dish> GetDishes(string jwtToken, string restId)
        {
            var request = new RestRequest(Method.GET)
                .AddParameter("restaurantId", restId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<Dish> dishList = JsonSerializer.Deserialize<List<Dish>>(jsonString);

            return dishList;
        }

        public int UpdateDish(Dish dish, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.PUT)
                .AddUrlSegment("id", dish.id);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                name = dish.name,
                price = dish.price,
                description = dish.description,
                likes = dish.likes,
                dishCategory = dish.dishCategory.id,
                restaurant = dish.restaurant.id
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public int DeleteDish(string dishId, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.DELETE)
                .AddUrlSegment("id", dishId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public List<Table> GetTables(string jwtToken, string restId) 
        {
            var request = new RestRequest(Method.GET)
                .AddParameter("restaurantId", restId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<Table> tableList = JsonSerializer.Deserialize<List<Table>>(jsonString);

            return tableList;
        }

        public int CreateTable(Table table, string jwtToken)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                seatNumber = table.seatNumber,
                description = table.description,
                restaurant = table.restaurant.id,
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }
        //broken API endpoint
        public int UpdateTable(Table table, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.PUT)
                .AddUrlSegment("id", table.id);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
             seatNumber = table.seatNumber,
             description = table.description,
             restaurant = table.restaurant.id
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            System.Diagnostics.Debug.WriteLine(RestClient.BuildUri(request).ToString());
            return (int)result.StatusCode;
        }

        public int DeleteTable(string tableId, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.DELETE)
                .AddUrlSegment("id", tableId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public int DeletePeriod(string periodId, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.DELETE)
                .AddUrlSegment("id", periodId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public List<DiningPeriod> GetPeriods(string jwtToken, string restId)
        {
            var request = new RestRequest(Method.GET)
                .AddParameter("restaurantId", restId);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<DiningPeriod> periodList = JsonSerializer.Deserialize<List<DiningPeriod>>(jsonString);

            return periodList;
        }
        //broken API endpoint
        public int UpdatePeriod(DiningPeriod period, string jwtToken)
        {
            var request = new RestRequest("/{id}", Method.PUT)
                .AddUrlSegment("id", period.id);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                name = period.name,
                timeStartMinutes = period.timeStartMinutes,
                durationMinutes = period.durationMinutes,
                restaurant = period.restaurant.id
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public int CreatePeriod(DiningPeriod period, string jwtToken)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var body = new
            {
                name = period.name,
                timeStartMinutes = period.timeStartMinutes,
                durationMinutes = period.durationMinutes,
                restaurant = period.restaurant.id,
            };
            request.AddJsonBody(body);
            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }

        public List<Order> GetOrders(string restaurantId, string date, string bearerToken)
        {

            var request = new RestRequest(Method.GET)
                                     .AddParameter("restaurantId", restaurantId)
                                     .AddParameter("date", date);
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<Order> orders = JsonSerializer.Deserialize<List<Order>>(jsonString);
            return orders;
        }

        public List<OrderStage> GetOrderStages(string bearerToken)
        {
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<OrderStage> orderStages = JsonSerializer.Deserialize<List<OrderStage>>(jsonString);
            return orderStages;
        }

        public bool PatchOrderStage(string orderId, string orderStageId, string bearerToken)
        {
            var path = orderId + "?orderStageId=" + orderStageId;
            var request = new RestRequest(path, Method.PATCH);
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            return true;
        }

        public List<Booking> GetBookings(string jwtToken, string restId, string date)
        {

            var request = new RestRequest(Method.GET)
                                     .AddParameter("restaurantId", restId)
                                     .AddParameter("date", date);
            request.AddHeader("Authorization", "Bearer " + jwtToken);
            var result = RestClient.Execute(request);
            string jsonString = result.Content;
            List<Booking> orders = JsonSerializer.Deserialize<List<Booking>>(jsonString);
            System.Diagnostics.Debug.WriteLine(jsonString);
            return orders;
        }
    }
}
