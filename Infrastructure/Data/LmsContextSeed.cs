using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Infrastructure.Data
{
    public class LmsContextSeed
    {
        private const string authorsSeedJson = "../Infrastructure/Data/SeedData/authors.json";
        private const string booksSeedJson = "../Infrastructure/Data/SeedData/books.json";

        public static async Task SeedAsync(AppDbContext context)
        {
            await SeedAsyncHelper(context.Authors, authorsSeedJson, context);
            await SeedAsyncHelper(context.Books, booksSeedJson, context);
        }


        private static async Task SeedAsyncHelper<T>(DbSet<T> dbSet, string filePath, AppDbContext context) where T : BaseEntity
        {
            if (await dbSet.AnyAsync())
                return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var items = JsonSerializer.Deserialize<List<T>>(jsonData);
            if (items == null)
                return;

            dbSet.AddRange(items);
            await context.SaveChangesAsync();
        }

    }
}
