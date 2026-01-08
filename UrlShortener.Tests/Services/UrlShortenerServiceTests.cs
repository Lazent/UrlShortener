using UrlShortener.Services;
using Xunit;
using FluentAssertions;

namespace UrlShortener.Tests.Services
{
    public class UrlShortenerServiceTests
    {
        private readonly UrlShortenerService _service;

        public UrlShortenerServiceTests()
        {
            _service = new UrlShortenerService();
        }

        #region Encode Tests

        [Fact]
        public void Encode_WithZero_ReturnsZero()
        {
            var id = 0;

            var result = _service.Encode(id);

            result.Should().Be("0");
        }

        [Fact]
        public void Encode_WithOne_ReturnsOne()
        {
            var id = 1;

            var result = _service.Encode(id);

            result.Should().Be("1");
        }

        [Theory]
        [InlineData(10, "a")]
        [InlineData(35, "z")]
        [InlineData(36, "A")]
        [InlineData(61, "Z")]
        [InlineData(62, "10")]
        [InlineData(125, "21")]
        [InlineData(1000, "g8")]
        public void Encode_WithVariousIds_ReturnsCorrectShortCode(int id, string expected)
        {
            var result = _service.Encode(id);

            result.Should().Be(expected);
        }

        #endregion

        #region Decode Tests

        [Fact]
        public void Decode_WithZero_ReturnsZero()
        {
            var shortCode = "0";

            var result = _service.Decode(shortCode);

            result.Should().Be(0);
        }

        [Fact]
        public void Decode_WithOne_ReturnsOne()
        {
            var shortCode = "1";

            var result = _service.Decode(shortCode);

            result.Should().Be(1);
        }

        [Theory]
        [InlineData("a", 10)]
        [InlineData("z", 35)]
        [InlineData("A", 36)]
        [InlineData("Z", 61)]
        [InlineData("10", 62)]
        [InlineData("21", 125)]
        [InlineData("g8", 1000)]
        public void Decode_WithVariousShortCodes_ReturnsCorrectId(string shortCode, int expected)
        {
            var result = _service.Decode(shortCode);

            result.Should().Be(expected);
        }

        [Fact]
        public void Decode_WithInvalidCharacter_ThrowsArgumentException()
        {
            var invalidShortCode = "abc@123";

            Action act = () => _service.Decode(invalidShortCode);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Invalid character*");
        }

        [Fact]
        public void Decode_WithNullOrEmpty_ThrowsArgumentException()
        {
            Action actNull = () => _service.Decode(null);
            Action actEmpty = () => _service.Decode("");

            actNull.Should().Throw<ArgumentException>();
            actEmpty.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Encode/Decode Roundtrip Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(999999)]
        public void EncodeAndDecode_ShouldReturnOriginalId(int originalId)
        {
            var encoded = _service.Encode(originalId);
            var decoded = _service.Decode(encoded);

            decoded.Should().Be(originalId);
        }

        #endregion

        #region IsValidUrl Tests

        [Theory]
        [InlineData("https://google.com", true)]
        [InlineData("http://example.com", true)]
        [InlineData("https://github.com/user/repo", true)]
        [InlineData("https://subdomain.example.com/path? query=123", true)]
        public void IsValidUrl_WithValidUrls_ReturnsTrue(string url, bool expected)
        {
            var result = _service.IsValidUrl(url);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("not-a-url")]
        [InlineData("ftp://example.com")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("htp://wrong-scheme.com")]
        public void IsValidUrl_WithInvalidUrls_ReturnsFalse(string url)
        {
            var result = _service.IsValidUrl(url);

            result.Should().BeFalse();
        }

        #endregion
    }
}