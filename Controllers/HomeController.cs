using Microsoft.AspNetCore.Mvc;
using WebFinancialHelper.Models;
using WebFinancialHelper.Services;
using Newtonsoft.Json;
using WebFinancialHelper.Data;
using System.Collections;

namespace WebFinancialHelper.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationService _applicationService;
        private BufferedUploadLocalService _bufferedFileUpload;

        public HomeController(ApplicationService applicationService, BufferedUploadLocalService bufferedFileUpload)
        {
            _applicationService = applicationService;
            _bufferedFileUpload = bufferedFileUpload;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Display all the data that is in the database
            IEnumerable data = _applicationService.DisplayItemsFromDb();

            return View(data);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
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
            if (_applicationService.AddItemsFromForms(obj))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            // Deserialize the json file and map it to the Model Class and display in the view
            CollectedData deserializedJson = _applicationService.DeserializeJsonFile();
            return View(deserializedJson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(CollectedData obj)
        {
            CollectedData deserializedJson = _applicationService.DeserializeJsonFile();
            deserializedJson.PlaceOfPurchase = obj.PlaceOfPurchase;

            if (_applicationService.SaveDetailsToDb(obj))
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var obj = _applicationService.ShowItemsFromDbById(id);
            return View(obj);
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
            var obj = _applicationService.ShowItemsFromDbById(id);
            return View(obj);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(CollectedData obj)
        {
            if (_applicationService.EditItemsFromDb(obj))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}