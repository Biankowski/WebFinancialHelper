using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFinancialHelper.Helpers;
using WebFinancialHelper.Interfaces;
using WebFinancialHelper.Models;
using WebFinancialHelper.Services;
using Tesseract;

namespace WebFinancialHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBufferedFileUpload _bufferedFileUpload;


        public HomeController(IBufferedFileUpload bufferedFileUpload)
        {
            _bufferedFileUpload = bufferedFileUpload;
        }
        public IActionResult Index()
        {
            return View();
        }
        //GET
        public IActionResult Add()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile file)
        {
            
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
            catch (Exception)
            {
                ViewBag.Message = "File Upload Failed";
            }

            var textFilePath = @"C:\Users\Biankovsky\Desktop\Projetos C#\OCRFinancialHelper\WebFinancialHelper\test.txt";
            var imagePath = @"C:\Users\Biankovsky\Desktop\Projetos C#\OCRFinancialHelper\WebFinancialHelper\UploadedFiles\kims.jpeg";
            var tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
            var tessDataLanguage = "por";

            var readImage = new ReadImage(textFilePath, imagePath, new TesseractEngine(tessDataPath, tessDataLanguage, EngineMode.Default));
            readImage.ReadImageFromUser();
            return View();
        }
    }
}