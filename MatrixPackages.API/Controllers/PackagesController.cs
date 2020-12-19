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
    [Route("[controller]")]// accepts requests with /Packages
    public class PackagesController : Controller
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        public PackagesController(DbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<PackagesController>();
        }

        // Insert new package to db.
        // Method will be used when sending POST to the route /Packages with json body
        // Validation is handled by model binding and validation through asp.net and model field attributes
        [HttpPost]
        public async Task<IActionResult> Input([FromBody] Package package)
        {
            //Ensure no duplicates
            if (await _context.Package.AnyAsync(p => p.TrackingNumber == package.TrackingNumber))
                return new ObjectResult("A package with the same TrackingNumber already exists") { StatusCode = 409 };
            _context.Package.Add(package);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        // Select a package from db via its trackingNumber.
        // Method will be used when sending GET to the route /Packages/{trackingNumber}
        // If no such package is found returns 404 with appropriate message.
        // otherwise: returns the selected package as Json.
        [HttpGet("{trackingNumber}")]
        public async Task<IActionResult> Fetch(string trackingNumber)
        {
            var query = _context.Package.Include(p => p.ParcelContent)
                .Where(p => p.TrackingNumber == trackingNumber);

            if (await query.AnyAsync())
                return new OkObjectResult(await query.FirstAsync());
            else
                return new NotFoundObjectResult($"TrackingNumber: \"{trackingNumber}\" not found");
        }

        [HttpPatch]
        // Update the statusCode of packages in the db via their trackingNumber.
        // Method will be used when sending PATCH to the route /Packages
        // with a query of the form: ?trackingNumbers=trackingNumber1&trackingNumbers=trackingNumber2
        // and with a body containing the new statusCode as int.
        // Only updates packages whose trackingNumber exists in the given trackingNumbers.
        // Returns the amount of documents affected in the db.
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
        // Delete packages in the db via their trackingNumber.
        // Method will be used when sending DELETE to the route /Packages
        // with a query of the form: ?trackingNumbers=trackingNumber1&trackingNumbers=trackingNumber2
        // Only deletes packages whose trackingNumber exists in the given trackingNumbers.
        // Returns the amount of documents affected in the db.
        public async Task<IActionResult> Delete([FromQuery] string[] trackingNumbers)
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
