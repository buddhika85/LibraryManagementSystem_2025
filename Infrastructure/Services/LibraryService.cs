using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IGenericRepository<Author> authorRepo;

        public LibraryService(IGenericRepository<Author> authorRepo)
        {
            this.authorRepo = authorRepo;
        }

        public async Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync()
        {
            var authors = new List<AuthorDto>();
            foreach (var item in await authorRepo.ListAllAsync())
            {
                authors.Add(new AuthorDto { Id = item.Id, Name = item.Name, Biography = item.Biography, Country = item.Country, DateOfBirth = item.DateOfBirth });
            }
            return authors;
        }
    }
}
