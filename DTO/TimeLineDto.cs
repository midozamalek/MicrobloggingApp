namespace MicrobloggingApp.DTO
{
    public class TimeLineDto
    {
        public int Id { get; set; }
        public string Text { get; set; } 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
