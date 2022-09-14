using Newtonsoft.Json;
using System.Collections;
using Tesseract;
using WebFinancialHelper.Data;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _db;

        public ApplicationService(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get all items avaliable on the Database and display them ordered by date
        public IEnumerable<CollectedData> DisplayItemsFromDb(UserSessionModel user)
        {
            var collectedData = _db.CollectedData.Where(x => x.ResponsibleUsername == user.Username);
            return collectedData;
        }
        
        // Method responsible to Process an Image and convert it to text
        public void ProcessImage()
        {
            var imagePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles", "photo.jpeg"));
            var textFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "TextFiles", "text.txt"));
            var tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
            var tessDataLanguage = "por";

            // Instantiate the ReadImage class and pass an instance of the Tesseract Engine to it
            var readImage = new ReadImageService(new TesseractEngine(tessDataPath, tessDataLanguage, EngineMode.Default));
            readImage.ReadImageFromUser(textFilePath, imagePath);
            readImage.FilterText();
        }

        // Method responsible to add an Object to the database
        public bool AddItemsFromForms(CollectedData obj, UserSessionModel user)
        {
            obj.ResponsibleUsername = user.Username;
            if (obj != null)
            {
                _db.CollectedData.Add(obj);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        // Method responsible to deserialize the json file and map it to a Class
        public CollectedData DeserializeJsonFile()
        {
            var jsonText = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "JsonFiles", "jsonFiles.json")));
            var jsonData = JsonConvert.DeserializeObject<CollectedData>(jsonText);
            return jsonData;
        }

        // Method responsible to get the deserialized items and insert it to the database
        public bool SaveDetailsToDb(CollectedData obj, UserSessionModel user)
        {
            CollectedData deserializedData = DeserializeJsonFile();
            deserializedData.PlaceOfPurchase = obj.PlaceOfPurchase;
            deserializedData.ResponsibleUsername = user.Username;

            if (obj != null)
            {
                _db.CollectedData.Add(deserializedData);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        // Method responsible to display items from Database by Id
        public CollectedData ShowItemsFromDbById(int? id)
        {
            var itemFromDb = _db.CollectedData.Find(id);
            if (id == null || id == 0)
            {
                return null;
            }
            if (itemFromDb == null)
            {
                return null;
            }
            return itemFromDb;
        }

        // Method responsible to delete items from Database by Id
        public bool DeleteItemsFromDb(int? id)
        {
            var obj = _db.CollectedData.Find(id);
            if (obj == null)
            {
                return false;
            }
            _db.CollectedData.Remove(obj);
            _db.SaveChanges();
            return true;
        }

        // Method responsible to edit items from Database by Id
        public bool EditItemsFromDb(CollectedData obj, UserSessionModel user)
        {
            obj.ResponsibleUsername = user.Username;
            if (obj != null)
            {
                _db.CollectedData.Update(obj);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
