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

        public void ReadImageFromUser(string textFileLogPath, string imagePath)
        {
            try
            {
                using (var image = Pix.LoadFromFile(imagePath))
                {
                    using (var page = _engine.Process(image))
                    {
                        var text = page.GetText();

                        if (Directory.Exists(textFileLogPath))
                        {
                            Directory.Delete(textFileLogPath, true);
                            Directory.CreateDirectory(textFileLogPath);
                        }
                        using (var sw = new StreamWriter(@"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\test.txt"))
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
            DateTime uploadDate = DateTime.UtcNow;
            var matchesDate = new Regex(@"(\d+\/\d+\/\d+)");
            var matchesValue = new Regex(@"(\d+\,\d{2})");
            var matchesTime = new Regex(@"(\d+\:\d+)");
            var resultList = new Dictionary<string, string>();

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\test.txt"))
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
                        File.WriteAllText(@"C:\Users\Biankovsky\Desktop\Projetos C#\WebOCR\WebFinancialHelper\jsonList.json", jsonList);

                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }
    }
}
