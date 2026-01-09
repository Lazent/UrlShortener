using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.Entities;

namespace UrlShortener.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AboutController> _logger;

        public AboutController(
            ApplicationDbContext context,
            ILogger<AboutController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var about = await _context.AboutContents
                .Include(a => a.LastModifiedBy)
                .FirstOrDefaultAsync();

            if (about == null)
            {
                _logger.LogWarning("AboutContent not found in database, returning default content");

                about = new AboutContent
                {
                    Content = "<h2>About Content Not Found</h2><p>Please contact administrator.</p>",
                    LastModified = DateTime.UtcNow,
                    LastModifiedById = 1
                };
            }

            ViewBag.IsAdmin = true;

            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Content cannot be empty";
                return RedirectToAction(nameof(Index));
            }

            var about = await _context.AboutContents.FirstOrDefaultAsync();

            if (about == null)
            {
                
                about = new AboutContent
                {
                    Content = content,
                    LastModified = DateTime.UtcNow,
                    LastModifiedById = 1
                };
                _context.AboutContents.Add(about);
            }
            else
            { 
                about.Content = content;
                about.LastModified = DateTime.UtcNow;
                about.LastModifiedById = 1;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("About content updated at {Time}", DateTime.UtcNow);

            TempData["SuccessMessage"] = "About content updated successfully! ";

            return RedirectToAction(nameof(Index));
        }
    }
}
