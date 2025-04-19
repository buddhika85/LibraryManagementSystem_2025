using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;


namespace Infrastructure.Data
{
    public class LmsContextSeed
    {
        private const string authorsSeedJson = "../Infrastructure/Data/SeedData/authors.json";
        private const string booksSeedJson = "../Infrastructure/Data/SeedData/books.json";

        private static AppDbContext appDbContext = null!;

        public static async Task SeedAsync(AppDbContext context)
        {
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
    }
}
