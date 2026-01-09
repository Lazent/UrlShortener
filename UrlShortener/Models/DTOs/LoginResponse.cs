namespace UrlShortener.Models.DTOs
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
