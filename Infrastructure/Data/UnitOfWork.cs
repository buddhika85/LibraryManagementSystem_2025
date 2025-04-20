using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context,
                // IGenericRepository<Author> authorRepository,
                IBooksRepository booksRepository,
                IAuthorRepository authorRepository)
        {
            this.context = context;
            BookRepository = booksRepository;
            AuthorRepository = authorRepository;
        }

        public IBooksRepository BookRepository { get; }

        public IAuthorRepository AuthorRepository { get; }

        //public IGenericRepository<Author> AuthorRepository { get; }



        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Dispose() => context.Dispose();
    }
}
