using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        public Task<IList<Author>> GetAuthorsByIdsAsync(List<int> ids);
    }
}
