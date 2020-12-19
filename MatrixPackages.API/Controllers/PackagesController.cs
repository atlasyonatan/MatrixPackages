using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixPackages.Models;
using Microsoft.Extensions.Logging;

namespace MatrixPackages.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackagesController : Controller
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        public PackagesController(DbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<PackagesController>();
        }

        [HttpPost]
        public async Task<IActionResult> Input([FromBody] Package package)
        {
            if (await _context.Package.AnyAsync(p => p.TrackingNumber == package.TrackingNumber))
                return new ObjectResult("A package with the same TrackingNumber already exists") { StatusCode = 409 };
            _context.Package.Add(package);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        [HttpGet("{trackingNumber}")]
        public async Task<IActionResult> Fetch(string trackingNumber)
        {
            var query = _context.Package.Include(p => p.ParcelContent)
                .Where(p => p.TrackingNumber == trackingNumber);

            if (await query.AnyAsync())
                return new OkObjectResult(await query.FirstAsync());
            else
                return new ObjectResult($"TrackingNumber: \"{trackingNumber}\" not found") { StatusCode = 404 };
        }

        [HttpPatch]
        // Returns the emount of documents affected
        public async Task<IActionResult> UpdateStatus([FromQuery] string[] trackingNumbers, [FromBody] int StatusCode)
        {
            var packages = await _context.Package
                .Where(package => trackingNumbers.Contains(package.TrackingNumber))
                .ToListAsync();
            foreach (var package in packages) package.StatusCode = StatusCode;
            var changes = await _context.SaveChangesAsync();
            _logger.LogInformation($"{changes} of {trackingNumbers.Length} documents affected");
            return new OkObjectResult(changes);
        }

        [HttpDelete]
        // Returns the emount of documents affected
        public async Task<IActionResult> DeleteRange([FromQuery] string[] trackingNumbers)
        {
            var packages = await _context.Package
                .Include(p => p.ParcelContent)
                .Where(package => trackingNumbers.Contains(package.TrackingNumber))
                .ToListAsync();
            _context.Package.RemoveRange(packages);
            var changes = await _context.SaveChangesAsync();
            _logger.LogInformation($"{changes} of {trackingNumbers.Length} documents affected");
            return new OkObjectResult(changes);
        }
    }
}
