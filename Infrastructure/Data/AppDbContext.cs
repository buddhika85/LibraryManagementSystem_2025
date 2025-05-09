using Core.DTOs;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // SQL server connectin string comes in using options parameter
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        // DB Tables
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Borrowals> Borrowals { get; set; }
        public DbSet<BorrowalReturn> BorrowalReturns { get; set; }


        // Stored Procedure result sets  
        public DbSet<BorrowalSummaryDto> BorrowalSummaryDtos { get; set; }           // 'GetBorrowalsSummarySP' SP



        // On model creation (before creating DB table) do below
        // you can specify exact column name
        // exact precision of data type
        // null ness ...etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);

            ConfigureStoredProcedureResultSets(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfigurations).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookConfigurations).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BorrowalsConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BorrowalReturnConfigurations).Assembly);
        }

        private void ConfigureStoredProcedureResultSets(ModelBuilder modelBuilder)
        {
            // HasNoKey() - Do not track or store this DTO as an actual table
            // EF will skip this as it does not have any key
            modelBuilder.Entity<BorrowalSummaryDto>().HasNoKey();               // No primary key - Used with 'GetBorrowalsSummarySP' Stored Procedure
        }
    }
}
