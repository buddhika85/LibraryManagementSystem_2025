using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class BorrowalsRepository : GenericRepository<Borrowals>, IBorrowalsRepository
    {
        public BorrowalsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
