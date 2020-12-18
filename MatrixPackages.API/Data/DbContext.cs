using Microsoft.EntityFrameworkCore;
using MatrixPackages.Models;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Package { get; set; }
}
