namespace MicrobloggingApp.Infrastructure
{
    public interface IAzureStorageService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<byte[]> GetImageAsync(string imageUrl);
    }

}
