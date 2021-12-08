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
                email = user.Email, 
                password = user.Password, 
                phoneNumber = user.PhoneNumber, 
                firstName = user.FirstName,
                lastName = user.LastName,
                address = new 
                {
                    street = user.Address.Street,
                    appartment = user.Address.Appartment
                }
            };
            request.AddJsonBody(body);

            var result = RestClient.Execute(request);
            return (int)result.StatusCode;
        }
    }
}
