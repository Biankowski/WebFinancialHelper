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
            var client = WebApiHttpClientService.GetCLient();
            
            var jsonRequest = JsonConvert.SerializeObject(request).ToString();

            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                return View("Login");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            var client = WebApiHttpClientService.GetCLient();

            var jsonRequest = JsonConvert.SerializeObject(request).ToString();

            HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Username already exists";
                return View("Register");
            }
            return RedirectToAction("Login");
        }
    }
}
