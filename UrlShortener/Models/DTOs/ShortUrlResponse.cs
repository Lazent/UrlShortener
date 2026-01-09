namespace UrlShortener.Models.DTOs
{
    public class ShortUrlResponse
    {
        public int Id { get; set; }

        public string OriginalUrl { get; set; } = string.Empty;

        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string CreatedByLogin { get; set; } = string.Empty;
    }
}
