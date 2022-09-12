using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using UsersApi.Data.Requests;
using UsersApi.Models;
using UsersApi.Services;
using WebFinancialHelper.Models;
using WebFinancialHelper.Services;

namespace WebFinancialHelper.Controllers
{
    public class AuthController : Controller
    {
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginModel request)
        {
            //string username = request.Username;
            //string password = request.Password;

            var client = WebApiHttpClientService.GetCLient();
            
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();

            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Login", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }

    }
}
