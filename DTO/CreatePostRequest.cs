using System.ComponentModel.DataAnnotations;

namespace MicrobloggingApp.DTO
{
    public class CreatePostRequest
    {
        [Required]
        [MaxLength(140)]
        public string Text { get; set; }

        public IFormFile Image { get; set; }
    }
}
