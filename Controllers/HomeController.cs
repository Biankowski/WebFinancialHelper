using Microsoft.AspNetCore.Mvc;
using WebFinancialHelper.Models;
using WebFinancialHelper.Services;
using Newtonsoft.Json;
using WebFinancialHelper.Data;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using FluentResults;

namespace WebFinancialHelper.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationService _applicationService;
        private BufferedUploadLocalService _bufferedFileUpload;
        private WebApiHttpClientService _webApiHttpClient;

        public HomeController(ApplicationService applicationService, BufferedUploadLocalService bufferedFileUpload, WebApiHttpClientService webApiHttpClient)
        {
            _applicationService = applicationService;
            _bufferedFileUpload = bufferedFileUpload;
            _webApiHttpClient = webApiHttpClient;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                var user = JsonConvert.DeserializeObject<UserSessionModel>(HttpContext.Session.GetString("UserSession"));
                // Display all the data that is in the database
                var data = _applicationService.DisplayItemsFromDb(user);

                return View(data);
            }
            return RedirectToAction("Login", "Auth");

        }

        [HttpGet]
        public IActionResult About()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }

        // Method to Add a Photo through an Uploaded file
        [HttpPost, ActionName("AddPhoto")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            // Try to get the uploaded file and save it in the destinated folder
            try
            {
                if (await _bufferedFileUpload.UploadFile(file))
                {
                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception) { }

            _applicationService.ProcessImage();
            return RedirectToAction("Details");
        }

        // Method to add data manually through the form
        [HttpPost, ActionName("AddForms")]
        [ValidateAntiForgeryToken]
        public IActionResult AddForms(CollectedData obj)
        {
            var user = JsonConvert.DeserializeObject<UserSessionModel>(HttpContext.Session.GetString("UserSession"));
            if (_applicationService.AddItemsFromForms(obj, user))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                // Deserialize the json file and map it to the Model Class and display in the view
                CollectedData deserializedJson = _applicationService.DeserializeJsonFile();

                return View(deserializedJson);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(CollectedData obj)
        {
            CollectedData deserializedJson = _applicationService.DeserializeJsonFile();
            var user = JsonConvert.DeserializeObject<UserSessionModel>(HttpContext.Session.GetString("UserSession"));
            deserializedJson.ResponsibleUsername = user.Username;
            deserializedJson.PlaceOfPurchase = obj.PlaceOfPurchase;

            if (_applicationService.SaveDetailsToDb(obj, user))
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                var obj = _applicationService.ShowItemsFromDbById(id);
                return View(obj);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (_applicationService.DeleteItemsFromDb(id))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                var obj = _applicationService.ShowItemsFromDbById(id);
                return View(obj);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(CollectedData obj)
        {
            var user = JsonConvert.DeserializeObject<UserSessionModel>(HttpContext.Session.GetString("UserSession"));
            if (_applicationService.EditItemsFromDb(obj, user))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _webApiHttpClient.GetCLient();

            var response = await _webApiHttpClient.LogoutUser();

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login", "Auth");
            }
            return NotFound();

        }
    }
}