using NuGet.Common;
using UsersApi.Services;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Services
{
    public static class WebApiHttpClientService
    {
        public const string ApiBaseAddress = "https://localhost:7193/";

        public static HttpClient GetCLient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

    }
}
