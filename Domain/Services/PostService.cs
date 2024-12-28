using MicrobloggingApp.DTO;
using MicrobloggingApp.Infrastructure;
using MicrobloggingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicrobloggingApp.Domain.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAzureStorageService _storageService;

        public PostService(ApplicationDbContext context, IAzureStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<Post> CreatePostAsync(string userId, CreatePostRequest request)
        {
            // Generate random geographic coordinates
            var random = new Random();
            var latitude = random.NextDouble() * 180 - 90;
            var longitude = random.NextDouble() * 360 - 180;

            // Upload image to Azure Blob Storage (if provided)
            string imageUrl = null;
            if (request.Image != null)
            {
                //imageUrl = await _storageService.UploadAsync(request.Image);
            }

            // Create the post
            var post = new Post
            {
                UserId = userId,
                Text = request.Text,
                OriginalImageUrl = "",
                Latitude = latitude,
                Longitude = longitude,
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<List<TimeLineDto>> GetTimelineAsync()
        {
            var posts = await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();

            var result = posts.Select(post => new TimeLineDto
            {
                Id = post.Id,
                Text = post.Text,
                ImageUrl = DetermineImageSize(post),
                Latitude = post.Latitude,
                Longitude = post.Longitude,
                CreatedAt = post.CreatedAt
            }).ToList();

            return result;
        }

        public async Task<byte[]> GetImageForPostAsync(string imageUrl)
        {
            return await _storageService.GetImageAsync(imageUrl);
        }

        private string DetermineImageSize(Post post)
        {
            // Example logic for screen size matching (can be adjusted as needed)
            var screenWidth = GetScreenWidth(); // Get from request headers or default to a size.

            if (screenWidth <= 480)
                return GetResizedImageUrl(post.ResizedImageUrlSmall); // Resize to 600x400 for small screens

            if (screenWidth <= 1024)
                return GetResizedImageUrl(post.ResizedImageUrlMedium); // Resize to 600x400 for medium screens

            return GetResizedImageUrl(post.ResizedImageUrlLarge) ?? post.OriginalImageUrl; // Default large image
        }


        private int GetScreenWidth()
        {
            // Simulate fetching screen width (replace with actual implementation)
            return 1024; // Default example
        }

        private string GetResizedImageUrl(string imageUrl)
        {
            var uriBuilder = new UriBuilder(imageUrl)
            {
                Query = $"resize=600,400" // Add query parameters for image resizing
            };
            return uriBuilder.ToString();
        }


    }

}
