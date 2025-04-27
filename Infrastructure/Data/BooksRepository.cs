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
    }
}
