using Newtonsoft.Json;
using NuGet.Common;
using UsersApi.Services;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Services
{
    public class WebApiHttpClientService
    {
        public const string ApiBaseAddress = "https://localhost:7193/";

        public HttpClient GetCLient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<HttpResponseMessage> GetResponse(RegisterModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();
            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Register", content);

            return response;
        }

        public async Task<HttpResponseMessage> GetResponse(LoginModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();
            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Login", content);

            return response;
        }

        public async Task<HttpResponseMessage> LogoutUser()
        {
            HttpContent content = new StringContent(ApiBaseAddress, System.Text.Encoding.UTF8, "application/json");
            var response = await GetCLient().PostAsync("/Logout", content);

            return response;
        }
    }
}
