using Core.Entities;
namespace Core.Interfaces
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        public Task<IReadOnlyList<Book>> GetBooksIncludingAuthorsAsync();
    }
}
