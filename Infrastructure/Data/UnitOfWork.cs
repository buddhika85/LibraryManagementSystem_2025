using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context,
                // IGenericRepository<Author> authorRepository,
                IBooksRepository booksRepository,
                IAuthorRepository authorRepository,
                IUserRepository userRepository)
        {
            this.context = context;
            BookRepository = booksRepository;
            AuthorRepository = authorRepository;
            UserRepository = userRepository;
        }

        public IBooksRepository BookRepository { get; }

        public IAuthorRepository AuthorRepository { get; }
        public IUserRepository UserRepository { get; }

        //public IGenericRepository<Author> AuthorRepository { get; }



        public async Task<bool> SaveAllAsync()
        {
            var count = await context.SaveChangesAsync();
            return count > 0;
        }

        public void Dispose() => context.Dispose();
    }
}
