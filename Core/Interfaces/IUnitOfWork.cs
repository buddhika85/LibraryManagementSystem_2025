using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IBooksRepository BookRepository { get; }
        IGenericRepository<Author> AuthorRepository { get; }


        public Task<bool> SaveAllAsync();
    }
}
