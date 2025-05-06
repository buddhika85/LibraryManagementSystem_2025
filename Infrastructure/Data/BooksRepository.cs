using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BooksRepository : GenericRepository<Book>, IBooksRepository
    {
        public BooksRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await context.Books.Include(x => x.Authors).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Book>> GetBooksIncludingAuthorsAsync()
        {
            return await context.Books.Include(x => x.Authors).ToListAsync();
        }

        public async Task<IList<Book>> GetBooksIncludingAuthorsAsync(BookFilterDto bookFilterDto, bool isAvailable)
        {
            return await context.Books.Include(x => x.Authors)
                            .Where(x => x.IsAvailable &&
                                (
                                    (!bookFilterDto.AuthorIds.Any() && !bookFilterDto.BookGenres.Any()) || // No filtering
                                    (bookFilterDto.AuthorIds.Any() && x.Authors.Any(a => bookFilterDto.AuthorIds.Contains(a.Id))) ||
                                    (bookFilterDto.BookGenres.Any() && bookFilterDto.BookGenres.Contains((int)x.Genre))
                                )
                            )
                            .ToListAsync();
        }
    }
}
