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
            System.Diagnostics.Debug.WriteLine(jsonString);
            JWTToken jwtToken = JsonSerializer.Deserialize<JWTToken>(jsonString);
            return jwtToken;
        }

        public int Register(User user)
        {
            var request = new RestRequest("register", Method.POST);
            var body = new 
            { 
                email = user.Email, 
                password = user.Password, 
                phoneNumber = user.PhoneNumber, 
                firstName = user.FirstName,
                lastName = user.LastName,
                accountingAddress = new 
                {
                    street = user.address.street,
                    appartment = user.address.appartment
                }
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
            //.AddUrlSegment("date", DateTime.Now);
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            

            var result = RestClient.Execute(request);

            
            string jsonString = result.Content;

            System.Diagnostics.Debug.WriteLine(RestClient.BuildUri(request).ToString());
            System.Diagnostics.Debug.WriteLine(jsonString);
            System.Diagnostics.Debug.WriteLine(bearerToken);
            Console.Out.WriteLine("test");
            Console.WriteLine(jsonString);
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
            System.Diagnostics.Debug.WriteLine(jsonString);
            System.Diagnostics.Debug.WriteLine(RestClient.BuildUri(request).ToString());
            return true;
        }
    }
}
