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
        private readonly WebApiHttpClientService _webApiHttpClient;
        public AuthController(WebApiHttpClientService webApiHttpClient)
        {
            _webApiHttpClient = webApiHttpClient;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginModel request)
        {
            _webApiHttpClient.GetCLient();

            var response = await _webApiHttpClient.GetResponse(request);

            if (!response.IsSuccessStatusCode)
            {
                return View("Login");
            }
            var userSession = new UserSessionModel() {Id = 1, Username = request.Username };
            HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(userSession));
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
            _webApiHttpClient.GetCLient();

            var response = await _webApiHttpClient.GetResponse(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Username already exists";
                return View("Register");
            }
            return RedirectToAction("Login");
        }
    }
}
