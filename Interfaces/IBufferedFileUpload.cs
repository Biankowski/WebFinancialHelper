namespace WebFinancialHelper.Interfaces
{
    public interface IBufferedFileUpload
    {
        Task<bool> UploadFile(IFormFile file);
    }

}
