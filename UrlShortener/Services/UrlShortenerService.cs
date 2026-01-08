namespace UrlShortener.Services
{
    public class UrlShortenerService: IUrlShortenerService
    {
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int Base = 62;

        public string Encode(int id)
        {
            if (id == 0) return Alphabet[0].ToString();

            var result = string.Empty;

            while(id > 0)
            {
                var remainder = id % Base;
                result = Alphabet[remainder] + result;
                id /= Base;
            }
            return result;
        }

        public int Decode(string shortCode)
        {
            var result = 0;

            foreach (var character in shortCode)
            {
                result = result * Base + Alphabet.IndexOf(character);
            }
            return result;
        }

        public bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
