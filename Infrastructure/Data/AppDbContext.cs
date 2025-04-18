using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;


// dotnet-ef migrations add InitialCreate -s API -p Infrastructure 
// dotnet-ef database update -s API -p Infrastructure 

// dotnet ef migrations remove --startup-project API --project Infrastructure --context AppDbContext
// dotnet-ef migrations remove -s API -p Infrastructure

// apply migration =>                           dotnet-ef database update -s API -p Infrastructure
// revert last applied migration from DB =>     dotnet ef database update 0 -s API -p Infrastructure

// DROP DB - dotnet ef database drop -p Infrastructure -s API
namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // SQL server connectin string comes in using options parameter
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        // Tables
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }


        // On model creation (before creating DB table) do below
        // you can specify exact column name
        // exact precision of data type
        // null ness ...etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfigurations).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookConfigurations).Assembly);
        }
    }
}
