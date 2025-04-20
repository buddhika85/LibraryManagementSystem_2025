using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        //IGenericRepository<Author> AuthorRepository { get; }

        IBooksRepository BookRepository { get; }       
        IAuthorRepository AuthorRepository { get; }


        public Task<bool> SaveAllAsync();
    }
}
