using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Tesseract;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Helpers
{
    public class ReadImage
    {
        private readonly TesseractEngine _engine;

        public ReadImage(TesseractEngine engine)
        {
            
            _engine = engine;
        }

        public void ReadImageFromUser(string path, string imagePath)
        {
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "TextFiles"));
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            try
            {
                using (var image = Pix.LoadFromFile(imagePath))
                {
                    using (var page = _engine.Process(image))
                    {
                        var text = page.GetText();
                        using (var sw = new StreamWriter(Path.Combine(path, "text.txt")))
                        {
                            foreach (var line in text)
                            {
                                sw.Write(line);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }
        public void FilterText()
        {
            string? line;
            string? placeOfPurchase = "";
            string textFilepath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "TextFiles"));
            string jsonFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "JsonFiles"));
            DateTime uploadDate = DateTime.Now;
            var matchesDate = new Regex(@"(\d+\/\d+\/\d+)");
            var matchesValue = new Regex(@"(\d+\,\d{2})");
            var matchesTime = new Regex(@"(\d+\:\d+)");
            var resultList = new Dictionary<string, string>();

            if (Directory.Exists(jsonFilePath))
            {
                Directory.Delete(jsonFilePath, true);
            }
            Directory.CreateDirectory(jsonFilePath);

            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(textFilepath, "text.txt")))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var date = matchesDate.Match(line);
                        var value = matchesValue.Match(line);
                        var time = matchesTime.Match(line);
                        if (date.Success)
                        {
                            string dateFound = date.Groups[0].Value;
                            resultList.Add("PurchaseDate", dateFound);
                        }
                        if (value.Success)
                        {
                            string valueFound = value.Groups[0].Value;
                            resultList.Add("Price", valueFound);
                        }
                        if (time.Success)
                        {
                            string timeFound = time.Groups[0].Value;
                            resultList.Add("PurchaseTime", timeFound);
                            resultList.Add("UploadDate", uploadDate.ToString());
                            resultList.Add("PlaceOfPurchase", placeOfPurchase);
                        }
                        var jsonList = JsonConvert.SerializeObject(resultList, Formatting.Indented);
                        File.WriteAllText(Path.Combine(jsonFilePath, "jsonFiles.json"), jsonList);

                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }
    }
}
