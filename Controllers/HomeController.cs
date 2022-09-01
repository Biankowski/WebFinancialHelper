using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet]
        public IActionResult Index()
        {
            // Display all the data that is in the database
            IEnumerable<CollectedData> collectedData = _db.CollectedData.OrderByDescending(x => x.PurchaseDate);
        
            return View(collectedData);
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

        // Method to Add a Photo through the form
        [HttpPost, ActionName("AddPhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var imagePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles", "photo.jpeg"));
            var textFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "TextFiles", "text.txt"));
            var tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
            var tessDataLanguage = "por";

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

            // Instantiate the ReadImage class and passa an instance of the Tesseract Engine to it
            var readImage = new ReadImage(new TesseractEngine(tessDataPath, tessDataLanguage, EngineMode.Default));
            readImage.ReadImageFromUser(textFilePath, imagePath);
            readImage.FilterText();
            return RedirectToAction("Details");
        }

        // Method to add data manually through the form
        [HttpPost, ActionName("AddForms")]
        [ValidateAntiForgeryToken]
        public IActionResult AddForms(CollectedData obj)
        {
            if (ModelState.IsValid)
            {
                _db.CollectedData.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            // Deserialize the json file and map it to the Model Class and display in the view
            var jsonText = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "JsonFiles", "jsonFiles.json")));
            var jsonData = JsonConvert.DeserializeObject<CollectedData>(jsonText);

            return View(jsonData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(CollectedData obj)
        {
            // Deserialize the json file again to add it to the database
            // This method recieves a CollectedData object and assign it to the place of purchase.
            var jsonText = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "JsonFiles", "jsonFiles.json")));
            var jsonData = JsonConvert.DeserializeObject<CollectedData>(jsonText);
            jsonData.PlaceOfPurchase = obj.PlaceOfPurchase;
            

            if (ModelState.IsValid)
            {
                _db.CollectedData.Add(jsonData);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var itemFromDb = _db.CollectedData.Find(id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            if (itemFromDb == null)
            {
                return NotFound();
            }
            return View(itemFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.CollectedData.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.CollectedData.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var itemFromDb = _db.CollectedData.Find(id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            if (itemFromDb == null)
            {
                return NotFound();
            }
            return View(itemFromDb);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(CollectedData obj)
        {
            if (ModelState.IsValid)
            {
                _db.CollectedData.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }

    }
}