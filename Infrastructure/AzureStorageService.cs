using Azure.Storage.Blobs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MicrobloggingApp.Infrastructure
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;

        public AzureStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var connectionString = _configuration["AzureStorage:BlobConnectionString"];
            var containerName = _configuration["AzureStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            var imageUrl = await ResizeAndUploadImageAsync(file, containerClient);

            return imageUrl;
        }

        private async Task<string> ResizeAndUploadImageAsync(IFormFile file, BlobContainerClient containerClient)
        {
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            using var image = await Image.LoadAsync(file.OpenReadStream());

            image.Mutate(x => x.Resize(1200, 800));

            await using var stream = new MemoryStream();
            await image.SaveAsync(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
            stream.Position = 0;

            await blobClient.UploadAsync(stream);

            return blobClient.Uri.ToString();
        }

        public async Task<byte[]> GetImageAsync(string imageUrl)
        {
            var connectionString = _configuration["AzureStorage:BlobConnectionString"];
            var containerName = _configuration["AzureStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(new Uri(imageUrl).AbsolutePath);

            var response = await blobClient.DownloadAsync();
            await using var stream = response.Value.Content;
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

}
