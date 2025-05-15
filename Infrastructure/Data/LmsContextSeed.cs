using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
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
            if (!await context.Authors.AnyAsync())
                await SeedAsyncHelper(context.Authors, authorsSeedJson);
           
            if (await context.Authors.AnyAsync() && !await context.Books.AnyAsync()) 
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
        private static UserManager<AppUser> userManager = null!;
        private static readonly (AppUser, UserRoles)[] usersToSeed =
        [
            (new AppUser
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@lms.com",
                PhoneNumber = "1234567891",
                UserName = "admin@lms.com",
                Address = new Address { Line1 = "1", Line2 = "First Lane", City = "Sunny City", State = "NSW", Postcode = "2000", Country = "Australia" }
            }, UserRoles.Admin),
            (new AppUser
            {
                FirstName = "Staff",
                LastName = "User 1",
                Email = "staff1@lms.com",
                PhoneNumber = "1234567891",
                UserName = "staff1@lms.com",
                Address = new Address { Line1 = "2", Line2 = "Second Lane", City = "Rainy City", State = "NSW", Postcode = "2001", Country = "Australia" }
            }, UserRoles.Staff),
            (new AppUser
            {
                FirstName = "Staff",
                LastName = "User 2",
                Email = "staff2lms.com",
                PhoneNumber = "54321098761",
                UserName = "staff2@lms.com",
                Address = new Address { Line1 = "5", Line2 = "Fifth Lane", City = "Snowy City", State = "NSW", Postcode = "2004", Country = "Australia" }
            }, UserRoles.Staff),
            (new AppUser
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@lms.com",
                PhoneNumber = "98765432101",
                UserName = "john@lms.com",
                Address = new Address { Line1 = "3", Line2 = "Third Lane", City = "Summer City", State = "NSW", Postcode = "2002", Country = "Australia" }
            }, UserRoles.Member),
            (new AppUser
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jame@lms.com",
                PhoneNumber = "98765432101",
                UserName = "jane@lms.com",
                Address = new Address { Line1 = "4", Line2 = "Fourth Lane", City = "Winter City", State = "NSW", Postcode = "2003", Country = "Australia" }
            }, UserRoles.Member),
        ];
        private const string defaultPassword = "Pass#Word1";

        public static async Task SeedIdentiyAsync(RoleManager<IdentityRole> roleMngr, UserManager<AppUser> userMngr)
        {
            roleManager = roleMngr;
            userManager = userMngr;
            await SeedRoles();
            await SeedUsers();
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

        private static async Task SeedUsers()
        {
            if (!await userManager.Users.AnyAsync() && await roleManager.Roles.AnyAsync())
            {
                foreach (var user in usersToSeed)
                {
                    var result = await userManager.CreateAsync(user.Item1, defaultPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user.Item1, UserRoles.Admin.ToString());
                    }
                }
            }
        }       
        #endregion
    }
}
