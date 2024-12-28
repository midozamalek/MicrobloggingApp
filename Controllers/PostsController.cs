using MicrobloggingApp.Domain.Services;
using MicrobloggingApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicrobloggingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostsController(IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name; // Extract from token
            var post = await _postService.CreatePostAsync(userId, request);
            return CreatedAtAction(nameof(CreatePost), new { id = post.Id }, post);
        }

        [HttpGet("timeline")]
        public async Task<IActionResult> GetTimeline()
        {
            var timeline = await _postService.GetTimelineAsync();
            return Ok(timeline);
        }

        [HttpGet("timeline/image/{imageUrl}")]
        public async Task<IActionResult> GetImage(string imageUrl)
        {
            var imageBytes = await _postService.GetImageForPostAsync(imageUrl);
            return File(imageBytes, "image/jpeg");
        }

    }

}
