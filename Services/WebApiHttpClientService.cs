using Newtonsoft.Json;
using NuGet.Common;
using UsersApi.Services;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Services
{
    public class WebApiHttpClientService
    {
        public const string ApiBaseAddress = "https://localhost:7193/";
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(ApiBaseAddress)
        };

        // Start HttpClient to comunicate with the UsersApi
        public HttpClient GetCLient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
        
        // Send a request to the Register route of the UsersApi and return the response
        public async Task<HttpResponseMessage> GetResponse(RegisterModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();
            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Register", content);

            return response;
        }

        // Send a request to the Login route of the UsersApi and return the response
        public async Task<HttpResponseMessage> GetResponse(LoginModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();
            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Login", content);

            return response;
        }

        // Send a request to the Logout route of the UsersApi and return the response
        public async Task<HttpResponseMessage> LogoutUser()
        {
            HttpContent content = new StringContent(ApiBaseAddress, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Logout", content);

            return response;
        }
    }
}
