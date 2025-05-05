using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BorrowalsRepository : GenericRepository<Borrowals>, IBorrowalsRepository
    {
        public BorrowalsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Borrowals>> GetAllBorrowalsWithNavPropsAsync()
        {
            return await context.Borrowals.Include(x => x.Book).Include(x => x.AppUser).ToListAsync();
        }
    }
}
