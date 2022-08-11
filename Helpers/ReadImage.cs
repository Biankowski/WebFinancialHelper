using Tesseract;

namespace WebFinancialHelper.Helpers
{
    public class ReadImage
    {
        public string textFileLogPath;
        public string imagePath;
        private readonly TesseractEngine _engine;

        public ReadImage(string textFileLogPath, string imagePath, TesseractEngine engine)
        {
            this.textFileLogPath = textFileLogPath;
            this.imagePath = imagePath;
            _engine = engine;
        }

        public void ReadImageFromUser()
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
                        using (var sw = new StreamWriter(@"C:\Users\Biankovsky\Desktop\Projetos C#\OCRFinancialHelper\WebFinancialHelper\test.txt"))
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
    }
}
