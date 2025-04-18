using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // SQL server connectin string comes in using options parameter
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
