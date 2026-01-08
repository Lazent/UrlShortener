using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities
{
    public class ShortUrl
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string OriginalUrl { get; set; }
        [Required]
        [MaxLength(10)]
        public string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
    }
}
