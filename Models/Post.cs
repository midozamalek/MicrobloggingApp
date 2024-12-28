namespace MicrobloggingApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public string OriginalImageUrl { get; set; }
        public string? ResizedImageUrlSmall { get; set; }
        public string? ResizedImageUrlMedium { get; set; }
        public string? ResizedImageUrlLarge { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
