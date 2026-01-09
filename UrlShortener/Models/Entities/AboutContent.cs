using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities
{
    public class AboutContent
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public int LastModifiedById { get; set; }
        public User LastModifiedBy { get; set; } = null!;
    }
}
