using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatrixPackages.Models;

namespace MatrixPackages.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackagesController : Controller
    {
        private readonly DbContext _context;

        public PackagesController(DbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> Input([FromBody] Package package)
        {
            _context.Package.Add(package);
            //Todo: make sure no duplicates
            var changes = await _context.SaveChangesAsync();
            return new OkObjectResult(changes);
        }

        [HttpGet("{trackingNumber}")]
        public async Task<Package> Fetch(string trackingNumber)
        {
            var package = await _context.Package.Include(p => p.ParcelContent)
                .Where(p => p.TrackingNumber == trackingNumber)
                .FirstAsync();
            return package;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateStatus([FromQuery] string[] trackingNumbers, [FromBody] int StatusCode)
        {
            var packages = await _context.Package
                .Where(package => trackingNumbers.Contains(package.TrackingNumber))
                .ToListAsync();
            foreach (var package in packages) package.StatusCode = StatusCode;
            var changes = await _context.SaveChangesAsync();
            return new OkObjectResult(changes);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([FromQuery] string[] trackingNumbers)
        {
            var packages = await _context.Package
                .Include(p => p.ParcelContent)
                .Where(package => trackingNumbers.Contains(package.TrackingNumber))
                .ToListAsync();
            _context.Package.RemoveRange(packages);
            var changes = await _context.SaveChangesAsync();
            return new OkObjectResult(changes);
        }
    }
}
