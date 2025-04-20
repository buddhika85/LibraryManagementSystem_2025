using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IList<Author>> GetAuthorsByIdsAsync(List<int> ids)
        {
            var list = new List<Author>();
            foreach (var id in ids)
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    list.Add(entity);
                }
            }
            return list;
        }
    }
}
