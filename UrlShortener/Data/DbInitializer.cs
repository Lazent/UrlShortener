using UrlShortener.Models.Entities;

namespace UrlShortener.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var users = new User[]
                {
            new User
            {
                Login = "admin",
                PasswordHash = "admin123",
                IsAdmin = true,
                CreatedAt = DateTime. UtcNow
            },
            new User
            {
                Login = "user",
                PasswordHash = "user123",
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.ShortUrls.Any())
            {
                var urls = new ShortUrl[]
                {
            new ShortUrl
            {
                OriginalUrl = "https://github.com",
                ShortCode = "temp",
                CreatedAt = DateTime.UtcNow,
                CreatedById = 1
            }
                };

                context.ShortUrls.AddRange(urls);
                context.SaveChanges();

                foreach (var url in urls)
                {
                    url.ShortCode = EncodeBase62(url.Id);
                }
                context.SaveChanges();
            }

            if (!context.AboutContents.Any())
            {
                var aboutContent = new AboutContent
                {
                    Content = @"<h2>URL Shortener Algorithm - Base62 Encoding</h2>

<h3>Overview</h3>
<p>This URL shortener uses <strong>Base62 encoding</strong> to convert numeric IDs into short alphanumeric codes. </p>

<h3>1.How It Works</h3>

<h4>1. Sequential ID Generation</h4>
<p>When a new URL is added:</p>
<ul>
    <li>The database generates a sequential ID (1, 2, 3, ... )</li>
    <li>This ID is unique and auto-incremented</li>
</ul>

<h4>2.Base62 Encoding</h4>
<p>The ID is converted to Base62 using this alphabet: </p>
<pre class='bg-light p-2 rounded'><code>0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ</code></pre>

<h4>3.Examples</h4>
<table class='table table-striped'>
    <thead class='table-dark'>
        <tr><th>ID</th><th>Short Code</th><th>Full URL</th></tr>
    </thead>
    <tbody>
        <tr><td>1</td><td><code>1</code></td><td>localhost/1</td></tr>
        <tr><td>10</td><td><code>a</code></td><td>localhost/a</td></tr>
        <tr><td>62</td><td><code>10</code></td><td>localhost/10</td></tr>
        <tr><td>125</td><td><code>21</code></td><td>localhost/21</td></tr>
        <tr><td>1,000,000</td><td><code>4c92</code></td><td>localhost/4c92</td></tr>
    </tbody>
</table>

<h4>4.Benefits</h4>
<ul>
    <li><strong>Short codes: </strong> 6 characters = 56 billion URLs</li>
    <li><strong>Unique:</strong> Each ID maps to exactly one code</li>
    <li><strong>Reversible: </strong> Code can be decoded back to ID</li>
    <li><strong>No collisions:</strong> Sequential IDs guarantee uniqueness</li>
</ul>

<h4>5.Algorithm Complexity</h4>
<ul>
    <li><strong>Encoding:</strong> O(log n)</li>
    <li><strong>Decoding:</strong> O(n) where n = code length</li>
</ul>

<h4>6.Implementation</h4>
<p>The algorithm is implemented in <code>UrlShortenerService.cs</code> with <strong>36 comprehensive unit tests</strong> covering edge cases. </p>",
                    LastModified = DateTime.UtcNow,
                    LastModifiedById = 1
                };

                context.AboutContents.Add(aboutContent);
                context.SaveChanges();
            }
        }

        private static string EncodeBase62(int id)
        {
            const string alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (id == 0) return alphabet[0].ToString();

            var result = string.Empty;
            while (id > 0)
            {
                result = alphabet[id % 62] + result;
                id /= 62;
            }
            return result;
        }
    }
}