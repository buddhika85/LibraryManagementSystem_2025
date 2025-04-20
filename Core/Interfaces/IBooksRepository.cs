using Core.DTOs;
using Core.Entities;
namespace Core.Interfaces
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        public Task<IList<Book>> GetBooksIncludingAuthorsAsync();
        public Task<Book?> GetBookByIdAsync(int id);
    }
}
