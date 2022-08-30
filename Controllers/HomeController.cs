using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFinancialHelper.Helpers;
using WebFinancialHelper.Interfaces;
using WebFinancialHelper.Models;
using WebFinancialHelper.Services;
using Tesseract;
using Newtonsoft.Json;
using WebFinancialHelper.Data;

namespace WebFinancialHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBufferedFileUpload _bufferedFileUpload;
        private readonly ApplicationDbContext _db;


        public HomeController(IBufferedFileUpload bufferedFileUpload, ApplicationDbContext db)
        {
            _bufferedFileUpload = bufferedFileUpload;
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<CollectedData> collectedData = _db.CollectedData;
            return View(collectedData);
        }
        //GET
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile file)
        {
            var textFilePath = @"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\test.txt";
            var imagePath = @"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\UploadedFiles\cielo.jpeg";
            var tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
            var tessDataLanguage = "por";

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
            var readImage = new ReadImage(new TesseractEngine(tessDataPath, tessDataLanguage, EngineMode.Default));
            readImage.ReadImageFromUser(textFilePath, imagePath);
            readImage.FilterText();
            return RedirectToAction("Details");
        }

        public IActionResult Details()
        {
            var jsonText = System.IO.File.ReadAllText(@"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\jsonList.json");
            var jsonData = JsonConvert.DeserializeObject<CollectedData>(jsonText);
            return View(jsonData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(CollectedData obj)
        {
            var jsonText = System.IO.File.ReadAllText(@"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\jsonList.json");
            var jsonData = JsonConvert.DeserializeObject<CollectedData>(jsonText);
            if (ModelState.IsValid)
            {
                _db.CollectedData.Add(jsonData);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            var itemsFromDb = _db.CollectedData.Find(id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            

            if(itemsFromDb == null)
            {
                return NotFound();
            }
            return View(itemsFromDb);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.CollectedData.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _db.CollectedData.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}