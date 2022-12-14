
namespace WebFinancialHelper.Services
{
    public class BufferedUploadLocalService
    {
        // Method used to Upload the recived photo to the local directory
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            try
            {
                if (file == null)
                {
                    return false;
                }
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(path, "photo.jpeg"), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
    }
}
