using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public ICollection<ShortUrl> CreatedUrls { get; set; } = new List<ShortUrl>();

    }
}
