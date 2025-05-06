using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private IDbContextTransaction? transaction;


        public UnitOfWork(AppDbContext context,                
                IBooksRepository booksRepository,
                IAuthorRepository authorRepository,
                IUserRepository userRepository,
                IBorrowalsRepository borrowalsRepository,
                IGenericRepository<Address> addressRepository)
        {
            this.context = context;
            BookRepository = booksRepository;
            AuthorRepository = authorRepository;
            UserRepository = userRepository;
            AddressRepository = addressRepository;
            BorrowalsRepository = borrowalsRepository;
        }

        public IBooksRepository BookRepository { get; }

        public IAuthorRepository AuthorRepository { get; }
        public IUserRepository UserRepository { get; }
        public IBorrowalsRepository BorrowalsRepository { get; }

        public IGenericRepository<Address> AddressRepository { get; }



        /// <summary>
        /// Saves all changes with in a new transaction 
        /// </summary>
        /// <returns>Returns effected record count</returns>
        public async Task<bool> SaveAllAsTransactionAsync()
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


        #region Manual Transaction Management

        // Explicit Transaction Management
        public async Task BeginTransactionAsync()
        {
            if (transaction == null)
            {
                transaction = await context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        /// <summary>
        /// Row save - transaction needs to be managed by programmer
        /// </summary>
        /// <returns>Returns effected record count</returns>
        public async Task<bool> SaveAllAsync()
        {
            try
            {
                var count = await context.SaveChangesAsync();
                return count > 0;
            }
            catch
            {
                throw;
            }
        }

        #endregion  Manual Transaction Management

        public void Dispose() => context.Dispose();
    }
}
