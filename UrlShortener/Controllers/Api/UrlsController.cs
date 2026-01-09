using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.DTOs;
using UrlShortener.Models.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlShortenerService _shortenerService;
        private readonly ILogger<UrlsController> _logger;

        public UrlsController(
            ApplicationDbContext context,
            IUrlShortenerService shortenerService,
            ILogger<UrlsController> logger)
        {
            _context = context;
            _shortenerService = shortenerService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShortUrlResponse>>> GetAllUrls()
        {
            var urls = await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            var response = urls.Select(url => new ShortUrlResponse
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                CreatedByLogin = url.CreatedBy.Login
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShortUrlResponse>> GetUrl(int id)
        {
            var url = await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (url == null)
            {
                return NotFound(new ErrorResponse($"URL with ID {id} not found"));
            }

            var response = new ShortUrlResponse
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                CreatedByLogin = url.CreatedBy.Login
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShortUrlResponse>> CreateShortUrl([FromBody] CreateUrlRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new ErrorResponse("Validation failed", errors));
            }

            if (!_shortenerService.IsValidUrl(request.OriginalUrl))
            {
                return BadRequest(new ErrorResponse("Invalid URL format.  URL must start with http:// or https://"));
            }

            var existingUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(u => u.OriginalUrl == request.OriginalUrl);

            if (existingUrl != null)
            {
                await _context.Entry(existingUrl).Reference(u => u.CreatedBy).LoadAsync();

                var existingResponse = new ShortUrlResponse
                {
                    Id = existingUrl.Id,
                    OriginalUrl = existingUrl.OriginalUrl,
                    ShortCode = existingUrl.ShortCode,
                    ShortUrl = $"{Request.Scheme}://{Request.Host}/{existingUrl.ShortCode}",
                    CreatedAt = existingUrl.CreatedAt,
                    CreatedByLogin = existingUrl.CreatedBy.Login
                };

                return Ok(existingResponse);
            }

            var url = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = string.Empty,
                CreatedAt = DateTime.UtcNow,
                CreatedById = 1 // TODO: Заменить на реального пользователя после реализации аутентификации
            };

            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            url.ShortCode = _shortenerService.Encode(url.Id);
            await _context.SaveChangesAsync();

            await _context.Entry(url).Reference(u => u.CreatedBy).LoadAsync();

            var response = new ShortUrlResponse
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                CreatedByLogin = url.CreatedBy.Login
            };

            _logger.LogInformation("Created short URL: {ShortCode} for {OriginalUrl}", url.ShortCode, url.OriginalUrl);

            return CreatedAtAction(nameof(GetUrl), new { id = url.Id }, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var url = await _context.ShortUrls
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (url == null)
            {
                return NotFound(new ErrorResponse($"URL with ID {id} not found"));
            }

            var currentUserId = 1;
            var isAdmin = false;

            if (!isAdmin && url.CreatedById != currentUserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ErrorResponse("You don't have permission to delete this URL"));
            }

            _context.ShortUrls.Remove(url);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted short URL: {ShortCode}", url.ShortCode);

            return NoContent();
        }
    }
}