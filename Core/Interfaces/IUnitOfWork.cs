using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBooksRepository BookRepository { get; }       
        IAuthorRepository AuthorRepository { get; }
        IUserRepository UserRepository { get; }
        IGenericRepository<Address> AddressRepository { get; }

        Task<bool> SaveAllAsync();
    }
}
