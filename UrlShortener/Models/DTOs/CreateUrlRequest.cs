using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.DTOs
{
    public class CreateUrlRequest
    {
        [Required(ErrorMessage = "URL is required")]
        [MaxLength(2000, ErrorMessage = "URL cannot exceed 2000 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}
