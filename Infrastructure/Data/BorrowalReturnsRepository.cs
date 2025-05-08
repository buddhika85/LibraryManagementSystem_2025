using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class BorrowalReturnsRepository : GenericRepository<BorrowalReturn>, IBorrowalReturnsRepository
    {
        public BorrowalReturnsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
