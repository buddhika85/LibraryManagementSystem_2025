using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBooksRepository BookRepository { get; }       
        IAuthorRepository AuthorRepository { get; }
        IUserRepository UserRepository { get; }
        IGenericRepository<Address> AddressRepository { get; }

        IBorrowalsRepository BorrowalsRepository { get;  }


        Task BeginTransactionAsync();       
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveAllAsync();
        Task<bool> SaveAllAsTransactionAsync();

    }
}
