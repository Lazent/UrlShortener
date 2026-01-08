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
            if (string.IsNullOrEmpty(shortCode))
                throw new ArgumentException("Short code cannot be null or empty", nameof(shortCode));

            var result = 0;

            foreach (var character in shortCode)
            {
                var position = Alphabet.IndexOf(character);

                if (position == -1)
                    throw new ArgumentException($"Invalid character '{character}' in short code", nameof(shortCode));

                result = result * Base + position;
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
