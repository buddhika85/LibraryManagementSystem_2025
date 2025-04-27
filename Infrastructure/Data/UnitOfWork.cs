using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context,                
                IBooksRepository booksRepository,
                IAuthorRepository authorRepository,
                IUserRepository userRepository,
                IGenericRepository<Address> addressRepository)
        {
            this.context = context;
            BookRepository = booksRepository;
            AuthorRepository = authorRepository;
            UserRepository = userRepository;
            AddressRepository = addressRepository;
        }

        public IBooksRepository BookRepository { get; }

        public IAuthorRepository AuthorRepository { get; }
        public IUserRepository UserRepository { get; }

        public IGenericRepository<Address> AddressRepository { get; }

        public async Task<bool> SaveAllAsync()
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var count = await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return count > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose() => context.Dispose();
    }
}
