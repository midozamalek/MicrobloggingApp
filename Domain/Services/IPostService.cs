using MicrobloggingApp.DTO;
using MicrobloggingApp.Models;

namespace MicrobloggingApp.Domain.Services
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(string userId, CreatePostRequest request);
        Task<List<TimeLineDto>> GetTimelineAsync();
        Task<byte[]> GetImageForPostAsync(string imageUrl);

    }
}
