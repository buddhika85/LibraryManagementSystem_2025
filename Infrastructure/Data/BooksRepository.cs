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

        public async Task<IReadOnlyList<Book>> GetBooksIncludingAuthorsAsync()
        {
            return await context.Books.Include(x => x.Authors).ToListAsync();
        }
    }
}
