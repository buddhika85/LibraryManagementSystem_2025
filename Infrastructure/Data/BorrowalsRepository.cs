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

        public async Task<Borrowals?> GetAllBorrowalWithNavPropsAsync(int borrowalId)
        {
            return await context.Borrowals.Where(x => x.Id == borrowalId).Include(x => x.Book).Include(x => x.AppUser).SingleOrDefaultAsync();
        }
    }
}
