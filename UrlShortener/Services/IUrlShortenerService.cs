namespace UrlShortener.Services
{
    public interface IUrlShortenerService
    {
        string Encode(int id);
        int Decode(string shortCode);
        bool IsValidUrl(string url);
    }
}
