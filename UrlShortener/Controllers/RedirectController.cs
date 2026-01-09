using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.DTOs;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlShortenerService _shortenerService;
        private readonly ILogger<RedirectController> _logger;

        public RedirectController(
            ApplicationDbContext context,
            IUrlShortenerService shortenerService,
            ILogger<RedirectController> logger)
        {
            _context = context;
            _shortenerService = shortenerService;
            _logger = logger;
        }

        [HttpGet("/{shortCode}")]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return NotFound(new ErrorResponse("Short code is required"));
            }

            try
            {
                var id = _shortenerService.Decode(shortCode);

                var url = await _context.ShortUrls.FindAsync(id);

                if (url == null)
                {
                    _logger.LogWarning("Short code {ShortCode} not found", shortCode);
                    return NotFound(new ErrorResponse($"Short URL '{shortCode}' not found"));
                }

                _logger.LogInformation("Redirecting {ShortCode} to {OriginalUrl}", shortCode, url.OriginalUrl);

                return Redirect(url.OriginalUrl);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid short code: {ShortCode}. Error: {Error}", shortCode, ex.Message);
                return NotFound(new ErrorResponse($"Invalid short code: {shortCode}"));
            }
        }
    }
}