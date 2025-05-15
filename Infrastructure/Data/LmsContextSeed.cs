using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;


namespace Infrastructure.Data
{
    public class LmsContextSeed
    {
        // below only works with development
        //private const string authorsSeedJson = "../Infrastructure/Data/SeedData/authors.json";
        //private const string booksSeedJson = "../Infrastructure/Data/SeedData/books.json";
                

        private static AppDbContext appDbContext = null!;

        #region BusinessDomainSeeding
        public static async Task SeedAsync(AppDbContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var authorsSeedJson = path + @"/Data/SeedData/authors.json";
            var booksSeedJson = path + @"/Data/SeedData/books.json";

            appDbContext = context;
            await SeedAsyncHelper(context.Authors, authorsSeedJson);
           
            if (await context.Authors.AnyAsync()) 
                await SeedAsyncHelper(context.Books, booksSeedJson);            
        }


        private static async Task SeedAsyncHelper<T>(DbSet<T> dbSet, string filePath) where T : BaseEntity
        {
            if (await dbSet.AnyAsync())
                return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var items = JsonSerializer.Deserialize<List<T>>(jsonData);

            if (items == null)
                return;

            if (typeof(T) == typeof(Book))
            {
                var books = items.Cast<Book>().ToList();
                AttachAuthorsFromDB(books);
                appDbContext.Books.AddRange(books); // Add to correct DbSet
            }
            else
            {
                dbSet.AddRange(items);
            }
            await appDbContext.SaveChangesAsync();
        }

        private static void AttachAuthorsFromDB(List<Book> books)
        {
            var existingAuthors = appDbContext.Authors.ToList();
            foreach (var book in books)
            {
                var matchedAuthors = new List<Author>();
                foreach (var author in book.Authors)
                {
                    var existingAuthor = existingAuthors.FirstOrDefault(x => x.Name == author.Name);
                    if (existingAuthor != null)
                        matchedAuthors.Add(existingAuthor);
                }
                book.Authors = matchedAuthors;
            }
        }

        #endregion

        #region IdentitySeeding

        private static RoleManager<IdentityRole> roleManager = null!;

        public static async Task SeedIdentiyAsync(RoleManager<IdentityRole> roleMngr)
        {
            roleManager = roleMngr;
            await SeedRoles();
        }

        private static async Task SeedRoles()
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                var roleNames = new[] { "Admin", "Staff", "Member" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
        #endregion
    }
}
